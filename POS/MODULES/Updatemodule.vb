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
            sql = "UPDATE " + table + " SET " + fields + " WHERE " & where
            cmd = New MySqlCommand(sql, LocalhostConn())
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
        End Try
    End Sub
End Module
