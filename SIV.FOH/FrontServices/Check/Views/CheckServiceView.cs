using System;
using SIV.Entities;
using SIV.Utils;
using System.Windows.Forms;

namespace SIV.Views
{
    public partial class CheckServices : DevExpress.XtraEditors.XtraForm
    {
        private Context context;
        public CheckServices(Context context)
        {
            InitializeComponent();
            this.context = context;           
        }

        private void CheckServices_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            
        }

        private void CheckServices_Load(object sender, EventArgs e)
        {
            buttonsMenuContainer1.DataSource = new Menus() 
            {
                new Entities.Menu(){ DisplayText = "Desayunos" },
                new Entities.Menu(){ DisplayText = "Comida del dia" },
                new Entities.Menu(){ DisplayText = "Mariscos y pescados" },
                new Entities.Menu(){ DisplayText = "Especial dia de las madres" },
                new Entities.Menu(){ DisplayText = "Especiales" },
                new Entities.Menu(){ DisplayText = "Cocina Mexicana" },
                new Entities.Menu(){ DisplayText = "Pizzas" },
                new Entities.Menu(){ DisplayText = "Bebidas" }
            };
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            context.LoginView.Show();
            this.Hide();
        }

        private void btnPrintCheck_Click(object sender, EventArgs e)
        {
            PrinterHelper.PrintCheck(new Items());
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            context.Paymentsview.Show();
            this.Hide();
        }
    }
}
