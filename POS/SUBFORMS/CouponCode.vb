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
    Private Sub ButtonSubmit_Click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click
        Try
            Dim CountItem As Integer = 0
            With POS
                For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                    CountItem += .DataGridViewOrders.Rows(i).Cells(1).Value
                Next
            End With
            If CountItem < 1 Then
                MsgBox("Cannot apply coupon! Minimum product quantity is 1", vbInformation)
                Exit Sub
            ElseIf Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString = "Percentage" Then
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
                .DISCOUNTTYPE = Me.DataGridViewCoupons.Item(5, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub couponpercentage()
        Try
            If S_ZeroRated = "0" Then
                CouponDefault()
                With POS.DataGridViewOrders
                    Dim AmountDueWaffle As Double = 0
                    Dim AmountDueDrinks As Double = 0
                    Dim SeniorPwdDisk = DataGridViewCoupons.SelectedRows(0).Cells(3).Value / 100
                    Dim Tax = 1 + Val(S_Tax)
                    Dim GROSSSALES As Double = 0
                    Dim DISCOUNTAMOUNT As Double = SeniorPWd + SeniorPWdDrinks
                    Dim VATEXEMPTSALES As Double = DISCOUNTAMOUNT / Tax
                    VATEXEMPTSALES = Format(VATEXEMPTSALES, "0.00")
                    Dim LESSVAT As Double = DISCOUNTAMOUNT - VATEXEMPTSALES
                    Dim LESSDISCOUNT As Double = VATEXEMPTSALES * SeniorPwdDisk
                    LESSDISCOUNT = Format(LESSDISCOUNT, "0.00")
                    For i As Integer = 0 To .Rows.Count - 1 Step +1
                        GROSSSALES += .Rows(i).Cells(3).Value
                        If .Rows(i).Cells(9).Value.ToString = "WAFFLE" Then
                            AmountDueWaffle += .Rows(i).Cells(3).Value
                        Else
                            AmountDueDrinks += .Rows(i).Cells(3).Value
                        End If
                    Next
                    AmountDueWaffle = AmountDueWaffle - SeniorPWd
                    AmountDueDrinks = AmountDueDrinks - SeniorPWdDrinks
                    Dim TOTALPRODUCT As Double = AmountDueWaffle + AmountDueDrinks
                    Dim VATABLESALES As Double = TOTALPRODUCT / Tax
                    VATABLESALES = Format(VATABLESALES, "0.00")
                    Dim VAT12PERCENT As Double = VATABLESALES * S_Tax
                    VAT12PERCENT = Format(VAT12PERCENT, "0.00")
                    Dim TOTALAMOUNTDUE As Double = DISCOUNTAMOUNT - LESSVAT - LESSDISCOUNT + TOTALPRODUCT
                    With POS
                        .GROSSSALE = GROSSSALES
                        .TOTALDISCOUNTEDAMOUNT = DISCOUNTAMOUNT
                        .VATEXEMPTSALES = VATEXEMPTSALES
                        .LESSVAT = LESSVAT
                        .TOTALDISCOUNT = LESSDISCOUNT
                        .VATABLESALES = VATABLESALES
                        .VAT12PERCENT = VAT12PERCENT
                        .TOTALAMOUNTDUE = TOTALAMOUNTDUE
                        .TextBoxGRANDTOTAL.Text = TOTALAMOUNTDUE
                        .TextBoxDISCOUNT.Text = LESSDISCOUNT
                    End With
                    CouponTotal = LESSDISCOUNT
                    MsgBox("Applied")
                End With

            End If
            CouponApplied = True
            CouponName = Me.DataGridViewCoupons.Item(1, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub couponfix1()
        CouponDefault()
        Dim GROSSSALES As Double = Double.Parse(POS.Label76.Text)
        Dim DISCOUNTAMOUNT As Double = 0
        Dim LESSVAT As Double = 0
        Dim VATABLE As Double = 0
        Dim TOTALDISCOUNT As Double = DataGridViewCoupons.SelectedRows(0).Cells(3).Value
        Dim TOTALAMOUNTDUE As Double = 0
        Dim Tax = 1 + Val(S_Tax)
        Dim VATABLESALES As Double = 0
        Dim VAT12PERCENT As Double = 0
        With POS
            If Double.Parse(TOTALDISCOUNT) < GROSSSALES Then
                TOTALAMOUNTDUE = GROSSSALES - TOTALDISCOUNT
                MsgBox(TOTALAMOUNTDUE)
                VATABLESALES = Format(TOTALAMOUNTDUE / Tax, "0.00")
                VAT12PERCENT = Format(VATABLESALES * S_Tax, "0.00")
                .GROSSSALE = GROSSSALES
                .TOTALDISCOUNTEDAMOUNT = GROSSSALES
                .VATEXEMPTSALES = 0
                .LESSVAT = 0
                .TOTALDISCOUNT = TOTALDISCOUNT
                .VATABLESALES = VATABLESALES
                .VAT12PERCENT = VAT12PERCENT
                .TOTALAMOUNTDUE = TOTALAMOUNTDUE
                .TextBoxGRANDTOTAL.Text = TOTALAMOUNTDUE
                .TextBoxDISCOUNT.Text = TOTALDISCOUNT
                CouponDesc = ""
                CouponTotal = TOTALDISCOUNT
                CouponApplied = True
                CouponName = Me.DataGridViewCoupons.Item(1, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                MsgBox("Applied")
            Else
                TOTALDISCOUNT = GROSSSALES
                TOTALAMOUNTDUE = 0
                VATABLESALES = Format(TOTALAMOUNTDUE / Tax, "0.00")
                VAT12PERCENT = Format(VATABLESALES * S_Tax, "0.00")
                .GROSSSALE = GROSSSALES
                .TOTALDISCOUNTEDAMOUNT = GROSSSALES
                .VATEXEMPTSALES = 0
                .LESSVAT = 0
                .TOTALDISCOUNT = TOTALDISCOUNT
                .VATABLESALES = VATABLESALES
                .VAT12PERCENT = VAT12PERCENT
                .TOTALAMOUNTDUE = TOTALAMOUNTDUE
                .TextBoxGRANDTOTAL.Text = TOTALAMOUNTDUE
                .TextBoxDISCOUNT.Text = TOTALDISCOUNT
                CouponDesc = ""
                CouponTotal = TOTALDISCOUNT
                CouponApplied = True
                CouponName = Me.DataGridViewCoupons.Item(1, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                MsgBox("Applied")
            End If
        End With

    End Sub
    Private Sub couponfix2()
        With POS
            If Double.Parse(.Label76.Text) < Double.Parse(Me.DataGridViewCoupons.Item(4, Me.DataGridViewCoupons.CurrentRow.Index).Value) Then
                MsgBox("Condition not meet")
                CouponApplied = False
            Else
                Dim GROSSSALES As Double = 0
                Dim TAX As Double = 1 + Val(S_Tax)
                Dim TOTALDISCOUNT As Double = DataGridViewCoupons.SelectedRows(0).Cells(3).Value
                Dim TOTALAMOUNTDUE As Double = 0
                With .DataGridViewOrders
                    For i As Integer = 0 To .Rows.Count - 1 Step +1
                        GROSSSALES += .Rows(i).Cells(3).Value
                    Next
                End With
                Dim VATABLESALES As Double = Format(GROSSSALES / TAX, "0.00")
                Dim VAT12PERCENT As Double = Format(VATABLESALES * S_Tax, "0.00")
                TOTALAMOUNTDUE = Format(VATABLESALES - TOTALDISCOUNT, "0.00")
                .GROSSSALE = GROSSSALES
                .TOTALDISCOUNTEDAMOUNT = GROSSSALES
                .VATEXEMPTSALES = 0
                .LESSVAT = 0
                .TOTALDISCOUNT = TOTALDISCOUNT
                .VATABLESALES = VATABLESALES
                .VAT12PERCENT = VAT12PERCENT
                .TOTALAMOUNTDUE = TOTALAMOUNTDUE
                .TextBoxGRANDTOTAL.Text = TOTALAMOUNTDUE
                .TextBoxDISCOUNT.Text = TOTALDISCOUNT
                CouponDesc = ""
                CouponTotal = TOTALDISCOUNT
                CouponApplied = True
                CouponName = Me.DataGridViewCoupons.Item(1, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                MsgBox("Applied")
            End If
        End With
    End Sub
    Private Sub couponbundle1()
        Try
            With POS
                Dim ReferenceExist As Boolean = False
                Dim referenceID As String = Me.DataGridViewCoupons.Item(6, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                Dim refIds As String() = referenceID.Split(New Char() {","c})
                'Total Discount
                Dim DISCOUNTEDAMOUNT As Double = 0
                Dim GROSSSALES As Double = 0
                Dim VATABLESALES As Double = 0
                Dim TAX As Double = 1 + Val(S_Tax)
                Try
                    For Each getRefids In refIds
                        For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                            If .DataGridViewOrders.Rows(i).Cells(5).Value.ToString.Contains(getRefids) = True Then
                                If .DataGridViewOrders.Rows(i).Cells(1).Value >= Me.DataGridViewCoupons.Item(7, Me.DataGridViewCoupons.CurrentRow.Index).Value Then
                                    DISCOUNTEDAMOUNT = .DataGridViewOrders.Rows(i).Cells(2).Value
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
                'Gross Sales/ Subtotal
                Dim TOTALAMOUNTDUE As Double = 0
                For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                    GROSSSALES += .DataGridViewOrders.Rows(i).Cells(3).Value
                Next
                TOTALAMOUNTDUE = GROSSSALES - DISCOUNTEDAMOUNT
                Dim QtyCondMeet As Boolean = True
                Dim TotalPrice As Double = 0
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
                                    VATABLESALES = DISCOUNTEDAMOUNT / TAX
                                    .GROSSSALE = GROSSSALES
                                    .TOTALDISCOUNTEDAMOUNT = DISCOUNTEDAMOUNT
                                    .VATEXEMPTSALES = 0
                                    .LESSVAT = 0
                                    .TOTALDISCOUNT = DISCOUNTEDAMOUNT
                                    .VATABLESALES = Format(VATABLESALES, "0.00")
                                    .VAT12PERCENT = Format(TOTALAMOUNTDUE - VATABLESALES, "0.00")
                                    .TOTALAMOUNTDUE = TOTALAMOUNTDUE
                                    .TextBoxGRANDTOTAL.Text = Format(TOTALAMOUNTDUE, "0.00")
                                    .TextBoxDISCOUNT.Text = Format(DISCOUNTEDAMOUNT, "0.00")
                                    CouponDesc = ""
                                    CouponTotal = DISCOUNTEDAMOUNT
                                    CouponApplied = True
                                    CouponName = Me.DataGridViewCoupons.Item(1, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                                Else
                                    CouponDefault()
                                    MsgBox("Bundle promo qty not meet")
                                    CouponApplied = False
                                    Exit For
                                End If
                            End If
                        Next
                    Next
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
        Dim GROSSSALES As Double = 0
        Dim VATABLESALES As Double = 0
        Dim TAX As Double = 1 + Val(S_Tax)
        Dim referenceID As String = DataGridViewCoupons.Item(6, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
        Dim TOTALDISCOUNT As Double = DataGridViewCoupons.SelectedRows(0).Cells(3).Value
        Dim refIds As String() = referenceID.Split(New Char() {","c})
        With POS
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
            Dim TOTALAMOUNTDUE As Double = 0
            For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                GROSSSALES += .DataGridViewOrders.Rows(i).Cells(3).Value
            Next
            TOTALAMOUNTDUE = GROSSSALES - TOTALDISCOUNT
            VATABLESALES = TOTALAMOUNTDUE / TAX
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
                                    .GROSSSALE = GROSSSALES
                                    .TOTALDISCOUNTEDAMOUNT = TOTALAMOUNTDUE
                                    .VATEXEMPTSALES = 0
                                    .LESSVAT = 0
                                    .TOTALDISCOUNT = TOTALDISCOUNT
                                    .VATABLESALES = Format(VATABLESALES, "0.00")
                                    .VAT12PERCENT = Format(TOTALAMOUNTDUE - VATABLESALES, "0.00")
                                    .TOTALAMOUNTDUE = TOTALAMOUNTDUE
                                    .TextBoxGRANDTOTAL.Text = Format(TOTALAMOUNTDUE, "0.00")
                                    .TextBoxDISCOUNT.Text = Format(TOTALDISCOUNT, "0.00")
                                    CouponDesc = ""
                                    CouponTotal = TOTALDISCOUNT
                                    CouponApplied = True
                                    CouponName = Me.DataGridViewCoupons.Item(1, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
                                    Exit Try
                                Else
                                    CouponApplied = False
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
                Dim TAX As Double = 1 + Val(S_Tax)
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
                    Dim GROSSSALES As Double = 0
                    Dim DISCOUNTAMOUNT As Double = 0
                    Dim Percentage As Double = DataGridViewCoupons.SelectedRows(0).Cells(3).Value / 100
                    Dim TOTALDISCOUNT As Double = 0
                    Dim VATABLESALES As Double = 0
                    Dim TOTALAMOUNTDUE As Double = 0

                    If BundpromoID = True Then
                        For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                            GROSSSALES += .DataGridViewOrders.Rows(i).Cells(2).Value
                            CountQty += .DataGridViewOrders.Rows(i).Cells(1).Value
                            If CountQty = 3 Then
                                DISCOUNTAMOUNT = .DataGridViewOrders.Rows(i).Cells(2).Value
                                TOTALDISCOUNT = Percentage * DISCOUNTAMOUNT
                                TOTALAMOUNTDUE = GROSSSALES - TOTALDISCOUNT
                                VATABLESALES = Format(TOTALAMOUNTDUE / TAX, "0.00")
                                .GROSSSALE = GROSSSALES
                                .TOTALDISCOUNTEDAMOUNT = DISCOUNTAMOUNT
                                .VATEXEMPTSALES = 0
                                .LESSVAT = 0
                                .TOTALDISCOUNT = TOTALDISCOUNT
                                .VATABLESALES = Format(VATABLESALES, "0.00")
                                .VAT12PERCENT = Format(GROSSSALES - VATABLESALES, "0.00")
                                .TOTALAMOUNTDUE = TOTALAMOUNTDUE
                                .TextBoxGRANDTOTAL.Text = Format(TOTALAMOUNTDUE, "0.00")
                                .TextBoxDISCOUNT.Text = Format(TOTALDISCOUNT, "0.00")
                                CouponDesc = ""
                                CouponTotal = TOTALDISCOUNT
                                CouponApplied = True
                                CouponName = Me.DataGridViewCoupons.Item(1, Me.DataGridViewCoupons.CurrentRow.Index).Value.ToString
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