namespace Test
{
	partial class ExecutionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExecutionForm));
            this.label2 = new System.Windows.Forms.Label();
            this.disassemblyListView = new System.Windows.Forms.ListView();
            this.colAddress = new System.Windows.Forms.ColumnHeader();
            this.colBytes = new System.Windows.Forms.ColumnHeader();
            this.colInstruction = new System.Windows.Forms.ColumnHeader();
            this.RefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.RunButton = new System.Windows.Forms.ToolStripButton();
            this.RunToSelectedButton = new System.Windows.Forms.ToolStripButton();
            this.StepButton = new System.Windows.Forms.ToolStripButton();
            this.BreakpointButton = new System.Windows.Forms.ToolStripButton();
            this.StepOverButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(214, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 9;
            // 
            // disassemblyListView
            // 
            this.disassemblyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAddress,
            this.colBytes,
            this.colInstruction});
            this.disassemblyListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.disassemblyListView.FullRowSelect = true;
            this.disassemblyListView.GridLines = true;
            this.disassemblyListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.disassemblyListView.HideSelection = false;
            this.disassemblyListView.Location = new System.Drawing.Point(0, 0);
            this.disassemblyListView.MultiSelect = false;
            this.disassemblyListView.Name = "disassemblyListView";
            this.disassemblyListView.Size = new System.Drawing.Size(333, 291);
            this.disassemblyListView.TabIndex = 0;
            this.disassemblyListView.UseCompatibleStateImageBehavior = false;
            this.disassemblyListView.View = System.Windows.Forms.View.Details;
            this.disassemblyListView.DoubleClick += new System.EventHandler(this.disassemblyListView_DoubleClick);
            // 
            // colAddress
            // 
            this.colAddress.Text = "Address";
            // 
            // colBytes
            // 
            this.colBytes.Text = "Bytes";
            this.colBytes.Width = 120;
            // 
            // colInstruction
            // 
            this.colInstruction.Text = "Instruction";
            this.colInstruction.Width = 120;
            // 
            // RefreshTimer
            // 
            this.RefreshTimer.Tick += new System.EventHandler(this.RefreshTimer_Tick);
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.disassemblyListView);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(333, 291);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(333, 316);
            this.toolStripContainer1.TabIndex = 10;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RunButton,
            this.RunToSelectedButton,
            this.StepButton,
            this.StepOverButton,
            this.BreakpointButton});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(156, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // RunButton
            // 
            this.RunButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RunButton.Image = ((System.Drawing.Image)(resources.GetObject("RunButton.Image")));
            this.RunButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(23, 22);
            this.RunButton.Text = "Run";
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // RunToSelectedButton
            // 
            this.RunToSelectedButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RunToSelectedButton.Image = ((System.Drawing.Image)(resources.GetObject("RunToSelectedButton.Image")));
            this.RunToSelectedButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RunToSelectedButton.Name = "RunToSelectedButton";
            this.RunToSelectedButton.Size = new System.Drawing.Size(23, 22);
            this.RunToSelectedButton.Text = "Run to selected";
            this.RunToSelectedButton.Click += new System.EventHandler(this.RunToSelectedButton_Click);
            // 
            // StepButton
            // 
            this.StepButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StepButton.Image = ((System.Drawing.Image)(resources.GetObject("StepButton.Image")));
            this.StepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StepButton.Name = "StepButton";
            this.StepButton.Size = new System.Drawing.Size(23, 22);
            this.StepButton.Text = "Step";
            this.StepButton.Click += new System.EventHandler(this.StepButton_Click);
            // 
            // BreakpointButton
            // 
            this.BreakpointButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BreakpointButton.Image = ((System.Drawing.Image)(resources.GetObject("BreakpointButton.Image")));
            this.BreakpointButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BreakpointButton.Name = "BreakpointButton";
            this.BreakpointButton.Size = new System.Drawing.Size(23, 22);
            this.BreakpointButton.Text = "Toggle breakpoint";
            this.BreakpointButton.Click += new System.EventHandler(this.BreakpointButton_Click);
            // 
            // StepOverButton
            // 
            this.StepOverButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.StepOverButton.Image = ((System.Drawing.Image)(resources.GetObject("StepOverButton.Image")));
            this.StepOverButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.StepOverButton.Name = "StepOverButton";
            this.StepOverButton.Size = new System.Drawing.Size(23, 22);
            this.StepOverButton.Text = "Step over";
            this.StepOverButton.Click += new System.EventHandler(this.StepOverButton_Click);
            // 
            // ExecutionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 316);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ExecutionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Execution";
            this.VisibleChanged += new System.EventHandler(this.MemoryForm_VisibleChanged);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Timer RefreshTimer;
        private System.Windows.Forms.ListView disassemblyListView;
        private System.Windows.Forms.ColumnHeader colAddress;
        private System.Windows.Forms.ColumnHeader colInstruction;
        private System.Windows.Forms.ColumnHeader colBytes;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton RunButton;
		private System.Windows.Forms.ToolStripButton RunToSelectedButton;
		private System.Windows.Forms.ToolStripButton StepButton;
		private System.Windows.Forms.ToolStripButton BreakpointButton;
        private System.Windows.Forms.ToolStripButton StepOverButton;

	}
}