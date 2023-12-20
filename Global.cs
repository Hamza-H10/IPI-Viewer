using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace InclinoRS485
{
    static class GlobalCode
    {
        public class BoreHole
        {  
            public short Id;
            public string SiteName;
            public string Location;
           // public string DateTime;
           // public int sensor;
            public float Depth;
            public string BaseFile;
        }

        private static SQLiteConnection sqlite_conn;         
        private static SQLiteCommand sqlite_cmd;
        private static SQLiteDataReader sqlite_datareader;
        private static SQLiteDataAdapter sqliteAdapter;
        private static string myCustomFolderPath;

        public static void OpenDatabase()
        {
            Console.WriteLine("Now Inside the OpenDataBase() function");

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Console.WriteLine("Local User App Data Path: " + appDataPath);
            // create a new database connection: with file data.sqlite
            sqlite_conn = new SQLiteConnection("Data Source=" + Application.LocalUserAppDataPath + @"\data.sqlite;Version=3;");

            //---------------------------------------------------------------------------------------------------------------------------------------         
            // Example: Get the path to the My Documents folder
            /* string myCustomFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
             Console.WriteLine("My Documents Path: " + myCustomFolderPath);
             sqlite_conn = new SQLiteConnection("Data Source=" + myCustomFolderPath + "\\data.sqlite;Version=3;");*/
            //---------------------------------------------------------------------------------------------------------------------------------------
            /*string appFolderPath = AppDomain.CurrentDomain.BaseDirectory;
            string dbFilePath = Path.Combine(appFolderPath, "data.sqlite");
            sqlite_conn = new SQLiteConnection("Data Source=" + dbFilePath + ";Version=3;");*/

            // open the connection:
            sqlite_conn.Open();

            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText = @"CREATE TABLE IF NOT EXISTS
              [Boreholes] (
              [Id]       INTEGER NOT NULL PRIMARY KEY,
              [SITENAME] VARCHAR(256)  NULL,
              [LOCATION] VARCHAR(256)  NULL,
              [DEPTH]    DOUBLE(10,4) NOT NULL,
              [BASEFILE] VARCHAR(256))";
            //[DateTime] DATETIME  null,
            //[sensor] integer null,

            // Now lets execute the SQL ;-)
            sqlite_cmd.ExecuteNonQuery();
        }

        public static void CloseDatabase()
        {
            Console.WriteLine("Inside CloseDatabase");
            sqlite_cmd.Dispose();
            sqlite_conn.Close();
        }

        public static bool AddBorehole(ref BoreHole bh)
        {
            short result;
            //******************************************
           
            sqlite_cmd.CommandText = @" INSERT OR REPLACE INTO Boreholes (
                [Id], [SITENAME], [LOCATION], [DEPTH], [BASEFILE] )

              VALUES (@ID, @SiteName, @Location,  @Depth, '')";
            //[DateTime],[sensor],
            //@DateTime, @sensor,
            sqlite_cmd.Parameters.AddWithValue("@ID", bh.Id);
            sqlite_cmd.Parameters.AddWithValue("@SiteName", bh.SiteName);
            sqlite_cmd.Parameters.AddWithValue("@Location", bh.Location);
            //sqlite_cmd.Parameters.AddWithValue("@DateTime", bh.DateTime);
            //sqlite_cmd.Parameters.AddWithValue("@sensor", bh.sensor);
            sqlite_cmd.Parameters.AddWithValue("@Depth", bh.Depth);
            try
            {
                result = (short)sqlite_cmd.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool UpdateBorehole(ref BoreHole bh)
        {
            short result;
            bool bnAddBaseFile = true;
            if (bh.BaseFile.Length < 2)
                bnAddBaseFile = false;

            if (bnAddBaseFile)
            {
                sqlite_cmd.CommandText = @" UPDATE Boreholes SET [SITENAME]=@SiteName, [LOCATION]=@Location,  [DEPTH]=@Depth, [BASEFILE]=@BaseFile 
                
              WHERE [Id]=@ID";
                //[DateTime]=@DateTime, [sensor]=@sensor,
            }
            else
            {
                sqlite_cmd.CommandText = " UPDATE Boreholes SET [SITENAME]=@SiteName, [LOCATION]=@Location,  [DEPTH]=@Depth WHERE [Id]=@ID";
                //[DateTime]=@DateTime, [sensor]=@sensor,
            }
            sqlite_cmd.Parameters.AddWithValue("@ID", bh.Id);
            sqlite_cmd.Parameters.AddWithValue("@SiteName", bh.SiteName);
            sqlite_cmd.Parameters.AddWithValue("@Location", bh.Location);
            //sqlite_cmd.Parameters.AddWithValue("@DateTime", bh.DateTime);
            //sqlite_cmd.Parameters.AddWithValue("@sensor", bh.sensor);
            sqlite_cmd.Parameters.AddWithValue("@Depth", bh.Depth);
            if (bnAddBaseFile)
            {
                sqlite_cmd.Parameters.AddWithValue("@BaseFile", bh.BaseFile);
            }
            try
            {
                result = (short)sqlite_cmd.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static short DeleteBorehole(ref short id)// check this function 
        {
            sqlite_cmd.CommandText = " DELETE FROM Boreholes WHERE Id=" + id;
            return (short)sqlite_cmd.ExecuteNonQuery();
        }

        public static short _DeleteAllBoreholes()
        {
            sqlite_cmd.CommandText = " DELETE FROM Boreholes";
            return (short)sqlite_cmd.ExecuteNonQuery();
        }

        public static List<BoreHole> GetBoreholes()
        {
            var bh = new List<BoreHole>();
            sqlite_cmd.CommandText = "SELECT Id, SITENAME, LOCATION,  DEPTH, BASEFILE FROM Boreholes ORDER BY Id";
            //DateTime, sensor,

            sqlite_datareader = sqlite_cmd.ExecuteReader();

            while (sqlite_datareader.Read())


                bh.Add(new BoreHole() { Id = Conversions.ToShort(sqlite_datareader.GetValue(0)), SiteName = Conversions.ToString(sqlite_datareader.GetValue(1)), Location = Conversions.ToString(sqlite_datareader.GetValue(2)), Depth = Conversions.ToSingle(sqlite_datareader.GetValue(3)), BaseFile = Conversions.ToString(Operators.ConcatenateObject("", sqlite_datareader.GetValue(4))) });
            //DateTime = Conversions.ToString(sqlite_datareader.GetValue(3)), sensor = Conversions.ToInteger(sqlite_datareader.GetValue(4)), 
            sqlite_datareader.Close();
            return bh;
        }
        //-----------------------------------------------------------------------------------------------------------------------------

        //----------------------------------------------------------------------------------------------------------------------------
        public static string[][] ReadCSVFile(ref string FileName)
        {
            string[][] ReadCSVFileRet = default; // Declare a string array (string[][]) to store the result
            var data = new List<string[]>(); // Create a list to hold string arrays

            Console.WriteLine("INSIDE READ CSV"); // Print a message to the console

            try
            {
                using (var MyReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(FileName))
                {
                    MyReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited; // Set the text field type to delimited
                    MyReader.SetDelimiters(","); // Set the delimiter to a comma (',')

                    while (!MyReader.EndOfData) // Continue reading until the end of the file is reached
                    {
                        try//in the orginal code of C# the caret returns from the data.Add(split); line. It dosent leave the try block
                        {
                            string[] split = MyReader.ReadFields(); // Read a line and split it into an array of strings
                            data.Add(split); // Add the array to the list
                        }//at this point the original code base make display the whole report
                        catch (Microsoft.VisualBasic.FileIO.MalformedLineException ex)
                        {
                            Console.WriteLine("inside Catch block of MyReader");
                            // Handle a malformed line exception (if encountered)
                            // In this case, the line is considered invalid and skipped
                            ReadCSVFileRet = null; // Set the result to null
                        }
                    }
                    //Console.WriteLine("ReadCSVFileRet:" + ReadCSVFileRet);

                    //printing the elements of the file(printing Raw data) 
                    foreach (string[] row in data)
                    {
                        foreach (string cell in row)
                        {
                            Console.Write(cell + "\t"); // Print the cell value followed by a tab
                        }
                        Console.WriteLine(); // Move to the next line for the next row
                    }
                }

                return data.ToArray(); // Convert the list of string arrays to a 2D string array and return it
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that may occur during file reading
                Interaction.MsgBox(ex.Message, Constants.vbOKOnly | Constants.vbExclamation, "File Read"); // Show a message box with the error message
                return null; // Return null to indicate an error
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------
        public static string GetBoreholeDirectory(ref short bhnum)
        {
            //Console.WriteLine("Inside GetBoreholeDirectory");
            return Application.LocalUserAppDataPath + @"\" + bhnum.ToString().PadLeft(2, '0');
            //return myCustomFolderPath + @"\" + bhnum.ToString().PadLeft(2, '0');
        }
    }
}