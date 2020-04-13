Public Class LoadSettings
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label1.Text = Date.Now.ToString("yyyy-MM-dd hh:mm:ss tt")
    End Sub
    Private Sub LoadSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label1.Text = Date.Now.ToString("yyyy-MM-dd hh:mm:ss tt")
        Timer1.Start()
    End Sub
End Class