using DevExpress.XtraEditors;
using System;
using Newtonsoft.Json;
using System.Configuration;
using SIV.Entities;
using System.Drawing;
using System.Windows.Forms;

namespace SIV.Views
{
    public partial class LoginView : XtraForm
    {
        private Context Context;
        public LoginView(Context context)
        {
            InitializeComponent();
            Context = context;
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
                });

                if (response.Code == CodeResponse.Error)
                {
                    txtEmpNumber.Text = "";
                    throw new Exception((string)response.Data);
                }

                employee = JsonConvert.DeserializeObject<Employee>(response.Data.ToString());

                response = Utils.ClientHelper.Send(new SIVRequest
                {
                    Method = "SetUserBlockedByTerminal",
                    Parameters = new SIVParameters { EmployeeParameters = employee },
                    TerminalSource = Convert.ToInt32(Properties.Settings.Default.Terminal)
                });

                if (response.Code == CodeResponse.Error) { throw new Exception((string)response.Data); }

                Context.CheckView.Show();
                this.Hide();

                txtEmpNumber.Text = "";
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message); }
        }

        private void LoginView_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '1':
                    txtEmpNumber.Text += "1";
                    break;
                case '2':
                    txtEmpNumber.Text += "2";
                    break;
                case '3':
                    txtEmpNumber.Text += "3";
                    break;
                case '4':
                    txtEmpNumber.Text += "4";
                    break;
                case '5':
                    txtEmpNumber.Text += "5";
                    break;
                case '6':
                    txtEmpNumber.Text += "6";
                    break;
                case '7':
                    txtEmpNumber.Text += "7";
                    break;
                case '8':
                    txtEmpNumber.Text += "8";
                    break;
                case '9':
                    txtEmpNumber.Text += "9";
                    break;
                case '0':
                    txtEmpNumber.Text += "0";
                    break;
                case '\u007F':
                    txtEmpNumber.Text = "";
                    break;
                case '\r':
                    Login_Click(new object(), new EventArgs());
                    break;
                case '\b':
                    txtEmpNumber.Text = "";
                    break;
                default:
                    e.Handled = true;
                    break;
            }


        }
    }
}