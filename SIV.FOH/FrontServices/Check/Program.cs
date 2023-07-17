using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SIV
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Entities.Context Context = CreateContext();
            Application.Run(Context.LoginView);
        }

        static Entities.Context CreateContext()
        {
            var context = new Entities.Context();
            context.LoginView = new Views.LoginView(context);
            context.CheckView = new Views.CheckServices(context);
            context.Paymentsview = new Views.PaymentsView(context);
            
            return context;
        }
    }
}
