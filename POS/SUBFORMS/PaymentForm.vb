Public Class PaymentForm
    Private Sub PaymentForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        POS.Enabled = True
        POS.BringToFront()
    End Sub
    Private Sub PaymentForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBoxMONEY.Focus()
        TextBoxTransactionType.Text = transactionmode
        TopMost = True
    End Sub
    Private Sub TextBoxMONEY_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBoxMONEY.KeyDown
        If e.KeyCode = Keys.F9 Then
            ButtonESC.PerformClick()
        ElseIf e.KeyCode = Keys.Enter Then
            ButtonSubmitPayment.PerformClick()
        End If
    End Sub
    Private Sub ButtonSubmitPayment_Click(sender As Object, e As EventArgs) Handles ButtonSubmitPayment.Click
        Try
            With POS
                If Double.Parse(TextBoxTOTALPAY.Text) > Val(TextBoxMONEY.Text) Then
                    MsgBox("Insufficient money")
                    TextBoxMONEY.Clear()
                    TextBoxCHANGE.Clear()
                    messageboxappearance = False
                Else
                    TEXTBOXMONEYVALUE = TextBoxMONEY.Text
                    TEXTBOXCHANGEVALUE = TextBoxCHANGE.Text
                    .BackgroundWorker1.WorkerSupportsCancellation = True
                    .BackgroundWorker1.WorkerReportsProgress = True
                    .BackgroundWorker1.RunWorkerAsync()
                    .Label9.Text = TextBoxMONEY.Text
                    .Label13.Text = TextBoxCHANGE.Text
                    Close()
                    WaitFrm.Show()
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub TextBoxMONEY_TextChanged(sender As Object, e As EventArgs) Handles TextBoxMONEY.TextChanged
        Try
            TextBoxCHANGE.Text = Val(TextBoxMONEY.Text) - Double.Parse(TextBoxTOTALPAY.Text)
            If TextBoxMONEY.Text = "" Then
                TextBoxCHANGE.Text = 0
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub ButtonESC_Click(sender As Object, e As EventArgs) Handles ButtonESC.Click
        Me.Close()
    End Sub
    Private Sub TextBoxMONEY_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxMONEY.KeyPress
        If (e.KeyChar.ToString = ".") And (TextBoxMONEY.Text.Contains(e.KeyChar.ToString)) Then
            e.Handled = True
            Exit Sub
        End If
        Numeric(sender:=sender, e:=e)
    End Sub
End Class