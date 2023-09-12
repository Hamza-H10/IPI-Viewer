using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace InclinoView
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    public partial class Form1 : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components is not null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.SplitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstBoreholes = new System.Windows.Forms.ListBox();
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbBack = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbImport = new System.Windows.Forms.ToolStripButton();
            this.tbDelete = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbViewGraph = new System.Windows.Forms.ToolStripButton();
            this.tbReport = new System.Windows.Forms.ToolStripButton();
            this.tbBaseFile = new System.Windows.Forms.ToolStripButton();
            this.SplitContainer2 = new System.Windows.Forms.SplitContainer();
            this.DataGridView1 = new System.Windows.Forms.DataGridView();
            this.CartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            this.lblDepth = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.lblSiteName = new System.Windows.Forms.Label();
            this.lblBoreholeNumber = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.ToolStrip2 = new System.Windows.Forms.ToolStrip();
            this.PrintToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.tbZoom = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tbAxisX = new System.Windows.Forms.ToolStripButton();
            this.tbAxisY = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tbGraphType = new System.Windows.Forms.ToolStripComboBox();
            this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.PrintDocument1 = new System.Drawing.Printing.PrintDocument();
            this.PrintPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.PrintDialog1 = new System.Windows.Forms.PrintDialog();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).BeginInit();
            this.SplitContainer1.Panel1.SuspendLayout();
            this.SplitContainer1.Panel2.SuspendLayout();
            this.SplitContainer1.SuspendLayout();
            this.ToolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).BeginInit();
            this.SplitContainer2.Panel1.SuspendLayout();
            this.SplitContainer2.Panel2.SuspendLayout();
            this.SplitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).BeginInit();
            this.ToolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SplitContainer1
            // 
            this.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer1.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer1.Name = "SplitContainer1";
            // 
            // SplitContainer1.Panel1
            // 
            this.SplitContainer1.Panel1.Controls.Add(this.lstBoreholes);
            this.SplitContainer1.Panel1.Controls.Add(this.ToolStrip1);
            // 
            // SplitContainer1.Panel2
            // 
            this.SplitContainer1.Panel2.Controls.Add(this.SplitContainer2);
            this.SplitContainer1.Panel2.Controls.Add(this.ToolStrip2);
            this.SplitContainer1.Size = new System.Drawing.Size(1084, 450);
            this.SplitContainer1.SplitterDistance = 358;
            this.SplitContainer1.TabIndex = 0;
            // 
            // lstBoreholes
            // 
            this.lstBoreholes.BackColor = System.Drawing.SystemColors.Window;
            this.lstBoreholes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstBoreholes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstBoreholes.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBoreholes.ForeColor = System.Drawing.Color.Teal;
            this.lstBoreholes.FormattingEnabled = true;
            this.lstBoreholes.ItemHeight = 16;
            this.lstBoreholes.Location = new System.Drawing.Point(0, 54);
            this.lstBoreholes.Name = "lstBoreholes";
            this.lstBoreholes.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstBoreholes.Size = new System.Drawing.Size(358, 396);
            this.lstBoreholes.TabIndex = 1;
            this.lstBoreholes.Click += new System.EventHandler(this.lstBoreholes_Click);
            this.lstBoreholes.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.ListBox1_DrawItem);
            this.lstBoreholes.DoubleClick += new System.EventHandler(this.lstBoreholes_DoubleClick);
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ToolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbBack,
            this.ToolStripSeparator1,
            this.tbImport,
            this.tbDelete,
            this.ToolStripSeparator2,
            this.tbViewGraph,
            this.tbReport,
            this.tbBaseFile});
            this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Size = new System.Drawing.Size(358, 54);
            this.ToolStrip1.Stretch = true;
            this.ToolStrip1.TabIndex = 0;
            this.ToolStrip1.Text = "ToolStrip1";
            // 
            // tbBack
            // 
            this.tbBack.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBack.Image = ((System.Drawing.Image)(resources.GetObject("tbBack.Image")));
            this.tbBack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbBack.Name = "tbBack";
            this.tbBack.Size = new System.Drawing.Size(37, 51);
            this.tbBack.Text = "Back";
            this.tbBack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbBack.Click += new System.EventHandler(this.tbBack_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 54);
            // 
            // tbImport
            // 
            this.tbImport.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbImport.Image = ((System.Drawing.Image)(resources.GetObject("tbImport.Image")));
            this.tbImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbImport.Name = "tbImport";
            this.tbImport.Size = new System.Drawing.Size(71, 51);
            this.tbImport.Text = "Import CSV";
            this.tbImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbImport.Click += new System.EventHandler(this.tbImport_Click);
            // 
            // tbDelete
            // 
            this.tbDelete.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tbDelete.Image")));
            this.tbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbDelete.Name = "tbDelete";
            this.tbDelete.Size = new System.Drawing.Size(45, 51);
            this.tbDelete.Text = "Delete";
            this.tbDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbDelete.Click += new System.EventHandler(this.tbDelete_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 54);
            // 
            // tbViewGraph
            // 
            this.tbViewGraph.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbViewGraph.Image = ((System.Drawing.Image)(resources.GetObject("tbViewGraph.Image")));
            this.tbViewGraph.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbViewGraph.Name = "tbViewGraph";
            this.tbViewGraph.Size = new System.Drawing.Size(45, 51);
            this.tbViewGraph.Text = "Graph";
            this.tbViewGraph.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbViewGraph.ToolTipText = "View Graph";
            this.tbViewGraph.Click += new System.EventHandler(this.tbViewGraph_Click);
            // 
            // tbReport
            // 
            this.tbReport.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbReport.Image = ((System.Drawing.Image)(resources.GetObject("tbReport.Image")));
            this.tbReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbReport.Name = "tbReport";
            this.tbReport.Size = new System.Drawing.Size(47, 51);
            this.tbReport.Text = "Report";
            this.tbReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbReport.Click += new System.EventHandler(this.tbReport_Click);
            // 
            // tbBaseFile
            // 
            this.tbBaseFile.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBaseFile.Image = ((System.Drawing.Image)(resources.GetObject("tbBaseFile.Image")));
            this.tbBaseFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbBaseFile.Name = "tbBaseFile";
            this.tbBaseFile.Size = new System.Drawing.Size(60, 51);
            this.tbBaseFile.Text = "Base File";
            this.tbBaseFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbBaseFile.Click += new System.EventHandler(this.tbBaseFile_Click);
            // 
            // SplitContainer2
            // 
            this.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.SplitContainer2.Location = new System.Drawing.Point(0, 54);
            this.SplitContainer2.Name = "SplitContainer2";
            this.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SplitContainer2.Panel1
            // 
            this.SplitContainer2.Panel1.Controls.Add(this.DataGridView1);
            this.SplitContainer2.Panel1.Controls.Add(this.CartesianChart1);
            // 
            // SplitContainer2.Panel2
            // 
            this.SplitContainer2.Panel2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.SplitContainer2.Panel2.Controls.Add(this.lblDepth);
            this.SplitContainer2.Panel2.Controls.Add(this.lblLocation);
            this.SplitContainer2.Panel2.Controls.Add(this.lblSiteName);
            this.SplitContainer2.Panel2.Controls.Add(this.lblBoreholeNumber);
            this.SplitContainer2.Panel2.Controls.Add(this.Label4);
            this.SplitContainer2.Panel2.Controls.Add(this.Label5);
            this.SplitContainer2.Panel2.Controls.Add(this.Label6);
            this.SplitContainer2.Panel2.Controls.Add(this.Label3);
            this.SplitContainer2.Panel2.Controls.Add(this.Label2);
            this.SplitContainer2.Panel2.Controls.Add(this.Label1);
            this.SplitContainer2.Size = new System.Drawing.Size(722, 396);
            this.SplitContainer2.SplitterDistance = 303;
            this.SplitContainer2.TabIndex = 1;
            // 
            // DataGridView1
            // 
            this.DataGridView1.AllowUserToAddRows = false;
            this.DataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.DataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridView1.BackgroundColor = System.Drawing.Color.FloralWhite;
            this.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGridView1.Location = new System.Drawing.Point(0, 0);
            this.DataGridView1.Name = "DataGridView1";
            this.DataGridView1.ReadOnly = true;
            this.DataGridView1.RowHeadersWidth = 51;
            this.DataGridView1.Size = new System.Drawing.Size(722, 303);
            this.DataGridView1.TabIndex = 3;
            // 
            // CartesianChart1
            // 
            this.CartesianChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CartesianChart1.Location = new System.Drawing.Point(0, 0);
            this.CartesianChart1.Name = "CartesianChart1";
            this.CartesianChart1.Size = new System.Drawing.Size(722, 303);
            this.CartesianChart1.TabIndex = 2;
            this.CartesianChart1.Text = "CartesianChart1";
            // 
            // lblDepth
            // 
            this.lblDepth.AutoSize = true;
            this.lblDepth.BackColor = System.Drawing.Color.White;
            this.lblDepth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepth.Location = new System.Drawing.Point(127, 12);
            this.lblDepth.Name = "lblDepth";
            this.lblDepth.Size = new System.Drawing.Size(45, 13);
            this.lblDepth.TabIndex = 9;
            this.lblDepth.Text = "Label7";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.BackColor = System.Drawing.Color.White;
            this.lblLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.Location = new System.Drawing.Point(7, 56);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(45, 13);
            this.lblLocation.TabIndex = 8;
            this.lblLocation.Text = "Label9";
            // 
            // lblSiteName
            // 
            this.lblSiteName.AutoSize = true;
            this.lblSiteName.BackColor = System.Drawing.Color.White;
            this.lblSiteName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSiteName.Location = new System.Drawing.Point(7, 34);
            this.lblSiteName.Name = "lblSiteName";
            this.lblSiteName.Size = new System.Drawing.Size(45, 13);
            this.lblSiteName.TabIndex = 7;
            this.lblSiteName.Text = "Label8";
            // 
            // lblBoreholeNumber
            // 
            this.lblBoreholeNumber.AutoSize = true;
            this.lblBoreholeNumber.BackColor = System.Drawing.Color.White;
            this.lblBoreholeNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBoreholeNumber.Location = new System.Drawing.Point(7, 12);
            this.lblBoreholeNumber.Name = "lblBoreholeNumber";
            this.lblBoreholeNumber.Size = new System.Drawing.Size(45, 13);
            this.lblBoreholeNumber.TabIndex = 6;
            this.lblBoreholeNumber.Text = "Label7";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.Label4.Location = new System.Drawing.Point(413, 12);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(54, 16);
            this.Label4.TabIndex = 5;
            this.Label4.Text = "Label4";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.ForeColor = System.Drawing.Color.Navy;
            this.Label5.Location = new System.Drawing.Point(413, 34);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(54, 16);
            this.Label5.TabIndex = 4;
            this.Label5.Text = "Label5";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.ForeColor = System.Drawing.Color.Maroon;
            this.Label6.Location = new System.Drawing.Point(413, 56);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(54, 16);
            this.Label6.TabIndex = 3;
            this.Label6.Text = "Label6";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.ForeColor = System.Drawing.Color.Goldenrod;
            this.Label3.Location = new System.Drawing.Point(253, 56);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(54, 16);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "Label3";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.ForeColor = System.Drawing.Color.OrangeRed;
            this.Label2.Location = new System.Drawing.Point(253, 34);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(54, 16);
            this.Label2.TabIndex = 1;
            this.Label2.Text = "Label2";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.Label1.Location = new System.Drawing.Point(253, 12);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(54, 16);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Label1";
            // 
            // ToolStrip2
            // 
            this.ToolStrip2.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ToolStrip2.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.ToolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PrintToolStripButton,
            this.tbZoom,
            this.toolStripSeparator,
            this.tbAxisX,
            this.tbAxisY,
            this.ToolStripSeparator3,
            this.tbGraphType});
            this.ToolStrip2.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip2.Name = "ToolStrip2";
            this.ToolStrip2.Size = new System.Drawing.Size(722, 54);
            this.ToolStrip2.TabIndex = 0;
            this.ToolStrip2.Text = "ToolStrip2";
            // 
            // PrintToolStripButton
            // 
            this.PrintToolStripButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.PrintToolStripButton.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrintToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("PrintToolStripButton.Image")));
            this.PrintToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PrintToolStripButton.Name = "PrintToolStripButton";
            this.PrintToolStripButton.Size = new System.Drawing.Size(38, 51);
            this.PrintToolStripButton.Text = "&Print";
            this.PrintToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.PrintToolStripButton.Click += new System.EventHandler(this.PrintToolStripButton_Click);
            // 
            // tbZoom
            // 
            this.tbZoom.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tbZoom.CheckOnClick = true;
            this.tbZoom.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbZoom.Image = ((System.Drawing.Image)(resources.GetObject("tbZoom.Image")));
            this.tbZoom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbZoom.Name = "tbZoom";
            this.tbZoom.Size = new System.Drawing.Size(44, 51);
            this.tbZoom.Text = "Zoom ";
            this.tbZoom.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbZoom.Click += new System.EventHandler(this.tbZoom_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 54);
            // 
            // tbAxisX
            // 
            this.tbAxisX.Checked = true;
            this.tbAxisX.CheckOnClick = true;
            this.tbAxisX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tbAxisX.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAxisX.Image = ((System.Drawing.Image)(resources.GetObject("tbAxisX.Image")));
            this.tbAxisX.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbAxisX.Name = "tbAxisX";
            this.tbAxisX.Size = new System.Drawing.Size(44, 51);
            this.tbAxisX.Text = "Axis A";
            this.tbAxisX.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbAxisX.Click += new System.EventHandler(this.tbAxisX_Click);
            // 
            // tbAxisY
            // 
            this.tbAxisY.CheckOnClick = true;
            this.tbAxisY.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAxisY.Image = ((System.Drawing.Image)(resources.GetObject("tbAxisY.Image")));
            this.tbAxisY.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbAxisY.Name = "tbAxisY";
            this.tbAxisY.Size = new System.Drawing.Size(43, 51);
            this.tbAxisY.Text = "Axis B";
            this.tbAxisY.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tbAxisY.Click += new System.EventHandler(this.tbAxisY_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(6, 54);
            // 
            // tbGraphType
            // 
            this.tbGraphType.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.tbGraphType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tbGraphType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbGraphType.Items.AddRange(new object[] {
            "Mean",
            "Absolute",
            "Deviation"});
            this.tbGraphType.Name = "tbGraphType";
            this.tbGraphType.Size = new System.Drawing.Size(121, 54);
            this.tbGraphType.SelectedIndexChanged += new System.EventHandler(this.TbGraphType_SelChange);
            // 
            // OpenFileDialog1
            // 
            this.OpenFileDialog1.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            this.OpenFileDialog1.Multiselect = true;
            this.OpenFileDialog1.Title = "Import CSV";
            // 
            // PrintDocument1
            // 
            this.PrintDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintForm1_PrintPage);
            // 
            // PrintPreviewDialog1
            // 
            this.PrintPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.PrintPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.PrintPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.PrintPreviewDialog1.Enabled = true;
            this.PrintPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("PrintPreviewDialog1.Icon")));
            this.PrintPreviewDialog1.Name = "PrintPreviewDialog1";
            this.PrintPreviewDialog1.Visible = false;
            // 
            // PrintDialog1
            // 
            this.PrintDialog1.Document = this.PrintDocument1;
            this.PrintDialog1.UseEXDialog = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1084, 450);
            this.Controls.Add(this.SplitContainer1);
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "IPI View";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SplitContainer1.Panel1.ResumeLayout(false);
            this.SplitContainer1.Panel1.PerformLayout();
            this.SplitContainer1.Panel2.ResumeLayout(false);
            this.SplitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer1)).EndInit();
            this.SplitContainer1.ResumeLayout(false);
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.SplitContainer2.Panel1.ResumeLayout(false);
            this.SplitContainer2.Panel2.ResumeLayout(false);
            this.SplitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer2)).EndInit();
            this.SplitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView1)).EndInit();
            this.ToolStrip2.ResumeLayout(false);
            this.ToolStrip2.PerformLayout();
            this.ResumeLayout(false);

        }

        internal SplitContainer SplitContainer1;
        internal ListBox lstBoreholes;
        internal ToolStrip ToolStrip1;
        internal ToolStripButton tbBack;
        internal ToolStripButton tbImport;
        internal ToolStrip ToolStrip2;
        internal ToolStripButton PrintToolStripButton;
        internal ToolStripSeparator toolStripSeparator;
        internal OpenFileDialog OpenFileDialog1;
        internal ToolStripButton tbAxisX;
        internal ToolStripButton tbAxisY;
        internal ToolStripButton tbViewGraph;
        internal SplitContainer SplitContainer2;
        internal LiveCharts.WinForms.CartesianChart CartesianChart1;
        internal Label Label4;
        internal Label Label5;
        internal Label Label6;
        internal Label Label3;
        internal Label Label2;
        internal Label Label1;
        internal ToolStripSeparator ToolStripSeparator1;
        internal ToolStripButton tbDelete;
        internal ToolStripSeparator ToolStripSeparator2;
        internal ToolStripButton tbBaseFile;
        internal ToolStripComboBox tbGraphType;
        internal ToolStripSeparator ToolStripSeparator3;
        internal ToolStripButton tbZoom;
        internal Label lblLocation;
        internal Label lblSiteName;
        internal Label lblBoreholeNumber;
        internal Label lblDepth;
        internal ToolStripButton tbReport;
        internal System.Drawing.Printing.PrintDocument PrintDocument1;
        internal PrintPreviewDialog PrintPreviewDialog1;
        internal DataGridView DataGridView1;
        internal PrintDialog PrintDialog1;
    }
}