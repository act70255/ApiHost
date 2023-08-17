using ApiHost.Host;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApiHost.Forms
{
    public partial class FormApiHost : Form
    {
        List<Process> ProcessList { get; set; } = new List<Process>();

        string Port()
        {
            return ConfigurationSettings.AppSettings["HostPort"];
        }
        string IPAddress()
        {
            return ConfigurationSettings.AppSettings["HostIP"];
        }
        string HostAddress()
        {
            return $"{IPAddress()}:{Port()}/";
        }

        IEnumerable<string> GetProcessPathFromConfig()
        {
            var keys = ConfigurationManager.AppSettings.Keys;
            foreach(var each in keys.Cast<object>().Where(f => f.ToString().StartsWith("ExecPath")).Select(s => ConfigurationManager.AppSettings.Get(s.ToString())))
            {
                yield return each;
            }
        }

        TextBox txtConsole;
        Button btnStart;
        Button btnStop;

        public FormApiHost()
        {
            InitializeComponent();
            
            btnStart = new Button()
            {
                BackColor = Color.Black,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Text = "Start",
                AutoSize = true,
                Location = new Point(0, 0),
            };
            btnStop = new Button()
            {
                BackColor = Color.Black,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Text = "Stop",
                AutoSize = true,
                Location = new Point(btnStart.Right, 0),
                Enabled = false,
            };
            txtConsole = new TextBox()
            {
                BackColor = Color.Black,
                ForeColor = Color.White,
                AutoSize = true,
                Multiline = true,
                Size = new Size(500, 0),
                ScrollBars = ScrollBars.Horizontal,
                Location = new Point(0, btnStart.Bottom),
            };

            txtConsole.TextChanged += (s, e) =>
            {
                if (txtConsole.Lines.Length > 40)
                {
                    var contentText = txtConsole.Text;
                    int firstCharIndex = contentText.IndexOf(Environment.NewLine);
                    if (firstCharIndex >= 0)
                    {
                        string remainingText = txtConsole.Text.Substring(firstCharIndex+1, txtConsole.Text.Length - firstCharIndex-1);
                        txtConsole.Text = remainingText;
                    }
                }
                int newHeight = txtConsole.GetLineFromCharIndex(txtConsole.TextLength - 1) * txtConsole.Font.Height;
                txtConsole.Height = newHeight;
            };

            txtConsole.GotFocus += (s, e) =>
            {
                txtConsole.DeselectAll();
            };

            btnStart.Click += (s, e) =>
            {
                Task.Run(() =>
                {
                    StopAllProcess();

                    foreach (var each in GetProcessPathFromConfig())
                    {
                        Execute(each, HostAddress(),
                        (data) =>
                        {
                            txtConsole.BeginInvoke(new Action(() =>
                            {
                                txtConsole.Text += $"{Environment.NewLine}{data}";
                            }));
                            Debug.WriteLine(data);
                        },
                        (error) =>
                        {
                            Debug.WriteLine(error);
                        });
                    }
                });
                btnStart.Enabled = false;
                btnStop.Enabled = true;
            };

            btnStop.Click += (s, e) =>
            {
                StopAllProcess();
                btnStart.Enabled = true;
                btnStop.Enabled = false;
            };

            Controls.Add(txtConsole);
            Controls.Add(btnStart);
            Controls.Add(btnStop);

            FormClosing += (s, e) =>
            {
                StopAllProcess();
            };
        }

        ~FormApiHost()
        {
            StopAllProcess();
        }

        //void StartServer()
        //{
        //    var label = this.Controls.Find("label", true).FirstOrDefault() as System.Windows.Forms.Label;
        //    HostProvider apiServer = null;
        //    apiServer = new HostProvider(HostAddress());
        //    apiServer.HostStatusChanged += (s, e) =>
        //    {
        //        label.Invoke(new Action(() =>
        //        {
        //            label.Text = $"HostStatusChanged {e} {DateTime.Now}";
        //        }));
        //    };
        //}

        void StopAllProcess()
        {
            ProcessList.ForEach(p =>
            {
                try
                {
                    p.Kill();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            });
            ProcessList.Clear();
        }

        int Execute(string exeFile, string argument = "", Action<string> outputCallback = null, Action<string> errorCallback = null)
        {
            ProcessStartInfo si = new ProcessStartInfo()
            {
                FileName = exeFile,
                Arguments = argument,
                //必須要設定以下兩個屬性才可將輸出結果導向
                UseShellExecute = false,
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
            ProcessList.Add(p);
            p.Start();

            //透過OutputDataReceived及ErrorDataReceived即時接收輸出內容
            p.OutputDataReceived +=
                (o, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data) && outputCallback != null)
                    {
                        outputCallback(e.Data);
                    }
                };
            p.ErrorDataReceived +=
                (o, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data) && errorCallback != null)
                    {
                        errorCallback(e.Data);
                    }
                };
            //呼叫Begin*ReadLine()開始接收輸出結果
            p.BeginOutputReadLine();
            p.BeginErrorReadLine();

            p.WaitForExit();
            return p.ExitCode;
        }
    }
}
