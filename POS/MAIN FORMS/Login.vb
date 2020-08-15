
Imports MySql.Data.MySqlClient

Public Class Login
    Declare Function Wow64DisableWow64FsRedirection Lib "kernel32" (ByRef oldvalue As Long) As Boolean
    Private osk As String = "C:\Windows\System32\osk.exe"
    Private Sub Login_Load_1(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckDatabaseBackup()
        txtusername.Focus()
        Timer1.Enabled = True
        ButttonLogin.Text = "LOGIN (" & ClientStorename & ")"
    End Sub
    Public Function FirstDayOfMonth(ByVal sourceDate As DateTime)
        Dim FirstDay As DateTime = New DateTime(sourceDate.Year, sourceDate.Month, 1)
        Dim FormatDay As String = "yyyy-MM-dd"
        Dim displaythis = FirstDay.ToString(FormatDay)
        Return displaythis
    End Function
    Private Sub BackupDatabase()
        Try
            Dim DatabaseName = "\POS" & Format(Now(), "yyyy-MM-dd") & ".sql"
            Process.Start("cmd.exe", "/k cd C:\xampp\mysql\bin & mysqldump --databases -h " & connectionModule.LocServer & " -u " & connectionModule.LocUser & " -p " & connectionModule.LocPass & " " & connectionModule.LocDatabase & " > """ & S_ExportPath & DatabaseName & """")
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub CheckDatabaseBackup()
        Try
            If S_Backup_Interval = "1" Then
                'Daily
                If S_Backup_Date <> Format(Now(), "yyyy-MM-dd") Then
                    S_Backup_Date = Format(Now(), "yyyy-MM-dd")
                    BackupDatabase()
                    Dim sql As String = "UPDATE loc_settings SET S_BackupDate = '" & S_Backup_Date & "' WHERE settings_id = 1"
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Database backup path: " & S_ExportPath, "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            ElseIf S_Backup_Interval = "2" Then
                'Weekly
                S_Backup_Date = Format(DateAdd("d", 7, S_Zreading), "yyyy-MM-dd")
                If S_Backup_Date = Format(Now(), "yyyy-MM-dd") Then
                    S_Backup_Date = Format(Now(), "yyyy-MM-dd")
                    BackupDatabase()
                    Dim sql As String = "UPDATE loc_settings SET S_BackupDate = '" & S_Backup_Date & "' WHERE settings_id = 1"
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Database backup path: " & S_ExportPath, "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            ElseIf S_Backup_Interval = "3" Then
                'Monthly
                If FirstDayOfMonth(Now()) = Format(Now(), "yyyy-MM-dd") Then
                    S_Backup_Date = Format(Now(), "yyyy-MM-dd")
                    BackupDatabase()
                    Dim sql As String = "UPDATE loc_settings SET S_BackupDate = '" & S_Backup_Date & "' WHERE settings_id = 1"
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                    cmd.ExecuteNonQuery()
                    MessageBox.Show("Database backup path: " & S_ExportPath, "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Dim sql As String = "SELECT S_BackupDate FROM loc_settings WHERE settings_id = 1"
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                    Dim result = cmd.ExecuteScalar()
                    If FirstDayOfMonth(Now) <> result Then
                        S_Backup_Date = FirstDayOfMonth(Now)
                        BackupDatabase()
                        Dim sql1 As String = "UPDATE loc_settings SET S_BackupDate = '" & S_Backup_Date & "' WHERE settings_id = 1"
                        Dim cmd1 As MySqlCommand = New MySqlCommand(sql1, LocalhostConn)
                        cmd1.ExecuteNonQuery()
                        MessageBox.Show("Database backup path: " & S_ExportPath, "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            ElseIf S_Backup_Interval = "4" Then
                Dim YearNow As Integer = Date.Now.Year
                If YearNow <> Format(S_Backup_Date, ("yyyy")) Then
                    S_Backup_Date = Format(Now(), "yyyy-MM-dd")
                    BackupDatabase()
                    Dim sql1 As String = "UPDATE loc_settings SET S_BackupDate = '" & S_Backup_Date & "' WHERE settings_id = 1"
                    Dim cmd1 As MySqlCommand = New MySqlCommand(sql1, LocalhostConn)
                    cmd1.ExecuteNonQuery()
                    MessageBox.Show("Database backup path: " & S_ExportPath, "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                'Yearly
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub ButttonLogin_Click(sender As Object, e As EventArgs) Handles ButttonLogin.Click
        retrieveLoginDetails()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        If MessageBox.Show("Are you sure you really want to exit ?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            Application.Exit()
        End If
    End Sub
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Me.Hide()
        Registration.Show()
    End Sub

    Private Sub txtpassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtpassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            ButttonLogin.PerformClick()
        End If
    End Sub
    Private Sub txtusername_KeyDown(sender As Object, e As KeyEventArgs) Handles txtusername.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtpassword.Focus()
        End If
    End Sub
    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        Application.Exit()
    End Sub
    Dim bat As String
    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        If CheckForInternetConnection() Then
            Enabled = False
            Auth.Show()
        Else
            MessageBox.Show("Cannot connect to cloud server please try again.", "No internet connection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
    Private Sub PopupKeyboard(sender As Object, e As EventArgs) Handles txtusername.Click, txtpassword.Click
        Wow64DisableWow64FsRedirection(0)
        Process.Start(osk)
    End Sub
    Private Sub txtusername_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtusername.KeyPress, txtpassword.KeyPress
        Try
            If InStr(DisallowedCharacters, e.KeyChar) > 0 Then
                e.Handled = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class