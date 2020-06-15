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
    Public VATABLE As Double
    Public vat As Decimal
    Public total
    Dim result As Integer
    Dim stockqty
    Dim stocktotal
    Dim insertcurrenttime As String
    Dim insertcurrentdate As String
    Dim discountgrandtotal As Double
    Dim thread As Thread
    Public VATEXEMPTSALES As Double
    Public LESSVAT As Double
    Public GRANDTOTALDISCOUNT As Double
    Public discounttype As String = "N/A"

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
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ButtonApplyCoupon.Click
        GetProductHighestValue()
        CouponCode.Show()
        CouponCode.ButtonSubmit.Enabled = True
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
    Private Sub ButtonCP_Click(sender As Object, e As EventArgs) Handles ButtonCP.Click
        Dim discount As Double = Val(TextBoxDISCOUNT.Text / 100)
        Dim discounttotal As Double = Val(TextBoxSUBTOTAL.Text * discount)
        discountgrandtotal = discounttotal
        TextBoxGRANDTOTAL.Text = TextBoxSUBTOTAL.Text - discounttotal
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
        If DataGridViewOrders.RowCount > 0 Then
            ButtonApplyCoupon.Enabled = True
        Else
            ButtonApplyCoupon.Enabled = False
        End If

        Label76.Text = SumOfColumnsToDecimal(DataGridViewOrders, 3)

        'Label76.Text = Format(Val(Label76.Text), "##,##0.00")

        TextBoxGRANDTOTAL.Text = Label76.Text
        TextBoxSUBTOTAL.Text = Label76.Text
        'If DiscountType = "All" Then
        '    TextBoxSUBTOTAL.Text = sum.ToString()
        '    Dim discount As Double = Val(TextBoxDISCOUNT.Text / 100)
        '    Dim discounttotal As Double = Val(TextBoxSUBTOTAL.Text * discount)
        '    discountgrandtotal = discounttotal
        '    TextBoxGRANDTOTAL.Text = TextBoxSUBTOTAL.Text - discounttotal
        'Else
        '    TextBoxSUBTOTAL.Text = sum.ToString()
        '    TextBoxGRANDTOTAL.Text = sum.ToString()
        'End If
    End Sub
    Private Sub POS_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Expenses.Dispose()
        Promo.Dispose()
        Couponisavailable = False
    End Sub
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
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        insertcurrenttime = TimeOfDay.ToString("HH:mm:ss")
        insertcurrentdate = String.Format("{0:yyyy/MM/dd}", DateTime.Now)
        Label11.Text = Date.Now.ToString("hh:mm:ss tt")
    End Sub

    Dim threadList As List(Of Thread) = New List(Of Thread)
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            With WaitFrm
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
                    Thread.Sleep(10)
                    BackgroundWorker1.ReportProgress(i)
                    If i = 0 Then
                        .Label1.Text = "Transaction is processing. Please wait."
                        thread = New Thread(AddressOf TranFunction)
                        thread.Start()
                        threadList.Add(thread)
                    End If
                Next
                For Each t In threadList
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
            transactionmode = "Walk-In"
            discounttype = "N/A"
            CouponApplied = False
            CouponName = ""
            CouponDesc = ""
            '=================================================================================================
        Else
            MsgBox("Select Transaction First!")
        End If
        a = 0
    End Sub
    Dim TIMETOINSERT As String
    Dim Active As Integer = 1
    Private Sub TranFunction()
        '=================================================================================================
        Try
            For i As Integer = 0 To DataGridViewInv.Rows.Count - 1 Step +1
                table = "loc_fm_stock"
                fields = "(`formula_id`, `stock_quantity`, `stock_total`,`crew_id`, `store_id`, `guid`, `date`, `time`, `status`)"
                value = "('" & DataGridViewInv.Rows(i).Cells(1).Value & "'  
                                ," & DataGridViewInv.Rows(i).Cells(2).Value & "    
                                ," & DataGridViewInv.Rows(i).Cells(0).Value & "                   
                                ,'" & ClientCrewID & "'
                                ,'" & ClientStoreID & "'
                                ,'" & ClientGuid & "'
                                ,'" & Format(Now, ("yyyy-MM-dd")) & "'                    
                                ,'" & Format(Now, ("hh:mm:ss")) & "'
                                , " & 1 & ")"
                GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, successmessage:=successmessage, errormessage:=errormessage)
                '=================================================================================================
                Try
                    sql = "SELECT stock_quantity, stock_total FROM loc_pos_inventory WHERE formula_id = " & DataGridViewInv.Rows(i).Cells(1).Value
                    cmd = New MySqlCommand
                    With cmd
                        .CommandText = sql
                        .Connection = LocalhostConn()
                        Using readerObj As MySqlDataReader = cmd.ExecuteReader
                            While readerObj.Read
                                stockqty = readerObj("stock_quantity").ToString
                                stocktotal = readerObj("stock_total").ToString
                            End While
                        End Using
                    End With
                    fields = "`stock_quantity` = " & stockqty - DataGridViewInv.Rows(i).Cells(2).Value & ", `stock_total` = " & stocktotal - DataGridViewInv.Rows(i).Cells(0).Value & ",`synced`= 'Unsynced'"
                    table = " loc_pos_inventory "
                    where = " inventory_id = " & DataGridViewInv.Rows(i).Cells(1).Value
                    GLOBAL_FUNCTION_UPDATE(table:=table, fields:=fields, where:=where)
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
                cmd.Dispose()
            Next
        Catch ex As Exception
        End Try
        '  =================================================================================================
        Try
            If transactionmode = "Representation Expenses" Then
                Active = 3
            End If
            total = SumOfColumnsToDecimal(datagrid:=DataGridViewOrders, celltocompute:=3)
            If TextBoxDISCOUNT.Text = "" Then
                VATABLE = 0.00
            Else
                VATABLE = Math.Round(total / 1.12, 2)
                LESSVAT = Math.Round(total - VATABLE, 2)
            End If
            table = "loc_daily_transaction"
            fields = "(`transaction_number`,`crew_id`,`guid`,`active`,`amounttendered`,`moneychange`,`amountdue`,`store_id`,`vatable`,`vat`,`date`,`time`,`discount`,`synced`,`transaction_type`,`shift`,`vat_exempt`,`si_number`,`zreading`,`discount_type`)"
            TIMETOINSERT = insertcurrenttime
            value = "('" & TextBoxMAXID.Text & "'                         
                            ,'" & ClientCrewID & "'
                            ,'" & ClientGuid & "'              
                            ," & Active & "
                            ,'" & TEXTBOXMONEYVALUE & "'
                            ,'" & TEXTBOXCHANGEVALUE & "'
                            ,'" & Double.Parse(TextBoxGRANDTOTAL.Text) & "'
                            ,'" & ClientStoreID & "'
                            , " & VATABLE & "
                            , " & LESSVAT & "
                            , '" & insertcurrentdate & "'
                            , '" & TIMETOINSERT & "'
                            , " & TextBoxDISCOUNT.Text & "
                            , 'Unsynced'
                            , '" & Trim(transactionmode) & "'
                            , '" & Shift & "'
                            , " & VATEXEMPTSALES & "
                            , " & SINumber & "
                            , '" & S_Zreading & "'
                            , '" & discounttype & "')"
            successmessage = "Success"
            errormessage = "Error holdorder(loc_daily_transaction)"
            GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, successmessage:=successmessage, errormessage:=errormessage)
        Catch ex As Exception
        End Try
        '=================================================================================================
        Try
            messageboxappearance = False
            For i As Integer = 0 To DataGridViewOrders.Rows.Count - 1 Step +1
                Dim totalcostofgoods As Decimal
                table = "loc_daily_transaction_details"
                fields = "(`product_id`,`product_sku`,`product_name`,`quantity`,`price`,`total`,`crew_id`,`transaction_number`,`active`,`created_at`,`timenow`,`guid`,`store_id`,`synced`,`total_cost_of_goods`,`product_category`,`zreading`)"

                For a As Integer = 0 To DataGridViewInv.Rows.Count - 1 Step +1
                    If DataGridViewInv.Rows(a).Cells(4).Value = DataGridViewOrders.Rows(i).Cells(0).Value Then
                        totalcostofgoods += DataGridViewInv.Rows(a).Cells(6).Value
                    End If
                Next
                value = "(" & DataGridViewOrders.Rows(i).Cells(5).Value & "
                            ,'" & DataGridViewOrders.Rows(i).Cells(6).Value & "'
                            ,'" & DataGridViewOrders.Rows(i).Cells(0).Value & "'
                            , " & DataGridViewOrders.Rows(i).Cells(1).Value & "
                            , " & DataGridViewOrders.Rows(i).Cells(2).Value & "
                            , " & DataGridViewOrders.Rows(i).Cells(3).Value & "
                            , '" & ClientCrewID & "'
                            , '" & TextBoxMAXID.Text & "'
                            , " & Active & "
                            , '" & insertcurrentdate & "'
                            , '" & insertcurrenttime & "'
                            , '" & ClientGuid & "'
                            , '" & ClientStoreID & "'
                            , 'Unsynced'
                            , " & totalcostofgoods & "
                            , '" & DataGridViewOrders.Rows(i).Cells(7).Value & "'
                            , '" & S_Zreading & "')"
                successmessage = "Success"
                errormessage = "error holdorder(loc_daily_transaction_details)"
                GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, successmessage:=successmessage, errormessage:=errormessage)
                totalcostofgoods = 0
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        '=================================================================================================
        Try
            If modeoftransaction = True Then
                table = "loc_transaction_mode_details"
                fields = "(`transaction_type`, `transaction_number`, `fullname`, `reference`, `markup`, `status`, `synced`, `store_id`, `guid`)"
                value = "( '" & transactionmode & "'
                            ,'" & TextBoxMAXID.Text & "'
                            , '" & TEXTBOXFULLNAMEVALUE & "'
                            , '" & TEXTBOXREFERENCEVALUE & "'
                            , '" & TEXTBOXMARKUPVALUE & "'
                            , " & 1 & "
                            , 'Unsynced'
                            , '" & ClientStoreID & "'
                            , '" & ClientGuid & "')"
                successmessage = "Success"
                errormessage = "error holdorder(loc_daily_transaction_details)"
                GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, successmessage:=successmessage, errormessage:=errormessage)
            End If
            ButtonClickCount = 0

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        '=================================================================================================
        Try
            table = "loc_coupon_data"
            fields = "(`transaction_number`, `coupon_name`, `coupon_type`, `coupon_desc`, `coupon_line`, `coupon_total`)"
            value = "( '" & TextBoxMAXID.Text & "'
                      ,'" & CouponName & "'
                      , '" & discounttype & "'
                      , '" & CouponDesc & "'
                      , '" & CouponLine & "'
                      , '" & CouponTotal & "')"
            successmessage = ""
            errormessage = ""
            GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, successmessage:=successmessage, errormessage:=errormessage)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printdoc.PrintPage
        With Me
            a = 0
            Dim font As New Font("Kelson Sans Normal", 7)
            Dim fontAddon As New Font("Kelson Sans Normal", 5)
            Dim font1 As New Font("Kelson Sans Normal", 7)
            Dim font2 As New Font("Kelson Sans Normal", 9)
            Dim font3 As New Font("Kelson Sans Normal", 11, FontStyle.Bold)
            Dim brandfont As New Font("Kelson Sans Normal", 8)
            'Dim font As New Font("Kelson Sans Normal", 7)
            'Dim fontAddon As New Font("Bahnschrift Light SemiCondensed", 5)
            'Dim font1 As New Font("Bahnschrift  SemiCondensed", 7)
            'Dim font2 As New Font("Bahnschrift Light SemiCondensed", 9)
            'Dim font3 As New Font("Bahnschrift Condensed", 12)
            'Dim brandfont As New Font("Bahnschrift Condensed", 8)
            Dim shopnameX As Integer = 10, shopnameY As Integer = 20
            Dim StrRight As New StringFormat()
            'HEADER
            ReceiptHeader(sender, e)
            'Items
            Dim format1st As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
            Dim abc As Integer = 0
            For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                Dim rect1st As RectangleF = New RectangleF(10.0F, 115 + abc, 173.0F, 100.0F)
                Dim price = Format(.DataGridViewOrders.Rows(i).Cells(3).Value, "##,##0.00")

                '=========================================================================================================================================================
                If DataGridViewOrders.Rows(i).Cells(7).Value.ToString = "Add-Ons" Then
                    e.Graphics.DrawString("     @" & .DataGridViewOrders.Rows(i).Cells(1).Value & " " & .DataGridViewOrders.Rows(i).Cells(6).Value, fontAddon, Brushes.Black, rect1st)
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
                SimpleTextDisplay(sender, e, CouponName & "(" & discounttype & ")", font, 0, a)
                SimpleTextDisplay(sender, e, CouponDesc, font, 0, a + 10)
                a += 40 + CouponLine
                RightToLeftDisplay(sender, e, a - 18, "Total Discount:", "P" & CouponTotal, font)
            Else
                a += 120
            End If

            If Val(TextBoxDISCOUNT.Text) < 1 Then
                'Total
                Dim format As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
                Dim aNumber As Double = TEXTBOXMONEYVALUE
                Dim cash = String.Format("{0:n2}", aNumber)
                Dim aNumber1 As Double = TEXTBOXCHANGEVALUE
                Dim change = String.Format("{0:n2}", aNumber1)
                'AMOUT DUE
                RightToLeftDisplay(sender, e, a, "AMOUNT DUE:", "P" & total, font3)
                'Cash
                RightToLeftDisplay(sender, e, a + 15, "CASH:", "P" & cash, font2)
                'Change
                RightToLeftDisplay(sender, e, a + 25, "CHANGE:", "P" & change, font2)
                'Vatable

                SimpleTextDisplay(sender, e, "********************************************************", font, 0, a + 27)
                'Vatable
                RightToLeftDisplay(sender, e, a + 52, "     Vatable", "    " & vatable, font)
                'Vat Exempt
                RightToLeftDisplay(sender, e, a + 62, "     Vat Exempt Sales", "    " & "0.00", font)
                'Zero Rated Sales
                RightToLeftDisplay(sender, e, a + 72, "     Zero Rated Sales", "    " & "0.00", font)
                'VAT
                RightToLeftDisplay(sender, e, a + 82, "     VAT" & "(" & Val(S_Tax) * 100 & "%)", "    " & Math.Round(total - VATABLE, 2) & "-", font)
                'Total
                RightToLeftDisplay(sender, e, a + 92, "     Total", "    " & total, font)
                'INFOS
                SimpleTextDisplay(sender, e, "********************************************************", font, 0, a + 82)
                SimpleTextDisplay(sender, e, "Transaction Type: " & Trim(transactionmode), font, 0, a + 90)
                SimpleTextDisplay(sender, e, "Total Item(s): " & .DataGridViewOrders.Rows.Count, font, 0, a + 100)
                SimpleTextDisplay(sender, e, "Cashier: " & ClientCrewID & " " & returnfullname(where:=ClientCrewID), font, 0, a + 110)
                SimpleTextDisplay(sender, e, "Str No: " & ClientStoreID, font, 110, a + 100)
                SimpleTextDisplay(sender, e, "Date & Time: " & insertcurrentdate & " " & TIMETOINSERT, font, 0, a + 120)
                SimpleTextDisplay(sender, e, "Terminal No: ", font, 110, a + 130)
                SimpleTextDisplay(sender, e, "Ref. #: " & TextBoxMAXID.Text, font, 0, a + 130)
                SimpleTextDisplay(sender, e, "SI No: " & SiNumberToString, font, 0, a + 140)
                SimpleTextDisplay(sender, e, "This serves as your Sales Invoice", font, 0, a + 150)
                SimpleTextDisplay(sender, e, "********************************************************", font, 0, a + 160)
                'Additional Info
                ReceiptFooter(sender, e, a - 10)
            Else
                Dim aNumber1 As Double = TEXTBOXCHANGEVALUE
                Dim change = String.Format("{0:n2}", aNumber1)
                Dim aNumber As Double = TEXTBOXMONEYVALUE
                Dim cash = String.Format("{0:n2}", aNumber)
                Dim format As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
                'amount due
                RightToLeftDisplay(sender, e, a, "AMOUNT DUE:", "P" & Math.Round(Val(TextBoxGRANDTOTAL.Text), 2), font3)
                'Sub total
                RightToLeftDisplay(sender, e, a + 15, "SUB TOTAL:", "P" & total, font2)
                'Discount
                Dim aNumber0 As Double = TextBoxDISCOUNT.Text
                Dim disc = String.Format(aNumber0)
                RightToLeftDisplay(sender, e, a + 25, "DISCOUNT:", disc & "-", font2)
                'cash
                RightToLeftDisplay(sender, e, a + 35, "CASH:", "P" & cash, font2)
                'change
                RightToLeftDisplay(sender, e, a + 45, "CHANGE:", "P" & change, font2)
                'vatable
                If S_ZeroRated = "0" Then
                    SimpleTextDisplay(sender, e, "********************************************************", font, 0, a + 47)
                    'Vatable
                    RightToLeftDisplay(sender, e, a + 72, "     Vatable", "    " & "0.00", font)
                    'Vat Exempt
                    RightToLeftDisplay(sender, e, a + 82, "     Vat Exempt Sales", "    " & VATEXEMPTSALES, font)
                    'Zero Rated Sales
                    RightToLeftDisplay(sender, e, a + 92, "     Zero Rated Sales", "    " & "0.00", font)
                    'VAT
                    RightToLeftDisplay(sender, e, a + 102, "     VAT" & "(" & Val(S_Tax) * 100 & "%)", "    " & LESSVAT & "-", font)
                    'BODY
                    SimpleTextDisplay(sender, e, "********************************************************", font, 0, a + 92)
                    SimpleTextDisplay(sender, e, "Transaction Type: " & Trim(transactionmode), font, 0, a + 100)
                    SimpleTextDisplay(sender, e, "Total Item(s): " & .DataGridViewOrders.Rows.Count, font, 0, a + 110)
                    SimpleTextDisplay(sender, e, "Cashier: " & ClientCrewID & " " & returnfullname(where:=ClientCrewID), font, 0, a + 120)
                    SimpleTextDisplay(sender, e, "Str No: " & ClientStoreID, font, 120, a + 110)
                    SimpleTextDisplay(sender, e, "Date & Time: " & insertcurrentdate & " " & TIMETOINSERT, font, 0, a + 130)
                    SimpleTextDisplay(sender, e, "Terminal No: ", font, 120, a + 140)
                    SimpleTextDisplay(sender, e, "Ref. #: " & TextBoxMAXID.Text, font, 0, a + 140)
                    SimpleTextDisplay(sender, e, "SI No: " & SiNumberToString, font, 0, a + 150)
                    SimpleTextDisplay(sender, e, "This serves as your Sales Invoice", font, 0, a + 160)
                    SimpleTextDisplay(sender, e, "********************************************************", font, 0, a + 170)
                    'INFOS.
                    ReceiptFooter(sender, e, a)
                Else
                    SimpleTextDisplay(sender, e, "********************************************************", font, 0, a + 47)
                    'Vatable
                    RightToLeftDisplay(sender, e, a + 72, "     Vatable", "    " & "0.00", font)
                    'Vat Exempt
                    RightToLeftDisplay(sender, e, a + 82, "     Vat Exempt Sales", "    " & Val(TextBoxGRANDTOTAL.Text), font)
                    'Zero Rated Sales
                    RightToLeftDisplay(sender, e, a + 92, "     Zero Rated Sales", "    " & "0.00", font)
                    'VAT
                    RightToLeftDisplay(sender, e, a + 102, "     VAT", "    " & "0.00", font)
                    'BODY
                    SimpleTextDisplay(sender, e, "********************************************************", font, 0, a + 92)
                    SimpleTextDisplay(sender, e, "Transaction Type: " & Trim(transactionmode), font, 0, a + 100)
                    SimpleTextDisplay(sender, e, "Total Item(s): " & .DataGridViewOrders.Rows.Count, font, 0, a + 110)
                    SimpleTextDisplay(sender, e, "Cashier: " & ClientCrewID & " " & returnfullname(where:=ClientCrewID), font, 0, a + 120)
                    SimpleTextDisplay(sender, e, "Str No: " & ClientStoreID, font, 120, a + 110)
                    SimpleTextDisplay(sender, e, "Date & Time: " & insertcurrentdate & " " & TIMETOINSERT, font, 0, a + 130)
                    SimpleTextDisplay(sender, e, "Terminal No: ", font, 120, a + 140)
                    SimpleTextDisplay(sender, e, "Ref. #: " & TextBoxMAXID.Text, font, 0, a + 140)
                    SimpleTextDisplay(sender, e, "SI No: " & SiNumberToString, font, 0, a + 150)
                    SimpleTextDisplay(sender, e, "This serves as your Sales Invoice", font, 0, a + 160)
                    SimpleTextDisplay(sender, e, "********************************************************", font, 0, a + 170)
                    'Dev Info
                    ReceiptFooter(sender, e, a)
                End If
            End If
        End With
    End Sub
    Private Sub ButtonCDISC_Click(sender As Object, e As EventArgs) Handles ButtonCDISC.Click
        TextBoxDISCOUNT.Text = 0
        ButtonCP.PerformClick()
        Couponisavailable = False
    End Sub
    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles ButtonTransactionMode.Click
        Enabled = False
        TransactionType.Show()
    End Sub
    Private Sub Panel14_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel14.Paint
        '    MyBase.OnPaint(e)
        '    Dim borderWidth As Integer = 1
        '    Dim theColor As Color = Color.Blue
        '    ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, theColor, borderWidth, ButtonBorderStyle.Solid, theColor, borderWidth, ButtonBorderStyle.Solid, theColor, borderWidth, ButtonBorderStyle.Solid, theColor, borderWidth, ButtonBorderStyle.Solid)
    End Sub
    Private Sub GetProductHighestValue()
        Try
            Dim max As Integer
            Dim maxDrinks As Integer

            Dim GroupSales = New DataTable
            GroupSales.Columns.Add("Sameval", GetType(Integer))
            GroupSales.Columns.Add("Total", GetType(Decimal))
            Dim RealDataTable = New DataTable
            RealDataTable.Columns.Add("Total", GetType(Integer))
            '===================================================
            Dim Drinks = New DataTable
            Drinks.Columns.Add("Sameval", GetType(Integer))
            Drinks.Columns.Add("Total", GetType(Decimal))
            Dim drinksDatatable = New DataTable
            drinksDatatable.Columns.Add("Total", GetType(Integer))
            '===================================================
            With DataGridViewOrders
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If .Rows(i).Cells(9).Value.ToString = "DRINKS" Then
                        Drinks.Rows.Add(.Rows(i).Cells(8).Value, .Rows(i).Cells(3).Value / .Rows(i).Cells(1).Value)
                    Else
                        GroupSales.Rows.Add(.Rows(i).Cells(8).Value, .Rows(i).Cells(3).Value / .Rows(i).Cells(1).Value)
                    End If
                Next
            End With
            '===================================================
            Dim query = From row In GroupSales
                        Group row By Sameval = row.Field(Of Int32)("Sameval") Into MonthGroup = Group
                        Select New With {
                            Key Sameval,
                            .Sales = MonthGroup.Sum(Function(r) r.Field(Of Decimal)("Total"))
                       }
            '===================================================
            Dim query2 = From row In Drinks
                         Group row By Sameval = row.Field(Of Int32)("Sameval") Into MonthGroup = Group
                         Select New With {
                            Key Sameval,
                            .Sales = MonthGroup.Sum(Function(r) r.Field(Of Decimal)("Total"))
                       }
            '===================================================
            For Each xs In query
                Console.WriteLine(xs.Sales)
                RealDataTable.Rows.Add(xs.Sales)
            Next
            For Each xss In query2
                Console.WriteLine(xss.Sales)
                drinksDatatable.Rows.Add(xss.Sales)
            Next
            '===================================================
            For i As Integer = 0 To RealDataTable.Rows.Count - 1 Step +1
                If i = 0 Then
                    max = RealDataTable(i)(0)
                End If
                If max < RealDataTable(i)(0) Then
                    max = RealDataTable(i)(0)
                End If
            Next
            '===================================================
            For i As Integer = 0 To drinksDatatable.Rows.Count - 1 Step +1
                If i = 0 Then
                    maxDrinks = drinksDatatable(i)(0)
                End If
                If maxDrinks < drinksDatatable(i)(0) Then
                    maxDrinks = drinksDatatable(i)(0)
                End If
            Next
            '===================================================
            SeniorPWd = max
            SeniorPWdDrinks = maxDrinks
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button1_Click_3(sender As Object, e As EventArgs) Handles Button1.Click
        Dim message = MessageBox.Show("Do you want to add Extra packaging?", "Extra packaging", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If message = DialogResult.Yes Then
            Try
                table = "loc_pos_inventory"
                fields = "stock_quantity = stock_quantity - 1, stock_total = stock_total - 1"
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
End Class


