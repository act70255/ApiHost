using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpiderNets.Compoments.Controls
{
    public partial class FormBorderlessAndAlwaysTop : Form, IMessageFilter
    {
        public bool IsAlwaysOnTop { get; set; }

        #region Dragable
        const int WM_NCLBUTTONDOWN = 0xA1;
        const int HT_CAPTION = 0x2;
        public const int WM_LBUTTONDOWN = 0x0201;
        internal HashSet<Control> controlsToMove = new HashSet<Control>();

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN &&
                 controlsToMove.Contains(Control.FromHandle(m.HWnd)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                return true;
            }
            return false;
        }
        #endregion
        public FormBorderlessAndAlwaysTop()
        {
            InitializeComponent();
            this.TopMost = false;
            this.Load+=(s, e) =>
            {
                InitializeContent();
                InitializeForm();
            };
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        }

        protected virtual void InitializeContent()
        {
            
        }

        protected virtual void InitializeForm()
        {
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.Location = new Point(0, 0);
            SetDragable();
        }

        protected void SetDragable()
        {
            //Dragable
            Application.AddMessageFilter(this);
            #region Register dragable
            foreach (var ctrl in GetAllControls(this))
            {
                if (ctrl is Label || ctrl is Panel)
                {
                    controlsToMove.Add(ctrl);
                }
            }
            #endregion
        }

        public static IEnumerable<Control> GetAllControls(Control control)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetAllControls(ctrl))
                                          .Concat(controls);
        }
    }
}
