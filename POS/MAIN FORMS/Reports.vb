Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing
Imports System.IO
Imports System.Text
Public Class Reports
    Private WithEvents printdoc As PrintDocument = New PrintDocument
    Private WithEvents printdocXread As PrintDocument = New PrintDocument

    Private PrintPreviewDialog1 As New PrintPreviewDialog
    Private PrintPreviewDialogXread As New PrintPreviewDialog
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
    Private Sub Reports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TabControl1.TabPages(0).Text = "Daily Transactions"
        TabControl1.TabPages(1).Text = "System Logs"
        TabControl1.TabPages(2).Text = "Sales Report"
        TabControl1.TabPages(3).Text = "Expense Report"
        TabControl1.TabPages(4).Text = "Total Expenses"
        TabControl1.TabPages(5).Text = "Transaction Logs"
        TabControl1.TabPages(6).Text = "Item Return"
        TabControl1.TabPages(7).Text = "Deposit Slip"
        TabControl1.TabPages(8).Text = "Z/X Reading"

        reportsdailytransaction(False)
        reportssystemlogs(False)
        reportssales(False)
        reportstransactionlogs(False)
        expensereports(False)
        reportexpensedet(False)
        reportsreturnsandrefunds(False)
        viewdeposit(False)
        If S_Zreading = returndateformat(Now) Then
            ButtonZread.Enabled = False
        Else
            ButtonZread.Enabled = True
        End If
    End Sub
    Public Sub reportssystemlogs(ByVal searchdate As Boolean)
        table = "`loc_system_logs`"
        fields = "`log_type`, `log_description`, `log_date_time`"
        If searchdate = False Then
            where = " date(zreading) = CURRENT_DATE() AND log_type <> 'TRANSACTION' AND log_store = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewSysLog, errormessage:="", fields:=fields, successmessage:="", where:=where)
        Else
            where = " log_type <> 'TRANSACTION' AND log_store = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' AND date(zreading) >= '" & returndateformat(DateTimePicker9.Text) & "' AND date(zreading) <= '" & returndateformat(DateTimePicker10.Text) & "'"
            GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewSysLog, errormessage:="", fields:=fields, successmessage:="", where:=where)
        End If
        With DataGridViewSysLog
            .Columns(0).HeaderText = "Type"
            .Columns(1).HeaderText = "Description"
            .Columns(2).HeaderText = "Date and Time"
        End With
    End Sub
    Public Sub reportsreturnsandrefunds(ByVal searchdate As Boolean)
        table = "`loc_refund_return_details`"
        fields = "`transaction_number`, `crew_id`, `reason`, `datestamp`"
        If searchdate = False Then
            where = " date(zreading) = CURRENT_DATE() AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewReturns, errormessage:="", fields:=fields, successmessage:="", where:=where)
        Else
            where = " date(zreading) >= '" & returndateformat(DateTimePicker14.Text) & "' AND date(zreading) <= '" & returndateformat(DateTimePicker13.Text) & "' AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
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
    End Sub
    Public Sub reportstransactionlogs(ByVal searchdate As Boolean)
        table = "`loc_system_logs`"
        fields = "`log_type`, `log_description`, `log_date_time`"
        If searchdate = False Then
            where = " log_type = 'TRANSACTION' AND date(zreading) = CURRENT_DATE() AND log_store = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' "
            GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewTRANSACTIONLOGS, errormessage:="", fields:=fields, successmessage:="", where:=where)
        Else
            where = " log_type = 'TRANSACTION' AND date(zreading) >= '" & returndateformat(DateTimePicker11.Text) & "' AND date(zreading) <= '" & returndateformat(DateTimePicker12.Text) & "' AND log_store = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' "
            GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewTRANSACTIONLOGS, errormessage:="", fields:=fields, successmessage:="", where:=where)
        End If
        With DataGridViewTRANSACTIONLOGS
            .Columns(0).HeaderText = "Type"
            .Columns(1).HeaderText = "Description"
            .Columns(2).HeaderText = "Date and Time"
        End With
    End Sub
    Public Sub reportsdailytransaction(ByVal searchdate As Boolean)
        Try
            table = "`loc_daily_transaction`"
            fields = "`transaction_id`,`date`,`time`,`transaction_number`,`crew_id`,`amounttendered`,`moneychange`,`active`,`discount`,`amountdue`,`vat_exempt`,`transaction_type`,`vat`,`si_number`"
            If searchdate = False Then
                where = " zreading = CURRENT_DATE() AND active IN(1,3) AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewDaily, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " zreading >= '" & returndateformat(DateTimePicker1.Text) & "' and zreading <= '" & returndateformat(DateTimePicker2.Text) & "' AND active IN(1,3) AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewDaily, errormessage:="", fields:=fields, successmessage:="", where:=where)
            End If
            With DataGridViewDaily
                .Columns(0).Visible = False
                .Columns(1).HeaderCell.Value = "TRN. Date"

                .Columns(2).HeaderCell.Value = "TRN. Time"
                .Columns(3).HeaderCell.Value = "TRN. Number"
                .Columns(4).HeaderCell.Value = "Service Crew"
                .Columns(5).HeaderCell.Value = "Money"
                .Columns(6).HeaderCell.Value = "Change"
                .Columns(7).Visible = False
                .Columns(8).HeaderCell.Value = "Discount"
                .Columns(9).HeaderCell.Value = "Amount Due"
                .Columns(10).HeaderCell.Value = "Vat Exempt"
                .Columns(11).HeaderCell.Value = "TRN. Type"
                .Columns(12).Visible = False
                .Columns(13).Visible = False
                .Columns.Item(5).DefaultCellStyle.Format = "n2"
                .Columns.Item(6).DefaultCellStyle.Format = "n2"
                .Columns.Item(1).DefaultCellStyle.Format = "yyyy/MM/dd"
                .Font = New Font("Century Gothic", 9)
                For Each row As DataRow In dt.Rows
                    row("crew_id") = GLOBAL_SELECT_FUNCTION_RETURN(table:="loc_users", fields:="full_name", returnvalrow:="full_name", values:="uniq_id ='" & row("crew_id") & "'")
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub reportssales(ByVal searchdate As Boolean)
        Try
            table = "`loc_daily_transaction_details`"
            fields = "`product_sku`, `product_name`, `quantity`, `price`, `total`, `created_at`"
            If searchdate = False Then
                where = " zreading = CURRENT_DATE()  AND active = 1 AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewSales, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " zreading >= '" & returndateformat(DateTimePicker3.Text) & "' AND zreading <= '" & returndateformat(DateTimePicker4.Text) & "' AND active = 1  AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
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
        End Try
    End Sub
    Public Sub reportexpensedet(ByVal searchdate As Boolean)
        Try
            table = "`loc_expense_details`"
            fields = "`expense_type`, `item_info`, `quantity`, `price`, `amount`, `attachment`, `created_at`, `time`"
            If searchdate = False Then
                where = " zreading = CURRENT_DATE() AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewExpenseDetails, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " zreading >= '" & returndateformat(DateTimePicker5.Text) & "' AND zreading <= '" & returndateformat(DateTimePicker6.Text) & "' AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewExpenseDetails, errormessage:="", fields:=fields, successmessage:="", where:=where)
            End If

            With DataGridViewExpenseDetails
                .Columns(0).HeaderText = "Expense Type"
                .Columns(1).HeaderText = "Description"
                .Columns(2).HeaderText = "Quantity"
                .Columns(3).HeaderText = "Price"
                .Columns(4).HeaderText = "Total Amount"
                .Columns(5).Visible = False
                .Columns(6).HeaderText = "Date Created"
                .Columns(7).HeaderText = "Time Created"
                Label15.Text = SumOfColumnsToDecimal(DataGridViewExpenseDetails, 4)
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Public Sub expensereports(ByVal searchdate As Boolean)
        Try
            table = "`loc_expense_list`"
            fields = "`expense_id`, `crew_id`, `expense_number`, `total_amount`, `paid_amount`, `unpaid_amount`, `date`, `time`"
            If searchdate = False Then
                where = " zreading = CURRENT_DATE() AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewEXPENSES, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " zreading >= '" & returndateformat(DateTimePicker7.Text) & "' and zreading <= '" & returndateformat(DateTimePicker8.Text) & "' AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
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
                .Columns(7).HeaderCell.Value = "Time"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub viewexpensesdetails(ByVal expense_number As String)
        Try
            table = "`loc_expense_details`"
            fields = "`expense_type`, `item_info`, `quantity`, `price`, `amount`, `created_at`, `time`"
            GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewEXPENSEDET, errormessage:="", fields:=fields, successmessage:="", where:=" expense_number = '" & expense_number & "'")
            With DataGridViewEXPENSEDET
                .Columns(0).HeaderCell.Value = "Type"
                .Columns(1).HeaderCell.Value = "Description"
                .Columns(2).HeaderCell.Value = "Quantity"
                .Columns(3).HeaderCell.Value = "Price"
                .Columns(4).HeaderCell.Value = "Amount"
                .Columns(5).HeaderCell.Value = "Date"
                .Columns(6).HeaderCell.Value = "Time"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub viewtransactiondetails(ByVal transaction_number As String)
        Try
            table = "`loc_daily_transaction_details`"
            fields = "`product_name`, `quantity`, `price`, `total`, `product_category`"
            GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewTransactionDetails, errormessage:="", fields:=fields, successmessage:="", where:=" transaction_number = '" & transaction_number & "'")
            With DataGridViewTransactionDetails
                .Columns(0).HeaderCell.Value = "Product Name"
                .Columns(1).HeaderCell.Value = "Quantity"
                .Columns(2).HeaderCell.Value = "Price"
                .Columns(3).HeaderCell.Value = "Total"
                .Columns(4).Visible = False
                .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopRight
                .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopRight
                .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopRight
                .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns.Item(1).DefaultCellStyle.Format = "n2"
                .Columns.Item(2).DefaultCellStyle.Format = "n2"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            localconn.Close()
            da.Dispose()
        End Try
    End Sub
    Public Sub viewdeposit(ByVal searchdate As Boolean)
        Try
            table = "`loc_deposit`"
            fields = "`dep_id`, `name`, `crew_id`, `transaction_number`, `amount`, `bank`, `transaction_date`, `store_id`, `guid`, `date_created`"
            If searchdate = False Then
                where = " transaction_date = CURRENT_DATE() AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewDeposits, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " transaction_date >= '" & returndateformat(DateTimePicker16.Text) & "' and transaction_date <= '" & returndateformat(DateTimePicker15.Text) & "' AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
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
        End Try
    End Sub
    Private Sub ButtonExit_Click(sender As Object, e As EventArgs) Handles ButtonExit.Click
        MDIFORM.Button2.PerformClick()
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles ButtonSearchDailyTransaction.Click
        reportsdailytransaction(True)
        DataGridViewTransactionDetails.DataSource = Nothing
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
    Private Sub ButtonTotalExpenses_Click(sender As Object, e As EventArgs) Handles ButtonTotalExpenses.Click
        reportexpensedet(True)
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
            Dim datagridid = DataGridViewEXPENSES.SelectedRows(0).Cells(2).Value.ToString()
            viewexpensesdetails(datagridid)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub DataGridViewDaily_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewDaily.CellClick
        Try
            data = DataGridViewDaily.SelectedRows(0).Cells(3).Value.ToString()
            data2 = DataGridViewDaily.SelectedRows(0).Cells(4).Value.ToString()
            TextBoxCustomerID.Text = data
            transaction_number = (Val(TextBoxCustomerID.Text))
            viewtransactiondetails(transaction_number:=transaction_number)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridViewTransactionDetails.Rows.Count > 0 Then
            a = 115
            total = SumOfColumnsToDecimal(DataGridViewTransactionDetails, 3)
            b = 0
            Try
                For i As Integer = 0 To DataGridViewTransactionDetails.Rows.Count - 1 Step +1
                    printdoc.DefaultPageSettings.PaperSize = New PaperSize("Custom", 200, 420 + b)
                    b += 10
                Next
                PrintPreviewDialog1.Document = printdoc
                PrintPreviewDialog1.ShowDialog()
                ' printdoc.Print()
            Catch exp As Exception
                MessageBox.Show("An error occurred while trying to load the " &
                    "document for Print Preview. Make sure you currently have " &
                    "access to a printer. A printer must be localconnected and " &
                    "accessible for Print Preview to work.", Me.Text,
                     MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MsgBox("Select Transaction First!")
        End If
    End Sub
    Private Sub pdoc_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles printdoc.PrintPage
        a = 0
        Dim font As New Font("Bahnschrift Light SemiCondensed", 7)
        Dim font1 As New Font("Bahnschrift  SemiCondensed", 7)
        Dim fontAddon As New Font("Bahnschrift Light SemiCondensed", 5)
        Dim font2 As New Font("Bahnschrift Light SemiCondensed", 9)
        Dim font3 As New Font("Bahnschrift Condensed", 12)
        Dim brandfont As New Font("Bahnschrift Condensed", 8)
        '    e.Graphics.DrawRectangle(Pens.Black, e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width, e.MarginBounds.Height)
        Dim shopnameX As Integer = 10, shopnameY As Integer = 20
        Dim StrRight As New StringFormat()
        'Receipt Header
        'Dim font As New Font("Tohama", 7, FontStyle.Regular)
        'Dim font1 As New Font("Tohama", 7, FontStyle.Regular)
        'Dim font2 As New Font("Tohama", 10, FontStyle.Regular)
        'Dim brandfont As New Font("Tohama", 7, FontStyle.Bold)
        Dim sngCenterPagebrand As Single
        sngCenterPagebrand = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(ClientBrand, brandfont).Width / 2)
        e.Graphics.DrawString(ClientBrand, brandfont, Brushes.Black, sngCenterPagebrand, 10)

        Dim sngCenterPageVatReg As Single
        sngCenterPageVatReg = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString("VAT REG TIN " & ClientTin, brandfont).Width / 2)
        e.Graphics.DrawString("VAT REG TIN " & ClientTin, font, Brushes.Black, sngCenterPageVatReg, 21)

        Dim sngCenterPageaddbrgy As Single
        sngCenterPageaddbrgy = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(ClientAddress & " Brgy." & ClientBrgy, brandfont).Width / 2)
        e.Graphics.DrawString(ClientAddress & " Brgy." & ClientBrgy, font, Brushes.Black, sngCenterPageaddbrgy, 31)

        Dim sngCenterPagemun As Single
        sngCenterPagemun = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(getmunicipality & ", " & getprovince, brandfont).Width / 2)
        e.Graphics.DrawString(getmunicipality & ", " & getprovince, font, Brushes.Black, sngCenterPagemun, 41)

        Dim sngCenterPagetel As Single
        sngCenterPagetel = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(ClientTel, brandfont).Width / 2)
        e.Graphics.DrawString(ClientTel, font, Brushes.Black, sngCenterPagetel, 51)

        e.Graphics.DrawString("Name: ", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + 50))
        e.Graphics.DrawLine(Pens.Black, 37, 77, 180, 77)
        e.Graphics.DrawString("Tin:", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + 60))
        e.Graphics.DrawLine(Pens.Black, 25, 87, 180, 87)
        e.Graphics.DrawString("Address:", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + 70))
        e.Graphics.DrawLine(Pens.Black, 46, 97, 180, 97)
        e.Graphics.DrawString("Business Style:", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + 80))
        e.Graphics.DrawLine(Pens.Black, 70, 107, 180, 107)
        'Items
        Dim format1st As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
        Dim abc As Integer = 0
        For i As Integer = 0 To DataGridViewTransactionDetails.Rows.Count - 1 Step +1
            Dim rect1st As RectangleF = New RectangleF(10.0F, 115 + abc, 173.0F, 100.0F)
            Dim price = Convert.ToDecimal(DataGridViewTransactionDetails.Rows(i).Cells(3).FormattedValue).ToString("0.00")
            '=========================================================================================================================================================
            table = "loc_admin_products"
            fields = "product_sku"
            value = " product_name ='" & DataGridViewTransactionDetails.Rows(i).Cells(0).Value & "'"
            returnvalrow = "product_sku"
            If DataGridViewTransactionDetails.Rows(i).Cells(4).Value.ToString = "Add-Ons" Then
                e.Graphics.DrawString("     @" & DataGridViewTransactionDetails.Rows(i).Cells(1).Value & " " & GLOBAL_SELECT_FUNCTION_RETURN(table:=table, fields:=fields, returnvalrow:=returnvalrow, values:=value), fontAddon, Brushes.Black, rect1st)
            Else
                e.Graphics.DrawString(DataGridViewTransactionDetails.Rows(i).Cells(1).Value & " " & GLOBAL_SELECT_FUNCTION_RETURN(table:=table, fields:=fields, returnvalrow:=returnvalrow, values:=value), font, Brushes.Black, rect1st)
            End If
            e.Graphics.DrawString(price, font, Brushes.Black, rect1st, format1st)
            a += 10
            abc += 10
            '=========================================================================================================================================================
        Next
        a += 120
        Dim SiNumberDgv As Integer = DataGridViewDaily.SelectedRows(0).Cells(13).Value
        Dim SiNumberString As String = SiNumberDgv.ToString("0000000000")
        If DataGridViewDaily.SelectedRows(0).Cells(8).Value = 0 Then
            Dim format As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
            Dim rect3 As RectangleF = New RectangleF(10.0F, a, 173.0F, 100.0F)
            e.Graphics.DrawString("AMOUNT DUE:", font3, Brushes.Black, rect3)
            e.Graphics.DrawString("P" & total, font3, Brushes.Black, rect3, format)
            'Cash
            Dim aNumber As Double = DataGridViewDaily.SelectedRows(0).Cells(5).Value
            Dim cash = String.Format("{0:n2}", aNumber)
            Dim rect4 As RectangleF = New RectangleF(10.0F, a + 15, 173.0F, 100.0F)
            e.Graphics.DrawString("CASH:", font2, Brushes.Black, rect4)
            e.Graphics.DrawString("P" & cash, font2, Brushes.Black, rect4, format)
            'Change
            Dim aNumber1 As Double = DataGridViewDaily.SelectedRows(0).Cells(6).Value
            Dim change = String.Format("{0:n2}", aNumber1)
            Dim rect5 As RectangleF = New RectangleF(10.0F, a + 25, 173.0F, 100.0F)
            e.Graphics.DrawString("CHANGE:", font2, Brushes.Black, rect5)
            e.Graphics.DrawString("P" & change, font2, Brushes.Black, rect5, Format)
            'Vatable
            Dim vatable = Math.Round(total / 1.12, 2)
            e.Graphics.DrawString("**************************************************", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 27))
            Dim rect6 As RectangleF = New RectangleF(10.0F, a + 52, 173.0F, 100.0F)
            e.Graphics.DrawString("     Vatable", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 32))
            e.Graphics.DrawString("    " & vatable, font1, Brushes.Black, rect6, Format)
            'Vat Exempt Sales
            Dim rect7 As RectangleF = New RectangleF(10.0F, a + 62, 173.0F, 100.0F)
            e.Graphics.DrawString("     Vat Exempt Sales", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 42))
            e.Graphics.DrawString("    " & "0.00", font1, Brushes.Black, rect7, Format)
            'Zero Rated Sales
            Dim rect8 As RectangleF = New RectangleF(10.0F, a + 72, 173.0F, 100.0F)
            e.Graphics.DrawString("     Zero Rated Sales", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 52))
            e.Graphics.DrawString("    " & "0.00", font1, Brushes.Black, rect8, Format)
            'VAT
            Dim rect9 As RectangleF = New RectangleF(10.0F, a + 82, 173.0F, 100.0F)
            e.Graphics.DrawString("     VAT", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 62))
            e.Graphics.DrawString("    " & Math.Round(total - vatable, 2) & "-", font1, Brushes.Black, rect9, format)
            'Total
            Dim rect10 As RectangleF = New RectangleF(10.0F, a + 92, 173.0F, 100.0F)
            e.Graphics.DrawString("     Total", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 72))
            e.Graphics.DrawString("    " & total, font1, Brushes.Black, rect10, Format)
            'INFO
            e.Graphics.DrawString("**************************************************", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 82))
            e.Graphics.DrawString("Transaction Type: " & DataGridViewDaily.SelectedRows(0).Cells(11).Value.ToString, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 90))
            e.Graphics.DrawString("Total Item(s): " & DataGridViewTransactionDetails.Rows.Count, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 100))
            e.Graphics.DrawString("Store No: " & ClientStoreID, font, Brushes.Black, New PointF(shopnameX + 110, shopnameY + a + 100))
            '=========================================================================================================================================================
            e.Graphics.DrawString("Cashier: " & returnuserid(full_name:=data2) & " " & data2, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 110))
            '=========================================================================================================================================================
            e.Graphics.DrawString("Date & Time: " & returndateformatDGV(DataGridViewDaily.SelectedRows(0).Cells(1).Value.ToString) & " " & DataGridViewDaily.SelectedRows(0).Cells(2).Value.ToString, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 120))
            e.Graphics.DrawString("Terminal No: ", font, Brushes.Black, New PointF(shopnameX + 110, shopnameY + a + 130))
            e.Graphics.DrawString("Trans ID: " & DataGridViewDaily.SelectedRows(0).Cells(3).Value, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 130))
            e.Graphics.DrawString("This serves as your Sales Invoice", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 140))
            e.Graphics.DrawString("SI No: " & SiNumberString, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 150))
            e.Graphics.DrawString("**************************************************", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 160))

            ReceiptFooter(sender, e, a - 10)
        Else
            'Total
            Dim format As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
            Dim rect3 As RectangleF = New RectangleF(10.0F, a + 15, 173.0F, 100.0F)
            e.Graphics.DrawString("TOTAL:", font2, Brushes.Black, rect3)
            e.Graphics.DrawString("P" & total, font2, Brushes.Black, rect3, format)
            'Discount
            Dim aNumber0 As Double = DataGridViewDaily.SelectedRows(0).Cells(8).Value
            Dim disc = String.Format(aNumber0)
            Dim rect0 As RectangleF = New RectangleF(10.0F, a + 25, 173.0F, 100.0F)
            e.Graphics.DrawString("DISCOUNT:", font2, Brushes.Black, rect0)
            e.Graphics.DrawString(disc & "-", font2, Brushes.Black, rect0, format)
            'cash
            Dim aNumber As Double = DataGridViewDaily.SelectedRows(0).Cells(5).Value
            Dim cash = String.Format("{0:n2}", aNumber)
            Dim rect4 As RectangleF = New RectangleF(10.0F, a + 35, 173.0F, 100.0F)
            e.Graphics.DrawString("CASH:", font2, Brushes.Black, rect4)
            e.Graphics.DrawString("P" & cash, font2, Brushes.Black, rect4, format)
            'change
            Dim aNumber1 As Double = DataGridViewDaily.SelectedRows(0).Cells(6).Value
            Dim change = String.Format("{0:n2}", aNumber1)
            Dim rect5 As RectangleF = New RectangleF(10.0F, a + 45, 173.0F, 100.0F)
            e.Graphics.DrawString("CHANGE:", font2, Brushes.Black, rect5)
            e.Graphics.DrawString("P" & change, font2, Brushes.Black, rect5, format)
            'amount due
            Dim aNumber2 As Double = DataGridViewDaily.SelectedRows(0).Cells(9).Value
            Dim amountdue = String.Format("{0:n2}", aNumber2)
            Dim rect11 As RectangleF = New RectangleF(10.0F, a, 173.0F, 100.0F)
            e.Graphics.DrawString("AMOUNT DUE:", font3, Brushes.Black, rect11)
            e.Graphics.DrawString("P" & aNumber2, font3, Brushes.Black, rect11, format)
            e.Graphics.DrawString("**************************************************", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 47))
            If S_ZeroRated = "0" Then
                'Vatable
                Dim vatExemptSales As Double = Math.Round(total / 1.12, 2)
                Dim vatable = Math.Round(aNumber2 / 1.12, 2)
                Dim rect6 As RectangleF = New RectangleF(10.0F, a + 72, 173.0F, 100.0F)
                e.Graphics.DrawString("     Vatable", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 52))
                e.Graphics.DrawString("    " & "0.00", font1, Brushes.Black, rect6, format)

                'Vat Exempt Sales
                Dim rect7 As RectangleF = New RectangleF(10.0F, a + 82, 173.0F, 100.0F)
                e.Graphics.DrawString("     Vat Exempt Sales", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 62))
                e.Graphics.DrawString("    " & DataGridViewDaily.SelectedRows(0).Cells(10).Value, font1, Brushes.Black, rect7, format)
                'Zero Rated Sales
                Dim rect8 As RectangleF = New RectangleF(10.0F, a + 92, 173.0F, 100.0F)
                e.Graphics.DrawString("     Zero Rated Sales", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 72))
                e.Graphics.DrawString("    " & "0.00", font1, Brushes.Black, rect8, format)
                'VAT
                Dim rect9 As RectangleF = New RectangleF(10.0F, a + 102, 173.0F, 100.0F)
                e.Graphics.DrawString("     VAT", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 82))
                e.Graphics.DrawString("    " & DataGridViewDaily.SelectedRows(0).Cells(12).Value & "-", font1, Brushes.Black, rect9, format)
                'Total
                'Dim rect10 As RectangleF = New RectangleF(10.0F, a + 112, 173.0F, 100.0F)
                'e.Graphics.DrawString("     Total", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 92))
                'e.Graphics.DrawString("    " & DataGridViewDaily.SelectedRows(0).Cells(9).Value, font1, Brushes.Black, rect10, format)
                'INFO
                e.Graphics.DrawString("**************************************************", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 92))
                e.Graphics.DrawString("Transaction Type: " & DataGridViewDaily.SelectedRows(0).Cells(11).Value.ToString, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 100))
                e.Graphics.DrawString("Total Item(s): " & DataGridViewTransactionDetails.Rows.Count, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 110))
                e.Graphics.DrawString("Store No: " & ClientStoreID, font, Brushes.Black, New PointF(shopnameX + 110, shopnameY + a + 110))
                '=========================================================================================================================================================
                e.Graphics.DrawString("Cashier: " & returnuserid(full_name:=data2) & " " & data2, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 120))
                '=========================================================================================================================================================
                e.Graphics.DrawString("Date & Time: " & DataGridViewDaily.SelectedRows(0).Cells(1).Value.ToString & " " & DataGridViewDaily.SelectedRows(0).Cells(2).Value.ToString, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 130))
                e.Graphics.DrawString("Terminal No: ", font, Brushes.Black, New PointF(shopnameX + 110, shopnameY + a + 140))
                e.Graphics.DrawString("Trans ID: " & DataGridViewDaily.SelectedRows(0).Cells(3).Value, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 140))
                e.Graphics.DrawString("This serves as your Sales Invoice", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 150))
                e.Graphics.DrawString("SI No: " & SiNumberString, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 160))
                e.Graphics.DrawString("**************************************************", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 170))

                ReceiptFooter(sender, e, a)
            Else
                Dim vatExemptSales As Double = Math.Round(total / 1.12, 2)
                Dim vatable = Math.Round(aNumber2 / 1.12, 2)
                Dim rect6 As RectangleF = New RectangleF(10.0F, a + 72, 173.0F, 100.0F)
                e.Graphics.DrawString("     Vatable", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 52))
                e.Graphics.DrawString("    " & "0.00", font1, Brushes.Black, rect6, format)

                'Vat Exempt Sales
                Dim rect7 As RectangleF = New RectangleF(10.0F, a + 82, 173.0F, 100.0F)
                e.Graphics.DrawString("     Vat Exempt Sales", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 62))
                e.Graphics.DrawString("    " & DataGridViewDaily.SelectedRows(0).Cells(9).Value, font1, Brushes.Black, rect7, format)
                'Zero Rated Sales
                Dim rect8 As RectangleF = New RectangleF(10.0F, a + 92, 173.0F, 100.0F)
                e.Graphics.DrawString("     Zero Rated Sales", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 72))
                e.Graphics.DrawString("    " & "0.00", font1, Brushes.Black, rect8, format)
                'VAT
                Dim rect9 As RectangleF = New RectangleF(10.0F, a + 102, 173.0F, 100.0F)
                e.Graphics.DrawString("     VAT", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 82))
                e.Graphics.DrawString("    " & "0.00", font1, Brushes.Black, rect9, format)
                'Total
                'Dim rect10 As RectangleF = New RectangleF(10.0F, a + 112, 173.0F, 100.0F)
                'e.Graphics.DrawString("     Total", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 92))
                'e.Graphics.DrawString("    " & DataGridViewDaily.SelectedRows(0).Cells(9).Value, font1, Brushes.Black, rect10, format)
                'INFO
                e.Graphics.DrawString("**************************************************", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 92))
                e.Graphics.DrawString("Transaction Type: " & DataGridViewDaily.SelectedRows(0).Cells(11).Value.ToString, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 100))
                e.Graphics.DrawString("Total Item(s): " & DataGridViewTransactionDetails.Rows.Count, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 110))
                e.Graphics.DrawString("Store No: " & ClientStoreID, font, Brushes.Black, New PointF(shopnameX + 110, shopnameY + a + 110))
                '=========================================================================================================================================================
                e.Graphics.DrawString("Cashier: " & returnuserid(full_name:=data2) & " " & data2, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 120))
                '=========================================================================================================================================================
                e.Graphics.DrawString("Date & Time: " & DataGridViewDaily.SelectedRows(0).Cells(1).Value.ToString & " " & DataGridViewDaily.SelectedRows(0).Cells(2).Value.ToString, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 130))
                e.Graphics.DrawString("Terminal No: ", font, Brushes.Black, New PointF(shopnameX + 110, shopnameY + a + 140))
                e.Graphics.DrawString("Trans ID: " & DataGridViewDaily.SelectedRows(0).Cells(3).Value, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 140))
                e.Graphics.DrawString("This serves as your Sales Invoice", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 150))
                e.Graphics.DrawString("SI No: " & SiNumberString, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 160))
                e.Graphics.DrawString("**************************************************", font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 170))
                ReceiptFooter(sender, e, a)
            End If
        End If
    End Sub
    Dim XreadOrZread As String
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        XreadOrZread = "X-READ"
        printdocXread.DefaultPageSettings.PaperSize = New PaperSize("Custom", 200, 800)
        PrintPreviewDialogXread.Document = printdocXread
        PrintPreviewDialogXread.ShowDialog()
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printdocXread.PrintPage
        Dim font As New Font("Bahnschrift Light SemiCondensed", 7)
        Dim brandfont As New Font("Bahnschrift Condensed", 9)
        CenterTextDisplay(sender, e, ClientBrand.ToUpper, brandfont, 10)
        '============================================================================================================================
        CenterTextDisplay(sender, e, "Opt by : Innovention Food Asia Co.", font, 21)
        '============================================================================================================================
        CenterTextDisplay(sender, e, ClientAddress & ", Brgy. " & ClientBrgy, font, 31)
        '============================================================================================================================
        CenterTextDisplay(sender, e, getmunicipality & ", " & getprovince, Font, 41)
        '============================================================================================================================
        CenterTextDisplay(sender, e, "VAT REG TIN : " & ClientTin, Font, 51)
        '============================================================================================================================
        CenterTextDisplay(sender, e, "MSN : T500114100140", Font, 61)
        '============================================================================================================================
        CenterTextDisplay(sender, e, "MIN : 140351765", Font, 71)
        '============================================================================================================================
        CenterTextDisplay(sender, e, "PTUN : 0414-038-184993-000", Font, 81)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 100, "TERMINAL REPORT", XreadOrZread, font)
        '============================================================================================================================
        SimpleTextDisplay(sender, e, "XT0000002110", Font, 0, 90)
        SimpleTextDisplay(sender, e, "----------------------------------------", Font, 0, 95)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 123, "DESCRIPTION", "QTY/AMOUNT", Font)
        '============================================================================================================================
        SimpleTextDisplay(sender, e, "----------------------------------------", Font, 0, 110)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 140, "TERMINAL N0.", "1", Font)
        RightToLeftDisplay(sender, e, 155, "GROSS", "3000.00", Font)
        RightToLeftDisplay(sender, e, 165, "LESS VAT (VE)", "3000.00", Font)
        RightToLeftDisplay(sender, e, 175, "LESS VAT DIPLOMAT", "3000.00", Font)
        RightToLeftDisplay(sender, e, 185, "LESS VAT (OTHER)", "3000.00", Font)
        RightToLeftDisplay(sender, e, 195, "ADD VAT", "3000.00", Font)
        RightToLeftDisplay(sender, e, 205, "DAILY SALES", "3000.00", Font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 220, "VAT AMOUNT", "1", Font)
        RightToLeftDisplay(sender, e, 230, "LOCAL GOV'T TAX", "3000.00", Font)
        RightToLeftDisplay(sender, e, 240, "VATABLE SALES", "3000.00", Font)
        RightToLeftDisplay(sender, e, 250, "ZERO RATED SALES", "3000.00", Font)
        RightToLeftDisplay(sender, e, 260, "VAT EXEMPT SALES", "3000.00", Font)
        RightToLeftDisplay(sender, e, 270, "LESS DISC (VE)", "3000.00", Font)
        RightToLeftDisplay(sender, e, 280, "NET SALES", "3000.00", Font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 295, "CASH TOTAL", "1", Font)
        RightToLeftDisplay(sender, e, 305, "CREDIT CARD", "3000.00", Font)
        RightToLeftDisplay(sender, e, 315, "DEBIT CARD", "3000.00", Font)
        RightToLeftDisplay(sender, e, 325, "MISC/CHEQUES", "3000.00", Font)
        RightToLeftDisplay(sender, e, 335, "   EXCESS CHK", "3000.00", Font)
        RightToLeftDisplay(sender, e, 345, "IN-HOUSE CHARGE", "3000.00", Font)
        RightToLeftDisplay(sender, e, 355, "   EXCESS GC", "3000.00", Font)
        RightToLeftDisplay(sender, e, 365, "A/R", "3000.00", Font)
        RightToLeftDisplay(sender, e, 375, "OTHERS", "3000.00", Font)
        RightToLeftDisplay(sender, e, 385, "DEPOSIT", "3000.00", Font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 400, "CASH IN DRAWER", "3000.00", Font)
        RightToLeftDisplay(sender, e, 410, "PICK-UP", "3000.00", Font)
        RightToLeftDisplay(sender, e, 420, "RCVD-ON-ACCOUNT", "3000.00", Font)
        RightToLeftDisplay(sender, e, 430, "PAID-OUT", "3000.00", Font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 445, "ITEM VOID/E.C", "1", Font)
        RightToLeftDisplay(sender, e, 455, "TRANS. VOID", "3000.00", Font)
        RightToLeftDisplay(sender, e, 465, "TRANS. CANCEL", "3000.00", Font)
        RightToLeftDisplay(sender, e, 475, "DIPLOMAT", "3000.00", Font)
        RightToLeftDisplay(sender, e, 485, "TOTAL DISCOUNTS", "3000.00", Font)
        RightToLeftDisplay(sender, e, 495, "   SENIOR CITIZEN", "3000.00", Font)
        RightToLeftDisplay(sender, e, 505, "ITEM DISCOUNTS", "3000.00", Font)
        RightToLeftDisplay(sender, e, 515, "S. CHARGE", "3000.00", Font)
        RightToLeftDisplay(sender, e, 525, "CORKAGE", "3000.00", Font)
        RightToLeftDisplay(sender, e, 535, "TOTAL SURCHARGE", "3000.00", Font)
        RightToLeftDisplay(sender, e, 545, "TAKE OUT CHARGE", "3000.00", Font)
        RightToLeftDisplay(sender, e, 555, "DELIVERY CHARGE", "3000.00", Font)
        RightToLeftDisplay(sender, e, 565, "UNCONSUMABLE", "3000.00", Font)
        RightToLeftDisplay(sender, e, 575, "RETURNS EXCHANGE", "3000.00", Font)
        RightToLeftDisplay(sender, e, 585, "RETURNS REFUND", "3000.00", Font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 600, "TOTAL QTY. SOLD", "3000.00", Font)
        RightToLeftDisplay(sender, e, 610, "TRANSACTION COUNT", "3000.00", Font)
        RightToLeftDisplay(sender, e, 620, "TOTAL GUEST", "3000.00", Font)
        RightToLeftDisplay(sender, e, 630, "BEG. OR NO.", "3000.00", Font)
        RightToLeftDisplay(sender, e, 640, "END OR NO.", "3000.00", Font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 655, "CURRENT TOTAL SALES", "3000.00", Font)
        RightToLeftDisplay(sender, e, 665, "OLD GRAND TOTAL", "3000.00", Font)
        RightToLeftDisplay(sender, e, 675, "NEW GRAND TOTAL", "3000.00", Font)
        '============================================================================================================================
        SimpleTextDisplay(sender, e, "----------------------------------------", Font, 0, 665)
        '============================================================================================================================
        CenterTextDisplay(sender, e, Format(Now, "MM/dd/yyyy hh:mm:ss tt"), Font, 700)
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles ButtonZread.Click
        Dim result As Integer = MessageBox.Show("It seems like you have not generated Z-reading before ? Would you like to generate now ?", "Z-Reading", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            XreadOrZread = "Z-READ"
            printdocXread.DefaultPageSettings.PaperSize = New PaperSize("Custom", 200, 800)
            PrintPreviewDialogXread.Document = printdocXread
            PrintPreviewDialogXread.ShowDialog()

            dbconnection()
            sql = "UPDATE loc_settings SET S_Zreading = '" & returndateformat(Now()) & "'"
            cmd = New MySqlCommand(sql, localconn)
            cmd.ExecuteNonQuery()
            localconn.Close()
            cmd.Dispose()
            S_Zreading = returndateformat(Now())
        Else
            MessageBox.Show("This will continue your yesterday's record ...", "Z-Reading", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Class