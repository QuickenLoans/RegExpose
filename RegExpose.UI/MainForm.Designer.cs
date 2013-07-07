namespace RegExpose.UI
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.compileMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ignoreCaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.multiLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCompile = new WindowsFormsToolkit.Controls.SplitButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnParse = new WindowsFormsToolkit.Controls.SplitButton();
            this.parseMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtInput = new RegExpose.UI.RegExposeRichTextBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tvRegex = new System.Windows.Forms.TreeView();
            this.lvMessages = new RegExpose.UI.HighlightableListView();
            this.columnHeaderIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderNodeType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPattern = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblMessage = new RegExpose.UI.RegExposeRichTextBox();
            this.txtPattern = new RegExpose.UI.RegExposeRichTextBox();
            this.compileMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // compileMenuStrip
            // 
            this.compileMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ignoreCaseToolStripMenuItem,
            this.singleLineToolStripMenuItem,
            this.multiLineToolStripMenuItem});
            this.compileMenuStrip.Name = "contextMenuStrip";
            this.compileMenuStrip.Size = new System.Drawing.Size(137, 70);
            // 
            // ignoreCaseToolStripMenuItem
            // 
            this.ignoreCaseToolStripMenuItem.CheckOnClick = true;
            this.ignoreCaseToolStripMenuItem.Name = "ignoreCaseToolStripMenuItem";
            this.ignoreCaseToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.ignoreCaseToolStripMenuItem.Text = "Ignore Case";
            // 
            // singleLineToolStripMenuItem
            // 
            this.singleLineToolStripMenuItem.CheckOnClick = true;
            this.singleLineToolStripMenuItem.Name = "singleLineToolStripMenuItem";
            this.singleLineToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.singleLineToolStripMenuItem.Text = "Single-Line";
            // 
            // multiLineToolStripMenuItem
            // 
            this.multiLineToolStripMenuItem.CheckOnClick = true;
            this.multiLineToolStripMenuItem.Name = "multiLineToolStripMenuItem";
            this.multiLineToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.multiLineToolStripMenuItem.Text = "Multi-Line";
            // 
            // btnCompile
            // 
            this.btnCompile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompile.AutoSize = true;
            this.btnCompile.ContextMenuStrip = this.compileMenuStrip;
            this.btnCompile.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.btnCompile.Location = new System.Drawing.Point(856, 11);
            this.btnCompile.Margin = new System.Windows.Forms.Padding(2);
            this.btnCompile.Name = "btnCompile";
            this.btnCompile.Size = new System.Drawing.Size(141, 41);
            this.btnCompile.TabIndex = 1;
            this.btnCompile.Text = "Compile";
            this.btnCompile.UseVisualStyleBackColor = true;
            this.btnCompile.Click += new System.EventHandler(this.BtnCompileOnClick);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnParse);
            this.splitContainer2.Panel1.Controls.Add(this.txtInput);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(986, 597);
            this.splitContainer2.SplitterDistance = 64;
            this.splitContainer2.TabIndex = 6;
            this.splitContainer2.TabStop = false;
            // 
            // btnParse
            // 
            this.btnParse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParse.AutoSize = true;
            this.btnParse.ContextMenuStrip = this.parseMenuStrip;
            this.btnParse.Enabled = false;
            this.btnParse.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F);
            this.btnParse.Location = new System.Drawing.Point(845, 0);
            this.btnParse.Margin = new System.Windows.Forms.Padding(2);
            this.btnParse.Name = "btnParse";
            this.btnParse.Size = new System.Drawing.Size(141, 41);
            this.btnParse.TabIndex = 1;
            this.btnParse.Text = "Parse";
            this.btnParse.UseVisualStyleBackColor = true;
            this.btnParse.Click += new System.EventHandler(this.BtnParseOnClick);
            // 
            // parseMenuStrip
            // 
            this.parseMenuStrip.Name = "contextMenuStrip";
            this.parseMenuStrip.Size = new System.Drawing.Size(61, 4);
            // 
            // txtInput
            // 
            this.txtInput.AcceptsTab = true;
            this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInput.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInput.Location = new System.Drawing.Point(0, 0);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(841, 64);
            this.txtInput.TabIndex = 0;
            this.txtInput.Text = "";
            this.txtInput.TextChanged += new System.EventHandler(this.TxtInputOnTextChanged);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tvRegex);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.lvMessages);
            this.splitContainer3.Size = new System.Drawing.Size(986, 529);
            this.splitContainer3.SplitterDistance = 360;
            this.splitContainer3.TabIndex = 0;
            this.splitContainer3.TabStop = false;
            // 
            // tvRegex
            // 
            this.tvRegex.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tvRegex.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvRegex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvRegex.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvRegex.Location = new System.Drawing.Point(0, 0);
            this.tvRegex.Name = "tvRegex";
            this.tvRegex.Size = new System.Drawing.Size(360, 529);
            this.tvRegex.TabIndex = 0;
            this.tvRegex.TabStop = false;
            this.tvRegex.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.TvRegexOnBeforeSelect);
            // 
            // lvMessages
            // 
            this.lvMessages.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvMessages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lvMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderIndex,
            this.columnHeaderType,
            this.columnHeaderNodeType,
            this.columnHeaderPattern,
            this.columnHeaderMessage});
            this.lvMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMessages.Font = new System.Drawing.Font("Consolas", 12F);
            this.lvMessages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvMessages.HideSelection = false;
            this.lvMessages.Location = new System.Drawing.Point(0, 0);
            this.lvMessages.MultiSelect = false;
            this.lvMessages.Name = "lvMessages";
            this.lvMessages.OwnerDraw = true;
            this.lvMessages.Size = new System.Drawing.Size(622, 529);
            this.lvMessages.TabIndex = 0;
            this.lvMessages.TabStop = false;
            this.lvMessages.UseCompatibleStateImageBehavior = false;
            this.lvMessages.View = System.Windows.Forms.View.Details;
            this.lvMessages.SelectedIndexChanged += new System.EventHandler(this.LvMessagesOnSelectedIndexChanged);
            // 
            // columnHeaderIndex
            // 
            this.columnHeaderIndex.Text = "Index";
            this.columnHeaderIndex.Width = 25;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Step";
            this.columnHeaderType.Width = 25;
            // 
            // columnHeaderNodeType
            // 
            this.columnHeaderNodeType.Text = "Regex Node";
            this.columnHeaderNodeType.Width = 25;
            // 
            // columnHeaderPattern
            // 
            this.columnHeaderPattern.Text = "Pattern";
            this.columnHeaderPattern.Width = 25;
            // 
            // columnHeaderMessage
            // 
            this.columnHeaderMessage.Text = "Message";
            this.columnHeaderMessage.Width = 25;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(11, 54);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblMessage);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(986, 665);
            this.splitContainer1.SplitterDistance = 64;
            this.splitContainer1.TabIndex = 7;
            this.splitContainer1.TabStop = false;
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.SystemColors.ControlLight;
            this.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.ReadOnly = true;
            this.lblMessage.Size = new System.Drawing.Size(986, 64);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.TabStop = false;
            this.lblMessage.Text = "";
            // 
            // txtPattern
            // 
            this.txtPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPattern.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPattern.Font = new System.Drawing.Font("Consolas", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPattern.Location = new System.Drawing.Point(11, 11);
            this.txtPattern.Margin = new System.Windows.Forms.Padding(2);
            this.txtPattern.Multiline = false;
            this.txtPattern.Name = "txtPattern";
            this.txtPattern.Size = new System.Drawing.Size(841, 39);
            this.txtPattern.TabIndex = 0;
            this.txtPattern.Text = "";
            this.txtPattern.TextChanged += new System.EventHandler(this.TxtPatternOnTextChanged);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnCompile;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnCompile);
            this.Controls.Add(this.txtPattern);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Regular Expressions - Now You Have Two Problems!";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.compileMenuStrip.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RegExpose.UI.RegExposeRichTextBox txtPattern;
        private WindowsFormsToolkit.Controls.SplitButton btnCompile;
        private System.Windows.Forms.ContextMenuStrip compileMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem ignoreCaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem singleLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem multiLineToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private RegExpose.UI.RegExposeRichTextBox txtInput;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TreeView tvRegex;
        private RegExpose.UI.HighlightableListView lvMessages;
        private System.Windows.Forms.ColumnHeader columnHeaderIndex;
        private System.Windows.Forms.ColumnHeader columnHeaderNodeType;
        private System.Windows.Forms.ColumnHeader columnHeaderPattern;
        private System.Windows.Forms.ColumnHeader columnHeaderMessage;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private RegExpose.UI.RegExposeRichTextBox lblMessage;
        private WindowsFormsToolkit.Controls.SplitButton btnParse;
        private System.Windows.Forms.ContextMenuStrip parseMenuStrip;
    }
}

