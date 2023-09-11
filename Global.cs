using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace InclinoView
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

        public static void OpenDatabase()
        {

            // create a new database connection: with file data.sqlite
            sqlite_conn = new SQLiteConnection("Data Source=" + Application.LocalUserAppDataPath + @"\data.sqlite;Version=3;");

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
            sqlite_cmd.Dispose();
            sqlite_conn.Close();
        }

        public static bool AddBorehole(ref BoreHole bh)
        {
            short result;
            //******************************************888888
            //yaha change kiya 'or replce'. pehle nhi tha
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

        public static short DeleteBorehole(ref short id)
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

        public static string[][] ReadCSVFile(ref string FileName)//acc. to gpt 
        {
            string[][] ReadCSVFileRet = default;
            var data = new List<string[]>();
            Console.WriteLine("inside read csv");
            try
            {
                using (var MyReader = new Microsoft.VisualBasic.FileIO.TextFieldParser(FileName))
                {
                    MyReader.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
                    MyReader.SetDelimiters(",");

                    while (!MyReader.EndOfData)
                    {
                        try
                        {
                            string[] split = MyReader.ReadFields();
                            data.Add(split);
                        }
                        catch (Microsoft.VisualBasic.FileIO.MalformedLineException ex)
                        {
                             //MsgBox("Line " & ex.Message &
                             //"is not valid and will be skipped.")
                            ReadCSVFileRet = null;
                        }
                    }
                    Console.WriteLine("ReadCSVFileRet:" + ReadCSVFileRet);
                    foreach (string[] row in data)
                    {
                        foreach (string cell in row)
                        {
                            Console.Write(cell + "\t"); // Print the cell value followed by a tab
                        }
                        Console.WriteLine(); // Move to the next line for the next row
                    }
                }
                return data.ToArray();
            }
            catch (Exception ex)
            {
                Interaction.MsgBox(ex.Message, Constants.vbOKOnly | Constants.vbExclamation, "File Read");
                return null;
            }
        }
        public static string GetBoreholeDirectory(ref short bhnum)
        {
            return Application.LocalUserAppDataPath + @"\" + bhnum.ToString().PadLeft(2, '0');
        }
    }
}