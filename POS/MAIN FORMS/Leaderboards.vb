Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Windows.Forms.DataVisualization.Charting

Public Class Leaderboards
    Dim thread1 As Thread
    Dim solocloudconn As New MySqlConnection
    Dim hasinternet As Boolean = False
    Dim StopThread As Boolean = False
    Dim BestsellerDataTable As DataTable
    Dim BestsellerDataAdapter As MySqlDataAdapter
    Dim BestSellerCmd As MySqlCommand
    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Enum ProgressBarColor
        Green = &H1
        Red = &H2
        Yellow = &H3
    End Enum
    Private Shared Sub ChangeProgBarColor(ByVal ProgressBar_Name As System.Windows.Forms.ProgressBar, ByVal ProgressBar_Color As ProgressBarColor)
        SendMessage(ProgressBar_Name.Handle, &H410, ProgressBar_Color, 0)
    End Sub
    Private Sub ProductList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TabControl1.TabPages(0).Text = "Sales"
        TabControl1.TabPages(1).Text = "Expenses"
        TabControl1.TabPages(2).Text = "Transfers"
        TabControl1.TabPages(3).Text = "Logs"
        CheckForIllegalCrossThreadCalls = False
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.RunWorkerAsync()

        LoadChart("SELECT DATE_FORMAT(zreading, '%Y-%m-%d') as zreading, SUM(total) FROM loc_daily_transaction_details WHERE DATE(CURRENT_DATE) - INTERVAL 7 DAY GROUP BY zreading DESC LIMIT 7", 0)

    End Sub
    Private Sub ProductList_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If BackgroundWorker1.WorkerSupportsCancellation = True Then
            BackgroundWorker1.CancelAsync()
        End If
    End Sub
    Dim threadList As List(Of Thread) = New List(Of Thread)
    Private Sub loadbestseller()
        Try
            GLOBAL_SELECT_ALL_FUNCTION("loc_daily_transaction_details  GROUP BY product_name ORDER by COUNT(transaction_number) DESC limit 10", "product_name ,  product_category, SUM(quantity) as Qty, price , Sum(total) as totalprice", DatagridviewTOPSELLER)
            With DatagridviewTOPSELLER
                .Columns(0).HeaderCell.Value = "Product Name"
                .Columns(0).Width = 150
                .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopLeft
                .Columns(1).HeaderCell.Value = "Category"
                .Columns(1).Width = 150
                .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopLeft
                .Columns(2).HeaderCell.Value = "Sales Volume"
                .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(3).HeaderCell.Value = "Price"
                .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(4).HeaderCell.Value = "Total Sale"
                .Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(2).Width = 120
                .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(3).Width = 100
                .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(4).Width = 100
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadTransactions()
        Try
            GLOBAL_SELECT_ALL_FUNCTION("loc_daily_transaction ORDER BY transaction_id DESC LIMIT 10 ", "transaction_number  , CONCAT(date,' ', time) AS datetime , crew_id , amountdue, active", DataGridViewRecentSales)
            With DataGridViewRecentSales
                .Columns(0).HeaderCell.Value = "Invoice id"
                .Columns(0).Width = 150
                .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopLeft

                .Columns(1).HeaderCell.Value = "Created At"
                .Columns(1).Width = 150
                .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopLeft

                .Columns(2).HeaderCell.Value = "Crew"
                .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopLeft

                .Columns(3).HeaderCell.Value = "Total Sales"
                .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.TopRight

                .Columns(4).HeaderCell.Value = "Status"
            End With
            For Each row As DataRow In dt.Rows
                row("crew_id") = returnfullname(row("crew_id"))
                If row("active") = 1 Then
                    row("active") = "Active"
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadExpenses()
        Try
            GLOBAL_SELECT_ALL_FUNCTION("loc_expense_list GROUP BY expense_id DESC LIMIT 10", "expense_number  , CONCAT(date,' ', time) AS datetime , crew_id , total_amount, active", DataGridViewRecentExpenses)

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadTransfers()
        Try
            GLOBAL_SELECT_ALL_FUNCTION("loc_system_logs WHERE log_type = 'STOCK TRANSFER' GROUP BY log_date_time DESC LIMIT 10 ", "loc_systemlog_id  , log_date_time , crew_id , log_description", DatagridviewTransfers)

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadLogs()
        Try
            GLOBAL_SELECT_ALL_FUNCTION("loc_system_logs WHERE log_type <> 'STOCK TRANSFER'  GROUP BY log_date_time DESC LIMIT 10", "loc_systemlog_id  , log_date_time , crew_id , log_description", DatagridviewLogs)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadProducts()
        Try
            Chart2.Series("Series1").IsVisibleInLegend = False
            Dim sql = "SELECT product_name , Sum(total) as totalprice FROM loc_daily_transaction_details GROUP BY product_name ORDER by COUNT(transaction_number) DESC limit 10"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
            Dim dr As MySqlDataReader
            dr = cmd.ExecuteReader
            While dr.Read
                Chart2.Series("Series1").Points.AddXY(dr.GetString("product_name"), dr.GetInt64("totalprice"))
            End While
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub LoadChart(sql As String, trueorfalse As Integer)
        Try
            Chart1.Series.Clear()
            Dim Series1 As New DataVisualization.Charting.Series
            With Series1
                .Name = "Series1"
                .ChartType = SeriesChartType.Column
            End With
            Chart1.Series.Add(Series1)
            Chart1.Invalidate()
            Chart1.Series("Series1").IsVisibleInLegend = False
            DataGridView1.Rows.Clear()
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            With DataGridView1
                For Each row As DataRow In dt.Rows
                    If trueorfalse = 0 Then
                        .Rows.Add(row("zreading").ToString, row("SUM(total)"))
                    ElseIf trueorfalse = 1 Then
                        .Rows.Add(row("YEAR(zreading)"), row("SUM(total)"))
                    ElseIf trueorfalse = 2 Then
                        .Rows.Add(row("MONTHNAME(zreading)"), row("SUM(total)"))
                    End If
                Next
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Me.Chart1.Series("Series1").Points.AddXY(.Rows(i).Cells(0).Value.ToString, .Rows(i).Cells(1).Value.ToString)
                Next
            End With
            LocalhostConn.close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub RadioButton3_Click(sender As Object, e As EventArgs) Handles RadioButtonWeek.Click
        LoadChart("SELECT DATE_FORMAT(zreading, '%Y-%m-%d') as zreading, SUM(total) FROM loc_daily_transaction_details WHERE DATE(CURRENT_DATE) - INTERVAL 7 DAY GROUP BY zreading DESC LIMIT 7", 0)
    End Sub
    Private Sub RadioButton2_Click(sender As Object, e As EventArgs) Handles RadioButtonMonth.Click
        LoadChart("SELECT MONTHNAME(zreading) , SUM(total) FROM `loc_daily_transaction_details` WHERE DATE(zreading) - INTERVAL 1 MONTH GROUP BY MONTHNAME(zreading)", 2)
    End Sub
    Private Sub RadioButton4_Click(sender As Object, e As EventArgs) Handles RadioButtonYear.Click
        LoadChart("SELECT YEAR(zreading), SUM(total) FROM `loc_daily_transaction_details` WHERE YEAR(zreading) GROUP BY YEAR(zreading)", 1)
    End Sub
    Private Sub RadioButton5_Click(sender As Object, e As EventArgs) Handles RadioButtonLastYear.Click
        LoadChart("SELECT YEAR(zreading), SUM(total) FROM `loc_daily_transaction_details` WHERE YEAR(zreading) - INTERVAL 1 YEAR GROUP BY YEAR(zreading)", 1)
    End Sub
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            For i = 0 To 100
                Thread.Sleep(30)
                BackgroundWorker1.ReportProgress(i)
                If i = 0 Then
                    thread1 = New Thread(AddressOf LoadProducts)
                    thread1.Start()
                    threadList.Add(thread1)
                    For Each t In threadList
                        t.Join()
                    Next
                    thread1 = New Thread(AddressOf loadbestseller)
                    thread1.Start()
                    threadList.Add(thread1)
                    For Each t In threadList
                        t.Join()
                    Next
                    thread1 = New Thread(AddressOf LoadTransactions)
                    thread1.Start()
                    threadList.Add(thread1)
                    For Each t In threadList
                        t.Join()
                    Next
                    thread1 = New Thread(AddressOf LoadExpenses)
                    thread1.Start()
                    threadList.Add(thread1)
                    For Each t In threadList
                        t.Join()
                    Next
                    thread1 = New Thread(AddressOf LoadTransfers)
                    thread1.Start()
                    threadList.Add(thread1)
                    For Each t In threadList
                        t.Join()
                    Next
                    thread1 = New Thread(AddressOf LoadLogs)
                    thread1.Start()
                    threadList.Add(thread1)
                    For Each t In threadList
                        t.Join()
                    Next
                End If
            Next

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class