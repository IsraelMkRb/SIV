using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Entities
{
    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public byte[] FingerTemplate { get; set; }
        public int TerminalLogged { get; set; }
    }

    public class Users : List<Employee> { }
}
