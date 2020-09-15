Imports MySql.Data.MySqlClient
Module Addmodule
    Public Sub GLOBAL_SYSTEM_LOGS(ByVal logtype As String, ByVal logdesc As String)
        Try
            If ClientCrewID = "" Then
                ClientCrewID = 0
            End If
            table = "loc_system_logs"
            fields = "(`log_type`,`crew_id`,`log_description`, `log_store`, `guid`, `loc_systemlog_id`, `synced`, `zreading`, `log_date_time`)"
            value = "('" & logtype & "'
                , '" & ClientCrewID & "'
                , '" & logdesc & "'
                , '" & ClientStoreID & "'
                , '" & ClientGuid & "'
                , '" & Format(Now, ("yyyyMMdd-HHmmss")) & "'
                , 'Unsynced'
                , '" & S_Zreading & "'
                , '" & FullDate24HR() & "')"
            GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Dim result As Integer
    Public Sub GLOBAL_INSERT_FUNCTION(ByVal table As String, ByVal fields As String, ByVal values As String)
        Try
            If LocalhostConn.State <> ConnectionState.Open Then
                LocalhostConn.Open()
            End If
            Dim Query As String = "INSERT INTO " + table + fields + " VALUES " + values
            Dim cmd As MySqlCommand = New MySqlCommand(Query, LocalhostConn)
            cmd.ExecuteNonQuery()
            LocalhostConn.close
            cmd.Dispose()
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub SendErrorReport(MSG)
        If LocalhostConn.State = ConnectionState.Open Then
            Try
                Dim Query As String = "INSERT INTO `loc_send_bug_report`(`bug_desc`, `crew_id`, `guid`, `store_id`, `date_created`, `synced`) VALUES (@1,@2,@3,@4,@5,@6)"
                Dim Command As MySqlCommand = New MySqlCommand(Query, LocalhostConn)
                Command.Parameters.Add("@1", MySqlDbType.Text).Value = MSG
                Command.Parameters.Add("@2", MySqlDbType.Text).Value = ClientCrewID
                Command.Parameters.Add("@3", MySqlDbType.Text).Value = ClientGuid
                Command.Parameters.Add("@4", MySqlDbType.Text).Value = ClientStoreID
                Command.Parameters.Add("@5", MySqlDbType.Text).Value = FullDate24HR()
                Command.Parameters.Add("@6", MySqlDbType.Text).Value = "Unsynced"
                Command.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Else
            MsgBox("Localhost connection is not valid.")
        End If
    End Sub
End Module
