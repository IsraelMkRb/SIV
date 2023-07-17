using System;
using System.Windows.Forms;

namespace SIV.Entities
{
    public class Context
    {
        private Form pLoginView;
        public Form LoginView
        {
            get
            {
                return pLoginView;
            }
            set
            {
                if (value is null) { return; }

                value.VisibleChanged += (object sender, EventArgs e) => { if (((Form)sender).Visible) { Timer.Stop(); } else { Timer.Start(); } };
                pLoginView = value;
            }
        }
        private Form pCheckView;
        public Form CheckView
        {
            get
            {
                return pCheckView;
            }
            set
            {
                value.Click += (object sender, EventArgs e) => { RestartTimerLock(); };
                RecursiveControlTraversal(value.Controls);

                pCheckView = value;
            }
        }
        public Form pPaymentsView;
        public Form Paymentsview
        {
            get
            {
                return pPaymentsView;
            }
            set
            {
                value.Click += (object sender, EventArgs e) => { RestartTimerLock(); };
                RecursiveControlTraversal(value.Controls);

                pPaymentsView = value;
            }
        }
        private Timer Timer { get; set; }
        private int interval = 10000;

        public Context()
        {
            Timer = new Timer
            {
                Interval = interval,
                Enabled = true
            };
            Timer.Tick += Timer_Tick;
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            if (LoginView.Visible) { return; }
            LoginView.Show();
            CheckView.Hide();
            Paymentsview.Hide();
        }
        private void RestartTimerLock()
        {
            Timer.Stop();
            Timer.Start();
        }
        public void SetIntervalLock(int Interval)
        {
            this.interval = Interval;
        }
        private void RecursiveControlTraversal(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control.Controls.Count > 0) { RecursiveControlTraversal(control.Controls); }
                control.Click += (object sender, EventArgs e) => { RestartTimerLock(); };
            }
        }
    }
}
