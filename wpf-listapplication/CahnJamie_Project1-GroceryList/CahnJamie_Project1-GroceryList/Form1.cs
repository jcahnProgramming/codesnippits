using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CahnJamie_Project1_GroceryList
{
    public partial class Form1 : Form
    {
        public Form1()
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

        public Form1 activeForm = null;
        public AddItems newForm = null;
        List<Item> itemListNeed = new List<Item>();
        List<Item> itemListHave = new List<Item>();
        List<Item> itemList = new List<Item>();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddItems newForm = new AddItems();
            newForm.sendList += newForm_SendList;
            newForm.ShowDialog();
        }

        private void newForm_SendList(object sender, EventArgs e)
        {
            for (int i = 0; i < ((List<Item>)sender).Count; i++)
            {
                if (((List<Item>)sender)[i].Have == 1)
                {
                    itemListHave.Add(((List<Item>)sender)[i]);
                }
                else if (((List<Item>)sender)[i].Need == 1)
                {
                    itemListNeed.Add(((List<Item>)sender)[i]);
                }
                else
                {
                    //this will catch any data that slips through the cracks.
                    itemList.Add(((List<Item>)sender)[i]);
                }
            }

            for (int i = 0; i < itemListHave.Count; i++)
            {
                lbHave.Items.Add(itemListHave[i].ToString());
            }
            for (int i = 0; i < itemListNeed.Count; i++)
            {
                lbNeed.Items.Add(itemListNeed[i].ToString());
            }
        }


        private void btnHaveToNeed_Click(object sender, EventArgs e)
        {
            if (lbHave.SelectedIndex >= 0)
            {
                string tmp = lbHave.SelectedItem.ToString();

                for (int i = 0; i < itemListHave.Count; i++)
                {
                    if (tmp == itemListHave[i].ToString())
                    {
                        itemListHave[i].Have = 0;
                        itemListHave[i].Need = 1;

                        itemListNeed.Add(itemListHave[i]);
                        itemListHave.RemoveAt(i);
                    }
                }

                lbHave.Items.Clear();
                lbNeed.Items.Clear();

                for (int i = 0; i < itemListHave.Count; i++)
                {
                    lbHave.Items.Add(itemListHave[i].ToString());
                }

                for (int i = 0; i < itemListNeed.Count; i++)
                {
                    lbNeed.Items.Add(itemListNeed[i].ToString());
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbNeed.SelectedIndex >= 0)
            {
                string tmp = lbNeed.SelectedItem.ToString();

                for (int i = 0; i < itemListNeed.Count; i++)
                {
                    if (tmp == itemListNeed[i].ToString())
                    {
                        itemListNeed.RemoveAt(i);
                    }
                }

                lbNeed.Items.Clear();

                for (int i = 0; i < itemListNeed.Count; i++)
                {
                    lbNeed.Items.Add(itemListNeed[i].ToString());
                }

            }
            else
            {
                string tmp = lbHave.SelectedItem.ToString();

                for (int i = 0; i < itemListNeed.Count; i++)
                {
                    if (tmp == itemListNeed[i].ToString())
                    {
                        itemListHave.RemoveAt(i);
                    }
                }

                lbHave.Items.Clear();

                for (int i = 0; i < itemListHave.Count; i++)
                {
                    lbHave.Items.Add(itemListHave[i].ToString());
                }
            }
        }

        private void btnNeedToHave_Click(object sender, EventArgs e)
        {
            if (lbNeed.SelectedIndex >= 0)
            {
                string tmp = lbNeed.SelectedItem.ToString();

                for (int i = 0; i < itemListNeed.Count; i++)
                {
                    if (tmp == itemListNeed[i].ToString())
                    {
                        itemListNeed[i].Have = 1;
                        itemListNeed[i].Need = 0;

                        itemListHave.Add(itemListNeed[i]);
                        itemListNeed.RemoveAt(i);
                    }
                }

                lbHave.Items.Clear();
                lbNeed.Items.Clear();

                for (int i = 0; i < itemListHave.Count; i++)
                {
                    lbHave.Items.Add(itemListHave[i].ToString());
                }

                for (int i = 0; i < itemListNeed.Count; i++)
                {
                    lbNeed.Items.Add(itemListNeed[i].ToString());
                }
            }
        }

        private void btnSaveList_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text File | *.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    sw.WriteLine("Grocery List");
                    sw.WriteLine("-------------");
                    sw.WriteLine();
                    sw.WriteLine("Items I Have: ");
                    sw.WriteLine();
                    for (int i = 0; i < itemListHave.Count; i++)
                    {
                        sw.WriteLine(itemListHave[i].ToString());
                    }

                    sw.WriteLine();
                    sw.WriteLine("Items I Need: ");
                    sw.WriteLine("-------------");
                    sw.WriteLine();
                    for (int i = 0; i < itemListNeed.Count; i++)
                    {
                        sw.WriteLine(itemListNeed[i].ToString());
                    }
                }
            }
        }

        private void lbHave_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbHave.SelectedIndex >= 0)
            {
                lbNeed.SelectedIndex = -1;
            }
        }

        private void lbNeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbNeed.SelectedIndex >= 0)
            {
                lbHave.SelectedIndex = -1;
            }
        }
    }
}
