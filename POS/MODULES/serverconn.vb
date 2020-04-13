Imports MySql.Data.MySqlClient
Module serverlocalconn
    Dim ConnStr As String
    Dim ConnStr2 As String
    Public Sub serverconn()
        Try
            sql = "SELECT `C_Server`, `C_Username`, `C_Password`, `C_Database`, `C_Port` FROM  loc_settings WHERE settings_id = 1"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, localconn)
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            CloudConnectionString = "server=" & ConvertB64ToString(dt(0)(0)) &
            ";userid= " & ConvertB64ToString(dt(0)(1)) &
            ";password=" & ConvertB64ToString(dt(0)(2)) &
            ";port=" & ConvertB64ToString(dt(0)(4)) &
            ";database=" & ConvertB64ToString(dt(0)(3))
            cloudconn = New MySqlConnection
            cloudconn.ConnectionString = CloudConnectionString
            cloudconn.Open()
            If cloudconn.State = ConnectionState.Open Then
                If My.Settings.ValidCloudConn = False Then
                    My.Settings.ValidCloudConn = True
                    My.Settings.Save()
                End If
            End If
        Catch ex As MySqlException
            My.Settings.ValidCloudConn = 0
            My.Settings.Save()
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Function LoadConn(Path As String)
        Try
            If My.Settings.LocalConnectionPath <> "" Then
                If System.IO.File.Exists(Path) Then
                    'The File exists 
                    Dim CreateConnString As String = ""
                    Dim filename As String = String.Empty
                    Dim TextLine As String = ""
                    Dim objReader As New System.IO.StreamReader(Path)
                    Dim lineCount As Integer
                    Do While objReader.Peek() <> -1
                        TextLine = objReader.ReadLine()
                        If lineCount = 0 Then
                            ConnStr = ConvertB64ToString(RemoveCharacter(TextLine, "server="))
                            ConnStr2 = "server=" & ConnStr
                        End If
                        If lineCount = 1 Then
                            ConnStr = ConvertB64ToString(RemoveCharacter(TextLine, "user id="))
                            ConnStr2 += ";user id=" & ConnStr
                        End If
                        If lineCount = 2 Then
                            ConnStr = ConvertB64ToString(RemoveCharacter(TextLine, "password="))
                            ConnStr2 += ";password=" & ConnStr
                        End If
                        If lineCount = 3 Then
                            ConnStr = ConvertB64ToString(RemoveCharacter(TextLine, "database="))
                            ConnStr2 += ";database=" & ConnStr
                        End If
                        If lineCount = 4 Then
                            ConnStr = ConvertB64ToString(RemoveCharacter(TextLine, "port="))
                            ConnStr2 += ";port=" & ConnStr
                        End If
                        If lineCount = 5 Then
                            ConnStr2 += ";" & TextLine
                        End If
                        lineCount = lineCount + 1
                    Loop
                    CloudConnectionString = ConnStr2
                    objReader.Close()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return ConnStr2
    End Function
End Module
