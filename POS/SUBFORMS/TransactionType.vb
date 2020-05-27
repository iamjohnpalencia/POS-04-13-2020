﻿Public Class TransactionType
    Private Sub TransactionType_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        POS.Enabled = True
    End Sub
    Private Sub ButtonESC_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Me.Close()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ButtonWalkIn.Click
        TransactionType(ButtonWalkIn)
        If Application.OpenForms().OfType(Of TransactionTypeInfo).Any Then
            TransactionTypeInfo.Close()
        End If
        Me.Close()
    End Sub
    Private Sub ButtonRegistered_Click(sender As Object, e As EventArgs) Handles ButtonRegistered.Click
        TransactionType(ButtonRegistered)
        If Application.OpenForms().OfType(Of TransactionTypeInfo).Any Then
            TransactionTypeInfo.Close()
        End If
        Me.Close()
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles ButtonGcash.Click
        modeoftransaction = True
        TransactionType(ButtonGcash)
        ButtonCancel.Enabled = False
        TransactionTypeInfo.Show()
    End Sub
    Private Sub ButtonGrab_Click(sender As Object, e As EventArgs) Handles ButtonGrab.Click
        modeoftransaction = True
        TransactionType(ButtonGrab)
        ButtonCancel.Enabled = False
        TransactionTypeInfo.Show()
    End Sub
    Private Sub ButtonPayMaya_Click(sender As Object, e As EventArgs) Handles ButtonPayMaya.Click
        modeoftransaction = True
        TransactionType(ButtonPayMaya)
        ButtonCancel.Enabled = False
        TransactionTypeInfo.Show()
    End Sub
    Private Sub ButtonCashalo_Click(sender As Object, e As EventArgs) Handles ButtonCashalo.Click
        modeoftransaction = True
        TransactionType(ButtonCashalo)
        ButtonCancel.Enabled = False
        TransactionTypeInfo.Show()
    End Sub
    Private Sub ButtonRepEx_Click(sender As Object, e As EventArgs) Handles ButtonRepEx.Click
        transactionmode = "Representation Expenses"
        If Application.OpenForms().OfType(Of TransactionTypeInfo).Any Then
            TransactionTypeInfo.Close()
        End If
        Me.Close()
    End Sub
    Private Sub ButtonFoodP_Click(sender As Object, e As EventArgs) Handles ButtonFoodP.Click
        modeoftransaction = True
        TransactionType(ButtonFoodP)
        ButtonCancel.Enabled = False
        TransactionTypeInfo.Show()
    End Sub
    Private Sub ButtonOthers_Click(sender As Object, e As EventArgs) Handles ButtonOthers.Click
        modeoftransaction = True
        TransactionType(ButtonOthers)
        ButtonCancel.Enabled = False
        TransactionTypeInfo.Show()
    End Sub
    Private Sub TransactionType(btn)
        transactionmode = btn.text
        ButtonEnableability(Me, False)
    End Sub
End Class