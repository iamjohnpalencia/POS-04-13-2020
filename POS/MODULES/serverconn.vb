Imports MySql.Data.MySqlClient
Module serverlocalconn
    Dim ConnStr As String
    Dim ConnStr2 As String
    'Public Sub serverconn()
    '    Try
    '        sql = "SELECT `C_Server`, `C_Username`, `C_Password`, `C_Database`, `C_Port` FROM  loc_settings WHERE settings_id = 1"
    '        Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
    '        Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
    '        Dim dt As DataTable = New DataTable
    '        da.Fill(dt)
    '        CloudConnectionString = "server=" & ConvertB64ToString(dt(0)(0)) &
    '        ";userid= " & ConvertB64ToString(dt(0)(1)) &
    '        ";password=" & ConvertB64ToString(dt(0)(2)) &
    '        ";port=" & ConvertB64ToString(dt(0)(4)) &
    '        ";database=" & ConvertB64ToString(dt(0)(3))
    '        cloudconn = New MySqlConnection
    '        cloudconn.ConnectionString = CloudConnectionString
    '        cloudconn.Open()
    '        If cloudconn.State = ConnectionState.Open Then
    '            If My.Settings.ValidCloudConn = False Then
    '                My.Settings.ValidCloudConn = True
    '                My.Settings.Save()
    '            End If
    '        End If
    '    Catch ex As MySqlException
    '        My.Settings.ValidCloudConn = False
    '        My.Settings.Save()
    '    End Try
    'End Sub
    Public Function ServerCloudCon()
        Dim servercloudconn As MySqlConnection
        servercloudconn = New MySqlConnection
        Try
            sql = "SELECT `C_Server`, `C_Username`, `C_Password`, `C_Database`, `C_Port` FROM  loc_settings WHERE settings_id = 1"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn())
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            CloudConnectionString = "server=" & ConvertB64ToString(dt(0)(0)) &
            ";userid= " & ConvertB64ToString(dt(0)(1)) &
            ";password=" & ConvertB64ToString(dt(0)(2)) &
            ";port=" & ConvertB64ToString(dt(0)(4)) &
            ";database=" & ConvertB64ToString(dt(0)(3))
            servercloudconn.ConnectionString = CloudConnectionString
            servercloudconn.Open()
            If servercloudconn.State = ConnectionState.Open Then
                If My.Settings.ValidCloudConn = False Then
                    My.Settings.ValidCloudConn = True
                    My.Settings.Save()
                End If
            End If
        Catch ex As Exception
            My.Settings.ValidCloudConn = False
            My.Settings.Save()
        End Try
        Return servercloudconn
    End Function
End Module
