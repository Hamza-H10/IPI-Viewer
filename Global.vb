Imports System.Data.SQLite

Module GlobalCode
    Public Class BoreHole
        Public Id As Short
        Public SiteName As String
        Public Location As String
        Public Depth As Single
        Public BaseFile As String
    End Class

    Dim sqlite_conn As SQLiteConnection
    Dim sqlite_cmd As SQLiteCommand
    Dim sqlite_datareader As SQLiteDataReader
    Dim sqliteAdapter As SQLiteDataAdapter

    Sub OpenDatabase()

        ' create a new database connection: with file data.sqlite
        sqlite_conn = New SQLiteConnection("Data Source=" & Application.LocalUserAppDataPath & "\data.sqlite;Version=3;")

        ' open the connection:
        sqlite_conn.Open()

        sqlite_cmd = sqlite_conn.CreateCommand()

        sqlite_cmd.CommandText =
           "CREATE TABLE IF NOT EXISTS
              [Boreholes] (
              [Id]       INTEGER NOT NULL PRIMARY KEY,
              [SITENAME] VARCHAR(256) NULL,
              [LOCATION] VARCHAR(256) NULL,
              [DEPTH]    DOUBLE(10,4) NOT NULL,
              [BASEFILE] VARCHAR(256))"

        ' Now lets execute the SQL ;-)
        sqlite_cmd.ExecuteNonQuery()
    End Sub

    Sub CloseDatabase()
        sqlite_cmd.Dispose()
        sqlite_conn.Close()
    End Sub

    Function AddBorehole(ByRef bh As BoreHole) As Boolean
        Dim result As Short
        sqlite_cmd.CommandText =
            " INSERT INTO Boreholes (
                [Id], [SITENAME], [LOCATION], [DEPTH], [BASEFILE] )
              VALUES (@ID, @SiteName, @Location, @Depth, '')"
        sqlite_cmd.Parameters.AddWithValue("@ID", bh.Id)
        sqlite_cmd.Parameters.AddWithValue("@SiteName", bh.SiteName)
        sqlite_cmd.Parameters.AddWithValue("@Location", bh.Location)
        sqlite_cmd.Parameters.AddWithValue("@Depth", bh.Depth)
        Try
            result = sqlite_cmd.ExecuteNonQuery()
        Catch
            Return False
        End Try
        Return True
    End Function

    Function UpdateBorehole(ByRef bh As BoreHole) As Boolean
        Dim result As Short, bnAddBaseFile As Boolean = True
        If bh.BaseFile.Length < 2 Then bnAddBaseFile = False

        If bnAddBaseFile Then
            sqlite_cmd.CommandText =
            " UPDATE Boreholes SET [SITENAME]=@SiteName, [LOCATION]=@Location, [DEPTH]=@Depth, [BASEFILE]=@BaseFile 
              WHERE [Id]=@ID"
        Else
            sqlite_cmd.CommandText =
            " UPDATE Boreholes SET [SITENAME]=@SiteName, [LOCATION]=@Location, [DEPTH]=@Depth WHERE [Id]=@ID"
        End If
        sqlite_cmd.Parameters.AddWithValue("@ID", bh.Id)
        sqlite_cmd.Parameters.AddWithValue("@SiteName", bh.SiteName)
        sqlite_cmd.Parameters.AddWithValue("@Location", bh.Location)
        sqlite_cmd.Parameters.AddWithValue("@Depth", bh.Depth)
        If bnAddBaseFile Then
            sqlite_cmd.Parameters.AddWithValue("@BaseFile", bh.BaseFile)
        End If
        Try
            result = sqlite_cmd.ExecuteNonQuery()
        Catch
            Return False
        End Try
        Return True
    End Function

    Function DeleteBorehole(ByRef id As Short) As Short
        sqlite_cmd.CommandText = " DELETE FROM Boreholes WHERE Id=" & id
        Return sqlite_cmd.ExecuteNonQuery()
    End Function

    Function _DeleteAllBoreholes() As Short
        sqlite_cmd.CommandText = " DELETE FROM Boreholes"
        Return sqlite_cmd.ExecuteNonQuery()
    End Function

    Function GetBoreholes() As List(Of BoreHole)
        Dim bh As New List(Of BoreHole)
        sqlite_cmd.CommandText = "SELECT Id, SITENAME, LOCATION, DEPTH, BASEFILE FROM Boreholes ORDER BY Id"

        sqlite_datareader = sqlite_cmd.ExecuteReader()

        Do While (sqlite_datareader.Read())

            bh.Add(New BoreHole With {.Id = sqlite_datareader.GetValue(0), .SiteName = sqlite_datareader.GetValue(1), .Location = sqlite_datareader.GetValue(2), .Depth = sqlite_datareader.GetValue(3), .BaseFile = "" & sqlite_datareader.GetValue(4)})

        Loop
        sqlite_datareader.Close()
        Return bh
    End Function

    Function ReadCSVFile(ByRef FileName As String) As String()()
        Dim data As New List(Of String())()

        Try
            Using MyReader As New FileIO.TextFieldParser(FileName)
                MyReader.TextFieldType = FileIO.FieldType.Delimited
                MyReader.SetDelimiters(",")

                While Not MyReader.EndOfData
                    Try
                        Dim split As String() = MyReader.ReadFields()
                        data.Add(split)
                    Catch ex As FileIO.MalformedLineException
                        'MsgBox("Line " & ex.Message &
                        '"is not valid and will be skipped.")
                        ReadCSVFile = Nothing
                    End Try
                End While
            End Using
            Return data.ToArray()
        Catch ex As System.Exception
            MsgBox(ex.Message, vbOKOnly Or vbExclamation, "File Read")
            Return Nothing
        End Try
    End Function

    Function GetBoreholeDirectory(ByRef bhnum As Short) As String
        Return Application.LocalUserAppDataPath & "\" & bhnum.ToString().PadLeft(2, "0")
    End Function

End Module
