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
        End Try
    End Sub
    Dim result As Integer
    Public Sub GLOBAL_INSERT_FUNCTION(ByVal table As String, ByVal fields As String, ByVal values As String)
        Try
            Dim Query As String = "INSERT INTO " + table + fields + " VALUES " + values
            Dim cmd As MySqlCommand = New MySqlCommand(Query, LocalhostConn)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            LocalhostConn.close
            cmd.Dispose()
        End Try
    End Sub
End Module
