﻿Imports MySql.Data.MySqlClient
Imports System.Threading
Public Class Auth
    Dim threadList As List(Of Thread) = New List(Of Thread)
    Dim thread As Thread
    Dim UserCount As Integer = 0
    Dim Account
    Dim TimerCount As Integer = 0
    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Enum ProgressBarColor
        Green = &H1
        Red = &H2
        Yellow = &H3
    End Enum
    Private Shared Sub ChangeProgBarColor(ByVal ProgressBar_Name As System.Windows.Forms.ProgressBar, ByVal ProgressBar_Color As ProgressBarColor)
        SendMessage(ProgressBar_Name.Handle, &H410, ProgressBar_Color, 0)
    End Sub
    Private Sub Auth_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        CheckForIllegalCrossThreadCalls = False
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.RunWorkerAsync()
        ChangeProgBarColor(ProgressBar1, ProgressBarColor.Yellow)
    End Sub
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            ProgressBar1.Maximum = 100
            ProgressBar1.Value = 0
            For i = 0 To 100
                BackgroundWorker1.ReportProgress(i)
                Thread.Sleep(50)
                If i = 0 Then
                    thread = New Thread(AddressOf SyncToLocalUsers)
                    thread.Start()
                    threadList.Add(thread)
                End If
                If i = 50 Then
                    Label4.Text = "Checking user(s) availability...  "
                End If
            Next
            For Each t In threadList
                t.Join()
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub SyncToLocalUsers()
        UserCount = 0
        Try
            With DataGridViewRESULT
                sql = "SELECT `user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `active`, `guid`, `store_id`, `uniq_id` , `created_at`, `updated_at` , `pwd` FROM `loc_users` WHERE guid = '" & ClientGuid & "' AND store_id = '" & ClientStoreID & "' AND synced = 'Unsynced' AND active = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, ServerCloudCon())
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim DataTableServer As DataTable = New DataTable
                da.Fill(DataTableServer)
                .DataSource = DataTableServer
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Account += "Username: " & .Rows(i).Cells(2).Value.ToString & vbNewLine & "Password: " & .Rows(i).Cells(14).Value.ToString & vbNewLine & vbNewLine
                    UserCount = UserCount + 1
                    table = "triggers_loc_users"
                    fields = "(`user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `active`, `guid`, `store_id`, `uniq_id`, `synced`)"
                    value = "(
                         '" & .Rows(i).Cells(0).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(1).Value.ToString & "'    
                         ,'" & .Rows(i).Cells(2).Value.ToString & "'                 
                         ,'" & .Rows(i).Cells(3).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(4).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(5).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(6).Value.ToString & "'                   
                         ,'" & .Rows(i).Cells(7).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(8).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(9).Value.ToString & "'    
                         ,'" & .Rows(i).Cells(10).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(11).Value.ToString & "'       
                         ,'Unsynced')"
                    GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, successmessage:=successmessage, errormessage:=errormessage)
                    sql = "UPDATE loc_users SET synced = 'Synced' WHERE uniq_id = '" & .Rows(i).Cells(11).Value.ToString & "'"
                    cmd = New MySqlCommand(sql, ServerCloudCon())
                    cmd.ExecuteNonQuery()
                    sql = "UPDATE loc_users SET `full_name` = '" & .Rows(i).Cells(1).Value.ToString & "'  , `username` = '" & .Rows(i).Cells(2).Value.ToString & "' , `password` = '" & .Rows(i).Cells(3).Value.ToString & "'  , `contact_number` = '" & .Rows(i).Cells(4).Value.ToString & "'  WHERE uniq_id = '" & .Rows(i).Cells(11).Value.ToString & "' "
                    cmd = New MySqlCommand(sql, LocalhostConn())
                    cmd.ExecuteNonQuery()
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
        Label2.Text = e.ProgressPercentage
    End Sub
    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        ProgressBar1.Value = 100
        If UserCount = 0 Then
            Label4.Text = "No new user(s) available.  "
            Timer1.Start()
        Else
            Label4.Text = "New user(s) available.  "
            Dim msg = "New user account has been added" & vbNewLine & Account
            Dim msgs = MsgBox(msg, MsgBoxStyle.OkOnly)
            If msgs = DialogResult.OK Then
                Timer1.Start()
            End If
        End If
    End Sub
    Private Sub Auth_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Login.Enabled = True
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            TimerCount = TimerCount + 1
            If TimerCount = 1 Then
                Label1.Text = "Closing in 3."
            ElseIf TimerCount = 2 Then
                Label1.Text = "Closing in 2."
            ElseIf TimerCount = 3 Then
                Label1.Text = "Closing in 1."
            ElseIf TimerCount = 4 Then
                Close()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class