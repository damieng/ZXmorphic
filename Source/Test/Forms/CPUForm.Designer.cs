namespace Test.Forms
{
	partial class CPUForm
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
			if (disposing && (components != null)) {
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "IX",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "IY",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "PC",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "SP",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "IR",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] {
            "AF",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] {
            "BC",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "DE",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] {
            "HL",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem(new string[] {
            "AF",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem(new string[] {
            "BC",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem12 = new System.Windows.Forms.ListViewItem(new string[] {
            "DE",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem13 = new System.Windows.Forms.ListViewItem(new string[] {
            "HL",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem14 = new System.Windows.Forms.ListViewItem(new string[] {
            "T-states",
            "",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem15 = new System.Windows.Forms.ListViewItem(new string[] {
            "Interrupt mode",
            "",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem16 = new System.Windows.Forms.ListViewItem(new string[] {
            "Instruction set",
            "",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem17 = new System.Windows.Forms.ListViewItem(new string[] {
            "Indexer register",
            "",
            ""}, -1);
            this.CommonListView = new System.Windows.Forms.ListView();
            this.RegisterColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.MainListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AltListView = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.StatusListView = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.FlagsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CommonListView
            // 
            this.CommonListView.AllowColumnReorder = true;
            this.CommonListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RegisterColumn,
            this.ValueColumn});
            this.CommonListView.FullRowSelect = true;
            this.CommonListView.GridLines = true;
            this.CommonListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.CommonListView.Location = new System.Drawing.Point(138, 25);
            this.CommonListView.Name = "CommonListView";
            this.CommonListView.Size = new System.Drawing.Size(125, 132);
            this.CommonListView.TabIndex = 4;
            this.CommonListView.UseCompatibleStateImageBehavior = false;
            this.CommonListView.View = System.Windows.Forms.View.Details;
            // 
            // RegisterColumn
            // 
            this.RegisterColumn.Text = "Register";
            // 
            // ValueColumn
            // 
            this.ValueColumn.Text = "Value";
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 500;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Common";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(266, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Main";
            // 
            // MainListView
            // 
            this.MainListView.AllowColumnReorder = true;
            this.MainListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.MainListView.FullRowSelect = true;
            this.MainListView.GridLines = true;
            this.MainListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9});
            this.MainListView.Location = new System.Drawing.Point(269, 25);
            this.MainListView.Name = "MainListView";
            this.MainListView.Size = new System.Drawing.Size(125, 132);
            this.MainListView.TabIndex = 8;
            this.MainListView.UseCompatibleStateImageBehavior = false;
            this.MainListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Register";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Value";
            // 
            // AltListView
            // 
            this.AltListView.AllowColumnReorder = true;
            this.AltListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.AltListView.FullRowSelect = true;
            this.AltListView.GridLines = true;
            this.AltListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem10,
            listViewItem11,
            listViewItem12,
            listViewItem13});
            this.AltListView.Location = new System.Drawing.Point(400, 25);
            this.AltListView.Name = "AltListView";
            this.AltListView.Size = new System.Drawing.Size(125, 132);
            this.AltListView.TabIndex = 9;
            this.AltListView.UseCompatibleStateImageBehavior = false;
            this.AltListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Register";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Value";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(397, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Alternate";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Internal";
            // 
            // StatusListView
            // 
            this.StatusListView.AllowColumnReorder = true;
            this.StatusListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.StatusListView.GridLines = true;
            this.StatusListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem14,
            listViewItem15,
            listViewItem16,
            listViewItem17});
            this.StatusListView.Location = new System.Drawing.Point(12, 185);
            this.StatusListView.Name = "StatusListView";
            this.StatusListView.Size = new System.Drawing.Size(437, 96);
            this.StatusListView.TabIndex = 12;
            this.StatusListView.UseCompatibleStateImageBehavior = false;
            this.StatusListView.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "State";
            this.columnHeader5.Width = 158;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Current";
            this.columnHeader6.Width = 130;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Previous";
            this.columnHeader7.Width = 133;
            // 
            // FlagsCheckedListBox
            // 
            this.FlagsCheckedListBox.FormattingEnabled = true;
            this.FlagsCheckedListBox.Items.AddRange(new object[] {
            "Carry",
            "Negative",
            "Parity/overflow",
            "3",
            "Half carry",
            "5",
            "Zero",
            "Sign"});
            this.FlagsCheckedListBox.Location = new System.Drawing.Point(12, 25);
            this.FlagsCheckedListBox.Name = "FlagsCheckedListBox";
            this.FlagsCheckedListBox.Size = new System.Drawing.Size(120, 132);
            this.FlagsCheckedListBox.TabIndex = 13;
            this.FlagsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.FlagsCheckedListBox_ItemCheck);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Flags";
            // 
            // CPUForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 294);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.FlagsCheckedListBox);
            this.Controls.Add(this.StatusListView);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AltListView);
            this.Controls.Add(this.MainListView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CommonListView);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CPUForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "CPU";
            this.VisibleChanged += new System.EventHandler(this.CPUForm_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView CommonListView;
		private System.Windows.Forms.ColumnHeader RegisterColumn;
		private System.Windows.Forms.ColumnHeader ValueColumn;
        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView MainListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView AltListView;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView StatusListView;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.CheckedListBox FlagsCheckedListBox;
		private System.Windows.Forms.Label label5;
	}
}