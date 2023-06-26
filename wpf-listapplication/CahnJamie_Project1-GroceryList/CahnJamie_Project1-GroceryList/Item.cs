using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CahnJamie_Project1_GroceryList
{
    public class Item
    {
            private int have;
            private int need;
            private string item;

            public int Have { get => have; set => have = value; }
            public int Need { get => need; set => need = value; }
            public string ItemName { get => item; set => item = value; }

            public Item(string ItemName, int Have, int Need)
            {
                item = ItemName;
                have = Have;
                need = Need;
            }

            public override string ToString()
            {
                return ItemName;
            }

        
    }
}
