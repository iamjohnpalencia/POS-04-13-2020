Imports MySql.Data.MySqlClient
Imports System.Threading
Public Class AdminConfirmation
    Dim thread As Thread
    Dim threadList As List(Of Thread) = New List(Of Thread)
    Dim result As Integer
    Dim cipherText As String
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.RunWorkerAsync()
        Button1.Enabled = False
        'Button2.Enabled = False
    End Sub
    Private Sub retrieveadmincredentials()
        Try
            cipherText = ConvertPassword(SourceString:=TextBoxADMINPASSWORD.Text)
            sql = "SELECT user_name, user_pass FROM admin_user WHERE user_name = '" & TextBoxADMINUSERNAME.Text & "' AND user_pass = '" & cipherText & "' AND user_role = 'Admin'"
            cmd = New MySqlCommand(sql, ServerCloudCon)
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim hasinternet As Boolean
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        If CheckForInternetConnection() = True Then
            thread = New Thread(AddressOf retrieveadmincredentials)
            thread.Start()
            threadList.Add(thread)
            For Each t In threadList
                t.Join()
            Next
            If BackgroundWorker1.CancellationPending = True Then
                e.Cancel = True
            End If
            hasinternet = True
        Else
            hasinternet = False
            BackgroundWorker1.CancelAsync()
            MsgBox("No Internet Connection.")
        End If
    End Sub
    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        'If dt.Rows.Count = 1 Then
        '    MsgBox("Account Exist")
        '    Dim newMDIchild As New ConfigurationSettings()
        '    If Application.OpenForms().OfType(Of ConfigurationSettings).Any Then
        '    Else
        '        newMDIchild.MdiParent = Settings
        '        newMDIchild.ShowIcon = False
        '        newMDIchild.Show()
        '    End If
        '    cloudconn.Close()
        '    Me.Close()
        'Else
        '    If hasinternet = True Then
        '        MsgBox("Account doesnt exist")
        '    End If
        'End If
        Button1.Enabled = True
        'Button2.Enabled = True
    End Sub
End Class