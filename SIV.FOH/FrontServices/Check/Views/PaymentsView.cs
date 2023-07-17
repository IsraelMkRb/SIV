using DevExpress.XtraEditors;
using System;
using SIV.Entities;

namespace SIV.Views
{
    public partial class PaymentsView : DevExpress.XtraEditors.XtraForm
    {
        private Context context;
        public PaymentsView(Context context)
        {
            InitializeComponent();
            this.context = context;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            context.LoginView.Show();
            this.Hide();
        }

        private void btnBackCheckView_Click(object sender, EventArgs e)
        {
            context.CheckView.Show();
            this.Hide();
        }

        private void PaymentsView_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}