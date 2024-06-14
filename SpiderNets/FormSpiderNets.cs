using SpiderNets.Compoments.Controls;
using SpiderNets.Service.SystemMonitor;
using SpiderNets.Utility.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpiderNets
{
    public partial class FormSpiderNets : FormBorderlessAndAlwaysTop
    {
        private static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        SystemMonitorService systemMonitorService = SystemMonitorService.Instance;
        List<CounterMonitor> CounterList = new List<CounterMonitor>();
        public string RefreshRate { get; set; } = "3000";
        public int GetInt(string key)
        {
            return Convert.ToInt32(RefreshRate);
        }

        public FormSpiderNets()
        {
            InitializeComponent();
            this.Shown += (s, e) =>
            {
                var refreshRate = GetInt("RefreshRate");
                CounterList.ForEach(each => { Task.Factory.StartNew(() => { each.StartNext(refreshRate); }); });
            };
        }

        protected override void InitializeContent()
        {
            this.pnlContent.AutoSize = true;
            this.pnlContent.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            int yPos = 0;
            #region Counter
            CounterList = systemMonitorService.GetCounterMonitors();
            foreach (var each in CounterList)
            {
                Label label = new Label
                {
                    AutoSize = true,
                    Text = "Initializing...",
                    Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    BackColor = Color.Black,
                    ForeColor = Color.White,
                    Location = new Point(0, yPos),
                };
                each.ValueUpdated += (s, e) =>
                {
                    if (s is CounterMonitor data)
                        label.BeginInvoke((MethodInvoker)delegate () { label.Text = $"[{data.CounterType.GetDescription()}]\t - {e.Data}"; });
                };
                this.pnlContent.Controls.Add(label);
                yPos += label.Height;
            }
            #endregion
            #region Operator
            #region TextBox RefreshRate
            TextBox txtRefreshRate = new TextBox
            {
                BackColor = Color.Black,
                ForeColor = Color.White,
                Width = 60,
                Text = RefreshRate,
                Location = new Point(0, yPos),
                TextAlign = HorizontalAlignment.Right,
            };
            txtRefreshRate.TextChanged += (s, e) =>
            {
                txtRefreshRate.Text = Regex.Replace(txtRefreshRate.Text, @"[^0-9]", "");
            };
            txtRefreshRate.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
            txtRefreshRate.TextChanged += (s, e) =>
            {
                RefreshRate = txtRefreshRate.Text;
            };
            this.pnlContent.Controls.Add(txtRefreshRate);
            #endregion
            #region CheckBox TopMost
            CheckBox chkLock = new CheckBox
            {
                BackColor = Color.Black,
                ForeColor = Color.White,
                Width = 80,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "TopMost",
                Checked = this.TopMost,
                Location = new Point(txtRefreshRate.Right + 10, yPos),
            };
            chkLock.CheckedChanged += (s, e) =>
            {
                this.TopMost = chkLock.Checked;
            };
            this.pnlContent.Controls.Add(chkLock);
            #endregion
            #region Button Exit
            Button btnExit = new Button
            {
                BackColor = Color.Black,
                ForeColor = Color.White,
                Text = "Exit",
                Location = new Point(chkLock.Right + 10, yPos),
            };
            btnExit.Click += (s, e) =>
            {
                this.Close();
            };
            this.pnlContent.Controls.Add(btnExit);
            #endregion

            #endregion
            base.InitializeContent();
        }
    }
}
