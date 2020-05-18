Imports System.Net
Imports System.Threading
Imports MySql.Data.MySqlClient
Public Class Loading
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
                    If LocalhostConn.State = ConnectionState.Open Then
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
                    If LocalhostConn.State = ConnectionState.Open Then
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
                        If CheckIfNeedToReset(LocalhostConn) = True Then
                            IfNeedsToReset = True
                        Else
                            IfNeedsToReset = False
                        End If
                    End If
                End If
                If i = 80 Then
                    If LocalhostConn.State = ConnectionState.Open Then
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
                Dim sql = "SELECT A_Export_Path, A_Tax, A_SIFormat, A_Terminal_No, A_ZeroRated, S_Zreading FROM loc_settings WHERE settings_id = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn())
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                For Each row As DataRow In dt.Rows
                    If row("A_Export_Path") <> "" Then
                        If row("A_Tax") <> "" Then
                            If row("A_SIFormat") <> "" Then
                                If row("A_Terminal_No") <> "" Then
                                    If row("A_ZeroRated") <> "" Then
                                        If row("S_Zreading") <> "" Then
                                            S_ExportPath = ConvertB64ToString(row("A_Export_Path"))
                                            S_Tax = row("A_Tax")
                                            S_SIFormat = row("A_SIFormat")
                                            S_Terminal_No = row("A_Terminal_No")
                                            S_ZeroRated = row("A_ZeroRated")
                                            S_Zreading = row("S_Zreading")
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadMasterList()
        Try
            If LocalConnectionIsOnOrValid = True Then
                sql = "SELECT * FROM admin_masterlist WHERE masterlist_id = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
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
            Dim sql = "SELECT * FROM admin_outlets WHERE user_guid = '" & ClientGuid & "' AND store_id = " & ClientStoreID & ";"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
            Dim dr As MySqlDataReader = cmd.ExecuteReader()
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
            sql = "INSERT INTO loc_inv_temp_data (`store_id`, `formula_id`, `product_ingredients`, `sku`, `stock_quantity`, `stock_total`, `stock_status`, `critical_limit`, `guid`, `date_modified`, `date_created`)  SELECT `store_id`, `formula_id`, `product_ingredients`, `sku`, `stock_quantity`, `stock_total`, `stock_status`, `critical_limit`, `guid`, `date_modified` ,(SELECT date_add(date_add(LAST_DAY(NOW()),interval 1 DAY),interval -1 MONTH) AS first_day) FROM loc_pos_inventory"
            cmd = New MySqlCommand
            With cmd
                .CommandText = sql
                .Connection = LocalhostConn()
                .ExecuteNonQuery()
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub ResetStocks()
        Try
            sql = "UPDATE `loc_pos_inventory` SET `stock_quantity`= 0,`stock_total`= 0"
            cmd = New MySqlCommand
            With cmd
                .CommandText = sql
                .Connection = LocalhostConn()
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
    Private Sub SyncToLocalUsers()
        Try
            With DataGridViewRESULT
                sql = "SELECT `user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `active`, `guid`, `store_id`, `uniq_id` , `created_at`, `updated_at` FROM `loc_users` WHERE guid = '" & ClientGuid & "' AND store_id = '" & ClientStoreID & "' AND synced = 'Unsynced' AND active = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, ServerCloudCon())
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim DataTableServer As DataTable = New DataTable
                da.Fill(DataTableServer)
                .DataSource = DataTableServer
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    table = "triggers_loc_users"
                    fields = "(`user_level`, `full_name`, `username`, `password`, `contact_number`, `email`, `position`, `gender`, `active`, `guid`, `store_id`, `uniq_id`, `synced`)"
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
                         ,'Unsynced')"
                    GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, successmessage:=successmessage, errormessage:=errormessage)
                    sql = "UPDATE loc_users SET synced = 'Synced' WHERE uniq_id = '" & .Rows(i).Cells(11).Value.ToString & "'"
                    cmd = New MySqlCommand(sql, ServerCloudCon())
                    cmd.ExecuteNonQuery()
                    sql = "UPDATE loc_users SET `full_name` = '" & .Rows(i).Cells(1).Value.ToString & "'  , `username` = '" & .Rows(i).Cells(2).Value.ToString & "' , `password` = '" & .Rows(i).Cells(3).Value.ToString & "'  , `contact_number` = '" & .Rows(i).Cells(4).Value.ToString & "'  WHERE uniq_id = '" & .Rows(i).Cells(11).Value.ToString & "' "
                    cmd = New MySqlCommand(sql, LocalhostConn())
                    cmd.ExecuteNonQuery()
                Next
            End With
        Catch ex As Exception
            Label1.Text = "Invalid connection..."
            'MsgBox(ex.ToString)
        End Try
    End Sub
#Region "Updates"
    Private Function LoadFormulaLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        Try
            Dim sql = "SELECT date_modified, server_formula_id FROM loc_product_formula"
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
            Dim sql = "SELECT server_date_modified , server_inventory_id FROM loc_pos_inventory"
            cmdlocal = New MySqlCommand(sql, LocalhostConn())
            dalocal = New MySqlDataAdapter(cmdlocal)
            dalocal.Fill(dtlocal)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dtlocal
    End Function
    Private Function LoadCategoryLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        Try
            Dim sql = "SELECT updated_at, category_id FROM loc_admin_category"
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
            Dim cmdserver As MySqlCommand
            Dim daserver As MySqlDataAdapter
            Dim dtserver As DataTable
            CategoryDTUpdate = New DataTable
            CategoryDTUpdate.Columns.Add("category_id")
            CategoryDTUpdate.Columns.Add("category_name")
            CategoryDTUpdate.Columns.Add("brand_name")
            CategoryDTUpdate.Columns.Add("updated_at")
            CategoryDTUpdate.Columns.Add("origin")
            CategoryDTUpdate.Columns.Add("status")
            Dim Ids As String = ""
            If My.Settings.ValidCloudConn = True Then
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
                    If returndateformatfulldate(LoadCategoryLocal(i)(0)) <> returndateformatfulldate(dtserver(i)(3)) Then
                        DataGridView1.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5))
                        Dim Category As DataRow = CategoryDTUpdate.NewRow
                        Category("category_id") = dtserver(i)(0)
                        Category("category_name") = dtserver(i)(1)
                        Category("brand_name") = dtserver(i)(2)
                        Category("updated_at") = dtserver(i)(3)
                        Category("origin") = dtserver(i)(4)
                        Category("status") = dtserver(i)(5)
                        CategoryDTUpdate.Rows.Add(Category)
                    End If
                Next
                Dim sql2 = "SELECT `category_id`, `category_name`, `brand_name`, `updated_at`, `origin`, `status` FROM admin_category WHERE category_id NOT IN (" & Ids & ")"
                cmdserver = New MySqlCommand(sql2, ServerCloudCon())
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    If returndateformatfulldate(LoadCategoryLocal(i)(0)) <> returndateformatfulldate(dtserver(i)(3)) Then
                        DataGridView1.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5))
                        Dim Category As DataRow = CategoryDTUpdate.NewRow
                        Category("category_id") = dtserver(i)(0)
                        Category("category_name") = dtserver(i)(1)
                        Category("brand_name") = dtserver(i)(2)
                        Category("updated_at") = dtserver(i)(3)
                        Category("origin") = dtserver(i)(4)
                        Category("status") = dtserver(i)(5)
                        CategoryDTUpdate.Rows.Add(Category)
                    End If
                Next
            End If
        Catch ex As Exception
            'If table doesnt have data
        End Try
    End Sub
    Private Function LoadProductLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        Try
            Dim sql = "SELECT date_modified, server_product_id FROM loc_admin_products"
            cmdlocal = New MySqlCommand(sql, LocalhostConn())
            dalocal = New MySqlDataAdapter(cmdlocal)
            dalocal.Fill(dtlocal)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dtlocal
    End Function
    Private Sub Function2()
        Try
            Dim cmdserver As MySqlCommand
            Dim daserver As MySqlDataAdapter
            Dim dtserver As DataTable
            Dim Ids As String = ""
            ProductDTUpdate = New DataTable
            ProductDTUpdate.Columns.Add("product_id")
            ProductDTUpdate.Columns.Add("product_sku")
            ProductDTUpdate.Columns.Add("product_name")
            ProductDTUpdate.Columns.Add("formula_id")
            ProductDTUpdate.Columns.Add("product_barcode")
            ProductDTUpdate.Columns.Add("product_category")
            ProductDTUpdate.Columns.Add("product_price")
            ProductDTUpdate.Columns.Add("product_desc")
            ProductDTUpdate.Columns.Add("product_image")
            ProductDTUpdate.Columns.Add("product_status")
            ProductDTUpdate.Columns.Add("origin")
            ProductDTUpdate.Columns.Add("date_modified")
            If My.Settings.ValidCloudConn = True Then
                For i As Integer = 0 To LoadProductLocal.Rows.Count - 1 Step +1
                    If Ids = "" Then
                        Ids = "" & LoadProductLocal(i)(1) & ""
                    Else
                        Ids += "," & LoadProductLocal(i)(1) & ""
                    End If
                Next
                Dim sql = "SELECT `product_id`, `product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified` FROM admin_products_org WHERE product_id IN (" & Ids & ")"
                cmdserver = New MySqlCommand(sql, ServerCloudCon())
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    If returndateformatfulldate(LoadProductLocal(i)(0)) <> returndateformatfulldate(dtserver(i)(11)) Then
                        DataGridView2.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8), dtserver(i)(9), dtserver(i)(10), dtserver(i)(11))
                        Dim Product As DataRow = ProductDTUpdate.NewRow
                        Product("product_id") = dtserver(i)(0)
                        Product("product_sku") = dtserver(i)(1)
                        Product("product_name") = dtserver(i)(2)
                        Product("formula_id") = dtserver(i)(3)
                        Product("product_barcode") = dtserver(i)(4)
                        Product("product_category") = dtserver(i)(5)
                        Product("product_price") = dtserver(i)(6)
                        Product("product_desc") = dtserver(i)(7)
                        Product("product_image") = dtserver(i)(8)
                        Product("product_status") = dtserver(i)(9)
                        Product("origin") = dtserver(i)(10)
                        Product("date_modified") = dtserver(i)(11)
                        ProductDTUpdate.Rows.Add(Product)
                    End If
                Next
                Dim sql2 = "SELECT `product_id`, `product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified` FROM admin_products_org WHERE product_id NOT IN (" & Ids & ")"
                cmdserver = New MySqlCommand(sql2, ServerCloudCon())
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    If returndateformatfulldate(LoadProductLocal(i)(0)) <> returndateformatfulldate(dtserver(i)(11)) Then
                        DataGridView2.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8), dtserver(i)(9), dtserver(i)(10), dtserver(i)(11))
                        Dim Product As DataRow = ProductDTUpdate.NewRow
                        Product("product_id") = dtserver(i)(0)
                        Product("product_sku") = dtserver(i)(1)
                        Product("product_name") = dtserver(i)(2)
                        Product("formula_id") = dtserver(i)(3)
                        Product("product_barcode") = dtserver(i)(4)
                        Product("product_category") = dtserver(i)(5)
                        Product("product_price") = dtserver(i)(6)
                        Product("product_desc") = dtserver(i)(7)
                        Product("product_image") = dtserver(i)(8)
                        Product("product_status") = dtserver(i)(9)
                        Product("origin") = dtserver(i)(10)
                        Product("date_modified") = dtserver(i)(11)
                        ProductDTUpdate.Rows.Add(Product)
                    End If
                Next
            End If
        Catch ex As Exception
            'If table doesnt have data
        End Try
    End Sub

    Private Sub Function3()
        Try
            Dim Ids As String = ""
            FormulaDTUpdate = New DataTable
            FormulaDTUpdate.Columns.Add("formula_id")
            FormulaDTUpdate.Columns.Add("product_ingredients")
            FormulaDTUpdate.Columns.Add("primary_unit")
            FormulaDTUpdate.Columns.Add("primary_value")
            FormulaDTUpdate.Columns.Add("secondary_unit")
            FormulaDTUpdate.Columns.Add("secondary_value")
            FormulaDTUpdate.Columns.Add("serving_unit")
            FormulaDTUpdate.Columns.Add("serving_value")
            FormulaDTUpdate.Columns.Add("no_servings")
            FormulaDTUpdate.Columns.Add("status")
            FormulaDTUpdate.Columns.Add("date_modified")
            FormulaDTUpdate.Columns.Add("unit_cost")
            FormulaDTUpdate.Columns.Add("origin")

            If My.Settings.ValidCloudConn = True Then
                For i As Integer = 0 To LoadFormulaLocal.Rows.Count - 1 Step +1
                    If Ids = "" Then
                        Ids = "" & LoadFormulaLocal(i)(1) & ""
                    Else
                        Ids += "," & LoadFormulaLocal(i)(1) & ""
                    End If
                Next
                Dim cmdserver As MySqlCommand
                Dim daserver As MySqlDataAdapter
                Dim dtserver As DataTable
                Dim sql = "SELECT `formula_id`, `product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin` FROM admin_product_formula_org WHERE formula_id  IN (" & Ids & ") "
                cmdserver = New MySqlCommand(sql, ServerCloudCon())
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    If returndateformatfulldate(LoadFormulaLocal(i)(0)) <> returndateformatfulldate(dtserver(i)(10)) Then
                        DataGridView3.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8), dtserver(i)(9), dtserver(i)(10), dtserver(i)(11), dtserver(i)(12))
                        Dim Formula As DataRow = FormulaDTUpdate.NewRow
                        Formula("formula_id") = dtserver(i)(0)
                        Formula("product_ingredients") = dtserver(i)(1)
                        Formula("primary_unit") = dtserver(i)(2)
                        Formula("primary_value") = dtserver(i)(3)
                        Formula("secondary_unit") = dtserver(i)(4)
                        Formula("secondary_value") = dtserver(i)(5)
                        Formula("serving_unit") = dtserver(i)(6)
                        Formula("serving_value") = dtserver(i)(7)
                        Formula("no_servings") = dtserver(i)(8)
                        Formula("status") = dtserver(i)(9)
                        Formula("date_modified") = dtserver(i)(10)
                        Formula("unit_cost") = dtserver(i)(11)
                        Formula("origin") = dtserver(i)(12)
                        FormulaDTUpdate.Rows.Add(Formula)
                    End If
                Next
                Dim sql2 = "SELECT `formula_id`, `product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin` FROM admin_product_formula_org WHERE formula_id NOT IN (" & Ids & ") "
                cmdserver = New MySqlCommand(sql2, ServerCloudCon())
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    If returndateformatfulldate(LoadFormulaLocal(i)(0)) <> returndateformatfulldate(dtserver(i)(10)) Then
                        DataGridView3.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8), dtserver(i)(9), dtserver(i)(10), dtserver(i)(11), dtserver(i)(12))
                        Dim Formula As DataRow = FormulaDTUpdate.NewRow
                        Formula("formula_id") = dtserver(i)(0)
                        Formula("product_ingredients") = dtserver(i)(1)
                        Formula("primary_unit") = dtserver(i)(2)
                        Formula("primary_value") = dtserver(i)(3)
                        Formula("secondary_unit") = dtserver(i)(4)
                        Formula("secondary_value") = dtserver(i)(5)
                        Formula("serving_unit") = dtserver(i)(6)
                        Formula("serving_value") = dtserver(i)(7)
                        Formula("no_servings") = dtserver(i)(8)
                        Formula("status") = dtserver(i)(9)
                        Formula("date_modified") = dtserver(i)(10)
                        Formula("unit_cost") = dtserver(i)(11)
                        Formula("origin") = dtserver(i)(12)
                        FormulaDTUpdate.Rows.Add(Formula)
                    End If
                Next
            End If
        Catch ex As Exception
            'If table doesnt have data
        End Try
    End Sub
    Private Sub Function4()
        Try
            InventoryDTUpdate = New DataTable
            InventoryDTUpdate.Columns.Add("inventory_id")
            InventoryDTUpdate.Columns.Add("formula_id")
            InventoryDTUpdate.Columns.Add("product_ingredients")
            InventoryDTUpdate.Columns.Add("sku")
            InventoryDTUpdate.Columns.Add("stock_quantity")
            InventoryDTUpdate.Columns.Add("stock_total")
            InventoryDTUpdate.Columns.Add("stock_status")
            InventoryDTUpdate.Columns.Add("critical_limit")
            InventoryDTUpdate.Columns.Add("date_modified")
            Dim Ids As String = ""
            If My.Settings.ValidCloudConn = True Then
                For i As Integer = 0 To LoadInventoryLocal.Rows.Count - 1 Step +1
                    If Ids = "" Then
                        Ids = "" & LoadInventoryLocal(i)(1) & ""
                    Else
                        Ids += "," & LoadInventoryLocal(i)(1) & ""
                    End If
                Next
                Dim cmdserver As MySqlCommand
                Dim daserver As MySqlDataAdapter
                Dim dtserver As DataTable
                Dim sql = "SELECT `inventory_id`, `formula_id`, `product_ingredients`, `sku`, `stock_quantity`, `stock_total`, `stock_status`, `critical_limit`, `date_modified` FROM admin_pos_inventory_org WHERE inventory_id IN (" & Ids & ")"
                cmdserver = New MySqlCommand(sql, ServerCloudCon())
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    If returndateformatfulldate(LoadInventoryLocal(i)(0)) <> returndateformatfulldate(dtserver(i)(8)) Then
                        DataGridView4.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8))
                        Dim Inventory As DataRow = InventoryDTUpdate.NewRow
                        Inventory("inventory_id") = dtserver(i)(0)
                        Inventory("formula_id") = dtserver(i)(1)
                        Inventory("product_ingredients") = dtserver(i)(2)
                        Inventory("sku") = dtserver(i)(3)
                        Inventory("stock_quantity") = dtserver(i)(4)
                        Inventory("stock_total") = dtserver(i)(5)
                        Inventory("stock_status") = dtserver(i)(6)
                        Inventory("critical_limit") = dtserver(i)(7)
                        Inventory("date_modified") = dtserver(i)(8)
                        InventoryDTUpdate.Rows.Add(Inventory)
                    End If
                Next
                Dim sql2 = "SELECT `inventory_id`, `formula_id`, `product_ingredients`, `sku`, `stock_quantity`, `stock_total`, `stock_status`, `critical_limit`, `date_modified` FROM admin_pos_inventory_org WHERE inventory_id NOT IN (" & Ids & ")"
                cmdserver = New MySqlCommand(sql2, ServerCloudCon())
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    If returndateformatfulldate(LoadInventoryLocal(i)(0)) <> returndateformatfulldate(dtserver(i)(8)) Then
                        DataGridView4.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8))
                        Dim Inventory As DataRow = InventoryDTUpdate.NewRow
                        Inventory("inventory_id") = dtserver(i)(0)
                        Inventory("formula_id") = dtserver(i)(1)
                        Inventory("product_ingredients") = dtserver(i)(2)
                        Inventory("sku") = dtserver(i)(3)
                        Inventory("stock_quantity") = dtserver(i)(4)
                        Inventory("stock_total") = dtserver(i)(5)
                        Inventory("stock_status") = dtserver(i)(6)
                        Inventory("critical_limit") = dtserver(i)(7)
                        Inventory("date_modified") = dtserver(i)(8)
                        InventoryDTUpdate.Rows.Add(Inventory)
                    End If
                Next
            End If
        Catch ex As Exception
            'If table doesnt have data
        End Try
    End Sub
#End Region
End Class
