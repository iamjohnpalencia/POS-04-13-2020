Imports MySql.Data.MySqlClient
Module POSMODULE
    Dim DatagridviewRowIndex As Integer
    Dim servingtotal
    Public Sub new_product_button_click(ByVal sender As Object, ByVal e As EventArgs)
        DatagridviewRowIndex = getCurrentCellButton_Click(sender, e)
        Try
            deleteitem = False
            Dim btn As Button = DirectCast(sender, Button)
            Dim price As String = btn.Text
            Dim name As String = btn.Name
            With POS
                .TextBoxPRICE.Text = Val(price)
                .TextBoxNAME.Text = name
                If .TextBoxQTY.Text = 0 Then
                    hastextboxqty = False
                    retrieveformulaids()
                    .Label76.Text = SumOfColumnsToDecimal(.DataGridViewOrders, 3)
                    .Label11.Focus()
                Else
                    hastextboxqty = True
                    retrieveformulaids()
                    .Label76.Text = SumOfColumnsToDecimal(.DataGridViewOrders, 3)
                    .Label11.Focus()
                End If
                'End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub retrieveformulaids()
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim sql = "SELECT product_id, product_sku, formula_id, product_category, origin, server_inventory_id, addontype FROM `loc_admin_products` WHERE product_name = '" & POS.TextBoxNAME.Text & "'"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, ConnectionLocal)
            Using readerObj As MySqlDataReader = cmd.ExecuteReader
                While readerObj.Read
                    Dim formula_id = readerObj("formula_id")
                    Dim product_id = readerObj("product_id").ToString
                    Dim product_sku = readerObj("product_sku").ToString
                    Dim product_category = readerObj("product_category").ToString
                    Dim origin = readerObj("origin").ToString
                    Dim inventoryid = readerObj("server_inventory_id").ToString
                    Dim addontype = readerObj("addontype").ToString
                    With POS
                        If POS.WaffleUpgrade = True Then
                            Dim formulaId As String = ""
                            Dim str As String = formula_id
                            Dim words As String() = str.Split(New Char() {","c})
                            Dim word As String
                            For Each word In words
                                If word.Length = 1 Then
                                    If word = S_Batter Then
                                        word = word.Replace(word, S_Brownie_Mix)
                                        formulaId += word & ","
                                    Else
                                        formulaId += word & ","
                                    End If
                                Else
                                    formulaId += word & ","
                                End If
                            Next
                            formula_id = formulaId.Substring(0, formulaId.Length - 1)

                        End If
                        .TextBoxFormulaID.Text = formula_id
                        checkcriticallimit(formula_id:=formula_id, ID:=product_id, SKU:=product_sku, CAT:=product_category, ORIGIN:=origin, INVID:=inventoryid, addontype:=addontype)
                    End With
                End While
            End Using
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        Finally
            LocalhostConn.close
            cmd.Dispose()
        End Try
    End Sub
    Public HASOTHERSLOCALPRODUCT As Boolean = False
    Public HASOTHERSSERVERPRODUCT As Boolean = False
    Public Sub preventdgvordersdup(ByVal price, ByVal name, ByVal ID, ByVal SKU, ByVal CAT, ByVal ORIGIN, ByVal INVID, ByVal addontype)
        Try

            With POS
                Dim TotalPrice As Double = 0
                If S_ZeroRated = "0" Then
                    TotalPrice = 1 * Val(.TextBoxPRICE.Text)
                Else
                    Dim Tax = 1 + Val(S_Tax)
                    Dim ZeroRated = Val(.TextBoxPRICE.Text) / Tax
                    TotalPrice = Math.Round(ZeroRated, 2, MidpointRounding.AwayFromZero)
                End If
                .TextBoxPRICE.Text = Val(price)
                .TextBoxNAME.Text = name
                If CAT = "Add-Ons" Then
                    .ButtonClickCount += 1
                    .TextBoxINC.Text = POS.ButtonClickCount
                    Dim test2 As Boolean = False
                    If addontype = "Classic" Then
                        For Each row In .DataGridViewOrders.Rows
                            If .TextBoxNAME.Text = row.Cells("Column1").value Then
                                If row.Cells("Column17").value = .DataGridViewOrders.SelectedRows(0).Cells(5).Value Then
                                    test2 = True
                                    Exit For
                                End If
                            End If
                        Next
                    Else
                        For Each row In .DataGridViewOrders.Rows
                            If .TextBoxNAME.Text = row.Cells("Column1").value Then
                                test2 = True
                                Exit For
                            End If
                        Next
                    End If
                    If test2 = False Then
                        If .DataGridViewOrders.Rows.Count > 0 Then
                            If addontype = "Classic" Then
                                If .DataGridViewOrders.SelectedRows(0).Cells(7).Value = "Add-Ons" Then
                                    MsgBox("Select product")
                                ElseIf .DataGridViewOrders.SelectedRows(0).Cells(7).Value = "Others" Then
                                    MsgBox("Add-Ons are exclusive for fbw waffles only")
                                Else
                                    ThisIsMyInventoryID = .TextBoxINC.Text
                                    If hastextboxqty = False Then
                                        .DataGridViewOrders.Rows.Insert(DatagridviewRowIndex + 1, name, 1, .TextBoxPRICE.Text, 1 * Val(.TextBoxPRICE.Text), .TextBoxINC.Text, ID, SKU, CAT, .DataGridViewOrders.SelectedRows(0).Cells(5).Value.ToString, .DataGridViewOrders.SelectedRows(0).Cells(9).Value, INVID, 0, ORIGIN, addontype)
                                    Else
                                        .DataGridViewOrders.Rows.Insert(DatagridviewRowIndex + 1, name, .TextBoxQTY.Text, .TextBoxPRICE.Text, .TextBoxQTY.Text * Val(.TextBoxPRICE.Text), .TextBoxINC.Text, ID, SKU, CAT, .DataGridViewOrders.SelectedRows(0).Cells(5).Value.ToString, .DataGridViewOrders.SelectedRows(0).Cells(9).Value, INVID, .TextBoxQTY.Text, ORIGIN, addontype)
                                    End If
                                End If
                            Else
                                ThisIsMyInventoryID = .TextBoxINC.Text
                                If hastextboxqty = False Then
                                    .DataGridViewOrders.Rows.Add(name, 1, .TextBoxPRICE.Text, TotalPrice, .TextBoxINC.Text, ID, SKU, CAT, ID, "AOPREMIUM", INVID, 0, ORIGIN, addontype)
                                Else
                                    .DataGridViewOrders.Rows.Add(name, 1, .TextBoxPRICE.Text, TotalPrice, .TextBoxINC.Text, ID, SKU, CAT, ID, "AOPREMIUM", INVID, 0, ORIGIN, addontype)
                                End If
                            End If
                        Else
                            If addontype = "Classic" Then
                                MsgBox("Select product first")
                            Else
                                ThisIsMyInventoryID = .TextBoxINC.Text
                                If hastextboxqty = False Then
                                    .DataGridViewOrders.Rows.Add(name, 1, .TextBoxPRICE.Text, 1 * TotalPrice, .TextBoxINC.Text, ID, SKU, CAT, ID, "AOPREMIUM", INVID, 0, ORIGIN, addontype)
                                Else
                                    .DataGridViewOrders.Rows.Add(name, 1, .TextBoxPRICE.Text, 1 * TotalPrice, .TextBoxINC.Text, ID, SKU, CAT, ID, "AOPREMIUM", INVID, 0, ORIGIN, addontype)
                                End If
                            End If
                        End If
                    Else
                        For i As Integer = 0 To POS.DataGridViewOrders.Rows.Count - 1 Step +1
                            If .TextBoxNAME.Text = .DataGridViewOrders.Rows(i).Cells(0).Value.ToString() Then
                                If addontype = "Classic" Then
                                    If .DataGridViewOrders.SelectedRows(0).Cells(5).Value = .DataGridViewOrders.Rows(i).Cells(8).Value Then
                                        If hastextboxqty = False Then
                                            .DataGridViewOrders.Rows(i).Cells(1).Value = .DataGridViewOrders.Rows(i).Cells(1).Value + 1
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            ThisIsMyInventoryID = .DataGridViewOrders.Rows(i).Cells(4).Value
                                        Else
                                            .DataGridViewOrders.Rows(i).Cells(1).Value = .TextBoxQTY.Text
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = .TextBoxQTY.Text * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            ThisIsMyInventoryID = .DataGridViewOrders.Rows(i).Cells(4).Value
                                        End If
                                        .ButtonPayMent.Enabled = True
                                        .Buttonholdoder.Enabled = True
                                        .ButtonPendingOrders.Enabled = False
                                    End If
                                Else
                                    If hastextboxqty = False Then
                                        .DataGridViewOrders.Rows(i).Cells(1).Value = .DataGridViewOrders.Rows(i).Cells(1).Value + 1
                                        .DataGridViewOrders.Rows(i).Cells(3).Value = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                        ThisIsMyInventoryID = .DataGridViewOrders.Rows(i).Cells(4).Value
                                    Else
                                        .DataGridViewOrders.Rows(i).Cells(1).Value = .TextBoxQTY.Text
                                        .DataGridViewOrders.Rows(i).Cells(3).Value = .TextBoxQTY.Text * .DataGridViewOrders.Rows(i).Cells(2).Value
                                        ThisIsMyInventoryID = .DataGridViewOrders.Rows(i).Cells(4).Value
                                    End If
                                    .ButtonPayMent.Enabled = True
                                    .Buttonholdoder.Enabled = True
                                    .ButtonPendingOrders.Enabled = False
                                End If
                            End If
                        Next
                    End If
                Else
                    Dim test As Boolean = False
                    For Each row In .DataGridViewOrders.Rows
                        If .TextBoxNAME.Text = row.Cells("Column1").Value Then
                            test = True
                            Exit For
                        End If
                    Next
                    If test = False Then
                        .ButtonClickCount += 1
                        .TextBoxINC.Text = POS.ButtonClickCount
                        If hastextboxqty = False Then
                            If CAT = "Famous Blends" Then
                                DISABLESERVEROTHERSPRODUCT = True
                                .DataGridViewOrders.Rows.Add(name, 1, .TextBoxPRICE.Text, 1 * TotalPrice, .TextBoxINC.Text, ID, SKU, CAT, ID, "DRINKS", INVID, 0, ORIGIN, addontype)
                            ElseIf CAT = "Others" Then
                                If ORIGIN = "Server" Then
                                    If DISABLESERVEROTHERSPRODUCT = False Then
                                        For Each row In .DataGridViewOrders.Rows
                                            If row.Cells("Column42").Value = "Local" Then
                                                HASOTHERSLOCALPRODUCT = True
                                                Exit For
                                            End If
                                        Next
                                        If HASOTHERSLOCALPRODUCT = False Then
                                            .DataGridViewOrders.Rows.Add(name, 1, .TextBoxPRICE.Text, 1 * TotalPrice, .TextBoxINC.Text, ID, SKU, CAT, ID, "OTHERS", INVID, 0, ORIGIN, addontype)
                                            .ButtonTransactionMode.Enabled = True
                                            .ButtonPayMent.Enabled = True
                                            .ButtonTransactionMode.Text = "Cancel"
                                            .ButtonPayMent.Text = "Mix"
                                            .Buttonholdoder.Enabled = False
                                            .ButtonPendingOrders.Enabled = False
                                            .Panel3.Enabled = False
                                            .ButtonWaffleUpgrade.Enabled = False
                                            .WaffleUpgrade = False
                                            .ButtonWaffleUpgrade.Text = "Waffle Upgrade"
                                        Else
                                            MsgBox("Remove product first")
                                        End If
                                    Else
                                        MsgBox("Remove product first")
                                    End If
                                Else
                                    For Each row In .DataGridViewOrders.Rows
                                        If row.Cells("Column42").Value = "Server" Then
                                            If row.cells("Column19").value = "OTHERS" Then
                                                HASOTHERSSERVERPRODUCT = True
                                            Else
                                                HASOTHERSSERVERPRODUCT = False
                                            End If
                                            Exit For
                                        End If
                                    Next
                                    If HASOTHERSSERVERPRODUCT = False Then
                                        .DataGridViewOrders.Rows.Add(name, 1, .TextBoxPRICE.Text, 1 * TotalPrice, .TextBoxINC.Text, ID, SKU, CAT, ID, "OTHERS", INVID, 0, ORIGIN, addontype)
                                        .ButtonTransactionMode.Enabled = True
                                        .ButtonPayMent.Enabled = True
                                        .Buttonholdoder.Enabled = True
                                        .ButtonPendingOrders.Enabled = False
                                        .WaffleUpgrade = False
                                        .ButtonWaffleUpgrade.Text = "Waffle Upgrade"
                                    Else
                                        MsgBox("Remove ingredient first")
                                    End If
                                End If
                            Else

                                DISABLESERVEROTHERSPRODUCT = True
                                If POS.WaffleUpgrade = True Then
                                    .DataGridViewOrders.Rows.Add(name, 1, .TextBoxPRICE.Text, 1 * Val(TotalPrice + Val(S_Upgrade_Price)), .TextBoxINC.Text, ID, SKU, CAT, ID, "WAFFLE", INVID, 1, ORIGIN, addontype)
                                Else
                                    .DataGridViewOrders.Rows.Add(name, 1, .TextBoxPRICE.Text, TotalPrice, .TextBoxINC.Text, ID, SKU, CAT, ID, "WAFFLE", INVID, 0, ORIGIN, addontype)
                                End If
                            End If
                        Else
                            If CAT = "Famous Blends" Then
                                DISABLESERVEROTHERSPRODUCT = True
                                .DataGridViewOrders.Rows.Add(name, Val(.TextBoxQTY.Text), TotalPrice, Val(.TextBoxQTY.Text) * Val(.TextBoxPRICE.Text), .TextBoxINC.Text, ID, SKU, CAT, ID, "DRINKS", INVID, 0, ORIGIN, addontype)
                            ElseIf CAT = "Others" Then
                                If ORIGIN = "Server" Then
                                    If DISABLESERVEROTHERSPRODUCT = False Then
                                        For Each row In .DataGridViewOrders.Rows
                                            If row.Cells("Column42").Value = "Local" Then
                                                HASOTHERSLOCALPRODUCT = True
                                                Exit For
                                            End If
                                        Next
                                        If HASOTHERSLOCALPRODUCT = False Then
                                            .DataGridViewOrders.Rows.Add(name, Val(.TextBoxQTY.Text), .TextBoxPRICE.Text, Val(.TextBoxQTY.Text) * TotalPrice, .TextBoxINC.Text, ID, SKU, CAT, ID, "OTHERS", INVID, 0, ORIGIN, addontype)
                                            .ButtonTransactionMode.Enabled = True
                                            .ButtonPayMent.Enabled = True
                                            .ButtonTransactionMode.Text = "Cancel"
                                            .ButtonPayMent.Text = "Mix"
                                            .Buttonholdoder.Enabled = False
                                            .ButtonPendingOrders.Enabled = False
                                            .Panel3.Enabled = False
                                            .ButtonWaffleUpgrade.Enabled = False
                                            .WaffleUpgrade = False
                                            .ButtonWaffleUpgrade.Text = "Waffle Upgrade"
                                        Else
                                            MsgBox("Remove product first")
                                        End If
                                    Else
                                        MsgBox("Remove product first")
                                    End If
                                Else
                                    For Each row In .DataGridViewOrders.Rows
                                        If row.Cells("Column42").Value = "Server" Then
                                            If row.cells("Column19").value = "OTHERS" Then
                                                HASOTHERSSERVERPRODUCT = True
                                            Else
                                                HASOTHERSSERVERPRODUCT = False
                                            End If
                                            Exit For
                                        End If
                                    Next
                                    If HASOTHERSSERVERPRODUCT = False Then
                                        .DataGridViewOrders.Rows.Add(name, Val(.TextBoxQTY.Text), .TextBoxPRICE.Text, Val(.TextBoxQTY.Text) * TotalPrice, .TextBoxINC.Text, ID, SKU, CAT, ID, "OTHERS", INVID, 0, ORIGIN, addontype)
                                        .ButtonTransactionMode.Enabled = True
                                        .ButtonPayMent.Enabled = True
                                        .Buttonholdoder.Enabled = True
                                        .ButtonPendingOrders.Enabled = False
                                        .WaffleUpgrade = False
                                        .ButtonWaffleUpgrade.Text = "Waffle Upgrade"
                                    Else
                                        MsgBox("Remove ingredient first")
                                    End If
                                End If
                            Else
                                DISABLESERVEROTHERSPRODUCT = True
                                If POS.WaffleUpgrade = True Then
                                    Dim priceadd = Val(.TextBoxQTY.Text) * S_Upgrade_Price
                                    .DataGridViewOrders.Rows.Add(name, Val(.TextBoxQTY.Text), .TextBoxPRICE.Text, Val(.TextBoxQTY.Text) * TotalPrice + priceadd, .TextBoxINC.Text, ID, SKU, CAT, ID, "WAFFLE", INVID, Val(.TextBoxQTY.Text), ORIGIN, addontype)
                                Else
                                    .DataGridViewOrders.Rows.Add(name, Val(.TextBoxQTY.Text), .TextBoxPRICE.Text, Val(.TextBoxQTY.Text) * TotalPrice, .TextBoxINC.Text, ID, SKU, CAT, ID, "WAFFLE", INVID, 0, ORIGIN, addontype)
                                End If
                            End If
                        End If
                        .TextBoxPressQTY.Text = 1
                        .ButtonPayMent.Enabled = True
                    Else
                        For i As Integer = 0 To POS.DataGridViewOrders.Rows.Count - 1 Step +1
                            If .DataGridViewOrders.Rows(i).Cells(0).Value.ToString() = name Then
                                .TextBoxPressQTY.Text = drasd
                                drasd = .DataGridViewOrders.Rows(i).Cells(1).Value.ToString()
                                If hastextboxqty = False Then
                                    .DataGridViewOrders.Rows(i).Cells(1).Value = drasd + 1
                                    .TextBoxPressQTY.Text = drasd + 1
                                    If .WaffleUpgrade = True Then
                                        If CAT = "Others" Then
                                            .WaffleUpgrade = False
                                            .ButtonWaffleUpgrade.Text = "Waffle Upgrade"
                                        End If
                                        .DataGridViewOrders.Rows(i).Cells(11).Value += 1
                                        Dim AddPRice = Val(S_Upgrade_Price) * .DataGridViewOrders.Rows(i).Cells(11).Value
                                        .DataGridViewOrders.Rows(i).Cells(3).Value = .DataGridViewOrders.Rows(i).Cells(1).Value * TotalPrice + AddPRice
                                    Else
                                        Dim AddPRice = Val(S_Upgrade_Price) * .DataGridViewOrders.Rows(i).Cells(11).Value
                                        .DataGridViewOrders.Rows(i).Cells(3).Value = .DataGridViewOrders.Rows(i).Cells(1).Value * TotalPrice + AddPRice
                                    End If
                                Else
                                    .DataGridViewOrders.Rows(i).Cells(1).Value = drasd + Val(.TextBoxQTY.Text)
                                    .TextBoxPressQTY.Text = drasd + +Val(.TextBoxQTY.Text)
                                    If .WaffleUpgrade = True Then
                                        If CAT = "Others" Then
                                            .WaffleUpgrade = False
                                            .ButtonWaffleUpgrade.Text = "Waffle Upgrade"
                                        End If
                                        .DataGridViewOrders.Rows(i).Cells(11).Value += Val(.TextBoxQTY.Text)
                                        Dim AddPRice = Val(S_Upgrade_Price) * .DataGridViewOrders.Rows(i).Cells(11).Value
                                        .DataGridViewOrders.Rows(i).Cells(3).Value = .DataGridViewOrders.Rows(i).Cells(1).Value * TotalPrice + AddPRice
                                    Else
                                        Dim AddPRice = Val(S_Upgrade_Price) * .DataGridViewOrders.Rows(i).Cells(11).Value
                                        .DataGridViewOrders.Rows(i).Cells(3).Value = .DataGridViewOrders.Rows(i).Cells(1).Value * TotalPrice + AddPRice
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
                If .DataGridViewOrders.Rows.Count > 0 Then
                    .ButtonPayMent.Enabled = True
                    .Buttonholdoder.Enabled = True
                    .ButtonPendingOrders.Enabled = False
                Else
                    .ButtonPayMent.Enabled = False
                    .Buttonholdoder.Enabled = False
                    .ButtonPendingOrders.Enabled = True
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Dim DataTableCriticalLimit As New DataTable
    Dim DataAdapterCriticalLimit As MySqlDataAdapter
    Dim CmdCriticalLimit As MySqlCommand
    Dim ListOfIngredients As String
    Public Sub checkcriticallimit(ByVal formula_id, ByVal ID, ByVal SKU, ByVal CAT, ByVal ORIGIN, ByVal INVID, ByVal addontype)
        Try
            ListOfIngredients = ""
            sql = "SELECT product_ingredients, critical_limit, stock_primary, stock_secondary, stock_no_of_servings FROM `loc_pos_inventory` WHERE stock_primary <= critical_limit AND server_inventory_id IN (" & Trim(formula_id) & ");"
            CmdCriticalLimit = New MySqlCommand(sql, LocalhostConn())
            DataAdapterCriticalLimit = New MySqlDataAdapter(CmdCriticalLimit)
            DataTableCriticalLimit = New DataTable
            DataAdapterCriticalLimit.Fill(DataTableCriticalLimit)
            If DataTableCriticalLimit.Rows.Count > 0 Then
                For i As Integer = 0 To DataTableCriticalLimit.Rows.Count - 1 Step +1
                    ListOfIngredients += DataTableCriticalLimit(i)(0) & ", "
                Next
                Dim criticalmessage = ListOfIngredients.Substring(0, ListOfIngredients.Length - 2)
                Dim outofstock = MessageBox.Show("Item (" & criticalmessage & ") is out of stock, do you wish to continue?", "Out of Stock", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                If outofstock = DialogResult.Yes Then
                    preventdgvordersdup(price:=POS.TextBoxPRICE.Text, name:=POS.TextBoxNAME.Text, ID:=ID, SKU:=SKU, CAT:=CAT, ORIGIN:=ORIGIN, INVID:=INVID, addontype:=addontype)
                    retrieveanddeduct(formulaID:=POS.TextBoxFormulaID.Text, Cat:=CAT, Origin:=ORIGIN, addontype:=addontype)
                End If
            Else
                preventdgvordersdup(price:=POS.TextBoxPRICE.Text, name:=POS.TextBoxNAME.Text, ID:=ID, SKU:=SKU, CAT:=CAT, ORIGIN:=ORIGIN, INVID:=INVID, addontype:=addontype)
                retrieveanddeduct(formulaID:=POS.TextBoxFormulaID.Text, Cat:=CAT, Origin:=ORIGIN, addontype:=addontype)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        Finally
            DataAdapterCriticalLimit.Dispose()
            CmdCriticalLimit.Dispose()
            LocalhostConn.Close()
        End Try
    End Sub
    Public Sub retrieveanddeduct(ByVal formulaID, ByVal Cat, ByVal Origin, ByVal addontype)
        Try
            Dim dt As DataTable = New DataTable
            If Origin = "Server" Then
                dt = New DataTable
                dt.Columns.Add("serving_value")
                dt.Columns.Add("server_formula_id")
                dt.Columns.Add("unit_cost")

                Dim splitformulaID As String = formulaID
                Dim words As String() = splitformulaID.Split(New Char() {","c})
                Dim word As String
                For Each word In words
                    Dim sql = "SELECT serving_value, server_formula_id, unit_cost FROM `loc_product_formula` WHERE server_formula_id = " & word
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                    Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                    Dim dtfill As DataTable = New DataTable
                    da.Fill(dtfill)
                    Dim FRID As DataRow = dt.NewRow
                    FRID("serving_value") = dtfill(0)(0)
                    FRID("server_formula_id") = dtfill(0)(1)
                    FRID("unit_cost") = dtfill(0)(2)
                    dt.Rows.Add(FRID)
                    LocalhostConn.Close()
                Next
            Else
                dt = New DataTable
                dt.Columns.Add("serving_value")
                dt.Columns.Add("formula_id")
                dt.Columns.Add("unit_cost")

                Dim splitformulaID As String = formulaID
                Dim words As String() = splitformulaID.Split(New Char() {","c})
                Dim word As String
                For Each word In words
                    Dim sql = "SELECT serving_value, formula_id, unit_cost FROM `loc_product_formula` WHERE server_formula_id " & word
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                    Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                    Dim dtfill As DataTable = New DataTable
                    da.Fill(dtfill)
                    Dim FRID As DataRow = dt.NewRow
                    FRID("serving_value") = dtfill(0)(0)
                    FRID("formula_id") = dtfill(0)(1)
                    FRID("unit_cost") = dtfill(0)(2)
                    dt.Rows.Add(FRID)
                    LocalhostConn.Close()
                Next
            End If
            With POS
                If Cat = "Add-Ons" Then
                    Dim ID = ""
                    Dim test As Boolean = False
                    If addontype = "Classic" Then
                        If .DataGridViewOrders.Rows.Count > 0 Then
                            ID = .DataGridViewOrders.SelectedRows(0).Cells(5).Value
                            For Each row In .DataGridViewInv.Rows
                                If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                                    If .DataGridViewOrders.SelectedRows(0).Cells(5).Value = row.cells("Column18").value Then
                                        test = True
                                        Exit For
                                    End If
                                End If
                            Next
                        End If
                    Else
                        For Each row In .DataGridViewInv.Rows
                            If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                                test = True
                                Exit For
                            End If
                        Next
                    End If
                    If test = False Then
                        If addontype = "Classic" Then
                            If .DataGridViewOrders.Rows.Count > 0 Then
                                If .DataGridViewOrders.SelectedRows(0).Cells(7).Value.ToString <> "Add-Ons" Then
                                    For Each row As DataRow In dt.Rows
                                        If deleteitem = False Then
                                            If .TextBoxQTY.Text <> 0 Then
                                                servingtotal = Val(row("serving_value")) * Val(.TextBoxQTY.Text)
                                                If Origin = "Server" Then
                                                    .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), ID, Origin)
                                                Else
                                                    .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), ID, Origin)
                                                End If
                                            Else
                                                If .DataGridViewOrders.Rows.Count > 0 Then
                                                    If Origin = "Server" Then
                                                        .DataGridViewInv.Rows.Add(row("serving_value"), row("server_formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), ID, Origin)
                                                    Else
                                                        .DataGridViewInv.Rows.Add(row("serving_value"), row("formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), ID, Origin)

                                                    End If
                                                End If
                                            End If
                                        End If
                                    Next row
                                End If
                            End If
                        Else
                            'Premium
                            For Each row As DataRow In dt.Rows
                                If deleteitem = False Then
                                    If .TextBoxQTY.Text <> 0 Then
                                        servingtotal = Val(row("serving_value")) * Val(.TextBoxQTY.Text)
                                        If Origin = "Server" Then
                                            .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                        Else
                                            .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                        End If
                                    Else
                                        If .DataGridViewOrders.Rows.Count > 0 Then
                                            If Origin = "Server" Then
                                                .DataGridViewInv.Rows.Add(row("serving_value"), row("server_formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), 0, Origin)
                                            Else
                                                .DataGridViewInv.Rows.Add(row("serving_value"), row("formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), 0, Origin)

                                            End If
                                        End If
                                    End If
                                End If
                            Next row
                        End If
                    Else
                        For i As Integer = 0 To .DataGridViewInv.Rows.Count - 1 Step +1
                            If .TextBoxNAME.Text = .DataGridViewInv.Rows(i).Cells(4).Value.ToString Then
                                If addontype = "Classic" Then
                                    If .DataGridViewOrders.SelectedRows(0).Cells(5).Value = .DataGridViewInv.Rows(i).Cells(8).Value Then
                                        .DataGridViewInv.Rows(i).Cells(2).Value = .DataGridViewInv.Rows(i).Cells(2).Value + 1
                                        .DataGridViewInv.Rows(i).Cells(0).Value = .DataGridViewInv.Rows(i).Cells(2).Value * .DataGridViewInv.Rows(i).Cells(5).Value
                                        .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                    End If
                                Else
                                    .DataGridViewInv.Rows(i).Cells(2).Value = .DataGridViewInv.Rows(i).Cells(2).Value + 1
                                    .DataGridViewInv.Rows(i).Cells(0).Value = .DataGridViewInv.Rows(i).Cells(2).Value * .DataGridViewInv.Rows(i).Cells(5).Value
                                    .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                End If
                            End If
                        Next
                    End If

                ElseIf Cat = "Others" Then
                    If Origin = "Server" Then
                        If DISABLESERVEROTHERSPRODUCT = False Then
                            If HASOTHERSLOCALPRODUCT = False Then
                                Dim test As Boolean = False
                                For Each row In .DataGridViewInv.Rows
                                    If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                                        test = True
                                        Exit For
                                    End If
                                Next
                                If test = False Then
                                    For Each row As DataRow In dt.Rows
                                        If deleteitem = False Then
                                            If .TextBoxQTY.Text <> 0 Then
                                                servingtotal = Val(row("serving_value")) * Val(.TextBoxQTY.Text)
                                                If Origin = "Server" Then
                                                    .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                                Else
                                                    .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                                End If
                                            Else
                                                servingtotal = Val(row("serving_value")) * Val(.TextBoxPressQTY.Text)
                                                If Origin = "Server" Then
                                                    .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), .TextBoxPressQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), 0, Origin)
                                                Else
                                                    .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxPressQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), 0, Origin)
                                                End If
                                            End If
                                        End If
                                    Next row
                                Else
                                    For i As Integer = 0 To .DataGridViewInv.Rows.Count - 1 Step +1
                                        If .TextBoxNAME.Text = .DataGridViewInv.Rows(i).Cells(4).Value.ToString() Then
                                            For a As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                                                .DataGridViewInv.Rows(i).Cells(2).Value = .TextBoxPressQTY.Text
                                                .DataGridViewInv.Rows(i).Cells(0).Value = Val(.TextBoxPressQTY.Text) * Val(.DataGridViewInv.Rows(i).Cells(5).Value)
                                                .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                            Next
                                        End If
                                    Next
                                End If
                                .TextBoxQTY.Text = 0
                            End If
                        End If
                    Else
                        If HASOTHERSSERVERPRODUCT = False Then
                            Dim test As Boolean = False
                            For Each row In .DataGridViewInv.Rows
                                If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                                    test = True
                                    Exit For
                                End If
                            Next
                            If test = False Then
                                For Each row As DataRow In dt.Rows
                                    If deleteitem = False Then
                                        If .TextBoxQTY.Text <> 0 Then
                                            servingtotal = Val(row("serving_value")) * Val(.TextBoxQTY.Text)
                                            If Origin = "Server" Then
                                                .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                            Else
                                                .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                            End If
                                        Else
                                            servingtotal = Val(row("serving_value")) * Val(.TextBoxPressQTY.Text)
                                            If Origin = "Server" Then
                                                .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), .TextBoxPressQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), 0, Origin)
                                            Else
                                                .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxPressQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), 0, Origin)
                                            End If
                                        End If
                                    End If
                                Next row
                            Else
                                For i As Integer = 0 To .DataGridViewInv.Rows.Count - 1 Step +1
                                    If .TextBoxNAME.Text = .DataGridViewInv.Rows(i).Cells(4).Value.ToString() Then
                                        For a As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                                            .DataGridViewInv.Rows(i).Cells(2).Value = .TextBoxPressQTY.Text
                                            .DataGridViewInv.Rows(i).Cells(0).Value = Val(.TextBoxPressQTY.Text) * Val(.DataGridViewInv.Rows(i).Cells(5).Value)
                                            .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                        Next
                                    End If
                                Next
                            End If
                            .TextBoxQTY.Text = 0
                        End If
                    End If
                Else
                    ' Waffle
                    Dim test As Boolean = False
                    For Each row In .DataGridViewInv.Rows
                        If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                            test = True
                            Exit For
                        End If
                    Next
                    If test = False Then
                        For Each row As DataRow In dt.Rows
                            If deleteitem = False Then
                                If .TextBoxQTY.Text <> 0 Then
                                    servingtotal = Val(row("serving_value")) * Val(.TextBoxQTY.Text)
                                    If Origin = "Server" Then
                                        .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                    Else
                                        .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                    End If
                                Else
                                    servingtotal = Val(row("serving_value")) * Val(.TextBoxPressQTY.Text)
                                    If Origin = "Server" Then
                                        .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                    Else
                                        .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                    End If
                                End If
                            End If
                        Next row
                    Else
                        ' UPGRADE FUNCTION
                        If .WaffleUpgrade = True Then
                            Dim BrownieExist As Boolean = False
                            For Each row In .DataGridViewInv.Rows
                                If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                                    If S_Brownie_Mix = row.cells("Column6").value Then
                                        BrownieExist = True
                                        Exit For
                                    End If
                                End If
                            Next
                            If BrownieExist = False Then
                                Dim query = "SELECT serving_value, server_formula_id, unit_cost FROM `loc_product_formula` WHERE server_formula_id = " & S_Brownie_Mix
                                Dim cmd As MySqlCommand = New MySqlCommand(query, LocalhostConn)
                                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                                Dim dtt As DataTable = New DataTable
                                da.Fill(dtt)
                                If Val(.TextBoxQTY.Text) <> 0 Then
                                    .DataGridViewInv.Rows.Add(dtt(0)(0), dtt(0)(1), Val(.TextBoxQTY.Text), .TextBoxINC.Text, .TextBoxNAME.Text, dtt(0)(0), dtt(0)(2) * Val(.TextBoxQTY.Text), dtt(0)(2), 0, Origin)
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Batter Then
                                            If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                                row.Cells("Column7").Value += Val(.TextBoxQTY.Text)
                                                row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            End If
                                        End If
                                    Next
                                Else
                                    .DataGridViewInv.Rows.Add(dtt(0)(0), dtt(0)(1), 1, .TextBoxINC.Text, .TextBoxNAME.Text, dtt(0)(0), dtt(0)(2) * 1, dtt(0)(2), 0, Origin)
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Batter Then
                                            If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                                row.Cells("Column7").Value = .TextBoxPressQTY.Text
                                                row.Cells("Column5").Value = Val(.TextBoxPressQTY.Text) * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            End If
                                        End If
                                    Next
                                End If
                            Else
                                If Val(.TextBoxQTY.Text) <> 0 Then
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Batter Then
                                            If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                                row.Cells("Column7").Value += Val(.TextBoxQTY.Text)
                                                row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            Else
                                                row.Cells("Column7").Value += Val(.TextBoxQTY.Text)
                                                row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            End If
                                        End If
                                    Next
                                Else
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Batter Then
                                            If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                                row.Cells("Column7").Value = .TextBoxPressQTY.Text
                                                row.Cells("Column5").Value = Val(.TextBoxPressQTY.Text) * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            Else
                                                row.Cells("Column7").Value += 1
                                                row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            End If
                                        End If
                                    Next

                                End If
                            End If
                        Else
                            ' FAMOUS BATTER
                            Dim BatterExist As Boolean = False
                            For Each row In .DataGridViewInv.Rows
                                If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                                    If S_Batter = row.cells("Column6").value Then
                                        BatterExist = True
                                        Exit For
                                    End If
                                End If
                            Next
                            If BatterExist = False Then
                                Dim query = "SELECT serving_value, server_formula_id, unit_cost FROM `loc_product_formula` WHERE server_formula_id = " & S_Batter
                                Dim cmd As MySqlCommand = New MySqlCommand(query, LocalhostConn)
                                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                                Dim dt1 As DataTable = New DataTable
                                da.Fill(dt1)
                                If Val(.TextBoxQTY.Text) <> 0 Then
                                    .DataGridViewInv.Rows.Add(dt1(0)(0), dt1(0)(1), Val(.TextBoxQTY.Text), .TextBoxINC.Text, .TextBoxNAME.Text, dt1(0)(0), dt1(0)(2) * Val(.TextBoxQTY.Text), dt1(0)(2), 0, Origin)
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        MsgBox(row.Cells("Column6").Value)
                                        If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                            If row.Cells("Column6").Value <> S_Batter Then
                                                row.Cells("Column7").Value += Val(.TextBoxQTY.Text)
                                                row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            End If
                                        End If
                                    Next
                                Else
                                    .DataGridViewInv.Rows.Add(dt1(0)(0), dt1(0)(1), 1, .TextBoxINC.Text, .TextBoxNAME.Text, dt1(0)(0), dt1(0)(2) * 1, dt1(0)(2), 0, Origin)
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                            If row.Cells("Column6").Value <> S_Batter Then
                                                row.Cells("Column7").Value = .TextBoxPressQTY.Text
                                                row.Cells("Column5").Value = Val(.TextBoxPressQTY.Text) * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            End If
                                        End If
                                    Next
                                End If
                            Else
                                If Val(.TextBoxQTY.Text) <> 0 Then
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                            If row.Cells("Column6").Value <> S_Batter Then
                                                row.Cells("Column7").Value = row.Cells("Column7").Value + Val(.TextBoxQTY.Text)
                                                row.Cells("Column5").Value = Val(.TextBoxPressQTY.Text) * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            Else
                                                row.Cells("Column7").Value += Val(.TextBoxQTY.Text)
                                                row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            End If
                                        End If
                                    Next
                                Else
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                            If row.Cells("Column6").Value <> S_Batter Then
                                                row.Cells("Column7").Value = .TextBoxPressQTY.Text
                                                row.Cells("Column5").Value = Val(.TextBoxPressQTY.Text) * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            Else
                                                row.Cells("Column7").Value += 1
                                                row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    End If
                    .TextBoxQTY.Text = 0
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        Finally
            cmd.Dispose()
        End Try
    End Sub
End Module
