Imports MySql.Data.MySqlClient
Public Class Inventory
    Dim boolinventory As Boolean = False
    Dim prodid As String
    Dim tbl As String
    Dim flds As String
    Private Sub Inventory_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TabControl1.TabPages(0).Text = "Stock Inventory"
        TabControl1.TabPages(1).Text = "Critical Stock"
        TabControl1.TabPages(2).Text = "Fast Moving Stock"
        TabControl1.TabPages(3).Text = "Stock Adjustment"
        TabControl1.TabPages(4).Text = "Stock in (Receiving) Entry"
        TabControl2.TabPages(0).Text = "Product Ingredients"
        TabControl3.TabPages(0).Text = "Stock Adjustment (Add/Deduct/Transfer)"
        TabControl3.TabPages(1).Text = "Stock Adjustment (Settings)"
        loadinventory()
        loadcriticalstocks()
        loadstockadjustmentreport(False)
        loadfastmovingstock()
        loadstockentry()
        loadcomboboxingredients()

    End Sub
    Private Sub Inventory_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Me.Hide()
        MDIFORM.Show()
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        MDIFORM.Button2.PerformClick()
    End Sub
    Sub loadinventory()
        Try
            fields = "I.product_ingredients as Ingredients, CONCAT_WS(' ', I.stock_primary, F.primary_unit) as PrimaryValue , CONCAT_WS(' ', I.stock_secondary, F.secondary_unit) as UOM , I.stock_no_of_servings, I.stock_status, I.critical_limit, I.created_at"
            GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:="loc_pos_inventory I INNER JOIN loc_product_formula F ON F.formula_id = I.formula_id ", datagrid:=DataGridViewINVVIEW, errormessage:="", successmessage:="", fields:=fields, where:=" I.stock_status = 1 AND I.store_id = " & ClientStoreID)
            With DataGridViewINVVIEW
                .Columns(3).HeaderCell.Value = "No. of Servings"
                .Columns(4).HeaderCell.Value = "Status"
                .Columns(5).HeaderCell.Value = "Critical Limit"
                .Columns(6).HeaderCell.Value = "Date Modified"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Sub loadcriticalstocks()
        fields = "`product_ingredients`, `sku`, `stock_primary`, `critical_limit`, `created_at`"
        GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:="loc_pos_inventory", datagrid:=DataGridViewCriticalStocks, errormessage:="", successmessage:="", fields:=fields, where:=" stock_status = 1 AND critical_limit >= stock_primary AND store_id = " & ClientStoreID)
        With DataGridViewCriticalStocks
            .Columns(0).HeaderCell.Value = "Product Name"
            .Columns(1).HeaderCell.Value = "Code"
            .Columns(2).HeaderCell.Value = "Quantity"
            .Columns(3).HeaderCell.Value = "Critical Limit"
            .Columns(4).HeaderCell.Value = "Date Modified"
        End With
    End Sub
    Sub loadfastmovingstock()
        Try
            fields = "`formula_id`, SUM(stock_primary)"
            GLOBAL_SELECT_ALL_FUNCTION(table:="loc_fm_stock GROUP by formula_id ORDER BY `SUM(stock_primary)` DESC", datagrid:=DataGridViewFASTMOVING, fields:=fields)
            For Each row As DataRow In dt.Rows
                row("formula_id") = GLOBAL_SELECT_FUNCTION_RETURN(table:="loc_product_formula", fields:="product_ingredients", returnvalrow:="product_ingredients", values:="formula_id ='" & row("formula_id") & "'")
            Next
            With DataGridViewFASTMOVING
                .Columns(0).HeaderCell.Value = "Product Ingredients"
                .Columns(1).HeaderCell.Value = "Total Stock Quantity"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            cloudconn.Close()
        End Try
    End Sub
    Private Sub loadpanelstockadjustment()
        Try
            fields = "`formula_id`, `product_ingredients`, `stock_primary`, `stock_secondary`, `stock_status`, `stock_no_of_servings`, `critical_limit`"
            GLOBAL_SELECT_ALL_FUNCTION("`loc_pos_inventory` WHERE `stock_status` = 1 AND `store_id` = " & ClientStoreID, fields, DataGridViewPanelStockAdjustment)
            With DataGridViewPanelStockAdjustment
                .Columns(0).Visible = False
                .Columns(1).HeaderText = "Ingredient"
                .Columns(2).Visible = False
                .Columns(3).Visible = False
                .Columns(4).Visible = False
                .Columns(5).HeaderText = "No. of servings"
                .Columns(6).Visible = False
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Sub loadcomboboxingredients()
        Try
            Dim sql = "SELECT product_ingredients FROM loc_pos_inventory"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            For i As Integer = 0 To dt.Rows.Count - 1 Step +1
                ComboBoxDESC.Items.Add(dt(i)(0))
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim DataTableInventory As New DataTable
    Dim DataTableFormula As New DataTable
    Dim inv
    Public Sub selectingredients()
        Try
            Dim sql = "SELECT inventory_id FROM loc_pos_inventory WHERE product_ingredients = '" & ComboBoxDESC.Text & "'"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            Dim inventory_id = dt(0)(0)
            Dim sql1 = "Select formula_id, serving_unit, serving_value FROM loc_product_formula WHERE formula_id = " & inventory_id & ""
            cmd = New MySqlCommand(sql1, LocalhostConn)
            da = New MySqlDataAdapter(cmd)
            Dim dt1 As DataTable = New DataTable
            da.Fill(dt1)
            For Each row As DataRow In dt1.Rows
                TextBoxSERVINGUNIT.Text = row("serving_unit")
                TextBoxSERVINGVAL.Text = row("serving_value")
                TextBoxINVENTORYID.Text = row("formula_id")
            Next
            Dim sql2 = "SELECT formula_id, stock_primary FROM loc_pos_inventory WHERE formula_id = " & inventory_id & " "
            cmd = New MySqlCommand(sql2, LocalhostConn)
            da = New MySqlDataAdapter(cmd)
            Dim dt2 As DataTable = New DataTable
            da.Fill(dt2)
            For Each row As DataRow In dt2.Rows
                TextBoxSTCKONHAND.Text = row("stock_primary")
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Sub loadstockentry()

        where = " date(log_date_time) = CURRENT_DATE() AND log_type = 'STOCK ENTRY' "
        fields = "`crew_id`, `log_type`, `log_description`, `log_date_time`"
        GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:="loc_system_logs", datagrid:=DataGridViewSTOCKENTRY, errormessage:="", successmessage:="", fields:=fields, where:=where)
        With DataGridViewSTOCKENTRY
            .Columns(0).HeaderText = "Service Crew"
            .Columns(1).Visible = False
            .Columns(2).HeaderText = "Description"
            .Columns(3).HeaderText = "Date and Time"
            For Each row As DataRow In dt.Rows
                row("crew_id") = GLOBAL_SELECT_FUNCTION_RETURN(table:="loc_users", fields:="full_name", returnvalrow:="full_name", values:="uniq_id ='" & row("crew_id") & "'")
            Next
        End With
    End Sub
    Dim inventoryid
    Private Sub DataGridViewPanelStockAdjustment_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewPanelStockAdjustment.CellClick
        Try
            TextBox1.Text = DataGridViewPanelStockAdjustment.SelectedRows(0).Cells(2).Value
            TextBox2.Text = DataGridViewPanelStockAdjustment.SelectedRows(0).Cells(3).Value
            Dim FormulaID As Integer = DataGridViewPanelStockAdjustment.SelectedRows(0).Cells(0).Value
            SelectFormula(FormulaID)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub SelectFormula(FormulaID)
        Try
            Dim sql As String = "SELECT `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings` FROM loc_product_formula WHERE formula_id = " & FormulaID
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            For Each row As DataRow In dt.Rows
                TextBox3.Text = row("primary_unit")
                TextBox4.Text = row("primary_value")
                TextBox5.Text = row("secondary_unit")
                TextBox6.Text = row("secondary_value")
                TextBox7.Text = row("serving_unit")
                TextBox8.Text = row("serving_value")
                TextBox9.Text = row("no_servings")
            Next
            LocalhostConn.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Sub loadstockadjustmentreport(searchdate As Boolean)
        Try
            fields = "`crew_id`, `log_type`, `log_description`, `log_date_time`, `log_store`, `guid`, `loc_systemlog_id`, `synced`"
            table = "loc_system_logs"
            If searchdate = False Then
                where = " date(log_date_time) = CURRENT_DATE() AND log_type IN('NEW STOCK ADDED','STOCK REMOVAL','STOCK TRANSFER')"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewStockAdjustment, errormessage:="", fields:=fields, successmessage:="", where:=where)
            Else
                where = " log_type IN('NEW STOCK ADDED','STOCK REMOVAL','STOCK TRANSFER') AND date(log_date_time) >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' AND date(log_date_time) <= '" & Format(DateTimePicker2.Value, "yyyy-MM-dd") & "'"
                GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewStockAdjustment, errormessage:="", fields:=fields, successmessage:="", where:=where)
            End If
            With DataGridViewStockAdjustment
                .Columns(0).HeaderText = "Service Crew"
                .Columns(1).HeaderText = "Action"
                .Columns(2).HeaderText = "Description"
                .Columns(3).HeaderText = "Date and Time"
                .Columns(4).Visible = False
                .Columns(5).Visible = False
                .Columns(6).Visible = False
                .Columns(7).Visible = False
                For Each row As DataRow In dt.Rows
                    row("crew_id") = GLOBAL_SELECT_FUNCTION_RETURN(table:="loc_users", fields:="full_name", returnvalrow:="full_name", values:="uniq_id ='" & row("crew_id") & "'")
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        loadpanelstockadjustment()
        '
        PanelSTOCKADJUSTMENT.Location = New Point(ClientSize.Width / 2 - PanelSTOCKADJUSTMENT.Size.Width / 2, ClientSize.Height / 2 - PanelSTOCKADJUSTMENT.Size.Height / 2)
        PanelSTOCKADJUSTMENT.Anchor = AnchorStyles.None

        'PanelSTOCKADJUSTMENT.Top = (Me.Height - PanelSTOCKADJUSTMENT.Height) / 5
        'PanelSTOCKADJUSTMENT.Left = (Me.Width - PanelSTOCKADJUSTMENT.Width) / 4
        PanelSTOCKADJUSTMENT.Visible = True
        countingredients()
    End Sub

    Sub countingredients()
        Label7.Text = "(" & count(table:="loc_pos_inventory", tocount:="inventory_id") & ") record(s) count"
    End Sub
    Dim totalqty As Integer
    'Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
    '    If String.IsNullOrWhiteSpace(TextBoxName.Text) Then
    '        MsgBox("Select item first")
    '    ElseIf String.IsNullOrWhiteSpace(TextBoxADJQTY.Text) Then
    '        MsgBox("Input Quantity first")
    '    ElseIf String.IsNullOrWhiteSpace(ComboBoxAction.Text) Then
    '        MsgBox("Select action first")
    '    ElseIf String.IsNullOrWhiteSpace(TextBoxReason.Text) Then
    '        MsgBox("Input reason first")
    '    Else
    '        If ComboBoxAction.SelectedIndex = 0 Then
    '            totalqty = Val(TextBoxStockOnhand.Text) + Val(TextBoxADJQTY.Text)
    '            SystemLogType = "NEW STOCK ADDED"
    '            SystemLogDesc = "Adding stock of: " & TextBoxName.Text & " QTY: " & TextBoxADJQTY.Text & " Reason: " & TextBoxReason.Text
    '            SaveLog()
    '        ElseIf ComboBoxAction.SelectedIndex = 1 Then
    '            totalqty = Val(TextBoxStockOnhand.Text) - Val(TextBoxADJQTY.Text)
    '            SystemLogType = "STOCK REMOVAL"
    '            SystemLogDesc = "Removing stock of: " & TextBoxName.Text & " QTY: " & TextBoxADJQTY.Text & " Reason: " & TextBoxReason.Text
    '            SaveLog()
    '        ElseIf ComboBoxAction.SelectedIndex = 2 Then
    '            If ComboBoxOutlets.SelectedIndex = -1 Then
    '                MsgBox("Select outlet first")
    '            Else
    '                totalqty = Val(TextBoxStockOnhand.Text) - Val(TextBoxADJQTY.Text)
    '                SystemLogType = "STOCK TRANSFER"
    '                SystemLogDesc = "Transfer stock to: " & ComboBoxOutlets.Text & " Item: " & TextBoxName.Text & " QTY: " & TextBoxADJQTY.Text & " Reason: " & TextBoxReason.Text
    '                SaveLog()
    '            End If
    '        End If
    '    End If
    'End Sub
    'Private Sub SaveLog()
    '    table = " loc_pos_inventory "
    '    fields = "`stock_primary`= " & totalqty & ", `stock_secondary`= " & totalqty * Val(TextBox1.Text) & ",`synced`= 'Unsynced'"
    '    where = " inventory_id = " & inventoryid
    '    GLOBAL_FUNCTION_UPDATE(table:=table, fields:=fields, where:=where)
    '    GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
    '    loadpanelstockadjustment()
    '    loadinventory()
    '    loadstockadjustmentreport(False)
    '    loadcriticalstocks()
    '    MDIFORM.LabelTotalAvailStock.Text = sum(table:="loc_pos_inventory WHERE store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'", tototal:="stock_primary")
    '    MDIFORM.LabelTotalCrititems.Text = count(table:="loc_pos_inventory WHERE stock_status = 1 AND critical_limit >= stock_primary AND store_id ='" & ClientStoreID & "' AND guid = '" & ClientGuid & "'", tocount:="inventory_id")
    '    MDIFORM.LabelTotalAvailStock.Text = sum(table:="loc_pos_inventory WHERE store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'", tototal:="stock_primary")
    '    ClearTextBox(Panel23)
    'End Sub

    Private Sub ButtonENTRYADDSTOCK_Click(sender As Object, e As EventArgs) Handles ButtonENTRYADDSTOCK.Click
        Try
            If Val(TextBoxENTRYQTY.Text) > 0 Then
                totalqty = TextBoxENTRYTOTALQTY.Text
                table = " loc_pos_inventory "
                fields = "`stock_primary`= " & totalqty & ", `stock_secondary`= " & totalqty * Val(TextBoxSERVINGVAL.Text) & ",`synced`= 'Unsynced'"
                where = " inventory_id = " & TextBoxINVENTORYID.Text
                GLOBAL_FUNCTION_UPDATE(table:=table, fields:=fields, where:=where)
                SystemLogType = "STOCK ENTRY"
                SystemLogDesc = ComboBoxDESC.Text & " | Qty: " & TextBoxENTRYQTY.Text
                GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
                ClearTextBox(Panel21)
                ComboBoxDESC.SelectedIndex = 0
                TextBoxENTRYTOTALQTY.Text = 0
                loadstockentry()
                loadpanelstockadjustment()
                loadinventory()
                loadstockadjustmentreport(False)
                loadcriticalstocks()
            Else
                MsgBox("Zero Quantity ????")
            End If
            MDIFORM.LabelTotalAvailStock.Text = sum(table:="loc_pos_inventory WHERE store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'", tototal:="stock_primary")
            MDIFORM.LabelTotalCrititems.Text = count(table:="loc_pos_inventory WHERE stock_status = 1 AND critical_limit >= stock_primary AND store_id ='" & ClientStoreID & "' AND guid = '" & ClientGuid & "'", tocount:="inventory_id")
        Catch ex As Exception
            MsgBox("Error 2.0.1" & vbNewLine & "Please let us know whether you are still facing the problem.")
            messageboxappearance = False
            SystemLogType = "ERROR"
            SystemLogDesc = "Error 2.0 UPDATE OF CUSTOM PRODUCT: " & ex.ToString
            GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
        End Try
    End Sub
    Private Sub TextBoxENTRYQTY_TextChanged(sender As Object, e As EventArgs) Handles TextBoxENTRYQTY.TextChanged
        TextBoxENTRYTOTALQTY.Text = Val(TextBoxENTRYQTY.Text) + Val(TextBoxSTCKONHAND.Text)
    End Sub
    Private Sub TextBoxSTCKONHAND_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSTCKONHAND.TextChanged
        If TextBoxSTCKONHAND.Text = "" Then
            TextBoxENTRYQTY.ReadOnly = True
        Else
            TextBoxENTRYQTY.ReadOnly = False
        End If
        TextBoxENTRYTOTALQTY.Text = Val(TextBoxSTCKONHAND.Text) + Val(TextBoxENTRYQTY.Text)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If DateTimePicker1.Value.Date > DateTimePicker2.Value.Date Then
            MsgBox("")
        Else
            loadstockadjustmentreport(True)
        End If
    End Sub

    'Private Sub ComboBoxAction_SelectedIndexChanged(sender As Object, e As EventArgs)
    '    If ComboBoxAction.Text = "Transfer" Then
    '        ComboBoxOutlets.Visible = True
    '        Label10.Visible = True
    '    Else
    '        ComboBoxOutlets.Visible = False
    '        Label10.Visible = False
    '    End If
    'End Sub

    Private Sub ComboBoxDESC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxDESC.SelectedIndexChanged
        selectingredients()
    End Sub

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        Try
            If TextboxIsEmpty(Panel23) Then
                If ComboBoxAction.Text = "ADD" Then
                    SystemLogType = "NEW STOCK ADDED"
                    SystemLogDesc = "Adding stock of: " & TextBox1.Text & " QTY: " & TextBox10.Text & " Reason: " & TextBox11.Text
                    Dim stockqty = Double.Parse(TextBox1.Text) + Double.Parse(TextBox10.Text)
                    Dim stocktotal = stockqty * Double.Parse(TextBox6.Text)
                    Dim noofservings = stockqty * Double.Parse(TextBox9.Text)
                    Dim table = "loc_pos_inventory"
                    Dim fields = "`stock_primary`=" & stockqty & ",`stock_secondary`= " & stocktotal & " , `stock_no_of_servings`= " & noofservings
                    Dim where = "formula_id = " & DataGridViewPanelStockAdjustment.SelectedRows(0).Cells(0).Value
                    GLOBAL_FUNCTION_UPDATE(table, fields, where)
                ElseIf ComboBoxAction.Text = "TRANSFER" Then
                    SystemLogType = "STOCK TRANSFER"
                    SystemLogDesc = "Transfer stock to: " & ComboBoxtransfer.Text & " Item: " & TextBox1.Text & " QTY: " & TextBox10.Text & " Reason: " & TextBox11.Text
                    Dim stockqty = Double.Parse(TextBox1.Text) - Double.Parse(TextBox10.Text)
                    Dim stocktotal = stockqty * Double.Parse(TextBox6.Text)
                    Dim noofservings = stockqty * Double.Parse(TextBox9.Text)
                    Dim table = "loc_pos_inventory"
                    Dim fields = "`stock_primary`=" & stockqty & ",`stock_secondary`= " & stocktotal & " , `stock_no_of_servings`= " & noofservings
                    Dim where = "formula_id = " & DataGridViewPanelStockAdjustment.SelectedRows(0).Cells(0).Value
                    GLOBAL_FUNCTION_UPDATE(table, fields, where)
                ElseIf ComboBoxAction.Text = "DEDUCT" Then
                    SystemLogType = "STOCK REMOVAL"
                    SystemLogDesc = "Removing stock of: " & TextBox1.Text & " QTY: " & TextBox10.Text & " Reason: " & TextBox11.Text
                    Dim stockqty = Double.Parse(TextBox1.Text) - Double.Parse(TextBox10.Text)
                    Dim stocktotal = stockqty * Double.Parse(TextBox6.Text)
                    Dim noofservings = stockqty * Double.Parse(TextBox9.Text)
                    Dim table = "loc_pos_inventory"
                    Dim fields = "`stock_primary`=" & stockqty & ",`stock_secondary`= " & stocktotal & " , `stock_no_of_servings`= " & noofservings
                    Dim where = "formula_id = " & DataGridViewPanelStockAdjustment.SelectedRows(0).Cells(0).Value
                    GLOBAL_FUNCTION_UPDATE(table, fields, where)
                End If
                GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
                loadpanelstockadjustment()
                loadinventory()
                loadstockadjustmentreport(False)
                loadcriticalstocks()
                MDIFORM.LabelTotalAvailStock.Text = sum(table:="loc_pos_inventory WHERE store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'", tototal:="stock_primary")
                MDIFORM.LabelTotalCrititems.Text = count(table:="loc_pos_inventory WHERE stock_status = 1 AND critical_limit >= stock_primary AND store_id ='" & ClientStoreID & "' AND guid = '" & ClientGuid & "'", tocount:="inventory_id")
                MDIFORM.LabelTotalAvailStock.Text = sum(table:="loc_pos_inventory WHERE store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'", tototal:="stock_primary")
            Else
                MsgBox("Fill up all empty fields")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub ComboBoxAction_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxAction.SelectedIndexChanged
        Try
            If ComboBoxAction.Text = "DEDUCT" Then
                ComboBoxDeduction.Enabled = True
                ComboBoxtransfer.Enabled = False
            ElseIf ComboBoxAction.Text = "TRANSFER" Then
                ComboBoxDeduction.Enabled = False
                ComboBoxtransfer.Enabled = True
            Else
                ComboBoxDeduction.Enabled = False
                ComboBoxtransfer.Enabled = False
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PanelSTOCKADJUSTMENT.Visible = False
    End Sub
End Class