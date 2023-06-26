using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CahnJamie_Project1_GroceryList
{
    public partial class AddItems : Form
    {
        public AddItems()
        {
            InitializeComponent();
            //HandleClientWindowSize();
        }

        //Written by Keith Webster.  Used with permission.  Not to be distributed.  
        //Place this inside the class space in the form you would like to size.
        //Call this method AFTER InitializeComponent() inside the form's constructor.
        void HandleClientWindowSize()
        {
            //Modify ONLY these float values
            float HeightValueToChange = 1.4f;
            float WidthValueToChange = 6.0f;


            //DO NOT MODIFY THIS CODE
            int height = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Size.Height / HeightValueToChange);
            int width = Convert.ToInt32(Screen.PrimaryScreen.WorkingArea.Size.Width / WidthValueToChange);
            if (height < Size.Height)

                height = Size.Height;
            if (width < Size.Width)

                width = Size.Width;
            this.Size = new Size(width, height);
            this.Size = new Size(376, 720);
        }

        public List<Item> itemList = new List<Item>();
        int haveItem;
        int needItem;
        public EventHandler sendList;
        public Form1 activeForm = null;


        private void btnClear_Click(object sender, EventArgs e)
        {
            txtItemName.Clear();
            rdoHave.Checked = true;
        }

        private void btnAddtoList_Click(object sender, EventArgs e)
        {
            string itemName = txtItemName.Text;
            if (itemName == "")
            {
                MessageBox.Show("Please type in the name of an item to add it to the list.");
            }
            if (rdoHave.Checked == true)
            {
                haveItem = 1;
                needItem = 0;
            }
            else if (rdoHave.Checked == false)
            {
                needItem = 1;
                haveItem = 0;
            }
            else
            {
                MessageBox.Show("Invalid Response, Please Fix issues then resubmit.");
            }

            Item tmp = new Item(itemName, haveItem, needItem);
            itemList.Add(tmp);

            txtItemName.Clear();
            rdoHave.Checked = true;
        }

        private void btnGroceryList_Click(object sender, EventArgs e)
        {
            sendList(itemList, new EventArgs());
            this.Close();
        }
    }
}
