﻿Public Class About
    Private Sub About_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Panel1.Top = (Me.Height - Panel1.Height) / 2
        Panel1.Left = (Me.Width - Panel1.Width) / 2
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Dim url As String = "https://www.facebook.com/FamousBelgianWafflesOFFICIAL/"
        Process.Start(url)
    End Sub
    Private Sub PictureBox5_Click(sender As Object, e As EventArgs) Handles PictureBox5.Click

    End Sub
    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs)
        Dim url As String = "https://famousbelgianwaffles.com/"
        Process.Start(url)
    End Sub
End Class