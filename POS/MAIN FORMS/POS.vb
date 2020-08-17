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

    Public WaffleUpgrade As Boolean = False
    Public SeniorGC As Boolean = False

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If Application.OpenForms().OfType(Of SynctoCloud).Any Then
                SynctoCloud.BringToFront()
            End If
            LabelStorename.Text = ClientStorename
            Label11.Focus()
            Timer1.Start()
            Label76.Text = 0
            'listviewproductsshow(where:="Simply Perfect")
            selectmax(whatform:=1)
            DataGridViewOrders.Font = New Font("Tahoma", 11.25)
            LoadCategory()
            For Each btn As Button In Panel3.Controls.OfType(Of Button)()
                If btn.Text = "Simply Perfect" Then
                    btn.PerformClick()
                End If
            Next
            DataGridViewOrders.CellBorderStyle = DataGridViewCellBorderStyle.None
            Enabled = False
            BegBalance.Show()
            BegBalance.TopMost = True
            BackgroundWorker2.WorkerReportsProgress = True
            BackgroundWorker2.WorkerSupportsCancellation = True
            BackgroundWorker2.RunWorkerAsync()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadCategory()
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
                        .Font = New Font("Tahoma", 9, FontStyle.Bold)
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
        If Application.OpenForms().OfType(Of CouponCode).Any Then
            CouponCode.BringToFront()
        Else
            CouponCode.Show()
            CouponCode.ButtonSubmit.Enabled = False
        End If
    End Sub

    Private Sub Button38_Click(sender As Object, e As EventArgs) Handles ButtonEnter.Click
        If payment = False Then
            Try
                If TextBoxPRICE.Text = "" And TextBoxNAME.Text = "" Then
                    MsgBox("Select Product first!")
                Else
                    If TextBoxQTY.Text <> 0 Then
                        If DataGridViewOrders.Rows.Count > 0 Then
                            DataGridViewOrders.SelectedRows(0).Cells(1).Value = Val(TextBoxQTY.Text)
                            If DataGridViewOrders.SelectedRows(0).Cells(11).Value > 0 Then
                                Dim priceadd = DataGridViewOrders.SelectedRows(0).Cells(11).Value * S_Upgrade_Price
                                DataGridViewOrders.SelectedRows(0).Cells(3).Value = DataGridViewOrders.SelectedRows(0).Cells(1).Value * DataGridViewOrders.SelectedRows(0).Cells(2).Value + priceadd
                            Else
                                DataGridViewOrders.SelectedRows(0).Cells(3).Value = DataGridViewOrders.SelectedRows(0).Cells(1).Value * DataGridViewOrders.SelectedRows(0).Cells(2).Value
                            End If
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
    Private Sub ButtonPay_Click(sender As Object, e As EventArgs) Handles ButtonPayMent.Click
        If ButtonPayMent.Text = "Checkout" Then
            If Shift = "" Then
                MessageBox.Show("Input cashier balance first", "", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Else
                If S_Zreading <> Format(Now(), "yyyy-MM-dd") Then
                    MessageBox.Show("Z-read first", "Z-Reading", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Enabled = False
                    PaymentForm.Show()
                    Application.DoEvents()
                    PaymentForm.textboxmoney.Focus
                    PaymentForm.TextBoxTOTALPAY.Text = TextBoxGRANDTOTAL.Text
                    PaymentForm.Focus()
                End If
            End If
        Else
            BackgroundWorker3.WorkerReportsProgress = True
            BackgroundWorker3.WorkerSupportsCancellation = True
            BackgroundWorker3.RunWorkerAsync()
        End If
    End Sub
    Private Sub ButtonWaffleUpgrade_Click(sender As Object, e As EventArgs) Handles ButtonWaffleUpgrade.Click
        Try
            If WaffleUpgrade = False Then
                WaffleUpgrade = True
                ButtonWaffleUpgrade.Text = "Cancel Upgrade"
            Else
                WaffleUpgrade = False
                ButtonWaffleUpgrade.Text = "Brownie Upgrade"
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Mix()
        Try
            Dim inventory_id As Integer = 0
            Dim totalQuantity As Integer = 0
            Dim Ingredient As String = ""
            Dim Query As String = ""
            Dim SqlCommand As MySqlCommand
            Dim SqlAdapter As MySqlDataAdapter
            Dim SqlDt As DataTable = New DataTable

            Dim FORMPrimaryval As Double = 0
            Dim FORMSecondaryval As Double = 0
            Dim FORMServingval As Double = 0
            Dim FORMNoofservings As Double = 0

            Dim TotalPrimaryVal As Double = 0
            Dim TotalSecondaryVal As Double = 0
            Dim TotalNoOfServings As Double = 0

            Dim RetStockSec As Double = 0
            Dim RetStockPrim As Double = 0
            Dim RetNoServ As Double = 0

            With DataGridViewOrders
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    inventory_id = .Rows(i).Cells(10).Value
                    totalQuantity = .Rows(i).Cells(1).Value
                    Ingredient = .Rows(i).Cells(0).Value
                    Query = "SELECT `primary_value`, `secondary_value`, `serving_value`, `no_servings` FROM loc_product_formula WHERE server_formula_id = " & inventory_id
                    SqlCommand = New MySqlCommand(Query, LocalhostConn)
                    SqlAdapter = New MySqlDataAdapter(SqlCommand)
                    SqlDt = New DataTable
                    SqlAdapter.Fill(SqlDt)
                    For Each row As DataRow In SqlDt.Rows
                        FORMPrimaryval = row("primary_value")
                        FORMSecondaryval = row("secondary_value")
                        FORMServingval = row("serving_value")
                        FORMNoofservings = row("no_servings")
                    Next

                    TotalPrimaryVal = totalQuantity * FORMPrimaryval
                    TotalSecondaryVal = FORMSecondaryval * totalQuantity
                    TotalNoOfServings = FORMNoofservings * totalQuantity

                    Query = "SELECT `stock_primary`,`stock_secondary`,`stock_no_of_servings` FROM `loc_pos_inventory` WHERE server_inventory_id = " & inventory_id
                    SqlCommand = New MySqlCommand(Query, LocalhostConn)
                    SqlAdapter = New MySqlDataAdapter(SqlCommand)
                    SqlDt = New DataTable
                    SqlAdapter.Fill(SqlDt)
                    For Each row As DataRow In SqlDt.Rows
                        RetStockPrim = row("stock_primary")
                        RetStockSec = row("stock_secondary")
                        RetNoServ = row("stock_no_of_servings")
                    Next

                    Dim TotalPrimary As Double = RetStockPrim + TotalPrimaryVal
                    Dim Secondary As Double = RetStockSec + TotalSecondaryVal
                    Dim ServingValue As Double = RetNoServ + TotalNoOfServings

                    Query = "UPDATE loc_pos_inventory SET `stock_secondary` = " & Secondary & " , `stock_no_of_servings` = " & ServingValue & " , `stock_primary` = " & TotalPrimary & " WHERE `server_inventory_id` = " & inventory_id
                    SqlCommand = New MySqlCommand(Query, LocalhostConn())
                    SqlCommand.ExecuteNonQuery()
                    GLOBAL_SYSTEM_LOGS("MIX", "MIXED : " & Ingredient & ", Crew : " & ClientCrewID)
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim ThreadlistMIX As List(Of Thread) = New List(Of Thread)
    Dim ThreadMix As Thread
    Private Sub BackgroundWorker3_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker3.DoWork
        Try
            ThreadMix = New Thread(AddressOf Mix)
            ThreadMix.Start()
            ThreadlistMIX.Add(ThreadMix)
            For Each t In ThreadlistMIX
                t.Join()
            Next
            ThreadMix = New Thread(AddressOf UpdateInventory)
            ThreadMix.Start()
            ThreadlistMIX.Add(ThreadMix)
            For Each t In ThreadlistMIX
                t.Join()
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorker3_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker3.RunWorkerCompleted
        MessageBox.Show("Ingredient Mixed", "Mix Products", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Panel3.Enabled = True
        ButtonWaffleUpgrade.Enabled = True
        ButtonPayMent.Text = "Checkout"
        ButtonTransactionMode.Text = "Transaction Type"
        DataGridViewOrders.Rows.Clear()
        DataGridViewInv.Rows.Clear()
        DISABLESERVEROTHERSPRODUCT = False
    End Sub
    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Try
            ButtonCDISC.PerformClick()
            If DataGridViewOrders.Rows.Count > 0 Then
                datas = DataGridViewOrders.SelectedRows(0).Cells(0).Value.ToString()
                For x As Integer = DataGridViewInv.Rows.Count - 1 To 0 Step -1
                    If DataGridViewInv.Rows(x).Cells("Column10").Value = datas Then
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
                ButtonPayMent.Enabled = True
                ButtonPendingOrders.Enabled = False
                Buttonholdoder.Enabled = True
            Else

                HASOTHERSLOCALPRODUCT = False
                HASOTHERSSERVERPRODUCT = False

                DISABLESERVEROTHERSPRODUCT = False
                Panel3.Enabled = True
                ButtonWaffleUpgrade.Enabled = True
                ButtonPayMent.Text = "Checkout"
                ButtonTransactionMode.Text = "Transaction Type"
                ButtonClickCount = 0
                ButtonPayMent.Enabled = False
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
        'Promo.Dispose()
        Couponisavailable = False
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        insertcurrenttime = TimeOfDay.ToString("HH:mm:ss")
        insertcurrentdate = String.Format("{0:yyyy/MM/dd}", DateTime.Now)
        Label11.Text = Date.Now.ToString("hh:mm:ss tt")
        If LabelCheckingUpdates.Text = "Checking for updates." Then
            LabelCheckingUpdates.Text = "Checking for updates.."
        ElseIf LabelCheckingUpdates.Text = "Checking for updates.." Then
            LabelCheckingUpdates.Text = "Checking for updates..."
        ElseIf LabelCheckingUpdates.Text = "Checking for updates..." Then
            LabelCheckingUpdates.Text = "Checking for updates."
        End If
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
        CouponApplied = False
    End Sub
    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles ButtonTransactionMode.Click
        If ButtonTransactionMode.Text = "Transaction Type" Then
            Enabled = False
            TransactionType.Show()
        Else
            ButtonCancel.PerformClick()
            ButtonPayMent.Text = "Checkout"
            ButtonTransactionMode.Text = "Transaction Type"
            Panel3.Enabled = True
            ButtonWaffleUpgrade.Enabled = True
        End If
    End Sub
    Private Sub Button1_Click_3(sender As Object, e As EventArgs) Handles Button1.Click
        Enabled = False
        TakeOut.Show()

    End Sub
#Region "Button Functions"
    Private Sub POS_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyData = Keys.Alt + Keys.F4 Then
            e.Handled = True
        End If
        If e.KeyCode = Keys.F9 Then
            ButtonPayMent.PerformClick()
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
    Private Sub TextBoxQTY_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxQTY.KeyPress
        Try
            If InStr(DisallowedCharacters, e.KeyChar) > 0 Then
                e.Handled = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
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
    Dim THREADLISTUPDATE As List(Of Thread) = New List(Of Thread)
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
                            If .Rows(i).Cells(11).Value > 0 Then
                                Dim addprice = .Rows(i).Cells(11).Value * S_Upgrade_Price
                                HighestWafflesPrice = .Rows(i).Cells(2).Value + addprice
                            Else
                                HighestWafflesPrice = .Rows(i).Cells(2).Value
                            End If
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
    Dim secondary_value As Double = 0
    Dim stock_secondary As Double = 0

    Private Sub UpdateInventory()
        Dim SqlCommand As MySqlCommand
        Dim SqlAdapter As MySqlDataAdapter
        Dim SqlDt As DataTable
        Dim UpdateInventoryCon As MySqlConnection = LocalhostConn()
        Try
            Dim Query As String = ""
            With DataGridViewInv
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim TotalQuantity As Double = 0
                    Dim TotalServingValue As Double = 0
                    Dim Secondary As Double = 0
                    Dim ServingValue As Double = 0
                    Dim TotalPrimary As Double = 0
                    TotalQuantity = .Rows(i).Cells(2).Value
                    TotalServingValue = Double.Parse(.Rows(i).Cells(0).Value.ToString)
                    If .Rows(i).Cells(9).Value.ToString = "Server" Then
                        Query = "SELECT `secondary_value` FROM `loc_product_formula` WHERE formula_id = " & .Rows(i).Cells(1).Value
                    Else
                        Query = "SELECT `secondary_value` FROM `loc_product_formula` WHERE server_formula_id = " & .Rows(i).Cells(1).Value
                    End If
                    SqlCommand = New MySqlCommand(Query, UpdateInventoryCon)
                    SqlAdapter = New MySqlDataAdapter(SqlCommand)
                    SqlDt = New DataTable
                    SqlAdapter.Fill(SqlDt)
                    For Each row As DataRow In SqlDt.Rows
                        secondary_value = row("secondary_value")
                    Next
                    If .Rows(i).Cells(9).Value.ToString = "Server" Then
                        Query = "SELECT `stock_secondary` FROM `loc_pos_inventory` WHERE server_inventory_id = " & .Rows(i).Cells(1).Value
                    Else
                        Query = "SELECT `stock_secondary` FROM `loc_pos_inventory` WHERE inventory_id = " & .Rows(i).Cells(1).Value
                    End If
                    SqlCommand = New MySqlCommand(Query, UpdateInventoryCon)
                    SqlAdapter = New MySqlDataAdapter(SqlCommand)
                    SqlDt = New DataTable
                    SqlAdapter.Fill(SqlDt)
                    For Each row As DataRow In SqlDt.Rows
                        stock_secondary = row("stock_secondary")
                    Next
                    Secondary = stock_secondary - TotalServingValue
                    ServingValue = Secondary / Double.Parse(.Rows(i).Cells(5).Value.ToString)
                    TotalPrimary = Secondary / secondary_value
                    If .Rows(i).Cells(9).Value.ToString = "Server" Then
                        Query = "UPDATE loc_pos_inventory SET `stock_secondary` = " & Secondary & " , `stock_no_of_servings` = " & ServingValue & " , `stock_primary` = " & TotalPrimary & " WHERE `server_inventory_id` = " & .Rows(i).Cells(1).Value
                    Else
                        Query = "UPDATE loc_pos_inventory SET `stock_secondary` = " & Secondary & " , `stock_no_of_servings` = " & ServingValue & " , `stock_primary` = " & TotalPrimary & " WHERE `inventory_id` = " & .Rows(i).Cells(1).Value
                    End If
                    SqlCommand = New MySqlCommand(Query, UpdateInventoryCon)
                    SqlCommand.ExecuteNonQuery()
                Next
                UpdateInventoryCon.Close()
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
            Dim fields As String = " (`transaction_number`, `amounttendered`, `totaldiscount`, `change`, `amountdue`, `vatablesales`, `vatexemptsales`, `zeroratedsales`
                     , `lessvat`, `si_number`, `crew_id`, `guid`, `active`, `store_id`, `created_at`, `transaction_type`, `shift`, `zreading`, `synced`
                     , `discount_type`, `vatpercentage`, `grosssales`, `totaldiscountedamount`) "
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
                Dim fields As String = "(`product_id`,`product_sku`,`product_name`,`quantity`,`price`,`total`,`crew_id`,`transaction_number`,`active`,`created_at`,`guid`,`store_id`,`synced`,`total_cost_of_goods`,`product_category`,`zreading`,`transaction_type`,`upgraded`,`addontype`)"
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
                            , '" & S_Zreading & "'
                            , '" & TRANSACTIONMODE & "'
                            , " & DataGridViewOrders.Rows(i).Cells(11).Value & "
                            , '" & DataGridViewOrders.Rows(i).Cells(13).Value & "')"
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
    Private Sub InsertSeniorDetails()
        Try
            Dim table As String = "loc_senior_details"
            Dim fields As String = "(`transaction_number`, `senior_id`, `senior_name`, `active`, `crew_id`, `store_id`, `guid`, `date_created`, `synced`)"
            Dim value As String = "( '" & TextBoxMAXID.Text & "'
                      , '" & SeniorDetailsID & "'
                      , '" & SeniorDetailsName & "'
                      , '" & 1 & "'
                      , '" & ClientCrewID & "'
                      , '" & ClientStoreID & "'
                      , '" & ClientGuid & "'
                      , '" & FullDate24HR() & "'
                      , 'Unsynced')"
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
                        For Each t In THREADLIST
                            t.Join()
                        Next
                        thread = New Thread(AddressOf UpdateInventory)
                        thread.Start()
                        THREADLIST.Add(thread)
                        For Each t In THREADLIST
                            t.Join()
                        Next
                        thread = New Thread(AddressOf InsertDailyTransaction)
                        thread.Start()
                        THREADLIST.Add(thread)
                        For Each t In THREADLIST
                            t.Join()
                        Next
                        thread = New Thread(AddressOf InsertDailyDetails)
                        thread.Start()
                        THREADLIST.Add(thread)
                        For Each t In THREADLIST
                            t.Join()
                        Next
                        If modeoftransaction = True Then
                            thread = New Thread(AddressOf InsertModeofTransaction)
                            thread.Start()
                            THREADLIST.Add(thread)
                            For Each t In THREADLIST
                                t.Join()
                            Next
                        End If
                        If SENIORDETAILSBOOL = True Then
                            thread = New Thread(AddressOf InsertSeniorDetails)
                            thread.Start()
                            THREADLIST.Add(thread)
                            For Each t In THREADLIST
                                t.Join()
                            Next
                        End If
                        If CouponApplied = True Then
                            thread = New Thread(AddressOf InsertCouponData)
                            thread.Start()
                            THREADLIST.Add(thread)
                            For Each t In THREADLIST
                                t.Join()
                            Next
                        End If
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
            ButtonPayMent.Enabled = False
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
            DISABLESERVEROTHERSPRODUCT = False
            WaffleUpgrade = False
            ButtonWaffleUpgrade.Text = "Brownie Upgrade"
            SENIORDETAILSBOOL = False
            SeniorDetailsID = ""
            SeniorDetailsName = ""
        Else
            MsgBox("Select Transaction First!")
        End If
        a = 0
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printdoc.PrintPage
        Try
            Dim totalDisplay = Format(SUPERAMOUNTDUE, "##,##0.00")
            With Me
                a = 0
                ' Microsoft Sans Serif, 8.25pt
                Dim font1 As New Font("Tahoma", 6, FontStyle.Bold)
                Dim font2 As New Font("Tahoma", 7, FontStyle.Bold)
                Dim font As New Font("Tahoma", 6)
                Dim fontaddon As New Font("Tahoma", 5)
                ReceiptHeader(sender, e)
                Dim format1st As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
                Dim abc As Integer = 0
                For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                    Dim rect1st As RectangleF = New RectangleF(10.0F, 115 + abc, 173.0F, 100.0F)
                    Dim price = Format(.DataGridViewOrders.Rows(i).Cells(3).Value, "##,##0.00")

                    If DataGridViewOrders.Rows(i).Cells(7).Value.ToString = "Add-Ons" Then
                        If DataGridViewOrders.Rows(i).Cells(13).Value.ToString = "Classic" Then
                            RightToLeftDisplay(sender, e, abc + 115, "     @" & .DataGridViewOrders.Rows(i).Cells(0).Value, price, fontaddon, 0, 0)
                        Else
                            RightToLeftDisplay(sender, e, abc + 115, .DataGridViewOrders.Rows(i).Cells(1).Value & " " & DataGridViewOrders.Rows(i).Cells(0).Value, price, font, 0, 0)
                        End If
                    Else
                        RightToLeftDisplay(sender, e, abc + 115, .DataGridViewOrders.Rows(i).Cells(1).Value & " " & DataGridViewOrders.Rows(i).Cells(0).Value, price, font, 0, 0)
                        If DataGridViewOrders.Rows(i).Cells(11).Value > 0 Then
                            abc += 10
                            .a += 10
                            RightToLeftDisplay(sender, e, abc + 115, "     + UPGRADE BRWN " & DataGridViewOrders.Rows(i).Cells(11).Value, "", fontaddon, 0, 0)

                        End If
                    End If
                    .a += 10
                    abc += 10
                Next
                If CouponApplied = True Then
                    a += 100
                    SimpleTextDisplay(sender, e, CouponName & "(" & DISCOUNTTYPE & ")", font, 0, a)
                    SimpleTextDisplay(sender, e, CouponDesc, font, 0, a + 10)
                    a += 40 + CouponLine
                    RightToLeftDisplay(sender, e, a - 18, "Total Discount:", "P" & CouponTotal, font, 0, 0)
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
                    RightToLeftDisplay(sender, e, a, "AMOUNT DUE:", "P" & totalDisplay, font2, 0, 0)
                    RightToLeftDisplay(sender, e, a + 15, "CASH:", "P" & cash, font1, 0, 0)
                    RightToLeftDisplay(sender, e, a + 25, "CHANGE:", "P" & change, font1, 0, 0)
                    SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 23)
                    RightToLeftDisplay(sender, e, a + 52, "     Vatable", "    " & VATABLESALES, font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 62, "     Vat Exempt Sales", "    " & "0.00", font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 72, "     Zero Rated Sales", "    " & "0.00", font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 82, "     VAT" & "(" & Val(S_Tax) * 100 & "%)", "    " & VAT12PERCENT, font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 92, "     Less Vat", "    " & "0.00" & "-", font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 102, "     Total", "    " & totalDisplay, font, 0, 0)
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
                    RightToLeftDisplay(sender, e, a, "SUB TOTAL:", "P" & TextBoxSUBTOTAL.Text, font1, 0, 0)
                    RightToLeftDisplay(sender, e, a + 10, "DISCOUNT:", TOTALDISCOUNT & "-", font1, 0, 0)
                    RightToLeftDisplay(sender, e, a + 20, "AMOUNT DUE:", "P" & TOTALAMOUNTDUE, font2, 0, 0)
                    RightToLeftDisplay(sender, e, a + 30, "CASH:", "P" & cash, font1, 0, 0)
                    RightToLeftDisplay(sender, e, a + 40, "CHANGE:", "P" & change, font1, 0, 0)
                    If S_ZeroRated = "0" Then
                        SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 37)
                        RightToLeftDisplay(sender, e, a + 65, "     Vatable", "    " & VATABLESALES, font, 0, 0)
                        If DISCOUNTTYPE = "Percentage(w/o vat)" Then
                            RightToLeftDisplay(sender, e, a + 75, "     Vat Exempt Sales", "    " & VATEXEMPTSALES, font, 0, 0)
                        Else
                            If SeniorGC = False Then
                                RightToLeftDisplay(sender, e, a + 75, "     Vat Exempt Sales", "    " & "0.00", font, 0, 0)
                            Else
                                RightToLeftDisplay(sender, e, a + 75, "     Vat Exempt Sales", "    " & VATEXEMPTSALES, font, 0, 0)
                            End If
                        End If
                        RightToLeftDisplay(sender, e, a + 85, "     Zero Rated Sales", "    " & "0.00", font, 0, 0)
                        RightToLeftDisplay(sender, e, a + 95, "     VAT" & "(" & Val(S_Tax) * 100 & "%)", "    " & VAT12PERCENT, font, 0, 0)
                        If DISCOUNTTYPE = "Percentage(w/o vat)" Then
                            RightToLeftDisplay(sender, e, a + 105, "     Less Vat", "    " & LESSVAT & "-", font, 0, 0)
                        Else
                            If SeniorGC = False Then
                                RightToLeftDisplay(sender, e, a + 105, "     Less Vat", "    " & "0.00" & "-", font, 0, 0)
                            Else
                                RightToLeftDisplay(sender, e, a + 105, "     Less Vat", "    " & LESSVAT & "-", font, 0, 0)
                            End If
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
                        RightToLeftDisplay(sender, e, a + 72, "     Vatable", "    " & "0.00", font, 0, 0)
                        RightToLeftDisplay(sender, e, a + 82, "     Vat Exempt Sales", "    " & Val(TextBoxGRANDTOTAL.Text), font, 0, 0)
                        RightToLeftDisplay(sender, e, a + 92, "     Zero Rated Sales", "    " & "0.00", font, 0, 0)
                        RightToLeftDisplay(sender, e, a + 102, "    VAT", "    " & "0.00", font, 0, 0)
                        RightToLeftDisplay(sender, e, a + 112, "     Less Vat", "    " & LESSVAT & "-", font, 0, 0)
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
#Region "Updates"
#Region "Categories Update"

    Private Sub CheckPriceChanges()
        Try
            Dim ConnectionServer As MySqlConnection = ServerCloudCon()
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()

            Dim Query = "SELECT * FROM admin_price_request WHERE store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' AND synced = 'Unsynced' AND active = 2"
            Dim CmdCheck As MySqlCommand = New MySqlCommand(Query, ConnectionServer)
            Dim DaCheck As MySqlDataAdapter = New MySqlDataAdapter(CmdCheck)
            Dim DtCheck As DataTable = New DataTable
            DaCheck.Fill(DtCheck)

            For i As Integer = 0 To DtCheck.Rows.Count - 1 Step +1
                Dim sql = "UPDATE loc_admin_products SET product_price = " & DtCheck(i)(3) & ", price_change = 1 WHERE server_product_id = " & DtCheck(i)(2) & ""
                CmdCheck = New MySqlCommand(sql, ConnectionLocal)
                CmdCheck.ExecuteNonQuery()
                Dim sql2 = "UPDATE loc_price_request_change SET active = " & DtCheck(i)(5) & " WHERE request_id = " & DtCheck(i)(0) & ""
                CmdCheck = New MySqlCommand(sql2, ConnectionLocal)
                CmdCheck.ExecuteNonQuery()
                Dim sq3 = "UPDATE admin_price_request SET synced = 'Synced' WHERE request_id = " & DtCheck(i)(0) & ""
                CmdCheck = New MySqlCommand(sq3, ConnectionServer)
                CmdCheck.ExecuteNonQuery()
            Next
            If DtCheck.Rows.Count > 0 Then
                PRICECHANGE = True
            Else
                PRICECHANGE = False
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Function LoadCategoryLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        dtlocal.Columns.Add("updated_at")
        dtlocal.Columns.Add("category_id")
        Dim dtlocal1 As DataTable = New DataTable
        Try
            Dim sql = "SELECT updated_at, category_id FROM loc_admin_category"
            cmdlocal = New MySqlCommand(sql, LocalhostConn())
            dalocal = New MySqlDataAdapter(cmdlocal)
            dalocal.Fill(dtlocal1)
            For i As Integer = 0 To dtlocal1.Rows.Count - 1 Step +1
                Dim Cat As DataRow = dtlocal.NewRow
                Cat("updated_at") = dtlocal1(i)(0).ToString
                Cat("category_id") = dtlocal1(i)(1)
                dtlocal.Rows.Add(Cat)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dtlocal
    End Function
    Private Sub Function1()
        Try
            Dim Query = "SELECT * FROM loc_admin_category"
            Dim CmdCheck As MySqlCommand = New MySqlCommand(Query, LocalhostConn)
            Dim DaCheck As MySqlDataAdapter = New MySqlDataAdapter(CmdCheck)
            Dim DtCheck As DataTable = New DataTable
            DaCheck.Fill(DtCheck)
            Dim cmdserver As MySqlCommand
            Dim daserver As MySqlDataAdapter
            Dim dtserver As DataTable
            If DtCheck.Rows.Count < 1 Then
                Dim sql = "SELECT `category_id`, `category_name`, `brand_name`, `updated_at`, `origin`, `status` FROM admin_category"
                cmdserver = New MySqlCommand(sql, ServerCloudCon())
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    DataGridView1.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3).ToString, dtserver(i)(4), dtserver(i)(5))
                Next
            Else
                Dim Ids As String = ""
                If ValidCloudConnection = True Then
                    For i As Integer = 0 To LoadCategoryLocal.Rows.Count - 1 Step +1
                        If Ids = "" Then
                            Ids = "" & LoadCategoryLocal(i)(1) & ""
                        Else
                            Ids += "," & LoadCategoryLocal(i)(1) & ""
                        End If
                    Next
                    Dim sql = "SELECT `category_id`, `category_name`, `brand_name`, `updated_at`, `origin`, `status` FROM admin_category WHERE category_id IN (" & Ids & ")"
                    cmdserver = New MySqlCommand(sql, ServerCloudCon())
                    daserver = New MySqlDataAdapter(cmdserver)
                    dtserver = New DataTable
                    daserver.Fill(dtserver)
                    For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                        If LoadCategoryLocal(i)(0).ToString <> dtserver(i)(3).ToString Then
                            DataGridView1.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3).ToString, dtserver(i)(4), dtserver(i)(5))

                        End If
                    Next
                    Dim sql2 = "SELECT `category_id`, `category_name`, `brand_name`, `updated_at`, `origin`, `status` FROM admin_category WHERE category_id NOT IN (" & Ids & ")"
                    cmdserver = New MySqlCommand(sql2, ServerCloudCon())
                    daserver = New MySqlDataAdapter(cmdserver)
                    dtserver = New DataTable
                    daserver.Fill(dtserver)
                    For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                        If LoadCategoryLocal(i)(0) <> dtserver(i)(3) Then
                            DataGridView1.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3).ToString, dtserver(i)(4), dtserver(i)(5))

                        End If
                    Next
                End If
            End If
        Catch ex As Exception

            BackgroundWorker2.CancelAsync()
            'If table doesnt have data
        End Try
    End Sub
#End Region
#Region "Products Update"

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If CheckForInternetConnection() = True Then
            UPDATEPRODUCTONLY = True
            Button3.Enabled = False
            BackgroundWorker2.WorkerReportsProgress = True
            BackgroundWorker2.WorkerSupportsCancellation = True
            BackgroundWorker2.RunWorkerAsync()
            LabelCheckingUpdates.Text = "Checking for updates."
        Else
            MsgBox("Internet connection is not available")
        End If
    End Sub
    Dim UPDATEPRODUCTONLY As Boolean = False
    Dim FillDatagridProduct As DataTable
    Private Sub GetProducts()
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim ConnectionServer As MySqlConnection = ServerCloudCon()

            FillDatagridProduct = New DataTable
            FillDatagridProduct.Columns.Add("product_id")
            FillDatagridProduct.Columns.Add("product_sku")
            FillDatagridProduct.Columns.Add("product_name")
            FillDatagridProduct.Columns.Add("formula_id")
            FillDatagridProduct.Columns.Add("product_barcode")
            FillDatagridProduct.Columns.Add("product_category")
            FillDatagridProduct.Columns.Add("product_price")
            FillDatagridProduct.Columns.Add("product_desc")
            FillDatagridProduct.Columns.Add("product_image")
            FillDatagridProduct.Columns.Add("product_status")
            FillDatagridProduct.Columns.Add("origin")
            FillDatagridProduct.Columns.Add("date_modified")
            FillDatagridProduct.Columns.Add("inventory_id")
            FillDatagridProduct.Columns.Add("addontype")

            Dim Query = "SELECT * FROM loc_admin_products"
            Dim CmdCheck As MySqlCommand = New MySqlCommand(Query, ConnectionLocal)
            Dim DaCheck As MySqlDataAdapter = New MySqlDataAdapter(CmdCheck)
            Dim DtCheck As DataTable = New DataTable
            DaCheck.Fill(DtCheck)
            If DtCheck.Rows.Count < 1 Then
                GetAllProducts()
            Else
                Dim DtCount As DataTable

                Dim SqlCount = "SELECT COUNT(product_id) FROM admin_products_org"
                Dim CmdCount As MySqlCommand = New MySqlCommand(SqlCount, ConnectionServer)
                Dim result As Integer = CmdCount.ExecuteScalar
                Dim DaCount As MySqlDataAdapter
                Dim FillDt As DataTable = New DataTable

                For a = 1 To result
                    Dim Query1 As String = "SELECT date_modified, price_change FROM loc_admin_products WHERE server_product_id = " & a
                    Dim cmd As MySqlCommand = New MySqlCommand(Query1, ConnectionLocal)
                    DaCount = New MySqlDataAdapter(cmd)
                    FillDt = New DataTable
                    DaCount.Fill(FillDt)
                    Dim Prod As DataRow = FillDatagridProduct.NewRow
                    If FillDt.Rows.Count > 0 Then
                        Dim PriceChange = FillDt(0)(1)
                        'Exist then check for update
                        Query1 = "SELECT * FROM admin_products_org WHERE product_id = " & a
                        cmd = New MySqlCommand(Query1, ConnectionServer)
                        DaCount = New MySqlDataAdapter(cmd)
                        DtCount = New DataTable
                        DaCount.Fill(DtCount)
                        If FillDt(0)(0).ToString <> DtCount(0)(11) Then
                            Prod("product_id") = DtCount(0)(0)
                            Prod("product_sku") = DtCount(0)(1)
                            Prod("product_name") = DtCount(0)(2)
                            Prod("formula_id") = DtCount(0)(3)
                            Prod("product_barcode") = DtCount(0)(4)
                            Prod("product_category") = DtCount(0)(5)
                            If FillDt(0)(1) = 1 Then
                                Dim sql2 = "SELECT product_price FROM loc_admin_products WHERE server_product_id = " & a
                                Dim cmd2 As MySqlCommand = New MySqlCommand(sql2, LocalhostConn)
                                Dim da2 As MySqlDataAdapter = New MySqlDataAdapter(cmd2)
                                Dim dt2 As DataTable = New DataTable
                                da2.Fill(dt2)
                                Prod("product_price") = dt2(0)(0)
                            Else
                                Prod("product_price") = DtCount(0)(6)
                            End If
                            Prod("product_desc") = DtCount(0)(7)
                            Prod("product_image") = DtCount(0)(8)
                            Prod("product_status") = DtCount(0)(9)
                            Prod("origin") = DtCount(0)(10)
                            Prod("date_modified") = DtCount(0)(11)
                            Prod("inventory_id") = DtCount(0)(12)
                            Prod("addontype") = DtCount(0)(13)
                            FillDatagridProduct.Rows.Add(Prod)
                        End If
                    Else
                        'Insert new product
                        Query1 = "SELECT * FROM admin_products_org WHERE product_id = " & a
                        cmd = New MySqlCommand(Query1, ConnectionServer)
                        DaCount = New MySqlDataAdapter(cmd)
                        DtCount = New DataTable
                        DaCount.Fill(DtCount)
                        Prod("product_id") = DtCount(0)(0)
                        Prod("product_sku") = DtCount(0)(1)
                        Prod("product_name") = DtCount(0)(2)
                        Prod("formula_id") = DtCount(0)(3)
                        Prod("product_barcode") = DtCount(0)(4)
                        Prod("product_category") = DtCount(0)(5)
                        Prod("product_price") = DtCount(0)(6)
                        Prod("product_desc") = DtCount(0)(7)
                        Prod("product_image") = DtCount(0)(8)
                        Prod("product_status") = DtCount(0)(9)
                        Prod("origin") = DtCount(0)(10)
                        Prod("date_modified") = DtCount(0)(11)
                        Prod("inventory_id") = DtCount(0)(12)
                        Prod("addontype") = DtCount(0)(13)
                        FillDatagridProduct.Rows.Add(Prod)
                    End If
                Next
                ConnectionLocal.Close()
                ConnectionServer.Close()
            End If
        Catch ex As Exception
            BackgroundWorker2.CancelAsync()
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub GetAllProducts()
        Try
            Dim Connection As MySqlConnection = ServerCloudCon()
            Dim SqlCount = "SELECT COUNT(product_id) FROM admin_products_org"
            Dim CmdCount As MySqlCommand = New MySqlCommand(SqlCount, Connection)
            Dim result As Integer = CmdCount.ExecuteScalar
            Dim Cmd As MySqlCommand
            FillDatagridProduct = New DataTable
            FillDatagridProduct.Columns.Add("product_id")
            FillDatagridProduct.Columns.Add("product_sku")
            FillDatagridProduct.Columns.Add("product_name")
            FillDatagridProduct.Columns.Add("formula_id")
            FillDatagridProduct.Columns.Add("product_barcode")
            FillDatagridProduct.Columns.Add("product_category")
            FillDatagridProduct.Columns.Add("product_price")
            FillDatagridProduct.Columns.Add("product_desc")
            FillDatagridProduct.Columns.Add("product_image")
            FillDatagridProduct.Columns.Add("product_status")
            FillDatagridProduct.Columns.Add("origin")
            FillDatagridProduct.Columns.Add("date_modified")
            FillDatagridProduct.Columns.Add("inventory_id")
            FillDatagridProduct.Columns.Add("addontype")
            Dim DaCount As MySqlDataAdapter
            Dim FillDt As DataTable = New DataTable
            For a = 1 To result
                Dim Query As String = "SELECT * FROM admin_products_org WHERE product_id = " & a
                Cmd = New MySqlCommand(Query, Connection)
                DaCount = New MySqlDataAdapter(Cmd)
                FillDt = New DataTable
                DaCount.Fill(FillDt)
                For i As Integer = 0 To FillDt.Rows.Count - 1 Step +1
                    Dim Prod As DataRow = FillDatagridProduct.NewRow
                    Prod("product_id") = FillDt(i)(0)
                    Prod("product_sku") = FillDt(i)(1)
                    Prod("product_name") = FillDt(i)(2)
                    Prod("formula_id") = FillDt(i)(3)
                    Prod("product_barcode") = FillDt(i)(4)
                    Prod("product_category") = FillDt(i)(5)
                    Prod("product_price") = FillDt(i)(6)
                    Prod("product_desc") = FillDt(i)(7)
                    Prod("product_image") = FillDt(i)(8)
                    Prod("product_status") = FillDt(i)(9)
                    Prod("origin") = FillDt(i)(10)
                    Prod("date_modified") = FillDt(i)(11)
                    Prod("inventory_id") = FillDt(i)(12)
                    Prod("addontype") = FillDt(i)(13)
                    FillDatagridProduct.Rows.Add(Prod)
                Next
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
#End Region
#Region "Formulas Update"
    Private Function LoadFormulaLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        dtlocal.Columns.Add("server_date_modified")
        dtlocal.Columns.Add("server_formula_id")
        Dim dtlocal1 As DataTable = New DataTable
        Try
            Dim sql = "SELECT server_date_modified, server_formula_id FROM loc_product_formula"
            cmdlocal = New MySqlCommand(sql, LocalhostConn)
            dalocal = New MySqlDataAdapter(cmdlocal)
            dalocal.Fill(dtlocal1)
            For i As Integer = 0 To dtlocal1.Rows.Count - 1 Step +1
                Dim Cat As DataRow = dtlocal.NewRow
                Cat("server_date_modified") = dtlocal1(i)(0).ToString
                Cat("server_formula_id") = dtlocal1(i)(1)
                dtlocal.Rows.Add(Cat)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dtlocal
    End Function
    Private Sub Function3()
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim ConnectionServer As MySqlConnection = ServerCloudCon()
            Dim FormulaLocal = LoadFormulaLocal()

            Dim Query = "SELECT * FROM loc_product_formula"
            Dim CmdCheck As MySqlCommand = New MySqlCommand(Query, ConnectionLocal)
            Dim DaCheck As MySqlDataAdapter = New MySqlDataAdapter(CmdCheck)
            Dim DtCheck As DataTable = New DataTable
            DaCheck.Fill(DtCheck)
            Dim cmdserver As MySqlCommand
            Dim daserver As MySqlDataAdapter
            Dim dtserver As DataTable
            If DtCheck.Rows.Count < 1 Then
                Dim sql = "SELECT `server_formula_id`, `product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin` FROM admin_product_formula_org"
                cmdserver = New MySqlCommand(sql, ConnectionServer)
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    DataGridView3.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8), dtserver(i)(9), dtserver(i)(10).ToString, dtserver(i)(11), dtserver(i)(12))
                Next
            Else
                Dim Ids As String = ""

                If ValidCloudConnection = True Then
                    For i As Integer = 0 To FormulaLocal.Rows.Count - 1 Step +1
                        If Ids = "" Then
                            Ids = "" & FormulaLocal(i)(1) & ""
                        Else
                            Ids += "," & FormulaLocal(i)(1) & ""
                        End If
                    Next
                    Dim sql = "SELECT `server_formula_id`, `product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin` FROM admin_product_formula_org WHERE server_formula_id  IN (" & Ids & ") "
                    cmdserver = New MySqlCommand(sql, ConnectionServer)
                    daserver = New MySqlDataAdapter(cmdserver)
                    dtserver = New DataTable
                    daserver.Fill(dtserver)
                    For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                        If FormulaLocal(i)(0).ToString <> dtserver(i)(10).ToString Then
                            DataGridView3.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8), dtserver(i)(9), dtserver(i)(10).ToString, dtserver(i)(11), dtserver(i)(12))
                        End If
                    Next
                    Dim sql2 = "SELECT `server_formula_id`, `product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin` FROM admin_product_formula_org WHERE server_formula_id NOT IN (" & Ids & ") "
                    cmdserver = New MySqlCommand(sql2, ConnectionServer)
                    daserver = New MySqlDataAdapter(cmdserver)
                    dtserver = New DataTable
                    daserver.Fill(dtserver)
                    For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                        If FormulaLocal(i)(0).ToString <> dtserver(i)(10) Then
                            DataGridView3.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8), dtserver(i)(9), dtserver(i)(10).ToString, dtserver(i)(11), dtserver(i)(12))
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            BackgroundWorker2.CancelAsync()
            MsgBox(ex.ToString)
        End Try
    End Sub
#End Region
#Region "Inventory Update"
    Private Function LoadInventoryLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        dtlocal.Columns.Add("server_date_modified")
        dtlocal.Columns.Add("server_inventory_id")
        Dim dtlocal1 As DataTable = New DataTable
        Try
            Dim sql = "SELECT server_date_modified , server_inventory_id FROM loc_pos_inventory"
            cmdlocal = New MySqlCommand(sql, LocalhostConn)
            dalocal = New MySqlDataAdapter(cmdlocal)
            dalocal.Fill(dtlocal)
            For i As Integer = 0 To dtlocal1.Rows.Count - 1 Step +1
                Dim Cat As DataRow = dtlocal.NewRow
                Cat("server_date_modified") = dtlocal1(i)(0).ToString
                Cat("server_inventory_id") = dtlocal1(i)(1)
                dtlocal.Rows.Add(Cat)
            Next
            LocalhostConn.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dtlocal
    End Function
    Private Sub Function4()
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim ConnectionServer As MySqlConnection = ServerCloudCon()
            Dim InventoryLocal = LoadInventoryLocal()

            Dim Query = "SELECT * FROM loc_pos_inventory"
            Dim CmdCheck As MySqlCommand = New MySqlCommand(Query, ConnectionLocal)
            Dim DaCheck As MySqlDataAdapter = New MySqlDataAdapter(CmdCheck)
            Dim DtCheck As DataTable = New DataTable
            DaCheck.Fill(DtCheck)
            Dim cmdserver As MySqlCommand
            Dim daserver As MySqlDataAdapter
            Dim dtserver As DataTable
            If DtCheck.Rows.Count < 1 Then
                Dim sql = "SELECT `server_inventory_id`, `product_ingredients`, `sku`, `stock_primary`, `stock_secondary`, `stock_no_of_servings`, `stock_status`, `critical_limit`, `date_modified`, `main_inventory_id`, `origin` FROM admin_pos_inventory_org"
                cmdserver = New MySqlCommand(sql, ConnectionServer)
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    DataGridView4.Rows.Add(dtserver(i)(0), 0, dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8).ToString, dtserver(i)(9).ToString, dtserver(i)(10).ToString)
                Next
            Else
                Dim Ids As String = ""

                If ValidCloudConnection = True Then
                    For i As Integer = 0 To InventoryLocal.Rows.Count - 1 Step +1
                        If Ids = "" Then
                            Ids = "" & InventoryLocal(i)(1) & ""
                        Else
                            Ids += "," & InventoryLocal(i)(1) & ""
                        End If
                    Next
                    Dim sql = "SELECT `server_inventory_id`, `product_ingredients`, `sku`, `stock_primary`, `stock_secondary`, `stock_no_of_servings`, `stock_status`, `critical_limit`, `date_modified`,`main_inventory_id`, `origin` FROM admin_pos_inventory_org WHERE server_inventory_id IN (" & Ids & ")"
                    cmdserver = New MySqlCommand(sql, ConnectionServer)
                    daserver = New MySqlDataAdapter(cmdserver)
                    dtserver = New DataTable
                    daserver.Fill(dtserver)
                    For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                        If InventoryLocal(i)(0).ToString <> dtserver(i)(8).ToString Then
                            DataGridView4.Rows.Add(dtserver(i)(0), 0, dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8).ToString, dtserver(i)(9).ToString, dtserver(i)(10).ToString)
                        End If
                    Next
                    Dim sql2 = "SELECT `server_inventory_id`, `product_ingredients`, `sku`, `stock_primary`, `stock_secondary`, `stock_no_of_servings`, `stock_status`, `critical_limit`, `date_modified`,`main_inventory_id`, `origin` FROM admin_pos_inventory_org WHERE server_inventory_id NOT IN (" & Ids & ")"
                    cmdserver = New MySqlCommand(sql2, ConnectionServer)
                    daserver = New MySqlDataAdapter(cmdserver)
                    dtserver = New DataTable
                    daserver.Fill(dtserver)
                    For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                        If InventoryLocal(i)(0).ToString <> dtserver(i)(8) Then
                            DataGridView4.Rows.Add(dtserver(i)(0), 0, dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8).ToString, dtserver(i)(9).ToString, dtserver(i)(10).ToString)

                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            BackgroundWorker2.CancelAsync()
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public POSISUPDATING As Boolean = False
    Dim PRICECHANGE As Boolean = False
    Private Sub BackgroundWorker2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork
        Try
            If ValidDatabaseLocalConnection Then
                thread = New Thread(AddressOf ServerCloudCon)
                thread.Start()
                THREADLISTUPDATE.Add(thread)
                For Each t In THREADLISTUPDATE
                    t.Join()
                Next
                If CheckForInternetConnection() = True Then
                    If ServerCloudCon.state = ConnectionState.Open Then
                        If UPDATEPRODUCTONLY = False Then
                            POSISUPDATING = True
                            thread = New Thread(AddressOf CheckPriceChanges)
                            thread.Start()
                            THREADLISTUPDATE.Add(thread)

                            For Each t In THREADLISTUPDATE
                                t.Join()
                            Next
                            thread = New Thread(AddressOf Function1)
                            thread.Start()
                            THREADLISTUPDATE.Add(thread)
                            thread = New Thread(AddressOf GetProducts)
                            thread.Start()
                            THREADLISTUPDATE.Add(thread)
                            thread = New Thread(AddressOf Function3)
                            thread.Start()
                            THREADLISTUPDATE.Add(thread)
                            thread = New Thread(AddressOf Function4)
                            thread.Start()
                            THREADLISTUPDATE.Add(thread)
                        Else
                            thread = New Thread(AddressOf CheckPriceChanges)
                            thread.Start()
                            THREADLISTUPDATE.Add(thread)
                            For Each t In THREADLISTUPDATE
                                t.Join()
                            Next
                            thread = New Thread(AddressOf GetProducts)
                            thread.Start()
                            THREADLISTUPDATE.Add(thread)
                        End If
                    Else
                    End If
                End If

                For Each t In THREADLISTUPDATE
                    t.Join()
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorker2_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker2.RunWorkerCompleted
        Try
            If PRICECHANGE = True Then
                listviewproductsshow(where:="Simply Perfect")
                MsgBox("Product price changes approved")
            End If
            PRICECHANGE = False
            If CheckForInternetConnection() = True Then
                DataGridView2.DataSource = FillDatagridProduct
                Button3.Enabled = True
                UPDATEPRODUCTONLY = False
                POSISUPDATING = False
                If DataGridView1.Rows.Count > 0 Or DataGridView2.Rows.Count > 0 Or DataGridView3.Rows.Count > 0 Or DataGridView4.Rows.Count > 0 Then
                    Dim updatemessage = MessageBox.Show("New Updates are available. Would you like to update now ?", "New Updates", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    If updatemessage = DialogResult.Yes Then
                        InstallUpdatesFormula()
                        InstallUpdatesInventory()
                        InstallUpdatesCategory()
                        InstallUpdatesProducts()
                        For Each btn As Button In Panel3.Controls.OfType(Of Button)()
                            If btn.Text = "Simply Perfect" Then
                                btn.PerformClick()
                            End If
                        Next
                        LabelCheckingUpdates.Text = "Update Completed."
                    Else
                        LabelCheckingUpdates.Text = "Completed."
                    End If
                Else
                    LabelCheckingUpdates.Text = "Complete Checking! No updates found."
                End If
            Else
                Button3.Enabled = True
                LabelCheckingUpdates.Text = "Internet connection is not available."
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InstallUpdatesCategory()
        Try
            Dim cmdlocal As MySqlCommand
            With DataGridView1
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim sql = "SELECT category_id FROM loc_admin_category WHERE category_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                    If result = 0 Then
                        Dim sqlinsert = "INSERT INTO `loc_admin_category`(`category_name`, `brand_name`, `updated_at`, `origin`, `status`) VALUES (@0,@1,@2,@3,@4)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    Else
                        Dim sqlupdate = "UPDATE `loc_admin_category` SET `category_name`=@0,`brand_name`=@1,`updated_at`=@2,`origin`=@3,`status`=@4 WHERE category_id = " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlupdate, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InstallUpdatesFormula()
        Try
            Dim cmdlocal As MySqlCommand
            With DataGridView3
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim sql = "SELECT formula_id FROM loc_product_formula WHERE formula_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                    If result = 0 Then
                        Dim sqlinsert = "INSERT INTO loc_product_formula (`server_formula_id`,`product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin`, `store_id`, `guid`, `crew_id`, `server_date_modified`) VALUES
                                        (@0 ,@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11 , @12 , @13 , @14, @15, @16)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Int64).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value
                        cmdlocal.Parameters.Add("@11", MySqlDbType.Decimal).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@16", MySqlDbType.Text).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    Else
                        Dim sqlupdate = "UPDATE `loc_product_formula` SET `server_formula_id`= @0,`product_ingredients`= @1,`primary_unit`= @2,`primary_value`= @3,`secondary_unit`= @4,`secondary_value`=@5,`serving_unit`=@6,`serving_value`=@7,`no_servings`=@8,`status`=@9,`date_modified`=@10,`unit_cost`=@11,`origin`=@12,`store_id`=@13,`guid`=@14,`crew_id`=@15,`server_date_modified`=@16 WHERE server_formula_id =  " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlupdate, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Int64).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value
                        cmdlocal.Parameters.Add("@11", MySqlDbType.Decimal).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@16", MySqlDbType.Text).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InstallUpdatesInventory()
        Try
            Dim cmdlocal As MySqlCommand
            With DataGridView4
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim sql = "SELECT inventory_id FROM loc_pos_inventory WHERE inventory_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                    If result = 0 Then
                        Dim sqlinsert = "INSERT INTO loc_pos_inventory (`server_inventory_id`,`formula_id`,`product_ingredients`,`sku`,`stock_primary`,`stock_secondary`,`stock_no_of_servings`,`stock_status`,`critical_limit`,`created_at`,`server_date_modified`,`store_id`,`crew_id`,`guid`,`synced`,`main_inventory_id`,`origin`) VALUES
                                        (@0 ,@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15 , @16)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.Int64).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.Int64).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.Int64).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = "Synced"
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.Parameters.Add("@16", MySqlDbType.Text).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    Else
                        ',`stock_primary`=@4,`stock_secondary`=@5,`stock_no_of_servings`=@6
                        Dim sqlUpdate = "UPDATE `loc_pos_inventory` SET `server_inventory_id`= @0,`formula_id`=@1,`product_ingredients`=@2,`sku`=@3,`stock_status`=@7,`critical_limit`=@8,`created_at`=@9,`server_date_modified`=@10,`store_id`=@11,`crew_id`=@12,`guid`=@13,`synced`=@14,`main_inventory_id`=@15,`origin`=@16 WHERE `server_inventory_id`= " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlUpdate, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.Int64).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        'cmdlocal.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                        'cmdlocal.Parameters.Add("@5", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                        'cmdlocal.Parameters.Add("@6", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.Int64).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.Int64).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = "Synced"
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.Parameters.Add("@16", MySqlDbType.Text).Value = .Rows(i).Cells(11).Value.ToString()

                        cmdlocal.ExecuteNonQuery()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InstallUpdatesProducts()
        Try
            Dim cmdlocal As MySqlCommand
            With DataGridView2
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim sql = "SELECT product_id FROM loc_admin_products WHERE product_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                    If result = 0 Then
                        Dim sqlinsert = "INSERT INTO loc_admin_products (`server_product_id`, `product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `server_inventory_id`, `guid`, `store_id`, `crew_id`, `synced`, `addontype`) VALUES
                                        (@0 ,@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16, @17)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.Int64).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.Text).Value = .Rows(i).Cells(12).Value.ToString()

                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@14", MySqlDbType.Int64).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@16", MySqlDbType.VarChar).Value = "Synced"
                        cmdlocal.Parameters.Add("@17", MySqlDbType.Text).Value = .Rows(i).Cells(13).Value.ToString()

                        cmdlocal.ExecuteNonQuery()
                    Else
                        ',`formula_id`=@3
                        Dim sqlupdate = "UPDATE `loc_admin_products` SET `server_product_id`=@0,`product_sku`=@1,`product_name`=@2,`product_barcode`=@4,`product_category`=@5,`product_price`=@6,`product_desc`=@7,`product_image`=@8,`product_status`=@9,`origin`=@10,`date_modified`=@11,`server_inventory_id`=@12,`guid`=@13,`store_id`=@14,`crew_id`=@15,`synced`=@16,`addontype`=@17 WHERE server_product_id =  " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlupdate, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        'cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.Int64).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.Text).Value = .Rows(i).Cells(12).Value.ToString()

                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@14", MySqlDbType.Int64).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@16", MySqlDbType.VarChar).Value = "Synced"

                        cmdlocal.Parameters.Add("@17", MySqlDbType.Text).Value = .Rows(i).Cells(13).Value.ToString()

                        cmdlocal.ExecuteNonQuery()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

#End Region
#End Region
End Class


