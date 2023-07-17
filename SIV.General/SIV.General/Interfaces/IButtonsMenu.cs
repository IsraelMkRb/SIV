using System;
using System.Drawing;

namespace SIV.Entities
{
    public interface IButtonsMenu
    {
         Action Action { get; set; }
        string DisplayText { get; set; }
        Font DisplayFont { get; set; }
        Color DisplayBackColor { get; set; }
        Color DisplayForeColor { get; set; }
        Image DisplayBackImage { get; set; }
    }
}
