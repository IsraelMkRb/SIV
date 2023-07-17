using System;
using SIV.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SIV.Controls
{
    public partial class ButtonsMenuContainer : UserControl
    {
        public ButtonsMenuContainer()
        {
            InitializeComponent();
        }
        private int CurrentGroup { get; set; } = 1;
        private int TotalGroup { get; set; } = 1;
        private IEnumerable<IButtonsMenu> _DataSource { get; set; }
        public IEnumerable<IButtonsMenu> DataSource
        {
            get { return _DataSource; }
            set
            {
                if (value is null || value.Count() == 0) return;
                SetButtonsConfig(value);
                _DataSource = value;
            }
        }

        private void SetButtonsConfig(IEnumerable<IButtonsMenu> dataSource)
        {
            List<IButtonsMenu> _datasource = dataSource.ToList();
            int CompletedGroups = _datasource.Count / 5;
            int incompletedGroup = _datasource.ToList().Count % 5 > 0 ? 1 : 0;
            TotalGroup = CompletedGroups + incompletedGroup;

            Btn_GotoLeft.Enabled = CurrentGroup != 1;
            Btn_GotoRight.Enabled = TotalGroup > 1;
            Btn_GotoRight.Enabled = CurrentGroup < TotalGroup;

            int startIndex = (CurrentGroup - 1) * 5;

            List<SimpleButton> currentGroupButtons = new List<SimpleButton>() { Position_1, Position_2, Position_3, Position_4, Position_5 };

            int availableElements = Math.Min(_datasource.Count - startIndex, 5);

            for (int i = 0; i < availableElements; i++)
            {
                currentGroupButtons[i].Text = _datasource[startIndex + i].DisplayText;
                currentGroupButtons[i].Tag = _datasource[startIndex + i].Action;
                currentGroupButtons[i].BackgroundImage = _datasource[startIndex + i].DisplayBackImage;
                currentGroupButtons[i].Appearance.Font = _datasource[startIndex + i].DisplayFont;
                currentGroupButtons[i].Appearance.ForeColor = _datasource[startIndex + i].DisplayForeColor;
                currentGroupButtons[i].Appearance.BackColor = _datasource[startIndex + i].DisplayBackColor;
                currentGroupButtons[i].Enabled = true;
            }

            for (int i = availableElements; i < 5; i++)
            {
                currentGroupButtons[i].Text = "";
                currentGroupButtons[i].BackColor = Color.Transparent;
                currentGroupButtons[i].BackgroundImage = null;
                currentGroupButtons[i].Enabled = false;
            }
        }

        private void Btn_GotoLeft_Click(object sender, EventArgs e)
        {
            CurrentGroup -= 1;
            SetButtonsConfig(_DataSource);
        }

        private void Btn_GotoRight_Click(object sender, EventArgs e)
        {
            CurrentGroup += 1;
            SetButtonsConfig(_DataSource);
        }
    }
}
