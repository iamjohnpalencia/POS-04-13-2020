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
    Dim ReadingOR
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
        If ClientRole = "Head Crew" Then
            Button6.Visible = True
        Else
            Button6.Visible = False
        End If
        If S_Zreading = Format(Now(), "yyyy-MM-dd") Then
            ButtonZread.Enabled = False
            Button6.Enabled = False
        End If
    End Sub
    Public Sub reportssystemlogs(ByVal searchdate As Boolean)
        Try
            table = "`loc_system_logs`"
            fields = "`log_type`, `log_description`, `log_date_time`"
            If searchdate = False Then
                where = " date(log_date_time) = CURRENT_DATE() AND log_type <> 'TRANSACTION' AND log_store = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewSysLog, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " log_type <> 'TRANSACTION' AND log_store = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' AND date(log_date_time) >= '" & Format(DateTimePicker9.Value, "yyyy-MM-dd") & "' AND date(log_date_time) <= '" & Format(DateTimePicker10.Value, "yyyy-MM-dd") & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewSysLog, errormessage:="", fields:=fields, successmessage:="", where:=where)
            End If
            With DataGridViewSysLog
                .Columns(0).HeaderText = "Type"
                .Columns(1).HeaderText = "Description"
                .Columns(2).HeaderText = "Date and Time"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
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
        End Try
    End Sub
    Public Sub reportsdailytransaction(ByVal searchdate As Boolean)
        Try
            table = "`loc_daily_transaction`"
            fields = "`transaction_id`,`created_at`,`transaction_number`,`crew_id`,`amounttendered`,`moneychange`,`active`,`discount`,`amountdue`,`vat_exempt`,`transaction_type`,`vat`,`si_number`"
            If searchdate = False Then
                where = " zreading = CURRENT_DATE() AND active IN(1,3) AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewDaily, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " zreading >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' and zreading <= '" & Format(DateTimePicker2.Value, "yyyy-MM-dd") & "' AND active IN(1,3) AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewDaily, errormessage:="", fields:=fields, successmessage:="", where:=where)
            End If
            With DataGridViewDaily
                .Columns(0).Visible = False
                .Columns(1).HeaderCell.Value = "Date and Time"
                .Columns(2).HeaderCell.Value = "Ref. #"
                .Columns(2).Width = 100
                .Columns(3).HeaderCell.Value = "Crew"
                .Columns(4).HeaderCell.Value = "Cash"
                .Columns(5).HeaderCell.Value = "Change"
                .Columns(6).Visible = False
                .Columns(7).HeaderCell.Value = "Discount"
                .Columns(8).HeaderCell.Value = "Amt. due"
                .Columns(9).HeaderCell.Value = "Vat Exempt"
                .Columns(10).HeaderCell.Value = "TRN. Type"
                .Columns(11).Visible = False
                .Columns(12).Visible = False
                .Columns.Item(4).DefaultCellStyle.Format = "n2"
                .Columns.Item(5).DefaultCellStyle.Format = "n2"
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
                where = " zreading >= '" & Format(DateTimePicker3.Value, "yyyy-MM-dd") & "' AND zreading <= '" & Format(DateTimePicker4.Value, "yyyy-MM-dd") & "' AND active = 1  AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
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
            fields = "`expense_type`, `item_info`, `quantity`, `price`, `amount`, `attachment`, `created_at`"
            If searchdate = False Then
                where = " zreading = date(CURRENT_DATE()) AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewExpenseDetails, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " zreading >= '" & Format(DateTimePicker5.Value, "yyyy-MM-dd") & "' AND zreading <= '" & Format(DateTimePicker6.Value, "yyyy-MM-dd") & "' AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "'"
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
                Label15.Text = SumOfColumnsToDecimal(DataGridViewExpenseDetails, 4)
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
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
        End Try
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
            e.Graphics.DrawString("Date & Time: " & DataGridViewDaily.SelectedRows(0).Cells(1).Value.ToString & " " & DataGridViewDaily.SelectedRows(0).Cells(2).Value.ToString, font, Brushes.Black, New PointF(shopnameX + 0, shopnameY + a + 120))
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
        ReadingOR = "X" & Format(Now, "yyddMMHHmmssyy")
        printdocXread.DefaultPageSettings.PaperSize = New PaperSize("Custom", 200, 800)
        PrintPreviewDialogXread.Document = printdocXread
        PrintPreviewDialogXread.ShowDialog()
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printdocXread.PrintPage
        Dim ZreadDateFormat = S_Zreading
        Dim font As New Font("Bahnschrift Light SemiCondensed", 7)
        Dim brandfont As New Font("Bahnschrift Condensed", 9)
        Dim GrossSale = sum("total", "loc_daily_transaction_details WHERE zreading = '" & ZreadDateFormat & "' ")
        Dim LessVat = sum("vat", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "' ")
        Dim TotalDiscount = sum("discount", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "' ")
        Dim begORNm = returnselect("transaction_number", "`loc_daily_transaction` WHERE date(zreading) = CURRENT_DATE Limit 1")
        Dim EndORNumber = Format(Now, "yyddMMHHmmssyy")
        Dim DailySales = GrossSale - LessVat - TotalDiscount
        Dim ReturnsTotal = sum("total", "loc_daily_transaction_details WHERE active = 2 AND zreading = '" & ZreadDateFormat & "' ")
        Dim ReturnsExchange = sum("quantity", "loc_daily_transaction_details WHERE active = 2 AND zreading = '" & ZreadDateFormat & "' ")
        Dim OLDgrandtotal = sum("total", "loc_daily_transaction_details WHERE zreading <> '" & ZreadDateFormat & "' ")
        Dim NEWgrandtotal = sum("total", "loc_daily_transaction_details") - ReturnsTotal
        Dim TotalGuest = count("transaction_id", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "' ")
        Dim TotalQuantity = sum("quantity", "loc_daily_transaction_details WHERE zreading = '" & ZreadDateFormat & "' ") - ReturnsExchange

        Dim SrDiscount = sum("discount", "loc_daily_transaction WHERE discount_type = 'Percentage' AND zreading = '" & ZreadDateFormat & "' ")
        Dim totalExpenses = sum("amount", "loc_expense_details WHERE zreading = '" & ZreadDateFormat & "'")
        Dim CashInDrawer = sum("CAST(log_description AS DECIMAL(10,2))", "loc_system_logs WHERE log_type IN ('BG-1','BG-2','BG-3','BG-4') AND zreading = '" & ZreadDateFormat & "' ORDER by log_date_time DESC LIMIT 1") + Val(NEWgrandtotal) - Val(totalExpenses)
        Dim VatExempt = sum("vat_exempt", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "'")
        Dim zeroratedsales = sum("zero_rated", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "'")
        Dim vatablesales = sum("vatable", "loc_daily_transaction WHERE zreading = '" & ZreadDateFormat & "'")
        Dim DepositSlip = sum("amount", "loc_deposit WHERE date(transaction_date) = '" & ZreadDateFormat & "' ")
        'Dim BegBalance = sum("CAST(log_description AS DECIMAL(10,2))", "loc_system_logs WHERE log_type IN ('BG-1','BG-2','BG-3','BG-4') AND zreading = '" & ZreadDateFormat & "' ORDER by log_date_time DESC LIMIT 1")
        Dim CashTotal = CashInDrawer
        'Select Case sum(CAST(log_description As Decimal(10, 2))) As CashierBal FROM `loc_system_logs` WHERE log_type In ('BG-1','BG-2','BG-3','BG-4')
        Dim NetSales = GrossSale - LessVat - TotalDiscount
        CenterTextDisplay(sender, e, ClientBrand.ToUpper, brandfont, 10)
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
        RightToLeftDisplay(sender, e, 100, "TERMINAL REPORT", XreadOrZread, font)
        '============================================================================================================================
        SimpleTextDisplay(sender, e, ReadingOR, font, 0, 90)
        SimpleTextDisplay(sender, e, "----------------------------------------", font, 0, 95)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 123, "DESCRIPTION", "QTY/AMOUNT", font)
        '============================================================================================================================
        SimpleTextDisplay(sender, e, "----------------------------------------", font, 0, 110)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 140, "TERMINAL N0.", S_Terminal_No, font)
        RightToLeftDisplay(sender, e, 155, "GROSS", GrossSale, font)
        RightToLeftDisplay(sender, e, 165, "LESS VAT (VE)", LessVat & "-", font)
        RightToLeftDisplay(sender, e, 175, "LESS VAT DIPLOMAT", "IDK", font)
        RightToLeftDisplay(sender, e, 185, "LESS VAT (OTHER)", "IDK", font)
        RightToLeftDisplay(sender, e, 195, "ADD VAT", "IDK", font)
        RightToLeftDisplay(sender, e, 205, "DAILY SALES", DailySales, font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 220, "VAT AMOUNT", "IDK", font)
        RightToLeftDisplay(sender, e, 230, "LOCAL GOV'T TAX", "IDK", font)
        RightToLeftDisplay(sender, e, 240, "VATABLE SALES", vatablesales, font)
        RightToLeftDisplay(sender, e, 250, "ZERO RATED SALES", zeroratedsales, font)
        RightToLeftDisplay(sender, e, 260, "VAT EXEMPT SALES", VatExempt, font)
        RightToLeftDisplay(sender, e, 270, "LESS DISC (VE)", TotalDiscount, font)
        RightToLeftDisplay(sender, e, 280, "NET SALES", NetSales, font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 295, "CASH TOTAL", CashTotal, font)
        RightToLeftDisplay(sender, e, 305, "CREDIT CARD", "N/A", font)
        RightToLeftDisplay(sender, e, 315, "DEBIT CARD", "N/A", font)
        RightToLeftDisplay(sender, e, 325, "MISC/CHEQUES", "N/A", font)
        RightToLeftDisplay(sender, e, 335, "GIFT CARD(GC)", "N/A", font)
        RightToLeftDisplay(sender, e, 345, "A/R", "N/A", font)
        RightToLeftDisplay(sender, e, 355, "TOTAL EXPENSES", totalExpenses, font)
        RightToLeftDisplay(sender, e, 365, "OTHERS", "N/A", font)
        RightToLeftDisplay(sender, e, 375, "DEPOSIT", DepositSlip, font)
        RightToLeftDisplay(sender, e, 385, "CASH IN DRAWER", CashInDrawer, font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 400, "ITEM VOID E/C", ReturnsExchange, font)
        RightToLeftDisplay(sender, e, 410, "TRANSACTION VOID", ReturnsExchange, font)
        RightToLeftDisplay(sender, e, 420, "TRANSACTION CANCEL", ReturnsExchange, font)
        RightToLeftDisplay(sender, e, 430, "DIMPLOMAT", "N/A", font)
        RightToLeftDisplay(sender, e, 440, "TOTAL DISCOUNTS", TotalDiscount, font)
        RightToLeftDisplay(sender, e, 450, " - SENIOR CITIZEN", SrDiscount, font)
        RightToLeftDisplay(sender, e, 460, "TAKE OUT CHARGE", "N/A", font)
        RightToLeftDisplay(sender, e, 470, "DELIVERY CHARGE", "N/A", font)
        RightToLeftDisplay(sender, e, 480, "RETURNS EXCHANGE", ReturnsExchange, font)
        RightToLeftDisplay(sender, e, 490, "RETURNS REFUND", ReturnsTotal, font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 505, "TOTAL QTY SOLD", TotalQuantity, font)
        RightToLeftDisplay(sender, e, 515, "TOTAL TRANS. COUNT", TotalGuest, font)
        RightToLeftDisplay(sender, e, 525, "TOTAL GUEST", TotalGuest, font)
        RightToLeftDisplay(sender, e, 535, "BEGINNING OR NO.", begORNm, font)
        RightToLeftDisplay(sender, e, 545, "END OR NO.", EndORNumber, font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 560, "CURRENT TOTAL SALES", DailySales, font)
        RightToLeftDisplay(sender, e, 570, "OLD GRAND TOTAL", OLDgrandtotal, font)
        RightToLeftDisplay(sender, e, 580, "NEW GRAND TOTAL", NEWgrandtotal, font)
        'RightToLeftDisplay(sender, e, 575, "RETURNS EXCHANGE", "3000.00", Font)
        'RightToLeftDisplay(sender, e, 585, "RETURNS REFUND", "3000.00", Font)
        'RightToLeftDisplay(sender, e, 600, "TOTAL QTY. SOLD", "3000.00", Font)
        'RightToLeftDisplay(sender, e, 610, "TRANSACTION COUNT", "3000.00", Font)
        'RightToLeftDisplay(sender, e, 620, "TOTAL GUEST", "3000.00", Font)
        'RightToLeftDisplay(sender, e, 630, "BEG. OR NO.", "3000.00", Font)
        'RightToLeftDisplay(sender, e, 640, "END OR NO.", "3000.00", Font)
        '============================================================================================================================
        'RightToLeftDisplay(sender, e, 655, "CURRENT TOTAL SALES", "3000.00", Font)
        'RightToLeftDisplay(sender, e, 665, "OLD GRAND TOTAL", "3000.00", Font)
        'RightToLeftDisplay(sender, e, 675, "NEW GRAND TOTAL", "3000.00", Font)
        '============================================================================================================================
        SimpleTextDisplay(sender, e, "----------------------------------------", font, 0, 590)
        MsgBox(S_Zreading)
        '============================================================================================================================
        CenterTextDisplay(sender, e, S_Zreading & " " & Format(Now(), "HH:mm:ss"), font, 595)
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles ButtonZread.Click
        Try
            Dim result As Integer = MessageBox.Show("It seems like you have not generated Z-reading before ? Would you like to generate now ?", "Z-Reading", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                XreadOrZread = "Z-READ"
                ReadingOR = "Z" & Format(Now, "yyddMMHHmmssyy")
                printdocXread.DefaultPageSettings.PaperSize = New PaperSize("Custom", 200, 800)
                PrintPreviewDialogXread.Document = printdocXread
                PrintPreviewDialogXread.ShowDialog()
                GLOBAL_SYSTEM_LOGS("Z-READ", ClientCrewID & " : " & S_Zreading)
                S_Zreading = Format(DateAdd("d", 1, S_Zreading), "yyyy-MM-dd")
                sql = "UPDATE loc_settings SET S_Zreading = '" & S_Zreading & "'"
                cmd = New MySqlCommand(sql, LocalhostConn())
                cmd.ExecuteNonQuery()
                cmd.Dispose()
                If S_Zreading = Format(Now(), "yyyy-MM-dd") Then
                    ButtonZread.Enabled = False
                    Button6.Enabled = False
                End If
            Else
                MessageBox.Show("This will continue your yesterday's record ...", "Z-Reading", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button6_Click_1(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            Dim result As Integer = MessageBox.Show("It seems like you have not generated Z-reading before ? Would you like to generate now ?", "Z-Reading", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                XreadOrZread = "Z-READ"
                ReadingOR = "Z" & Format(Now, "yyddMMHHmmssyy")
                printdocXread.DefaultPageSettings.PaperSize = New PaperSize("Custom", 200, 800)
                PrintPreviewDialogXread.Document = printdocXread
                PrintPreviewDialogXread.ShowDialog()
                Dim datenow = Format(Now(), "yyyy-MM-dd")
                GLOBAL_SYSTEM_LOGS("Z-READ", ClientCrewID & " : " & datenow)
                sql = "UPDATE loc_settings SET S_Zreading = '" & datenow & "'"
                cmd = New MySqlCommand(sql, LocalhostConn())
                cmd.ExecuteNonQuery()
                cmd.Dispose()
                S_Zreading = Format(Now(), "yyyy-MM-dd")
                ButtonZread.Enabled = False
            Else
                MessageBox.Show("This will continue your yesterday's record ...", "Z-Reading", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class