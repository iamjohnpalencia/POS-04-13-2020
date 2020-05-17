Imports Microsoft.VisualBasic
Public Class Login
    Private Sub Login_Load_1(sender As Object, e As EventArgs) Handles MyBase.Load
        txtusername.Focus()
        Timer1.Enabled = True
        ButttonLogin.Text = "LOGIN (" & ClientStorename & ")"
    End Sub
    'Private Sub buttonFB_Click(sender As Object, e As EventArgs) Handles buttonFB.Click
    '    System.Diagnostics.Process.Start("https://www.facebook.com/FamousBelgianWafflesOFFICIAL/")
    'End Sub
    'Private Sub buttonTWIT_Click(sender As Object, e As EventArgs) Handles buttonTWIT.Click
    '    System.Diagnostics.Process.Start("https://twitter.com")
    'End Sub
    'Private Sub buttonINST_Click(sender As Object, e As EventArgs) Handles buttonINST.Click
    '    System.Diagnostics.Process.Start("https://instagram.com")
    'End Sub
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
    Private OSK As System.Diagnostics.Process
    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        OSK = System.Diagnostics.Process.Start("osk.exe")
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)

        Try
            'Dim startInfo As System.Diagnostics.ProcessStartInfo
            'Dim pStart As New System.Diagnostics.Process
            'startInfo = New System.Diagnostics.ProcessStartInfo("C:\Users\Jay Reyes\Desktop\New folder\POS-03-15-2020\POS\bin\Debug\BATFILES\exitkeyboard.bat")

            'pStart.StartInfo = startInfo
            'pStart.Start()
            'pStart.WaitForExit()
            'Dim Proc As New System.Diagnostics.Process
            'Proc.StartInfo = New ProcessStartInfo("C:\Windows\System32\cmd.exe")
            'Proc.StartInfo.Arguments = "tskill OSK"
            ''Proc.StartInfo.CreateNoWindow = True
            'Proc.StartInfo.RedirectStandardInput = True
            'Proc.StartInfo.RedirectStandardOutput = False
            'Proc.StartInfo.UseShellExecute = False
            'Proc.Start()
            '' Allows script to execute sequentially instead of simultaneously
            'Proc.WaitForExit()
            Dim p As Process = New Process()
            Dim psi As New ProcessStartInfo()
            psi.Verb = "runas" ' aka run as administrator
            psi.FileName = "cmd.exe"
            psi.Arguments = "/c " & "tskill osk" ' <- pass arguments for the command you want to run
            p.StartInfo = psi
            p.Start()
            Try
                'Process.Start(psi) ' <- run the process (user will be prompted to run with administrator access)
            Catch
                ' exception raised if user declines the admin prompt
            End Try


        Catch ex As Exception
            Console.WriteLine(ex.StackTrace.ToString())
        End Try
    End Sub
    Dim bat As String
    'Declare Function Wow64DisableWow64FsRedirection Lib "kernel32" (ByRef oldvalue As Long) As Boolean
    'Private osk As String = "C:\Windows\System32\osk.exe"
    'Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
    '    Wow64DisableWow64FsRedirection(0)
    '    Process.Start(osk)
    'End Sub
End Class