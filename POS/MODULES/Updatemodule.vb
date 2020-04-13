Imports System.Management
Imports System.Management.Instrumentation
Imports System
Imports System.IO
Imports MySql.Data.MySqlClient
Module Updatemodule
    Dim result As Integer
    Dim stockqty
    Dim stocktotal
    Public Sub GLOBAL_FUNCTION_UPDATE(ByVal table, ByVal fields, ByVal where)
        Try
            dbconnection()
            sql = "UPDATE " + table + " SET " + fields + " WHERE " & where
            cmd = New MySqlCommand
            With cmd
                .Connection = localconn
                .CommandText = sql
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            localconn.Close()
            cmd.Dispose()
        End Try
    End Sub
    Public Sub GLOBAL_FUNCTION_UPDATE_CLOUD(ByVal table, ByVal fields, ByVal where)
        Try
            MsgBox(cloudconn.State)
            If cloudconn.State = ConnectionState.Closed Then
                serverconn()
            End If
            sql = "UPDATE " + table + " SET " + fields + " WHERE " & where
            cloudcmd = New MySqlCommand
            With cloudcmd
                .Connection = cloudconn
                .CommandText = sql
                .ExecuteNonQuery()
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
