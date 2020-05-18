Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports System.Threading
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

    End Sub
    Private Sub SynctoCloud_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If BackgroundWorker1.IsBusy Then
                e.Cancel = True
            Else
                e.Cancel = False
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label6.Text = Val(Label6.Text) + 1
        'If CountStart = True Then

        'End If
    End Sub
    Private Sub filldatagridrefretdetails()
        Try
            fields = "*"
            table = "loc_refund_return_details WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill System Logs", successmessage:="Success!", datagrid:=DataGridViewRetrefdetails)
            gettablesize(tablename:="loc_refund_return_details")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    '=====================================================SYSTEMLOGS
    Private Sub filldatagridsystemlog1()
        Try
            fields = "*"
            table = "loc_system_logs WHERE synced = 'Unsynced' AND log_type IN ('LOG OUT', 'LOGIN', 'ERROR') AND log_store = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill System Logs", successmessage:="Success!", datagrid:=DataGridViewSYSLOG1)
            gettablesize(tablename:="loc_system_logs")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridsystemlog2()
        Try
            fields = "*"
            table = "loc_system_logs WHERE synced = 'Unsynced' AND log_type IN ('MENU FORM', 'STOCK ENTRY', 'STOCK REMOVAL', 'STOCK TRANSFER') AND log_store = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill System Logs", successmessage:="Success!", datagrid:=DataGridViewSYSLOG2)
            gettablesize(tablename:="loc_system_logs")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridsystemlog3()
        Try
            fields = "*"
            table = "loc_system_logs WHERE synced = 'Unsynced' AND log_type IN ('NEW CUSTOM PRODUCT', 'NEW EXPENSE', 'NEW STOCK ADDED', 'NEW USER') AND log_store = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill System Logs", successmessage:="Success!", datagrid:=DataGridViewSYSLOG3)
            gettablesize(tablename:="loc_system_logs")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridsystemlog4()
        Try
            fields = "*"
            table = "loc_system_logs WHERE synced = 'Unsynced' AND log_type IN ('TRANSACTION', 'USER UPDATE') AND log_store = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill System Logs", successmessage:="Success!", datagrid:=DataGridViewSYSLOG4)
            gettablesize(tablename:="loc_system_logs")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    '=====================================================SYSTEMLOGS
    Private Sub filldatagridtransaction()
        Try
            fields = "*"
            table = "loc_daily_transaction WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill Transaction", successmessage:="Success!", datagrid:=DataGridViewTRAN)
            gettablesize(tablename:="loc_daily_transaction")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    '======================================================TRANSACTION DETAILS
    Private Sub filldatagridtransactiondetails1()
        Try
            fields = "*"
            table = "loc_daily_transaction_details WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill Transaction Details", successmessage:="Success!", datagrid:=DataGridViewTRANDET)
            gettablesize(tablename:="loc_daily_transaction_details")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridinventory()
        Try
            fields = "*"
            table = "loc_pos_inventory WHERE store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "' AND synced = 'Unsynced'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill Transaction Details", successmessage:="Success!", datagrid:=DataGridViewINV)
            gettablesize(tablename:="loc_pos_inventory")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridexpenses()
        Try
            fields = "*"
            table = "loc_expense_list WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill Transaction Details", successmessage:="Success!", datagrid:=DataGridViewEXP)
            gettablesize(tablename:="loc_expense_list")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridexpensesdetails()
        Try
            fields = "*"
            table = "loc_expense_details WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill Transaction Details", successmessage:="Success!", datagrid:=DataGridViewEXPDET)
            gettablesize(tablename:="loc_expense_details")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridlocusers()
        Try
            fields = "*"
            table = "loc_users WHERE store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "' AND synced = 'Unsynced'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill Local Users", successmessage:="Success!", datagrid:=DataGridViewLocusers)
            gettablesize(tablename:="loc_users")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridproducts()
        Try
            fields = "*"
            table = "loc_admin_products WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "' AND product_status = 0"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill Transaction", successmessage:="Success!", datagrid:=DataGridViewCUSTOMPRODUCTS)
            gettablesize(tablename:="loc_daily_transaction")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagridmodeoftransaction()
        Try
            fields = "*"
            table = "loc_transaction_mode_details WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill Transaction", successmessage:="Success!", datagrid:=DataGridViewMODEOFTRANSACTION)
            gettablesize(tablename:="loc_transaction_mode_details")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub filldatagriddepositslip()
        Try
            fields = "*"
            table = "loc_deposit WHERE synced = 'Unsynced' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'"
            GLOBAL_SELECT_ALL_FUNCTION(fields:=fields, table:=table, errormessage:="Fill Transaction", successmessage:="Success!", datagrid:=DataGridViewDepositSlip)
            gettablesize(tablename:="loc_deposit")
            countrows(tablename:=table)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub countrows(ByVal tablename As String)
        Try
            sql = "SELECT COUNT(*) FROM " & tablename & " "
            cmd = New MySqlCommand(sql, LocalhostConn())
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            For Each row As DataRow In dt.Rows
                DataGridView2.Rows.Add(row("COUNT(*)"), tablename)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub gettablesize(ByVal tablename As String)
        Try
            sql = "SELECT table_name AS `Table`, round(((data_length + index_length) / 1024 / 1024), 2) `Size in MB` FROM information_schema.TABLES WHERE table_schema = 'pos' AND table_name = '" & tablename & "'"
            cmd = New MySqlCommand(sql, LocalhostConn())
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            For Each row As DataRow In dt.Rows
                DataGridView1.Rows.Add(row("Table"), row("Size in MB"))
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadData()
        Try
            Timer1.Start()
            Label1.Text = ""
            Label2.Text = ""
            Label3.Text = ""
            Label5.Text = ""
            ClearDataGridViewRows(Me)
            filldatagridtransaction()
            '================================================
            filldatagridtransactiondetails1()
            '================================================
            filldatagridinventory()
            filldatagridexpenses()
            filldatagridexpensesdetails()
            filldatagridlocusers()
            '================================================
            filldatagridsystemlog1()
            filldatagridsystemlog2()
            filldatagridsystemlog3()
            filldatagridsystemlog4()
            filldatagridrefretdetails()
            filldatagridproducts()
            '================================================
            filldatagridmodeoftransaction()
            filldatagriddepositslip()
            '================================================
            totalrow = SumOfColumnsToInt(DataGridView2, 0)
            Label3.Text = totalrow
            Button1.Enabled = False
            Label2.Text = "Item(s)"
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LoadData()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If My.Settings.ValidCloudConn = True Then
            'Button1.PerformClick()
            ProgressBar1.Value = 0
            BackgroundWorker1.WorkerSupportsCancellation = True
            BackgroundWorker1.WorkerReportsProgress = True
            BackgroundWorker1.RunWorkerAsync()
            Button2.Enabled = False
            SyncIsOnProcess = True
        Else
            MsgBox("Cloud connection is not valid.")
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
    Dim thread1 As Thread
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
                ProgressBar1.Maximum = Val(Label3.Text)
                'POS.ProgressBar1.Maximum = Val(Label7.Text)
                '   ============================================================================Transaction
                thread1 = New Thread(AddressOf insertlocaldailytransaction)
                thread1.Start()
                threadListLOCTRAN.Add(thread1)
                thread1 = New Thread(AddressOf inserttransactiondetails1)
                thread1.Start()
                threadListLOCTD1.Add(thread1)
                '  ============================================================================Inventory
                thread1 = New Thread(AddressOf insertinventory)
                thread1.Start()
                threadListLOCINV.Add(thread1)
                '   ============================================================================Expenses
                thread1 = New Thread(AddressOf insertexpenses)
                thread1.Start()
                threadListLOCEXP.Add(thread1)
                thread1 = New Thread(AddressOf insertexpensedetails)
                thread1.Start()
                threadListLOCEXPD.Add(thread1)
                '============================================================================Users
                thread1 = New Thread(AddressOf insertlocalusers)
                thread1.Start()
                threadListLOCTUSER.Add(thread1)
                '============================================================================System Logs
                thread1 = New Thread(AddressOf insertsystemlogs1)
                thread1.Start()
                threadListLOCSYSLOG1.Add(thread1)
                thread1 = New Thread(AddressOf insertsystemlogs2)
                thread1.Start()
                threadListLOCSYSLOG2.Add(thread1)
                thread1 = New Thread(AddressOf insertsystemlogs3)
                thread1.Start()
                threadListLOCSYSLOG3.Add(thread1)
                thread1 = New Thread(AddressOf insertsystemlogs4)
                thread1.Start()
                threadListLOCSYSLOG4.Add(thread1)
                '============================================================================Returns / Refunds
                thread1 = New Thread(AddressOf insertrefretdetails)
                thread1.Start()
                threadListLOCREFRET.Add(thread1)

                thread1 = New Thread(AddressOf insertlocproducts)
                thread1.Start()
                threadListLOCPRODUCT.Add(thread1)

                thread1 = New Thread(AddressOf insertlocmodeoftransaction)
                thread1.Start()
                threadListMODEOFTRANSACTION.Add(thread1)

                thread1 = New Thread(AddressOf insertlocdeposit)
                thread1.Start()
                threadListLocDeposit.Add(thread1)

                For Each t In threadListLOCTRAN
                    t.Join()
                Next
                For Each t In threadListLOCTD1
                    t.Join()
                Next
                For Each t In threadListLOCTD2
                    t.Join()
                Next
                For Each t In threadListLOCINV
                    t.Join()
                Next
                For Each t In threadListLOCEXP
                    t.Join()
                Next
                For Each t In threadListLOCEXPD
                    t.Join()
                Next
                For Each t In threadListLOCTUSER
                    t.Join()
                Next
                For Each t In threadListLOCSYSLOG1
                    t.Join()
                Next
                For Each t In threadListLOCSYSLOG2
                    t.Join()
                Next
                For Each t In threadListLOCSYSLOG3
                    t.Join()
                Next
                For Each t In threadListLOCSYSLOG4
                    t.Join()
                Next
                For Each t In threadListLOCREFRET
                    t.Join()
                Next
                For Each t In threadListLOCPRODUCT
                    t.Join()
                Next

                For Each t In threadListMODEOFTRANSACTION
                    t.Join()
                Next
                For Each t In threadListLocDeposit
                    t.Join()
                Next
            Else
                Unsuccessful = True
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            If Unsuccessful = True Then
                ChangeProgBarColor(ProgressBar1, ProgressBarColor.Yellow)
                ProgressBar1.Maximum = Val(Label3.Text)
                'POS.ProgressBar1.Maximum = Val(Label3.Text)
                If Label7.Text <> "0" Then
                    Label1.Text = "Synced " & Label7.Text & " of " & Label3.Text & " "
                Else
                    Label3.Text = "0"
                    Label2.Text = "Item(s)"
                    Label1.Text = "Synced 0 of 0"
                End If
                Label5.Text = "Synchronization Failed"
                MsgBox("Internet connection lost")
                Button1.Enabled = True
                Button2.Enabled = True
            Else
                SyncIsOnProcess = False
                Timer1.Enabled = False
                Label1.Text = "Synced " & Label3.Text & " of " & Label3.Text
                Label5.Text = "Successfully Synchronized "
                Dim sync = MessageBox.Show("Synchronization Complete", "Sync", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If sync = DialogResult.OK Then
                    Me.Close()
                    Button1.Enabled = True
                    Button2.Enabled = True
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
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

            With DataGridViewTRAN
                Label8.Text = "Syncing Daily Transaction"
                messageboxappearance = False
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_daily_transaction( `loc_transaction_id`, `transaction_number`, `amounttendered`, `discount`, `moneychange`, `amountdue`, `vatable`, `vat_exempt`, `zero_rated`, `vat`, `si_number`, `crew_id`, `guid`, `active`, `store_id`, `date`, `time`, `transaction_type`, `shift`, `zreading`, `discount_type`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16, @17, @18, @19, @20)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.Decimal).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.Decimal).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@7", MySqlDbType.Decimal).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@8", MySqlDbType.Decimal).Value = .Rows(i).Cells(8).Value.ToString()
                    cmd.Parameters.Add("@9", MySqlDbType.Decimal).Value = .Rows(i).Cells(9).Value.ToString()
                    cmd.Parameters.Add("@10", MySqlDbType.Int64).Value = .Rows(i).Cells(10).Value.ToString()
                    cmd.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                    cmd.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                    cmd.Parameters.Add("@13", MySqlDbType.VarChar).Value = .Rows(i).Cells(13).Value.ToString()
                    cmd.Parameters.Add("@14", MySqlDbType.VarChar).Value = .Rows(i).Cells(14).Value.ToString()
                    cmd.Parameters.Add("@15", MySqlDbType.VarChar).Value = returndateformat(.Rows(i).Cells(15).Value.ToString())
                    cmd.Parameters.Add("@16", MySqlDbType.VarChar).Value = .Rows(i).Cells(16).Value.ToString()
                    cmd.Parameters.Add("@17", MySqlDbType.VarChar).Value = .Rows(i).Cells(17).Value.ToString()
                    cmd.Parameters.Add("@18", MySqlDbType.VarChar).Value = .Rows(i).Cells(18).Value.ToString()
                    cmd.Parameters.Add("@19", MySqlDbType.Text).Value = Dateandtimeformat(.Rows(i).Cells(19).Value.ToString(), "MM/dd/yyyy", "yyyy-MM-dd")
                    cmd.Parameters.Add("@20", MySqlDbType.Text).Value = .Rows(i).Cells(21).Value.ToString()
                    Label7.Text = Val(Label7.Text + 1)
                    Label30.Text = Val(Label30.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label8.Text = "Synced Daily Transaction"
                Label21.Text = Label6.Text & " Seconds"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
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

            Label9.Text = "Syncing Transaction Details"
            With DataGridViewTRANDET
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_daily_transaction_details(`loc_details_id`, `product_id`, `product_sku`, `product_name`, `quantity`, `price`, `total`, `crew_id`
                                            , `transaction_number`, `active`, `created_at`, `timenow`, `guid`, `store_id`, `total_cost_of_goods`, `product_category` , `zreading`) 
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
                    cmd.Parameters.Add("@a10", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(10).Value.ToString(), "MM/dd/yyyy", "yyyy-MM-dd")
                    cmd.Parameters.Add("@a11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                    cmd.Parameters.Add("@a12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                    cmd.Parameters.Add("@a13", MySqlDbType.VarChar).Value = .Rows(i).Cells(13).Value.ToString()
                    cmd.Parameters.Add("@a14", MySqlDbType.Decimal).Value = .Rows(i).Cells(14).Value.ToString()
                    cmd.Parameters.Add("@a15", MySqlDbType.VarChar).Value = .Rows(i).Cells(15).Value.ToString()
                    cmd.Parameters.Add("@a16", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(16).Value.ToString(), "MM/dd/yyyy", "yyyy-MM-dd")
                    '====================================================================
                    Label7.Text = Val(Label7.Text + 1)
                    Label31.Text = Val(Label31.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label9.Text = "Synced Transaction Details"
                Label20.Text = Label6.Text & " Seconds"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
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

            Label10.Text = "Syncing Inventory"
            With DataGridViewINV
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmdupdateinventory = New MySqlCommand("UPDATE admin_pos_inventory SET stock_quantity = " & .Rows(i).Cells(6).Value & " , stock_total = " & .Rows(i).Cells(7).Value & " WHERE store_id =" & ClientStoreID & " AND guid = '" & ClientGuid & "' AND loc_inventory_id = " & .Rows(i).Cells(0).Value, server)
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_pos_inventory( `loc_inventory_id`, `store_id`, `formula_id`, `product_ingredients`, `sku`, `stock_quantity`, `stock_total`, `stock_status`, `critical_limit`, `guid`, `date`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.Int64).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.Int64).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.Int64).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@7", MySqlDbType.Int64).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@8", MySqlDbType.Int64).Value = .Rows(i).Cells(8).Value.ToString()
                    cmd.Parameters.Add("@9", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                    cmd.Parameters.Add("@10", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(10).Value.ToString(), "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd HH:mm:ss")
                    With cmd
                        Label7.Text = Val(Label7.Text + 1)
                        Label32.Text = Val(Label32.Text) + 1
                        ProgressBar1.Value = Val(Label7.Text)
                        'POS.ProgressBar1.Value = Val(Label7.Text)
                        Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label10.Text = "Synced Inventories"
                Label19.Text = Label6.Text & " Seconds"
            End With
        Catch ex As Exception
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
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


            Label11.Text = "Syncing Expense List"
            With DataGridViewEXP
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_expense_list(`loc_expense_id`
                                                                         , `crew_id`
                                                                         , `expense_number`
                                                                         , `total_amount`
                                                                         , `paid_amount`
                                                                         , `unpaid_amount`
                                                                         , `store_id`
                                                                         , `guid`
                                                                         , `date`
                                                                         , `time`
                                                                         , `active`) 
                                             VALUES (@loc_expense_id, @crew_id, @expense_number
                                             , @total_amount, @paid_amount, @unpaid_amount, @store_id, @guid, @date
                                             , @time , @active)", server)
                    cmd.Parameters.Add("@loc_expense_id", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@crew_id", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@expense_number", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@total_amount", MySqlDbType.Decimal).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@paid_amount", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@unpaid_amount", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@store_id", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@guid", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                    'MsgBox(.Rows(i).Cells(8).Value.ToString())
                    'MsgBox(Dateandtimeformat(.Rows(i).Cells(8).Value.ToString(), "MM/dd/yyyy", "yyyy-MM-dd"))
                    cmd.Parameters.Add("@date", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(8).Value.ToString(), "MM/dd/yyyy", "yyyy-MM-dd")
                    cmd.Parameters.Add("@time", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                    cmd.Parameters.Add("@active", MySqlDbType.Int64).Value = .Rows(i).Cells(10).Value.ToString()
                    '====================================================================
                    Label7.Text = Val(Label7.Text + 1)
                    Label33.Text = Val(Label33.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label11.Text = "Synced Expense List"
                Label18.Text = Label6.Text & " Seconds"
            End With
            'truncatetable(tablename:="loc_expense_list")
        Catch ex As Exception
            MsgBox(ex.ToString)
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
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


            Label12.Text = "Syncing Expense Details"
            With DataGridViewEXPDET
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_expense_details(`loc_details_id`
                                                                            , `expense_number`
                                                                            , `expense_type`
                                                                            , `item_info`
                                                                            , `quantity`
                                                                            , `price`
                                                                            , `amount`
                                                                            , `attachment`
                                                                            , `created_at`
                                                                            , `time`
                                                                            , `crew_id`
                                                                            , `guid`
                                                                            , `store_id`
                                                                            , `active`) 
                                             VALUES (@loc_details_id, @expense_number, @expense_type
                                             , @item_info, @quantity, @price, @amount, @attachment, @created_at
                                             , @time , @crew_id, @guid, @store_id, @active)", server)
                    cmd.Parameters.Add("@loc_details_id", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@expense_number", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@expense_type", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@item_info", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@quantity", MySqlDbType.Int64).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@price", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@amount", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@attachment", MySqlDbType.Text).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@created_at", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(8).Value.ToString(), "MM/dd/yyyy", "yyyy-MM-dd")
                    cmd.Parameters.Add("@time", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                    cmd.Parameters.Add("@crew_id", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                    cmd.Parameters.Add("@guid", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                    cmd.Parameters.Add("@store_id", MySqlDbType.Int64).Value = .Rows(i).Cells(12).Value.ToString()
                    cmd.Parameters.Add("@active", MySqlDbType.Int64).Value = .Rows(i).Cells(13).Value.ToString()
                    '====================================================================
                    Label7.Text = Val(Label7.Text + 1)
                    Label34.Text = Val(Label34.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label12.Text = "Synced Expense Details"
                Label17.Text = Label6.Text & " Seconds"
            End With
            'truncatetable(tablename:="loc_expense_details")
        Catch ex As Exception
            MsgBox(ex.ToString)
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
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


            Label13.Text = "Syncing Accounts"
            With DataGridViewLocusers
                messageboxappearance = False
                For i As Integer = 0 To .Rows.Count - 1 Step +1
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
                    cmd.Parameters.Add("@9", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(9).Value.ToString(), "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd hh:mm:ss")
                    cmd.Parameters.Add("@10", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(10).Value.ToString(), "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd hh:mm:ss")
                    cmd.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                    cmd.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                    cmd.Parameters.Add("@13", MySqlDbType.VarChar).Value = .Rows(i).Cells(13).Value.ToString()
                    cmd.Parameters.Add("@14", MySqlDbType.VarChar).Value = .Rows(i).Cells(14).Value.ToString()
                    '====================================================================
                    Label7.Text = Val(Label7.Text + 1)
                    Label35.Text = Val(Label35.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label13.Text = "Synced Accounts"
                Label16.Text = Label6.Text & " Seconds"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
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

            Label22.Text = "Syncing Systemlogs 1"
            With DataGridViewSYSLOG1
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_system_logs(`crew_id`
                                                                        , `log_type`
                                                                        , `log_description`
                                                                        , `log_date_time`
                                                                        , `log_store`
                                                                        , `guid`
                                                                        , `loc_systemlog_id`) 
                    VALUES (@0, @1, @2, @3, @4, @5, @6)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    '= "MM/dd/yyyy hh:mm:ss"
                    '"yyyy-MM-dd hh:mm:ss"
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(3).Value.ToString(), "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd hh:mm:ss")
                    cmd.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                    '====================================================================
                    Label7.Text = Val(Label7.Text + 1)
                    Label37.Text = Val(Label37.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label22.Text = "Synced Systemlogs 1"
                Label26.Text = Label6.Text & " Seconds"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
        End Try
    End Sub
    Private Sub insertsystemlogs2()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()

            Label23.Text = "Syncing Systemlogs 2"
            With DataGridViewSYSLOG2
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_system_logs(`crew_id`
                                                                        , `log_type`
                                                                        , `log_description`
                                                                        , `log_date_time`
                                                                        , `log_store`
                                                                        , `guid`
                                                                        , `loc_systemlog_id`) 
                    VALUES (@0, @1, @2, @3, @4, @5, @6)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(3).Value.ToString(), "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd hh:mm:ss")
                    cmd.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                    '====================================================================
                    Label7.Text = Val(Label7.Text + 1)
                    Label38.Text = Val(Label38.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label23.Text = "Synced Systemlogs 2"
                Label27.Text = Label6.Text & " Seconds"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
        End Try
    End Sub
    Private Sub insertsystemlogs3()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()

            Label24.Text = "Syncing Systemlogs 3"
            With DataGridViewSYSLOG3
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_system_logs(`crew_id`
                                                                        , `log_type`
                                                                        , `log_description`
                                                                        , `log_date_time`
                                                                        , `log_store`
                                                                        , `guid`
                                                                        , `loc_systemlog_id`) 
                    VALUES (@0, @1, @2, @3, @4, @5, @6)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(3).Value.ToString(), "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd hh:mm:ss")
                    cmd.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                    '====================================================================
                    Label7.Text = Val(Label7.Text + 1)
                    Label39.Text = Val(Label39.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label24.Text = "Synced Systemlogs 3"
                Label28.Text = Label6.Text & " Seconds"
            End With
        Catch ex As Exception
            Unsuccessful = True
            MsgBox(ex.ToString)
            BackgroundWorker1.CancelAsync()
        End Try
    End Sub
    Private Sub insertsystemlogs4()
        Try
            Dim cmd As MySqlCommand
            Dim cmdloc As MySqlCommand

            Dim server As MySqlConnection = New MySqlConnection
            server.ConnectionString = CloudConnectionString
            server.Open()

            Dim local As MySqlConnection = New MySqlConnection
            local.ConnectionString = LocalConnectionString
            local.Open()

            Label25.Text = "Syncing Systemlogs 4"
            With DataGridViewSYSLOG4
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_system_logs(`crew_id`
                                                                        , `log_type`
                                                                        , `log_description`
                                                                        , `log_date_time`
                                                                        , `log_store`
                                                                        , `guid`
                                                                        , `loc_systemlog_id`) 
                    VALUES (@0, @1, @2, @3, @4, @5, @6)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(3).Value.ToString(), "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd hh:mm:ss")
                    cmd.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                    '====================================================================
                    Label7.Text = Val(Label7.Text + 1)
                    Label40.Text = Val(Label40.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label25.Text = "Synced Systemlogs 4"
                Label29.Text = Label6.Text & " Seconds"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
        End Try
    End Sub
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

            Label36.Text = "Syncing Refund Details"
            With DataGridViewRetrefdetails
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_refund_return_details( `loc_refret_id`, `transaction_number`, `crew_id`, `reason`, `total`, `guid`, `ipaddress`, `store_id`, `datereturned`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.Text).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                    cmd.Parameters.Add("@7", MySqlDbType.Int64).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@8", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(8).Value.ToString(), "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd hh:mm:ss")
                    '====================================================================
                    Label7.Text = Val(Label7.Text + 1)
                    Label14.Text = Val(Label14.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label36.Text = "Synced Refund Details"
                Label15.Text = Label6.Text & " Seconds"
            End With
            'truncatetable(tablename:="loc_expense_list")
        Catch ex As Exception
            MsgBox(ex.ToString)
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
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

            Label43.Text = "Syncing Local Products"
            With DataGridViewCUSTOMPRODUCTS
                For i As Integer = 0 To .Rows.Count - 1 Step +1
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
                    cmd.Parameters.Add("@11", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(11).Value.ToString(), "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd hh:mm:ss")
                    cmd.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                    cmd.Parameters.Add("@13", MySqlDbType.Int64).Value = .Rows(i).Cells(13).Value.ToString()
                    cmd.Parameters.Add("@14", MySqlDbType.VarChar).Value = .Rows(i).Cells(14).Value.ToString()
                    '====================================================================
                    Label7.Text = Val(Label7.Text + 1)
                    Label41.Text = Val(Label41.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label43.Text = "Synced Local Products"
                Label42.Text = Label6.Text & " Seconds"
            End With
            'truncatetable(tablename:="loc_expense_list")
        Catch ex As Exception
            MsgBox(ex.ToString)
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
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

            Label44.Text = "Syncing Mode of Transaction"
            With DataGridViewMODEOFTRANSACTION
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_transaction_mode_details(`loc_mode_id`, `transaction_type`, `transaction_number`, `full_name`, `reference`, `markup`, `date_time_created`, `status`, `store_id`, `guid`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9)", server)
                    cmd.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmd.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                    cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    MsgBox(.Rows(i).Cells(6).Value.ToString())
                    cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = Dateandtimeformat(.Rows(i).Cells(6).Value.ToString(), "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd hh:mm:ss")
                    cmd.Parameters.Add("@7", MySqlDbType.Int64).Value = .Rows(i).Cells(7).Value.ToString()
                    cmd.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                    cmd.Parameters.Add("@9", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                    '====================================================================
                    Label7.Text = Val(Label7.Text + 1)
                    Label46.Text = Val(Label46.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label44.Text = "Synced Mode of Transaction"
                Label45.Text = Label6.Text & " Seconds"
            End With
            'truncatetable(tablename:="loc_expense_list")
        Catch ex As Exception
            MsgBox(ex.ToString)
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
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

            Label47.Text = "Syncing Deposit Details"
            With DataGridViewDepositSlip
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmd = New MySqlCommand("INSERT INTO Triggers_admin_deposit_slip_details( `loc_dep_id`, `name`, `crew_id`, `transaction_number`, `amount`, `bank`, `transaction_date`, `store_id`, `guid`, `date_created`)
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
                    MsgBox(.Rows(i).Cells(9).Value.ToString())
                    cmd.Parameters.Add("@9", MySqlDbType.Timestamp).Value = Dateandtimeformat(.Rows(i).Cells(9).Value.ToString(), "MM/dd/yyyy HH:mm:ss", "yyyy-MM-dd hh:mm:ss")
                    '====================================================================
                    Label7.Text = Val(Label7.Text + 1)
                    Label49.Text = Val(Label49.Text) + 1
                    ProgressBar1.Value = Val(Label7.Text)
                    'POS.ProgressBar1.Value = Val(Label7.Text)
                    Label1.Text = "Syncing " & Label7.Text & " of " & Label3.Text & " "
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
                Label47.Text = "Synced  Deposit Details"
                Label48.Text = Label6.Text & " Seconds"
            End With
            'truncatetable(tablename:="loc_expense_list")
        Catch ex As Exception
            MsgBox(ex.ToString)
            Unsuccessful = True
            BackgroundWorker1.CancelAsync()
        End Try
    End Sub
End Class