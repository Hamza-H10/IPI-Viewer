﻿using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using NodaTime;
using NodaTime.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;



namespace InclinoRS485
{
    /// <summary>
    /// This class represents the main form of the application.
    /// </summary>
    /// <author>Hamza</author>
    /// <date>2023-09-10</date>
    public partial class Form1
    {
        // Class-level fields for managing data
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

        // Reloads the list of boreholes in the user interface
        private void ReloadList()
        {
            Console.WriteLine("Inside ReloadList function");
            // Clear the list of boreholes
            lstBoreholes.Items.Clear();

            // Check if a specific borehole is selected
            if (boreHoleSelected == 0)
            {
                // Retrieve the list of boreholes from the database
                listBH = GlobalCode.GetBoreholes();

                // Populate the list box with borehole information
                foreach (var bitem in listBH)
                    lstBoreholes.Items.Add("[" + bitem.Id.ToString("D2") + "] " + bitem.SiteName + " - " + bitem.Location);

                // Configure list box selection mode and toolbar
                lstBoreholes.SelectionMode = SelectionMode.One;
                bool argenb = false;
                ToolBarEnable(ref argenb);
            }
            else
            {	// get directory listing
                // Get a listing of CSV files in the selected borehole directory
                var di = new System.IO.DirectoryInfo(GlobalCode.GetBoreholeDirectory(ref boreHoleSelected));
                Console.WriteLine("di: " + di);
                System.IO.FileInfo[] aryFi = di.GetFiles("*.csv");

                // Populate the list box with CSV file names
                foreach (var fi in aryFi)
                    lstBoreholes.Items.Add(fi.Name);

                // Configure list box selection mode and toolbar
                lstBoreholes.SelectionMode = SelectionMode.MultiSimple;
                bool argenb1 = true;
                ToolBarEnable(ref argenb1);
            }

            // Reset labels and hide chart and DataGridView
            ResetLabels();
            CartesianChart1.Visible = false;
            DataGridView1.Visible = false;
            ToolStrip2.Enabled = false;
            //toolStripSplitButton1.Enabled = false;


            // Reset the state
            //is_MM = false; // or true, depending on what you consider the initial state

            // Reset properties - Update the button text, color based on the reset state
            //toolStripSplitButton1.Text = is_MM ? "MM": "DEG";
            toolStripSplitButton1.Text = null;
            toolStripSplitButton1.BackgroundImage = null; // or set to initial image
            toolStripSplitButton1.BackgroundImageLayout = ImageLayout.None;
            toolStripSplitButton1.BackColor = is_MM ? Color.Cyan : Color.LightGreen; // or set to initial color

            // Reset other properties and states as needed...

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set label colors
            Label1.ForeColor = System.Drawing.Color.FromArgb(33, 149, 242);
            Label2.ForeColor = System.Drawing.Color.FromArgb(243, 67, 54);
            Label3.ForeColor = System.Drawing.Color.FromArgb(254, 192, 7);
            Label4.ForeColor = System.Drawing.Color.FromArgb(96, 125, 138);
            Label5.ForeColor = System.Drawing.Color.FromArgb(0, 187, 211);
            label7.ForeColor = System.Drawing.Color.FromArgb(255, 20, 147);
            label8.ForeColor = System.Drawing.Color.FromArgb(255, 69, 0);

            // Open the application's database
            GlobalCode.OpenDatabase();
            // _DeleteAllBoreholes() ' temporary delete all
            //tbGraphType.SelectedIndex = 0;

            // Load the list of boreholes
            ReloadList();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Close the application's database when the form is closed
            GlobalCode.CloseDatabase();
        }

        //============================================================================================================================ 
        private void tbImport_Click(object sender, EventArgs e)
        {
            // Initialize counters to keep track of import results
            short cnt = 0;          // Counter for successfully imported files
            short cntError = 0;     // Counter for files with incorrect format
            short cntRepeat = 0;    // Counter for files already imported

            // Prepare a message string to summarize the import process
            string msgString = "Import Summary:" + Environment.NewLine;

            // Check if the user selected files using the OpenFileDialog
            if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Loop through each selected file
                foreach (string strFileName in OpenFileDialog1.FileNames)
                {
                    // Create a temporary file name and extract the file name
                    string tempFileName = strFileName;
                    string strFileNew = strFileName.Split('\\').Last();

                    // Check if the file extension is "csv" (case-insensitive)
                    if (CultureInfo.CurrentCulture.CompareInfo.Compare(strFileNew.Split('.').Last().ToLower(), "csv", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                    {
                        // Read the CSV file data into a two-dimensional string array
                        string[][] strData = GlobalCode.ReadCSVFile(ref tempFileName);

                        // Check if the CSV data has a minimum number of rows
                        if (strData.Length < 5)
                        {
                            cntError = (short)(cntError + 1);
                        }
                        else
                        {
                            // Catch 4 parameters for the new borehole
                            // Parse the borehole number, directory name, and depth from the CSV data
                            short borehole_num;
                            float depth;
                            string strDirName;

                            borehole_num = short.Parse(strData[0][1]);
                            strDirName = GlobalCode.GetBoreholeDirectory(ref borehole_num);
                            strFileNew = strDirName + @"\" + strFileNew;


                            // Check if the file already exists (if imported previously)
                            if (System.IO.File.Exists(strFileNew))
                            {
                                cntRepeat = (short)(cntRepeat + 1);
                            }
                            else
                            {
                                // Check if the directory exists; if not, create it
                                if (!System.IO.Directory.Exists(strDirName))
                                {
                                    System.IO.Directory.CreateDirectory(strDirName);
                                }

                                // Copy the selected file to the destination directory
                                //FileSystem.FileCopy(strFileName, strFileNew);//(source path, target path)                                                                                                                          

                                depth = float.Parse(strData[4][2]);

                                // Create a new BoreHole object and add/update it
                                var bh = new GlobalCode.BoreHole() { Id = borehole_num, SiteName = strData[1][1], Location = strData[2][1], Depth = depth, BaseFile = "" };

                                // Add or update the BoreHole in the application
                                if (!GlobalCode.AddBorehole(ref bh))
                                {
                                    GlobalCode.UpdateBorehole(ref bh);
                                }

                                SplitCSVDataIntoSubFiles(strData, strDirName);

                                // Reload the list
                                ReloadList();
                                cnt = (short)(cnt + 1);
                            }
                        }
                    }
                }
                // Prepare a summary message with import results
                if (cnt > 0)
                    msgString += "You have added " + cnt + " CSV file(s) to the InclinoRS485 successfully." + Constants.vbCrLf;//Addition - no. of new subfiles have been created, no. of subfiles were already in the borehole directory,  
                if (cntError > 0)
                    msgString += cntError + " file(s) were found to be incorrect format." + Constants.vbCrLf;
                if (cntRepeat > 0)
                    msgString += cntRepeat + " file(s) were already imported into the application, hence ignored." + Constants.vbCrLf;

                // Display the summary message to the user
                Console.WriteLine(msgString);
                Interaction.MsgBox(msgString, MsgBoxStyle.OkOnly | MsgBoxStyle.Information, "Import");
            }
        }
        private void SplitCSVDataIntoSubFiles(string[][] strData, string strDirName)
        {
            Console.WriteLine("INSIDE THE SPLIT CSV IN SUBFILES FUNCTION");

            // Prepare a message string to summarize the import process
            string msgString = "Import Summary:" + Environment.NewLine;

            int cntSubRepeat = 0;
            // Define a base directory where you want to store the sub-files
            string baseDirectory = strDirName; // Change this to your desired directory
            Console.WriteLine("strDirName: " + strDirName);

            // Create a dictionary to store sub-file data by date
            Dictionary<string, Dictionary<string, List<string>>> subFiles = new Dictionary<string, Dictionary<string, List<string>>>();

            // Initialize a counter for sub-files
            int subFileCount = 0;

            // Iterate through the CSV data starting from the row with column headers (strData[3][0])
            for (int i = 4; i < strData.Length; i++)
            {
                // Parse the LocalDateTime value from the "DateTime" column using Noda Time
                var pattern1 = LocalDateTimePattern.CreateWithInvariantCulture("dd/MM/yyyy HH:mm");
                var pattern2 = LocalDateTimePattern.CreateWithInvariantCulture("dd-MM-yyyy HH:mm");

                var parseResult1 = pattern1.Parse(strData[i][0]);
                var parseResult2 = pattern2.Parse(strData[i][0]);

                if (!parseResult1.Success && !parseResult2.Success)
                {
                    // Handle the case where date parsing fails (invalid date format)
                    Console.WriteLine($"Error parsing date on row {i + 1}: Invalid date format");
                    continue; // Skip this row and continue with the next
                }

                var localDateTime = parseResult1.Success ? parseResult1.Value : parseResult2.Value;

                // Extract the date and time parts
                string datePart = localDateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                Console.WriteLine("datePart: " + datePart);
                string timePart = localDateTime.ToString("HH_mm", CultureInfo.InvariantCulture); // Replace ":" with "_"
                Console.WriteLine("timePart: " + timePart);

                // Check if the date is already in the dictionary
                if (!subFiles.ContainsKey(datePart))
                {
                    // Create a new dictionary for this date to store data by time
                    subFiles[datePart] = new Dictionary<string, List<string>>();
                }

                // Check if the time is already in the dictionary for this date
                if (!subFiles[datePart].ContainsKey(timePart))
                {
                    // Create a new list for this time
                    subFiles[datePart][timePart] = new List<string>();
                }

                // Add the row to the list for the corresponding date and time
                subFiles[datePart][timePart].Add(string.Join(",", strData[i])); // Assuming comma-separated values
            }

            // Create sub-files based on the dictionary and count them
            foreach (var dateEntry in subFiles)
            {
                string date = dateEntry.Key;
                Console.WriteLine("date: " + date);

                // Find the maximum timePart for this date
                //string maxTimePart = dateEntry.Value.Keys.Max();

                // Find the maximum hourPart for this date
                string maxHourPart = dateEntry.Value.Keys.Max(hourPart => hourPart.Substring(0, 2));

                // Select the rows for the maximum timePart
                //List<string> rows = dateEntry.Value[maxTimePart];

                // Select the rows for the maximum hourPart
                var filteredKeys = dateEntry.Value.Keys.Where(key => key.StartsWith(maxHourPart));
                List<string> rows = new List<string>();
                foreach (var key in filteredKeys)
                {
                    rows.AddRange(dateEntry.Value[key]);
                }

                // Create a sub-file with the date and maximum timePart as the filename
                DateTime subFileDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //string formattedDateForFilename = subFileDate.ToString("dd-MM-yyyy"); // Format for filename readability
                string formattedDateForOrdering = subFileDate.ToString("yyyyMMdd"); // Format for serial-wise ordering
                string formattedDateForFilename = subFileDate.ToString("dd-MM-yyyy"); // Format for filename readability
                //string subFileName = $"{formattedDateForFilename} {maxTimePart}.csv"; // You can change the file extension as needed
                //string subFileName = $"{formattedDateForOrdering}[ {formattedDateForFilename} ] [ {maxHourPart} ].csv";
                string subFileName = $"{formattedDateForOrdering}( {formattedDateForFilename} {maxHourPart} ).csv";

                if (System.IO.File.Exists(subFileName))
                {
                    cntSubRepeat = (short)(cntSubRepeat + 1);
                }

                // Construct the destination path for the sub-file in the borehole directory
                string destinationPath = Path.Combine(baseDirectory, subFileName);

                // Save the sub-file to the borehole directory
                File.WriteAllLines(destinationPath, rows);

                // Increment the sub-file count
                subFileCount++;

                // Read the contents of the sub-file and print the rows
                /*string[] fileContents = File.ReadAllLines(destinationPath);
                foreach (string row in fileContents)
                {
                    Console.WriteLine(row);
                }*/
            }
            // Print the total number of sub-files created
            Console.WriteLine($"Total number of sub-files created: {subFileCount}");
            if (subFileCount > 0)
                msgString += subFileCount + " Subfiles have been created and added to the InclinoRS485 successfully." + Environment.NewLine;
            if (cntSubRepeat > 0)
                msgString += cntSubRepeat + " Files were already present in the directory, hence ignored";

            Interaction.MsgBox(msgString, MsgBoxStyle.OkOnly | MsgBoxStyle.Information, "Import");
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

        //-----------------------------------------------------------------------------------------------------DISPLAY REPORT FOR IMPORTED FILES old code here 

        //----------------------------------------------------------------------------------------------------------------

        private void DisplayReport(bool bnLoadText = false) //DISPLAY REPORT FUNCTION FOR THE SUBFILES
        {
            //var ds = new DataTable(); // Create a DataTable to hold the report data
            var strBaseData = default(string[][]); // Store data from a base file
            var bnBaseFilePresent = default(bool); // Flag indicating if a base file is present

            short i;

            //-----------------------------------------------------------------------------------------------------
            //FOR BASE FILE UNCOMMENT THIS WHEN IMPLEMENT FOR BASEfile

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

            //------------------------------------------------------------------------------------------------------
            Label1.Text = lstBoreholes.SelectedItem.ToString().Split('.').First().Replace("_", ":");

            // Construct the path to the selected data file
            string argFileName = Conversions.ToString(Operators.ConcatenateObject(GlobalCode.GetBoreholeDirectory(ref boreHoleSelected) + @"\", lstBoreholes.SelectedItem));
            Console.WriteLine("argFileName: " + argFileName);

            // Read the CSV file
            string[][] strData = GlobalCode.ReadCSVFile(ref argFileName);

            // Create LocalDateTimePattern objects
            LocalDateTimePattern pattern1 = LocalDateTimePattern.CreateWithInvariantCulture("dd/MM/yyyy HH:mm");
            LocalDateTimePattern pattern2 = LocalDateTimePattern.CreateWithInvariantCulture("dd-MM-yyyy HH:mm");

            // Create columns in the DataTable to hold the report data
            DataTable ds = new DataTable();
            ds.Columns.Add("DateTime", typeof(LocalDateTime)); // Add a LocalDateTime column
            ds.Columns.Add("Sensor", typeof(int));
            ds.Columns.Add("Depth", typeof(float));
            ds.Columns.Add("A", typeof(float));
            ds.Columns.Add("B", typeof(float));

            // Process data rows
            var loopTo = (short)(strData.Length - 1);
            Console.WriteLine("size of the file: " + loopTo);
            for (int index = 0; index <= loopTo; index++)
            {
                if (strData[index].Length < 5)
                {
                    Console.WriteLine($"Error at row {index + 1}: Insufficient data columns");
                    continue; // Skip this row and continue with the next
                }

                try
                {
                    // Parse the date and time string using the Noda Time library
                    LocalDateTime dateTimeValue;
                    if (pattern1.Parse(strData[index][0]).TryGetValue(default(LocalDateTime), out dateTimeValue) ||
                    pattern2.Parse(strData[index][0]).TryGetValue(default(LocalDateTime), out dateTimeValue))
                    {
                        // Parse the other values in the row
                        int intValue1 = int.Parse(strData[index][1]);
                        float intValue2 = float.Parse(strData[index][2]);
                        float floatValue1 = float.Parse(strData[index][3]);
                        float floatValue2 = float.Parse(strData[index][4]);

                        // Add the row to the DataTable
                        ds.Rows.Add(new object[] { dateTimeValue, intValue1, intValue2, floatValue1, floatValue2 });
                    }
                    else
                    {
                        // Handle the case where parsing fails
                        Console.WriteLine($"Error parsing date at row {index + 1}: {strData[index][0]}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle the Exception
                    Console.WriteLine($"Error at row {index + 1}: {ex.Message}");
                }
            }

            //--------------------------------------------------------------------------------------------------------------------------------------
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

                        if (ds.Rows[row][i] != DBNull.Value)
                        {
                            // Check if the value is a DateTime
                            if (ds.Rows[row][i] is DateTime)
                            {
                                // Handle DateTime values by formatting them as a string
                                strItem = ((DateTime)ds.Rows[row][i]).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (ds.Rows[row][i] is decimal || ds.Rows[row][i] is int)
                            {
                                // Handle numeric values by formatting them as a number
                                strItem = Strings.FormatNumber(ds.Rows[row][i], 2);
                            }
                            else
                            {
                                // Handle other data types here (e.g., leave as-is or apply custom logic)
                                strItem = ds.Rows[row][i].ToString();
                            }
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
                }


                /*var loopTo2 = ds.Rows.Count - 1;            
                for (row = 0; row <= loopTo2; row++)
                {
                    var loopTo3 = (short)(ds.Columns.Count - 1);                 
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
                            //bsTextPrintData += "  " + FormatDateTime(ds.Rows[row][i]).PadLeft(8) + "  ";
                        }
                        else
                        {
                            bsTextPrintData += " " + strItem.PadLeft(6) + " ";
                            //bsTextPrintData += " " + FormatDateTime(ds.Rows[row][i]).PadLeft(6) + " ";
                        }
                    }
                    bsTextPrintData += Constants.vbCrLf;                
                }*/
            }
            else
            {
                // Display the report in a DataGridView (if not loading text)
                DataGridView1.DataSource = ds;
                DataGridView1.Visible = true;

                //Set column widths
                var loopTo4 = (short)(DataGridView1.Columns.Count - 1);
                Console.WriteLine(loopTo4);
                for (i = 0; i <= loopTo4; i++)
                {
                    DataGridView1.Columns[i].Width = (i > 6) ? 100 : 100;

                    DataGridView1.Columns["Sensor"].DefaultCellStyle.Format = "D"; // "D" format for integers

                    // Set the DateTime format for the "DateTime" column                
                    if (DataGridView1.Columns[i].Name == "DateTime")
                    {
                        DataGridView1.Columns[i].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                    }
                }
                // Configure DataGridView appearance
                DataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11f, FontStyle.Bold | FontStyle.Italic);
                DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //---------------------------
                // Change the background color of the rows
                DataGridView1.RowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
                //DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightSteelBlue;
                //----------------------------

                // Change the background color of the entire DataGridView
                DataGridView1.BackgroundColor = Color.WhiteSmoke;

                // Change the background color of selected cells
                DataGridView1.DefaultCellStyle.SelectionBackColor = Color.Red;
                DataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;


                // Enable or disable ToolStrip buttons based on conditions
                if (!ToolStrip2.Enabled)
                    ToolStrip2.Enabled = true;

                //PrintToolStripButton.Enabled = True
                tbAxisX.Enabled = false;
                tbAxisY.Enabled = false;
                tbZoom.Enabled = false;
                toolStripSplitButton1.Enabled = false;
                //tbGraphType.Enabled = false;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void DisplayGraph(bool is_MM = false)
        {
            // Initialize variables
            var cnt = default(short); // Counter for labels
            double maxX = 50.0d; // Maximum X value for the chart
            var strBaseData = default(string[][]); // Array to store base data

            //ResetToolStripSplitButton1(); //this will reset the state of this button

            // Create an empty collection for chart axis sections
            var axisSectionSeries = new SectionsCollection
            {
                new AxisSection()
                {
                    SectionWidth = 0d,
                    StrokeThickness = 2d,
                    Stroke = System.Windows.Media.Brushes.DarkGray,
                    Value = 0d
                }
            };

            // Create Y-axis with label formatter and styling
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
            // Create X-axis with label formatter, range, and styling
            
            var XAxis = new Axis()
            {
                Title = is_MM ? "Displacement (mm)" : "Displacement (deg)",

                LabelFormatter = new Func<double, string>(y => Math.Round(y, 2).ToString()),

                MaxValue = 60d,
                MinValue = -60d,
                Separator = new Separator()
                {
                    IsEnabled = true,
                    Step = 10d,
                    StrokeThickness = 1d
                },

                Sections = axisSectionSeries // Add axis sections
            };

            // Create a collection to hold chart series
            var seriesCollection = new SeriesCollection();

            // Reset labels and configure chart
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


            //-----------------------------------------------------

            // Check if a base file is selected
            //if (listBH[bhIndex].BaseFile is null || string.IsNullOrEmpty(listBH[bhIndex].BaseFile))
            //{
            //    //if () 
            //    //{ Interaction.MsgBox("Base is not selected. Please view graph in MM", Constants.vbOKOnly | Constants.vbExclamation, "Graph");
            //    //}
            //    return;
            //}

            //-------------------------------------------//check here the baseFile path here 
            string dirPath = "";
            string baseFile = "";
            string strFileBase = "";
            bool isBaseFilePresent = false; // flag variable
            //string strBaseData = "";

            try
            {
                dirPath = GlobalCode.GetBoreholeDirectory(ref boreHoleSelected);
                baseFile = listBH[bhIndex].BaseFile;

                if (!string.IsNullOrEmpty(baseFile)) // check if baseFile is not empty
                {
                    strFileBase = Path.Combine(dirPath, baseFile);
                    isBaseFilePresent = true; // update flag variable

                    // Read the base data from the base file
                    strBaseData = GlobalCode.ReadCSVFile(ref strFileBase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: {0}", ex.Message);
            }


            //----------------------------------------------

            // Get the path to the base file
            //string strFileBase = GlobalCode.GetBoreholeDirectory(ref boreHoleSelected) + @"\" + listBH[bhIndex].BaseFile;

            //below code was implemented for the deviate
            //Check if the base file exists
            //if (!System.IO.File.Exists(strFileBase))//if making the BaseFile concrete function then uncomment this.
            //{
            //    //Interaction.MsgBox("Base file does not exist. It must have been deleted. Please select another file as a base.", Constants.vbOKOnly | Constants.vbExclamation, "Graph");
            //    //return; //uncomment this return statment.
            //}

            // Read the base data from the base file 
            //strBaseData = GlobalCode.ReadCSVFile(ref strFileBase); //error - happening on this line

            // #ToDo: It will not read the data if basefile does not exist, make a logic here to bypass this situation
            //---------------------------------------------------------
            Console.WriteLine($"SelectedItems Count: {lstBoreholes.SelectedItems.Count}");

            // Loop through selected items
            foreach (string lstItem in lstBoreholes.SelectedItems)
            {
                Console.WriteLine("Inside foreach loop");
                // Get the path to the current file
                string argFileName = GlobalCode.GetBoreholeDirectory(ref boreHoleSelected) + @"\" + lstItem;
                string[][] strData = GlobalCode.ReadCSVFile(ref argFileName);
                string strFile = lstItem.Split('.').First().Replace("_", ":");


                // Create a new line series for the chart
                var lineSeries = new VerticalLineSeries()
                {
                    Title = "[" + strFile + "]",
                    Values = new ChartValues<ObservablePoint>(),
                    Fill = System.Windows.Media.Brushes.Transparent,

                    PointGeometry = DefaultGeometries.Diamond, // Change the data value indicator to a circle
                    PointGeometrySize = 9, // Adjust the size of the data value indicator
                    //Stroke = System.Windows.Media.Brushes.Blue, // Change the line color to blue
                    StrokeThickness = 1.3 // Adjust the line thickness
                };
                //--------------------------------------------------------------------
                //float Val;
                //Val = float.Parse(strData[i][3 + _axisValue]);
                //--------------------------------------------------------------------
                short i = 0;
                float Val_deg;
               
                var loopTo = (short)(strData.Length - 1);

                // Populate line series with data points
                for (i = 0; i <= loopTo; i++)
                {
                    try
                    {
                        if (strData.Length != 8)
                        {
                            MessageBox.Show($"File {"( " + strFile + " )"}  do not contain required sensors data. Please check file {"( " + strFile + " )"}. Graph may not be formed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        if (is_MM)
                        {
                            if (System.IO.File.Exists(strFileBase))//if making the BaseFile concrete function then uncomment this.
                            {
                                //Interaction.MsgBox("Base file does not exist. It may have been removed or deleted .Please select another file as a base to view Degree Graph.", Constants.vbOKOnly | Constants.vbExclamation, "Graph");
                                //return; 

                                //Val = (float.Parse(strData[i][3 + _axisValue]) - float.Parse(strData[i][2 + _axisValue])) / 2f;                 
                                //Val = float.Parse(strData[i][3 + _axisValue]);

                                float Val_base = float.TryParse(strBaseData[i][3 + _axisValue], out float parsedValueBase) ? parsedValueBase : 0.0f;

                                //the Val variable in below line was named as Val then i have changed it to Val_deg
                                Val_deg = float.TryParse(strData[i][3 + _axisValue], out float parsedValue) ? parsedValue : 0.0f;
                                Console.WriteLine("val_deg: " + Val_deg);

                                float Val_abs = Val_base - Val_deg;
                                Console.WriteLine("val_abs: " + Val_abs);
                                // Round Val_abs to two decimal places
                                // Val_abs = (float)Math.Round(Val_abs, 2);

                                // Convert Val_deg to Val_mm using the formula
                                //float Val_abs_mm = Val_abs_deg * (float)(Math.PI / 180) * 6000;
                                float Val_abs_mm = Val_abs * (float)(3.14 / 180) * 460;
                                // Round Val_mm to one digit before the decimal point and two decimal digits after the decimal point
                                Val_abs_mm = (float)Math.Round(Val_abs_mm, 2);
                                Console.WriteLine("val_abs_mm: " + Val_abs_mm);
                                //lineSeries.Values.Add(new ObservablePoint((double)Val_abs_mm, (double)-float.Parse(strData[i][2]))); //lineSeries.Values: This represents the collection of data points for the lineSeries. It is of type ChartValues<ObservablePoint>.
                                float yValue = -float.Parse(strData[i][2]);
                                lineSeries.Values.Add(new ObservablePoint((double)Val_abs_mm, (double)yValue));
                                Console.WriteLine($"Val_abs_mm: {Val_abs_mm}, yValue: {yValue}");
                            }
                            else
                            {
                                //Interaction.MsgBox("Base file does not exist. showing MM graph without absolute values. Please.", Constants.vbOKOnly | Constants.vbExclamation, "Graph");

                                {
                                    Val_deg = float.TryParse(strData[i][3 + _axisValue], out float parsedValue) ? parsedValue : 0.0f;
                                    Console.WriteLine("val_deg: " + Val_deg);
                                    float Val_mm = Val_deg * (float)(3.14 / 180) * 460;
                                    Val_mm = (float)Math.Round(Val_mm, 2);
                                    Console.WriteLine("val_mm: " + Val_mm);
                                    float yValue = -float.Parse(strData[i][2]);
                                    lineSeries.Values.Add(new ObservablePoint((double)Val_mm, (double)yValue));
                                    Console.WriteLine($"Val_mm: {Val_mm}, yValue: {yValue}");
                                }
                            }
                        }
                        else
                        {
                            float Val = float.TryParse(strData[i][3 + _axisValue], out float parsedValue) ? parsedValue : 0.0f;
                            Console.WriteLine("Val: "+Val);
                            lineSeries.Values.Add(new ObservablePoint((double)Val, (double)-float.Parse(strData[i][2])));
                        }
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        MessageBox.Show($"Error: Index was outside the bounds of the array for file {"(" + strFile + ")"} . Please check if file {"(" + strFile + ")"}  contains the all the sensors data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //MessageBox.Show($"If data discrepancies occur, kindly contact the support to report.", "Import", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                

                maxX += maxX * 0.2d;
                maxX = Math.Ceiling(maxX);
                XAxis.MinValue = -maxX;
                XAxis.MaxValue = maxX;

       
                if (maxX > 150d)
                    XAxis.Separator.Step = 40d;

                seriesCollection.Add(lineSeries);             

                // Set labels
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
                    case 5:
                        {
                            label7.Text = strFile;
                            break;
                        }
                    case 6:
                        {
                            label8.Text = strFile;
                            break;
                        }

                }
                cnt = (short)(cnt + 1);
            }
 //-----------------------------------------------       
     //there is something changed here please refer to that file     

            //Update Label6 based on graph type
            //if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(tbGraphType.SelectedItem, "Deviation", true)))
            //{
            //    Label6.Text = "Base File : " + listBH[bhIndex].BaseFile.Split('.').First().Replace("_", ":");
            //}
            //else
            //{
            //    Label6.Text = "";
            //}

            //Update Label6 based on graph type
            //if (Conversions.ToBoolean(Operators.ConditionalCompareObjectEqual(tbGraphType.SelectedItem, "Deviation", true)))

            if (System.IO.File.Exists(strFileBase))
            {
                Label6.Text = "Base File : " + listBH[bhIndex].BaseFile.Split('.').First().Replace("_", ":");
            }
            else
            {
                Label6.Text = "";
            }

            Console.WriteLine(seriesCollection);
            // Set the series collection for the chart
            CartesianChart1.Series = seriesCollection;
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        private void tbAxisX_Click(object sender, EventArgs e)
        {
            if (tbAxisY.Checked == true)
            {
                tbAxisY.Checked = false;
                _axisValue = 0;
                if (is_MM) // Check if 'is_MM' is true
                {
                    DisplayGraph(true); // If 'is_MM' is true, call DisplayGraph with true
                }
                else
                {
                    DisplayGraph(); // If 'is_MM' is false, call DisplayGraph with no parameters
                }
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
                _axisValue = 1;
                if (is_MM) // Check if 'is_MM' is true
                {
                    DisplayGraph(true); // If 'is_MM' is true, call DisplayGraph with true
                }
                else
                {
                    DisplayGraph(); // If 'is_MM' is false, call DisplayGraph with no parameters
                }
            }
            else
            {
                tbAxisY.Checked = true;
            }
        }

        private void tbViewGraph_Click(object sender, EventArgs e)
        {
            toolStripSplitButton1.Enabled = true;
            ResetToolStripSplitButton1();
            if (boreHoleSelected == 0)
                return;
            if (lstBoreholes.SelectedItems.Count == 0)
                return;
            if (lstBoreholes.SelectedItems.Count > 7)
            {
                Interaction.MsgBox("You have selected " + lstBoreholes.SelectedItems.Count + " files. You can select maximum 7 files for plotting graph", Constants.vbOKOnly | Constants.vbExclamation, "Graph");
                return;
            }
            DataGridView1.Visible = false;
            DisplayGraph();
        }

        private void ResetLabels()
        {
            Console.WriteLine("Inside ResetLabels function");
            Label1.Text = "";
            Label2.Text = "";
            Label3.Text = "";
            Label4.Text = "";
            Label5.Text = "";
            label7.Text = "";
            label8.Text = "";

            //Label6.Text = @"View Graph of one or multiple files.";
            Label6.Text = "";


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
                if (!string.IsNullOrEmpty(label7.Text))
                    bsTextPrintData += ", " + label7.Text;
                if (!string.IsNullOrEmpty(label8.Text))
                    bsTextPrintData += ", " + label8.Text;
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
            ResetLabels();
            DataGridView1.Visible = false;
            CartesianChart1.Visible = false;
            ResetToolStripSplitButton1();
            toolStripSplitButton1.Enabled = false;
            DisplayReport();
        }
        //private void tbBaseFile_Click(object sender, EventArgs e)
        //{
        //    listBH[bhIndex].BaseFile = Conversions.ToString(lstBoreholes.SelectedItem);
        //    var tmp = listBH;
        //    var argbh = tmp[bhIndex];
        //    GlobalCode.UpdateBorehole(ref argbh);
        //    tmp[bhIndex] = argbh;
        //    ReloadList();
        //}

        private void tbBaseFile_Click(object sender, EventArgs e)
        {
            // Get the selected item from the list
            string selectedBaseFile = Conversions.ToString(lstBoreholes.SelectedItem);
            // Create a temporary copy of the list
            var tmp = listBH;
            // Get the Borehole object at the current index
            var argbh = tmp[bhIndex];
            // Check if the selected item is the same as the current BaseFile
            if (string.Equals(argbh.BaseFile, selectedBaseFile))
            {
                // If it is the same, "deselect" the BaseFile (set it to null or an empty string)
                if (argbh.BaseFile != null) // Check if the BaseFile was previously present
                {
                    // Ask the user if they really want to remove the BaseFile
                    DialogResult dialogResult = MessageBox.Show("Are you sure you want to remove the BaseFile?", "Confirm", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        argbh.BaseFile = null; // You can also use String.Empty if you prefer
                        MessageBox.Show("The BaseFile has been removed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); // Notify the user
                    }
                }
            }
            else
            {
                // If it is different, update the Borehole object with the new BaseFile value
                argbh.BaseFile = selectedBaseFile;
            }
            // Call the UpdateBorehole method to update the borehole information
            GlobalCode.UpdateBorehole(ref argbh);
            // Update the Borehole object in the temporary list
            tmp[bhIndex] = argbh;
            // Reload the list with the updated data
            ReloadList();
        }



        private void ListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            if (e.Index < 0)
                return;

            // Define the default color of the brush as black.
            //var myBrush = System.Drawing.Brushes.Beige;
            var myBrush = System.Drawing.Brushes.DarkCyan;

            if (bhIndex >= 0)
            {
                if (CultureInfo.CurrentCulture.CompareInfo.Compare(listBH[bhIndex].BaseFile ?? "", lstBoreholes.Items[e.Index].ToString() ?? "", CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) == 0)
                {
                    myBrush = System.Drawing.Brushes.OrangeRed;
                }
                else
                {
                    myBrush = System.Drawing.Brushes.Black;
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

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        // Initial state
        bool is_MM = true;
        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            // Toggle the state
            is_MM = !is_MM;

            // Properties - Update the button text,color based on the current state
            toolStripSplitButton1.Text = is_MM ? "MM" : "DEG";
            toolStripSplitButton1.BackgroundImage = new Bitmap(1, 1);
            toolStripSplitButton1.BackgroundImageLayout = ImageLayout.None;
            toolStripSplitButton1.BackColor = is_MM ? Color.Cyan : Color.LightGreen;//previously LightBlue instead of Cyan

            // Check if a base file is selected
            if (listBH[bhIndex].BaseFile is null || string.IsNullOrEmpty(listBH[bhIndex].BaseFile))
            {
                //Interaction.MsgBox("No base file selected for this borehole. Go back and select a base file to view deviation.", Constants.vbOKOnly | Constants.vbExclamation, "Graph");
                //DisplayGraph(); //display graph in mm 
                //return;
            }

            // Handle the behavior based on the current state
            if (is_MM)
            {
                // Reset labels and hide chart and DataGridView
                ResetLabels();
                CartesianChart1.Visible = false;
                DataGridView1.Visible = false;
                //ToolStrip2.Enabled = false;
                // Get the path to the base file
                string strFileBase = GlobalCode.GetBoreholeDirectory(ref boreHoleSelected) + @"\" + listBH[bhIndex].BaseFile;

                //if (!System.IO.File.Exists(strFileBase))//if making the BaseFile concrete function then uncomment this.
                //{
                //    Interaction.MsgBox("Base file does not exist. It must have been removed. Please select any file as a base file.", Constants.vbOKOnly | Constants.vbExclamation, "Graph");
                //    //return; //uncomment this return statment.
                //}
                //else
                //{// Handle the 'deg' state
                //    //MessageBox.Show($"Showing graph in Degree", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    DisplayGraph(true);
                //}

                DisplayGraph(true);
            }
            else
            {
                //MessageBox.Show($"Showing graph in mm", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Handle the 'mm' state
                DisplayGraph();
                
            }
        }
        private void ResetToolStripSplitButton1()
        {
            // Reset the state
            is_MM = false; // or true, depending on what you consider the initial state

            // Reset properties - Update the button text, color based on the reset state
            toolStripSplitButton1.Text = is_MM ? "MM" : "DEG";
            toolStripSplitButton1.BackgroundImage = null; // or set to initial image
            toolStripSplitButton1.BackgroundImageLayout = ImageLayout.None;
            toolStripSplitButton1.BackColor = is_MM ? Color.Cyan : Color.LightGreen; // or set to initial color

            // Reset other properties and states as needed...
        }

        private void degToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Get the path to the base file
            //string strFileBase = GlobalCode.GetBoreholeDirectory(ref boreHoleSelected) + @"\" + listBH[bhIndex].BaseFile;
            //if (!System.IO.File.Exists(strFileBase))//if making the BaseFile concrete function then uncomment this.
            //{
            //    Interaction.MsgBox("Base file does not exist. It may have been removed or deleted .Please select another file as a base to view Degree Graph.", Constants.vbOKOnly | Constants.vbExclamation, "Graph");                //return; //uncomment this return statment.
            //}
            //else
            //{// Handle the 'deg' state        
            //    DisplayGraph(true);
            //    toolStripSplitButton1.Text = is_MM ? "MM" : "DEG";
            //    toolStripSplitButton1.BackColor = is_MM ? Color.Cyan : Color.LightGreen;
            //}

            DisplayGraph();
            toolStripSplitButton1.Text = is_MM ? "MM" : "DEG";
            //toolStripSplitButton1.Text = is_MM ? "DEG" : "MM";
            toolStripSplitButton1.BackColor = is_MM ? Color.Cyan : Color.LightGreen;
            ResetToolStripSplitButton1();
        }


        private void mMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayGraph(true);
            //toolStripSplitButton1.Text = is_MM ? "MM" : "DEG";
            
            toolStripSplitButton1.Text = is_MM ? "DEG" : "MM";
            toolStripSplitButton1.BackColor = is_MM ? Color.Cyan : Color.LightGreen;
            ResetToolStripSplitButton1();
        }
    }
}