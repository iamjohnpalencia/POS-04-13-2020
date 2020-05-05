Imports System.Net
Imports System.Threading
Imports MySql.Data.MySqlClient
Public Class Load
    Inherits Form
    Dim strHostName As String
    Dim strIPAddress As String
    Dim RowsReturned As Integer
    Dim thread As Thread
    Dim IfItsIstDayOfTheMonth As Boolean
    Dim IfInternetIsAvailable As Boolean
    Dim IfNeedsToReset As Boolean
    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Enum ProgressBarColor
        Green = &H1
        Red = &H2
        Yellow = &H3
    End Enum
    Private Shared Sub ChangeProgBarColor(ByVal ProgressBar_Name As System.Windows.Forms.ProgressBar, ByVal ProgressBar_Color As ProgressBarColor)
        SendMessage(ProgressBar_Name.Handle, &H410, ProgressBar_Color, 0)
    End Sub
    Dim if1stdayofthemonth
    Private Sub Loadme()
        Try
            CheckForIllegalCrossThreadCalls = False
            Label1.Text = "Initializing component..."
            strHostName = Dns.GetHostName()
            BackgroundWorker1.WorkerSupportsCancellation = True
            BackgroundWorker1.WorkerReportsProgress = True
            BackgroundWorker1.RunWorkerAsync()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Load2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Loadme()
    End Sub
    Dim threadList As List(Of Thread) = New List(Of Thread)
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try

            For i = 0 To 10
                BackgroundWorker1.ReportProgress(i)
                Thread.Sleep(50)
                If i = 0 Then
                    Label1.Text = "Checking local connection..."
                    thread = New Thread(AddressOf LoadLocalConnection)
                    thread.Start()
                    threadList.Add(thread)
                End If
            Next
            For Each t In threadList
                t.Join()
            Next
            For i = 10 To 100
                BackgroundWorker1.ReportProgress(i)
                Thread.Sleep(50)
                If i = 10 Then
                    If localconn.State = ConnectionState.Open Then
                        Label1.Text = "Getting information..."
                        IfConnectionIsConfigured = True
                        thread = New Thread(AddressOf LoadMasterList)
                        thread.Start()
                        threadList.Add(thread)
                        thread = New Thread(AddressOf Function1)
                        thread.Start()
                        threadList.Add(thread)
                        thread = New Thread(AddressOf Function2)
                        thread.Start()
                        threadList.Add(thread)
                        thread = New Thread(AddressOf Function3)
                        thread.Start()
                        threadList.Add(thread)
                        thread = New Thread(AddressOf Function4)
                        thread.Start()
                        threadList.Add(thread)
                    Else
                        IfConnectionIsConfigured = False
                        Label1.Text = "Please Setup Connection in Configuration Manager..."
                    End If
                End If
                If i = 25 Then
                    If localconn.State = ConnectionState.Open Then
                        Label1.Text = "Checking for updates..."
                    End If
                End If
                If i = 50 Then
                    If CheckForInternetConnection() = True Then
                        IfInternetIsAvailable = True
                        Label1.Text = "Connecting to cloud server..."
                        thread = New Thread(AddressOf SyncToLocalUsers)
                        thread.Start()
                        threadList.Add(thread)
                    Else
                        IfInternetIsAvailable = False
                        Label1.Text = "No Internet Connection..."
                    End If
                End If
                If i = 65 Then
                    If IfConnectionIsConfigured = True Then
                        If CheckIfNeedToReset() = True Then
                            IfNeedsToReset = True
                        Else
                            IfNeedsToReset = False
                        End If
                    End If
                End If
                If i = 80 Then
                    If localconn.State = ConnectionState.Open Then
                        thread = New Thread(AddressOf LoadSettings)
                        thread.Start()
                        threadList.Add(thread)
                    End If
                End If
                If i = 95 Then
                    Label1.Text = "Loading..."
                End If
            Next
            For Each t In threadList
                t.Join()
            Next
            If BackgroundWorker1.CancellationPending = True Then
                e.Cancel = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadSettings()
        Try
            If LocalConnectionIsOnOrValid = True Then
                sql = "SELECT A_Export_Path, A_Tax, A_SIFormat, A_Terminal_No, A_ZeroRated, S_Zreading FROM loc_settings WHERE settings_id = 1"
                da = New MySqlDataAdapter(sql, localconn)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    S_ExportPath = dt(0)(0)
                    S_Tax = dt(0)(1)
                    S_SIFormat = dt(0)(2)
                    S_Terminal_No = dt(0)(3)
                    S_ZeroRated = dt(0)(4)
                    S_Zreading = dt(0)(4)
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadMasterList()
        Try
            dbconnection()

            If LocalConnectionIsOnOrValid = True Then
                sql = "SELECT * FROM admin_masterlist WHERE masterlist_id = 1"
                da = New MySqlDataAdapter(sql, localconn)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    RowsReturned = 1
                    ClientGuid = dt(0)(4).ToString
                    ClientProductKey = dt(0)(5).ToString
                    ClientStoreID = dt(0)(9).ToString
                Else
                    RowsReturned = 0
                End If
            Else
                Label1.Text = "Cannot connect to local server..."
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
        Label2.Text = e.ProgressPercentage
    End Sub
    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        If IfConnectionIsConfigured = True Then
            'MsgBox("Connection is open")
            If RowsReturned = 1 Then
                'MsgBox("Activated")
                If IfInternetIsAvailable = True Then
                    'MsgBox("Has internet connection")
                    'MsgBox(IfNeedsToReset)
                    If IfNeedsToReset = True Then
                        BackgroundWorker2.WorkerSupportsCancellation = True
                        BackgroundWorker2.WorkerReportsProgress = True
                        BackgroundWorker2.RunWorkerAsync()
                    Else
                        GetLocalPosData()
                    End If
                Else
                    'MsgBox("No internet connection")
                    If IfNeedsToReset = True Then
                        BackgroundWorker2.WorkerSupportsCancellation = True
                        BackgroundWorker2.WorkerReportsProgress = True
                        BackgroundWorker2.RunWorkerAsync()
                    Else
                        NoInternetConnection()
                    End If
                End If
            Else
                'MsgBox("Not yet activated")
                NotYetActivated()
            End If
        Else
            'MsgBox("Connecion is close")
            ConnectionIsClose()
        End If
    End Sub
    Private Sub BackgroundWorker2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork
        Try
            ProgressBar1.Value = 0
            thread = New Thread(AddressOf Temptinventory)
            thread.Start()
            threadList.Add(thread)
            thread = New Thread(AddressOf ResetStocks)
            thread.Start()
            threadList.Add(thread)
            For i = 0 To 100
                BackgroundWorker2.ReportProgress(i)
                Thread.Sleep(50)
                If i = 0 Then
                    Label1.Text = "Performing inventory reset."
                ElseIf i = 20 Then
                    Label1.Text = "Performing inventory reset.."
                ElseIf i = 40 Then
                    Label1.Text = "Performing inventory reset..."
                ElseIf i = 60 Then
                    Label1.Text = "Performing inventory reset."
                ElseIf i = 80 Then
                    Label1.Text = "Performing inventory reset.."
                ElseIf i = 100 Then
                    Label1.Text = "Performing inventory reset..."
                End If
            Next

            For Each t In threadList
                t.Join()
            Next
            If BackgroundWorker1.CancellationPending = True Then
                e.Cancel = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    '===========================================================================================
    Private Sub NotYetActivated()
        ChangeProgBarColor(ProgressBar1, ProgressBarColor.Yellow)
        Dim result As Integer = MessageBox.Show("Your POS system is not yet activated. Would you like to activate the software now ?", "Activation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Dispose()
            ConfigManager.Show()
        Else
            Application.Exit()
        End If
    End Sub
    Private Sub ConnectionIsClose()
        Dim msg2 As Integer = MessageBox.Show("Would you like to setup server configuration?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If msg2 = DialogResult.Yes Then
            ConfigManager.Show()
            Close()
        ElseIf msg2 = DialogResult.No Then
            Application.Exit()
        End If
    End Sub
    Private Sub NoInternetConnection()
        Dim msg As Integer = MessageBox.Show("No internet connection found, Would you like to continue ?", "Error 5.0", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error)
        If msg = DialogResult.Yes Then
            GetLocalPosData()
        ElseIf msg = DialogResult.No Then
            Application.Exit()
        End If
    End Sub
    Private Sub GetLocalPosData()
        Try
            dbconnection()
            sql = "SELECT * FROM admin_outlets WHERE user_guid = '" & ClientGuid & "' AND store_id = " & ClientStoreID & ";"
            cmd = New MySqlCommand
            With cmd
                .Connection = localconn
                .CommandText = sql
                RowsReturned = .ExecuteScalar
                dr = .ExecuteReader()
            End With
            While dr.Read()
                ClientBrand = dr("brand_name")
                ClientLocation = dr("location_name")
                ClientPostalCode = dr("postal_code")
                ClientAddress = dr("address")
                ClientBrgy = dr("Barangay")
                ClientMunicipality = dr("municipality")
                ClientProvince = dr("province")
                ClientTin = dr("tin_no")
                ClientTel = dr("tel_no")
                ClientStorename = dr("store_name")
                ClientMIN = dr("MIN")
                ClientMSN = dr("MSN")
                ClientPTUN = dr("PTUN")
                getmunicipality = dr("municipality_name")
                getprovince = dr("province_name")
            End While
            localconn.Close()
            cmd.Dispose()
            Dispose()
            Login.Show()
            Login.txtusername.Focus()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Temptinventory()
        Try
            If localconn.State = ConnectionState.Closed Then
                dbconnection()
            Else
            End If
            sql = "INSERT INTO loc_inv_temp_data (`store_id`, `formula_id`, `product_ingredients`, `sku`, `stock_quantity`, `stock_total`, `stock_status`, `critical_limit`, `guid`, `date_modified`, `date_created`)  SELECT `store_id`, `formula_id`, `product_ingredients`, `sku`, `stock_quantity`, `stock_total`, `stock_status`, `critical_limit`, `guid`, `date_modified` ,(SELECT date_add(date_add(LAST_DAY(NOW()),interval 1 DAY),interval -1 MONTH) AS first_day) FROM loc_pos_inventory"
            cmd = New MySqlCommand
            With cmd
                .CommandText = sql
                .Connection = localconn
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub ResetStocks()
        Try
            dbconnection()
            sql = "UPDATE `loc_pos_inventory` SET `stock_quantity`= 0,`stock_total`= 0"
            cmd = New MySqlCommand
            With cmd
                .CommandText = sql
                .Connection = localconn
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorker2_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker2.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
        Label2.Text = e.ProgressPercentage
    End Sub
    Private Sub BackgroundWorker2_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker2.RunWorkerCompleted
        GetLocalPosData()
        Dispose()
        Login.Show()
        Login.txtusername.Focus()
    End Sub
    Dim DataTableServer As New DataTable
    Dim DataTableLocal As New DataTable
    Private Sub SyncToLocalUsers()
        Try
            With DataGridViewRESULT
                serverconn()
                sql = "SELECT `user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `active`, `guid`, `store_id`, `uniq_id` , `created_at`, `updated_at` FROM `loc_users` WHERE guid = '" & ClientGuid & "' AND store_id = '" & ClientStoreID & "' AND synced = 'Unsynced' AND active = 1"
                da = New MySqlDataAdapter(sql, cloudconn)
                DataTableServer = New DataTable
                da.Fill(DataTableServer)
                .DataSource = DataTableServer
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    table = "triggers_loc_users"
                    fields = "(`user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `active`, `guid`, `store_id`, `uniq_id`, `synced`, `created_at`, `updated_at`)"
                    value = "(
                         '" & .Rows(i).Cells(0).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(1).Value.ToString & "'    
                         ,'" & .Rows(i).Cells(2).Value.ToString & "'                 
                         ,'" & .Rows(i).Cells(3).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(4).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(5).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(6).Value.ToString & "'                   
                         ,'" & .Rows(i).Cells(7).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(8).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(9).Value.ToString & "'    
                         ,'" & .Rows(i).Cells(10).Value.ToString & "'   
                         ,'" & .Rows(i).Cells(11).Value.ToString & "'       
                         ,'Unsynced'
                         ,'" & returndatetimeformat(.Rows(i).Cells(12).Value.ToString) & "'   
                         ,'" & returndatetimeformat(.Rows(i).Cells(13).Value.ToString) & "'   
                                )"
                    GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, successmessage:=successmessage, errormessage:=errormessage)
                    sql = "UPDATE loc_users SET synced = 'Synced' WHERE uniq_id = '" & .Rows(i).Cells(11).Value.ToString & "'"
                    cmd = New MySqlCommand(sql, cloudconn)
                    cmd.ExecuteNonQuery()
                    dbconnection()
                    sql = "UPDATE loc_users SET `full_name` = '" & .Rows(i).Cells(1).Value.ToString & "'  , `username` = '" & .Rows(i).Cells(2).Value.ToString & "' , `password` = '" & .Rows(i).Cells(3).Value.ToString & "'  , `contact_number` = '" & .Rows(i).Cells(4).Value.ToString & "'  WHERE uniq_id = '" & .Rows(i).Cells(11).Value.ToString & "' "
                    cmd = New MySqlCommand(sql, localconn)
                    cmd.ExecuteNonQuery()
                Next
                localconn.Close()
            End With
        Catch ex As Exception
            Label1.Text = "Invalid connection..."
            'MsgBox(ex.ToString)
        End Try
    End Sub
#Region "Updates"
    Private Function LoadCategoryLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        Try
            Dim sql = "SELECT updated_at FROM loc_admin_category"
            cmdlocal = New MySqlCommand(sql, LocalhostConn())
            dalocal = New MySqlDataAdapter(cmdlocal)
            dalocal.Fill(dtlocal)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dtlocal
    End Function
    Private Function LoadFormulaLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        Try
            Dim sql = "SELECT date_modified FROM loc_product_formula"
            cmdlocal = New MySqlCommand(sql, LocalhostConn())
            dalocal = New MySqlDataAdapter(cmdlocal)
            dalocal.Fill(dtlocal)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dtlocal
    End Function
    Private Function LoadProductLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        Try
            Dim sql = "SELECT date_modified FROM loc_admin_products"
            cmdlocal = New MySqlCommand(sql, LocalhostConn())
            dalocal = New MySqlDataAdapter(cmdlocal)
            dalocal.Fill(dtlocal)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dtlocal
    End Function
    Private Function LoadInventoryLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        Try
            Dim sql = "SELECT server_date_modified , inventory_id FROM loc_pos_inventory"
            cmdlocal = New MySqlCommand(sql, LocalhostConn())
            dalocal = New MySqlDataAdapter(cmdlocal)
            dalocal.Fill(dtlocal)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dtlocal
    End Function
    Private Sub Function1()
        Try
            LoadCategoryLocal()
            Dim DateS = ""
            For i As Integer = 0 To LoadCategoryLocal.Rows.Count - 1 Step +1
                If DateS = "" Then
                    DateS = "'" & returndateformatfulldate(LoadCategoryLocal(i)(0)) & "'"
                Else
                    DateS += ",'" & returndateformatfulldate(LoadCategoryLocal(i)(0)) & "'"
                End If
            Next
            Dim cmdserver As MySqlCommand
            Dim daserver As MySqlDataAdapter
            Dim dtserver As DataTable
            Dim sql = "SELECT category_id, category_name, brand_name, updated_at, status FROM admin_category WHERE updated_at NOT IN (" & DateS & ")"
            cmdserver = New MySqlCommand(sql, ServerCloudCon())
            daserver = New MySqlDataAdapter(cmdserver)
            dtserver = New DataTable
            daserver.Fill(dtserver)
            daserver.Fill(CategoryDTUpdate)
            DataGridView1.DataSource = dtserver
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Function2()
        Try
            LoadProductLocal()
            Dim DateS As String = ""
            For i As Integer = 0 To LoadProductLocal.Rows.Count - 1 Step +1
                If DateS = "" Then
                    DateS = "'" & returndateformatfulldate(LoadProductLocal(i)(0)) & "'"
                Else
                    DateS += ",'" & returndateformatfulldate(LoadProductLocal(i)(0)) & "'"
                End If
            Next
            Dim cmdserver As MySqlCommand
            Dim daserver As MySqlDataAdapter
            Dim dtserver As DataTable
            Dim sql = "SELECT `formula_id`, `product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost` FROM admin_product_formula_org WHERE date_modified NOT IN (" & DateS & ")"
            cmdserver = New MySqlCommand(sql, ServerCloudCon())
            daserver = New MySqlDataAdapter(cmdserver)
            dtserver = New DataTable
            daserver.Fill(dtserver)
            daserver.Fill(FormulaDTUpdate)
            DataGridView2.DataSource = dtserver
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Function3()
        Try
            LoadCategoryLocal()
            Dim DateS As String = ""
            For i As Integer = 0 To LoadCategoryLocal.Rows.Count - 1 Step +1
                If DateS = "" Then
                    DateS = "'" & returndateformatfulldate(LoadCategoryLocal(i)(0)) & "'"
                Else
                    DateS += ",'" & returndateformatfulldate(LoadCategoryLocal(i)(0)) & "'"
                End If
            Next
            Dim cmdserver As MySqlCommand
            Dim daserver As MySqlDataAdapter
            Dim dtserver As DataTable

            Dim sql = "SELECT `product_id`, `product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified` FROM admin_products_org WHERE date_modified NOT IN (" & DateS & ") "
            cmdserver = New MySqlCommand(sql, ServerCloudCon())
            daserver = New MySqlDataAdapter(cmdserver)
            dtserver = New DataTable
            daserver.Fill(dtserver)
            daserver.Fill(ProductDTUpdate)
            DataGridView3.DataSource = dtserver
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Function4()
        Try
            LoadInventoryLocal()
            Dim DateS As String = ""
            For i As Integer = 0 To LoadInventoryLocal.Rows.Count - 1 Step +1
                If DateS = "" Then
                    DateS = "'" & returndateformatfulldate(LoadInventoryLocal(i)(0)) & "'"
                Else
                    DateS += ",'" & returndateformatfulldate(LoadInventoryLocal(i)(0)) & "'"
                End If
            Next
            Dim cmdserver As MySqlCommand
            Dim daserver As MySqlDataAdapter
            Dim dtserver As DataTable
            Dim sql = "SELECT `inventory_id`, `formula_id`, `product_ingredients`, `sku`, `stock_quantity`, `stock_total`, `stock_status`, `critical_limit`, `date_modified` FROM admin_pos_inventory_org WHERE date_modified NOT IN (" & DateS & ")"
            cmdserver = New MySqlCommand(sql, ServerCloudCon())
            daserver = New MySqlDataAdapter(cmdserver)
            dtserver = New DataTable
            daserver.Fill(dtserver)
            daserver.Fill(InventoryDTUpdate)
            DataGridView4.DataSource = dtserver
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
#End Region
End Class
