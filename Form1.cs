using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace InclinoView
{   /// <summary>
    /// This class represents the main form of the application.
    /// </summary>
    /// <author>Hamza</author>
    /// <date>2023-09-10</date>
    public partial class Form1
    {
        private List<GlobalCode.BoreHole> listBH;
        private short bhIndex = -1;
        private short boreHoleSelected = 0;
        private short _axisValue = 0;
        private string bsTextPrintData;
        private Font printFont;
        /// <summary>
        /// This method loads the main form of the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public Form1()
        
        {
            InitializeComponent();
        }

        private void ReloadList()
        {

            lstBoreholes.Items.Clear();
            if (boreHoleSelected == 0)
            {
                listBH = GlobalCode.GetBoreholes();

                foreach (var bitem in listBH)
                    lstBoreholes.Items.Add("[" + bitem.Id.ToString("D2") + "] " + bitem.SiteName + " - " + bitem.Location);
                lstBoreholes.SelectionMode = SelectionMode.One;
                bool argenb = false;
                ToolBarEnable(ref argenb);
            }
            else
            {
                // get directory listing
                var di = new System.IO.DirectoryInfo(GlobalCode.GetBoreholeDirectory(ref boreHoleSelected));
                System.IO.FileInfo[] aryFi = di.GetFiles("*.csv");

                foreach (var fi in aryFi)
                    lstBoreholes.Items.Add(fi.Name);

                lstBoreholes.SelectionMode = SelectionMode.MultiSimple;

                bool argenb1 = true;
                ToolBarEnable(ref argenb1);
            }
            ResetLabels();
            CartesianChart1.Visible = false;
            DataGridView1.Visible = false;
            ToolStrip2.Enabled = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Label1.ForeColor = System.Drawing.Color.FromArgb(33, 149, 242);
            Label2.ForeColor = System.Drawing.Color.FromArgb(243, 67, 54);
            Label3.ForeColor = System.Drawing.Color.FromArgb(254, 192, 7);
            Label4.ForeColor = System.Drawing.Color.FromArgb(96, 125, 138);
            Label5.ForeColor = System.Drawing.Color.FromArgb(0, 187, 211);
            GlobalCode.OpenDatabase();
            // _DeleteAllBoreholes() ' temporary delete all
            tbGraphType.SelectedIndex = 0;

            ReloadList();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalCode.CloseDatabase();
        }

        private void tbImport_Click(object sender, EventArgs e)
        {
            short cnt = 0;
            short cntError = 0;
            short cntRepeat = 0;
            string msgString = "Import Summary:" + Constants.vbCrLf;//what is vbCrLf?
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {

                foreach (string strFileName in OpenFileDialog1.FileNames)
                {

                    string tempFileName = strFileName; // Create a temporary variable
                    Console.WriteLine("tempFileName: "+ tempFileName);

                    string strFileNew = strFileName.Split('\\').Last();//opening a string file dialogue here when selected the already imported file
                    Console.WriteLine("strFileNew: "+ strFileNew);

                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(strFileNew.Split('.').Last().ToLower(), "csv", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    {
                        string[][] strData = GlobalCode.ReadCSVFile(ref tempFileName);//check the dimensions of strData to ensure that the it has enough rows and columns to support this indexing.
                    /*----------------------------------------------------------------------------*/
                      /*  //this is just to check the data of array on consol.
                        if (strData != null && strData.Length > 0) { 
                            // Iterate through rows
                            for (int i = 0; i < strData.Length; i++)
                            {
                                // Access the current row
                                string[] row = strData[i];

                                // Iterate through columns within the row
                                for (int j = 0; j < row.Length; j++)
                                {
                                    // Access the current cell's value
                                    string cellValue = row[j];

                                    // Do something with cellValue, such as printing it
                                    Console.WriteLine($"Row {i}, Column {j}: {cellValue}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("No data found in strData.");
                        }*/
                    /*------------------------------------------------------------------------------------*/
                        if (strData.Length < 5)
                        {
                            cntError = (short)(cntError + 1);
                        }
                        else
                        {
                            // Catch 4 poarameters for the new borehole
                            short borehole_num;
                            float depth;
                            string strDirName;

                            borehole_num = short.Parse(strData[0][1]);
                            strDirName = GlobalCode.GetBoreholeDirectory(ref borehole_num);//debugger is skipping this line// exception is being thrown here please check
                            strFileNew = strDirName + @"\" + strFileNew;
                            if (System.IO.File.Exists(strFileNew))
                            {
                                cntRepeat = (short)(cntRepeat + 1);
                            }
                            else
                            {
                                if (!System.IO.Directory.Exists(strDirName))
                                {
                                    System.IO.Directory.CreateDirectory(strDirName);
                                }
                                FileSystem.FileCopy(strFileName, strFileNew);
                                Console.WriteLine(strData[1][0]);
                                // depth = float.Parse(strData[4][0]);
                                Console.WriteLine("strData[10][2]: " + strData[10][2]);
                                depth = float.Parse(strData[10][2]);//check here if the value comes or not
                                
                                var bh = new GlobalCode.BoreHole() { Id = borehole_num, SiteName = strData[1][1], Location = strData[2][1], Depth = depth, BaseFile = "" };
                                if (!GlobalCode.AddBorehole(ref bh))
                                {
                                    GlobalCode.UpdateBorehole(ref bh);
                                }
                                ReloadList();
                                cnt = (short)(cnt + 1);
                            }

                        }
                    }
                }
                if (cnt > 0)
                    msgString += "You have added " + cnt + " CSV file(s) to the InclinoView successfully." + Constants.vbCrLf;
                if (cntError > 0)
                    msgString += cntError + " file(s) were found to be incorrect format." + Constants.vbCrLf;
                if (cntRepeat > 0)
                    msgString += cntRepeat + " file(s) were already imported into the application, hence ignored." + Constants.vbCrLf;

                Interaction.MsgBox(msgString, MsgBoxStyle.OkOnly | MsgBoxStyle.Information, "Import");
            }
        }

        private void tbBack_Click(object sender, EventArgs e)
        {
            if (boreHoleSelected > 0)
            {
                boreHoleSelected = 0;
                bhIndex = -1;
                ReloadList();
            }

        }

        private void lstBoreholes_DoubleClick(object sender, EventArgs e)
        {
            if (lstBoreholes.SelectedIndex < 0)
                return;
            if (boreHoleSelected == 0)
            {
                bhIndex = (short)lstBoreholes.SelectedIndex;
                boreHoleSelected = listBH[bhIndex].Id;
                ReloadList();
            }
            else
            {
                DataGridView1.Visible = false;
                CartesianChart1.Visible = false;
                DisplayReport();
                // lstboreholes.selecteditem is a CSV file
            }
        }


        private void DisplayReport(bool bnLoadText = true)
        {
            // Define variables to store data and calculations
            var ds = new DataTable(); // Create a DataTable to hold the report data
            var strBaseData = default(string[][]); // Store data from a base file
            var bnBaseFilePresent = default(bool); // Flag indicating if a base file is present
            short i;
            float ValA;
            float ValB;
            String DataTime;
            int Sensor;
            int Depth;
            /*float ValA;
            float ValB;
            float absValA = 0f; // Accumulated absolute values of A
            float absValB = 0f; // Accumulated absolute values of B
            float bsValA;
            float bsValB;
            float bsAbsValA = 0f; // Accumulated absolute values of A from the base file
            float bsAbsValB = 0f; // Accumulated absolute values of B from the base file
            float deviationA;
            float deviationB;*/

            // Reset labels and prepare for report generation

            if (listBH[bhIndex].BaseFile is null | string.IsNullOrEmpty(listBH[bhIndex].BaseFile))
            {
                bnBaseFilePresent = false; // No base file is present
                Label6.Text = "";
            }
            else
            {
                // Construct the path to the base file
                string strFile = GlobalCode.GetBoreholeDirectory(ref boreHoleSelected) + @"\" + listBH[bhIndex].BaseFile;

                if (System.IO.File.Exists(strFile))
                {
                    // Read data from the base file
                    strBaseData = GlobalCode.ReadCSVFile(ref strFile);
                    bnBaseFilePresent = true; // Base file is present
                    Label6.Text = "Base File : " + listBH[bhIndex].BaseFile.Split('.').First().Replace("_", ":");
                }
                else
                {
                    // Display a message if the base file does not exist
                    Interaction.MsgBox("Base file does not exist. It must have been deleted. Please select another file as base.", Constants.vbOKOnly | Constants.vbExclamation, "Graph");
                }
            }

            Label1.Text = lstBoreholes.SelectedItem.ToString().Split('.').First().Replace("_", ":");

            // Construct the path to the selected data file
            string argFileName = Conversions.ToString(Operators.ConcatenateObject(GlobalCode.GetBoreholeDirectory(ref boreHoleSelected) + @"\", lstBoreholes.SelectedItem));
            string[][] strData = GlobalCode.ReadCSVFile(ref argFileName);
            
            if (strData != null)
            {
                foreach (string[] row in strData)
                {
                    Console.WriteLine(string.Join(", ", row));
                }
            }
            else
            {
                Console.WriteLine("Failed to read CSV data.");
            }

            // Create columns in the DataTable to hold the report data
            ds.Columns.Add("DateTime", Type.GetType("System.Single"));
            ds.Columns.Add("Sensor", Type.GetType("System.Single"));
            ds.Columns.Add("Depth", Type.GetType("System.Single"));
            ds.Columns.Add("A", Type.GetType("System.Single"));
            ds.Columns.Add("B", Type.GetType("System.Single"));

            /* ds.Columns.Add("Mean A", Type.GetType("System.Single"));
             ds.Columns.Add("Mean B", Type.GetType("System.Single"));

             ds.Columns.Add("Absolute A", Type.GetType("System.Single"));
             ds.Columns.Add("Absolute B", Type.GetType("System.Single"));

             ds.Columns.Add("Deviation A", Type.GetType("System.Single"));
             ds.Columns.Add("Deviation B", Type.GetType("System.Single"));*/

            // Process data rows
            var loopTo = (short)(strData.Length - 1);
            for (i = 4; i <= loopTo; i++)
            {
                // Calculate values for A and B
                /*ValA = (float.Parse(strData[i][1]) - float.Parse(strData[i][2])) / 2f;
                ValB = (float.Parse(strData[i][3]) - float.Parse(strData[i][4])) / 2f;
                ValA = (float)Math.Round((double)ValA, 2);
                ValB = (float)Math.Round((double)ValB, 2);*/
                ValA = (float.Parse(strData[i][3]));
                ValB = (float.Parse(strData[i][4]));
                ValA = (float)Math.Round((double)ValA, 2);
                ValB = (float)Math.Round((double)ValB, 2);
                Console.WriteLine(ValA + " " + ValB);

                //my change remove this 
                ds.Rows.Add(new object[] {float.Parse(strData[i][3]), float.Parse(strData[i][4]), ValA, ValB });
                
                //float.Parse(strData[i][0]), float.Parse(strData[i][1]), float.Parse(strData[i][2]),
                // Accumulate absolute values for A and B
                /*absValA += ValA;
                absValB += ValB;
                absValA = (float)Math.Round((double)absValA, 2);
                absValB = (float)Math.Round((double)absValB, 2);*/

                /*if (bnBaseFilePresent)
                {
                    // Calculate values for A and B from the base file
                    bsValA = (float.Parse(strBaseData[i][1]) - float.Parse(strBaseData[i][2])) / 2f;
                    bsValB = (float.Parse(strBaseData[i][3]) - float.Parse(strBaseData[i][4])) / 2f;
                    bsValA = (float)Math.Round((double)bsValA, 2);
                    bsValB = (float)Math.Round((double)bsValB, 2);

                    // Accumulate absolute values for A and B from the base file
                    bsAbsValA += bsValA;
                    bsAbsValB += bsValB;
                    bsAbsValA = (float)Math.Round((double)bsAbsValA, 2);
                    bsAbsValB = (float)Math.Round((double)bsAbsValB, 2);

                    // Calculate deviations from the base file
                    deviationA = (float)Math.Round((double)(absValA - bsAbsValA), 2);
                    deviationB = (float)Math.Round((double)(absValB - bsAbsValB), 2);

                    // Add a row to the DataTable with all calculated values
                    ds.Rows.Add(new object[] { float.Parse(strData[i][0]), float.Parse(strData[i][1]), float.Parse(strData[i][2]), float.Parse(strData[i][3]), float.Parse(strData[i][4]), ValA, ValB, absValA, absValB, deviationA, deviationB });
                }
                else
                {
                    // Add a row to the DataTable with calculated values (without deviations)
                    ds.Rows.Add(new object[] { float.Parse(strData[i][0]), float.Parse(strData[i][1]), float.Parse(strData[i][2]), float.Parse(strData[i][3]), float.Parse(strData[i][4]), ValA, ValB, absValA, absValB });
                }*/
                

            }

            if (bnLoadText)
                {
                    // Prepare text data for loading (if needed)
                    int row;
                    string strItem;

                    var loopTo1 = (short)(ds.Columns.Count - 1);
                    Console.WriteLine(loopTo1);
                    for (i = 0; i <= loopTo1; i++)
                    {
                        if (i > 6)
                        {
                            bsTextPrintData += ds.Columns[i].ColumnName.PadLeft(12);
                            Console.WriteLine(bsTextPrintData);
                        }
                        else
                        {
                            bsTextPrintData += ds.Columns[i].ColumnName.PadLeft(8);
                            Console.WriteLine(bsTextPrintData);
                        }
                    }
                    bsTextPrintData += Constants.vbCrLf;
                    bsTextPrintData += "".PadRight(104, '=') + Constants.vbCrLf;

                    var loopTo2 = ds.Rows.Count - 1;
                    Console.WriteLine(loopTo2);
                    for (row = 0; row <= loopTo2; row++)
                    {
                        var loopTo3 = (short)(ds.Columns.Count - 1);
                        Console.WriteLine(loopTo3);
                        for (i = 0; i <= loopTo3; i++)
                        {
                            strItem = "";
                            if (ds.Rows[row][i] is not DBNull)//DBNull cannot be inherited
                            {
                                strItem = Strings.FormatNumber(ds.Rows[row][i], 2);
                                Console.WriteLine(strItem);
                            }
                            if (i > 6)
                            {
                                bsTextPrintData += "  " + strItem.PadLeft(8) + "  ";
                            }
                            else
                            {
                                bsTextPrintData += " " + strItem.PadLeft(6) + " ";
                            }
                        }
                        bsTextPrintData += Constants.vbCrLf;
                    Console.WriteLine(bsTextPrintData);
                    }
                }
                else
                {
                    // Display the report in a DataGridView (if not loading text)
                    DataGridView1.DataSource = ds;
                    DataGridView1.Visible = true;

                    var loopTo4 = (short)(DataGridView1.Columns.Count - 1);
                    Console.WriteLine(loopTo4);
                    for (i = 0; i <= loopTo4; i++)
                    {
                        if (i > 6)
                        {
                            DataGridView1.Columns[i].Width = 100;
                        }
                        else
                        {
                            DataGridView1.Columns[i].Width = 100;
                        }
                    }

                    // Configure DataGridView appearance
                    DataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11f, FontStyle.Bold | FontStyle.Italic);
                    DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                    // Change the background color of the entire DataGridView
                    DataGridView1.BackgroundColor = Color.LightGray;

                    // Change the background color of the rows
                    DataGridView1.RowsDefaultCellStyle.BackColor = Color.White;
                    DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

                    // Change the background color of selected cells
                    DataGridView1.DefaultCellStyle.SelectionBackColor = Color.Blue;
                    DataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;

                // Enable or disable ToolStrip buttons based on conditions
                if (!ToolStrip2.Enabled)
                        ToolStrip2.Enabled = true;

                //PrintToolStripButton.Enabled = True

                    tbAxisX.Enabled = false;
                    tbAxisY.Enabled = false;
                    tbZoom.Enabled = false;
                      tbGraphType.Enabled = false;
                }
            }
        
            private void DisplayGraph()
        {
            var cnt = default(short);
            double maxX = 50.0d;
            var strBaseData = default(string[][]);

            var axisSectionSeries = new SectionsCollection
            {
                new AxisSection()
                {
                    SectionWidth = 0d,
                    StrokeThickness = 2d,
                    Stroke = System.Windows.Media.Brushes.Gray,
                    Value = 0d
                }
            };

            var YAxis = new Axis()
            {
                LabelFormatter = new Func<double, string>(x => x.ToString() + "m"),
                Title = "Depth (Mtr)",
                Separator = new Separator()
                {
                    IsEnabled = true,
                    StrokeThickness = 1d
                }
            };
            var XAxis = new Axis()
            {
                Title = "Displacement (mm)",
                LabelFormatter = new Func<double, string>(y => Math.Round(y, 2).ToString()),
                MaxValue = 80d,
                MinValue = -80,
                Separator = new Separator()
                {
                    IsEnabled = true,
                    Step = 10d,
                    StrokeThickness = 1d
                },
                Sections = axisSectionSeries
            };
            var seriesCollection = new SeriesCollection();

            ResetLabels();

            CartesianChart1.BackColor = System.Drawing.Color.White;
            // CartesianChart1.Zoom = ZoomingOptions.X
            CartesianChart1.Series.Clear();
            CartesianChart1.AxisX.Clear();
            CartesianChart1.AxisY.Clear();
            CartesianChart1.AxisY.Add(YAxis);
            CartesianChart1.AxisX.Add(XAxis);
            if (DataGridView1.Visible)
                DataGridView1.Visible = false;
            if (CartesianChart1.Visible == false)
                CartesianChart1.Visible = true;
            if (!ToolStrip2.Enabled)
                ToolStrip2.Enabled = true;
            // PrintToolStripButton.Enabled = False
            tbAxisX.Enabled = true;
            tbAxisY.Enabled = true;
            tbZoom.Enabled = true;
            tbGraphType.Enabled = true;

            if (CultureInfo.CurrentCulture.CompareInfo.Compare(tbGraphType.Text, "Deviation", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
            {
                if (listBH[bhIndex].BaseFile is null | string.IsNullOrEmpty(listBH[bhIndex].BaseFile))
                {
                    Interaction.MsgBox("No base file selected for this borehole. Go back and select a base file to view deviation.", Constants.vbOKOnly | Constants.vbExclamation, "Graph");
                    return;
                }
                string strFile = GlobalCode.GetBoreholeDirectory(ref boreHoleSelected) + @"\" + listBH[bhIndex].BaseFile;
                if (!System.IO.File.Exists(strFile))
                {
                    Interaction.MsgBox("Base file does not exist. It must have been deleted. Please select another file as base.", Constants.vbOKOnly | Constants.vbExclamation, "Graph");
                    return;
                }
                strBaseData = GlobalCode.ReadCSVFile(ref strFile);
            }

            foreach (string lstItem in lstBoreholes.SelectedItems)
            {
                string argFileName = GlobalCode.GetBoreholeDirectory(ref boreHoleSelected) + @"\" + lstItem;
                string[][] strData = GlobalCode.ReadCSVFile(ref argFileName);
                string strFile = lstItem.Split('.').First().Replace("_", ":");

                if (CultureInfo.CurrentCulture.CompareInfo.Compare(tbGraphType.Text, "Deviation", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    if (strData.Length != strBaseData.Length)
                    {
                        Interaction.MsgBox("Scale or length mismatch between Base file and Selected file.", Constants.vbExclamation | Constants.vbOKOnly, "Graph");
                        return;
                    }
                }
                // Dim invertedYMapper
                // = LiveCharts.Configurations.Mappers.Xy(Of ObservablePoint)().X(Function(point) point.Y).Y(Function(point) -point.X)
                var lineSeries = new VerticalLineSeries()
                {
                    Title = "[" + strFile + "]",
                    Values = new ChartValues<ObservablePoint>(),
                    Fill = System.Windows.Media.Brushes.Transparent
                };
                short i = 0;
                float Val;
                float absVal = 0f;
                float Val2;
                float absVal2 = 0f;
                var loopTo = (short)(strData.Length - 1);
                for (i = 4; i <= loopTo; i++)
                {
                    Val = (float.Parse(strData[i][1 + _axisValue]) - float.Parse(strData[i][2 + _axisValue])) / 2f;
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(tbGraphType.Text, "Absolute", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    {
                        Val += absVal;
                        absVal = Val;
                    }
                    else if (CultureInfo.CurrentCulture.CompareInfo.Compare(tbGraphType.Text, "Deviation", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    {
                        Val += absVal;
                        absVal = Val;
                        Val2 = (float.Parse(strBaseData[i][1 + _axisValue]) - float.Parse(strBaseData[i][2 + _axisValue])) / 2f;
                        Val2 += absVal2;
                        absVal2 = Val2;
                        Val = absVal - absVal2;
                    }
                    if (Math.Ceiling((double)Math.Abs(Val)) > maxX)
                    {
                        maxX = Math.Ceiling((double)Math.Abs(Val));
                    }
                    lineSeries.Values.Add(new ObservablePoint((double)Val, (double)-float.Parse(strData[i][0])));

                }

                maxX += maxX * 0.2d;
                maxX = Math.Ceiling(maxX);
                XAxis.MinValue = -maxX;
                XAxis.MaxValue = maxX;
                if (maxX > 150d)
                    XAxis.Separator.Step = 40d;

                // set the inverted mapping...
                // lineSeries.Configuration = invertedYMapper

                seriesCollection.Add(lineSeries);

                // correct the labels
                // XAxis.LabelFormatter = Function(x) (x * -1).ToString() & "m"

                // Dim tooltip = New DefaultTooltip With {
                // .SelectionMode = TooltipSelectionMode.OnlySender
                // }
                // CartesianChart1.DataTooltip = tooltip
                switch (cnt)
                {
                    case 0:
                        {
                            Label1.Text = strFile;
                            break;
                        }
                    case 1:
                        {
                            Label2.Text = strFile;
                            break;
                        }
                    case 2:
                        {
                            Label3.Text = strFile;
                            break;
                        }
                    case 3:
                        {
                            Label4.Text = strFile;
                            break;
                        }
                    case 4:
                        {
                            Label5.Text = strFile;
                            break;
                        }
                }
                cnt = (short)(cnt + 1);
            }
            if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(tbGraphType.SelectedItem, "Deviation", true)))
            {
                Label6.Text = "Base File : " + listBH[bhIndex].BaseFile.Split('.').First().Replace("_", ":");
            }
            else
            {
                Label6.Text = "";
            }
            CartesianChart1.Series = seriesCollection;
        }

        private void tbAxisX_Click(object sender, EventArgs e)
        {
            if (tbAxisY.Checked == true)
            {
                tbAxisY.Checked = false;
                _axisValue = 0;
                DisplayGraph();
            }
            else
            {
                tbAxisX.Checked = true;
            }
        }

        private void tbAxisY_Click(object sender, EventArgs e)
        {
            if (tbAxisX.Checked == true)
            {
                tbAxisX.Checked = false;
                _axisValue = 2;
                DisplayGraph();
            }
            else
            {
                tbAxisY.Checked = true;
            }
        }

        private void tbViewGraph_Click(object sender, EventArgs e)
        {
            if (boreHoleSelected == 0)
                return;
            if (lstBoreholes.SelectedItems.Count == 0)
                return;
            if (lstBoreholes.SelectedItems.Count > 5)
            {
                Interaction.MsgBox("You have selected " + lstBoreholes.SelectedItems.Count + " files. You can select maximum 5 files for plotting graph", Constants.vbOKOnly | Constants.vbExclamation, "Graph");
                return;
            }
            DataGridView1.Visible = false;
            DisplayGraph();
        }

        private void ResetLabels()
        {
            Label1.Text = "";
            Label2.Text = "";
            Label3.Text = "";
            Label4.Text = "";
            Label5.Text = "";
            Label6.Text = @"View Graph of one or multiple files.";

            if (bhIndex >= 0)
            {
                lblBoreholeNumber.Text = "Borehole : " + boreHoleSelected.ToString().PadLeft(2, '0');
                lblDepth.Text = "Depth : " + listBH[bhIndex].Depth + "m";
                lblSiteName.Text = "Site : " + listBH[bhIndex].SiteName;
                lblLocation.Text = "Location : " + listBH[bhIndex].Location;
            }
            else
            {
                lblBoreholeNumber.Text = "";
                lblDepth.Text = "";
                lblSiteName.Text = "";
                lblLocation.Text = "";
            }
        }

        private void ToolBarEnable(ref bool enb)
        {
            bool bnOneFileSelected = lstBoreholes.SelectedItems.Count == 1;
            tbBack.Enabled = enb;
            if (enb & lstBoreholes.SelectedItems.Count > 0)
            {
                tbViewGraph.Enabled = true;
                tbDelete.Enabled = true;
                tbReport.Enabled = bnOneFileSelected;
                tbBaseFile.Enabled = bnOneFileSelected;
            }
            else
            {
                tbViewGraph.Enabled = false;
                tbDelete.Enabled = false;
                tbBaseFile.Enabled = false;
                tbReport.Enabled = false;
            }
        }

        private void lstBoreholes_Click(object sender, EventArgs e)
        {
            bool argenb = !(boreHoleSelected == 0);
            ToolBarEnable(ref argenb);
        }

        private void TbGraphType_SelChange(object sender, EventArgs e)
        {
            // to be implemented
            DisplayGraph();
        }

        private void tbZoom_Click(object sender, EventArgs e)
        {
            if (tbZoom.Checked)
            {
                CartesianChart1.Zoom = ZoomingOptions.X;
            }
            else
            {
                CartesianChart1.Zoom = ZoomingOptions.None;
            }
        }

        private void tbDelete_Click(object sender, EventArgs e)
        {
            if (lstBoreholes.SelectedItems.Count == 0)
                return;

            if (Interaction.MsgBox("Are you sure you want to delete " + lstBoreholes.SelectedItems.Count + " selected file(s)?", Constants.vbYesNo | Constants.vbQuestion, "Delete") == Constants.vbYes)
            {
                foreach (string strFile in lstBoreholes.SelectedItems)
                    System.IO.File.Delete(GlobalCode.GetBoreholeDirectory(ref boreHoleSelected) + @"\" + strFile);
                ReloadList();
            }
        }

        private void PrintToolStripButton_Click(object sender, EventArgs e)
        {
            if (PrintDialog1.ShowDialog() != DialogResult.OK)
                return;
            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings;

            bsTextPrintData = "Borehole  : " + boreHoleSelected.ToString().PadLeft(2, '0') + Constants.vbCrLf;
            bsTextPrintData += "Depth     : " + listBH[bhIndex].Depth + "m" + Constants.vbCrLf;
            bsTextPrintData += "Site      : " + listBH[bhIndex].SiteName + Constants.vbCrLf;
            bsTextPrintData += "Location  : " + listBH[bhIndex].Location + Constants.vbCrLf;
            bsTextPrintData += "Date/Time : " + Label1.Text;
            if (CartesianChart1.Visible)
            {
                if (!string.IsNullOrEmpty(Label2.Text))
                    bsTextPrintData += ", " + Label2.Text;
                if (!string.IsNullOrEmpty(Label3.Text))
                    bsTextPrintData += ", " + Label3.Text;
                if (!string.IsNullOrEmpty(Label4.Text))
                    bsTextPrintData += ", " + Label4.Text;
                if (!string.IsNullOrEmpty(Label5.Text))
                    bsTextPrintData += ", " + Label5.Text;
                PrintDocument1.DefaultPageSettings.Margins.Left = 20;
                PrintDocument1.DefaultPageSettings.Margins.Top = 20;
                PrintDocument1.DefaultPageSettings.Margins.Right = 15;
            }
            else
            {
                PrintDocument1.DefaultPageSettings.Margins.Left = 90;
                PrintDocument1.DefaultPageSettings.Margins.Top = 90;
                PrintDocument1.DefaultPageSettings.Margins.Right = 75;
            }
            bsTextPrintData += Constants.vbCrLf;
            if (!string.IsNullOrEmpty(Label6.Text))
                bsTextPrintData += Label6.Text + Constants.vbCrLf;

            // Report Printing code below
            printFont = new Font("Courier New", 9f, FontStyle.Regular);
            PrintDocument1.DefaultPageSettings.Landscape = true;
            if (DataGridView1.Visible)
                DisplayReport(true);
            // PrintDocument1.Print()

            // Show the Print Preview Dialog.
            PrintPreviewDialog1.Document = PrintDocument1;
            PrintPreviewDialog1.PrintPreviewControl.Zoom = 1d;
            PrintPreviewDialog1.ShowDialog();
            // PrintDocument1.Dispose()
        }

        private void tbReport_Click(object sender, EventArgs e)
        {
            DataGridView1.Visible = false;
            CartesianChart1.Visible = false;
            DisplayReport();
        }

        private void tbBaseFile_Click(object sender, EventArgs e)
        {
            listBH[bhIndex].BaseFile = Conversions.ToString(lstBoreholes.SelectedItem);
            var tmp = listBH;
            var argbh = tmp[bhIndex];
            GlobalCode.UpdateBorehole(ref argbh);
            tmp[bhIndex] = argbh;
            ReloadList();
        }

        private void ListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            if (e.Index < 0)
                return;

            // Define the default color of the brush as black.
            var myBrush = System.Drawing.Brushes.Black;

            if (bhIndex >= 0)
            {
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(listBH[bhIndex].BaseFile ?? "", lstBoreholes.Items[e.Index].ToString() ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    myBrush = System.Drawing.Brushes.OrangeRed;
                }
                else
                {
                    myBrush = System.Drawing.Brushes.Cyan;
                }
            }

            // Draw the current item text based on the current 
            // Font and the custom brush settings.
            e.Graphics.DrawString(lstBoreholes.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);

            // If the ListBox has focus, draw a focus rectangle around  _ 
            // the selected item.
            e.DrawFocusRectangle();
        }


        private void PrintForm1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var strFormat = new StringFormat();
            var rectDraw = new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width, e.MarginBounds.Height);

            strFormat.Trimming = StringTrimming.Word;
            if (DataGridView1.Visible)
            {
                int numChars;
                int numLines;
                string stringForPage;
                var sizeMeasure = new SizeF(e.MarginBounds.Width, e.MarginBounds.Height - printFont.GetHeight(e.Graphics));

                e.Graphics.MeasureString(bsTextPrintData, printFont, sizeMeasure, strFormat, out numChars, out numLines);
                stringForPage = bsTextPrintData.Substring(0, numChars);
                e.Graphics.DrawString(stringForPage, printFont, System.Drawing.Brushes.Black, rectDraw, strFormat);
                if (numChars < bsTextPrintData.Length)
                {
                    bsTextPrintData = bsTextPrintData.Substring(numChars);
                    e.HasMorePages = true;
                }
                else
                {
                    e.HasMorePages = false;
                }
            }
            else
            {
                var MyChartPanel = new Bitmap(SplitContainer2.Panel1.Width, SplitContainer2.Panel1.Height);
                SplitContainer2.Panel1.DrawToBitmap(MyChartPanel, new Rectangle(0, 0, SplitContainer2.Panel1.Width, SplitContainer2.Panel1.Height));
                var p1 = default(Point);
                p1.X = 5;
                p1.Y = 110;
                if (MyChartPanel.Size.Width < e.PageBounds.Width)
                {
                    p1.X = (int)Math.Round((e.PageBounds.Width - MyChartPanel.Size.Width) / 2d);
                }
                e.Graphics.DrawString(bsTextPrintData, printFont, System.Drawing.Brushes.Black, rectDraw, strFormat);
                e.Graphics.DrawImage(MyChartPanel, p1);
                e.HasMorePages = false;
            }
        }

        private void lstBoreholes_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}