﻿Public Class TransactionTypeInfo
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TopMost = True
        If transactionmode = "GCash" Then
            TextBoxMARKUP.Text = Val(POS.TextBoxGRANDTOTAL.Text) * 0.15
        Else
            TextBoxMARKUP.Text = "N/A"
        End If
    End Sub
    Private Sub ButtonESC_Click(sender As Object, e As EventArgs) Handles ButtonESC.Click
        Me.Close()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If String.IsNullOrWhiteSpace(TextBoxFULLNAME.Text) Or String.IsNullOrWhiteSpace(TextBoxREFERENCE.Text) Then
            MsgBox("Please fill out all fields.", vbInformation)
        Else
            TEXTBOXFULLNAMEVALUE = TextBoxFULLNAME.Text
            TEXTBOXREFERENCEVALUE = TextBoxREFERENCE.Text
            TEXTBOXMARKUPVALUE = TextBoxMARKUP.Text
            MsgBox(TEXTBOXFULLNAMEVALUE)
            MsgBox(TEXTBOXREFERENCEVALUE)
            MsgBox(TEXTBOXMARKUPVALUE)
            Close()
            TransactionType.Close()
        End If
    End Sub
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ButtonEnableability(TransactionType, True)
    End Sub
End Class