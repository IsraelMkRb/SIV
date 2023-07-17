using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIV.Entities
{
    public class Item
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public string LongName { get; set; }
        public decimal Price { get; set; }
    }
    public class Items : List<Item> { }
}
