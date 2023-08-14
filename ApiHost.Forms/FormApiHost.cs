using ApiHost.Host;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApiHost.Forms
{
    public partial class FormApiHost : Form
    {
        List<Process> ProcessList = new List<Process>();
        //private static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //Get config from app.config

        Label label;
        Button btnStart;
        Button btnStop;

        public FormApiHost()
        {
            InitializeComponent();

            label = new Label()
            {
                BackColor = Color.Black,
                ForeColor = Color.White,
                AutoSize = true,
            };
            btnStart = new Button()
            {
                Text = "Start",
                AutoSize = true,
                Location = new Point(0, label.Height),
            };
            btnStop = new Button()
            {
                Text = "Stop",
                AutoSize = true,
                Location = new Point(btnStart.Width, label.Height),
                Enabled = false,
            };
            Controls.Add(label);

            btnStart.Click += (s, e) =>
            {
                Task.Run(() =>
                {
                    StopAllProcess();
                    Execute("C:\\Users\\James.lin\\source\\repos\\ApiHost\\ApiHost.CLI\\bin\\Debug\\ApiHost.CLI.exe", "",
                        (data) =>
                        {
                            label.BeginInvoke(new Action(() =>
                            {
                                label.Text = data;
                            }));
                            Debug.WriteLine(data);
                        },
                        (error) =>
                        {
                            Debug.WriteLine(error);
                        });
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

            this.Controls.Add(btnStart);
            this.Controls.Add(btnStop);

            FormClosing += (s, e) =>
            {
                StopAllProcess();
            };
        }

        ~FormApiHost()
        {
            StopAllProcess();
        }

        void StartServer()
        {
            string Port()
            {
                return System.Configuration.ConfigurationSettings.AppSettings["HostPort"];
            }
            string IPAddress()
            {
                return System.Configuration.ConfigurationSettings.AppSettings["HostIP"];
            }

            var hostAddress = $"{IPAddress()}:{Port()}";
            var label = this.Controls.Find("label", true).FirstOrDefault() as System.Windows.Forms.Label;
            HostProvider apiServer = null;
            apiServer = new HostProvider(hostAddress);
            apiServer.HostStatusChanged += (s, e) =>
            {
                label.Invoke(new Action(() =>
                {
                    label.Text = $"HostStatusChanged {e} {DateTime.Now}";
                }));
            };

        }

        void StopAllProcess()
        {
            ProcessList.ForEach(p =>
            {
                p.Kill();
            });
            ProcessList.RemoveAll(p => p.HasExited);
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
