using ApiHost.Host;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApiHost.Forms
{
    public partial class FormApiHost : Form
    {
        //private static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //Get config from app.config
        private readonly string hostAddress = System.Configuration.ConfigurationSettings.AppSettings["hostAddress"];
        public string Port
        {
            get
            {
                return System.Configuration.ConfigurationSettings.AppSettings["HostPort"];
            }
            set
            {
                System.Configuration.ConfigurationSettings.AppSettings["HostPort"] = value;
            }
        }
        public string IPAddress
        {
            get
            {
                return System.Configuration.ConfigurationSettings.AppSettings["HostIP"];
            }
            set
            {
                System.Configuration.ConfigurationSettings.AppSettings["HostIP"] = value;
            }
        }
        public FormApiHost()
        {
            InitializeComponent();

            Label label = new Label()
            {
                BackColor = Color.Black,
                ForeColor = Color.White,
                AutoSize = true,
            };

            var hostAddress = $"{IPAddress}:{Port}";

            HostProvider apiServer = null;
            apiServer = new HostProvider(hostAddress);
            apiServer.HostStatusChanged += (s, e) =>
            {
                label.Invoke(new Action(() =>
                {
                    label.Text = $"HostStatusChanged {e} {DateTime.Now}";
                }));
            };
            Controls.Add(label);
            Shown += (s, e) =>
            {
                apiServer.Start();
            };
        }
    }
}
