Imports MySql.Data.MySqlClient
Public Class CouponCode
    Private Sub CouponCode_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GLOBAL_SELECT_ALL_FUNCTION(table:="tbcoupon", fields:="*", datagrid:=DataGridViewCoupons)
        With DataGridViewCoupons
            .Columns(0).Visible = False
            .Columns(3).Visible = False
            .Columns(4).Visible = False
            .Columns(6).Visible = False
            .Columns(7).Visible = False
            .Columns(8).Visible = False
            .Columns(9).Visible = False
        End With
    End Sub
    Private Sub ButtonExit_Click(sender As Object, e As EventArgs) Handles ButtonExit.Click
        Me.Close()
    End Sub
    Private Sub DataGridViewCoupons_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewCoupons.CellClick
        Try
            TextBoxCODE.Text = DataGridViewCoupons.SelectedRows(0).Cells(0).Value.ToString
        Catch ex As Exception
            MsgBox(ex.TargetSite)
        End Try
    End Sub
    Private Sub CouponCode_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        POS.Enabled = True
    End Sub
    Private Sub ButtonSubmit_Click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click
        Try
            'If POS.DataGridViewOrders.RowCount < 2 Then
            '    MsgBox("Cannot apply coupon, no product found!", vbInformation)
            '    Exit Sub
            If Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString = "Percentage" Then
                'MsgBox("Coupon is " & Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value)
                couponpercentage()
            ElseIf Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString = "Fix-1" Then
                MsgBox("Coupon is " & Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value)
                couponfix1()
            ElseIf Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString = "Fix-2" Then
                MsgBox("Coupon is " & Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value)
                couponfix2()
            ElseIf Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString = "Bundle-1(Fix)" Then
                MsgBox("Coupon is " & Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value)
                'couponbundle1()
            ElseIf Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString = "Bundle-2(Fix)" Then
                MsgBox("Coupon is " & Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value)
                'couponbundle2()
            ElseIf Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString = "Bundle-3(%)" Then
                MsgBox("Coupon is " & Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value)
                'couponbundle3()        
            End If
            With POS
                If TextBoxCODE.Text = "1" Then
                    .discounttype = "Percentage"
                ElseIf TextBoxCODE.Text = "2" Then
                    .discounttype = "Fix-1"
                ElseIf TextBoxCODE.Text = "3" Then
                    .discounttype = "Fix-2"
                ElseIf TextBoxCODE.Text = "4" Then
                    .discounttype = "Bundle-1(Fix)"
                ElseIf TextBoxCODE.Text = "5" Then
                    .discounttype = "Bundle-2(Fix)"
                ElseIf TextBoxCODE.Text = "6" Then
                    .discounttype = "Bundle-3(%)"
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub couponpercentage()
        Try
            If S_ZeroRated = "0" Then
                '45 Waffle
                Dim GrossSale = SeniorPWd
                Dim vatexemptsales As Double = Math.Round(GrossSale / 1.12, 2)
                Dim lessvat As Double = Math.Round(vatexemptsales * Val(S_Tax), 2)
                Dim Discount As Double = Math.Round(vatexemptsales * 0.2, 2)
                Dim TotalDiscount As Double = GrossSale - Discount - lessvat
                'Drinks
                Dim GrossSale1 = SeniorPWdDrinks
                Dim vatexemptsales1 As Double = Math.Round(GrossSale1 / 1.12, 2)
                Dim lessvat1 As Double = Math.Round(vatexemptsales1 * Val(S_Tax), 2)
                Dim Discount1 As Double = Math.Round(vatexemptsales1 * 0.2, 2)
                Dim TotalDiscount1 As Double = GrossSale1 - Discount1 - lessvat1
                '==============================================================
                POS.VATEXEMPTSALES = vatexemptsales + vatexemptsales1
                POS.LESSVAT = lessvat + lessvat1
                POS.TextBoxGRANDTOTAL.Text = TotalDiscount + TotalDiscount1 + POS.Label76.Text - SeniorPWd - SeniorPWdDrinks
                POS.TextBoxDISCOUNT.Text = Discount + Discount1
            Else
                'Waffle
                Dim GrossSale = SeniorPWd
                Dim vatableSales As Double = Math.Round(GrossSale / 1.12, 2)
                Dim Discount As Double = Math.Round(vatableSales * 0.2, 2)
                Dim TotalDiscount As Double = Math.Round(vatableSales - Discount, 2)
                'Drinks
                Dim GrossSale1 = SeniorPWdDrinks
                Dim vatableSales1 As Double = Math.Round(GrossSale1 / 1.12, 2)
                Dim Discount1 As Double = Math.Round(vatableSales1 * 0.2, 2)
                Dim TotalDiscount1 As Double = Math.Round(vatableSales1 - Discount1, 2)
                '==============================================================
                POS.GRANDTOTALDISCOUNT = TotalDiscount + TotalDiscount1
                POS.TextBoxGRANDTOTAL.Text = TotalDiscount + TotalDiscount1 + POS.Label76.Text - SeniorPWd - SeniorPWdDrinks
                POS.TextBoxDISCOUNT.Text = Discount + Discount1
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub couponfix1()
        Dim total As Integer = 0
        Dim tax As String = Me.DataGridViewCoupons.Item(3, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
        If Val(POS.Label76.Text) > DataGridViewCoupons.SelectedRows(0).Cells(3).Value Then
            For index As Integer = 0 To DataGridViewCoupons.RowCount - 1
                total = Val(POS.Label76.Text)
                POS.Label73.Text = Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                POS.TextBoxDISCOUNT.Text = tax
                POS.TextBoxGRANDTOTAL.Text = total - Val(POS.TextBoxDISCOUNT.Text)
            Next
        Else
            MsgBox("Gift certificate is greater than total")
        End If
    End Sub

    Private Sub couponfix2()
        Dim total As Integer = 0
        Dim tax As String = Me.DataGridViewCoupons.Item(3, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
        For index As Integer = 0 To DataGridViewCoupons.RowCount - 1
            total = Val(POS.Label76.Text)
            POS.Label76.Text = Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
            If CInt(POS.Label76.Text) < CInt(Me.DataGridViewCoupons.Item(4, Me.DataGridViewCoupons.CurrentRow.Index).Value) Then
                MsgBox(POS.Label76.Text & " " & CInt(Me.DataGridViewCoupons.Item(4, Me.DataGridViewCoupons.CurrentRow.Index).Value))
                Exit Sub
            Else
                POS.TextBoxDISCOUNT.Text = tax
                POS.TextBoxGRANDTOTAL.Text = Val(POS.Label76.Text) - Val(tax)
            End If
        Next
    End Sub
    'Private Sub couponbundle1()
    '    For index As Integer = 0 To DataGridViewCoupons.RowCount - 1
    '        POS.TextBoxDISCOUNT.Text = Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
    '        Dim result1 As Boolean = False
    '        If POS.DataGridViewOrders.Rows(index).Cells(0).Value.ToString.Contains(Me.DataGridViewCoupons.Item(6, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString) = True Then
    '            MsgBox("Product ID : " & POS.DataGridViewOrders.Rows(index).Cells(0).Value.ToString & " Product Base ID : " & Me.DataGridViewCoupons.Item(6, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString)
    '            MsgBox("Product Count : " & POS.DataGridViewOrders.Rows(index).Cells(4).Value.ToString & " Minimum Qty : " & Me.DataGridViewCoupons.Item(7, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString)
    '            result1 = True
    '            If Me.DataGridViewCoupons.Item(3, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString = Trim(String.Empty) Then
    '                'Form1.Label10.Text = "FREE"
    '            End If
    '        End If
    '        If result1 = True Then
    '            If Form1.DataGridView2.Rows(index).Cells(0).Value.ToString.Contains(Me.DataGridViewCoupons.Item(6, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString) = True Then
    '                Form1.DataGridView2.Rows(index).Cells(4).Value = Val(Form1.DataGridView2.Rows(index).Cells(4).Value.ToString) + 1
    '                Form1.Label7.Text = Val(Form1.Label7.Text) + 1
    '                'Its up either you combine the QTY or set it in a separate line - in my case i do not have receipt so i set it to the qty w/o affecting the price
    '                ' You can set the free item in a new line within the receipt with zeroed out price
    '                ' Ex : PB Waffle - - - - - 0.00
    '            End If
    '        End If
    '    Next
    'End Sub

    'Private Sub couponbundle2()
    '    Dim result1 As Boolean = False
    '    For index As Integer = 0 To DataGridViewCoupons.RowCount - 1
    '        Form1.Label73.Text = Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
    '        If Form1.DataGridView2.Rows(index).Cells(0).Value.ToString.Contains(Me.DataGridViewCoupons.Item(6, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString) = True Then
    '            MsgBox("Product ID : " & Form1.DataGridView2.Rows(index).Cells(0).Value.ToString & " Product Base ID : " & Me.DataGridViewCoupons.Item(6, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString)
    '            MsgBox("Product Count : " & Form1.DataGridView2.Rows(index).Cells(4).Value.ToString & " Minimum Qty : " & Me.DataGridViewCoupons.Item(7, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString)
    '            MsgBox(DataGridViewCoupons.Item(8, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString)
    '            result1 = True
    '            Form1.TextBoxDISCOUNT.Text = Me.DataGridViewCoupons.Item(3, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString * -1
    '            Form1.Label8.Text = Val(Form1.Label8.Text) - Val(DataGridViewCoupons.Item(3, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString)
    '            'Its up either you combine the QTY or set it in a separate line - in my case i do not have receipt so i set it to the qty w/o affecting the price
    '            ' You can set the free item in a new line within the receipt with discounted amount
    '            ' Ex : 10 OFF . . . . . . -10.00
    '        End If
    '    Next
    'End Sub
    'Private Sub couponbundle3()
    '    Dim result1 As Boolean = False
    '    For index As Integer = 0 To DataGridViewCoupons.RowCount - 1
    '        Form1.Label73.Text = Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
    '        If Form1.DataGridView2.Rows(index).Cells(0).Value.ToString = (Me.DataGridViewCoupons.Item(6, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString) Then
    '            MsgBox("Product ID : " & Form1.DataGridView2.Rows(index).Cells(0).Value.ToString & " Product Base ID : " & Me.DataGridViewCoupons.Item(6, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString)
    '            MsgBox("Product Count : " & Form1.DataGridView2.Rows(index).Cells(4).Value.ToString & " Minimum Qty : " & Me.DataGridViewCoupons.Item(7, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString)
    '            MsgBox(DataGridViewCoupons.Item(8, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString)
    '            result1 = True
    '            Form1.TextBoxDISCOUNT.Text = Me.DataGridViewCoupons.Item(3, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString & "%"
    '            'Put here the condition of minimum value
    '            'Before applying the condition 
    '            Dim percentagess As String = Me.DataGridViewCoupons.Item(3, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString / 100
    '            Dim stage1 As Integer = Form1.Label8.Text * percentagess
    '            Dim stage2 As Integer = Form1.Label8.Text - stage1
    '            Form1.Label8.Text = stage2
    '            ' You can set the free item in a new line within the receipt with discounted amount
    '            ' Ex : 10 OFF . . . . . . -10.00
    '        End If
    '    Next
    '    'This is not a bug free sample - kindly catch all conditional flaws i have here :) Thank you
    'End Sub
End Class