using ApiHost.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ApiHost.Spider
{
    internal class Program
    {
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        static void Main(string[] args)
        {
            string Port() => ConfigurationSettings.AppSettings["HostPort"];
            string IPAddress() => ConfigurationSettings.AppSettings["HostIP"];
            string HostAddress() => $"{IPAddress()}:{Port()}/";
            
            HostMessaging.Instance.DataReceived += (s, e) =>
            {
                Console.WriteLine($"DataReceived [{e.Item1}/{e.Item2}] {JsonConvert.SerializeObject(e.Item3)}");
            };
            new StartUp();
        }
        
        public class StartUp
        {
            #region Members
            ApiServer apiServer = null;
            DateTime lastUpdate = DateTime.Now;
            int refreshRate = 30;
            Process netProcess;
            Dictionary<string, DateTime> dicSpidernets = new Dictionary<string, DateTime>();
            string Port => ConfigurationSettings.AppSettings["HostPort"];
            string IPAddress => ConfigurationSettings.AppSettings["HostIP"];
            string HostAddress => $"{IPAddress}:{Port}/";
            Timer _timer;
            #endregion

            public StartUp()
            {
                _timer = new Timer(1000);
                _timer.Elapsed += _timer_Elapsed;
                _timer.AutoReset = true; // repeat forever
                _timer.Enabled = true;

                StartServer();
                ReStartNetProcess();

                var command = Console.ReadLine();
                while (command != "exit")
                {
                    switch (command)
                    {
                        case "start":
                            {
                                if (apiServer?.HostStatus != Host.HostStatus.HostStarted)
                                    apiServer.Start();
                                break;
                            }
                        case "stop":
                            {
                                apiServer.Stop();
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("UnHandeled command");
                                break;
                            }
                    }
                    command = Console.ReadLine();
                }
            }
            #region Method
            void StartServer()
            {
                if (apiServer == null)
                {
                    apiServer = new ApiServer(HostAddress);
                    apiServer.DataReceived += ApiServer_DataReceived; //+= (s, e) => { OnLog(e.Item2.ToString()); };
                }
                apiServer.Start();
            }

            void ReStartNetProcess()
            {
                StartServer();
                if (netProcess != null)
                {
                    netProcess.Kill();
                    netProcess.Dispose();
                    netProcess = null;
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
                    CreateNoWindow = true
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

            public void SetProcessForeground(string title)
            {
                IntPtr handle = FindWindow(null, title);

                if (handle == IntPtr.Zero)
                {
                    return;
                }

                SetForegroundWindow(handle);
            }

            private void _timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                foreach (var each in dicSpidernets.Where(f => f.Value < DateTime.Now.AddSeconds(-refreshRate)))
                {
                    var List = Process.GetProcessesByName("chrome");
                    foreach (Process p in List)
                    {
                        if (p.MainWindowTitle != "" && p.MainWindowTitle.Contains(each.Key))
                        {
                            Debug.WriteLine($"[Refresh] {p.MainWindowTitle}");
                            SetProcessForeground(p.MainWindowTitle);
                            PostMessage(p.MainWindowHandle, 0x100, 0x74, 0);
                            dicSpidernets[each.Key] = DateTime.Now;
                        }
                    }
                }
            }

            private void ApiServer_DataReceived(object sender, Tuple<string, string, object> e)
            {
                if(e.Item3 is string strData)
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