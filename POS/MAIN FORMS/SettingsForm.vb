Imports MySql.Data.MySqlClient
Public Class SettingsForm
    Public AddOrEdit As Boolean
    Private Sub SettingsForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        POS.Enabled = True
    End Sub
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
    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedIndex = 1 Then
            If Partners = False Then
                TabControl3.TabPages(0).Text = "Available Partners"
                TabControl3.TabPages(1).Text = "Deactivated Parners"
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
        fields = "`product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`"
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
        fields = "transaction_number, amounttendered, discount, moneychange, crew_id, time, vatable, vat_exempt, zero_rated, vat, transaction_type"
        If justload = True Then
            GLOBAL_SELECT_ALL_FUNCTION(table:="loc_daily_transaction WHERE date = CURDATE() AND active = 1 ORDER BY transaction_id DESC", datagrid:=DataGridViewITEMRETURN1, fields:=fields)
        Else
            If String.IsNullOrWhiteSpace(TextBoxSearchTranNumber.Text) Then
                FlowLayoutPanel1.Controls.Clear()
                GLOBAL_SELECT_ALL_FUNCTION(table:="loc_daily_transaction WHERE date = CURDATE() AND active = 1 ORDER BY transaction_id DESC", datagrid:=DataGridViewITEMRETURN1, fields:=fields)
            Else
                FlowLayoutPanel1.Controls.Clear()
                GLOBAL_SELECT_ALL_FUNCTION(table:="loc_daily_transaction WHERE transaction_number LIKE '%" & TextBoxSearchTranNumber.Text & "%'  AND date = CURDATE() AND active = 1 ORDER BY transaction_id DESC", datagrid:=DataGridViewITEMRETURN1, fields:=fields)
            End If
        End If
        With DataGridViewITEMRETURN1
            .Columns(0).HeaderText = "Reference #"
            .Columns(1).HeaderText = "Cash"
            .Columns(2).HeaderText = "Discount"
            .Columns(3).HeaderText = "Change"
            .Columns(4).HeaderText = "Crew"
            .Columns(5).HeaderText = "Time"
            .Columns(5).Name = "Time"
            .Columns(0).Width = 100
            .Columns(4).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            .Columns(6).Visible = False
            .Columns(7).Visible = False
            .Columns(8).Visible = False
            .Columns(9).Visible = False
            .Columns(10).Visible = False
            For Each row As DataRow In dt.Rows
                row("crew_id") = returnfullname(row("crew_id"))
            Next
        End With
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
            Dim query As String = "SELECT SUM(TOTAL) FROM loc_daily_transaction_details WHERE transaction_number = '" & DataGridViewITEMRETURN1.SelectedRows(0).Cells(0).Value.ToString & "'"
            Dim cmdquery As MySqlCommand = New MySqlCommand(query, LocalhostConn())
            Dim queryda As MySqlDataAdapter = New MySqlDataAdapter(cmdquery)
            Dim dt1 As DataTable = New DataTable
            queryda.Fill(dt1)
            Using readerObj As MySqlDataReader = cmdquery.ExecuteReader
                While readerObj.Read
                    grandtotal = readerObj("SUM(TOTAL)")
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
                    sql = "SELECT product_image FROM loc_admin_products WHERE product_id = " & row("product_id")
                    cmd = New MySqlCommand
                    With cmd
                        .CommandText = sql
                        .Connection = LocalhostConn()
                        Using readerObj As MySqlDataReader = cmd.ExecuteReader
                            While readerObj.Read
                                productimage = readerObj("product_image")
                            End While
                        End Using
                    End With
                    Dim Drawproduct As New Panel
                    Dim Myname As New Label
                    Dim MyQty As New Label
                    Dim MyPrice As New Label
                    Dim MyTotal As New Label
                    Dim MyImage As New Button
                    With Drawproduct
                        .Name = buttonname
                        .Text = buttonname
                        .BorderStyle = BorderStyle.None
                        .ForeColor = Color.White
                        .Font = New Font("Kelson Sans Normal", 10)
                        .Width = 345
                        .Height = 140
                        .Cursor = Cursors.Hand
                        With Myname
                            .Location = New Point(200, 10)
                            .Text = row("product_sku")
                            .ForeColor = Color.Black
                            .Font = New Font("Kelson Sans Normal", 10, FontStyle.Bold)
                            .Width = 200
                        End With
                        With MyQty
                            .Font = New Font("Kelson Sans Normal", 10)
                            .Location = New Point(200, 35)
                            .Text = "Quantity: " & row("quantity")
                            .ForeColor = Color.Black
                            .Width = 200
                        End With
                        With MyPrice
                            .Font = New Font("Kelson Sans Normal", 10)
                            .Location = New Point(200, 60)
                            .Text = "Price: " & row("price")
                            .ForeColor = Color.Black
                            .Width = 200
                        End With
                        With MyTotal
                            .Font = New Font("Kelson Sans Normal", 10)
                            .Location = New Point(200, 85)
                            .Text = "TOTAL: " & row("total")
                            .ForeColor = Color.Black
                            .Width = 200
                        End With
                        With MyImage
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
                    Drawproduct.Controls.Add(Myname)
                    Drawproduct.Controls.Add(MyQty)
                    Drawproduct.Controls.Add(MyPrice)
                    Drawproduct.Controls.Add(MyTotal)
                    Drawproduct.Controls.Add(MyImage)
                    FlowLayoutPanel1.Controls.Add(Drawproduct)
                Next
                With DataGridViewITEMRETURN1
                    LabelIRTRANSNUM.Text = .SelectedRows(0).Cells(0).Value.ToString
                    LabelIRSUBTOTAL.Text = countrow & " item(s)"
                    LabelIRTYPE.Text = .SelectedRows(0).Cells(10).Value.ToString
                    LabelIRTOTAL.Text = grandtotal
                    LabelIRCASH.Text = .SelectedRows(0).Cells(1).Value.ToString
                    LabelIRCHANGE.Text = .SelectedRows(0).Cells(3).Value.ToString
                    LabelIRDISC.Text = .SelectedRows(0).Cells(2).Value.ToString
                    LabelIRVAT.Text = .SelectedRows(0).Cells(6).Value.ToString
                    LabelIRVATEX.Text = .SelectedRows(0).Cells(7).Value.ToString
                    LabelIRZERO.Text = .SelectedRows(0).Cells(8).Value.ToString
                    LabelIRINPUTVAT.Text = .SelectedRows(0).Cells(9).Value.ToString
                    ButtonRefund.Text = "Refund PHP " & grandtotal
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
            Dim transaction_num As String = DataGridViewITEMRETURN1.SelectedRows(0).Cells(0).Value.ToString
            If String.IsNullOrWhiteSpace(TextBoxIRREASON.Text) Then
                MessageBox.Show("Reason for refund is required!", "Refund", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                sql = "SELECT * FROM loc_daily_transaction WHERE date = CURDATE() AND time >= Now() - INTERVAL 10 MINUTE AND transaction_number = '" & transaction_num & "'"
                cmd = New MySqlCommand(sql, LocalhostConn())
                da = New MySqlDataAdapter(cmd)
                dt = New DataTable
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
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub INSERTRETURNS(transaction_num As String)
        Try
            fields = "(`transaction_number`, `reason`, `total`, `guid`, `store_id`, `crew_id`, `synced`, `zreading`)"
            value = "('" & transaction_num & "'
                            , '" & TextBoxIRREASON.Text & "'
                            , " & grandtotal & "
                            , '" & ClientGuid & "'
                            , '" & ClientStoreID & "'
                            , '" & ClientCrewID & "'
                            , 'Unsynced'
                            , '" & S_Zreading & "')"
            GLOBAL_INSERT_FUNCTION(table:="`loc_refund_return_details`", fields:=fields, errormessage:="", successmessage:="", values:=value)
            GLOBAL_FUNCTION_UPDATE("loc_daily_transaction", "active = 2 , synced = 'Unsynced'", "transaction_number = '" & transaction_num & "'")
            GLOBAL_FUNCTION_UPDATE("loc_daily_transaction_details", "active = 2 , synced = 'Unsynced'", "transaction_number = '" & transaction_num & "'")
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
            GLOBAL_SELECT_ALL_FUNCTION(table:="tbcoupon", fields:="*", datagrid:=DataGridView2)
            With DataGridView2
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
            GLOBAL_INSERT_FUNCTION("tbcoupon", "(`Couponname_`, `Desc_`, `Discountvalue_`, `Referencevalue_`, `Type`, `Bundlebase_`, `BBValue_`, `Bundlepromo_`, `BPValue_`, `Effectivedate`, `Expirydate`)", value, "", "")
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
            If My.Settings.ValidLocalConn = True Then
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
                    My.Settings.ValidCloudConn = True
                    My.Settings.Save()
                Else
                    My.Settings.ValidCloudConn = False
                    My.Settings.Save()
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadAdditionalSettings()
        Try
            If My.Settings.ValidLocalConn = True Then
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
                For i As Integer = 0 To dt.Rows.Count - 1 Step +1
                    If dt(i)(0) = "" Then
                        My.Settings.ValidAddtionalSettings = False
                        Exit For
                    ElseIf dt(i)(1) = "" Then
                        My.Settings.ValidAddtionalSettings = False
                        Exit For
                    ElseIf dt(i)(2) = "" Then
                        My.Settings.ValidAddtionalSettings = False
                        Exit For
                    ElseIf dt(i)(3) = "" Then
                        My.Settings.ValidAddtionalSettings = False
                        Exit For
                    ElseIf dt(i)(4) = "" Then
                        My.Settings.ValidAddtionalSettings = False
                        Exit For
                    Else
                        My.Settings.ValidAddtionalSettings = True
                        Exit For
                    End If
                Next
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadDevInfo()
        Try
            If My.Settings.ValidLocalConn = True Then
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
                For i As Integer = 0 To dt.Rows.Count - 1 Step +1
                    If dt(i)(0) = "" Then
                        My.Settings.ValidDevSettings = False
                        My.Settings.Save()
                        Exit For
                    ElseIf dt(i)(1) = "" Then
                        My.Settings.ValidDevSettings = False
                        My.Settings.Save()
                        Exit For
                    ElseIf dt(i)(2) = "" Then
                        My.Settings.ValidDevSettings = False
                        My.Settings.Save()
                        Exit For
                    ElseIf dt(i)(3) = "" Then
                        My.Settings.ValidDevSettings = False
                        My.Settings.Save()
                        Exit For
                    ElseIf dt(i)(4) = "" Then
                        My.Settings.ValidDevSettings = False
                        My.Settings.Save()
                        Exit For
                    ElseIf dt(i)(5) = "" Then
                        My.Settings.ValidDevSettings = False
                        My.Settings.Save()
                        Exit For
                    ElseIf dt(i)(6) = "" Then
                        My.Settings.ValidDevSettings = False
                        My.Settings.Save()
                        Exit For
                    ElseIf dt(i)(7) = "" Then
                        My.Settings.ValidDevSettings = False
                        My.Settings.Save()
                        Exit For
                    ElseIf dt(i)(8) = "" Then
                        My.Settings.ValidDevSettings = False
                        My.Settings.Save()
                        Exit For
                    Else
                        My.Settings.ValidDevSettings = True
                        My.Settings.Save()
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub LoadAutoBackup()
        Try
            If My.Settings.ValidLocalConn = True Then
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
        BackupDatabase()
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
        If (OpenFileDialog1.ShowDialog = DialogResult.OK) Then
            TextBoxLocalRestorePath.Text = OpenFileDialog1.FileName
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
        RepairDatabase()
    End Sub
    Private Sub RepairDatabase()
        Try
            Process.Start("cmd.exe", "/k cd C:\xampp\mysql\bin & mysqlcheck -h " & TextBoxLocalServer.Text & " -u " & TextBoxLocalUsername.Text & " -p " & TextBoxLocalPassword.Text & " --auto-repair -c --databases " & TextBoxLocalDatabase.Text)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles ButtonOptimizeDB.Click
        OptimizeDatabase()
    End Sub
    Private Sub OptimizeDatabase()
        Try
            Process.Start("cmd.exe", "/k cd C:\xampp\mysql\bin & mysqlcheck -h " & TextBoxLocalServer.Text & " -u " & TextBoxLocalUsername.Text & " -p " & TextBoxLocalPassword.Text & " -o --databases " & TextBoxLocalDatabase.Text)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
#End Region
End Class