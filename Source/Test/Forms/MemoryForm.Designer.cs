namespace Test
{
	partial class MemoryForm
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
            this.BankCombo = new System.Windows.Forms.ComboBox();
            this.AddressNumeric = new System.Windows.Forms.NumericUpDown();
            this.AddressLabel = new System.Windows.Forms.Label();
            this.BankLabel = new System.Windows.Forms.Label();
            this.FollowCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.HexTab = new System.Windows.Forms.TabPage();
            this.HexTextBox = new System.Windows.Forms.RichTextBox();
            this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.AddressNumeric)).BeginInit();
            this.Tabs.SuspendLayout();
            this.HexTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // BankCombo
            // 
            this.BankCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BankCombo.FormattingEnabled = true;
            this.BankCombo.Items.AddRange(new object[] {
            "Physical"});
            this.BankCombo.Location = new System.Drawing.Point(83, 25);
            this.BankCombo.Name = "BankCombo";
            this.BankCombo.Size = new System.Drawing.Size(109, 21);
            this.BankCombo.TabIndex = 0;
            // 
            // AddressNumeric
            // 
            this.AddressNumeric.Hexadecimal = true;
            this.AddressNumeric.Location = new System.Drawing.Point(15, 25);
            this.AddressNumeric.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.AddressNumeric.Name = "AddressNumeric";
            this.AddressNumeric.Size = new System.Drawing.Size(62, 21);
            this.AddressNumeric.TabIndex = 1;
            this.AddressNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.AddressNumeric.ValueChanged += new System.EventHandler(this.AddressNumeric_ValueChanged);
            // 
            // AddressLabel
            // 
            this.AddressLabel.AutoSize = true;
            this.AddressLabel.Location = new System.Drawing.Point(12, 9);
            this.AddressLabel.Name = "AddressLabel";
            this.AddressLabel.Size = new System.Drawing.Size(46, 13);
            this.AddressLabel.TabIndex = 2;
            this.AddressLabel.Text = "Address";
            // 
            // BankLabel
            // 
            this.BankLabel.AutoSize = true;
            this.BankLabel.Location = new System.Drawing.Point(80, 9);
            this.BankLabel.Name = "BankLabel";
            this.BankLabel.Size = new System.Drawing.Size(30, 13);
            this.BankLabel.TabIndex = 3;
            this.BankLabel.Text = "Bank";
            // 
            // FollowCombo
            // 
            this.FollowCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FollowCombo.FormattingEnabled = true;
            this.FollowCombo.Items.AddRange(new object[] {
            "No",
            "Stack Pointer (SP)",
            "Last Access"});
            this.FollowCombo.Location = new System.Drawing.Point(198, 25);
            this.FollowCombo.Name = "FollowCombo";
            this.FollowCombo.Size = new System.Drawing.Size(109, 21);
            this.FollowCombo.TabIndex = 7;
            this.FollowCombo.SelectedIndexChanged += new System.EventHandler(this.FollowCombo_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Follow";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(214, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 9;
            // 
            // Tabs
            // 
            this.Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs.Controls.Add(this.HexTab);
            this.Tabs.Location = new System.Drawing.Point(15, 52);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(337, 242);
            this.Tabs.TabIndex = 10;
            this.Tabs.SelectedIndexChanged += new System.EventHandler(this.Tabs_SelectedIndexChanged);
            // 
            // HexTab
            // 
            this.HexTab.Controls.Add(this.HexTextBox);
            this.HexTab.Location = new System.Drawing.Point(4, 22);
            this.HexTab.Name = "HexTab";
            this.HexTab.Padding = new System.Windows.Forms.Padding(3);
            this.HexTab.Size = new System.Drawing.Size(329, 216);
            this.HexTab.TabIndex = 4;
            this.HexTab.Text = "Hex";
            this.HexTab.UseVisualStyleBackColor = true;
            // 
            // HexTextBox
            // 
            this.HexTextBox.DetectUrls = false;
            this.HexTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HexTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HexTextBox.Location = new System.Drawing.Point(3, 3);
            this.HexTextBox.Name = "HexTextBox";
            this.HexTextBox.ReadOnly = true;
            this.HexTextBox.Size = new System.Drawing.Size(323, 210);
            this.HexTextBox.TabIndex = 8;
            this.HexTextBox.Text = "";
            // 
            // RefreshTimer
            // 
            this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // MemoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 306);
            this.Controls.Add(this.Tabs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FollowCombo);
            this.Controls.Add(this.BankLabel);
            this.Controls.Add(this.AddressLabel);
            this.Controls.Add(this.AddressNumeric);
            this.Controls.Add(this.BankCombo);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MemoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Memory";
            this.VisibleChanged += new System.EventHandler(this.MemoryForm_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.AddressNumeric)).EndInit();
            this.Tabs.ResumeLayout(false);
            this.HexTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox BankCombo;
		private System.Windows.Forms.NumericUpDown AddressNumeric;
		private System.Windows.Forms.Label AddressLabel;
        private System.Windows.Forms.Label BankLabel;
        private System.Windows.Forms.ComboBox FollowCombo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage HexTab;
        private System.Windows.Forms.Timer RefreshTimer;
		private System.Windows.Forms.RichTextBox HexTextBox;

	}
}