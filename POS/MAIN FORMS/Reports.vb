Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing
Imports System.Threading
Public Class Reports
    Private WithEvents printdoc As PrintDocument = New PrintDocument
    Private WithEvents printdocXread As PrintDocument = New PrintDocument
    Private WithEvents printdocInventory As PrintDocument = New PrintDocument
    Private WithEvents printdocReturns As PrintDocument = New PrintDocument
    Private WithEvents printsales As PrintDocument = New PrintDocument
    Private WithEvents printtransactiontype As PrintDocument = New PrintDocument

    Private PrintPreviewDialog1 As New PrintPreviewDialog
    Private PrintPreviewDialogXread As New PrintPreviewDialog
    Private PrintPreviewDialogInventory As New PrintPreviewDialog
    Private PrintPreviewDialogReturns As New PrintPreviewDialog
    Private previewsales As New PrintPreviewDialog
    Private previewtransactiontype As New PrintPreviewDialog


    Dim buttons As DataGridViewButtonColumn = New DataGridViewButtonColumn()
    Dim user_id As String
    Dim pagingAdapter As MySqlDataAdapter
    Dim pagingDS As DataSet
    Dim scrollVal As Integer
    Dim fullname As String
    Dim tbl As String
    Dim flds As String
    Public Shared transaction_number As String
    Dim a = 0
    Dim b = 0
    Dim data As String
    Dim data2 As String
    Dim total
    Dim ReadingOR
    Private Sub Reports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            TabControl1.TabPages(0).Text = "Daily Transactions"
            TabControl1.TabPages(1).Text = "System Logs"
            TabControl1.TabPages(2).Text = "Sales Report"
            TabControl1.TabPages(3).Text = "Expense Report"
            TabControl1.TabPages(4).Text = "Transaction Logs"
            TabControl1.TabPages(5).Text = "Crew Sales"
            TabControl1.TabPages(6).Text = "Item Return"
            TabControl1.TabPages(7).Text = "Deposit Slip"
            TabControl1.TabPages(8).Text = "Z/X Reading"
            ComboBoxTransactionType.SelectedIndex = 0

            reportsdailytransaction(False)
            reportssystemlogs(False)
            reportssales(False)
            reportstransactionlogs(False)
            expensereports(False)
            LoadUsers()

            reportsreturnsandrefunds(False)
            viewdeposit(False)
            FillDatagridZreadInv(False)
            LoadCrewSales(False)
            If ClientRole = "Admin" Then
                ButtonZreadAdmin.Visible = True
                Button8.Visible = True
            Else
                ButtonZreadAdmin.Visible = False
                Button8.Visible = False
            End If


            If S_Zreading = Format(Now(), "yyyy-MM-dd") Then
                ButtonZread.Enabled = False
                ButtonZreadAdmin.Enabled = False
            End If
            If DataGridViewDaily.Rows.Count > 0 Then
                Dim arg = New DataGridViewCellEventArgs(0, 0)
                DataGridViewDaily_CellClick(sender, arg)
            End If

            If DataGridViewEXPENSES.Rows.Count > 0 Then
                'Dim arg = New DataGridViewCellEventArgs(0, 0)
                'DataGridViewEXPENSES_CellClick(sender, arg)
            End If
            With DataGridViewTransactionDetails
                .Columns.Item(2).DefaultCellStyle.Format = "n2"
                .Columns.Item(3).DefaultCellStyle.Format = "n2"
                .RowHeadersVisible = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .AllowUserToOrderColumns = False
                .AllowUserToResizeColumns = False
                .AllowUserToResizeRows = False
                .Font = New Font("tahoma", 10)
                .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
                .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub LoadCrewSales(bool As Boolean)
        Try
            DataGridViewCrewSales.Rows.Clear()
            Dim query As String = ""
            Dim Table As String = ""
            Dim Fields As String = "dt.transaction_number, dt.grosssales, SUM(dtd.quantity) , dt.crew_id, dt.created_at"
            If bool = False Then
                Table = "loc_daily_transaction dt LEFT JOIN loc_daily_transaction_details dtd ON dt.transaction_number = dtd.transaction_number WHERE DATE_FORMAT(dt.created_at , '%y-%m-%d') = DATE_FORMAT(CURDATE(), '%y-%m-%d') AND dt.crew_id = '" & ClientCrewID & "' GROUP BY dt.created_at"
            Else
                Table = "loc_daily_transaction dt LEFT JOIN loc_daily_transaction_details dtd ON dt.transaction_number = dtd.transaction_number WHERE dt.zreading >= '" & Format(DateTimePicker5.Value, "yyyy-MM-dd") & "' AND dt.zreading <= '" & Format(DateTimePicker6.Value, "yyyy-MM-dd") & "' AND dt.crew_id = '" & ComboBoxUserIDS.Text & "' GROUP BY dt.created_at"
            End If
            Dim CrewSalesDt = AsDatatable(Table, Fields, DataGridViewCrewSales)
            For Each row As DataRow In CrewSalesDt.Rows
                DataGridViewCrewSales.Rows.Add(row("transaction_number"), row("grosssales"), row("SUM(dtd.quantity)"), row("crew_id"), row("created_at"))
            Next

            LabelCrewSalesQty.Text = SumOfColumnsToDecimal(DataGridViewCrewSales, 2)
            LabelCrewSalesTotal.Text = SumOfColumnsToDecimal(DataGridViewCrewSales, 1)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub LoadUsers()
        Try
            Dim sql = "Select uniq_id FROM loc_users"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
            Using reader As MySqlDataReader = cmd.ExecuteReader
                While reader.Read
                    ComboBoxUserIDS.Items.Add(reader("uniq_id"))
                End While
            End Using
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub reportssystemlogs(ByVal searchdate As Boolean)
        Try
            table = "`loc_system_logs`"
            fields = "`log_type`, `log_description`, `log_date_time`"
            If searchdate = False Then
                where = " WHERE Date(log_date_time) = CURRENT_DATE() And log_type <> 'TRANSACTION' AND log_store = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' ORDER BY log_date_time DESC"
            Else
                where = " WHERE log_type <> 'TRANSACTION' AND log_store = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' AND date(log_date_time) >= '" & Format(DateTimePicker9.Value, "yyyy-MM-dd") & "' AND date(log_date_time) <= '" & Format(DateTimePicker10.Value, "yyyy-MM-dd") & "' ORDER BY  log_date_time DESC"
            End If
            With DataGridViewSysLog
                .Columns(0).HeaderText = "Type"
                .Columns(1).HeaderText = "Description"
                .Columns(2).HeaderText = "Date and Time"
            End With
            Dim AsDt = AsDatatable(table & where, "`log_type`, `log_description`, `log_date_time`", DataGridViewSysLog)
            Dim Desc As String = ""
            Dim Type As String = ""
            For Each row As DataRow In AsDt.Rows
                If row("log_type") = "BG-1" Then
                    row("log_type") = "Balance"
                    row("log_description") = "Begginning Balance : Shift 1 : " & row("log_description")
                ElseIf row("log_type") = "BG-2" Then
                    row("log_type") = "Balance"
                    row("log_description") = "Begginning Balance : Shift 2 : " & row("log_description")
                ElseIf row("log_type") = "BG-3" Then
                    row("log_type") = "Balance"
                    row("log_description") = "Begginning Balance : Shift 3 : " & row("log_description")
                ElseIf row("log_type") = "BG-4" Then
                    row("log_type") = "Balance"
                    row("log_description") = "Begginning Balance : Shift 4 : " & row("log_description")
                End If
                DataGridViewSysLog.Rows.Add(row("log_type"), row("log_description"), row("log_date_time"))
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub reportsreturnsandrefunds(ByVal searchdate As Boolean)
        Try
            table = "`loc_refund_return_details`"
            fields = "`transaction_number`, `crew_id`, `reason`, `created_at`"
            If searchdate = False Then
                where = " date(zreading) = CURRENT_DATE() AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewReturns, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " date(zreading) >= '" & Format(DateTimePicker14.Value, "yyyy-MM-dd") & "' AND date(zreading) <= '" & Format(DateTimePicker13.Value, "yyyy-MM-dd") & "' AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewReturns, errormessage:="", fields:=fields, successmessage:="", where:=where)
            End If
            With DataGridViewReturns
                .Columns(0).HeaderText = "Transaction #"
                .Columns(1).HeaderText = "Service Crew"
                .Columns(2).HeaderText = "Reason"
                .Columns(3).HeaderText = "Date and Time"
                For Each row As DataRow In dt.Rows
                    row("crew_id") = returnfullname(row("crew_id"))
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub reportstransactionlogs(ByVal searchdate As Boolean)
        Try
            table = "`loc_system_logs`"
            fields = "`log_type`, `log_description`, `log_date_time`"
            If searchdate = False Then
                where = " log_type = 'TRANSACTION' AND date(log_date_time) = CURRENT_DATE() AND log_store = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' "
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewTRANSACTIONLOGS, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " log_type = 'TRANSACTION' AND date(log_date_time) >= '" & Format(DateTimePicker11.Value, "yyyy-MM-dd") & "' AND date(log_date_time) <= '" & Format(DateTimePicker12.Value, "yyyy-MM-dd") & "' AND log_store = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' "
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewTRANSACTIONLOGS, errormessage:="", fields:=fields, successmessage:="", where:=where)
            End If
            With DataGridViewTRANSACTIONLOGS
                .Columns(0).HeaderText = "Type"
                .Columns(1).HeaderText = "Description"
                .Columns(2).HeaderText = "Date and Time"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub reportsdailytransaction(ByVal searchdate As Boolean)
        Try
            Dim table = "`loc_daily_transaction`"

            Dim fields = "`transaction_number`, `grosssales`, `totaldiscount`, `amounttendered`, `change`, `amountdue`, `vatablesales`, `vatexemptsales`, `zeroratedsales`, `vatpercentage`, `lessvat`, `transaction_type`, `discount_type`, `totaldiscountedamount`, `si_number`, `crew_id`, `created_at`"
            Dim DailyTable
            If searchdate = False Then
                where = " WHERE zreading = CURRENT_DATE() AND active IN(1,3) AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' ORDER BY `created_at` DESC"
                DailyTable = AsDatatable(table & where, fields, DataGridViewDaily)
                For Each row As DataRow In DailyTable.rows
                    DataGridViewDaily.Rows.Add(row("transaction_number"), row("grosssales"), row("totaldiscount"), row("amounttendered"), row("change"), row("amountdue"), row("vatablesales"), row("vatexemptsales"), row("zeroratedsales"), row("vatpercentage"), row("lessvat"), row("transaction_type"), row("discount_type"), row("totaldiscountedamount"), row("si_number"), row("crew_id"), row("created_at"))
                Next
            Else
                where = " WHERE zreading >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' and zreading <= '" & Format(DateTimePicker2.Value, "yyyy-MM-dd") & "' AND active IN(1,3) AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' ORDER BY `created_at` DESC"
                DailyTable = AsDatatable(table & where, fields, DataGridViewDaily)
                For Each row As DataRow In DailyTable.rows
                    DataGridViewDaily.Rows.Add(row("transaction_number"), row("grosssales"), row("totaldiscount"), row("amounttendered"), row("change"), row("amountdue"), row("vatablesales"), row("vatexemptsales"), row("zeroratedsales"), row("vatpercentage"), row("lessvat"), row("transaction_type"), row("discount_type"), row("totaldiscountedamount"), row("si_number"), row("crew_id"), row("created_at"))
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub reportssales(ByVal searchdate As Boolean)
        Try
            table = "`loc_daily_transaction_details`"
            fields = "`product_sku`, `product_name`, sum(`quantity`), `price`, sum(`total`), `created_at`"
            If searchdate = False Then
                where = " zreading = CURRENT_DATE()  AND active = 1 AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' GROUP BY `product_name`"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewSales, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " zreading >= '" & Format(DateTimePicker3.Value, "yyyy-MM-dd") & "' AND zreading <= '" & Format(DateTimePicker4.Value, "yyyy-MM-dd") & "' AND active = 1  AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'  GROUP BY `product_name`"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewSales, errormessage:="", fields:=fields, successmessage:="", where:=where)
            End If
            With DataGridViewSales
                .Columns(0).HeaderText = "Product Code"
                .Columns(1).HeaderText = "Product Name"
                .Columns(2).HeaderText = "Quantity"
                .Columns(3).HeaderText = "Price"
                .Columns(4).HeaderText = "Total Price"
                .Columns(5).HeaderText = "Date"
                Label10.Text = "P " & SumOfColumnsToDecimal(DataGridViewSales, 4)
                Label9.Text = SumOfColumnsToInt(DataGridViewSales, 2)
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub


    Public Sub expensereports(ByVal searchdate As Boolean)
        Try
            table = "`loc_expense_list`"
            fields = "`expense_id`, `crew_id`, `expense_number`, `total_amount`, `paid_amount`, `unpaid_amount`, `created_at`"
            If searchdate = False Then
                where = " zreading = date(CURRENT_DATE()) AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewEXPENSES, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " zreading >= '" & Format(DateTimePicker7.Value, "yyyy-MM-dd") & "' and zreading <= '" & Format(DateTimePicker8.Value, "yyyy-MM-dd") & "' AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewEXPENSES, errormessage:="", fields:=fields, successmessage:="", where:=where)
            End If
            With DataGridViewEXPENSES
                .Columns(0).Visible = False
                .Columns(1).Visible = False
                .Columns(2).HeaderCell.Value = "Expense Number"
                .Columns(3).HeaderCell.Value = "Amount"
                .Columns(4).HeaderCell.Value = "Paid Amount"
                .Columns(5).HeaderCell.Value = "Unpaid Amount"
                .Columns(6).HeaderCell.Value = "Date"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub viewexpensesdetails(ByVal expense_number As String)
        Try
            table = "`loc_expense_details`"
            fields = "`expense_type`, `item_info`, `quantity`, `price`, `amount`, `created_at`"
            GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewEXPENSEDET, errormessage:="", fields:=fields, successmessage:="", where:=" expense_number = '" & expense_number & "'")
            With DataGridViewEXPENSEDET
                .Columns(0).HeaderCell.Value = "Type"
                .Columns(1).HeaderCell.Value = "Description"
                .Columns(2).HeaderCell.Value = "Quantity"
                .Columns(3).HeaderCell.Value = "Price"
                .Columns(4).HeaderCell.Value = "Amount"
                .Columns(5).HeaderCell.Value = "Date"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub viewtransactiondetails(ByVal transaction_number As String)
        Try
            Dim DailyTable
            table = "`loc_daily_transaction_details` WHERE transaction_number = '" & transaction_number & "'"
            fields = "`product_name`, `quantity`, `price`, `total`, `product_category`, `upgraded`, `addontype`"
            DailyTable = AsDatatable(table, fields, DataGridViewTransactionDetails)
            For Each row As DataRow In DailyTable.rows
                Dim Upgrade = ""
                If row("upgraded") = 0 Then
                    Upgrade = "NO"
                Else
                    Upgrade = "YES"
                End If
                DataGridViewTransactionDetails.Rows.Add(row("product_name"), row("quantity"), row("price"), row("total"), row("product_category"), Upgrade, row("addontype"))
            Next

        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        Finally
            da.Dispose()
        End Try
    End Sub
    Public Sub viewdeposit(ByVal searchdate As Boolean)
        Try
            table = "`loc_deposit`"
            fields = "`dep_id`, `name`, `crew_id`, `transaction_number`, `amount`, `bank`, `transaction_date`, `store_id`, `guid`, `created_at`"
            If searchdate = False Then
                where = " date(transaction_date) = date(CURRENT_DATE()) AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewDeposits, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " date(transaction_date) >= '" & Format(DateTimePicker16.Value, "yyyy-MM-dd") & "' and date(transaction_date) <= '" & Format(DateTimePicker15.Value, "yyyy-MM-dd") & "' AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewDeposits, errormessage:="", fields:=fields, successmessage:="", where:=where)
            End If
            With DataGridViewDeposits
                .Columns(0).Visible = False
                .Columns(1).HeaderCell.Value = "Full Name"
                .Columns(2).HeaderCell.Value = "Service Crew"
                .Columns(3).HeaderCell.Value = "Transaction Number"
                .Columns(4).HeaderCell.Value = "Amount"
                .Columns(5).HeaderCell.Value = "Bank"
                .Columns(6).HeaderCell.Value = "Transaction Date"
                .Columns(7).Visible = False
                .Columns(8).Visible = False
                .Columns(9).Visible = False
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles ButtonSearchDailyTransaction.Click
        reportsdailytransaction(True)
        DataGridViewTransactionDetails.Rows.Clear()
    End Sub
    Private Sub ButtonSearchSystemLogs_Click(sender As Object, e As EventArgs) Handles ButtonSearchSystemLogs.Click
        reportssystemlogs(True)
    End Sub
    Private Sub ButtonSearchTotalDailySales_Click(sender As Object, e As EventArgs) Handles ButtonSearchTotalDailySales.Click
        reportssales(True)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        reportstransactionlogs(True)
    End Sub
    Private Sub ButtonSearchExpenses_Click(sender As Object, e As EventArgs) Handles ButtonSearchExpenses.Click
        expensereports(True)
        DataGridViewEXPENSEDET.DataSource = Nothing
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        reportsreturnsandrefunds(True)
    End Sub
    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        viewdeposit(True)
    End Sub
    Private Sub DataGridViewEXPENSES_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewEXPENSES.CellClick
        Try
            If DataGridViewEXPENSES.Rows.Count > 0 Then
                Dim datagridid = DataGridViewEXPENSES.SelectedRows(0).Cells(2).Value.ToString()
                viewexpensesdetails(datagridid)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub DataGridViewDaily_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewDaily.CellClick
        Try
            'transaction_number = (Val(TextBoxCustomerID.Text))
            If DataGridViewDaily.Rows.Count > 0 Then
                viewtransactiondetails(transaction_number:=DataGridViewDaily.SelectedRows(0).Cells(0).Value)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If ComboBoxTransactionType.Text = "All" Then
            If DataGridViewTransactionDetails.Rows.Count > 0 Then
                total = SumOfColumnsToDecimal(DataGridViewTransactionDetails, 3)
                Try
                    For i As Integer = 0 To DataGridViewTransactionDetails.Rows.Count - 1 Step +1
                        b += 10
                        If DataGridViewTransactionDetails.Rows(i).Cells(5).Value = "YES" Then
                            b += 10
                        End If
                    Next

                    If DataGridViewDaily.SelectedRows(0).Cells(2).Value < 1 Then
                        printdoc.DefaultPageSettings.PaperSize = New PaperSize("Custom", 200, 500 + b)
                    Else
                        printdoc.DefaultPageSettings.PaperSize = New PaperSize("Custom", 200, 520 + b)
                    End If

                    If S_Reprint = "YES" Then
                        printdoc.Print()
                    Else
                        PrintPreviewDialog1.Document = printdoc
                        PrintPreviewDialog1.ShowDialog()
                    End If
                    b = 0
                Catch ex As Exception
                    MessageBox.Show("An error occurred while trying to load the " &
                        "document for Print Preview. Make sure you currently have " &
                        "access to a printer. A printer must be localconnected and " &
                        "accessible for Print Preview to work.", Me.Text,
                         MessageBoxButtons.OK, MessageBoxIcon.Error)
                    SendErrorReport(ex.ToString)
                End Try
            Else
                MsgBox("Select Transaction First!")
            End If
        Else
            printtransactiontype.DefaultPageSettings.PaperSize = New PaperSize("Custom", 200, 200)
            If S_Reprint = "YES" Then
                printtransactiontype.Print()
            Else
                previewtransactiontype.Document = printtransactiontype
                previewtransactiontype.ShowDialog()
            End If
        End If
    End Sub

    Private Sub pdoctransactiontype_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles printtransactiontype.PrintPage
        Try
            ReceiptHeader(sender, e)
            Dim WalkinTotal As Decimal = 0
            Dim Registered As Decimal = 0
            Dim GCash As Decimal = 0
            Dim Grab As Decimal = 0
            Dim Paymaya As Decimal = 0
            Dim Lalafood As Decimal = 0
            Dim RepExpense As Decimal = 0
            Dim FoodPanda As Decimal = 0
            Dim Others As Decimal = 0

            Dim WalkinTotalqty As Integer = 0
            Dim Registeredqty As Integer = 0
            Dim GCashqty As Integer = 0
            Dim Grabqty As Integer = 0
            Dim Paymayaqty As Integer = 0
            Dim Lalafoodqty As Integer = 0
            Dim RepExpenseqty As Integer = 0
            Dim FoodPandaqty As Integer = 0
            Dim Othersqty As Integer = 0

            With DataGridViewDaily
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If .Rows(i).Cells(11).Value = "Walk-In" Then
                        WalkinTotal += .Rows(i).Cells(1).Value
                        WalkinTotalqty += 1
                    ElseIf .Rows(i).Cells(11).Value = "Registered" Then
                        Registered += .Rows(i).Cells(1).Value
                        Registeredqty += 1
                    ElseIf .Rows(i).Cells(11).Value = "GCash" Then
                        GCash += .Rows(i).Cells(1).Value
                        GCashqty += 1
                    ElseIf .Rows(i).Cells(11).Value = "Grab" Then
                        Grab += .Rows(i).Cells(1).Value
                        Grabqty += 1
                    ElseIf .Rows(i).Cells(11).Value = "Paymaya" Then
                        Paymaya += .Rows(i).Cells(1).Value
                        Paymayaqty += 1
                    ElseIf .Rows(i).Cells(11).Value = "Lalafood" Then
                        Lalafood += .Rows(i).Cells(1).Value
                        Lalafoodqty += 1
                    ElseIf .Rows(i).Cells(11).Value = "Representation Expenses" Then
                        RepExpense += .Rows(i).Cells(1).Value
                        RepExpenseqty += 1
                    ElseIf .Rows(i).Cells(11).Value = "Food Panda" Then
                        FoodPanda += .Rows(i).Cells(1).Value
                        FoodPandaqty += 1
                    ElseIf .Rows(i).Cells(11).Value = "Others" Then
                        Others += .Rows(i).Cells(1).Value
                        Othersqty += 1
                    End If
                Next
            End With

            Dim font As New Font("Tahoma", 6)
            Dim font1 As New Font("Tahoma", 6, FontStyle.Bold)
            RightToLeftDisplay(sender, e, 120, "LIST OF TRANSACTION TYPE:", "", font1, 0, 0)
            RightToLeftDisplay(sender, e, 140, "Type/Count:", ":" & "Total", font, 0, 0)

            With ComboBoxTransactionType
                If .Text = "Walk-In" Then
                    RightToLeftDisplay(sender, e, 160, "Walk-In(" & WalkinTotalqty & ")", NUMBERFORMAT(WalkinTotal), font, 0, 0)
                ElseIf .Text = "Registered" Then
                    RightToLeftDisplay(sender, e, 160, "Registered(" & Registeredqty & ")", NUMBERFORMAT(Registered), font, 0, 0)
                ElseIf .Text = "GCash" Then
                    RightToLeftDisplay(sender, e, 160, "GCash(" & GCashqty & ")", NUMBERFORMAT(GCash), font, 0, 0)
                ElseIf .Text = "Grab" Then
                    RightToLeftDisplay(sender, e, 160, "Grab(" & Grabqty & ")", NUMBERFORMAT(Grab), font, 0, 0)
                ElseIf .Text = "Paymaya" Then
                    RightToLeftDisplay(sender, e, 160, "Paymaya(" & Paymayaqty & ")", NUMBERFORMAT(Paymaya), font, 0, 0)
                ElseIf .Text = "Lalafood" Then
                    RightToLeftDisplay(sender, e, 160, "Lalafood(" & Lalafoodqty & ")", NUMBERFORMAT(Lalafood), font, 0, 0)
                ElseIf .Text = "Representation Expenses" Then
                    RightToLeftDisplay(sender, e, 160, "Rep. Expenses(" & RepExpenseqty & ")", NUMBERFORMAT(RepExpense), font, 0, 0)
                ElseIf .Text = "Food Panda" Then
                    RightToLeftDisplay(sender, e, 160, "Food Panda(" & FoodPandaqty & ")", NUMBERFORMAT(FoodPanda), font, 0, 0)
                ElseIf .Text = "Others" Then
                    RightToLeftDisplay(sender, e, 160, "Others(" & Othersqty & ")", NUMBERFORMAT(Others), font, 0, 0)
                End If
            End With

            CenterTextDisplay(sender, e, "From: " & Format(DateTimePicker1.Value, "yyyy-MM-dd") & " - To: " & Format(DateTimePicker2.Value, "yyyy-MM-dd"), font, 180)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub pdoc_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles printdoc.PrintPage
        Try
            Dim totalDisplay = Format(DataGridViewDaily.SelectedRows(0).Cells(8).Value, "###,###,##0.00")
            a = 0
            Dim font1 As New Font("Tahoma", 6, FontStyle.Bold)
            Dim font2 As New Font("Tahoma", 7, FontStyle.Bold)
            Dim font As New Font("Tahoma", 6)
            Dim fontaddon As New Font("Tahoma", 5)
            ReceiptHeader(sender, e)
            Dim format1st As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
            Dim abc As Integer = 0
            Try
                Dim Query1 As String = "SELECT senior_name FROM loc_senior_details WHERE transaction_number = '" & DataGridViewDaily.SelectedRows(0).Cells(0).Value & "'"
                Dim CmdQ As MySqlCommand = New MySqlCommand(Query1, LocalhostConn)
                Dim result = CmdQ.ExecuteScalar()
                SimpleTextDisplay(sender, e, result, font, 30, 45)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            For i As Integer = 0 To DataGridViewTransactionDetails.Rows.Count - 1 Step +1
                Dim rect1st As RectangleF = New RectangleF(10.0F, 115 + abc, 173.0F, 100.0F)
                Dim price = Format(DataGridViewTransactionDetails.Rows(i).Cells(3).Value, "###,###,##0.00")

                If DataGridViewTransactionDetails.Rows(i).Cells(4).Value.ToString = "Add-Ons" Then
                    If DataGridViewTransactionDetails.Rows(i).Cells(6).Value.ToString = "Classic" Then
                        RightToLeftDisplay(sender, e, abc + 115, "     @" & DataGridViewTransactionDetails.Rows(i).Cells(0).Value, price, fontaddon, 0, 0)
                    Else
                        RightToLeftDisplay(sender, e, abc + 115, DataGridViewTransactionDetails.Rows(i).Cells(1).Value & " " & DataGridViewTransactionDetails.Rows(i).Cells(0).Value, price, font, 0, 0)
                    End If
                Else
                    RightToLeftDisplay(sender, e, abc + 115, DataGridViewTransactionDetails.Rows(i).Cells(1).Value & " " & DataGridViewTransactionDetails.Rows(i).Cells(0).Value, price, font, 0, 0)
                    If DataGridViewTransactionDetails.Rows(i).Cells(5).Value = "YES" Then
                        abc += 10
                        a += 10

                        RightToLeftDisplay(sender, e, abc + 115, "     + UPGRADE BRWN " & DataGridViewTransactionDetails.Rows(i).Cells(5).Value, "", fontaddon, 0, 0)
                    End If
                End If
                a += 10
                abc += 10
            Next
            With DataGridViewDaily
                Dim b As Integer = .SelectedRows(0).Cells(14).Value
                Dim SINUMBERSTRING As String = b.ToString(S_SIFormat)
                If .SelectedRows(0).Cells(2).Value < 1 Then
                    a += 120
                    RightToLeftDisplay(sender, e, a, "AMOUNT DUE:", "P" & .SelectedRows(0).Cells(5).Value.ToString, font2, 0, 0)
                    RightToLeftDisplay(sender, e, a + 15, "CASH:", "P" & .SelectedRows(0).Cells(5).Value.ToString, font1, 0, 0)
                    RightToLeftDisplay(sender, e, a + 25, "CHANGE:", "P" & .SelectedRows(0).Cells(4).Value.ToString, font1, 0, 0)
                    SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 23)
                    RightToLeftDisplay(sender, e, a + 52, "     Vatable", "    " & .SelectedRows(0).Cells(6).Value.ToString, font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 62, "     Vat Exempt Sales", "    " & .SelectedRows(0).Cells(7).Value.ToString, font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 72, "     Zero Rated Sales", "    " & .SelectedRows(0).Cells(8).Value.ToString, font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 82, "     VAT" & "(" & Val(S_Tax) * 100 & "%)", "    " & .SelectedRows(0).Cells(9).Value.ToString, font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 92, "     Less Vat", "    " & .SelectedRows(0).Cells(10).Value.ToString, font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 102, "     Total", "    " & .SelectedRows(0).Cells(5).Value.ToString, font, 0, 0)
                    a += 4
                    SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 92)
                    a += 1
                    SimpleTextDisplay(sender, e, "Transaction Type: " & .SelectedRows(0).Cells(11).Value.ToString, font, 0, a + 100)
                    SimpleTextDisplay(sender, e, "Total Item(s): " & SumOfColumnsToInt(DataGridViewTransactionDetails, 1), font, 0, a + 110)
                    SimpleTextDisplay(sender, e, "Cashier: " & .SelectedRows(0).Cells(15).Value.ToString & " " & returnfullname(where:= .SelectedRows(0).Cells(15).Value.ToString), font, 0, a + 120)
                    SimpleTextDisplay(sender, e, "Str No: " & ClientStoreID, font, 110, a + 110)
                    SimpleTextDisplay(sender, e, "Date & Time: " & .SelectedRows(0).Cells(16).Value, font, 0, a + 130)
                    SimpleTextDisplay(sender, e, "Terminal No: " & S_Terminal_No, font, 110, a + 140)
                    SimpleTextDisplay(sender, e, "Ref. #: " & .SelectedRows(0).Cells(0).Value.ToString, font, 0, a + 140)
                    SimpleTextDisplay(sender, e, "SI No: " & SINUMBERSTRING, font, 0, a + 150)
                    SimpleTextDisplay(sender, e, "THIS SERVES AS AN OFFICIAL RECEIPT", font, 0, a + 160)

                    SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 174)
                    ReceiptFooter(sender, e, a + 12)
                Else
                    a += 100
                    Dim sql = "SELECT * FROM loc_coupon_data WHERE transaction_number = '" & .SelectedRows(0).Cells(0).Value.ToString & "'"
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                    Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                    Dim dt As DataTable = New DataTable
                    da.Fill(dt)
                    Dim CouponNameReports = dt(0)(2)
                    Dim CouponDescReports = dt(0)(3)
                    Dim CouponTypeReports = dt(0)(4)
                    Dim CouponLineReports = dt(0)(5)
                    Dim CouponTotalReports = dt(0)(6)
                    SimpleTextDisplay(sender, e, CouponNameReports & "(" & CouponTypeReports & ")", font, 0, a)
                    SimpleTextDisplay(sender, e, CouponDescReports, font, 0, a + 10)
                    a += 40 + CouponLineReports
                    RightToLeftDisplay(sender, e, a - 18, "Total Discount:", "P" & CouponTotalReports, font, 0, 0)
                    Dim SubTotal = SumOfColumnsToDecimal(DataGridViewTransactionDetails, 3)

                    RightToLeftDisplay(sender, e, a, "SUB TOTAL:", "P" & SubTotal, font1, 0, 0)
                    RightToLeftDisplay(sender, e, a + 10, "DISCOUNT:", .SelectedRows(0).Cells(2).Value.ToString & "-", font1, 0, 0)
                    RightToLeftDisplay(sender, e, a + 20, "AMOUNT DUE:", "P" & .SelectedRows(0).Cells(5).Value.ToString, font2, 0, 0)
                    RightToLeftDisplay(sender, e, a + 30, "CASH:", "P" & .SelectedRows(0).Cells(3).Value.ToString, font1, 0, 0)
                    RightToLeftDisplay(sender, e, a + 40, "CHANGE:", "P" & .SelectedRows(0).Cells(4).Value.ToString, font1, 0, 0)
                    SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 37)
                    a += 4
                    RightToLeftDisplay(sender, e, a + 65, "     Vatable", "    " & .SelectedRows(0).Cells(6).Value.ToString, font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 75, "     Vat Exempt Sales", "    " & .SelectedRows(0).Cells(7).Value.ToString, font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 85, "     Zero Rated Sales", "    " & .SelectedRows(0).Cells(8).Value.ToString, font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 95, "     VAT" & "(" & Val(S_Tax) * 100 & "%)", "    " & .SelectedRows(0).Cells(9).Value.ToString, font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 105, "     Less Vat", "    " & .SelectedRows(0).Cells(10).Value.ToString, font, 0, 0)
                    RightToLeftDisplay(sender, e, a + 115, "     Total", "    " & .SelectedRows(0).Cells(5).Value.ToString, font, 0, 0)
                    a += 5
                    SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 101)
                    a += 4
                    SimpleTextDisplay(sender, e, "Transaction Type: " & .SelectedRows(0).Cells(11).Value.ToString, font, 0, a + 110)
                    SimpleTextDisplay(sender, e, "Total Item(s): " & SumOfColumnsToInt(DataGridViewTransactionDetails, 1), font, 0, a + 120)
                    SimpleTextDisplay(sender, e, "Cashier: " & .SelectedRows(0).Cells(15).Value.ToString & " " & returnfullname(where:= .SelectedRows(0).Cells(15).Value.ToString), font, 0, a + 130)
                    SimpleTextDisplay(sender, e, "Str No: " & ClientStoreID, font, 120, a + 120)
                    SimpleTextDisplay(sender, e, "Date & Time: " & .SelectedRows(0).Cells(16).Value, font, 0, a + 140)
                    SimpleTextDisplay(sender, e, "Terminal No: " & S_Terminal_No, font, 120, a + 150)
                    SimpleTextDisplay(sender, e, "Ref. #: " & .SelectedRows(0).Cells(0).Value.ToString, font, 0, a + 150)
                    SimpleTextDisplay(sender, e, "SI No: " & SINUMBERSTRING, font, 0, a + 160)
                    SimpleTextDisplay(sender, e, "THIS SERVES AS AN OFFICIAL RECEIPT", font, 0, a + 170)
                    a += 6
                    SimpleTextDisplay(sender, e, "*************************************", font, 0, a + 180)
                    a += 16
                    ReceiptFooter(sender, e, a)
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Dim XreadOrZread As String
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        XreadOrZread = "X-READ"
        ReadingOR = "X" & Format(Now, "yyddMMHHmmssyy")

        printdocXread.DefaultPageSettings.PaperSize = New PaperSize("Custom", 215, 1000)

        If S_Print_XZRead = "YES" Then
            printdocXread.Print()
        Else
            PrintPreviewDialogXread.Document = printdocXread
            PrintPreviewDialogXread.ShowDialog()
        End If

        SystemLogDesc = "X Reading : " & FullDate24HR() & " Crew : " & returnfullname(ClientCrewID)
        SystemLogType = "X-READ"
        GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printdocXread.PrintPage
        Try
            Dim ZreadDateFormat = S_Zreading
            Dim font As New Font("tahoma", 6)
            Dim font2 As New Font("tahoma", 6, FontStyle.Bold)
            Dim brandfont As New Font("tahoma", 8, FontStyle.Bold)
            Dim GrossSale = sum("grosssales", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type IN ('Walk-in','Grab') AND active = 1")
            Dim LessVat = sum("lessvat", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type IN ('Walk-in','Grab') AND active = 1")
            Dim TotalDiscount = sum("totaldiscount", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type IN ('Walk-in','Grab') AND active = 1")
            Dim begORNm = returnselect("transaction_number", "`loc_daily_transaction` WHERE date(zreading) = zreading AND active = 1 Limit 1")
            Dim EndORNumber = Format(Now, "yyddMMHHmmssyy")
            Dim ReturnsTotal = sum("total", "loc_daily_transaction_details WHERE active = 2 AND zreading = '" & ZreadDateFormat & "' ")
            Dim ReturnsExchange = sum("quantity", "loc_daily_transaction_details WHERE active = 2 AND zreading = '" & ZreadDateFormat & "' ")
            Dim OLDgrandtotal = sum("total", "loc_daily_transaction_details WHERE zreading <> '" & ZreadDateFormat & "' AND active = 1")
            Dim NEWgrandtotal = sum("total", "loc_daily_transaction_details WHERE active = 1")
            Dim TotalGuest = count("transaction_id", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "' ")
            Dim TotalQuantity = sum("quantity", "loc_daily_transaction_details WHERE zreading = '" & ZreadDateFormat & "' AND active = 1")
            Dim SrDiscount = sum("totaldiscount", "loc_daily_transaction WHERE discount_type = 'Percentage' AND zreading = '" & ZreadDateFormat & "' AND transaction_type IN ('Walk-in','Grab') AND active = 1")
            Dim totalExpenses = sum("total_amount", "loc_expense_list WHERE zreading = '" & ZreadDateFormat & "'")
            Dim VatExempt = sum("vatexemptsales", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type IN ('Walk-in','Grab') AND active = 1")
            Dim zeroratedsales = sum("zeroratedsales", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "' AND active = 1")
            Dim vatablesales = sum("vatablesales", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type IN ('Walk-in','Grab') AND active = 1")
            Dim DepositSlip = sum("amount", "loc_deposit WHERE date(transaction_date) = '" & ZreadDateFormat & "' ")
            Dim BegBalance = sum("CAST(log_description AS DECIMAL(10,2))", "loc_system_logs WHERE log_type IN ('BG-1','BG-2','BG-3','BG-4') AND zreading = '" & ZreadDateFormat & "' ORDER by log_date_time DESC LIMIT 1")
            Dim vat12percent = sum("vatpercentage", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type IN ('Walk-in','Grab') AND active = 1")
            Dim DailySales = GrossSale - LessVat - TotalDiscount
            Dim NetSales = sum("amountdue", "loc_daily_transaction WHERE active = 1 AND zreading = '" & ZreadDateFormat & "' AND transaction_type IN ('Walk-in','Grab')")
            Dim CashInDrawer = DailySales + BeginningBalance - totalExpenses
            Dim CashTotal = CashInDrawer
            Dim TotalNewGrandTotal = CashInDrawer + OLDgrandtotal


            CenterTextDisplay(sender, e, ClientBrand.ToUpper, brandfont, 10)
            '============================================================================================================================
            CenterTextDisplay(sender, e, "Opt by : Innovention Food Asia Co.", font, 21)
            '============================================================================================================================
            CenterTextDisplay(sender, e, ClientAddress, font, 31)
            '============================================================================================================================
            CenterTextDisplay(sender, e, ClientBrgy & ", " & getmunicipality & ", " & getprovince, font, 41)
            '============================================================================================================================
            CenterTextDisplay(sender, e, "VAT REG TIN : " & ClientTin, font, 51)
            '============================================================================================================================
            CenterTextDisplay(sender, e, "MSN : " & ClientMSN, font, 61)
            '============================================================================================================================
            CenterTextDisplay(sender, e, "MIN : " & ClientMIN, font, 71)
            '============================================================================================================================
            CenterTextDisplay(sender, e, "PTUN : " & ClientPTUN, font, 81)
            '============================================================================================================================
            RightToLeftDisplay(sender, e, 100, "TERMINAL REPORT", XreadOrZread, font2, 20, 0)
            '============================================================================================================================
            SimpleTextDisplay(sender, e, ReadingOR, font, 0, 90)
            SimpleTextDisplay(sender, e, "----------------------------------------------------------------", font, 0, 95)
            '============================================================================================================================
            RightToLeftDisplay(sender, e, 123, "DESCRIPTION", "QTY/AMOUNT", font2, 20, 0)
            '============================================================================================================================
            SimpleTextDisplay(sender, e, "----------------------------------------------------------------", font, 0, 110)
            '============================================================================================================================
            RightToLeftDisplay(sender, e, 140, "TERMINAL N0.", S_Terminal_No, font, 20, 0)
            RightToLeftDisplay(sender, e, 155, "GROSS", NUMBERFORMAT(GrossSale), font, 20, 0)
            RightToLeftDisplay(sender, e, 165, "LESS VAT (VE)", LessVat, font, 20, 0)
            RightToLeftDisplay(sender, e, 175, "LESS VAT DIPLOMAT", "0.00", font, 20, 0)
            RightToLeftDisplay(sender, e, 185, "LESS VAT (OTHER)", "0.00", font, 20, 0)
            RightToLeftDisplay(sender, e, 195, "ADD VAT", "0.00", font, 20, 0)
            RightToLeftDisplay(sender, e, 205, "DAILY SALES", NUMBERFORMAT(DailySales), font, 20, 0)
            '============================================================================================================================
            RightToLeftDisplay(sender, e, 220, "VAT AMOUNT", NUMBERFORMAT(vat12percent), font, 20, 0)
            RightToLeftDisplay(sender, e, 230, "LOCAL GOV'T TAX", "0.00", font, 20, 0)
            RightToLeftDisplay(sender, e, 240, "VATABLE SALES", NUMBERFORMAT(vatablesales), font, 20, 0)
            RightToLeftDisplay(sender, e, 250, "ZERO RATED SALES", NUMBERFORMAT(zeroratedsales), font, 20, 0)
            RightToLeftDisplay(sender, e, 260, "VAT EXEMPT SALES", NUMBERFORMAT(VatExempt), font, 20, 0)
            RightToLeftDisplay(sender, e, 270, "LESS DISC (VE)", NUMBERFORMAT(TotalDiscount), font, 20, 0)
            RightToLeftDisplay(sender, e, 280, "NET SALES", NUMBERFORMAT(DailySales), font, 20, 0)
            '============================================================================================================================
            RightToLeftDisplay(sender, e, 295, "CASH TOTAL", NUMBERFORMAT(DailySales), font, 20, 0)
            RightToLeftDisplay(sender, e, 305, "CREDIT CARD", "N/A", font, 20, 0)
            RightToLeftDisplay(sender, e, 315, "DEBIT CARD", "N/A", font, 20, 0)
            RightToLeftDisplay(sender, e, 325, "MISC/CHEQUES", "N/A", font, 20, 0)
            RightToLeftDisplay(sender, e, 335, "GIFT CARD(GC)", "N/A", font, 20, 0)
            RightToLeftDisplay(sender, e, 345, "A/R", "N/A", font, 20, 0)
            RightToLeftDisplay(sender, e, 355, "TOTAL EXPENSES", NUMBERFORMAT(totalExpenses), font, 20, 0)
            RightToLeftDisplay(sender, e, 365, "OTHERS", "N/A", font, 20, 0)
            RightToLeftDisplay(sender, e, 375, "BEG.BALANCE", NUMBERFORMAT(BegBalance), font, 20, 0)
            RightToLeftDisplay(sender, e, 385, "DEPOSIT", NUMBERFORMAT(DepositSlip), font, 20, 0)
            RightToLeftDisplay(sender, e, 395, "CASH IN DRAWER", NUMBERFORMAT(CashInDrawer), font, 20, 0)
            '============================================================================================================================
            Dim CASHLESS = sum("amountdue", "loc_daily_transaction WHERE active IN (1,3) AND zreading = '" & ZreadDateFormat & "' AND transaction_type NOT IN ('Walk-in','Grab') ")
            RightToLeftDisplay(sender, e, 410, "CASHLESS", CASHLESS, font, 20, 0)
            Dim GCASH = sum("amountdue", "loc_daily_transaction WHERE active = 1 AND zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Gcash' ")
            RightToLeftDisplay(sender, e, 420, "GCASH", GCASH, font, 20, 0)
            Dim PAYMAYA = sum("amountdue", "loc_daily_transaction WHERE active = 1 AND zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Paymaya' ")
            RightToLeftDisplay(sender, e, 430, "PAYMAYA", PAYMAYA, font, 20, 0)
            Dim lalafood = sum("amountdue", "loc_daily_transaction WHERE active = 1 AND zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Lalafood' ")
            RightToLeftDisplay(sender, e, 440, "LALAFOOD", lalafood, font, 20, 0)
            Dim FOODPANDA = sum("amountdue", "loc_daily_transaction WHERE active = 1 AND zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Food Panda' ")
            RightToLeftDisplay(sender, e, 450, "FOOD PANDA", FOODPANDA, font, 20, 0)
            Dim REPEX = sum("amountdue", "loc_daily_transaction WHERE active = 3 AND zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Representation Expenses' ")
            RightToLeftDisplay(sender, e, 460, "REPEXPENSE", REPEX, font, 20, 0)
            Dim Others = sum("amountdue", "loc_daily_transaction WHERE active = 3 AND zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Others' ")
            RightToLeftDisplay(sender, e, 470, "OTHERS", Others, font, 20, 0)
            '============================================================================================================================
            RightToLeftDisplay(sender, e, 485, "ITEM VOID E/C", ReturnsExchange, font, 20, 0)
            RightToLeftDisplay(sender, e, 495, "TRANSACTION VOID", ReturnsExchange, font, 20, 0)
            RightToLeftDisplay(sender, e, 505, "TRANSACTION CANCEL", ReturnsExchange, font, 20, 0)
            RightToLeftDisplay(sender, e, 515, "DIMPLOMAT", "N/A", font, 20, 0)
            RightToLeftDisplay(sender, e, 525, "TOTAL DISCOUNTS", NUMBERFORMAT(TotalDiscount), font, 20, 0)
            RightToLeftDisplay(sender, e, 535, " - SENIOR CITIZEN", NUMBERFORMAT(SrDiscount), font, 20, 0)
            RightToLeftDisplay(sender, e, 545, "TAKE OUT CHARGE", "N/A", font, 20, 0)
            RightToLeftDisplay(sender, e, 555, "DELIVERY CHARGE", "N/A", font, 20, 0)
            RightToLeftDisplay(sender, e, 565, "RETURNS EXCHANGE", ReturnsExchange, font, 20, 0)
            RightToLeftDisplay(sender, e, 575, "RETURNS REFUND", NUMBERFORMAT(ReturnsTotal), font, 20, 0)
            '============================================================================================================================
            RightToLeftDisplay(sender, e, 590, "TOTAL QTY SOLD", TotalQuantity, font, 20, 0)
            RightToLeftDisplay(sender, e, 600, "TOTAL TRANS. COUNT", TotalGuest, font, 20, 0)
            RightToLeftDisplay(sender, e, 610, "TOTAL GUEST", TotalGuest, font, 20, 0)
            RightToLeftDisplay(sender, e, 620, "BEGINNING OR NO.", begORNm, font, 20, 0)
            RightToLeftDisplay(sender, e, 630, "END OR NO.", EndORNumber, font, 20, 0)
            '============================================================================================================================
            RightToLeftDisplay(sender, e, 645, "CURRENT TOTAL SALES", NUMBERFORMAT(CashTotal), font, 20, 0)
            RightToLeftDisplay(sender, e, 655, "OLD GRAND TOTAL", NUMBERFORMAT(OLDgrandtotal), font, 20, 0)
            RightToLeftDisplay(sender, e, 665, "NEW GRAND TOTAL", NUMBERFORMAT(TotalNewGrandTotal), font, 20, 0)

            Dim ADDONS = sum("quantity", "loc_daily_transaction_details WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Walk-in' AND product_category = 'Add-Ons'")
            Dim BLENDS = sum("quantity", "loc_daily_transaction_details WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Walk-in' AND product_category = 'Famous Blends'")
            Dim COMBO = sum("quantity", "loc_daily_transaction_details WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Walk-in' AND product_category = 'Combo'")
            Dim PERFECTC = sum("quantity", "loc_daily_transaction_details WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Walk-in' AND product_category = 'Perfect Combination'")
            Dim PREMIUM = sum("quantity", "loc_daily_transaction_details WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Walk-in' AND product_category = 'Premium'")
            Dim SAVORY = sum("quantity", "loc_daily_transaction_details WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Walk-in' AND product_category = 'Savory'")
            Dim SIMPERF = sum("quantity", "loc_daily_transaction_details WHERE zreading = '" & ZreadDateFormat & "' AND transaction_type = 'Walk-in' AND product_category = 'Simply Perfect'")

            SimpleTextDisplay(sender, e, "----------------------------------------------------------------", font, 0, 665)
            SimpleTextDisplay(sender, e, "SALES BY CLASS", font2, 0, 675)
            RightToLeftDisplay(sender, e, 685 + 20, "ADD ONS", ADDONS, font, 20, 0)
            RightToLeftDisplay(sender, e, 695 + 20, "FAMOUS BLENDS", BLENDS, font, 20, 0)
            RightToLeftDisplay(sender, e, 705 + 20, "COMBO", COMBO, font, 20, 0)
            RightToLeftDisplay(sender, e, 715 + 20, "PERFECT COMBINATION", PERFECTC, font, 20, 0)
            RightToLeftDisplay(sender, e, 725 + 20, "PREMIUM LINE", PREMIUM, font, 20, 0)
            RightToLeftDisplay(sender, e, 735 + 20, "SAVORY", SAVORY, font, 20, 0)
            RightToLeftDisplay(sender, e, 745 + 20, "SIMPY PERFECT", SIMPERF, font, 20, 0)

            SimpleTextDisplay(sender, e, "----------------------------------------------------------------", font, 0, 755)
            CenterTextDisplay(sender, e, S_Zreading & " " & Format(Now(), "HH:mm:ss"), font, 810)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles ButtonZread.Click
        Try
            Dim msg = MessageBox.Show("Are you sure you want to generate Z-READ ? Press Yes to continue or No to cancel", "Z-reading", MessageBoxButtons.YesNo, MessageBoxIcon.Information)

            If msg = DialogResult.Yes Then
                Dim ConnectionLocal As MySqlConnection = LocalhostConn()
                'Fill dgv inv
                GLOBAL_SELECT_ALL_FUNCTION("loc_pos_inventory", "*", DataGridViewZreadInventory)
                'Update inventory
                MainInventorySub()
                'Fill again
                GLOBAL_SELECT_ALL_FUNCTION("loc_pos_inventory", "*", DataGridViewZreadInventory)
                'Print zread
                XreadOrZread = "Z-READ"
                ReadingOR = "Z" & Format(Now, "yyddMMHHmmssyy")
                printdocXread.DefaultPageSettings.PaperSize = New PaperSize("Custom", 215, 1000)

                If S_Print_XZRead = "YES" Then
                    printdocXread.Print()
                Else
                    PrintPreviewDialogXread.Document = printdocXread
                    PrintPreviewDialogXread.ShowDialog()
                End If

                'Update Zread

                S_Zreading = Format(DateAdd("d", 1, S_Zreading), "yyyy-MM-dd")
                sql = "UPDATE loc_settings SET S_Zreading = '" & S_Zreading & "'"
                cmd = New MySqlCommand(sql, ConnectionLocal)
                cmd.ExecuteNonQuery()
                cmd.Dispose()

                sql = "UPDATE loc_pos_inventory SET zreading = '" & S_Zreading & "'"
                LocalhostConn.Close()
                cmd = New MySqlCommand(sql, ConnectionLocal)
                cmd.ExecuteNonQuery()

                cmd.Dispose()
                'Insert to local zread inv
                XZreadingInventory(S_Zreading)
                If S_Zreading = Format(Now(), "yyyy-MM-dd") Then
                    ButtonZread.Enabled = False
                    ButtonZreadAdmin.Enabled = False
                End If
                Button7.PerformClick()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Button6_Click_1(sender As Object, e As EventArgs) Handles ButtonZreadAdmin.Click
        Try
            Dim result As Integer = MessageBox.Show("It seems like you have not generated Z-reading before ? Would you like to generate now ?", "Z-Reading", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                Try
                    'Fill dgv inv
                    GLOBAL_SELECT_ALL_FUNCTION("loc_pos_inventory", "*", DataGridViewZreadInventory)
                    'Update inventory
                    MainInventorySub()
                    'Fill again
                    GLOBAL_SELECT_ALL_FUNCTION("loc_pos_inventory", "*", DataGridViewZreadInventory)
                    'Print zread
                    XreadOrZread = "Z-READ"
                    ReadingOR = "Z" & Format(Now, "yyddMMHHmmssyy")
                    printdocXread.DefaultPageSettings.PaperSize = New PaperSize("Custom", 215, 1000)

                    If S_Print_XZRead = "YES" Then
                        printdocXread.Print()
                    Else
                        PrintPreviewDialogXread.Document = printdocXread
                        PrintPreviewDialogXread.ShowDialog()
                    End If

                    'Update Zread
                    S_Zreading = Format(Now, "yyyy-MM-dd")
                    sql = "UPDATE loc_settings SET S_Zreading = '" & S_Zreading & "'"
                    cmd = New MySqlCommand(sql, LocalhostConn())
                    cmd.ExecuteNonQuery()
                    cmd.Dispose()
                    LocalhostConn.Close()
                    'Insert to local zread inv
                    XZreadingInventory(S_Zreading)
                    If S_Zreading = Format(Now(), "yyyy-MM-dd") Then
                        ButtonZread.Enabled = False
                        ButtonZreadAdmin.Enabled = False
                    End If
                    Button7.PerformClick()
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try

            Else
                MessageBox.Show("This will continue your yesterday's record ...", "Z-Reading", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub XZreadingInventory(zreaddate)
        Try
            Dim Con As MySqlConnection = New MySqlConnection
            Con = LocalhostConn()
            Dim Fields As String = "`inventory_id`, `store_id`, `formula_id`, `product_ingredients`, `sku`, `stock_primary`, `stock_secondary`, `stock_no_of_servings`, `stock_status`, `critical_limit`, `guid`, `created_at`, `crew_id`, `synced`, `server_date_modified`, `server_inventory_id`, `zreading`"
            Dim cmd As MySqlCommand
            With DataGridViewZreadInventory
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmd = New MySqlCommand("INSERT INTO loc_zread_inventory (" & Fields & ") VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17)", Con)
                    cmd.Parameters.Add("@1", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString
                    cmd.Parameters.Add("@3", MySqlDbType.Int64).Value = .Rows(i).Cells(2).Value.ToString
                    cmd.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString
                    cmd.Parameters.Add("@6", MySqlDbType.Double).Value = .Rows(i).Cells(5).Value.ToString
                    cmd.Parameters.Add("@7", MySqlDbType.Double).Value = .Rows(i).Cells(6).Value.ToString
                    cmd.Parameters.Add("@8", MySqlDbType.Double).Value = .Rows(i).Cells(7).Value.ToString
                    cmd.Parameters.Add("@9", MySqlDbType.Int64).Value = .Rows(i).Cells(8).Value.ToString
                    cmd.Parameters.Add("@10", MySqlDbType.Int64).Value = .Rows(i).Cells(9).Value.ToString
                    cmd.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString
                    cmd.Parameters.Add("@12", MySqlDbType.Text).Value = FullDate24HR()
                    cmd.Parameters.Add("@13", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString
                    cmd.Parameters.Add("@14", MySqlDbType.VarChar).Value = "Unsynced"
                    cmd.Parameters.Add("@15", MySqlDbType.Text).Value = "N/A"
                    cmd.Parameters.Add("@16", MySqlDbType.Int64).Value = .Rows(i).Cells(15).Value.ToString
                    cmd.Parameters.Add("@17", MySqlDbType.Text).Value = S_Zreading
                    cmd.ExecuteNonQuery()
                Next
                Con.Close()
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Dim threadlist As List(Of Thread) = New List(Of Thread)
    Dim thread1 As Thread
    Private Sub FillDatagridZreadInv(searchdate As Boolean)
        Try
            table = "loc_zread_inventory I INNER JOIN loc_product_formula F ON F.server_formula_id = I.server_inventory_id "
            fields = "I.product_ingredients as Ingredients, i.sku , CONCAT_WS(' ', ROUND(I.stock_primary,0), F.primary_unit) as PrimaryValue , CONCAT_WS(' ', ROUND(I.stock_secondary,0), F.secondary_unit) as UOM , ROUND(I.stock_no_of_servings,0) as NoofServings, I.zreading"
            If searchdate = False Then
                where = "zreading = '" & Format(Now(), "yyyy-MM-dd") & "' AND I.stock_status = 1 AND I.store_id = " & ClientStoreID & " ORDER BY I.product_ingredients ASC"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewZreadInvData, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = "zreading = '" & Format(DateTimePicker17.Value, "yyyy-MM-dd") & "' AND I.stock_status = 1 AND I.store_id = " & ClientStoreID & " ORDER BY I.product_ingredients ASC"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewZreadInvData, errormessage:="", fields:=fields, successmessage:="", where:=where)
            End If
            With DataGridViewZreadInvData
                .Columns(0).HeaderText = "Ingredients"
                .Columns(1).HeaderText = "SKU"
                .Columns(2).HeaderText = "Primary Value"
                .Columns(3).HeaderText = "UOM"
                .Columns(4).HeaderText = "No. of Servings"
                .Columns(5).HeaderText = "Zreading Date"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        FillDatagridZreadInv(True)
    End Sub
    Private Sub MainInventorySub()
        Try
            With DataGridViewZreadInventory
                Dim MainInvId As Integer = 0
                Dim SubInvId As Integer = 0

                Dim MICommand As MySqlCommand
                Dim MIDa As MySqlDataAdapter
                Dim MiDt As DataTable

                Dim MPrimary As Double = 0
                Dim MSecondary As Double = 0
                Dim MNoOfServings As Double = 0

                Dim ZPrimary As Double = 0
                Dim ZSecondary As Double = 0
                Dim ZNoOfServings As Double = 0

                Dim TPrimary As Double = 0
                Dim TSecondary As Double = 0
                Dim TNoOfServings As Double = 0

                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    MainInvId = .Rows(i).Cells(16).Value
                    SubInvId = .Rows(i).Cells(0).Value
                    If MainInvId <> 0 Then
                        Dim MIQuery As String = ""
                        'Get main product stock
                        MIQuery = "SELECT stock_primary, stock_secondary, stock_no_of_servings FROM loc_pos_inventory WHERE inventory_id = " & MainInvId
                        MICommand = New MySqlCommand(MIQuery, LocalhostConn)
                        MIDa = New MySqlDataAdapter(MICommand)
                        MiDt = New DataTable
                        MIDa.Fill(MiDt)
                        For Each row As DataRow In MiDt.Rows
                            MPrimary = row("stock_primary")
                            MSecondary = row("stock_secondary")
                            MNoOfServings = row("stock_no_of_servings")
                        Next
                        'Get sub product value : 5 stock_primary, 6 stock_secondary , 7 stock_no_of_servings
                        ZPrimary = .Rows(i).Cells(5).Value
                        ZSecondary = .Rows(i).Cells(6).Value
                        ZNoOfServings = .Rows(i).Cells(7).Value
                        'Total inventory : Main - Sub = Total zread inv

                        TPrimary = MPrimary - Math.Abs(ZPrimary)
                        TSecondary = MSecondary - Math.Abs(ZSecondary)
                        TNoOfServings = MNoOfServings - Math.Abs(ZNoOfServings)
                        'Update Main inventory 

                        Dim MIQuery1 = "Update loc_pos_inventory SET stock_primary = @1, stock_secondary = @2, stock_no_of_servings = @3 WHERE inventory_id = " & MainInvId
                        Dim MICommand1 = New MySqlCommand(MIQuery1, LocalhostConn)
                        MICommand1.Parameters.Add("@1", MySqlDbType.Double).Value = TPrimary
                        MICommand1.Parameters.Add("@2", MySqlDbType.Double).Value = TSecondary
                        MICommand1.Parameters.Add("@3", MySqlDbType.Double).Value = TNoOfServings
                        MICommand1.ExecuteNonQuery()
                        'Update Sub inventory 
                        Dim MIQuery2 = "Update loc_pos_inventory SET stock_primary = 0, stock_secondary = 0, stock_no_of_servings = 0 WHERE inventory_id = " & SubInvId
                        Dim MICommand2 = New MySqlCommand(MIQuery2, LocalhostConn)
                        MICommand2.ExecuteNonQuery()
                        MICommand2.Dispose()
                        LocalhostConn.close()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            Dim msg = MessageBox.Show("Are you sure you want to reset the sales ? Press Yes to continue or No to cancel", "NOTICE", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If msg = DialogResult.Yes Then
                Dim sql = "TRUNCATE `loc_daily_transaction`; TRUNCATE `loc_daily_transaction_details`;"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                cmd.ExecuteNonQuery()
                reportsdailytransaction(False)
                DataGridViewTransactionDetails.DataSource = Nothing
                DataGridViewTransactionDetails.Rows.Clear()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Dim loopb = 0
    Dim loopa = 0
    Dim PrintSalesDatatable As DataTable
    Private Sub ButtonPrintSales_Click(sender As Object, e As EventArgs) Handles ButtonPrintSales.Click
        Try
            loopa = 100
            loopb = 0
            Dim sql = "SELECT  product_sku , SUM(quantity), SUM(total) FROM loc_daily_transaction_details WHERE zreading >= '" & Format(DateTimePicker3.Value, "yyyy-MM-dd") & "' AND zreading <= '" & Format(DateTimePicker4.Value, "yyyy-MM-dd") & "' AND active = 1  AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' GROUP BY product_name"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            PrintSalesDatatable = New DataTable
            da.Fill(PrintSalesDatatable)
            For i As Integer = 0 To PrintSalesDatatable.Rows.Count - 1 Step +1
                loopb += 10
            Next
            printsales.DefaultPageSettings.PaperSize = New PaperSize("Custom", 200, 300 + loopb)
            previewsales.Document = printsales
            previewsales.ShowDialog()
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub printsales_printdoc(sender As Object, e As Printing.PrintPageEventArgs) Handles printsales.PrintPage
        Try
            Dim font As New Font("Tahoma", 5)
            Dim font1 As New Font("Tahoma", 5, FontStyle.Bold)

            ReceiptHeader(sender, e)
            SimpleTextDisplay(sender, e, "PRODUCT CODE", font1, 0, loopa)
            SimpleTextDisplay(sender, e, "QUANTITY", font1, 70, loopa)
            SimpleTextDisplay(sender, e, "TOTAL SALES", font1, 120, loopa)

            loopa += 20
            For i As Integer = 0 To PrintSalesDatatable.Rows.Count - 1 Step +1
                SimpleTextDisplay(sender, e, PrintSalesDatatable(i)(0), font, 0, loopa)
                RightDisplay1(sender, e, loopa + 20, "", PrintSalesDatatable(i)(1), font, 80, 0)
                RightDisplay1(sender, e, loopa + 20, "", PrintSalesDatatable(i)(2), font, 170, 0)
                loopa += 10
            Next
            CenterTextDisplay(sender, e, "*************************************", font, loopa + 30)
            CenterTextDisplay(sender, e, Format(Now(), "yyyy-MM-dd HH:mm:ss"), font, loopa + 50)

        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Dim ColumnSpacing As Integer = 0
    Private Sub printreturns_printdoc(sender As Object, e As Printing.PrintPageEventArgs) Handles printdocReturns.PrintPage
        Try
            Dim font As New Font("tahoma", 6)
            Dim font2 As New Font("tahoma", 6, FontStyle.Bold)
            Dim brandfont As New Font("tahoma", 8, FontStyle.Bold)
            CenterTextDisplay(sender, e, ClientBrand.ToUpper, brandfont, 10)
            ReadingOR = "R" & Format(Now, "yyddMMHHmmssyy")
            '============================================================================================================================
            CenterTextDisplay(sender, e, "Opt by : Innovention Food Asia Co.", font, 21)
            '============================================================================================================================
            CenterTextDisplay(sender, e, ClientAddress & ", Brgy. " & ClientBrgy, font, 31)
            '============================================================================================================================
            CenterTextDisplay(sender, e, getmunicipality & ", " & getprovince, font, 41)
            '============================================================================================================================
            CenterTextDisplay(sender, e, "VAT REG TIN : " & ClientTin, font, 51)
            '============================================================================================================================
            CenterTextDisplay(sender, e, "MSN : " & ClientMSN, font, 61)
            '============================================================================================================================
            CenterTextDisplay(sender, e, "MIN : " & ClientMIN, font, 71)
            '============================================================================================================================
            CenterTextDisplay(sender, e, "PTUN : " & ClientPTUN, font, 81)
            '============================================================================================================================
            RightToLeftDisplay(sender, e, 100, "TERMINAL REPORT", "RETURNS", font2, 20, 0)
            '============================================================================================================================
            SimpleTextDisplay(sender, e, ReadingOR, font, 0, 90)
            SimpleTextDisplay(sender, e, "----------------------------------------------------------------", font, 0, 100)
            '============================================================================================================================
            With DataGridViewReturns
                Dim FooterSpacing As Integer = 0
                If CheckBoxPRINTALL.Checked = False Then
                    RightToLeftDisplay(sender, e, 140, "RETURN ITEM LOGS : " & Format(DateTimePicker14.Value, "yyyy-MM-dd") & " " & Format(DateTimePicker13.Value, "yyyy-MM-dd"), "", font, 20, 0)
                    RightToLeftDisplay(sender, e, 160, "TRANSACTION NUMBER: ", "", font, 20, 0)
                    RightToLeftDisplay(sender, e, 170, "SERVICE CREW: ", "", font, 20, 0)
                    RightToLeftDisplay(sender, e, 180, "DATE: ", "", font, 20, 0)
                    RightToLeftDisplay(sender, e, 190, "REASON:", "", font, 20, 0)
                    RightToLeftDisplay(sender, e, 200, Space(5) & .SelectedRows(0).Cells(2).Value.ToString, "", font, 20, 0)
                    SimpleTextDisplay(sender, e, Space(40) & .SelectedRows(0).Cells(0).Value.ToString, font, 0, 140)
                    SimpleTextDisplay(sender, e, Space(40) & .SelectedRows(0).Cells(1).Value.ToString, font, 0, 150)
                    SimpleTextDisplay(sender, e, Space(40) & .SelectedRows(0).Cells(3).Value.ToString, font, 0, 160)
                    SimpleTextDisplay(sender, e, "----------------------------------------------------------------", font, 0, 260)
                    CenterTextDisplay(sender, e, S_Zreading & " " & Format(Now(), "HH:mm:ss"), font, 290)
                Else
                    For i As Integer = 0 To .Rows.Count - 1 Step +1
                        RightToLeftDisplay(sender, e, 140, "RETURN ITEM LOGS : " & Format(DateTimePicker14.Value, "yyyy-MM-dd") & " " & Format(DateTimePicker13.Value, "yyyy-MM-dd"), "", font, 20, 0)
                        RightToLeftDisplay(sender, e, 160 + ColumnSpacing, "TRANSACTION NUMBER: ", "", font, 20, 0)
                        RightToLeftDisplay(sender, e, 170 + ColumnSpacing, "SERVICE CREW: ", "", font, 20, 0)
                        RightToLeftDisplay(sender, e, 180 + ColumnSpacing, "DATE: ", "", font, 20, 0)
                        RightToLeftDisplay(sender, e, 190 + ColumnSpacing, "REASON:", "", font, 20, 0)
                        RightToLeftDisplay(sender, e, 200 + ColumnSpacing, Space(5) & .Rows(i).Cells(2).Value.ToString, "", font, 20, 0)
                        SimpleTextDisplay(sender, e, Space(40) & .Rows(i).Cells(0).Value.ToString, font, 0, 140 + ColumnSpacing)
                        SimpleTextDisplay(sender, e, Space(40) & .Rows(i).Cells(1).Value.ToString, font, 0, 150 + ColumnSpacing)
                        SimpleTextDisplay(sender, e, Space(40) & .Rows(i).Cells(3).Value.ToString, font, 0, 160 + ColumnSpacing)
                        ColumnSpacing += 70
                        FooterSpacing += 60
                    Next
                    SimpleTextDisplay(sender, e, "----------------------------------------------------------------", font, 0, 260 + FooterSpacing)
                    CenterTextDisplay(sender, e, S_Zreading & " " & Format(Now(), "HH:mm:ss"), font, 290 + FooterSpacing)
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If DataGridViewReturns.Rows.Count > 0 Then
            Dim B As Integer = 0
            If CheckBoxPRINTALL.Checked = False Then
                printdocReturns.DefaultPageSettings.PaperSize = New PaperSize("Custom", 215, 320)
                If S_Print_Returns = "YES" Then
                    printdocReturns.Print()
                Else
                    PrintPreviewDialogReturns.Document = printdocReturns
                    PrintPreviewDialogReturns.ShowDialog()
                End If
            Else
                For i As Integer = 0 To DataGridViewReturns.Rows.Count - 1 Step +1
                    B += 65
                Next
                printdocReturns.DefaultPageSettings.PaperSize = New PaperSize("Custom", 215, 320 + B)
                If S_Print_Returns = "YES" Then
                    printdocReturns.Print()
                Else
                    PrintPreviewDialogReturns.Document = printdocReturns
                    PrintPreviewDialogReturns.ShowDialog()
                End If
            End If
            B = 0
            ColumnSpacing = 0
        Else
            MsgBox("Select returned product first.")
        End If
    End Sub
    Private Sub ButtonSearchCrewSales_Click(sender As Object, e As EventArgs) Handles ButtonSearchCrewSales.Click
        Try
            If ComboBoxUserIDS.SelectedIndex = -1 Then
                MsgBox("Select crew id first")
            Else
                LoadCrewSales(True)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class