Imports MySql.Data.MySqlClient
Module Addmodule
    Public Sub GLOBAL_SYSTEM_LOGS(ByVal logtype As String, ByVal logdesc As String)
        Try
            messageboxappearance = False
            If ClientCrewID = "" Then
                ClientCrewID = 0
            End If
            messageboxappearance = False
            table = "loc_system_logs"
            fields = "(`log_type`,`crew_id`,`log_description`, `log_store`, `guid`, `loc_systemlog_id`, `synced`)"
            value = "('" & logtype & "'
                , '" & ClientCrewID & "'
                , '" & logdesc & "'
                , '" & ClientStoreID & "'
                , '" & ClientGuid & "'
                , '" & Format(Now, ("yyyyMMdd-HHmmss")) & "'
                , 'Unsynced')"
            GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, successmessage:="", errormessage:="System Logs")

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim result As Integer
    Public Sub GLOBAL_INSERT_FUNCTION(ByVal table As String, ByVal fields As String, ByVal values As String, ByVal successmessage As String, ByVal errormessage As String)
        Try
            dbconnection()
            sql = "INSERT INTO " + table + fields + " VALUES " + values
            cmd = New MySqlCommand
            With cmd
                .Connection = localconn
                .CommandText = sql
                result = .ExecuteNonQuery
            End With
            If messageboxappearance = True Then
                If result = 0 Then
                    MsgBox(errormessage)
                Else
                    MsgBox(successmessage)
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            localconn.Close()
            cmd.Dispose()
        End Try
    End Sub
    Public Sub GLOBAL_INSERT_FUNCTION_CLOUD(ByVal table As String, ByVal fields As String, ByVal values As String, ByVal successmessage As String, ByVal errormessage As String)
        Try
            If cloudconn.State = ConnectionState.Closed Then
                serverconn()
            End If
            sql = "INSERT INTO " + table + fields + " VALUES " + values
            cloudcmd = New MySqlCommand
            With cloudcmd
                .Connection = cloudconn
                .CommandText = sql
                result = .ExecuteNonQuery
            End With
            If messageboxappearance = True Then
                If result = 0 Then
                    MsgBox(errormessage)
                Else
                    MsgBox(successmessage)
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cloudconn.Close()
            cloudcmd.Dispose()
        End Try
    End Sub
End Module
