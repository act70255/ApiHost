using ApiHost.Host;
using ApiHost.Spider.Helper;
using ApiHost.Spider.Model;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;
using UIAutomationClient;

namespace ApiHost.Spider
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HostMessaging.Instance.DataReceived += (s, e) =>
            {
                Console.WriteLine($"DataReceived {DateTime.Now.ToString("MM/dd HH:mm")} [{e.Item1}/{e.Item2}] {JsonConvert.SerializeObject(e.Item3)}");
            };
            new StartUp();
        }

        class StartUp
        {
            #region Members
            ApiServer apiServer = null;
            Dictionary<string, DateTime> dicSpidernets { get; set; } = new Dictionary<string, DateTime>();

            string Port => ConfigurationSettings.AppSettings["HostPort"];
            string IPAddress => ConfigurationSettings.AppSettings["HostIP"];
            string HostAddress => $"{IPAddress}:{Port}/";
            public List<SpiderRecord> SpiderList { get; }
            int RefreshRate => int.Parse(ConfigurationSettings.AppSettings["RefreshRate"]);
            #endregion

            public StartUp()
            {
                SpiderList = ConfigurationSettings.AppSettings.AllKeys.Where(f => f.StartsWith("Spider_"))
                    .Select(s => new SpiderRecord(ConfigurationSettings.AppSettings[s])).ToList();

                //StartServer();
                SpiderPolling();


                var command = Console.ReadLine();
            }
            #region Method
            void StartServer()
            {
                if (apiServer == null)
                {
                    apiServer = new ApiServer(HostAddress);
                    apiServer.DataReceived += ApiServer_DataReceived;
                }
                apiServer.Start<ApiHost.Host.StartUp>();
            }

            async Task SpiderPolling()
            {
                while (true)
                {
                    var chromeList = Process.GetProcessesByName("chrome").Where(f => !string.IsNullOrEmpty(f.MainWindowTitle)); ;
                    var existSpider = chromeList.Select(s => new { Process = s, Url = ChromeHelper.Instance.GetChromeUrl(s) }).ToList();
                    int position = 5;
                    foreach (var each in SpiderList)
                    {
                        if (!existSpider.Any(a => each.PureUrl.Contains(a.Url)))
                        {
                            Debug.WriteLine($"[Add] {each.Url}");
                            each.Time = DateTime.Now;
                            Execute("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe", $"{ConfigurationSettings.AppSettings["SpiderPrefix"]} {each.FullParam}", IsConsole: false);
                            //await Task.Delay(7000);
                        }
                        else if (existSpider.FirstOrDefault(f => each.PureUrl.Contains(f.Url))?.Process is Process process)
                        {
                            //ChromeHelper.Instance.SetProcessForeground(process.MainWindowHandle);

                            if (each.IsExpired(RefreshRate))
                            {
                                try
                                {
                                    process.Kill();
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine(ex);
                                }
                                //each.Time = DateTime.Now;
                                //ChromeHelper.Instance.WinPostMessage(process.MainWindowHandle, 0x100, 0x74, 0);
                            }

                            //ChromeHelper.Instance.SetWindowRect(process, position, position, 1200, 600, true);
                            //position += 40;
                        }
                    }
                    await Task.Delay(5000);
                }
            }

            Process Execute(string exeFile, string argument = "", Action<string> outputCallback = null, Action<string> errorCallback = null, bool IsConsole = true)
            {
                ProcessStartInfo si = new ProcessStartInfo()
                {
                    FileName = exeFile,
                    Arguments = argument,
                    //必須要設定以下兩個屬性才可將輸出結果導向
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    //不顯示任何視窗
                    CreateNoWindow = true,
                    //以管理者身分執行
                    Verb = "runas",
                };
                Process p = new Process()
                {
                    StartInfo = si,
                    EnableRaisingEvents = true,
                };
                p.Start();

                //透過OutputDataReceived及ErrorDataReceived即時接收輸出內容
                p.OutputDataReceived += (o, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data) && outputCallback != null)
                    {
                        outputCallback(e.Data);
                    }
                };
                p.ErrorDataReceived += (o, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data) && errorCallback != null)
                    {
                        errorCallback(e.Data);
                    }
                };
                //呼叫Begin*ReadLine()開始接收輸出結果
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();

                if (IsConsole)
                    p.WaitForExit();

                return p;
            }

            private void ApiServer_DataReceived(object sender, Tuple<string, string, object> e)
            {
                if (e.Item3 is string strData)
                    OnLog(strData);
            }

            public void OnLog(string log)
            {
                if (log.Contains("#") && log.Split('#') is string[] arr && arr.Length == 2 && arr[1] is string url)
                {
                    if (!dicSpidernets.ContainsKey(url))
                    {
                        dicSpidernets.Add(url, DateTime.Now);
                    }
                    else if (log.Contains("data changed"))
                    {
                        dicSpidernets[url] = DateTime.Now;
                    }
                }
            }
            #endregion
        }
    }
}