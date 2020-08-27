﻿Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Globalization

Public Class SynctoCloud
    Dim totalrow As Integer
    Dim counter As Integer = 0
    Dim dt2 As New DataTable
    Dim nousers As Boolean = False
    Dim StopThread As Boolean = False
    Dim Unsuccessful As Boolean
    Dim CountStart As Boolean
    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Enum ProgressBarColor
        Green = &H1
        Red = &H2
        Yellow = &H3
    End Enum
    Private Shared Sub ChangeProgBarColor(ByVal ProgressBar_Name As System.Windows.Forms.ProgressBar, ByVal ProgressBar_Color As ProgressBarColor)
        SendMessage(ProgressBar_Name.Handle, &H410, ProgressBar_Color, 0)
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        ProgressBar1.Value = 0
    End Sub
    Private Sub SynctoCloud_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try

            If SyncIsOnProcess = True Then
                e.Cancel = True
            Else
                GLOBAL_SYSTEM_LOGS("CLOUD SYNC", "State: Canceled, Time End : " & FullDate24HR() & " Canceled by : " & returnfullname(ClientCrewID))
                e.Cancel = False
            End If
            'If BackgroundWorker1.IsBusy Then
            '    e.Cancel = True
            'Else
            '    e.Cancel = False
            'End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        LabelTime.Text = Val(LabelTime.Text) + 1
        'If CountStart = True Then
        'End If
    End Sub
#Region "FillDatagrid"

    Private Sub filldatagridrefretdetails()
        Try
            Dim fields = "*"
            Dim table = "loc_refund_return_details WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            'GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, datagrid:=DataGridViewRetrefdetails)
            Dim ThisDT = AsDatatable(table, fields, DataGridViewRetrefdetails)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewRetrefdetails.Rows.Add(row("refret_id"), row("transaction_number"), row("crew_id"), row("reason"), row("total"), row("guid"), row("store_id"), row("created_at"), row("zreading"), row("synced"))
            Next
            gettablesize(tablename:="loc_refund_return_details")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    '=====================================================SYSTEMLOGS
    Private Sub filldatagridsystemlog1()
        Try
            'Dim table = "loc_system_logs WHERE synced = 'Unsynced' AND log_type IN ('LOG OUT', 'LOGIN', 'ERROR') AND log_store = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            Dim table = "loc_system_logs WHERE synced = 'Unsynced' AND log_store = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            Dim fields = "*"
            Dim ThisDT = AsDatatable(table, fields, DataGridViewSYSLOG1)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewSYSLOG1.Rows.Add(row("crew_id"), row("log_type"), row("log_description"), row("log_date_time"), row("log_store"), row("guid"), row("loc_systemlog_id"), row("zreading"), row("synced"))
            Next
            gettablesize(tablename:="loc_system_logs")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub filldatagridsystemlog2()
        Try
            Dim fields = "*"
            Dim table = "loc_system_logs WHERE synced = 'Unsynced' AND log_type IN ('MENU FORM', 'STOCK ENTRY', 'STOCK REMOVAL', 'STOCK TRANSFER') AND log_store = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            Dim ThisDT = AsDatatable(table, fields, DataGridViewSYSLOG2)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewSYSLOG2.Rows.Add(row("crew_id"), row("log_type"), row("log_description"), row("log_date_time"), row("log_store"), row("guid"), row("loc_systemlog_id"), row("zreading"), row("synced"))
            Next
            gettablesize(tablename:="loc_system_logs")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridsystemlog3()
        Try
            Dim fields = "*"
            Dim table = "loc_system_logs WHERE synced = 'Unsynced' AND log_type IN ('NEW CUSTOM PRODUCT', 'NEW EXPENSE', 'NEW STOCK ADDED', 'NEW USER') AND log_store = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"

            Dim ThisDT = AsDatatable(table, fields, DataGridViewSYSLOG3)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewSYSLOG3.Rows.Add(row("crew_id"), row("log_type"), row("log_description"), row("log_date_time"), row("log_store"), row("guid"), row("loc_systemlog_id"), row("zreading"), row("synced"))
            Next

            'GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, datagrid:=DataGridViewSYSLOG3)
            gettablesize(tablename:="loc_system_logs")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridsystemlog4()
        Try
            Dim fields = "*"
            Dim table = "loc_system_logs WHERE synced = 'Unsynced' AND log_type IN ('TRANSACTION', 'USER UPDATE') AND log_store = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"

            'GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, datagrid:=DataGridViewSYSLOG4)
            Dim ThisDT = AsDatatable(table, fields, DataGridViewSYSLOG4)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewSYSLOG4.Rows.Add(row("crew_id"), row("log_type"), row("log_description"), row("log_date_time"), row("log_store"), row("guid"), row("loc_systemlog_id"), row("zreading"), row("synced"))
            Next


            gettablesize(tablename:="loc_system_logs")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    '=====================================================SYSTEMLOGS
    Private Sub filldatagridtransaction()
        Try
            Dim fields = "*"
            Dim table = "loc_daily_transaction WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(table, fields, DataGridViewTRAN)
            gettablesize(tablename:="loc_daily_transaction")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    '======================================================TRANSACTION DETAILS
    Private Sub filldatagridtransactiondetails1()
        Try
            Dim fields = "*"
            Dim table = "loc_daily_transaction_details WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            'GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, datagrid:=DataGridViewTRANDET)
            Dim ThisDT = AsDatatable(table, fields, DataGridViewTRANDET)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewTRANDET.Rows.Add(row("details_id"), row("product_id"), row("product_sku"), row("product_name"), row("quantity"), row("price"), row("total"), row("crew_id"), row("transaction_number"), row("active"), row("created_at"), row("guid"), row("store_id"), row("total_cost_of_goods"), row("product_category"), row("zreading"), row("transaction_type"), row("synced"))
            Next
            gettablesize(tablename:="loc_daily_transaction_details")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridinventory()
        Try
            Dim fields = "*"
            Dim table = "loc_pos_inventory WHERE store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "' AND synced = 'Unsynced'"
            'GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, datagrid:=DataGridViewINV)
            Dim ThisDT = AsDatatable(table, fields, DataGridViewINV)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewINV.Rows.Add(row("inventory_id"), row("store_id"), row("formula_id"), row("product_ingredients"), row("sku"), row("stock_primary"), row("stock_secondary"), row("stock_status"), row("critical_limit"), row("guid"), row("created_at"), row("crew_id"), row("synced"), row("server_date_modified"), row("server_inventory_id"))
            Next
            gettablesize(tablename:="loc_pos_inventory")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridexpenses()
        Try
            Dim fields = "*"
            Dim table = "loc_expense_list WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            'GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, datagrid:=DataGridViewEXP)

            Dim ThisDT = AsDatatable(table, fields, DataGridViewEXP)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewEXP.Rows.Add(row("expense_id"), row("crew_id"), row("expense_number"), row("total_amount"), row("paid_amount"), row("unpaid_amount"), row("store_id"), row("guid"), row("created_at").ToString, row("active"), row("zreading"), row("synced"))
            Next

            gettablesize(tablename:="loc_expense_list")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridexpensesdetails()
        Try
            Dim fields = "*"
            Dim table = "loc_expense_details WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            'GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, datagrid:=DataGridViewEXPDET)

            Dim ThisDT = AsDatatable(table, fields, DataGridViewEXPDET)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewEXPDET.Rows.Add(row("expense_id"), row("expense_number"), row("expense_type"), row("item_info"), row("quantity"), row("price"), row("amount"), row("attachment"), row("created_at"), row("crew_id"), row("guid"), row("store_id"), row("active"), row("zreading"), row("synced"))
            Next

            gettablesize(tablename:="loc_expense_details")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridlocusers()
        Try
            Dim fields = "*"
            Dim table = "loc_users WHERE store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "' AND synced = 'Unsynced'"
            'GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, datagrid:=DataGridViewLocusers)

            Dim ThisDT = AsDatatable(table, fields, DataGridViewLocusers)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewLocusers.Rows.Add(row("user_id"), row("user_level"), row("full_name"), row("username"), row("password"), row("contact_number"), row("email"), row("position"), row("gender"), row("created_at"), row("updated_at"), row("active"), row("guid"), row("store_id"), row("uniq_id"), row("synced"))
            Next

            gettablesize(tablename:="loc_users")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridproducts()
        Try
            Dim fields = "*"
            Dim table = "loc_admin_products WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "' AND product_status = 0"
            'GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, datagrid:=DataGridViewCUSTOMPRODUCTS)

            Dim ThisDT = AsDatatable(table, fields, DataGridViewCUSTOMPRODUCTS)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewCUSTOMPRODUCTS.Rows.Add(row("product_id"), row("product_sku"), row("product_name"), row("formula_id"), row("product_barcode"), row("product_category"), row("product_price"), row("product_desc"), row("product_image"), row("product_status"), row("origin"), row("date_modified"), row("guid"), row("store_id"), row("crew_id"), row("synced"), row("server_product_id"))
            Next

            gettablesize(tablename:="loc_daily_transaction")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagriddepositslip()
        Try
            Dim fields = "*"
            Dim table = "loc_deposit WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            'GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, datagrid:=DataGridViewDepositSlip)
            Dim ThisDT = AsDatatable(table, fields, DataGridViewDepositSlip)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewDepositSlip.Rows.Add(row("dep_id"), row("name"), row("crew_id"), row("transaction_number"), row("amount"), row("bank"), row("transaction_date"), row("store_id"), row("guid"), row("created_at"), row("synced"))
            Next
            gettablesize(tablename:="loc_deposit")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridmodeoftransaction()
        Try
            Dim fields = "*"
            Dim table = "loc_transaction_mode_details WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            'GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, datagrid:=DataGridViewMODEOFTRANSACTION)

            Dim ThisDT = AsDatatable(table, fields, DataGridViewMODEOFTRANSACTION)
            For Each row As DataRow In ThisDT.Rows
                DataGridViewMODEOFTRANSACTION.Rows.Add(row("mode_id"), row("transaction_type"), row("transaction_number"), row("fullname"), row("reference"), row("markup"), row("created_at"), row("status"), row("store_id"), row("guid"), row("synced"))
            Next

            gettablesize(tablename:="loc_transaction_mode_details")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub fillpricerequestchange()
        Try
            Dim fields = "*"
            Dim table = "loc_price_request_change WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(table, fields, DataGridViewPriceChangeRequest)
            gettablesize(tablename:="loc_price_request_change")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub countrows(ByVal tablename As String)
        Try
            Dim sql = "SELECT COUNT(*) FROM " & tablename & " "
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn())
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            For Each row As DataRow In dt.Rows
                DataGridView2.Rows.Add(row("COUNT(*)"), tablename)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        Finally
            LocalhostConn.close
            cmd.Dispose()
            da.Dispose()
        End Try
    End Sub
    Private Sub gettablesize(ByVal tablename As String)
        Try
            Dim sql = "SELECT table_name AS `Table`, round(((data_length + index_length) / 1024 / 1024), 2) `Size in MB` FROM information_schema.TABLES WHERE table_schema = 'pos' AND table_name = '" & tablename & "'"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            For Each row As DataRow In dt.Rows
                DataGridView1.Rows.Add(row("Table"), row("Size in MB"))
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        Finally
            LocalhostConn.close
            cmd.Dispose()
            da.Dispose()
        End Try
    End Sub
#End Region
    Private Sub LoadData()
        Try
            Label1.Text = ""
            Label2.Text = ""
            LabelTTLRowtoSync.Text = ""
            Label5.Text = ""
            filldatagridtransaction()
            filldatagridtransactiondetails1()
            filldatagridinventory()
            filldatagridexpenses()
            filldatagridexpensesdetails()
            filldatagridlocusers()
            filldatagridsystemlog1()
            filldatagridsystemlog2()
            filldatagridsystemlog3()
            filldatagridsystemlog4()
            filldatagridrefretdetails()
            filldatagridproducts()
            filldatagridmodeoftransaction()
            filldatagriddepositslip()
            fillpricerequestchange()
            totalrow = SumOfColumnsToInt(DataGridView2, 0)
            LabelTTLRowtoSync.Text = totalrow
            Button1.Enabled = False
            Label2.Text = "Item(s)"
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LoadData()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.Text = "SYNC" Then
            If ValidCloudConnection = True Then
                'Button1.PerformClick()
                GLOBAL_SYSTEM_LOGS("CLOUD SYNC", "Start : " & FullDate24HR() & " Synced by : " & returnfullname(ClientCrewID))
                BackgroundWorker1.WorkerSupportsCancellation = True
                BackgroundWorker1.WorkerReportsProgress = True
                BackgroundWorker1.RunWorkerAsync()
                Timer1.Start()
                SyncIsOnProcess = True
                Button2.Text = "CANCEL SYNC"
            Else
                MsgBox("Cloud connection is not valid.")
            End If
        Else
            Dim msg = MessageBox.Show("Are you sure do you want to cancel sync ?", "Cancel Sync", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If msg = DialogResult.Yes Then
                SyncIsOnProcess = False
                BackgroundWorker1.CancelAsync()
                WorkerCanceled = True
                Label1.Visible = False
                Label2.Visible = False
                LabelTTLRowtoSync.Visible = False
                Label8.Visible = True
                Button2.Enabled = False
                Timer2.Start()
            End If
        End If

    End Sub
    Dim threadListLOCTRAN As List(Of Thread) = New List(Of Thread)
    Dim threadListLOCTD1 As List(Of Thread) = New List(Of Thread)
    Dim threadListLOCTD2 As List(Of Thread) = New List(Of Thread)
    Dim threadListLOCINV As List(Of Thread) = New List(Of Thread)
    Dim threadListLOCEXP As List(Of Thread) = New List(Of Thread)
    Dim threadListLOCEXPD As List(Of Thread) = New List(Of Thread)
    Dim threadListLOCTUSER As List(Of Thread) = New List(Of Thread)
    Dim threadListLOCSYSLOG1 As List(Of Thread) = New List(Of Thread)
    Dim threadListLOCSYSLOG2 As List(Of Thread) = New List(Of Thread)
    Dim threadListLOCSYSLOG3 As List(Of Thread) = New List(Of Thread)
    Dim threadListLOCSYSLOG4 As List(Of Thread) = New List(Of Thread)
    Dim threadListLOCREFRET As List(Of Thread) = New List(Of Thread)
    Dim threadListLOCPRODUCT As List(Of Thread) = New List(Of Thread)
    Dim threadListMODEOFTRANSACTION As List(Of Thread) = New List(Of Thread)
    Dim threadListLocDeposit As List(Of Thread) = New List(Of Thread)
    Dim threadListloadData As List(Of Thread) = New List(Of Thread)
    Dim threadListPRICEREQUEST As List(Of Thread) = New List(Of Thread)
    Dim thread1 As Thread

    Dim WorkerCanceled As Boolean = False
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            If CheckForInternetConnection() = True Then
                'Button1.PerformClick()
                thread1 = New Thread(AddressOf LoadData)
                thread1.Start()
                threadListloadData.Add(thread1)
                For Each t In threadListloadData
                    t.Join()
                Next
                ProgressBar1.Maximum = Val(LabelTTLRowtoSync.Text)
                'POS.ProgressBar1.Maximum = Val(Label7.Text)
                'System Logs
                thread1 = New Thread(AddressOf insertsystemlogs1)
                thread1.Start()
                threadListLOCSYSLOG1.Add(thread1)
                'Transaction
                thread1 = New Thread(AddressOf insertlocaldailytransaction)
                thread1.Start()
                threadListLOCTRAN.Add(thread1)
                thread1 = New Thread(AddressOf inserttransactiondetails1)
                thread1.Start()
                threadListLOCTD1.Add(thread1)
                'Inventory
                thread1 = New Thread(AddressOf insertinventory)
                thread1.Start()
                threadListLOCINV.Add(thread1)
                'Expenses
                thread1 = New Thread(AddressOf insertexpenses)
                thread1.Start()
                threadListLOCEXP.Add(thread1)
                thread1 = New Thread(AddressOf insertexpensedetails)
                thread1.Start()
                threadListLOCEXPD.Add(thread1)
                'New Users
                thread1 = New Thread(AddressOf insertlocalusers)
                thread1.Start()
                threadListLOCTUSER.Add(thread1)
                'Returns
                thread1 = New Thread(AddressOf insertrefretdetails)
                thread1.Start()
                threadListLOCREFRET.Add(thread1)
                'Custom Products
                thread1 = New Thread(AddressOf insertlocproducts)
                thread1.Start()
                threadListLOCPRODUCT.Add(thread1)
                'Mode of transaction details
                thread1 = New Thread(AddressOf insertlocmodeoftransaction)
                thread1.Start()
                threadListMODEOFTRANSACTION.Add(thread1)
                'Deposits
                thread1 = New Thread(AddressOf insertlocdeposit)
                thread1.Start()
                threadListLocDeposit.Add(thread1)
                'Price Request
                thread1 = New Thread(AddressOf insertpricerequest)
                thread1.Start()
                threadListPRICEREQUEST.Add(thread1)
                'thread1 = New Thread(AddressOf insertsystemlogs2)
                'thread1.Start()
                'threadListLOCSYSLOG2.Add(thread1)
                'thread1 = New Thread(AddressOf insertsystemlogs3)
                'thread1.Start()
                'threadListLOCSYSLOG3.Add(thread1)
                'thread1 = New Thread(AddressOf insertsystemlogs4)
                'thread1.Start()
                'threadListLOCSYSLOG4.Add(thread1)
                'Refunds
                For Each t In threadListPRICEREQUEST
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCTRAN
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCTD1
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCTD2
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCINV
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCEXP
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCEXPD
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCTUSER
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCSYSLOG1
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCSYSLOG2
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCSYSLOG3
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCSYSLOG4
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCREFRET
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLOCPRODUCT
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next

                For Each t In threadListMODEOFTRANSACTION
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
                For Each t In threadListLocDeposit
                    t.Join()
                    If (BackgroundWorker1.CancellationPending) Then
                        ' Indicate that the task was canceled.
                        WorkerCanceled = True
                        e.Cancel = True
                        Exit For
                    End If
                Next
            Else
                Unsuccessful = True
            End If


        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            If Unsuccessful = True Then
                ChangeProgBarColor(ProgressBar1, ProgressBarColor.Yellow)
                ProgressBar1.Maximum = Val(LabelTTLRowtoSync.Text)
                'POS.ProgressBar1.Maximum = Val(Label3.Text)
                If LabelRowtoSync.Text <> "0" Then
                    Label1.Text = "Synced " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                Else
                    LabelTTLRowtoSync.Text = "0"
                    Label2.Text = "Item(s)"
                    Label1.Text = "Synced 0 of 0"
                End If
                Label5.Text = "Synchronization Failed"
                MsgBox("An error occured")
                Button1.Enabled = True
                Button2.Enabled = True
                GLOBAL_SYSTEM_LOGS("CLOUD SYNC", "State: Unsuccessful, Time End : " & FullDate24HR() & " Synced by : " & returnfullname(ClientCrewID))
            Else
                If WorkerCanceled = True Then
                    MsgBox("Canceled")
                End If
                If e.Cancelled Then

                    Close()
                Else
                    SyncIsOnProcess = False
                    Timer1.Enabled = False
                    Label1.Text = "Synced " & LabelTTLRowtoSync.Text & " of " & LabelTTLRowtoSync.Text
                    Label5.Text = "Successfully Synchronized "
                    Dim sync = MessageBox.Show("Synchronization Complete", "Sync", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    If sync = DialogResult.OK Then
                        Me.Close()
                        Button1.Enabled = True
                        Button2.Enabled = True
                    End If
                    GLOBAL_SYSTEM_LOGS("CLOUD SYNC", "State: Successful, Time End : " & FullDate24HR() & " Synced by : " & returnfullname(ClientCrewID))
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub insertlocaldailytransaction()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand
            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()
            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()
            LabelDTransac.Text = "Syncing Daily Transaction"
            With DataGridViewTRAN
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If WorkerCanceled = True Then
                        Exit For
                    End If
                    cmd = New MySqlCommand("INSERT INTO `Triggers_admin_daily_transaction`(`loc_transaction_id`, `transaction_number`, `grosssales`, `totaldiscount`, `amounttendered`, `change`, `amountdue`, `vatablesales`, `vatexemptsales`, `zeroratedsales`, `vatpercentage`, `lessvat`, `transaction_type`, `discount_type`, `totaldiscountedamount`, `si_number`, `crew_id`, `guid`, `active`, `store_id`, `created_at`, `shift`, `zreading`)
                                             VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20,@21,@22,@23)", server)

                    cmd.Parameters.Add("@1", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.Decimal).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@7", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@8", MySqlDbType.Decimal).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@9", MySqlDbType.Decimal).Value = .Rows(i).Cells(8).Value.ToString()
                    cmd.Parameters.Add("@10", MySqlDbType.Decimal).Value = .Rows(i).Cells(9).Value.ToString()
                    cmd.Parameters.Add("@11", MySqlDbType.Decimal).Value = .Rows(i).Cells(10).Value.ToString()
                    cmd.Parameters.Add("@12", MySqlDbType.Decimal).Value = .Rows(i).Cells(11).Value.ToString()
                    cmd.Parameters.Add("@13", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                    cmd.Parameters.Add("@14", MySqlDbType.Text).Value = .Rows(i).Cells(13).Value.ToString()
                    cmd.Parameters.Add("@15", MySqlDbType.Decimal).Value = .Rows(i).Cells(14).Value.ToString()
                    cmd.Parameters.Add("@16", MySqlDbType.Int64).Value = .Rows(i).Cells(15).Value.ToString()
                    cmd.Parameters.Add("@17", MySqlDbType.VarChar).Value = .Rows(i).Cells(16).Value.ToString()
                    cmd.Parameters.Add("@18", MySqlDbType.VarChar).Value = .Rows(i).Cells(17).Value.ToString()
                    cmd.Parameters.Add("@19", MySqlDbType.VarChar).Value = .Rows(i).Cells(18).Value.ToString()
                    cmd.Parameters.Add("@20", MySqlDbType.VarChar).Value = .Rows(i).Cells(19).Value.ToString()
                    cmd.Parameters.Add("@21", MySqlDbType.Text).Value = .Rows(i).Cells(20).Value.ToString()
                    cmd.Parameters.Add("@22", MySqlDbType.VarChar).Value = .Rows(i).Cells(21).Value.ToString()
                    cmd.Parameters.Add("@23", MySqlDbType.Text).Value = .Rows(i).Cells(22).Value.ToString()

                    LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
                    LabelDTransacItem.Text = Val(LabelDTransacItem.Text) + 1
                    ProgressBar1.Value = CInt(LabelRowtoSync.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                    cmd.ExecuteNonQuery()
                    '====================================================================
                    table = " loc_daily_transaction "
                    where = " transaction_number = '" & .Rows(i).Cells(1).Value.ToString & "'"
                    fields = "`synced`='Synced' "
                    sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
                    cmdloc = New MySqlCommand(sql, local)
                    cmdloc.ExecuteNonQuery()
                    '====================================================================

                Next
                server.Close()
                local.Close()

                LabelDTransac.Text = "Synced Daily Transaction"
                LabelDTransacTime.Text = LabelTime.Text & " Seconds"
            End With
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub inserttransactiondetails1()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()

            LabelDTransactD.Text = "Syncing Transaction Details"
            With DataGridViewTRANDET
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If WorkerCanceled = True Then
                        Exit For
                    End If
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_daily_transaction_details(`loc_details_id`, `product_id`, `product_sku`, `product_name`, `quantity`, `price`, `total`, `crew_id`
                                            , `transaction_number`, `active`, `created_at`, `guid`, `store_id`, `total_cost_of_goods`, `product_category` , `zreading`, `transaction_type`) 
                    VALUES (@a0, @a1, @a2, @a3, @a4, @a5, @a6, @a7, @a8, @a9, @a10, @a11, @a12, @a13, @a14, @a15, @a16)", server)
                    cmd.Parameters.Add("@a0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@a1", MySqlDbType.Int64).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@a2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@a3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@a4", MySqlDbType.Int64).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@a5", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@a6", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@a7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@a8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                    cmd.Parameters.Add("@a9", MySqlDbType.Int64).Value = .Rows(i).Cells(9).Value.ToString()
                    cmd.Parameters.Add("@a10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                    cmd.Parameters.Add("@a11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                    cmd.Parameters.Add("@a12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                    cmd.Parameters.Add("@a13", MySqlDbType.Decimal).Value = .Rows(i).Cells(13).Value.ToString()
                    cmd.Parameters.Add("@a14", MySqlDbType.VarChar).Value = .Rows(i).Cells(14).Value.ToString()
                    cmd.Parameters.Add("@a15", MySqlDbType.VarChar).Value = .Rows(i).Cells(15).Value.ToString()
                    cmd.Parameters.Add("@a16", MySqlDbType.VarChar).Value = .Rows(i).Cells(16).Value.ToString()
                    '====================================================================
                    LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
                    LabelDTransactDItem.Text = Val(LabelDTransactDItem.Text) + 1
                    ProgressBar1.Value = CInt(LabelRowtoSync.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                    cmd.ExecuteNonQuery()
                    '====================================================================
                    table = " loc_daily_transaction_details "
                    where = " details_id =" & .Rows(i).Cells(0).Value.ToString
                    fields = "`synced`='Synced' "
                    sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
                    cmdloc = New MySqlCommand(sql, local)
                    cmdloc.ExecuteNonQuery()
                    '====================================================================
                Next
                server.Close()
                local.Close()
                LabelDTransactD.Text = "Synced Transaction Details"
                LabelDTransactDTime.Text = LabelTime.Text & " Seconds"
            End With
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub insertinventory()
        Try
            Dim cmdupdateinventory As MySqlCommand
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()

            LabelINV.Text = "Syncing Inventory"
            With DataGridViewINV
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If WorkerCanceled = True Then
                        Exit For
                    End If
                    cmdupdateinventory = New MySqlCommand("UPDATE admin_pos_inventory SET stock_primary = " & .Rows(i).Cells(6).Value & " , stock_secondary = " & .Rows(i).Cells(7).Value & " , stock_no_of_servings = " & .Rows(i).Cells(8).Value & " WHERE store_id =" & ClientStoreID & " AND guid = '" & ClientGuid & "' AND loc_inventory_id = " & .Rows(i).Cells(0).Value, server)
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_pos_inventory( `loc_inventory_id`, `store_id`, `formula_id`, `product_ingredients`, `sku`, `stock_primary`, `stock_secondary`, `stock_no_of_servings`, `stock_status`, `critical_limit`, `guid`, `date`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.Int64).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.Int64).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@7", MySqlDbType.Decimal).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@8", MySqlDbType.Decimal).Value = .Rows(i).Cells(8).Value.ToString()
                    cmd.Parameters.Add("@9", MySqlDbType.Int64).Value = .Rows(i).Cells(8).Value.ToString()
                    cmd.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                    cmd.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                    With cmd
                        LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
                        LabelINVItem.Text = Val(LabelINVItem.Text) + 1
                        ProgressBar1.Value = CInt(LabelRowtoSync.Text)
                        'POS.ProgressBar1.Value = Val(Label7.Text)
                        Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                        .ExecuteNonQuery()
                        With cmdupdateinventory
                            .ExecuteNonQuery()
                        End With
                    End With
                    sql = "UPDATE loc_pos_inventory SET `synced`='Synced' WHERE inventory_id = " & .Rows(i).Cells(0).Value.ToString
                    cmdloc = New MySqlCommand(sql, local)
                    cmdloc.ExecuteNonQuery()
                Next
                server.Close()
                local.Close()
                LabelINV.Text = "Synced Inventories"
                LabelINVTime.Text = LabelTime.Text & " Seconds"
            End With
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub insertexpenses()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()


            LabelEXP.Text = "Syncing Expense List"
            With DataGridViewEXP
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If WorkerCanceled = True Then
                        Exit For
                    End If
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_expense_list(`loc_expense_id`, `crew_id`, `expense_number`, `total_amount`, `paid_amount`, `unpaid_amount`, `store_id`, `guid`, `created_at`, `active`, `zreading`) 
                                             VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11)", server)

                    cmd.Parameters.Add("@1", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()

                    cmd.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()

                    cmd.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()

                    cmd.Parameters.Add("@9", MySqlDbType.Text).Value = .Rows(i).Cells(8).Value.ToString()
                    cmd.Parameters.Add("@10", MySqlDbType.Int64).Value = .Rows(i).Cells(9).Value.ToString()
                    cmd.Parameters.Add("@11", MySqlDbType.Text).Value = .Rows(i).Cells(10).Value.ToString()
                    '====================================================================
                    LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
                    LabelEXPItem.Text = Val(LabelEXPItem.Text) + 1
                    ProgressBar1.Value = CInt(LabelRowtoSync.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                    '====================================================================
                    cmd.ExecuteNonQuery()
                    table = " loc_expense_list "
                    where = " expense_id = '" & .Rows(i).Cells(0).Value.ToString & "'"
                    fields = "`synced`='Synced' "
                    sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
                    cmdloc = New MySqlCommand(sql, local)
                    cmdloc.ExecuteNonQuery()
                    '====================================================================
                Next
                server.Close()
                local.Close()
                LabelEXP.Text = "Synced Expense List"
                LabelEXPTime.Text = LabelTime.Text & " Seconds"
            End With
            'truncatetable(tablename:="loc_expense_list")
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub insertexpensedetails()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()


            LabelEXPD.Text = "Syncing Expense Details"
            With DataGridViewEXPDET
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If WorkerCanceled = True Then
                        Exit For
                    End If
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_expense_details(`loc_details_id`
                                                                            , `expense_number`
                                                                            , `expense_type`
                                                                            , `item_info`
                                                                            , `quantity`
                                                                            , `price`
                                                                            , `amount`
                                                                            , `attachment`
                                                                            , `created_at`
                                                                            , `crew_id`
                                                                            , `guid`
                                                                            , `store_id`
                                                                            , `active`, `zreading`) 
                                             VALUES (@loc_details_id, @expense_number, @expense_type
                                             , @item_info, @quantity, @price, @amount, @attachment, @created_at
                                              , @crew_id, @guid, @store_id, @active, @zreading)", server)
                    cmd.Parameters.Add("@loc_details_id", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@expense_number", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@expense_type", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@item_info", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@quantity", MySqlDbType.Int64).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@price", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@amount", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@attachment", MySqlDbType.Text).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@created_at", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                    cmd.Parameters.Add("@crew_id", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                    cmd.Parameters.Add("@guid", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                    cmd.Parameters.Add("@store_id", MySqlDbType.Int64).Value = .Rows(i).Cells(11).Value.ToString()
                    cmd.Parameters.Add("@active", MySqlDbType.Int64).Value = .Rows(i).Cells(12).Value.ToString()
                    cmd.Parameters.Add("@zreading", MySqlDbType.LongText).Value = .Rows(i).Cells(13).Value.ToString()
                    '====================================================================
                    LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
                    LabelEXPDItem.Text = Val(LabelEXPDItem.Text) + 1
                    ProgressBar1.Value = CInt(LabelRowtoSync.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                    cmd.ExecuteNonQuery()
                    '====================================================================
                    table = " loc_expense_details "
                    fields = "`synced`='Synced' "
                    where = " expense_id = '" & .Rows(i).Cells(0).Value.ToString & "'"
                    sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
                    cmdloc = New MySqlCommand(sql, local)
                    cmdloc.ExecuteNonQuery()
                    '====================================================================
                Next
                server.Close()
                local.Close()
                LabelEXPD.Text = "Synced Expense Details"
                LabelEXPDTime.Text = LabelTime.Text & " Seconds"
            End With
            'truncatetable(tablename:="loc_expense_details")
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub insertlocalusers()
        Try
            Dim cmdupdateinventory As MySqlCommand
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()


            LabelACC.Text = "Syncing Accounts"
            With DataGridViewLocusers
                messageboxappearance = False
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If WorkerCanceled = True Then
                        Exit For
                    End If
                    cmdupdateinventory = New MySqlCommand("UPDATE loc_users SET full_name = '" & .Rows(i).Cells(2).Value & "' , username = '" & .Rows(i).Cells(3).Value & "' , email = '" & .Rows(i).Cells(6).Value & "' , password = '" & .Rows(i).Cells(4).Value & "' , contact_number = '" & .Rows(i).Cells(5).Value & "' WHERE uniq_id = '" & .Rows(i).Cells(14).Value & "' AND guid = '" & ClientGuid & "' AND store_id = '" & ClientStoreID & "'", server)
                    cmd = New MySqlCommand("INSERT INTO Triggers_loc_users (`loc_user_id`, `user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `created_at`, `updated_at`, `active`, `guid`, `store_id`, `uniq_id`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                    cmd.Parameters.Add("@9", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                    cmd.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                    cmd.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                    cmd.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                    cmd.Parameters.Add("@13", MySqlDbType.VarChar).Value = .Rows(i).Cells(13).Value.ToString()
                    cmd.Parameters.Add("@14", MySqlDbType.VarChar).Value = .Rows(i).Cells(14).Value.ToString()
                    '====================================================================
                    LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
                    LabelACCItem.Text = Val(LabelACCItem.Text) + 1
                    ProgressBar1.Value = CInt(LabelRowtoSync.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                    cmd.ExecuteNonQuery()
                    cmdupdateinventory.ExecuteNonQuery()
                    '====================================================================
                    table = " loc_users "
                    where = " user_id = " & .Rows(i).Cells(0).Value.ToString
                    fields = "`synced`='Synced' "
                    sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
                    cmdloc = New MySqlCommand(sql, local)
                    cmdloc.ExecuteNonQuery()
                Next
                server.Close()
                local.Close()
                LabelACC.Text = "Synced Accounts"
                LabelACCTime.Text = LabelTime.Text & " Seconds"
            End With
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub insertsystemlogs1()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()

            LabelSYS1.Text = "Syncing Systemlogs 1"
            With DataGridViewSYSLOG1
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If WorkerCanceled = True Then
                        Exit For
                    End If
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_system_logs(`crew_id`, `log_type`, `log_description`, `log_date_time`, `log_store`, `guid`, `loc_systemlog_id`, `zreading`) 
                    VALUES (@0, @1, @2, @3, @4, @5, @6, @7)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.Text).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@7", MySqlDbType.Text).Value = .Rows(i).Cells(7).Value.ToString()
                    '====================================================================
                    LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
                    LabelSYS1Item.Text = Val(LabelSYS1Item.Text) + 1
                    ProgressBar1.Value = CInt(LabelRowtoSync.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                    cmd.ExecuteNonQuery()
                    '====================================================================
                    table = " loc_system_logs "
                    where = " loc_systemlog_id = '" & .Rows(i).Cells(6).Value.ToString & "'"
                    fields = "`synced`='Synced' "
                    sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
                    cmdloc = New MySqlCommand(sql, local)
                    cmdloc.ExecuteNonQuery()
                    '====================================================================
                Next
                server.Close()
                local.Close()
                LabelSYS1.Text = "Synced Systemlogs 1"
                LabelSYS1Time.Text = LabelTime.Text & " Seconds"
            End With
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    'Private Sub insertsystemlogs2()
    '    Try
    '        Dim cmd As MySqlCommand
    '        Dim cmdloc As MySqlCommand

    '        Dim server As MySqlConnection = New MySqlConnection
    '        server.ConnectionString = CloudConnectionString
    '        server.Open()

    '        Dim local As MySqlConnection = New MySqlConnection
    '        local.ConnectionString = LocalConnectionString
    '        local.Open()

    '        LabelSYS2.Text = "Syncing Systemlogs 2"
    '        With DataGridViewSYSLOG2
    '            For i As Integer = 0 To .Rows.Count - 1 Step +1
    '                If WorkerCanceled = True Then
    '                    Exit For
    '                End If
    '                cmd = New MySqlCommand("INSERT INTO Triggers_admin_system_logs(`crew_id`, `log_type`, `log_description`, `log_date_time`, `log_store`, `guid`, `loc_systemlog_id`, `zreading`) 
    '                VALUES (@0, @1, @2, @3, @4, @5, @6, @7)", server)
    '                cmd.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(0).Value.ToString()
    '                cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
    '                cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
    '                cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
    '                cmd.Parameters.Add("@4", MySqlDbType.Text).Value = .Rows(i).Cells(4).Value.ToString()
    '                cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
    '                cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
    '                cmd.Parameters.Add("@7", MySqlDbType.Text).Value = .Rows(i).Cells(7).Value.ToString()
    '                '====================================================================
    '                LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
    '                LabelSYS2Item.Text = Val(LabelSYS2Item.Text) + 1
    '                ProgressBar1.Value = CInt(LabelRowtoSync.Text)
    '                'POS.ProgressBar1.Value = Val(Label7.Text)
    '                Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
    '                cmd.ExecuteNonQuery()
    '                '====================================================================
    '                table = " loc_system_logs "
    '                where = " loc_systemlog_id = '" & .Rows(i).Cells(6).Value.ToString & "'"
    '                fields = "`synced`='Synced' "
    '                sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
    '                cmdloc = New MySqlCommand(sql, local)
    '                cmdloc.ExecuteNonQuery()
    '                '====================================================================
    '            Next
    '            server.Close()
    '            local.Close()
    '            LabelSYS2.Text = "Synced Systemlogs 2"
    '            LabelSYS2Time.Text = LabelTime.Text & " Seconds"
    '        End With
    '    Catch ex As Exception
    '        Unsuccessful = True
    '        BackgroundWorker1.CancelAsync()
    '        MsgBox(ex.ToString)
    '        SendErrorReport(ex.ToString)
    '    End Try
    'End Sub
    'Private Sub insertsystemlogs3()
    '    Try
    '        Dim cmd As MySqlCommand
    '        Dim cmdloc As MySqlCommand

    '        Dim server As MySqlConnection = New MySqlConnection
    '        server.ConnectionString = CloudConnectionString
    '        server.Open()

    '        Dim local As MySqlConnection = New MySqlConnection
    '        local.ConnectionString = LocalConnectionString
    '        local.Open()

    '        LabelSYS3.Text = "Syncing Systemlogs 3"
    '        With DataGridViewSYSLOG3
    '            For i As Integer = 0 To .Rows.Count - 1 Step +1
    '                If WorkerCanceled = True Then
    '                    Exit For
    '                End If
    '                cmd = New MySqlCommand("INSERT INTO Triggers_admin_system_logs(`crew_id`, `log_type`, `log_description`, `log_date_time`, `log_store`, `guid`, `loc_systemlog_id`, `zreading`) 
    '                VALUES (@0, @1, @2, @3, @4, @5, @6, @7)", server)
    '                cmd.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(0).Value.ToString()
    '                cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
    '                cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
    '                cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
    '                cmd.Parameters.Add("@4", MySqlDbType.Text).Value = .Rows(i).Cells(4).Value.ToString()
    '                cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
    '                cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
    '                cmd.Parameters.Add("@7", MySqlDbType.Text).Value = .Rows(i).Cells(7).Value.ToString()
    '                '====================================================================
    '                LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
    '                LabelSYS3Item.Text = Val(LabelSYS3Item.Text) + 1
    '                ProgressBar1.Value = CInt(LabelRowtoSync.Text)
    '                'POS.ProgressBar1.Value = Val(Label7.Text)
    '                Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
    '                cmd.ExecuteNonQuery()
    '                '====================================================================
    '                table = " loc_system_logs "
    '                where = " loc_systemlog_id = '" & .Rows(i).Cells(6).Value.ToString & "'"
    '                fields = "`synced`='Synced' "
    '                sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
    '                cmdloc = New MySqlCommand(sql, local)
    '                cmdloc.ExecuteNonQuery()
    '                '====================================================================
    '            Next
    '            server.Close()
    '            local.Close()
    '            LabelSYS3.Text = "Synced Systemlogs 3"
    '            LabelSYS3Time.Text = LabelTime.Text & " Seconds"
    '        End With
    '    Catch ex As Exception
    '        Unsuccessful = True
    '        BackgroundWorker1.CancelAsync()
    '        MsgBox(ex.ToString)
    '        SendErrorReport(ex.ToString)
    '    End Try
    'End Sub
    'Private Sub insertsystemlogs4()
    '    Try
    '        Dim cmd As MySqlCommand
    '        Dim cmdloc As MySqlCommand

    '        Dim server As MySqlConnection = New MySqlConnection
    '        server.ConnectionString = CloudConnectionString
    '        server.Open()

    '        Dim local As MySqlConnection = New MySqlConnection
    '        local.ConnectionString = LocalConnectionString
    '        local.Open()

    '        LabelSYS4.Text = "Syncing Systemlogs 4"
    '        With DataGridViewSYSLOG4
    '            For i As Integer = 0 To .Rows.Count - 1 Step +1
    '                If WorkerCanceled = True Then
    '                    Exit For
    '                End If
    '                cmd = New MySqlCommand("INSERT INTO Triggers_admin_system_logs(`crew_id`, `log_type`, `log_description`, `log_date_time`, `log_store`, `guid`, `loc_systemlog_id`, `zreading`) 
    '                VALUES (@0, @1, @2, @3, @4, @5, @6, @7)", server)
    '                cmd.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(0).Value.ToString()
    '                cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
    '                cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
    '                cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
    '                cmd.Parameters.Add("@4", MySqlDbType.Text).Value = .Rows(i).Cells(4).Value.ToString()
    '                cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
    '                cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
    '                cmd.Parameters.Add("@7", MySqlDbType.Text).Value = .Rows(i).Cells(7).Value.ToString()
    '                '====================================================================
    '                LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
    '                LabelSYS4Item.Text = Val(LabelSYS4Item.Text) + 1
    '                ProgressBar1.Value = CInt(LabelRowtoSync.Text)
    '                'POS.ProgressBar1.Value = Val(Label7.Text)
    '                Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
    '                cmd.ExecuteNonQuery()
    '                '====================================================================
    '                table = " loc_system_logs "
    '                where = " loc_systemlog_id = '" & .Rows(i).Cells(6).Value.ToString & "'"
    '                fields = "`synced`='Synced' "
    '                sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
    '                cmdloc = New MySqlCommand(sql, local)
    '                cmdloc.ExecuteNonQuery()
    '                '====================================================================
    '            Next
    '            server.Close()
    '            local.Close()
    '            LabelSYS4.Text = "Synced Systemlogs 4"
    '            LabelSYS4Time.Text = LabelTime.Text & " Seconds"
    '        End With
    '    Catch ex As Exception
    '        Unsuccessful = True
    '        BackgroundWorker1.CancelAsync()
    '        MsgBox(ex.ToString)
    '        SendErrorReport(ex.ToString)
    '    End Try
    'End Sub
    Private Sub insertrefretdetails()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()

            LabelRET.Text = "Syncing Refund Details"
            With DataGridViewRetrefdetails
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If WorkerCanceled = True Then
                        Exit For
                    End If
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_refund_return_details( `loc_refret_id`, `transaction_number`, `crew_id`, `reason`, `total`, `guid`, `store_id`, `datereturned`, `zreading`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.Text).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.Int64).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@8", MySqlDbType.LongText).Value = .Rows(i).Cells(8).Value.ToString()
                    '====================================================================
                    LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
                    LabelRETItem.Text = Val(LabelRETItem.Text) + 1
                    ProgressBar1.Value = CInt(LabelRowtoSync.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                    '====================================================================
                    cmd.ExecuteNonQuery()
                    table = " loc_refund_return_details "
                    where = " refret_id = '" & .Rows(i).Cells(0).Value.ToString & "'"
                    fields = "`synced`='Synced' "
                    sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
                    cmdloc = New MySqlCommand(sql, local)
                    cmdloc.ExecuteNonQuery()
                    '====================================================================
                Next
                server.Close()
                local.Close()
                LabelRET.Text = "Synced Refund Details"
                LabelRETTime.Text = LabelTime.Text & " Seconds"
            End With
            'truncatetable(tablename:="loc_expense_list")
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub insertlocproducts()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()

            LabelCPROD.Text = "Syncing Local Products"
            With DataGridViewCUSTOMPRODUCTS
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If WorkerCanceled = True Then
                        Exit For
                    End If
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_products( `loc_product_id`, `product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `guid`, `store_id`, `crew_id`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.Int64).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@8", MySqlDbType.LongText).Value = .Rows(i).Cells(8).Value.ToString()
                    cmd.Parameters.Add("@9", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                    cmd.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                    cmd.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                    cmd.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                    cmd.Parameters.Add("@13", MySqlDbType.Int64).Value = .Rows(i).Cells(13).Value.ToString()
                    cmd.Parameters.Add("@14", MySqlDbType.VarChar).Value = .Rows(i).Cells(14).Value.ToString()
                    '====================================================================
                    LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
                    LabelCPRODItem.Text = Val(LabelCPRODItem.Text) + 1
                    ProgressBar1.Value = CInt(LabelRowtoSync.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                    '====================================================================
                    cmd.ExecuteNonQuery()
                    table = " loc_admin_products "
                    where = " product_id = '" & .Rows(i).Cells(0).Value.ToString & "'"
                    fields = " `synced`='Synced' "
                    sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
                    cmdloc = New MySqlCommand(sql, local)
                    cmdloc.ExecuteNonQuery()
                    '====================================================================
                Next
                server.Close()
                local.Close()
                LabelCPROD.Text = "Synced Local Products"
                LabelCPRODTime.Text = LabelTime.Text & " Seconds"
            End With
            'truncatetable(tablename:="loc_expense_list")
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub insertlocmodeoftransaction()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()

            LabelMODET.Text = "Syncing Mode of Transaction"
            With DataGridViewMODEOFTRANSACTION
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If WorkerCanceled = True Then
                        Exit For
                    End If
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_transaction_mode_details(`loc_mode_id`, `transaction_type`, `transaction_number`, `full_name`, `reference`, `markup`, `created_at`, `status`, `store_id`, `guid`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()

                    cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@7", MySqlDbType.Int64).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                    cmd.Parameters.Add("@9", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                    '====================================================================
                    LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
                    LabelMODETItem.Text = Val(LabelMODETItem.Text) + 1
                    ProgressBar1.Value = CInt(LabelRowtoSync.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                    '====================================================================
                    cmd.ExecuteNonQuery()
                    table = " loc_transaction_mode_details "
                    where = " mode_id = '" & .Rows(i).Cells(0).Value.ToString & "'"
                    fields = " `synced`='Synced' "
                    sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
                    cmdloc = New MySqlCommand(sql, local)
                    cmdloc.ExecuteNonQuery()
                    '====================================================================
                Next
                server.Close()
                local.Close()
                LabelMODET.Text = "Synced Mode of Transaction"
                LabelMODETTime.Text = LabelTime.Text & " Seconds"
            End With
            'truncatetable(tablename:="loc_expense_list")
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub insertlocdeposit()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()

            LabelDEPOSIT.Text = "Syncing Deposit Details"
            With DataGridViewDepositSlip
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If WorkerCanceled = True Then
                        Exit For
                    End If
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_deposit_slip_details( `loc_dep_id`, `name`, `crew_id`, `transaction_number`, `amount`, `bank`, `transaction_date`, `store_id`, `guid`, `created_at`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()

                    cmd.Parameters.Add("@9", MySqlDbType.Timestamp).Value = .Rows(i).Cells(9).Value.ToString()
                    '====================================================================
                    LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
                    LabelDEPOSITItem.Text = Val(LabelDEPOSITItem.Text) + 1
                    ProgressBar1.Value = CInt(LabelRowtoSync.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                    '====================================================================
                    cmd.ExecuteNonQuery()
                    table = " loc_deposit "
                    where = " dep_id = '" & .Rows(i).Cells(0).Value.ToString & "'"
                    fields = " `synced`='Synced' "
                    sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
                    cmdloc = New MySqlCommand(sql, local)
                    cmdloc.ExecuteNonQuery()
                    '====================================================================
                Next
                server.Close()
                local.Close()
                LabelDEPOSIT.Text = "Synced  Deposit Details"
                LabelDEPOSITTime.Text = LabelTime.Text & " Seconds"
            End With
            'truncatetable(tablename:="loc_expense_list")
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub insertpricerequest()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()

            LabelPRICEREQ.Text = "Syncing Price Request"
            With DataGridViewPriceChangeRequest
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    If WorkerCanceled = True Then
                        Exit For
                    End If
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_price_request(`loc_request_id`, `server_product_id`, `request_price`, `created_at`, `active`, `store_id`, `crew_id`, `guid`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7)", server)

                    cmd.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.Text).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.Text).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.Text).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.Text).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.Text).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.Text).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@7", MySqlDbType.Text).Value = .Rows(i).Cells(7).Value.ToString()

                    '====================================================================
                    LabelRowtoSync.Text = Val(LabelRowtoSync.Text + 1)
                    LabelPRICEREQItem.Text = Val(LabelPRICEREQItem.Text) + 1
                    ProgressBar1.Value = CInt(LabelRowtoSync.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & LabelRowtoSync.Text & " of " & LabelTTLRowtoSync.Text & " "
                    '====================================================================
                    cmd.ExecuteNonQuery()
                    table = " loc_price_request_change "
                    where = " request_id = " & .Rows(i).Cells(0).Value.ToString & ""
                    fields = " `synced`='Synced' "
                    sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
                    cmdloc = New MySqlCommand(sql, local)
                    cmdloc.ExecuteNonQuery()
                    '====================================================================
                Next
                server.Close()
                local.Close()
            End With
            LabelPRICEREQ.Text = "Synced Price Request Change"
            LabelPRICEREQTime.Text = LabelTime.Text & " Seconds"
            'truncatetable(tablename:="loc_expense_list")
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If Label8.Text = "Cancelling Sync." Then
            Label8.Text = "Cancelling Sync.."
        ElseIf Label8.Text = "Cancelling Sync.." Then
            Label8.Text = "Cancelling Sync..."
        ElseIf Label8.Text = "Cancelling Sync..." Then
            Label8.Text = "Cancelling Sync...."
        ElseIf Label8.Text = "Cancelling Sync...." Then
            Label8.Text = "Cancelling Sync."
        End If
    End Sub
End Class