﻿Public Class SeniorDetails
    Private Sub ButtonCANCEL_Click(sender As Object, e As EventArgs) Handles ButtonCANCEL.Click
        Close()
    End Sub
    Private Sub SeniorDetails_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        CouponCode.Enabled = True
    End Sub
    Private Sub ButtonSubmit_Click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click
        Try
            SeniorDetailsID = Trim(TextBoxSENIORID.Text)
            SeniorDetailsName = Trim(TextBoxSENIORNAME.Text)
            Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class