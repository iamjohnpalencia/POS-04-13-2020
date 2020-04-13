Imports MySql.Data.MySqlClient
Imports System.Threading
Public Class Leaderboards
    Dim thread1 As Thread
    Dim solocloudconn As New MySqlConnection
    Dim hasinternet As Boolean = False
    Dim StopThread As Boolean = False
    Dim BestsellerDataTable As DataTable
    Dim BestsellerDataAdapter As MySqlDataAdapter
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
        CheckForIllegalCrossThreadCalls = False
        'Button1.PerformClick()
    End Sub
    Private Sub ProductList_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If BackgroundWorker1.WorkerSupportsCancellation = True Then
            BackgroundWorker1.CancelAsync()
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            BackgroundWorker1.RunWorkerAsync()
            BackgroundWorker1.WorkerSupportsCancellation = True
            BackgroundWorker1.WorkerReportsProgress = True
            Button1.Enabled = False
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Dim threadList As List(Of Thread) = New List(Of Thread)
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            If CheckForInternetConnection() = True Then
                If BackgroundWorker1.IsBusy = True Then
                    thread1 = New Thread(AddressOf bestseller)
                    thread1.Start()
                    threadList.Add(thread1)
                    For Each t In threadList
                        t.Join()
                    Next
                    If BackgroundWorker1.CancellationPending = True Then
                        e.Cancel = True
                    End If
                End If
            Else
                Button1.Enabled = True
                Label2.Text = "No Internet Connection"
            End If
            'For i = 0 To 100
            '    BackgroundWorker1.ReportProgress(i)
            '    Thread.Sleep(100)
            'Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
        Label2.Text = "Loading Products " & e.ProgressPercentage.ToString() & " %"
    End Sub
    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            If BestsellerDataTable.Rows.Count > 0 Then
                With DataGridViewTOPSOLDS
                    .DataSource = Nothing
                    .DataSource = BestsellerDataTable
                    .RowHeadersVisible = False
                    .Font = New Font("Century Gothic", 10)
                    .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
                    .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
                    .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                End With
                Label2.Text = "Loaded Successfully!"
                Button1.Enabled = True
                Chart1.Series("Series1").Points.Clear()
                Chart2.Series("Series1").Points.Clear()
                For i As Integer = 0 To BestsellerDataTable.Rows.Count - 1 Step +1
                    Chart1.Series("Series1").Points.AddXY(BestsellerDataTable(i)(0), BestsellerDataTable(i)(2))
                    Chart2.Series("Series1").Points.AddXY(BestsellerDataTable(i)(0), BestsellerDataTable(i)(2))
                    Chart1.Series(0)("PieLabelStyle") = "Disabled"
                Next
            Else
                Label2.Text = "Please reload again"
                'ChangeProgBarColor(ProgressBar1, ProgressBarColor.Yellow)
                Button1.Enabled = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub bestseller()
        Try
            If cloudconn.State = ConnectionState.Closed Then
                serverconn()
            End If
            sql = "SELECT product_name, product_sku , SUM(quantity) FROM admin_daily_transaction_details WHERE store_id = " & ClientStoreID & " GROUP by product_name ORDER BY `SUM(quantity)` DESC"
            BestsellerDataAdapter = New MySqlDataAdapter(sql, cloudconn)
            BestsellerDataTable = New DataTable
            BestsellerDataAdapter.Fill(BestsellerDataTable)
            cloudconn.Close()
        Catch ex As Exception
            'MsgBox(ex.ToString)
            MsgBox("Please reload and try again.")
            SystemLogType = "ERROR"
            SystemLogDesc = "ERROR 6.0 Chart Load"
            GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
        End Try
    End Sub
End Class