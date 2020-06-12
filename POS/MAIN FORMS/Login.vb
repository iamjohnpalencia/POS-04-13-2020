
Public Class Login
    Declare Function Wow64DisableWow64FsRedirection Lib "kernel32" (ByRef oldvalue As Long) As Boolean
    Private osk As String = "C:\Windows\System32\osk.exe"
    Private Sub Login_Load_1(sender As Object, e As EventArgs) Handles MyBase.Load
        txtusername.Focus()
        Timer1.Enabled = True
        ButttonLogin.Text = "LOGIN (" & ClientStorename & ")"
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
        Enabled = False
        Auth.Show()
    End Sub
    Private Sub PopupKeyboard(sender As Object, e As EventArgs) Handles txtusername.Click, txtpassword.Click
        Wow64DisableWow64FsRedirection(0)
        Process.Start(osk)
    End Sub
End Class