namespace CahnJamie_Project1_GroceryList
{
    partial class AddItems
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddItems));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnGroceryList = new System.Windows.Forms.Button();
            this.btnAddtoList = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.rdoNeed = new System.Windows.Forms.RadioButton();
            this.rdoHave = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(199, 379);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnGroceryList
            // 
            this.btnGroceryList.Location = new System.Drawing.Point(8, 229);
            this.btnGroceryList.Margin = new System.Windows.Forms.Padding(2);
            this.btnGroceryList.Name = "btnGroceryList";
            this.btnGroceryList.Size = new System.Drawing.Size(156, 50);
            this.btnGroceryList.TabIndex = 7;
            this.btnGroceryList.Text = "Back To Grocery List";
            this.btnGroceryList.UseVisualStyleBackColor = true;
            this.btnGroceryList.Click += new System.EventHandler(this.btnGroceryList_Click);
            // 
            // btnAddtoList
            // 
            this.btnAddtoList.Location = new System.Drawing.Point(97, 108);
            this.btnAddtoList.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddtoList.Name = "btnAddtoList";
            this.btnAddtoList.Size = new System.Drawing.Size(68, 27);
            this.btnAddtoList.TabIndex = 6;
            this.btnAddtoList.Text = "Add to List";
            this.btnAddtoList.UseVisualStyleBackColor = true;
            this.btnAddtoList.Click += new System.EventHandler(this.btnAddtoList_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(8, 108);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(68, 27);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // rdoNeed
            // 
            this.rdoNeed.AutoSize = true;
            this.rdoNeed.Location = new System.Drawing.Point(38, 77);
            this.rdoNeed.Margin = new System.Windows.Forms.Padding(2);
            this.rdoNeed.Name = "rdoNeed";
            this.rdoNeed.Size = new System.Drawing.Size(51, 17);
            this.rdoNeed.TabIndex = 4;
            this.rdoNeed.TabStop = true;
            this.rdoNeed.Text = "Need";
            this.rdoNeed.UseVisualStyleBackColor = true;
            // 
            // rdoHave
            // 
            this.rdoHave.AutoSize = true;
            this.rdoHave.Checked = true;
            this.rdoHave.Location = new System.Drawing.Point(38, 58);
            this.rdoHave.Margin = new System.Windows.Forms.Padding(2);
            this.rdoHave.Name = "rdoHave";
            this.rdoHave.Size = new System.Drawing.Size(51, 17);
            this.rdoHave.TabIndex = 3;
            this.rdoHave.TabStop = true;
            this.rdoHave.Text = "Have";
            this.rdoHave.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 14);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Add Items";
            // 
            // txtItemName
            // 
            this.txtItemName.Location = new System.Drawing.Point(38, 35);
            this.txtItemName.Margin = new System.Windows.Forms.Padding(2);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(128, 20);
            this.txtItemName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 39);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Item:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGroceryList);
            this.groupBox1.Controls.Add(this.btnAddtoList);
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Controls.Add(this.rdoNeed);
            this.groupBox1.Controls.Add(this.rdoHave);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtItemName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(21, 53);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(173, 289);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // AddItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 395);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddItems";
            this.ShowIcon = false;
            this.Text = "AddItems";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnGroceryList;
        private System.Windows.Forms.Button btnAddtoList;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.RadioButton rdoNeed;
        private System.Windows.Forms.RadioButton rdoHave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}