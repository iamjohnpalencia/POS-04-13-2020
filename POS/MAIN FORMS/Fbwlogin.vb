Public Class Fbwlogin
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Enabled = False
        Auth.Show()
    End Sub
End Class