using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Entities
{

    public class Menu : IButtonsMenu
    {
        public Action Action { get; set; }
        public string DisplayText { get; set; }
        public Font DisplayFont { get; set; }
        public Color DisplayBackColor { get; set; }
        public Color DisplayForeColor { get; set; }
        public Image DisplayBackImage { get; set; }
    }

    public class Menus : List<Menu> { }
}
