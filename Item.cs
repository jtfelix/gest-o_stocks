using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projecto
{
    
    internal class Item
    {
        public int ID { get; set; }
        public string Value { get; set; }

        

        public Item()
        {
            ID = 0;
            Value = "";
        }



        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
