namespace CahnJamie_Project1_GroceryList
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSaveList = new System.Windows.Forms.Button();
            this.btnNeedToHave = new System.Windows.Forms.Button();
            this.btnHaveToNeed = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbNeed = new System.Windows.Forms.ListBox();
            this.lbHave = new System.Windows.Forms.ListBox();
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnSaveList);
            this.groupBox1.Controls.Add(this.btnNeedToHave);
            this.groupBox1.Controls.Add(this.btnHaveToNeed);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbNeed);
            this.groupBox1.Controls.Add(this.lbHave);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Location = new System.Drawing.Point(22, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(170, 287);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(47, 9);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 39);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add Items";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(62, 210);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(42, 41);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.TabStop = false;
            this.btnDelete.Text = "X";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSaveList
            // 
            this.btnSaveList.Location = new System.Drawing.Point(47, 254);
            this.btnSaveList.Margin = new System.Windows.Forms.Padding(2);
            this.btnSaveList.Name = "btnSaveList";
            this.btnSaveList.Size = new System.Drawing.Size(72, 31);
            this.btnSaveList.TabIndex = 11;
            this.btnSaveList.TabStop = false;
            this.btnSaveList.Text = "Save List";
            this.btnSaveList.UseVisualStyleBackColor = true;
            this.btnSaveList.Click += new System.EventHandler(this.btnSaveList_Click);
            // 
            // btnNeedToHave
            // 
            this.btnNeedToHave.Location = new System.Drawing.Point(107, 210);
            this.btnNeedToHave.Margin = new System.Windows.Forms.Padding(2);
            this.btnNeedToHave.Name = "btnNeedToHave";
            this.btnNeedToHave.Size = new System.Drawing.Size(42, 41);
            this.btnNeedToHave.TabIndex = 17;
            this.btnNeedToHave.TabStop = false;
            this.btnNeedToHave.Text = "<";
            this.btnNeedToHave.UseVisualStyleBackColor = true;
            this.btnNeedToHave.Click += new System.EventHandler(this.btnNeedToHave_Click);
            // 
            // btnHaveToNeed
            // 
            this.btnHaveToNeed.Location = new System.Drawing.Point(16, 210);
            this.btnHaveToNeed.Margin = new System.Windows.Forms.Padding(2);
            this.btnHaveToNeed.Name = "btnHaveToNeed";
            this.btnHaveToNeed.Size = new System.Drawing.Size(42, 41);
            this.btnHaveToNeed.TabIndex = 16;
            this.btnHaveToNeed.TabStop = false;
            this.btnHaveToNeed.Text = ">";
            this.btnHaveToNeed.UseVisualStyleBackColor = true;
            this.btnHaveToNeed.Click += new System.EventHandler(this.btnHaveToNeed_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(116, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Need";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Have";
            // 
            // lbNeed
            // 
            this.lbNeed.FormattingEnabled = true;
            this.lbNeed.Location = new System.Drawing.Point(96, 75);
            this.lbNeed.Margin = new System.Windows.Forms.Padding(2);
            this.lbNeed.Name = "lbNeed";
            this.lbNeed.Size = new System.Drawing.Size(74, 134);
            this.lbNeed.TabIndex = 13;
            this.lbNeed.TabStop = false;
            this.lbNeed.SelectedIndexChanged += new System.EventHandler(this.lbNeed_SelectedIndexChanged);
            // 
            // lbHave
            // 
            this.lbHave.FormattingEnabled = true;
            this.lbHave.Location = new System.Drawing.Point(1, 75);
            this.lbHave.Margin = new System.Windows.Forms.Padding(2);
            this.lbHave.Name = "lbHave";
            this.lbHave.Size = new System.Drawing.Size(74, 134);
            this.lbHave.TabIndex = 12;
            this.lbHave.TabStop = false;
            this.lbHave.SelectedIndexChanged += new System.EventHandler(this.lbHave_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 391);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "GroceryList";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSaveList;
        private System.Windows.Forms.Button btnNeedToHave;
        private System.Windows.Forms.Button btnHaveToNeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbNeed;
        private System.Windows.Forms.ListBox lbHave;
        private System.Windows.Forms.Button btnAdd;
    }
}

