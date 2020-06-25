Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing
Imports System.Threading
Imports System.Data
Imports System.Linq
Public Class POS
    Private WithEvents printdoc As PrintDocument = New PrintDocument
    Private PrintPreviewDialog1 As New PrintPreviewDialog
    Private Count_control As Integer = 0
    Private Location_control As New Point(0, 0)
    Private datas
    Public ButtonClickCount As Integer = 0
    Public a = 0
    Public b = 0

    Public vat As Decimal
    Public SUPERAMOUNTDUE
    Dim result As Integer

    Dim insertcurrenttime As String
    Dim insertcurrentdate As String
    Dim thread As Thread

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridViewPRODUCTUPDATE.DataSource = ProductDTUpdate
        DataGridViewFORMULAUPDATE.DataSource = FormulaDTUpdate
        DataGridViewCATEGORYUPDATE.DataSource = CategoryDTUpdate
        DataGridViewINVENTORYUPDATE.DataSource = InventoryDTUpdate
        If Application.OpenForms().OfType(Of SynctoCloud).Any Then
            SynctoCloud.BringToFront()
        End If
        LabelStorename.Text = ClientStorename
        Label11.Focus()
        Timer1.Start()
        Label76.Text = 0
        listviewproductsshow(where:="Simply Perfect")
        selectmax(whatform:=1)
        DataGridViewOrders.Font = New Font("Kelson Sans Normal", 11.25)
        Try
            sql = "SELECT category_name FROM loc_admin_category WHERE status = 1"
            cmd = New MySqlCommand(sql, LocalhostConn())
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable()
            da.Fill(dt)
            With cmd
                For Each row As DataRow In dt.Rows
                    Dim buttonname As String = row("category_name")
                    Dim new_Button As New Button
                    Dim panellocation As New Panel
                    With new_Button
                        .Name = buttonname
                        .Text = buttonname
                        .TextImageRelation = TextImageRelation.ImageBeforeText
                        .TextAlign = ContentAlignment.MiddleCenter
                        .ForeColor = Color.White
                        .Font = New Font("Oswald", 11, FontStyle.Bold)
                        .FlatStyle = FlatStyle.Flat
                        .FlatAppearance.BorderSize = 0
                        .Location = New Point(Location_control.X, Location_control.Y)
                        .Width = 130
                        .Height = 53
                        .Cursor = Cursors.Hand
                        Location_control.X += .Height + 75
                        AddHandler .Click, AddressOf new_Button_click
                    End With
                    Panel3.Controls.Add(new_Button)
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        DataGridViewOrders.CellBorderStyle = DataGridViewCellBorderStyle.None
        Enabled = False
        BegBalance.Show()
        BegBalance.TopMost = True
    End Sub
    Private Sub new_Button_click(ByVal sender As Object, ByVal e As EventArgs)
        'NEW BUTTON ON CLICK EVENT 
        If TypeOf sender Is Button Then
            Dim btn = sender
            Dim name = btn.name
            btnformcolor(changecolor:=sender)
            btndefaut(defaultcolor:=sender, form:=Me)
            listviewproductsshow(where:=name)
        End If
    End Sub
    Private Sub ButtonLogout_Click(sender As Object, e As EventArgs) Handles ButtonLogout.Click
        'LOGOUT
        If SyncIsOnProcess = True Then
            MessageBox.Show("Sync is on process please wait.", "Syncing", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            If MessageBox.Show("Are you sure you really want to Logout ?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
                FormIsOpen()
                SystemLogDesc = "User Logout: " & returnfullname(where:=ClientCrewID)
                SystemLogType = "LOG OUT"
                GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
                EndBalance()
                Login.Show()
                Dispose()
            End If
        End If
    End Sub
    Private Sub ButtonSettings_Click(sender As Object, e As EventArgs) Handles ButtonSettings.Click
        SettingsForm.Show()
        Enabled = False
    End Sub
    Private Sub ButtonExpense_Click(sender As Object, e As EventArgs) Handles ButtonExpense.Click
        Enabled = False
        Dim newMDIchild As New Addexpense()
        If Application.OpenForms().OfType(Of Addexpense).Any Then
            Addexpense.BringToFront()
        Else
            Addexpense.Show()
            Addexpense.Focus()
        End If
        'VIEW EXPENSE FORM
    End Sub
    Private Sub ButtonMenu_Click(sender As Object, e As EventArgs) Handles ButtonMenu.Click
        'VIEW MENU FORM
        messageboxappearance = False
        SystemLogType = "MENU FORM"
        SystemLogDesc = "Accessed by :" & returnfullname(ClientCrewID) & " : " & ClientRole
        GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
        Enabled = False
        MDIFORM.Show()
    End Sub
    Private Sub ButtonPromo_Click(sender As Object, e As EventArgs) Handles ButtonPromo.Click
        'VIEW PROMO FORM
        Me.Enabled = False
        Dim newMDIchild As New Promo()
        If Application.OpenForms().OfType(Of CouponCode).Any Then
            CouponCode.BringToFront()
        Else
            CouponCode.Show()
            CouponCode.ButtonSubmit.Enabled = False
        End If
    End Sub

    Private Sub Button38_Click(sender As Object, e As EventArgs) Handles ButtonEnter.Click
        enterpressorbuttonpress = False
        If payment = False Then
            Try
                If TextBoxPRICE.Text = "" And TextBoxNAME.Text = "" Then
                    MsgBox("Select Product first!")
                Else
                    If TextBoxQTY.Text <> 0 Then
                        If DataGridViewOrders.Rows.Count > 0 Then
                            DataGridViewOrders.SelectedRows(0).Cells(1).Value = Val(TextBoxQTY.Text)
                            DataGridViewOrders.SelectedRows(0).Cells(3).Value = DataGridViewOrders.SelectedRows(0).Cells(1).Value * DataGridViewOrders.SelectedRows(0).Cells(2).Value
                            Label76.Text = SumOfColumnsToDecimal(datagrid:=DataGridViewOrders, celltocompute:=3)
                            Dim test As Boolean = False
                            For Each row In DataGridViewInv.Rows
                                If TextBoxNAME.Text = row.Cells("Column10").Value Then
                                    test = True
                                    Exit For
                                End If
                            Next
                            For i As Integer = 0 To DataGridViewInv.Rows.Count - 1 Step +1
                                If DataGridViewOrders.SelectedRows(0).Cells(7).Value <> "Add-Ons" Then
                                    If DataGridViewInv.Rows(i).Cells(4).Value.ToString() = DataGridViewOrders.SelectedRows(0).Cells(0).Value Then
                                        DataGridViewInv.Rows(i).Cells(0).Value = DataGridViewOrders.SelectedRows(0).Cells(1).Value * DataGridViewInv.Rows(i).Cells(5).Value.ToString()
                                        DataGridViewInv.Rows(i).Cells(2).Value = TextBoxQTY.Text
                                    End If
                                Else
                                    If DataGridViewOrders.SelectedRows(0).Cells(8).Value = DataGridViewInv.Rows(i).Cells(8).Value Then
                                        If DataGridViewInv.Rows(i).Cells(4).Value.ToString = DataGridViewOrders.SelectedRows(0).Cells(0).Value.ToString Then
                                            DataGridViewInv.Rows(i).Cells(0).Value = DataGridViewOrders.SelectedRows(0).Cells(1).Value * DataGridViewInv.Rows(i).Cells(5).Value.ToString()
                                            DataGridViewInv.Rows(i).Cells(2).Value = TextBoxQTY.Text
                                        End If
                                    End If
                                End If

                            Next
                        Else
                            MsgBox("Select item first", vbInformation)
                        End If
                    End If
                    TextBoxQTY.Text = 0
                End If

            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Else
            'ButtonSubmitPayment.PerformClick()
        End If
    End Sub
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles ButtonPendingOrders.Click
        Dim newMDIchild As New PendingOrders()
        If Application.OpenForms().OfType(Of PendingOrders).Any Then
            PendingOrders.BringToFront()
        Else
            PendingOrders.Show()
            posandpendingenter = True
            Me.Enabled = False
        End If
    End Sub
    Private Sub Buttonholdoder_Click(sender As Object, e As EventArgs) Handles Buttonholdoder.Click
        If Application.OpenForms().OfType(Of HoldOrder).Any Then
            HoldOrder.BringToFront()
        Else
            HoldOrder.Show()
            Me.Enabled = False
        End If
    End Sub
    Private Sub ButtonPay_Click(sender As Object, e As EventArgs) Handles ButtonPay.Click
        If Shift = "" Then
            MessageBox.Show("Input cashier balance first", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            If S_Zreading <> Format(Now(), "yyyy-MM-dd") Then
                MessageBox.Show("Z-read", "Z-Reading", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Enabled = False
                PaymentForm.TextBoxTOTALPAY.Text = TextBoxGRANDTOTAL.Text
                PaymentForm.Show()
                PaymentForm.Focus()
            End If
        End If
    End Sub
    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Try
            If DataGridViewOrders.Rows.Count > 0 Then
                datas = DataGridViewOrders.SelectedRows(0).Cells(4).Value.ToString()
                For x As Integer = DataGridViewInv.Rows.Count - 1 To 0 Step -1
                    If DataGridViewInv.Rows(x).Cells("Column8").Value = datas Then
                        DataGridViewInv.Rows.Remove(DataGridViewInv.Rows(x))
                    End If
                Next
                datas = ""
                deleteitem = True
                Dim dr As DataGridViewRow
                For Each dr In DataGridViewOrders.SelectedRows
                    Dim sum As String = DataGridViewOrders.SelectedRows(0).Cells(3).Value.ToString
                    DataGridViewOrders.Rows.Remove(dr)
                    Label76.Text = SumOfColumnsToDecimal(datagrid:=DataGridViewOrders, celltocompute:=3)
                    Dim discount As Double = Val(TextBoxDISCOUNT.Text / 100)
                    Dim discounttotal As Double = Val(Label76.Text) * discount
                    TextBoxSUBTOTAL.Text = Val(Label76.Text)

                    TextBoxGRANDTOTAL.Text = TextBoxSUBTOTAL.Text - discounttotal
                    TextBoxSUBTOTAL.Text = Format(Val(TextBoxSUBTOTAL.Text), "##,##0.00")
                    TextBoxGRANDTOTAL.Text = Format(Val(TextBoxGRANDTOTAL.Text), "##,##0.00")
                Next
            Else
                TextBoxQTY.Text = 0
                MessageBox.Show("Add item first", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            If DataGridViewOrders.Rows.Count > 0 Then
                ButtonPay.Enabled = True
                Buttonholdoder.Enabled = True
                ButtonPendingOrders.Enabled = False
            Else
                ButtonClickCount = 0
                ButtonPay.Enabled = False
                Buttonholdoder.Enabled = False
                ButtonPendingOrders.Enabled = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Label76_TextChanged(sender As Object, e As EventArgs) Handles Label76.TextChanged
        Try
            If DataGridViewOrders.RowCount > 0 Then
                ButtonApplyCoupon.Enabled = True
            Else
                ButtonApplyCoupon.Enabled = False
            End If

            Label76.Text = SumOfColumnsToDecimal(DataGridViewOrders, 3)
            TextBoxGRANDTOTAL.Text = Label76.Text
            TextBoxSUBTOTAL.Text = Label76.Text
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub POS_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Expenses.Dispose()
        Promo.Dispose()
        Couponisavailable = False
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        insertcurrenttime = TimeOfDay.ToString("HH:mm:ss")
        insertcurrentdate = String.Format("{0:yyyy/MM/dd}", DateTime.Now)
        Label11.Text = Date.Now.ToString("hh:mm:ss tt")
    End Sub


    Private Sub ButtonCDISC_Click(sender As Object, e As EventArgs) Handles ButtonCDISC.Click
        TextBoxDISCOUNT.Text = 0
        TOTALDISCOUNT = 0
        GROSSSALE = 0
        VATEXEMPTSALES = 0
        LESSVAT = 0
        TOTALDISCOUNTEDAMOUNT = 0
        TOTALAMOUNTDUE = 0
        VATABLESALES = 0
        VAT12PERCENT = 0
        ZERORATEDSALES = 0
        TextBoxGRANDTOTAL.Text = SumOfColumnsToDecimal(DataGridViewOrders, 3)
        TextBoxSUBTOTAL.Text = SumOfColumnsToDecimal(DataGridViewOrders, 3)
        Couponisavailable = False
        CouponApplied = False
    End Sub
    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles ButtonTransactionMode.Click
        Enabled = False
        TransactionType.Show()
    End Sub
    Private Sub Button1_Click_3(sender As Object, e As EventArgs) Handles Button1.Click
        Dim message = MessageBox.Show("Do you want to add Extra packaging?", "Extra packaging", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If message = DialogResult.Yes Then
            Try
                table = "loc_pos_inventory"
                fields = "stock_primary = stock_primary - 1, stock_secondary = stock_secondary - 1"
                where = "product_ingredients = 'Extra Packaging'"
                GLOBAL_FUNCTION_UPDATE(table, fields, where)
                SystemLogType = "PACKAGING"
                SystemLogDesc = "Crew : " & ClientCrewID
                GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End If
    End Sub
#Region "Button Functions"
    Private Sub POS_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyData = Keys.Alt + Keys.F4 Then
            e.Handled = True
        End If
        If e.KeyCode = Keys.F9 Then
            ButtonPay.PerformClick()
        ElseIf e.KeyCode = Keys.Enter Then
            ButtonEnter.PerformClick()
        ElseIf e.KeyCode = Keys.F10 Then
            ButtonTransactionMode.PerformClick()
        ElseIf e.KeyCode = Keys.F11 Then
            Buttonholdoder.PerformClick()
        ElseIf e.KeyCode = Keys.F12 Then
            ButtonPendingOrders.PerformClick()
        ElseIf e.KeyCode = Keys.Delete Then
            ButtonCancel.PerformClick()
            '=================================
        End If
        If payment = False Then
            If e.KeyCode = Keys.NumPad0 Then
                ButtonNo0.PerformClick()
            ElseIf e.KeyCode = Keys.NumPad1 Then
                ButtonNo1.PerformClick()
            ElseIf e.KeyCode = Keys.NumPad2 Then
                ButtonNo2.PerformClick()
            ElseIf e.KeyCode = Keys.NumPad3 Then
                ButtonNo3.PerformClick()
            ElseIf e.KeyCode = Keys.NumPad4 Then
                ButtonNo4.PerformClick()
            ElseIf e.KeyCode = Keys.NumPad5 Then
                ButtonNo5.PerformClick()
            ElseIf e.KeyCode = Keys.NumPad6 Then
                ButtonNo6.PerformClick()
            ElseIf e.KeyCode = Keys.NumPad7 Then
                ButtonNo7.PerformClick()
            ElseIf e.KeyCode = Keys.NumPad8 Then
                ButtonNo8.PerformClick()
            ElseIf e.KeyCode = Keys.NumPad9 Then
                ButtonNo9.PerformClick()
            ElseIf e.KeyCode = Keys.Back Then
                ButtonClear.PerformClick()
            End If
        End If
    End Sub
    Private Sub ButtonNo9_Click(sender As Object, e As EventArgs) Handles ButtonNo9.Click
        If payment = False Then
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo9.Text)
            End If
        Else
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenterpayment(btntext:=ButtonNo9.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo8_Click(sender As Object, e As EventArgs) Handles ButtonNo8.Click
        If payment = False Then
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo8.Text)
            End If
        Else
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenterpayment(btntext:=ButtonNo8.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo7_Click(sender As Object, e As EventArgs) Handles ButtonNo7.Click
        If payment = False Then
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo7.Text)
            End If
        Else
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenterpayment(btntext:=ButtonNo7.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo6_Click(sender As Object, e As EventArgs) Handles ButtonNo6.Click
        If payment = False Then
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo6.Text)
            End If
        Else
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenterpayment(btntext:=ButtonNo6.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo5_Click(sender As Object, e As EventArgs) Handles ButtonNo5.Click
        If payment = False Then
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo5.Text)
            End If
        Else
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenterpayment(btntext:=ButtonNo5.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo4_Click(sender As Object, e As EventArgs) Handles ButtonNo4.Click
        If payment = False Then
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo4.Text)
            End If
        Else
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenterpayment(btntext:=ButtonNo4.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo3_Click(sender As Object, e As EventArgs) Handles ButtonNo3.Click
        If payment = False Then
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo3.Text)
            End If
        Else
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenterpayment(btntext:=ButtonNo3.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo2_Click(sender As Object, e As EventArgs) Handles ButtonNo2.Click
        If payment = False Then
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo2.Text)
            End If
        Else
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenterpayment(btntext:=ButtonNo2.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo1_Click(sender As Object, e As EventArgs) Handles ButtonNo1.Click
        If payment = False Then
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo1.Text)
            End If
        Else
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenterpayment(btntext:=ButtonNo1.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo0_Click(sender As Object, e As EventArgs) Handles ButtonNo0.Click
        If payment = False Then
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo0.Text)
            End If
        Else
            If TextBoxQTY.Text.Length > 6 Then
            Else
                buttonpressedenterpayment(btntext:=ButtonNo0.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo00_Click(sender As Object, e As EventArgs) Handles ButtonNo00.Click
        If payment = False Then
            If TextBoxQTY.Text.Length > 5 Then
            Else
                buttonpressedenter(btntext:=ButtonNo00.Text)
            End If
        Else
            If TextBoxQTY.Text.Length > 5 Then
            Else
                buttonpressedenterpayment(btntext:=ButtonNo00.Text)
            End If
        End If
    End Sub
    Private Sub Buttondot_Click(sender As Object, e As EventArgs) Handles Buttondot.Click
        If payment = False Then
            If Not TextBoxQTY.Text.Contains(".") Then
                TextBoxQTY.Text += "."
            End If
        End If
    End Sub
    Private Sub ButtonClear_Click(sender As Object, e As EventArgs) Handles ButtonClear.Click
        If payment = False Then
            TextBoxQTY.Text = 0
        End If
    End Sub
#End Region
#Region "POS Coupon Application/ Print/ Transaction"

    Public DISCOUNTTYPE As String = "N/A"
    Public TOTALDISCOUNT As Double = 0
    Public GROSSSALE As Double = 0
    Public VATEXEMPTSALES As Double = 0
    Public LESSVAT As Double = 0
    Public TOTALDISCOUNTEDAMOUNT As Double = 0
    Public TOTALAMOUNTDUE As Double = 0
    Public VATABLESALES As Double = 0
    Public VAT12PERCENT As Double = 0
    Public ZERORATEDSALES As Double = 0
    Dim THREADLIST As List(Of Thread) = New List(Of Thread)
    Dim TIMETOINSERT As String
    Dim ACTIVE As Integer = 1

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ButtonApplyCoupon.Click
        Enabled = False
        'GetProductHighestValue()
        GetHighest()
        CouponCode.Show()
        CouponCode.ButtonSubmit.Enabled = True
    End Sub
    Private Sub GetHighest()
        Try
            Dim HighestWafflesPrice As Double = 0
            Dim HighestDrinksPrice As Double = 0
            With DataGridViewOrders
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If .Rows(i).Cells(9).Value.ToString = "WAFFLE" Then
                        If HighestWafflesPrice < .Rows(i).Cells(2).Value Then
                            HighestWafflesPrice = .Rows(i).Cells(2).Value
                        End If
                    Else
                        If HighestDrinksPrice < .Rows(i).Cells(2).Value Then
                            HighestDrinksPrice = .Rows(i).Cells(2).Value
                        End If
                    End If
                Next
                SeniorPWd = HighestWafflesPrice
                SeniorPWdDrinks = HighestDrinksPrice
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

#Region "Transaction Process"
    Dim primary_value As Double = 0
    Dim secondary_value As Double = 0
    Dim serving_value As Double = 0
    Dim no_servings As Double = 0
    Dim stock_primary As Double = 0
    Dim stock_secondary As Double = 0
    Dim stock_no_of_servings As Double = 0
    Dim SERVVAL As Double
    Dim TOTALNOOFSERVINGS As Double = 0
    Dim TOTALSTOCKSEC As Double = 0
    Dim TOTALPRIMARYVAL As Double = 0
    Private Sub UpdateInventory()
        Try
            Dim Query As String = ""
            Dim SqlCommand As MySqlCommand
            Dim SqlAdapter As MySqlDataAdapter
            Dim SqlDt As DataTable
            With DataGridViewInv
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Query = "SELECT `primary_value`, `secondary_value`, `serving_value`, `no_servings` FROM `loc_product_formula` WHERE `formula_id` = " & .Rows(i).Cells(1).Value

                    SqlCommand = New MySqlCommand(Query, LocalhostConn())
                    SqlAdapter = New MySqlDataAdapter(SqlCommand)
                    SqlDt = New DataTable
                    SqlAdapter.Fill(SqlDt)
                    For Each row As DataRow In SqlDt.Rows
                        primary_value = row("primary_value")
                        secondary_value = row("secondary_value")
                        serving_value = row("serving_value")
                        no_servings = row("no_servings")
                    Next
                    LocalhostConn.Close()
                    Query = "SELECT `stock_secondary`, `stock_no_of_servings` FROM loc_pos_inventory WHERE `formula_id` = " & .Rows(i).Cells(1).Value
                    SqlCommand = New MySqlCommand(Query, LocalhostConn())
                    SqlAdapter = New MySqlDataAdapter(SqlCommand)
                    SqlDt = New DataTable
                    SqlAdapter.Fill(SqlDt)
                    For Each row As DataRow In SqlDt.Rows
                        stock_secondary = row("stock_secondary")
                        stock_no_of_servings = row("stock_no_of_servings")
                    Next
                    SERVVAL = .Rows(i).Cells(0).Value
                    TOTALSTOCKSEC = stock_secondary - SERVVAL
                    TOTALNOOFSERVINGS = stock_no_of_servings - .Rows(i).Cells(2).Value
                    TOTALPRIMARYVAL = TOTALSTOCKSEC / secondary_value
                    Query = "UPDATE loc_pos_inventory SET `stock_secondary` = " & TOTALSTOCKSEC & " , `stock_no_of_servings` = " & TOTALNOOFSERVINGS & " , `stock_primary` = " & TOTALPRIMARYVAL & " WHERE `formula_id` = " & .Rows(i).Cells(1).Value
                    SqlCommand = New MySqlCommand(Query, LocalhostConn())
                    SqlCommand.ExecuteNonQuery()
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertFMStock()
        Try
            For i As Integer = 0 To DataGridViewInv.Rows.Count - 1 Step +1
                table = "loc_fm_stock"
                fields = "(`formula_id`, `stock_primary`, `stock_secondary`,`crew_id`, `store_id`, `guid`, `created_at`, `status`)"
                value = "('" & DataGridViewInv.Rows(i).Cells(1).Value & "'  
                                ," & DataGridViewInv.Rows(i).Cells(2).Value & "    
                                ," & DataGridViewInv.Rows(i).Cells(0).Value & "                   
                                ,'" & ClientCrewID & "'
                                ,'" & ClientStoreID & "'
                                ,'" & ClientGuid & "'
                                ,'" & FullDate24HR() & "'                    
                                , " & 1 & ")"
                GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertDailyTransaction()
        Try
            Dim table As String = "loc_daily_transaction"
            Dim fields As String = "(`transaction_number`, `amounttendered`, `totaldiscount`, `change`, `amountdue`, `vatablesales`, `vatexemptsales`, `zeroratedsales`
                     , `lessvat`, `si_number`, `crew_id`, `guid`, `active`, `store_id`, `created_at`, `transaction_type`, `shift`, `zreading`, `synced`
                     , `discount_type`, `vatpercentage`, `grosssales`, `totaldiscountedamount`)"
            Dim value As String = "('" & TextBoxMAXID.Text & "'," & TEXTBOXMONEYVALUE & "," & TOTALDISCOUNT & "," & TEXTBOXCHANGEVALUE & "," & SUPERAMOUNTDUE & "," & VATABLESALES & "
                     ," & VATEXEMPTSALES & "," & ZERORATEDSALES & "," & LESSVAT & "," & SINumber & ",'" & ClientCrewID & "','" & ClientGuid & "','" & ACTIVE & "','" & ClientStoreID & "'
                     ,'" & INSERTTHISDATE & "','" & TRANSACTIONMODE & "','" & Shift & "','" & S_Zreading & "','Unsynced','" & DISCOUNTTYPE & "'," & VAT12PERCENT & "," & GROSSSALE & "," & TOTALDISCOUNTEDAMOUNT & ")"
            GLOBAL_INSERT_FUNCTION(table, fields, value)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertDailyDetails()
        Try
            For i As Integer = 0 To DataGridViewOrders.Rows.Count - 1 Step +1
                Dim totalcostofgoods As Decimal
                Dim table As String = "loc_daily_transaction_details"
                Dim fields As String = "(`product_id`,`product_sku`,`product_name`,`quantity`,`price`,`total`,`crew_id`,`transaction_number`,`active`,`created_at`,`guid`,`store_id`,`synced`,`total_cost_of_goods`,`product_category`,`zreading`)"
                For a As Integer = 0 To DataGridViewInv.Rows.Count - 1 Step +1
                    If DataGridViewInv.Rows(a).Cells(4).Value = DataGridViewOrders.Rows(i).Cells(0).Value Then
                        totalcostofgoods += DataGridViewInv.Rows(a).Cells(6).Value
                    End If
                Next
                Dim value As String = "(" & DataGridViewOrders.Rows(i).Cells(5).Value & "
                            ,'" & DataGridViewOrders.Rows(i).Cells(6).Value & "'
                            ,'" & DataGridViewOrders.Rows(i).Cells(0).Value & "'
                            , " & DataGridViewOrders.Rows(i).Cells(1).Value & "
                            , " & DataGridViewOrders.Rows(i).Cells(2).Value & "
                            , " & DataGridViewOrders.Rows(i).Cells(3).Value & "
                            , '" & ClientCrewID & "'
                            , '" & TextBoxMAXID.Text & "'
                            , " & ACTIVE & "
                            , '" & FullDate24HR() & "'
                            , '" & ClientGuid & "'
                            , '" & ClientStoreID & "'
                            , 'Unsynced'
                            , " & totalcostofgoods & "
                            , '" & DataGridViewOrders.Rows(i).Cells(7).Value & "'
                            , '" & S_Zreading & "')"
                GLOBAL_INSERT_FUNCTION(table, fields, value)
                totalcostofgoods = 0
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertModeofTransaction()
        Try
            Dim table As String = "loc_transaction_mode_details"
            Dim fields As String = "(`transaction_type`, `transaction_number`, `fullname`, `reference`, `markup`, `status`, `synced`, `store_id`, `guid`, `created_at`)"
            Dim value As String = "( '" & TRANSACTIONMODE & "'
                            ,'" & TextBoxMAXID.Text & "'
                            , '" & TEXTBOXFULLNAMEVALUE & "'
                            , '" & TEXTBOXREFERENCEVALUE & "'
                            , '" & TEXTBOXMARKUPVALUE & "'
                            , " & 1 & "
                            , 'Unsynced'
                            , '" & ClientStoreID & "'
                            , '" & ClientGuid & "'
                            , '" & FullDate24HR() & "')"
            GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value)
            ButtonClickCount = 0
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertCouponData()
        Try
            Dim table As String = "loc_coupon_data"
            Dim fields As String = "(`transaction_number`, `coupon_name`, `coupon_type`, `coupon_desc`, `coupon_line`, `coupon_total`)"
            Dim value As String = "( '" & TextBoxMAXID.Text & "'
                      ,'" & CouponName & "'
                      , '" & DISCOUNTTYPE & "'
                      , '" & CouponDesc & "'
                      , '" & CouponLine & "'
                      , '" & CouponTotal & "')"
            GLOBAL_INSERT_FUNCTION(table, fields, value)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
#End Region
    Dim INSERTTHISDATE

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            With WaitFrm
                INSERTTHISDATE = FullDate24HR()
                SUPERAMOUNTDUE = Convert.ToDecimal(Double.Parse(TextBoxGRANDTOTAL.Text))
                If TRANSACTIONMODE = "Representation Expenses" Then
                    ACTIVE = 3
                End If
                If Val(TextBoxDISCOUNT.Text) = 0 Then
                    VATABLESALES = Math.Round(SUPERAMOUNTDUE / Val(1 + S_Tax), 2, MidpointRounding.AwayFromZero)
                    VATEXEMPTSALES = 0.00
                    VAT12PERCENT = Math.Round(SUPERAMOUNTDUE - VATABLESALES, 2, MidpointRounding.AwayFromZero)
                    GROSSSALE = Math.Round(SUPERAMOUNTDUE, 2, MidpointRounding.AwayFromZero)
                End If
                sql = "SELECT si_number FROM loc_daily_transaction ORDER BY transaction_id DESC limit 1"
                cmd = New MySqlCommand(sql, LocalhostConn)
                da = New MySqlDataAdapter(cmd)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    SINumber = dt(0)(0)
                Else
                    SINumber = 0
                End If
                If SINumber = 0 Then
                    SINumber = 1
                    SiNumberToString = SINumber.ToString(S_SIFormat)
                Else
                    SINumber += 1
                    SiNumberToString = SINumber.ToString(S_SIFormat)
                End If
                For i = 0 To 100
                    BackgroundWorker1.ReportProgress(i)
                    If i = 0 Then
                        .Label1.Text = "Transaction is processing. Please wait."
                        thread = New Thread(AddressOf InsertFMStock)
                        thread.Start()
                        THREADLIST.Add(thread)
                        thread = New Thread(AddressOf UpdateInventory)
                        thread.Start()
                        THREADLIST.Add(thread)
                        thread = New Thread(AddressOf InsertDailyTransaction)
                        thread.Start()
                        THREADLIST.Add(thread)
                        thread = New Thread(AddressOf InsertDailyDetails)
                        thread.Start()
                        THREADLIST.Add(thread)
                        If modeoftransaction = True Then
                            thread = New Thread(AddressOf InsertModeofTransaction)
                            thread.Start()
                            THREADLIST.Add(thread)
                        End If
                        thread = New Thread(AddressOf InsertCouponData)
                        thread.Start()
                        THREADLIST.Add(thread)
                    End If
                    Thread.Sleep(10)
                Next
                For Each t In THREADLIST
                    t.Join()
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        With WaitFrm
            .ProgressBar1.Value = e.ProgressPercentage
            If e.ProgressPercentage = 20 Then
                .Label1.Text = "Transaction is processing. Please wait.."
            End If
            If e.ProgressPercentage = 40 Then
                .Label1.Text = "Transaction is processing. Please wait..."
            End If
            If e.ProgressPercentage = 60 Then
                .Label1.Text = "Transaction is processing. Please wait."
            End If
            If e.ProgressPercentage = 80 Then
                .Label1.Text = "Transaction is processing. Please wait.."
            End If
            If e.ProgressPercentage = 100 Then
                .Label1.Text = "Transaction is processing. Please wait..."
            End If
        End With
    End Sub
    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Enabled = True
        WaitFrm.Close()
        PaymentForm.Close()

        If DataGridViewOrders.Rows.Count > 0 Then
            b = 0
            Try
                For i As Integer = 0 To DataGridViewOrders.Rows.Count - 1 Step +1
                    'printdoc.PrinterSettings = printdoc.PrinterSettings
                    printdoc.DefaultPageSettings.PaperSize = New PaperSize("Custom", 200, 500 + b)
                    b += 10
                Next
                PrintPreviewDialog1.Document = printdoc
                'printdoc.Print()
                PrintPreviewDialog1.ShowDialog()
            Catch exp As Exception
                MessageBox.Show("An error occurred while trying to load the " &
                    "document for Print Preview. Make sure you currently have " &
                    "access to a printer. A printer must be localconnected and " &
                    "accessible for Print Preview to work.", Text,
                     MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            '=================================================================================================
            selectmax(whatform:=1)
            '=================================================================================================
            messageboxappearance = False
            SystemLogType = "TRANSACTION"
            SystemLogDesc = "Transaction of :" & returnfullname(ClientCrewID) & " Item(s): " & DataGridViewOrders.Rows.Count
            GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
            '=================================================================================================
            DataGridViewOrders.Rows.Clear()
            DataGridViewInv.Rows.Clear()
            modeoftransaction = False
            ButtonApplyCoupon.Enabled = False
            ButtonPay.Enabled = False
            Buttonholdoder.Enabled = False
            ButtonPendingOrders.Enabled = True
            payment = False
            Label76.Text = 0
            TextBoxDISCOUNT.Text = 0
            TRANSACTIONMODE = "Walk-In"
            CouponApplied = False
            CouponName = ""
            CouponDesc = ""
            ACTIVE = 1
            DISCOUNTTYPE = "N/A"
            TOTALDISCOUNT = 0
            GROSSSALE = 0
            VATEXEMPTSALES = 0
            LESSVAT = 0
            TOTALDISCOUNTEDAMOUNT = 0
            TOTALAMOUNTDUE = 0
            VATABLESALES = 0
            VAT12PERCENT = 0
            CouponLine = 10
        Else
            MsgBox("Select Transaction First!")
        End If
        a = 0
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printdoc.PrintPage
        Try
            MsgBox(DISCOUNTTYPE)
            Dim totalDisplay = Format(SUPERAMOUNTDUE, "##,##0.00")
            With Me
                a = 0
                ' Microsoft Sans Serif, 8.25pt
                Dim fontaddon As New Font("Tahoma", 5)
                Dim font1 As New Font("Tahoma", 6, FontStyle.Bold)
                Dim font2 As New Font("Tahoma", 7, FontStyle.Bold)
                Dim font As New Font("Tahoma", 6)
                ReceiptHeader(sender, e)
                Dim format1st As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
                Dim abc As Integer = 0
                For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                    Dim rect1st As RectangleF = New RectangleF(10.0F, 115 + abc, 173.0F, 100.0F)
                    Dim price = Format(.DataGridViewOrders.Rows(i).Cells(3).Value, "##,##0.00")

                    '=========================================================================================================================================================
                    If DataGridViewOrders.Rows(i).Cells(7).Value.ToString = "Add-Ons" Then
                        e.Graphics.DrawString("     @" & .DataGridViewOrders.Rows(i).Cells(1).Value & " " & .DataGridViewOrders.Rows(i).Cells(6).Value, fontaddon, Brushes.Black, rect1st)
                    Else
                        e.Graphics.DrawString(.DataGridViewOrders.Rows(i).Cells(1).Value & " " & .DataGridViewOrders.Rows(i).Cells(6).Value, font, Brushes.Black, rect1st)
                    End If
                    e.Graphics.DrawString(price, font, Brushes.Black, rect1st, format1st)
                    .a += 10
                    abc += 10
                    '=========================================================================================================================================================
                Next
                If CouponApplied = True Then
                    a += 100
                    SimpleTextDisplay(sender, e, CouponName & "(" & DISCOUNTTYPE & ")", font, 0, a)
                    SimpleTextDisplay(sender, e, CouponDesc, font, 0, a + 10)
                    a += 40 + CouponLine
                    RightToLeftDisplay(sender, e, a - 18, "Total Discount:", "P" & CouponTotal, font)
                Else
                    a += 120
                End If
                Dim Qty = SumOfColumnsToInt(.DataGridViewOrders, 1)
                If Val(TextBoxDISCOUNT.Text) < 1 Then
                    Dim format As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
                    Dim aNumber As Double = TEXTBOXMONEYVALUE
                    Dim cash = String.Format("{0:n2}", aNumber)
                    Dim aNumber1 As Double = TEXTBOXCHANGEVALUE
                    Dim change = String.Format("{0:n2}", aNumber1)
                    RightToLeftDisplay(sender, e, a, "AMOUNT DUE:", "P" & totalDisplay, font2)
                    RightToLeftDisplay(sender, e, a + 15, "CASH:", "P" & cash, font1)
                    RightToLeftDisplay(sender, e, a + 25, "CHANGE:", "P" & change, font1)
                    SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 23)
                    RightToLeftDisplay(sender, e, a + 52, "     Vatable", "    " & VATABLESALES, font)
                    RightToLeftDisplay(sender, e, a + 62, "     Vat Exempt Sales", "    " & "0.00", font)
                    RightToLeftDisplay(sender, e, a + 72, "     Zero Rated Sales", "    " & "0.00", font)
                    RightToLeftDisplay(sender, e, a + 82, "     VAT" & "(" & Val(S_Tax) * 100 & "%)", "    " & VAT12PERCENT, font)
                    RightToLeftDisplay(sender, e, a + 92, "     Less Vat", "    " & "0.00" & "-", font)
                    RightToLeftDisplay(sender, e, a + 102, "     Total", "    " & totalDisplay, font)
                    a += 4
                    SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 92)
                    a += 1
                    SimpleTextDisplay(sender, e, "Transaction Type: " & Trim(TRANSACTIONMODE), font, 0, a + 100)
                    SimpleTextDisplay(sender, e, "Total Item(s): " & Qty, font, 0, a + 110)
                    SimpleTextDisplay(sender, e, "Cashier: " & ClientCrewID & " " & returnfullname(where:=ClientCrewID), font, 0, a + 120)
                    SimpleTextDisplay(sender, e, "Str No: " & ClientStoreID, font, 110, a + 110)
                    SimpleTextDisplay(sender, e, "Date & Time: " & INSERTTHISDATE, font, 0, a + 130)
                    SimpleTextDisplay(sender, e, "Terminal No: " & S_Terminal_No, font, 110, a + 140)
                    SimpleTextDisplay(sender, e, "Ref. #: " & TextBoxMAXID.Text, font, 0, a + 140)
                    SimpleTextDisplay(sender, e, "SI No: " & SiNumberToString, font, 0, a + 150)
                    SimpleTextDisplay(sender, e, "This serves as your Sales Invoice", font, 0, a + 160)
                    SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 174)
                    ReceiptFooter(sender, e, a + 12)
                Else
                    Dim aNumber1 As Double = TEXTBOXCHANGEVALUE
                    Dim change = String.Format("{0:n2}", aNumber1)
                    Dim aNumber As Double = TEXTBOXMONEYVALUE
                    Dim cash = String.Format("{0:n2}", aNumber)
                    Dim format As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
                    RightToLeftDisplay(sender, e, a, "SUB TOTAL:", "P" & TextBoxSUBTOTAL.Text, font1)
                    RightToLeftDisplay(sender, e, a + 10, "DISCOUNT:", TOTALDISCOUNT & "-", font1)
                    RightToLeftDisplay(sender, e, a + 20, "AMOUNT DUE:", "P" & TOTALAMOUNTDUE, font2)
                    RightToLeftDisplay(sender, e, a + 30, "CASH:", "P" & cash, font1)
                    RightToLeftDisplay(sender, e, a + 40, "CHANGE:", "P" & change, font1)
                    If S_ZeroRated = "0" Then
                        SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 37)
                        RightToLeftDisplay(sender, e, a + 65, "     Vatable", "    " & VATABLESALES, font)
                        If DISCOUNTTYPE = "Percentage" Or DISCOUNTTYPE = "N/A" Then
                            RightToLeftDisplay(sender, e, a + 75, "     Vat Exempt Sales", "    " & VATEXEMPTSALES, font)
                        Else
                            RightToLeftDisplay(sender, e, a + 75, "     Vat Exempt Sales", "    " & "0.00", font)
                        End If
                        RightToLeftDisplay(sender, e, a + 85, "     Zero Rated Sales", "    " & "0.00", font)
                        RightToLeftDisplay(sender, e, a + 95, "     VAT" & "(" & Val(S_Tax) * 100 & "%)", "    " & VAT12PERCENT, font)
                        If DISCOUNTTYPE = "Percentage" Or DISCOUNTTYPE = "N/A" Then
                            RightToLeftDisplay(sender, e, a + 105, "     Less Vat", "    " & LESSVAT & "-", font)
                        Else
                            RightToLeftDisplay(sender, e, a + 105, "     Less Vat", "    " & "0.00" & "-", font)
                        End If

                        SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 101)
                        a += 4
                        SimpleTextDisplay(sender, e, "Transaction Type: " & Trim(TRANSACTIONMODE), font, 0, a + 110)
                        SimpleTextDisplay(sender, e, "Total Item(s): " & Qty, font, 0, a + 120)
                        SimpleTextDisplay(sender, e, "Cashier: " & ClientCrewID & " " & returnfullname(where:=ClientCrewID), font, 0, a + 130)
                        SimpleTextDisplay(sender, e, "Str No: " & ClientStoreID, font, 120, a + 120)
                        SimpleTextDisplay(sender, e, "Date & Time: " & INSERTTHISDATE, font, 0, a + 140)
                        SimpleTextDisplay(sender, e, "Terminal No: " & S_Terminal_No, font, 120, a + 150)
                        SimpleTextDisplay(sender, e, "Ref. #: " & TextBoxMAXID.Text, font, 0, a + 150)
                        SimpleTextDisplay(sender, e, "SI No: " & SiNumberToString, font, 0, a + 160)
                        SimpleTextDisplay(sender, e, "This serves as your Sales Invoice", font, 0, a + 170)
                        a += 6
                        SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 180)
                        a += 16
                        ReceiptFooter(sender, e, a)
                    Else
                        SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 47)
                        RightToLeftDisplay(sender, e, a + 72, "     Vatable", "    " & "0.00", font)
                        RightToLeftDisplay(sender, e, a + 82, "     Vat Exempt Sales", "    " & Val(TextBoxGRANDTOTAL.Text), font)
                        RightToLeftDisplay(sender, e, a + 92, "     Zero Rated Sales", "    " & "0.00", font)
                        RightToLeftDisplay(sender, e, a + 102, "    VAT", "    " & "0.00", font)
                        RightToLeftDisplay(sender, e, a + 112, "     Less Vat", "    " & LESSVAT & "-", font)
                        SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 102)
                        SimpleTextDisplay(sender, e, "Transaction Type: " & Trim(TRANSACTIONMODE), font, 0, a + 110)
                        SimpleTextDisplay(sender, e, "Total Item(s): " & Qty, font, 0, a + 120)
                        SimpleTextDisplay(sender, e, "Cashier: " & ClientCrewID & " " & returnfullname(where:=ClientCrewID), font, 0, a + 130)
                        SimpleTextDisplay(sender, e, "Str No: " & ClientStoreID, font, 120, a + 120)
                        SimpleTextDisplay(sender, e, "Date & Time: " & INSERTTHISDATE, font, 0, a + 140)
                        SimpleTextDisplay(sender, e, "Terminal No: " & S_Terminal_No, font, 120, a + 150)
                        SimpleTextDisplay(sender, e, "Ref. #: " & TextBoxMAXID.Text, font, 0, a + 15)
                        SimpleTextDisplay(sender, e, "SI No: " & SiNumberToString, font, 0, a + 160)
                        SimpleTextDisplay(sender, e, "This serves as your Sales Invoice", font, 0, a + 170)
                        SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 180)
                        ReceiptFooter(sender, e, a)
                    End If
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub


#End Region
End Class


