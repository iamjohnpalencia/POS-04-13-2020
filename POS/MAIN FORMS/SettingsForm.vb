Imports MySql.Data.MySqlClient
Imports System.Threading
Public Class SettingsForm
    Public AddOrEdit As Boolean
    Dim Partners As Boolean = False
    Dim Formula As Boolean = False
    Dim Returns As Boolean = False
    Dim Coupons As Boolean = False
    Dim Updates As Boolean = False
    Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TabControl1.TabPages(0).Text = "General Settings"
        TabControl1.TabPages(1).Text = "Partners Transaction"
        TabControl1.TabPages(2).Text = "Formula"
        TabControl1.TabPages(3).Text = "Item Refund"
        TabControl1.TabPages(4).Text = "Coupon Settings"
        TabControl1.TabPages(5).Text = "Updates"
        TabControl2.TabPages(0).Text = "Connection Settings"
        TabControl2.TabPages(1).Text = "Additional Settings"
        LoadConn()
        LoadCloudConn()
        LoadAdditionalSettings()
        LoadDevInfo()
        LoadAutoBackup()
    End Sub
    Private Sub SettingsForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        POS.Enabled = True
    End Sub
    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedIndex = 1 Then
            If Partners = False Then
                TabControl3.TabPages(0).Text = "Available Partners"
                TabControl3.TabPages(1).Text = "Deactivated Partners"
                LoadPartners()
                LoadPartnersDeact()
                Partners = True
            End If
        ElseIf TabControl1.SelectedIndex = 2 Then
            If Formula = False Then
                loadformula()
                Formula = True
            End If
        ElseIf TabControl1.SelectedIndex = 3 Then
            If Returns = False Then
                loaditemreturn(True)
                loadindexdgv()
                Returns = True
            End If
        ElseIf TabControl1.SelectedIndex = 4 Then
            If Coupons = False Then
                loaddatagrid1()
                loaddatagrid2()
                Coupons = True
            End If
        ElseIf TabControl1.SelectedIndex = 5 Then
            If My.Settings.Updatedatetime = "" Then
                LabelCheckingUpdates.Text = "last Checked: 2020-06-01 11:12:30"
            Else
                LabelCheckingUpdates.Text = "last Checked: " & My.Settings.Updatedatetime
            End If
        End If
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            If CheckForInternetConnection() = True Then

                If POS.POSISUPDATING = False Then
                    Timer1.Start()
                    DataGridView5.Rows.Clear()
                    Dim Products = count("product_id", "loc_admin_products")
                    Dim Category = count("category_id", "loc_admin_category")
                    Dim Inventory = count("inventory_id", "loc_pos_inventory")
                    Dim Formula = count("formula_id", "loc_product_formula")

                    DataGridView5.Rows.Add(Products)
                    DataGridView5.Rows.Add(Category)
                    DataGridView5.Rows.Add(Inventory)
                    DataGridView5.Rows.Add(Formula)

                    LabelCountAllRows.Text = SumOfColumnsToInt(DataGridView5, 0)
                    LabelCheckingUpdates.Text = "Checking for updates."
                    ProgressBar1.Maximum = Val(LabelCountAllRows.Text)
                    ProgressBar1.Value = 0
                    LabelNewRows.Text = 0
                    BackgroundWorker1.WorkerReportsProgress = True
                    BackgroundWorker1.WorkerSupportsCancellation = True
                    BackgroundWorker1.RunWorkerAsync()
                    Button4.Enabled = False
                Else
                    MsgBox("Updates is still on process please wait.")
                End If
            Else
                MsgBox("Internet connection is not available")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If LabelCheckingUpdates.Text = "Checking for updates." Then
            LabelCheckingUpdates.Text = "Checking for updates.."
        ElseIf LabelCheckingUpdates.Text = "Checking for updates.." Then
            LabelCheckingUpdates.Text = "Checking for updates..."
        ElseIf LabelCheckingUpdates.Text = "Checking for updates..." Then
            LabelCheckingUpdates.Text = "Checking for updates"
        ElseIf LabelCheckingUpdates.Text = "Checking for updates" Then
            LabelCheckingUpdates.Text = "Checking for updates."
        End If
    End Sub
#Region "Partners"

    Public Sub LoadPartners()
        GLOBAL_SELECT_ALL_FUNCTION("loc_partners_transaction WHERE active = 1 ORDER BY arrid ASC", "*", DataGridViewPartners)
        With DataGridViewPartners
            .Columns(0).Visible = False
            .Columns(1).Visible = False
            .Columns(2).HeaderText = "Bank Name"
            .Columns(3).HeaderText = "Date Modified"
            .Columns(4).HeaderText = "Service Crew ID"
            .Columns(5).Visible = False
            .Columns(6).Visible = False
            .Columns(7).HeaderText = "Status"
            .Columns(8).Visible = False
            'BUG
            'Change Status to Active

        End With

    End Sub
    Public Sub LoadPartnersDeact()
        GLOBAL_SELECT_ALL_FUNCTION("loc_partners_transaction WHERE active = 0 ORDER BY arrid ASC", "*", DataGridViewPartnersDeact)
        With DataGridViewPartnersDeact
            .Columns(0).Visible = False
            .Columns(1).Visible = False
            .Columns(2).HeaderText = "Bank Name"
            .Columns(3).HeaderText = "Date Modified"
            .Columns(4).HeaderText = "Service Crew ID"
            .Columns(5).Visible = False
            .Columns(6).Visible = False
            .Columns(7).HeaderText = "Status"
            .Columns(8).Visible = False
            'BUG
            'Change Status to Deactivated
        End With
    End Sub
    Private Sub ButtonDeleteProducts_Click(sender As Object, e As EventArgs) Handles ButtonDeleteProducts.Click
        Try
            If DataGridViewPartners.SelectedRows.Count < 1 Then
                MsgBox("Select bank first")
            ElseIf DataGridViewPartners.SelectedRows.Count > 1 Then
                MsgBox("Select one bank at a time")
            Else
                Dim BankID = DataGridViewPartners.SelectedRows(0).Cells(0).Value.ToString
                GLOBAL_FUNCTION_UPDATE("loc_partners_transaction", "active = 0", "id = " & BankID)
                LoadPartners()
                LoadPartnersDeact()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles ButtonPTActivate.Click
        Try
            If DataGridViewPartnersDeact.SelectedRows.Count < 1 Then
                MsgBox("Select bank first")
            ElseIf DataGridViewPartnersDeact.SelectedRows.Count > 1 Then
                MsgBox("Select one bank at a time")
            Else
                Dim BankID = DataGridViewPartnersDeact.SelectedRows(0).Cells(0).Value.ToString
                GLOBAL_FUNCTION_UPDATE("loc_partners_transaction", "active = 1", "id = " & BankID)
                LoadPartners()
                LoadPartnersDeact()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If DataGridViewPartners.SelectedRows.Count < 1 Then
                MsgBox("Select bank first")
            ElseIf DataGridViewPartners.SelectedRows.Count > 1 Then
                MsgBox("Select one bank at a time")
            Else
                Dim ID = GLOBAL_RETURN_FUNCTION("loc_partners_transaction WHERE arrid = 1", "bankname", "bankname", True)
                Dim Arrid = GLOBAL_RETURN_FUNCTION("loc_partners_transaction WHERE id = " & DataGridViewPartners.SelectedRows(0).Cells(0).Value, "bankname", "bankname", True)
                table = "loc_partners_transaction"
                fields = "arrid = 1, synced = 'Unsynced'"
                where = "bankname = '" & Arrid & "'"
                GLOBAL_FUNCTION_UPDATE(table, fields, where)
                fields = "arrid = " & DataGridViewPartners.SelectedRows(0).Cells(1).Value & ", synced = 'Unsynced'"
                where = "bankname = '" & ID & "'"
                GLOBAL_FUNCTION_UPDATE(table, fields, where)
                LoadPartners()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        AddOrEdit = True
        Enabled = False
        AddBank.Show()
    End Sub
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If DataGridViewPartners.SelectedRows.Count < 1 Then
            MsgBox("Select bank first")
        ElseIf DataGridViewPartners.SelectedRows.Count > 1 Then
            MsgBox("Select one bank at a time")
        Else
            AddOrEdit = False
            AddBank.TextBoxBankName.Text = DataGridViewPartners.SelectedRows(0).Cells(2).Value.ToString
            Enabled = False
            AddBank.Show()
        End If
    End Sub
#End Region
#Region "Formula"
    Public Sub loadformula()
        fields = "`product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, ROUND(`serving_value`,0) as servval, ROUND(`no_servings`,0) as noofservings"
        GLOBAL_SELECT_ALL_FUNCTION(table:="loc_product_formula WHERE status = 1 AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' ", datagrid:=DataGridViewFormula, fields:=fields)
        With DataGridViewFormula
            .Columns(0).HeaderText = "Ingredients"
            .Columns(1).HeaderText = "Primary Unit"
            .Columns(2).HeaderText = "Primary (Value)"
            .Columns(3).HeaderText = "Secondary Unit"
            .Columns(4).HeaderText = "Secondary (Value)"
            .Columns(5).HeaderText = "Srv. Unit"
            .Columns(6).HeaderText = "Srv. (Value)"
            .Columns(7).HeaderText = "No. of Serving"
            .Columns(0).Width = 150
            .Columns(2).Width = 70
        End With
    End Sub
#End Region
#Region "Returns"
    Private Sub rowindex()
        LabelITEMRET.Text = DataGridViewITEMRETURN1.CurrentCell.RowIndex
    End Sub
    Private Sub loaditemreturn(justload As Boolean)
        Try
            fields = "`transaction_number`, `amounttendered`, `totaldiscount`, `change`, `crew_id`, `vatablesales`, `vatexemptsales`, `zeroratedsales`, `lessvat`, `transaction_type`"
            If justload = True Then
                table = "`loc_daily_transaction` WHERE Date(created_at) = '" & S_Zreading & "' And `active` = 1 ORDER BY `transaction_id` DESC"
                GLOBAL_SELECT_ALL_FUNCTION(table, fields, DataGridViewITEMRETURN1)
            Else
                If String.IsNullOrWhiteSpace(TextBoxSearchTranNumber.Text) Then
                    FlowLayoutPanel1.Controls.Clear()
                    GLOBAL_SELECT_ALL_FUNCTION(table:="`loc_daily_transaction` WHERE Date(created_at) = '" & S_Zreading & "' And `active` = 1 ORDER BY `transaction_id` DESC", datagrid:=DataGridViewITEMRETURN1, fields:=fields)
                Else
                    FlowLayoutPanel1.Controls.Clear()
                    GLOBAL_SELECT_ALL_FUNCTION(table:="`loc_daily_transaction` WHERE `transaction_number` Like '%" & TextBoxSearchTranNumber.Text & "%'  AND date(created_at) = '" & S_Zreading & "' AND `active` = 1 ORDER BY `transaction_id` DESC", datagrid:=DataGridViewITEMRETURN1, fields:=fields)
                End If
            End If
            With DataGridViewITEMRETURN1
                .Columns(0).HeaderText = "Reference #"
                .Columns(1).HeaderText = "Cash"
                .Columns(2).HeaderText = "Discount"
                .Columns(3).HeaderText = "Change"
                .Columns(4).HeaderText = "Crew"
                .Columns(0).Width = 100
                .Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                .Columns(5).Visible = False
                .Columns(6).Visible = False
                .Columns(7).Visible = False
                .Columns(8).Visible = False
                .Columns(9).Visible = False
                For Each row As DataRow In dt.Rows
                    row("crew_id") = returnfullname(row("crew_id"))
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub DataGridViewITEMRETURN1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewITEMRETURN1.CellClick
        rowindex()
        loadtransactions()
    End Sub
    Dim productimage As String
    Dim grandtotal As Decimal
    Private Sub loadtransactions()
        Try
            Dim countrow As Integer = 0
            FlowLayoutPanel1.Controls.Clear()
            Dim sql = "SELECT product_id, product_name, quantity, price, total, product_sku FROM loc_daily_transaction_details WHERE transaction_number = '" & DataGridViewITEMRETURN1.SelectedRows(0).Cells(0).Value.ToString & "' AND active = 1"
            Dim query As String = "SELECT amountdue FROM loc_daily_transaction WHERE transaction_number = '" & DataGridViewITEMRETURN1.SelectedRows(0).Cells(0).Value.ToString & "'"
            Dim cmdquery As MySqlCommand = New MySqlCommand(query, LocalhostConn())
            Dim queryda As MySqlDataAdapter = New MySqlDataAdapter(cmdquery)
            Dim dt1 As DataTable = New DataTable
            queryda.Fill(dt1)
            Using readerObj As MySqlDataReader = cmdquery.ExecuteReader
                While readerObj.Read
                    grandtotal = readerObj("amountdue")
                End While
            End Using
            cmd = New MySqlCommand
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable()
            With cmd
                .CommandText = sql
                .Connection = LocalhostConn()
                da.Fill(dt)
                For Each row As DataRow In dt.Rows
                    countrow += 1
                    Dim buttonname As String = row("product_name")
                    sql = "select product_image from loc_admin_products where product_id = " & row("product_id")
                    cmd = New MySqlCommand
                    With cmd
                        .CommandText = sql
                        .Connection = LocalhostConn()
                        Using readerobj As MySqlDataReader = cmd.ExecuteReader
                            While readerobj.Read
                                productimage = readerobj("product_image")
                            End While
                        End Using
                    End With
                    Dim drawproduct As New Panel
                    Dim myname As New Label
                    Dim myqty As New Label
                    Dim myprice As New Label
                    Dim mytotal As New Label
                    Dim myimage As New Button
                    With drawproduct
                        .Name = buttonname
                        .Text = buttonname
                        .BorderStyle = BorderStyle.None
                        .ForeColor = Color.White
                        .Font = New Font("kelson sans normal", 10)
                        .Width = 345
                        .Height = 140
                        .Cursor = Cursors.Hand
                        With myname
                            .Location = New Point(200, 10)
                            .Text = row("product_sku")
                            .ForeColor = Color.Black
                            .Font = New Font("kelson sans normal", 10, FontStyle.Bold)
                            .Width = 200
                        End With
                        With myqty
                            .Font = New Font("kelson sans normal", 10)
                            .Location = New Point(200, 35)
                            .Text = "quantity: " & row("quantity")
                            .ForeColor = Color.Black
                            .Width = 200
                        End With
                        With myprice
                            .Font = New Font("kelson sans normal", 10)
                            .Location = New Point(200, 60)
                            .Text = "price: " & row("price")
                            .ForeColor = Color.Black
                            .Width = 200
                        End With
                        With mytotal
                            .Font = New Font("kelson sans normal", 10)
                            .Location = New Point(200, 85)
                            .Text = "total: " & row("total")
                            .ForeColor = Color.Black
                            .Width = 200
                        End With
                        With myimage
                            .Location = New Point(5, 5)
                            .Width = 166.5
                            .Height = 120
                            .BackgroundImage = Base64ToImage(productimage)
                            .BackgroundImageLayout = ImageLayout.Stretch
                            .FlatStyle = FlatStyle.Flat
                            .FlatAppearance.BorderSize = 0
                            .BackgroundImageLayout = ImageLayout.Stretch
                        End With
                    End With
                    drawproduct.Controls.Add(myname)
                    drawproduct.Controls.Add(myqty)
                    drawproduct.Controls.Add(myprice)
                    drawproduct.Controls.Add(mytotal)
                    drawproduct.Controls.Add(myimage)
                    FlowLayoutPanel1.Controls.Add(drawproduct)
                Next
                With DataGridViewITEMRETURN1
                    LabelIRTRANSNUM.Text = .SelectedRows(0).Cells(0).Value.ToString
                    LabelIRSUBTOTAL.Text = countrow & " item(s)"
                    LabelIRTYPE.Text = .SelectedRows(0).Cells(9).Value.ToString
                    LabelIRTOTAL.Text = grandtotal
                    LabelIRCASH.Text = .SelectedRows(0).Cells(1).Value.ToString
                    LabelIRCHANGE.Text = .SelectedRows(0).Cells(3).Value.ToString
                    LabelIRDISC.Text = .SelectedRows(0).Cells(2).Value.ToString
                    LabelIRVAT.Text = .SelectedRows(0).Cells(5).Value.ToString
                    LabelIRVATEX.Text = .SelectedRows(0).Cells(6).Value.ToString
                    LabelIRZERO.Text = .SelectedRows(0).Cells(7).Value.ToString
                    LabelIRINPUTVAT.Text = .SelectedRows(0).Cells(8).Value.ToString
                    ButtonRefund.Text = "Refund php " & grandtotal
                End With
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub TextBoxSearchTranNumber_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSearchTranNumber.TextChanged
        loaditemreturn(False)
        If DataGridViewITEMRETURN1.Rows.Count > 0 Then
            loadtransactions()
        End If
    End Sub
    Public Sub loadindexdgv()
        If DataGridViewITEMRETURN1.RowCount > 0 Then
            DataGridViewITEMRETURN1.ClearSelection()
            DataGridViewITEMRETURN1.CurrentCell = DataGridViewITEMRETURN1.Rows(Val(LabelITEMRET.Text)).Cells(0)
            DataGridViewITEMRETURN1.Rows(Val(LabelITEMRET.Text)).Selected = True
            loadtransactions()
        Else
            FlowLayoutPanel1.Controls.Clear()
        End If
    End Sub
    Private Sub ButtonRefund_Click(sender As Object, e As EventArgs) Handles ButtonRefund.Click
        Try
            If DataGridViewITEMRETURN1.Rows.Count > 0 Then
                Dim transaction_num As String = DataGridViewITEMRETURN1.SelectedRows(0).Cells(0).Value.ToString
                If String.IsNullOrWhiteSpace(TextBoxIRREASON.Text) Then
                    MessageBox.Show("Reason for refund is required!", "Refund", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    sql = "SELECT * FROM loc_daily_transaction WHERE date(created_at) = date(CURDATE()) AND created_at >= Now() - INTERVAL 10 MINUTE AND transaction_number = '" & transaction_num & "'"
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                    Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                    Dim dt As DataTable = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        Dim refund As Integer = MessageBox.Show("Are you sure do you want to refund this transaction?", "Return and Refund", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                        If refund = DialogResult.Yes Then
                            INSERTRETURNS(transaction_num)
                        End If
                    Else
                        MessageBox.Show("This transaction is non refundable or exceed's the given period of time.", "Refund", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If
            Else
                MsgBox("Create transaction first")
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub INSERTRETURNS(transaction_num As String)
        Try
            fields = "(`transaction_number`, `reason`, `total`, `guid`, `store_id`, `crew_id`, `synced`, `zreading`, `created_at`)"
            value = "('" & transaction_num & "'
                            , '" & TextBoxIRREASON.Text & "'
                            , " & grandtotal & "
                            , '" & ClientGuid & "'
                            , '" & ClientStoreID & "'
                            , '" & ClientCrewID & "'
                            , 'Unsynced'
                            , '" & S_Zreading & "'
                            , '" & FullDate24HR() & "')"
            GLOBAL_INSERT_FUNCTION(table:="`loc_refund_return_details`", fields:=fields, values:=value)
            GLOBAL_FUNCTION_UPDATE("loc_daily_transaction", "active = 2 , synced = 'Unsynced'", "transaction_number = '" & transaction_num & "'")
            GLOBAL_FUNCTION_UPDATE("loc_daily_transaction_details", "active = 2 , synced = 'Unsynced'", "transaction_number = '" & transaction_num & "'")
            GLOBAL_SYSTEM_LOGS("RETURN", "Reason: " & TextBoxIRREASON.Text & " Trn.Number: " & transaction_num & " Total Amount: " & grandtotal)
            LabelITEMRET.Text = 0
            loaditemreturn(True)
            loadindexdgv()
            TextBoxSearchTranNumber.Clear()
            TextBoxIRREASON.Clear()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
#End Region
#Region "Coupons"
    Private Sub ComboBox1_TextChanged(sender As Object, e As EventArgs) Handles ComboBoxCType.TextChanged
        If ComboBoxCType.Text = "Percentage" Then
            TextBoxCDVal.Enabled = True
            TextBoxCRefVal.Enabled = False
            TextBoxCBBP.Enabled = False
            TextBoxCBV.Enabled = False
            TextBoxCBP.Enabled = False
            TextBoxCBundVal.Enabled = False
            For Each a In Panel19.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        ElseIf ComboBoxCType.Text = "Fix-1" Then
            TextBoxCDVal.Enabled = True
            TextBoxCRefVal.Enabled = False
            TextBoxCBBP.Enabled = False
            TextBoxCBV.Enabled = False
            TextBoxCBP.Enabled = False
            TextBoxCBundVal.Enabled = False
            For Each a In Panel19.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        ElseIf ComboBoxCType.Text = "Fix-2" Then
            TextBoxCDVal.Enabled = True
            TextBoxCRefVal.Enabled = True
            TextBoxCBBP.Enabled = False
            TextBoxCBV.Enabled = False
            TextBoxCBP.Enabled = False
            TextBoxCBundVal.Enabled = False
            For Each a In Panel19.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        ElseIf ComboBoxCType.Text = "Bundle-1(Fix)" Then
            TextBoxCDVal.Enabled = False
            TextBoxCRefVal.Enabled = False
            TextBoxCBBP.Enabled = True
            TextBoxCBV.Enabled = True
            TextBoxCBP.Enabled = True
            TextBoxCBundVal.Enabled = True
            For Each a In Panel19.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        ElseIf ComboBoxCType.Text = "Bundle-2(Fix)" Then
            TextBoxCDVal.Enabled = True
            TextBoxCRefVal.Enabled = False
            TextBoxCBBP.Enabled = True
            TextBoxCBV.Enabled = True
            TextBoxCBP.Enabled = True
            TextBoxCBundVal.Enabled = True
            For Each a In Panel19.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        ElseIf ComboBoxCType.Text = "Bundle-3(%)" Then
            TextBoxCDVal.Enabled = True
            TextBoxCRefVal.Enabled = False
            TextBoxCBBP.Enabled = True
            TextBoxCBV.Enabled = True
            TextBoxCBP.Enabled = True
            TextBoxCBundVal.Enabled = True
            For Each a In Panel19.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        Else
            TextBoxCDVal.Enabled = True
            TextBoxCRefVal.Enabled = True
            TextBoxCBBP.Enabled = True
            TextBoxCBV.Enabled = True
            TextBoxCBP.Enabled = True
            TextBoxCBundVal.Enabled = True
            For Each a In Panel19.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        End If
    End Sub
    Private Sub loaddatagrid1()
        Try
            DataGridViewCProductList.Columns.Clear()
            GLOBAL_SELECT_ALL_FUNCTION("loc_admin_products", "product_id, product_name, product_category", DataGridViewCProductList)
            With DataGridViewCProductList
                .Columns(0).HeaderText = "Product ID"
                .Columns(1).HeaderText = "Product Name"
                .Columns(2).HeaderText = "Category"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub loaddatagrid2()
        Try
            GLOBAL_SELECT_ALL_FUNCTION(table:="tbcoupon", fields:="*", datagrid:=DataGridViewCouponList)
            With DataGridViewCouponList
                .Columns(0).Visible = False
                .Columns(3).Visible = False
                .Columns(4).Visible = False
                .Columns(6).Visible = False
                .Columns(7).Visible = False
                .Columns(8).Visible = False
                .Columns(9).Visible = False
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub SaveCoupon()
        Try
            value = "('" & TextBoxCName.Text & "' , '" & TextBoxCDesc.Text & "', '" & TextBoxCDVal.Text & "', '" & TextBoxCRefVal.Text & "', '" & ComboBoxCType.Text & "' , '" & TextBoxCBBP.Text & "' , '" & TextBoxCBV.Text & "', '" & TextBoxCBP.Text & "', '" & TextBoxCBundVal.Text & "', '" & CDate(DateTimePickerCEffectiveDate.Value).ToShortDateString & "' , '" & CDate(DateTimePickerCExpiryDate.Value).ToShortDateString & "')"
            GLOBAL_INSERT_FUNCTION("tbcoupon", "(`Couponname_`, `Desc_`, `Discountvalue_`, `Referencevalue_`, `Type`, `Bundlebase_`, `BBValue_`, `Bundlepromo_`, `BPValue_`, `Effectivedate`, `Expirydate`)", value)
            GLOBAL_SYSTEM_LOGS("NEW COUPON", "Name : " & TextBoxCName.Text & " Type : " & ComboBoxCType.Text)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        If TextboxIsEmpty(Panel19) = True Then
            SaveCoupon()
            loaddatagrid2()
        Else
            MsgBox("Fill up all blanks")
        End If
    End Sub

    Private Sub DataGridViewCProductList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewCProductList.CellClick
        Try
            If TextBoxCBBP.Enabled = False Then
                Exit Sub
            Else
                If TextBoxCBBP.Text = String.Empty Then
                    TextBoxCBBP.Text = Me.DataGridViewCProductList.Item(0, Me.DataGridViewCProductList.CurrentRow.Index).Value
                    '  Dim newString As String
                    '   newString = deleteDup(TextBox5.Text, TextBox5.Text)

                    '   TextBox5.Text = newString
                Else
                    TextBoxCBBP.Text = TextBoxCBBP.Text & "," & Me.DataGridViewCProductList.Item(0, Me.DataGridViewCProductList.CurrentRow.Index).Value
                    ' Dim newString As String
                    '  newString = deleteDup(TextBox5.Text, TextBox5.Text)

                    ' TextBox5.Text = newString
                End If
            End If

            If TextBoxCBP.Enabled = False Then
                Exit Sub
            Else
                If TextBoxCBP.Text = String.Empty Then
                    TextBoxCBP.Text = Me.DataGridViewCProductList.Item(0, Me.DataGridViewCProductList.CurrentRow.Index).Value
                Else
                    TextBoxCBP.Text = TextBoxCBP.Text & "," & Me.DataGridViewCProductList.Item(0, Me.DataGridViewCProductList.CurrentRow.Index).Value
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub


#End Region
#Region "Load"
    Private Sub LoadConn()
        Try
            If My.Settings.LocalConnectionPath <> "" Then
                If System.IO.File.Exists(My.Settings.LocalConnectionPath) Then
                    'The File exists 
                    Dim CreateConnString As String = ""
                    Dim filename As String = String.Empty
                    Dim TextLine As String = ""
                    Dim objReader As New System.IO.StreamReader(My.Settings.LocalConnectionPath)
                    Dim lineCount As Integer
                    Do While objReader.Peek() <> -1
                        TextLine = objReader.ReadLine()
                        If lineCount = 0 Then
                            TextBoxLocalServer.Text = ConvertB64ToString(RemoveCharacter(TextLine, "server="))
                        End If
                        If lineCount = 1 Then
                            TextBoxLocalUsername.Text = ConvertB64ToString(RemoveCharacter(TextLine, "user id="))
                        End If
                        If lineCount = 2 Then
                            TextBoxLocalPassword.Text = ConvertB64ToString(RemoveCharacter(TextLine, "password="))
                        End If
                        If lineCount = 3 Then
                            TextBoxLocalDatabase.Text = ConvertB64ToString(RemoveCharacter(TextLine, "database="))
                        End If
                        If lineCount = 4 Then
                            TextBoxLocalPort.Text = ConvertB64ToString(RemoveCharacter(TextLine, "port="))
                        End If
                        lineCount = lineCount + 1
                    Loop
                    objReader.Close()
                End If
            Else
                Dim path2 = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Innovention\user.config"
                If System.IO.File.Exists(path2) Then
                    'The File exists 
                    Dim ConnStr
                    Dim ConnStr2 = ""
                    Dim CreateConnString As String = ""
                    Dim filename As String = String.Empty
                    Dim TextLine As String = ""
                    Dim objReader As New System.IO.StreamReader(path2)
                    Dim lineCount As Integer
                    Do While objReader.Peek() <> -1
                        TextLine = objReader.ReadLine()
                        If lineCount = 0 Then
                            ConnStr = ConvertB64ToString(RemoveCharacter(TextLine, "server="))
                            ConnStr2 = "server=" & ConnStr
                        End If
                        If lineCount = 1 Then
                            ConnStr = ConvertB64ToString(RemoveCharacter(TextLine, "user id="))
                            ConnStr2 += ";user id=" & ConnStr
                        End If
                        If lineCount = 2 Then
                            ConnStr = ConvertB64ToString(RemoveCharacter(TextLine, "password="))
                            ConnStr2 += ";password=" & ConnStr
                        End If
                        If lineCount = 3 Then
                            ConnStr = ConvertB64ToString(RemoveCharacter(TextLine, "database="))
                            ConnStr2 += ";database=" & ConnStr
                        End If
                        If lineCount = 4 Then
                            ConnStr = ConvertB64ToString(RemoveCharacter(TextLine, "port="))
                            ConnStr2 += ";port=" & ConnStr
                        End If
                        If lineCount = 5 Then
                            ConnStr2 += ";" & TextLine
                        End If
                        lineCount = lineCount + 1
                    Loop
                    LocalConnectionString = ConnStr2
                    objReader.Close()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadCloudConn()
        Try
            If ValidLocalConnection = True Then
                sql = "SELECT C_Server, C_Username, C_Password, C_Database, C_Port FROM loc_settings WHERE settings_id = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    TextBoxCloudServer.Text = ConvertB64ToString(dt(0)(0))
                    TextBoxCloudUsername.Text = ConvertB64ToString(dt(0)(1))
                    TextBoxCloudPassword.Text = ConvertB64ToString(dt(0)(2))
                    TextBoxCloudDatabase.Text = ConvertB64ToString(dt(0)(3))
                    TextBoxCloudPort.Text = ConvertB64ToString(dt(0)(4))
                    ValidLocalConnection = True
                Else
                    ValidLocalConnection = False
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadAdditionalSettings()
        Try
            If ValidLocalConnection = True Then
                sql = "SELECT A_Export_Path, A_Tax, A_SIFormat, A_Terminal_No, A_ZeroRated FROM loc_settings WHERE settings_id = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If dt(0)(0) <> Nothing Then
                        TextBoxExportPath.Text = ConvertB64ToString(dt(0)(0))
                    End If
                    If dt(0)(1) <> Nothing Then
                        TextBoxTax.Text = dt(0)(1) * 100
                    End If
                    If dt(0)(2) <> Nothing Then
                        TextBoxSINumber.Text = dt(0)(2)
                    End If
                    If dt(0)(3) <> Nothing Then
                        TextBoxTerminalNo.Text = dt(0)(3)
                    End If
                    If dt(0)(4) <> Nothing Then
                        If dt(0)(4) = 0 Then
                            RadioButtonNO.Checked = True
                        ElseIf dt(0)(4) = 1 Then
                            RadioButtonYES.Checked = True
                        End If
                    End If
                End If
                'For i As Integer = 0 To dt.Rows.Count - 1 Step +1
                '    If dt(i)(0) = "" Then
                '        My.Settings.ValidAddtionalSettings = False
                '        Exit For
                '    ElseIf dt(i)(1) = "" Then
                '        My.Settings.ValidAddtionalSettings = False
                '        Exit For
                '    ElseIf dt(i)(2) = "" Then
                '        My.Settings.ValidAddtionalSettings = False
                '        Exit For
                '    ElseIf dt(i)(3) = "" Then
                '        My.Settings.ValidAddtionalSettings = False
                '        Exit For
                '    ElseIf dt(i)(4) = "" Then
                '        My.Settings.ValidAddtionalSettings = False
                '        Exit For
                '    Else
                '        My.Settings.ValidAddtionalSettings = True
                '        Exit For
                '    End If
                'Next
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadDevInfo()
        Try
            If ValidLocalConnection = True Then
                sql = "SELECT Dev_Company_Name, Dev_Address, Dev_Tin, Dev_Accr_No, Dev_Accr_Date_Issued, Dev_Accr_Valid_Until, Dev_PTU_No, Dev_PTU_Date_Issued, Dev_PTU_Valid_Until FROM loc_settings WHERE settings_id = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If dt(0)(0) <> Nothing Then
                        TextBoxDevname.Text = dt(0)(0)
                    End If
                    If dt(0)(1) <> Nothing Then
                        TextBoxDevAdd.Text = dt(0)(1)
                    End If
                    If dt(0)(2) <> Nothing Then
                        TextBoxDevTIN.Text = dt(0)(2)
                    End If
                    If dt(0)(3) <> Nothing Then
                        TextBoxDevAccr.Text = dt(0)(3)
                    End If
                    If dt(0)(4) <> Nothing Then
                        DateTimePicker1ACCRDI.Value = dt(0)(4)
                    End If
                    If dt(0)(5) <> Nothing Then
                        DateTimePicker2ACCRVU.Value = dt(0)(5)
                    End If
                    If dt(0)(6) <> Nothing Then
                        TextBoxDEVPTU.Text = dt(0)(6)
                    End If
                    If dt(0)(7) <> Nothing Then
                        DateTimePicker4PTUDI.Value = dt(0)(7)
                    End If
                    If dt(0)(8) <> Nothing Then
                        DateTimePickerPTUVU.Value = dt(0)(8)
                    End If
                End If
                'For i As Integer = 0 To dt.Rows.Count - 1 Step +1
                '    If dt(i)(0) = "" Then
                '        My.Settings.ValidDevSettings = False
                '        My.Settings.Save()
                '        Exit For
                '    ElseIf dt(i)(1) = "" Then
                '        My.Settings.ValidDevSettings = False
                '        My.Settings.Save()
                '        Exit For
                '    ElseIf dt(i)(2) = "" Then
                '        My.Settings.ValidDevSettings = False
                '        My.Settings.Save()
                '        Exit For
                '    ElseIf dt(i)(3) = "" Then
                '        My.Settings.ValidDevSettings = False
                '        My.Settings.Save()
                '        Exit For
                '    ElseIf dt(i)(4) = "" Then
                '        My.Settings.ValidDevSettings = False
                '        My.Settings.Save()
                '        Exit For
                '    ElseIf dt(i)(5) = "" Then
                '        My.Settings.ValidDevSettings = False
                '        My.Settings.Save()
                '        Exit For
                '    ElseIf dt(i)(6) = "" Then
                '        My.Settings.ValidDevSettings = False
                '        My.Settings.Save()
                '        Exit For
                '    ElseIf dt(i)(7) = "" Then
                '        My.Settings.ValidDevSettings = False
                '        My.Settings.Save()
                '        Exit For
                '    ElseIf dt(i)(8) = "" Then
                '        My.Settings.ValidDevSettings = False
                '        My.Settings.Save()
                '        Exit For
                '    Else
                '        My.Settings.ValidDevSettings = True
                '        My.Settings.Save()
                '    End If
                'Next
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub LoadAutoBackup()
        Try
            If ValidLocalConnection = True Then
                sql = "SELECT S_BackupInterval, S_BackupDate FROM loc_settings WHERE settings_id = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    If dt(0)(0).ToString = "1" Then
                        RadioButtonDaily.Checked = True
                        '=================================
                        RadioButtonWeekly.Enabled = False
                        RadioButtonMonthly.Enabled = False
                        RadioButtonYearly.Enabled = False
                    ElseIf dt(0)(0).ToString = "2" Then
                        RadioButtonWeekly.Checked = True
                        '=================================
                        RadioButtonDaily.Enabled = False
                        RadioButtonMonthly.Enabled = False
                        RadioButtonYearly.Enabled = False
                    ElseIf dt(0)(0).ToString = "3" Then
                        RadioButtonMonthly.Checked = True
                        '=================================
                        RadioButtonDaily.Enabled = False
                        RadioButtonWeekly.Enabled = False
                        RadioButtonYearly.Enabled = False
                    ElseIf dt(0)(0).ToString = "4" Then
                        RadioButtonYearly.Checked = True
                        '=================================
                        RadioButtonDaily.Enabled = False
                        RadioButtonWeekly.Enabled = False
                        RadioButtonMonthly.Enabled = False
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub ButtonExport_Click(sender As Object, e As EventArgs) Handles ButtonExport.Click
        If ClientRole = "Crew" Then
            MsgBox("You dont have administrator rights.")
        Else
            BackupDatabase()
        End If
    End Sub
    Private Sub BackupDatabase()
        Try
            Dim DatabaseName = "\" & TextBoxLocalDatabase.Text & Format(Now(), "yyyy-MM-dd") & ".sql"
            Process.Start("cmd.exe", "/k cd C:\xampp\mysql\bin & mysqldump --databases -h " & TextBoxLocalServer.Text & " -u " & TextBoxLocalUsername.Text & " -p " & TextBoxLocalPassword.Text & " " & TextBoxLocalDatabase.Text & " > """ & TextBoxExportPath.Text & DatabaseName & """")
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub ButtonImport_Click(sender As Object, e As EventArgs) Handles ButtonImport.Click
        If ClientRole = "Crew" Then
            MsgBox("You dont have administrator rights.")
        Else
            If (OpenFileDialog1.ShowDialog = DialogResult.OK) Then
                TextBoxLocalRestorePath.Text = OpenFileDialog1.FileName
            End If
        End If
    End Sub
    Private Function Connect() As MySqlConnection
        Dim con As MySqlConnection = New MySqlConnection
        Try
            con.ConnectionString = "server=" & TextBoxLocalServer.Text & ";user name=" & TextBoxLocalUsername.Text & ";password=" & TextBoxLocalPassword.Text
            con.Open()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return con
    End Function
    Private Sub RestoreDatabase()
        Try
            Dim sql = "CREATE DATABASE /*!32312 IF NOT EXISTS*/ `" & TextBoxLocalDatabase.Text & "` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;USE `" & TextBoxLocalDatabase.Text & "`;"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, Connect)
            cmd.ExecuteNonQuery()
            Process.Start("cmd.exe", "/k cd C:\xampp\mysql\bin & mysql -h " & TextBoxLocalServer.Text & " -u " & TextBoxLocalUsername.Text & " -p " & TextBoxLocalPassword.Text & " " & TextBoxLocalDatabase.Text & " < """ & TextBoxLocalRestorePath.Text & """")
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        TextBoxLocalRestorePath.Text = System.IO.Path.GetFullPath(OpenFileDialog1.FileName)
        RestoreDatabase()
    End Sub
    Private Sub ButtonMaintenance_Click(sender As Object, e As EventArgs) Handles ButtonMaintenance.Click
        If ClientRole = "Crew" Then
            MsgBox("You dont have administrator rights.")
        Else
            RepairDatabase()
        End If

    End Sub
    Private Sub RepairDatabase()
        Try
            Process.Start("cmd.exe", "/k cd C:\xampp\mysql\bin & mysqlcheck -h " & TextBoxLocalServer.Text & " -u " & TextBoxLocalUsername.Text & " -p " & TextBoxLocalPassword.Text & " --auto-repair -c --databases " & TextBoxLocalDatabase.Text)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles ButtonOptimizeDB.Click
        If ClientRole = "Crew" Then
            MsgBox("You dont have administrator rights.")
        Else
            OptimizeDatabase()
        End If
    End Sub
    Private Sub OptimizeDatabase()
        Try
            Process.Start("cmd.exe", "/k cd C:\xampp\mysql\bin & mysqlcheck -h " & TextBoxLocalServer.Text & " -u " & TextBoxLocalUsername.Text & " -p " & TextBoxLocalPassword.Text & " -o --databases " & TextBoxLocalDatabase.Text)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub ButtonDatabaseReset_Click(sender As Object, e As EventArgs) Handles ButtonDatabaseReset.Click
        If ClientRole = "Crew" Then
            MsgBox("You dont have administrator rights.")
        Else

        End If
    End Sub
    Private Function TestDBConnection(server, username, password, database, port) As MySqlConnection
        Dim testcon As MySqlConnection = New MySqlConnection
        Try
            testcon.ConnectionString = "server=" & server & ";user id=" & username & ";password=" & password & ";database=" & database & ";port=" & port & ""
            testcon.Open()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return testcon
    End Function
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles ButtonTestLocCon.Click
        Try
            Dim Con = TestDBConnection(TextBoxLocalServer.Text, TextBoxLocalUsername.Text, TextBoxLocalPassword.Text, TextBoxLocalDatabase.Text, TextBoxLocalPort.Text)
            If Con.State = ConnectionState.Open Then
                MsgBox("Connected successfully!")
            Else
                MsgBox("Cannot connect to server.")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub ButtonTestCloudCon_Click(sender As Object, e As EventArgs) Handles ButtonTestCloudCon.Click
        Try
            If CheckForInternetConnection() = True Then
                Dim Con = TestDBConnection(TextBoxCloudServer.Text, TextBoxCloudUsername.Text, TextBoxCloudPassword.Text, TextBoxCloudDatabase.Text, TextBoxCloudPort.Text)
                If Con.State = ConnectionState.Open Then
                    MessageBox.Show("Connected Successfully", "Connected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Cannot connect to server", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                MessageBox.Show("No internet connection available", "No internet connection", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub TextBoxIRREASON_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxSearchTranNumber.KeyPress, TextBoxIRREASON.KeyPress
        Try
            If InStr(DisallowedCharacters, e.KeyChar) > 0 Then
                e.Handled = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub TextBoxCName_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxCRefVal.KeyPress, TextBoxCName.KeyPress, TextBoxCDVal.KeyPress, TextBoxCDesc.KeyPress, TextBoxCBV.KeyPress, TextBoxCBundVal.KeyPress, TextBoxCBP.KeyPress, TextBoxCBBP.KeyPress
        Try
            If InStr(DisallowedCharacters, e.KeyChar) > 0 Then
                e.Handled = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim thread As Thread
    Dim THREADLISTUPDATE As List(Of Thread) = New List(Of Thread)
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            If ValidDatabaseLocalConnection Then
                thread = New Thread(AddressOf ServerCloudCon)
                thread.Start()
                THREADLISTUPDATE.Add(thread)
                For Each t In THREADLISTUPDATE
                    t.Join()
                Next
                If CheckForInternetConnection() = True Then
                    If ServerCloudCon.state = ConnectionState.Open Then
                        thread = New Thread(AddressOf CheckPriceChanges)
                        thread.Start()
                        THREADLISTUPDATE.Add(thread)
                        For Each t In THREADLISTUPDATE
                            t.Join()
                        Next
                        thread = New Thread(AddressOf Function1)
                        thread.Start()
                        THREADLISTUPDATE.Add(thread)
                        thread = New Thread(AddressOf GetProducts)
                        thread.Start()
                        THREADLISTUPDATE.Add(thread)
                        thread = New Thread(AddressOf Function3)
                        thread.Start()
                        THREADLISTUPDATE.Add(thread)
                        thread = New Thread(AddressOf Function4)
                        thread.Start()
                        THREADLISTUPDATE.Add(thread)
                    End If
                Else
                    MsgBox("Internet connection is not available. Please try again")
                End If
                For Each t In THREADLISTUPDATE
                    t.Join()
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            If PRICECHANGE = True Then
                listviewproductsshow(where:="Simply Perfect")
            End If
            LabelStatus.Text = "Item(s) " & LabelCountAllRows.Text & " Checked " & ProgressBar1.Value & " of " & LabelCountAllRows.Text
            DataGridView2.DataSource = FillDatagridProduct
            Button4.Enabled = True
            UPDATEPRODUCTONLY = False
            If DataGridView1.Rows.Count > 0 Or DataGridView2.Rows.Count > 0 Or DataGridView3.Rows.Count > 0 Or DataGridView4.Rows.Count > 0 Then
                Dim updatemessage = MessageBox.Show("New Updates are available. Would you like to update now ?", "New Updates", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                If updatemessage = DialogResult.Yes Then
                    InstallUpdatesFormula()
                    InstallUpdatesInventory()
                    InstallUpdatesCategory()
                    InstallUpdatesProducts()
                    listviewproductsshow(where:="Simply Perfect")
                    LabelCheckingUpdates.Text = "Update Completed : " & FullDate24HR()
                    My.Settings.Updatedatetime = FullDate24HR()
                Else
                    LabelCheckingUpdates.Text = "Completed."
                    My.Settings.Updatedatetime = FullDate24HR()
                End If
            Else
                LabelCheckingUpdates.Text = "Complete Checking! No updates found."
                My.Settings.Updatedatetime = FullDate24HR()
            End If
            MsgBox("Complete")
            My.Settings.Save()
            Timer1.Stop()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
#End Region
#Region "Updates"
#Region "Price Change"
    Dim PRICECHANGE As Boolean = False
    Private Sub CheckPriceChanges()
        Try
            Dim Query = "SELECT * FROM admin_price_request WHERE store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' AND synced = 'Unsynced'"
            Dim CmdCheck As MySqlCommand = New MySqlCommand(Query, ServerCloudCon)
            Dim DaCheck As MySqlDataAdapter = New MySqlDataAdapter(CmdCheck)
            Dim DtCheck As DataTable = New DataTable
            DaCheck.Fill(DtCheck)
            For i As Integer = 0 To DtCheck.Rows.Count - 1 Step +1
                Dim sql = "UPDATE loc_admin_products SET product_price = " & DtCheck(i)(3) & ", price_change = 1 WHERE server_product_id = " & DtCheck(i)(2) & ""
                CmdCheck = New MySqlCommand(sql, LocalhostConn)
                CmdCheck.ExecuteNonQuery()
                Dim sql2 = "UPDATE loc_price_request_change SET active = " & DtCheck(i)(5) & " WHERE request_id = " & DtCheck(i)(0) & ""
                CmdCheck = New MySqlCommand(sql2, LocalhostConn)
                CmdCheck.ExecuteNonQuery()
                Dim sq3 = "UPDATE admin_price_request SET synced = 'Synced' WHERE request_id = " & DtCheck(i)(0) & ""
                CmdCheck = New MySqlCommand(sq3, ServerCloudCon)
                CmdCheck.ExecuteNonQuery()
            Next
            If DtCheck.Rows.Count > 0 Then
                PRICECHANGE = True
            Else
                PRICECHANGE = False
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
#End Region
#Region "Categories Update"
    Private Function LoadCategoryLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        dtlocal.Columns.Add("updated_at")
        dtlocal.Columns.Add("category_id")
        Dim dtlocal1 As DataTable = New DataTable
        Try
            Dim sql = "SELECT updated_at, category_id FROM loc_admin_category"
            cmdlocal = New MySqlCommand(sql, LocalhostConn())
            dalocal = New MySqlDataAdapter(cmdlocal)
            dalocal.Fill(dtlocal1)
            For i As Integer = 0 To dtlocal1.Rows.Count - 1 Step +1
                Dim Cat As DataRow = dtlocal.NewRow
                Cat("updated_at") = dtlocal1(i)(0).ToString
                Cat("category_id") = dtlocal1(i)(1)
                dtlocal.Rows.Add(Cat)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dtlocal
    End Function
    Private Sub Function1()
        Try
            Dim Query = "SELECT * FROM loc_admin_category"
            Dim CmdCheck As MySqlCommand = New MySqlCommand(Query, LocalhostConn)
            Dim DaCheck As MySqlDataAdapter = New MySqlDataAdapter(CmdCheck)
            Dim DtCheck As DataTable = New DataTable
            DaCheck.Fill(DtCheck)

            Dim cmdserver As MySqlCommand
            Dim daserver As MySqlDataAdapter
            Dim dtserver As DataTable
            If DtCheck.Rows.Count < 1 Then
                Dim sql = "SELECT `category_id`, `category_name`, `brand_name`, `updated_at`, `origin`, `status` FROM admin_category"
                cmdserver = New MySqlCommand(sql, ServerCloudCon())
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    LabelNewRows.Text += 1
                    DataGridView1.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3).ToString, dtserver(i)(4), dtserver(i)(5))
                Next
            Else
                Dim Ids As String = ""
                If ValidCloudConnection = True Then
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
                        If LoadCategoryLocal(i)(0).ToString <> dtserver(i)(3).ToString Then
                            ProgressBar1.Value += 1
                            LabelStatus.Text = "Item(s) " & LabelCountAllRows.Text & " Checking " & ProgressBar1.Value & " of " & LabelCountAllRows.Text
                            DataGridView1.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3).ToString, dtserver(i)(4), dtserver(i)(5))
                        Else
                            ProgressBar1.Value += 1
                            LabelStatus.Text = "Item(s) " & LabelCountAllRows.Text & " Checking " & ProgressBar1.Value & " of " & LabelCountAllRows.Text
                        End If
                    Next
                    Dim sql2 = "SELECT `category_id`, `category_name`, `brand_name`, `updated_at`, `origin`, `status` FROM admin_category WHERE category_id NOT IN (" & Ids & ")"
                    cmdserver = New MySqlCommand(sql2, ServerCloudCon())
                    daserver = New MySqlDataAdapter(cmdserver)
                    dtserver = New DataTable
                    daserver.Fill(dtserver)
                    For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                        ProgressBar1.Value += 1
                        LabelStatus.Text = "Item(s) " & LabelCountAllRows.Text & " Checking " & ProgressBar1.Value & " of " & LabelCountAllRows.Text
                        DataGridView1.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3).ToString, dtserver(i)(4), dtserver(i)(5))
                    Next
                End If
            End If
        Catch ex As Exception

            BackgroundWorker1.CancelAsync()
            'If table doesnt have data
        End Try
    End Sub
#End Region
#Region "Products Update"
    Dim UPDATEPRODUCTONLY As Boolean = False
    Dim FillDatagridProduct As DataTable
    Private Sub GetProducts()
        Try
            FillDatagridProduct = New DataTable
            FillDatagridProduct.Columns.Add("product_id")
            FillDatagridProduct.Columns.Add("product_sku")
            FillDatagridProduct.Columns.Add("product_name")
            FillDatagridProduct.Columns.Add("formula_id")
            FillDatagridProduct.Columns.Add("product_barcode")
            FillDatagridProduct.Columns.Add("product_category")
            FillDatagridProduct.Columns.Add("product_price")
            FillDatagridProduct.Columns.Add("product_desc")
            FillDatagridProduct.Columns.Add("product_image")
            FillDatagridProduct.Columns.Add("product_status")
            FillDatagridProduct.Columns.Add("origin")
            FillDatagridProduct.Columns.Add("date_modified")
            FillDatagridProduct.Columns.Add("inventory_id")

            Dim Query = "SELECT * FROM loc_admin_products"
            Dim CmdCheck As MySqlCommand = New MySqlCommand(Query, LocalhostConn)
            Dim DaCheck As MySqlDataAdapter = New MySqlDataAdapter(CmdCheck)
            Dim DtCheck As DataTable = New DataTable
            DaCheck.Fill(DtCheck)
            If DtCheck.Rows.Count < 1 Then
                GetAllProducts()
            Else
                Dim DtCount As DataTable
                Dim Connection As MySqlConnection = ServerCloudCon()
                Dim SqlCount = "SELECT COUNT(product_id) FROM admin_products_org"
                Dim CmdCount As MySqlCommand = New MySqlCommand(SqlCount, Connection)
                Dim result As Integer = CmdCount.ExecuteScalar
                Dim DaCount As MySqlDataAdapter
                Dim FillDt As DataTable = New DataTable

                For a = 1 To result
                    ProgressBar1.Value += 1
                    LabelStatus.Text = "Item(s) " & LabelCountAllRows.Text & " Checking " & ProgressBar1.Value & " of " & LabelCountAllRows.Text
                    Dim Query1 As String = "SELECT date_modified FROM loc_admin_products WHERE product_id = " & a
                    Dim cmd As MySqlCommand = New MySqlCommand(Query1, LocalhostConn)
                    DaCount = New MySqlDataAdapter(cmd)
                    FillDt = New DataTable
                    DaCount.Fill(FillDt)
                    Dim Prod As DataRow = FillDatagridProduct.NewRow
                    If FillDt.Rows.Count > 0 Then
                        'Exist then check for update
                        Query1 = "SELECT * FROM admin_products_org WHERE product_id = " & a
                        cmd = New MySqlCommand(Query1, Connection)
                        DaCount = New MySqlDataAdapter(cmd)
                        DtCount = New DataTable
                        DaCount.Fill(DtCount)
                        If FillDt(0)(0).ToString <> DtCount(0)(11) Then
                            Prod("product_id") = DtCount(0)(0)
                            Prod("product_sku") = DtCount(0)(1)
                            Prod("product_name") = DtCount(0)(2)
                            Prod("formula_id") = DtCount(0)(3)
                            Prod("product_barcode") = DtCount(0)(4)
                            Prod("product_category") = DtCount(0)(5)
                            Prod("product_price") = DtCount(0)(6)
                            Prod("product_desc") = DtCount(0)(7)
                            Prod("product_image") = DtCount(0)(8)
                            Prod("product_status") = DtCount(0)(9)
                            Prod("origin") = DtCount(0)(10)
                            Prod("date_modified") = DtCount(0)(11)
                            Prod("inventory_id") = DtCount(0)(12)
                            FillDatagridProduct.Rows.Add(Prod)
                        End If
                    Else
                        'Insert new product
                        Query1 = "SELECT * FROM admin_products_org WHERE product_id = " & a
                        cmd = New MySqlCommand(Query1, Connection)
                        DaCount = New MySqlDataAdapter(cmd)
                        DtCount = New DataTable
                        DaCount.Fill(DtCount)
                        Prod("product_id") = DtCount(0)(0)
                        Prod("product_sku") = DtCount(0)(1)
                        Prod("product_name") = DtCount(0)(2)
                        Prod("formula_id") = DtCount(0)(3)
                        Prod("product_barcode") = DtCount(0)(4)
                        Prod("product_category") = DtCount(0)(5)
                        Prod("product_price") = DtCount(0)(6)
                        Prod("product_desc") = DtCount(0)(7)
                        Prod("product_image") = DtCount(0)(8)
                        Prod("product_status") = DtCount(0)(9)
                        Prod("origin") = DtCount(0)(10)
                        Prod("date_modified") = DtCount(0)(11)
                        Prod("inventory_id") = DtCount(0)(12)
                        FillDatagridProduct.Rows.Add(Prod)
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub GetAllProducts()
        Try
            Try
                'Dim DatatableProducts As DataTable
                Dim Connection As MySqlConnection = ServerCloudCon()
                Dim SqlCount = "SELECT COUNT(product_id) FROM admin_products_org"
                Dim CmdCount As MySqlCommand = New MySqlCommand(SqlCount, Connection)
                Dim result As Integer = CmdCount.ExecuteScalar

                FillDatagridProduct = New DataTable
                FillDatagridProduct.Columns.Add("product_id")
                FillDatagridProduct.Columns.Add("product_sku")
                FillDatagridProduct.Columns.Add("product_name")
                FillDatagridProduct.Columns.Add("formula_id")
                FillDatagridProduct.Columns.Add("product_barcode")
                FillDatagridProduct.Columns.Add("product_category")
                FillDatagridProduct.Columns.Add("product_price")
                FillDatagridProduct.Columns.Add("product_desc")
                FillDatagridProduct.Columns.Add("product_image")
                FillDatagridProduct.Columns.Add("product_status")
                FillDatagridProduct.Columns.Add("origin")
                FillDatagridProduct.Columns.Add("date_modified")
                FillDatagridProduct.Columns.Add("inventory_id")
                Dim DaCount As MySqlDataAdapter
                Dim FillDt As DataTable = New DataTable
                For a = 1 To result
                    LabelNewRows.Text += 1
                    Dim Query As String = "SELECT * FROM admin_products_org WHERE product_id = " & a
                    cmd = New MySqlCommand(Query, Connection)
                    DaCount = New MySqlDataAdapter(cmd)
                    FillDt = New DataTable
                    DaCount.Fill(FillDt)
                    For i As Integer = 0 To FillDt.Rows.Count - 1 Step +1
                        Dim Prod As DataRow = FillDatagridProduct.NewRow
                        Prod("product_id") = FillDt(i)(0)
                        Prod("product_sku") = FillDt(i)(1)
                        Prod("product_name") = FillDt(i)(2)
                        Prod("formula_id") = FillDt(i)(3)
                        Prod("product_barcode") = FillDt(i)(4)
                        Prod("product_category") = FillDt(i)(5)
                        Prod("product_price") = FillDt(i)(6)
                        Prod("product_desc") = FillDt(i)(7)
                        Prod("product_image") = FillDt(i)(8)
                        Prod("product_status") = FillDt(i)(9)
                        Prod("origin") = FillDt(i)(10)
                        Prod("date_modified") = FillDt(i)(11)
                        Prod("inventory_id") = FillDt(i)(12)
                        FillDatagridProduct.Rows.Add(Prod)
                    Next
                Next
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

#End Region
#Region "Formulas Update"
    Private Function LoadFormulaLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        dtlocal.Columns.Add("date_modified")
        dtlocal.Columns.Add("server_formula_id")
        Dim dtlocal1 As DataTable = New DataTable
        Try
            Dim sql = "SELECT date_modified, server_formula_id FROM loc_product_formula"
            cmdlocal = New MySqlCommand(sql, LocalhostConn())
            dalocal = New MySqlDataAdapter(cmdlocal)
            dalocal.Fill(dtlocal1)
            For i As Integer = 0 To dtlocal1.Rows.Count - 1 Step +1
                Dim Cat As DataRow = dtlocal.NewRow
                Cat("date_modified") = dtlocal1(i)(0).ToString
                Cat("server_formula_id") = dtlocal1(i)(1)
                dtlocal.Rows.Add(Cat)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dtlocal
    End Function
    Private Sub Function3()
        Try
            Dim Query = "SELECT * FROM loc_product_formula"
            Dim CmdCheck As MySqlCommand = New MySqlCommand(Query, LocalhostConn)
            Dim DaCheck As MySqlDataAdapter = New MySqlDataAdapter(CmdCheck)
            Dim DtCheck As DataTable = New DataTable
            DaCheck.Fill(DtCheck)

            Dim cmdserver As MySqlCommand
            Dim daserver As MySqlDataAdapter
            Dim dtserver As DataTable
            If DtCheck.Rows.Count < 1 Then
                Dim sql = "SELECT `server_formula_id`, `product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin` FROM admin_product_formula_org"
                cmdserver = New MySqlCommand(sql, ServerCloudCon())
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    LabelNewRows.Text += 1
                    DataGridView3.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8), dtserver(i)(9), dtserver(i)(10).ToString, dtserver(i)(11), dtserver(i)(12))

                Next
            Else
                Dim Ids As String = ""
                If ValidCloudConnection = True Then
                    For i As Integer = 0 To LoadFormulaLocal.Rows.Count - 1 Step +1
                        If Ids = "" Then
                            Ids = "" & LoadFormulaLocal(i)(1) & ""
                        Else
                            Ids += "," & LoadFormulaLocal(i)(1) & ""
                        End If
                    Next
                    Dim sql = "SELECT `server_formula_id`, `product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin` FROM admin_product_formula_org WHERE server_formula_id  IN (" & Ids & ") "
                    cmdserver = New MySqlCommand(sql, ServerCloudCon())
                    daserver = New MySqlDataAdapter(cmdserver)
                    dtserver = New DataTable
                    daserver.Fill(dtserver)
                    For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                        If LoadFormulaLocal(i)(0).ToString <> dtserver(i)(10).ToString Then
                            ProgressBar1.Value += 1
                            LabelStatus.Text = "Item(s) " & LabelCountAllRows.Text & " Checking " & ProgressBar1.Value & " of " & LabelCountAllRows.Text
                            DataGridView3.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8), dtserver(i)(9), dtserver(i)(10).ToString, dtserver(i)(11), dtserver(i)(12))
                        Else
                            ProgressBar1.Value += 1
                            LabelStatus.Text = "Item(s) " & LabelCountAllRows.Text & " Checking " & ProgressBar1.Value & " of " & LabelCountAllRows.Text
                        End If
                    Next
                    Dim sql2 = "SELECT `server_formula_id`, `product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin` FROM admin_product_formula_org WHERE server_formula_id NOT IN (" & Ids & ") "
                    cmdserver = New MySqlCommand(sql2, ServerCloudCon())
                    daserver = New MySqlDataAdapter(cmdserver)
                    dtserver = New DataTable
                    daserver.Fill(dtserver)
                    For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                        If LoadFormulaLocal(i)(0).ToString <> dtserver(i)(10) Then
                            ProgressBar1.Value += 1
                            LabelStatus.Text = "Item(s) " & LabelCountAllRows.Text & " Checking " & ProgressBar1.Value & " of " & LabelCountAllRows.Text
                            DataGridView3.Rows.Add(dtserver(i)(0), dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8), dtserver(i)(9), dtserver(i)(10).ToString, dtserver(i)(11), dtserver(i)(12))
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            BackgroundWorker1.CancelAsync()
            'If table doesnt have data
        End Try
    End Sub
#End Region
#Region "Inventory Update"
    Private Function LoadInventoryLocal() As DataTable
        Dim cmdlocal As MySqlCommand
        Dim dalocal As MySqlDataAdapter
        Dim dtlocal As DataTable = New DataTable
        dtlocal.Columns.Add("server_date_modified")
        dtlocal.Columns.Add("server_inventory_id")
        Dim dtlocal1 As DataTable = New DataTable
        Try
            Dim sql = "SELECT server_date_modified , server_inventory_id FROM loc_pos_inventory"
            cmdlocal = New MySqlCommand(sql, LocalhostConn())
            dalocal = New MySqlDataAdapter(cmdlocal)
            dalocal.Fill(dtlocal)
            For i As Integer = 0 To dtlocal1.Rows.Count - 1 Step +1
                Dim Cat As DataRow = dtlocal.NewRow
                Cat("server_date_modified") = dtlocal1(i)(0).ToString
                Cat("server_inventory_id") = dtlocal1(i)(1)
                dtlocal.Rows.Add(Cat)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dtlocal
    End Function
    Private Sub Function4()
        Try
            Dim Query = "SELECT * FROM loc_pos_inventory"
            Dim CmdCheck As MySqlCommand = New MySqlCommand(Query, LocalhostConn)
            Dim DaCheck As MySqlDataAdapter = New MySqlDataAdapter(CmdCheck)
            Dim DtCheck As DataTable = New DataTable
            DaCheck.Fill(DtCheck)
            Dim cmdserver As MySqlCommand
            Dim daserver As MySqlDataAdapter
            Dim dtserver As DataTable
            If DtCheck.Rows.Count < 1 Then
                Dim sql = "SELECT `server_inventory_id`, `product_ingredients`, `sku`, `stock_primary`, `stock_secondary`, `stock_no_of_servings`, `stock_status`, `critical_limit`, `date_modified`, `main_inventory_id` FROM admin_pos_inventory_org"
                cmdserver = New MySqlCommand(sql, ServerCloudCon())
                daserver = New MySqlDataAdapter(cmdserver)
                dtserver = New DataTable
                daserver.Fill(dtserver)
                For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                    LabelNewRows.Text += 1
                    DataGridView4.Rows.Add(dtserver(i)(0), 0, dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8).ToString, dtserver(i)(9).ToString)
                Next
            Else
                Dim Ids As String = ""
                If ValidCloudConnection = True Then
                    For i As Integer = 0 To LoadInventoryLocal.Rows.Count - 1 Step +1
                        If Ids = "" Then
                            Ids = "" & LoadInventoryLocal(i)(1) & ""
                        Else
                            Ids += "," & LoadInventoryLocal(i)(1) & ""
                        End If
                    Next
                    Dim sql = "SELECT `server_inventory_id`, `product_ingredients`, `sku`, `stock_primary`, `stock_secondary`, `stock_no_of_servings`, `stock_status`, `critical_limit`, `date_modified`,`main_inventory_id` FROM admin_pos_inventory_org WHERE server_inventory_id IN (" & Ids & ")"
                    cmdserver = New MySqlCommand(sql, ServerCloudCon())
                    daserver = New MySqlDataAdapter(cmdserver)
                    dtserver = New DataTable
                    daserver.Fill(dtserver)
                    For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                        If LoadInventoryLocal(i)(0).ToString <> dtserver(i)(8).ToString Then
                            ProgressBar1.Value += 1
                            LabelStatus.Text = "Item(s) " & LabelCountAllRows.Text & " Checking " & ProgressBar1.Value & " of " & LabelCountAllRows.Text
                            DataGridView4.Rows.Add(dtserver(i)(0), 0, dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8).ToString, dtserver(i)(9).ToString)
                        Else
                            ProgressBar1.Value += 1
                            LabelStatus.Text = "Item(s) " & LabelCountAllRows.Text & " Checking " & ProgressBar1.Value & " of " & LabelCountAllRows.Text
                        End If
                    Next
                    Dim sql2 = "SELECT `server_inventory_id`, `product_ingredients`, `sku`, `stock_primary`, `stock_secondary`, `stock_no_of_servings`, `stock_status`, `critical_limit`, `date_modified`,`main_inventory_id` FROM admin_pos_inventory_org WHERE server_inventory_id NOT IN (" & Ids & ")"
                    cmdserver = New MySqlCommand(sql2, ServerCloudCon())
                    daserver = New MySqlDataAdapter(cmdserver)
                    dtserver = New DataTable
                    daserver.Fill(dtserver)
                    For i As Integer = 0 To dtserver.Rows.Count - 1 Step +1
                        If LoadInventoryLocal(i)(0).ToString <> dtserver(i)(8) Then
                            ProgressBar1.Value += 1
                            LabelStatus.Text = "Item(s) " & LabelCountAllRows.Text & " Checking " & ProgressBar1.Value & " of " & LabelCountAllRows.Text
                            DataGridView4.Rows.Add(dtserver(i)(0), 0, dtserver(i)(1), dtserver(i)(2), dtserver(i)(3), dtserver(i)(4), dtserver(i)(5), dtserver(i)(6), dtserver(i)(7), dtserver(i)(8).ToString, dtserver(i)(9).ToString)
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            BackgroundWorker1.CancelAsync()
            'If table doesnt have data
        End Try
    End Sub


#End Region
#End Region
#Region "Install Updates"
    Private Sub InstallUpdatesCategory()
        Try
            Dim cmdlocal As MySqlCommand
            With DataGridView1
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim sql = "SELECT category_id FROM loc_admin_category WHERE category_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                    If result = 0 Then
                        Dim sqlinsert = "INSERT INTO `loc_admin_category`(`category_name`, `brand_name`, `updated_at`, `origin`, `status`) VALUES (@0,@1,@2,@3,@4)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    Else
                        Dim sqlupdate = "UPDATE `loc_admin_category` SET `category_name`=@0,`brand_name`=@1,`updated_at`=@2,`origin`=@3,`status`=@4 WHERE category_id = " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlupdate, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InstallUpdatesFormula()
        Try
            Dim cmdlocal As MySqlCommand
            With DataGridView3
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim sql = "SELECT formula_id FROM loc_product_formula WHERE server_formula_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                    If result = 0 Then
                        Dim sqlinsert = "INSERT INTO loc_product_formula (`server_formula_id`,`product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin`, `store_id`, `guid`, `crew_id`, `server_date_modified`) VALUES
                                        (@0 ,@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11 , @12 , @13 , @14, @15, @16)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Int64).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value
                        cmdlocal.Parameters.Add("@11", MySqlDbType.Decimal).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@16", MySqlDbType.Text).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    Else
                        Dim sqlupdate = "UPDATE `loc_product_formula` SET `server_formula_id`= @0,`product_ingredients`= @1,`primary_unit`= @2,`primary_value`= @3,`secondary_unit`= @4,`secondary_value`=@5,`serving_unit`=@6,`serving_value`=@7,`no_servings`=@8,`status`=@9,`date_modified`=@10,`unit_cost`=@11,`origin`=@12,`store_id`=@13,`guid`=@14, `crew_id`=@15,`server_date_modified`=@16 WHERE server_formula_id =  " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlupdate, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Int64).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value
                        cmdlocal.Parameters.Add("@11", MySqlDbType.Decimal).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@16", MySqlDbType.Text).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InstallUpdatesInventory()
        Try
            Dim cmdlocal As MySqlCommand
            With DataGridView4
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim sql = "SELECT inventory_id FROM loc_pos_inventory WHERE server_inventory_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                    If result = 0 Then
                        Dim sqlinsert = "INSERT INTO loc_pos_inventory (`server_inventory_id`,`formula_id`,`product_ingredients`,`sku`,`stock_primary`,`stock_secondary`,`stock_no_of_servings`,`stock_status`,`critical_limit`,`created_at`,`server_date_modified`,`store_id`,`crew_id`,`guid`,`synced`,`main_inventory_id`) VALUES
                                        (@0 ,@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.Int64).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.Int64).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.Int64).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = "Synced"
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    Else
                        Dim sqlUpdate = "UPDATE `loc_pos_inventory` SET `server_inventory_id`= @0,`formula_id`=@1,`product_ingredients`=@2,`sku`=@3,`stock_primary`=@4,`stock_secondary`=@5,`stock_no_of_servings`=@6,`stock_status`=@7,`critical_limit`=@8,`created_at`=@9,`server_date_modified`=@10,`store_id`=@11,`crew_id`=@12,`guid`=@13,`synced`=@14,`main_inventory_id`=@15 WHERE `server_inventory_id`= " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlUpdate, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.Int64).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.Int64).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.Int64).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = "Synced"
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InstallUpdatesProducts()
        Try
            Dim cmdlocal As MySqlCommand
            With DataGridView2
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim sql = "SELECT product_id FROM loc_admin_products WHERE server_product_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                    If result = 0 Then
                        Dim sqlinsert = "INSERT INTO loc_admin_products (`server_product_id`, `product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `server_inventory_id`, `guid`, `store_id`, `crew_id`, `synced`) VALUES
                                        (@0 ,@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.Int64).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.Text).Value = .Rows(i).Cells(12).Value.ToString()

                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@14", MySqlDbType.Int64).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@16", MySqlDbType.VarChar).Value = "Synced"
                        cmdlocal.ExecuteNonQuery()
                    Else
                        ',`formula_id`=@3
                        Dim sqlupdate = "UPDATE `loc_admin_products` SET `server_product_id`=@0,`product_sku`=@1,`product_name`=@2,`product_barcode`=@4,`product_category`=@5,`product_price`=@6,`product_desc`=@7,`product_image`=@8,`product_status`=@9,`origin`=@10,`date_modified`=@11,`server_inventory_id`=@12,`guid`=@13,`store_id`=@14,`crew_id`=@15,`synced`=@16 WHERE server_product_id =  " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlupdate, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        'cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.Int64).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.Text).Value = .Rows(i).Cells(12).Value.ToString()

                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@14", MySqlDbType.Int64).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@16", MySqlDbType.VarChar).Value = "Synced"
                        cmdlocal.ExecuteNonQuery()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
#End Region
    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Enabled = False
        Changeproductformula.Show()
    End Sub
End Class