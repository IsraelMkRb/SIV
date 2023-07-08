using DevExpress.XtraEditors;
using System;
using System.Configuration;
using SIV.Entities;
using System.Drawing;
using System.Windows.Forms;

namespace SIV.Views
{
    public partial class LoginView : XtraForm
    {
        public LoginView()
        {
            InitializeComponent();

        }

        public Employee employee;

        private void LoginView_Resize(object sender, EventArgs e)
        {
            Container_Buttons.Location = new Point(x: (this.Width / 2) - (Container_Buttons.Width / 2),
                                                   y: (this.Height / 2) - (Container_Buttons.Height / 2));
        }

        #region Numbers
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            txtEmpNumber.Text += "1";
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            txtEmpNumber.Text += "2";
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            txtEmpNumber.Text += "3";
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            txtEmpNumber.Text += "4";
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            txtEmpNumber.Text += "5";
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            txtEmpNumber.Text += "6";
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            txtEmpNumber.Text += "7";
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            txtEmpNumber.Text += "8";
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            txtEmpNumber.Text += "9";
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            txtEmpNumber.Text += "0";
        }
        private void simpleButton10_Click(object sender, EventArgs e)
        {
            txtEmpNumber.Text = "";
        }
        #endregion

        private void Login_Click(object sender, EventArgs e)
        {
            try
            {
                SIVResponse response = Utils.ClientHelper.Send(new SIVRequest
                {
                    Method = "Login_FromTerminal",
                    Parameters = new SIVParameters { EmployeeParameters = new Employee { Password = txtEmpNumber.Text } },
                    TerminalSource = Convert.ToInt32(ConfigurationManager.AppSettings["Terminal"])
                });

                if (response.Code == CodeResponse.Error) 
                    throw new Exception((string)response.Data);

                employee = (Employee)response.Data;
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }


        }
    }
}