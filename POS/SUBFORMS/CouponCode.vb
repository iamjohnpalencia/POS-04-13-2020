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
    Private Sub CouponCode_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        POS.Enabled = True
    End Sub
    Private Sub ButtonSubmit_Click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click
        Try
            If POS.DataGridViewOrders.RowCount < 2 Then
                MsgBox("Cannot apply coupon, no product found!", vbInformation)
                Exit Sub
            ElseIf Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString = "Percentage" Then
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
                couponbundle1()
            ElseIf Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString = "Bundle-2(Fix)" Then
                MsgBox("Coupon is " & Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value)
                couponbundle2()
            ElseIf Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString = "Bundle-3(%)" Then
                MsgBox("Coupon is " & Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value)
                couponbundle3()
            End If
            With POS
                .discounttype = Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
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
                CouponDesc = ""
                CouponTotal = Discount + Discount1
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
                POS.TextBoxGRANDTOTAL.Text = TotalDiscount + TotalDiscount1 + Double.Parse(POS.Label76.Text) - SeniorPWd - SeniorPWdDrinks
                POS.TextBoxDISCOUNT.Text = Discount + Discount1
                CouponDesc = ""
                CouponTotal = Discount + Discount1
            End If

            CouponApplied = True
            CouponName = Me.DataGridViewCoupons.Item(1, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub couponfix1()
        CouponDefault()
        Dim total As Integer = 0
        Dim tax As String = Me.DataGridViewCoupons.Item(3, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
        If Double.Parse(POS.Label76.Text) >= DataGridViewCoupons.SelectedRows(0).Cells(3).Value Then
            For index As Integer = 0 To DataGridViewCoupons.RowCount - 1
                total = Double.Parse(POS.Label76.Text)
                POS.Label73.Text = Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                POS.TextBoxDISCOUNT.Text = tax
                POS.TextBoxGRANDTOTAL.Text = total - Val(POS.TextBoxDISCOUNT.Text)
                POS.TextBoxGRANDTOTAL.Text = Format(Val(POS.TextBoxGRANDTOTAL.Text), "##,##0.00")
                CouponDesc = ""
                CouponTotal = tax
            Next
        Else
            MsgBox("Gift certificate is greater than total")
            POS.TextBoxGRANDTOTAL.Text = Format(0, "##,##0.00")
            POS.TextBoxDISCOUNT.Text = Me.DataGridViewCoupons.Item(3, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
            CouponDesc = ""
            CouponTotal = tax
        End If
        CouponApplied = True
        CouponName = Me.DataGridViewCoupons.Item(1, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString

    End Sub

    Private Sub couponfix2()
        Dim total As Integer = 0
        Dim tax As String = Me.DataGridViewCoupons.Item(3, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
        For index As Integer = 0 To DataGridViewCoupons.RowCount - 1
            total = Double.Parse(POS.Label76.Text)
            POS.Label76.Text = Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
            If Double.Parse(POS.Label76.Text) < Double.Parse(Me.DataGridViewCoupons.Item(4, Me.DataGridViewCoupons.CurrentRow.Index).Value) Then
                MsgBox(POS.Label76.Text & " " & Double.Parse(Me.DataGridViewCoupons.Item(4, Me.DataGridViewCoupons.CurrentRow.Index).Value))
                Exit Sub
            Else
                POS.TextBoxDISCOUNT.Text = tax
                POS.TextBoxGRANDTOTAL.Text = Double.Parse(POS.Label76.Text) - Val(tax)
                POS.TextBoxGRANDTOTAL.Text = Format(Val(POS.TextBoxGRANDTOTAL.Text), "##,##0.00")
            End If
        Next
        CouponApplied = True
    End Sub
    Private Sub CouponDefault()
        CouponApplied = False
        CouponName = ""
        CouponDesc = ""
        CouponLine = 10
        CouponTotal = 0
        For i As Integer = 0 To POS.DataGridViewOrders.Rows.Count - 1 Step +1
            POS.DataGridViewOrders.Rows(i).Cells(3).Value = POS.DataGridViewOrders.Rows(i).Cells(1).Value * POS.DataGridViewOrders.Rows(i).Cells(2).Value 
        Next
    End Sub
    Private Sub couponbundle1()
        CouponDefault()
        Try
            With POS
                Dim ReferenceExist As Boolean = False
                Dim referenceID As String = Me.DataGridViewCoupons.Item(6, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                Dim refIds As String() = referenceID.Split(New Char() {","c})
                Try
                    For Each getRefids In refIds
                        For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                            If POS.DataGridViewOrders.Rows(i).Cells(5).Value.ToString.Contains(getRefids) = True Then
                                If POS.DataGridViewOrders.Rows(i).Cells(1).Value >= Me.DataGridViewCoupons.Item(7, Me.DataGridViewCoupons.CurrentRow.Index).Value Then
                                    ReferenceExist = True
                                    Exit Try
                                Else
                                    CouponDefault()
                                End If
                            Else
                                ReferenceExist = False
                            End If
                        Next
                    Next
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
                Dim QtyCondMeet As Boolean = True
                Dim TotalPrice As Integer = 0
                Dim BundlepromoID As String = Me.DataGridViewCoupons.Item(8, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                Dim bundIds As String() = BundlepromoID.Split(New Char() {","c})
                If ReferenceExist = True Then
                    For Each getBundleids In bundIds
                        For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                            If POS.DataGridViewOrders.Rows(i).Cells(5).Value.ToString.Contains(getBundleids) = True Then
                                TotalPrice += .DataGridViewOrders.Rows(i).Cells(3).Value
                                If POS.DataGridViewOrders.Rows(i).Cells(1).Value >= Me.DataGridViewCoupons.SelectedRows(0).Cells(9).Value Then
                                    MsgBox("Bundle Promo Quantity meet")
                                    Dim OrgPrice = POS.DataGridViewOrders.Rows(i).Cells(2).Value
                                    Dim Qty = Me.DataGridViewCoupons.SelectedRows(0).Cells(9).Value
                                    Dim TotalLess = OrgPrice * Qty
                                    CouponLine += 10
                                    CouponDesc = CouponDesc + "   (" & Me.DataGridViewCoupons.SelectedRows(0).Cells(9).Value & "x) " & GLOBAL_SELECT_FUNCTION_RETURN("loc_admin_products", "product_sku", "product_id = " & getBundleids, "product_sku") & vbNewLine
                                    POS.DataGridViewOrders.Rows(i).Cells(3).Value = POS.DataGridViewOrders.Rows(i).Cells(3).Value - TotalLess
                                    POS.TextBoxDISCOUNT.Text = TotalPrice
                                Else
                                    CouponDefault()
                                    MsgBox("Bundle promo qty not meet")
                                    QtyCondMeet = False
                                    Exit For
                                End If
                            End If
                        Next
                        If QtyCondMeet = False Then
                            For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                                POS.DataGridViewOrders.Rows(i).Cells(3).Value = POS.DataGridViewOrders.Rows(i).Cells(1).Value * POS.DataGridViewOrders.Rows(i).Cells(2).Value
                            Next
                            CouponDefault()
                        Else
                            CouponApplied = True
                            CouponName = Me.DataGridViewCoupons.Item(1, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                        End If
                        POS.Label76.Text = SumOfColumnsToDecimal(datagrid:=POS.DataGridViewOrders, celltocompute:=3)
                    Next
                    CouponTotal = TotalPrice
                Else
                    CouponDefault()
                    MsgBox("Condition not meet")
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub couponbundle2()
        CouponDefault()
        Dim ReferenceExist As Boolean = False
        Dim referenceID As String = Me.DataGridViewCoupons.Item(6, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
        Dim refIds As String() = referenceID.Split(New Char() {","c})
        With POS
            Try
                For Each getRefids In refIds
                    For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                        MsgBox(POS.DataGridViewOrders.Rows(i).Cells(5).Value.ToString)
                        If POS.DataGridViewOrders.Rows(i).Cells(5).Value.ToString.Contains(getRefids) = True Then
                            If POS.DataGridViewOrders.Rows(i).Cells(1).Value >= Me.DataGridViewCoupons.Item(7, Me.DataGridViewCoupons.CurrentRow.Index).Value Then
                                ReferenceExist = True
                                Exit Try
                            Else
                                CouponDefault()
                            End If
                        Else
                            ReferenceExist = False
                        End If
                    Next
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            Dim QtyCondMeet As Boolean = True
            Dim TotalPrice As Integer = 0
            Dim BundlepromoID As String = Me.DataGridViewCoupons.Item(8, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
            Dim bundIds As String() = BundlepromoID.Split(New Char() {","c})
            Dim BundpromoID As Boolean = False
            If ReferenceExist = True Then
                Try
                    For Each getBundleids In bundIds
                        Console.Write(getBundleids)
                        For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                            If POS.DataGridViewOrders.Rows(i).Cells(5).Value.ToString.Contains(getBundleids) = True Then
                                If POS.DataGridViewOrders.Rows(i).Cells(1).Value >= Me.DataGridViewCoupons.SelectedRows(0).Cells(9).Value Then
                                    POS.DataGridViewOrders.Rows(i).Cells(3).Value = POS.DataGridViewOrders.Rows(i).Cells(3).Value - Me.DataGridViewCoupons.SelectedRows(0).Cells(3).Value
                                    CouponApplied = True
                                    CouponLine += 10
                                    CouponName = Me.DataGridViewCoupons.Item(1, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                                    CouponDesc = CouponDesc + "   (" & Me.DataGridViewCoupons.SelectedRows(0).Cells(9).Value & "x) " & GLOBAL_SELECT_FUNCTION_RETURN("loc_admin_products", "product_sku", "product_id = " & getBundleids, "product_sku") & vbNewLine
                                    POS.Label76.Text = SumOfColumnsToDecimal(datagrid:=POS.DataGridViewOrders, celltocompute:=3)
                                    BundpromoID = True
                                    CouponTotal = Me.DataGridViewCoupons.SelectedRows(0).Cells(3).Value
                                    POS.TextBoxDISCOUNT.Text = CouponTotal
                                    Exit Try
                                Else
                                    BundpromoID = False
                                End If
                            End If
                        Next
                    Next

                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
                MsgBox(BundpromoID)
            Else
                MsgBox("Condition not meet")
            End If
        End With
    End Sub
    Private Sub couponbundle3()
        CouponDefault()
        Try
            With POS
                Dim ReferenceExist As Boolean = False
                Dim referenceID As String = Me.DataGridViewCoupons.Item(6, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                Dim refIds As String() = referenceID.Split(New Char() {","c})
                Dim TotalQtyCount As Integer = 0
                Try
                    For Each getRefids In refIds
                        For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                            If POS.DataGridViewOrders.Rows(i).Cells(5).Value = getRefids Then
                                TotalQtyCount += POS.DataGridViewOrders.Rows(i).Cells(1).Value
                            End If
                        Next
                    Next
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
                Dim BundlepromoID As String = Me.DataGridViewCoupons.Item(8, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                Dim bundIds As String() = BundlepromoID.Split(New Char() {","c})
                Dim BundpromoID As Boolean = False
                Dim CountQty As Integer = 0
                If TotalQtyCount >= Me.DataGridViewCoupons.Item(7, Me.DataGridViewCoupons.CurrentRow.Index).Value Then
                    Try
                        For Each getBundleids In bundIds
                            For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                                If POS.DataGridViewOrders.Rows(i).Cells(5).Value.ToString.Contains(getBundleids) = True Then
                                    If POS.DataGridViewOrders.Rows(i).Cells(1).Value >= Me.DataGridViewCoupons.SelectedRows(0).Cells(9).Value Then
                                        BundpromoID = True
                                        Exit Try
                                    Else
                                        BundpromoID = False

                                    End If
                                End If
                            Next
                        Next
                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try
                    If BundpromoID = True Then
                        For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                            CountQty += .DataGridViewOrders.Rows(i).Cells(1).Value
                            If CountQty = 3 Then
                                CouponLine += 10
                                CouponApplied = True
                                Dim Percentage = Me.DataGridViewCoupons.Item(3, Me.DataGridViewCoupons.CurrentRow.Index).Value / 100
                                Dim DiscountedPrice = .DataGridViewOrders.Rows(i).Cells(2).Value * Percentage
                                .DataGridViewOrders.Rows(i).Cells(3).Value = .DataGridViewOrders.Rows(i).Cells(3).Value - DiscountedPrice
                                CouponTotal = DiscountedPrice
                                CouponName = Me.DataGridViewCoupons.Item(1, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                                CouponDesc = CouponDesc + "   (" & Me.DataGridViewCoupons.SelectedRows(0).Cells(9).Value & "x) " & GLOBAL_SELECT_FUNCTION_RETURN("loc_admin_products", "product_sku", "product_id = " & .DataGridViewOrders.Rows(i).Cells(5).Value, "product_sku") & vbNewLine
                                .Label76.Text = SumOfColumnsToDecimal(datagrid:= .DataGridViewOrders, celltocompute:=3)
                                POS.TextBoxDISCOUNT.Text = CouponTotal
                            End If
                        Next
                    End If
                Else
                    MsgBox("Cond not meet")
                End If

            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class