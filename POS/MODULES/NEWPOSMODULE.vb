﻿Imports MySql.Data.MySqlClient
Module NEWPOSMODULE
    Dim DatagridviewRowIndex As Integer
    Dim servingtotal
    Public Sub new_product_button_click(ByVal sender As Object, ByVal e As EventArgs)
        DatagridviewRowIndex = getCurrentCellButton_Click(sender, e)
        Try
            Deleteitem = False
            Dim btn As Button = DirectCast(sender, Button)
            Dim ProductName = btn.Name
            Dim ProductPrice As String = btn.Text
            Dim Tax = 1 + Val(S_Tax)
            With POS
                'Procedure: 1 Product Qty
                If S_ZeroRated = "0" Then
                    'Price not / by 1.12
                    If .WaffleUpgrade Then
                        'Price plus waffle upgrade price
                        If Val(.TextBoxQTY.Text) > 0 Then
                            Dim TotalPrice As Integer = 0
                            TotalPrice = Val(.TextBoxQTY.Text) * Val(ProductPrice)
                            Dim TotalUpgrade As Integer = 0
                            TotalUpgrade = Val(.TextBoxQTY.Text) * Val(S_Upgrade_Price)
                            ProductTotalPrice = TwoDecimalPlaces(TotalPrice + TotalUpgrade)
                        Else
                            ProductTotalPrice = TwoDecimalPlaces(Val(ProductPrice) + Val(S_Upgrade_Price))
                        End If
                    Else
                        If Val(.TextBoxQTY.Text) > 0 Then
                            Dim TotalPrice As Integer = 0
                            TotalPrice = Val(.TextBoxQTY.Text) * Val(ProductPrice)
                            ProductTotalPrice = TwoDecimalPlaces(TotalPrice)
                        Else
                            ProductTotalPrice = TwoDecimalPlaces(Val(ProductPrice))
                        End If
                    End If
                Else
                    If .WaffleUpgrade Then
                        If Val(.TextBoxQTY.Text) > 0 Then
                            Dim TotalPrice As Integer = 0
                            TotalPrice = Val(.TextBoxQTY.Text) * Val(ProductPrice)
                            Dim TotalUgrade As Integer = 0
                            TotalUgrade = Val(.TextBoxQTY.Text) * Val(S_Upgrade_Price)
                            Dim WaffleAddPriceTotal = TotalPrice + TotalUgrade
                            ProductTotalPrice = TwoDecimalPlaces(WaffleAddPriceTotal / Tax)
                        Else
                            Dim WaffleAddPriceTotal = Val(ProductPrice) + Val(S_Upgrade_Price)
                            ProductTotalPrice = TwoDecimalPlaces(WaffleAddPriceTotal / Tax)
                        End If
                    Else
                        If Val(.TextBoxQTY.Text) > 0 Then
                            Dim TotalPrice As Integer = 0
                            TotalPrice = Val(.TextBoxQTY.Text) * Val(ProductPrice)
                            ProductTotalPrice = TwoDecimalPlaces(TotalPrice / Tax)
                        Else
                            ProductTotalPrice = TwoDecimalPlaces(Val(ProductPrice) / Tax)
                        End If
                    End If
                End If
                .TextBoxPRICE.Text = ProductPrice
                .TextBoxNAME.Text = ProductName
            End With
            GetFormulaid(ProductName, ProductPrice)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub GetFormulaid(ProductName As String, ProductOriginalPrice As Double)
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim sql = "SELECT product_id, product_sku, formula_id, product_category, origin, server_inventory_id, addontype FROM `loc_admin_products` WHERE product_name = '" & ProductName & "'"
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
                        If .WaffleUpgrade = True Then
                            'This if block will change Batter ID to Brownie ID
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
                        CheckCriticalLimit(ProductName, ProductOriginalPrice, formula_id, product_id, product_sku, product_category, origin, inventoryid, addontype)
                    End With
                End While
            End Using
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub CheckCriticalLimit(ByVal ProductName, ByVal ProductOriginalPrice, ByVal formula_id, ByVal ID, ByVal SKU, ByVal CAT, ByVal ORIGIN, ByVal INVID, ByVal addontype)
        Try
            Dim LocalConnection As MySqlConnection = LocalhostConn()
            Dim DataTableCriticalLimit As New DataTable
            Dim DataAdapterCriticalLimit As MySqlDataAdapter
            Dim CmdCriticalLimit As MySqlCommand
            Dim ListOfIngredients As String
            ListOfIngredients = ""
            sql = "SELECT product_ingredients, critical_limit, stock_primary, stock_secondary, stock_no_of_servings FROM `loc_pos_inventory` WHERE stock_primary <= critical_limit AND server_inventory_id IN (" & Trim(formula_id) & ");"
            CmdCriticalLimit = New MySqlCommand(sql, LocalConnection)
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
                    AddProductToDatagridviewOrders(ProductName, ProductOriginalPrice, ID, SKU, CAT, ORIGIN, INVID, addontype)
                    AddInventoryToDatagridviewInv(formula_id, CAT, ORIGIN, addontype, ProductName)
                End If
            Else
                AddProductToDatagridviewOrders(ProductName, ProductOriginalPrice, ID, SKU, CAT, ORIGIN, INVID, addontype)
                AddInventoryToDatagridviewInv(formula_id, CAT, ORIGIN, addontype, ProductName)
            End If
            POS.TextBoxQTY.Text = "0"
            Compute()
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub AddProductToDatagridviewOrders(Name, ProductOriginalPrice, ProductID, Code, Category, Origin, InventoryID, Addontype)
        Try
            With POS

                If Category = "Add-Ons" Then
                    Dim TotalQuantity As Integer = 0
                    Dim TotalAddOnPrice As Double = 0
                    Dim Tax = 1 + Val(S_Tax)
                    .ButtonClickCount += 1
                    .TextBoxINC.Text = POS.ButtonClickCount

                    If Val(.TextBoxQTY.Text) > 0 Then
                        TotalQuantity = .TextBoxQTY.Text
                        If S_ZeroRated = "0" Then
                            TotalAddOnPrice = TotalQuantity * ProductOriginalPrice
                        Else
                            Dim DivideByTax As Double = TotalQuantity * ProductOriginalPrice
                            TotalAddOnPrice = TwoDecimalPlaces(DivideByTax / Tax)
                        End If
                    Else
                        TotalQuantity += 1
                        If S_ZeroRated = "0" Then
                            TotalAddOnPrice = ProductOriginalPrice
                        Else
                            TotalAddOnPrice = TwoDecimalPlaces(ProductOriginalPrice / Tax)
                        End If
                    End If

                    Dim AddOnExist As Boolean = False
                    If Addontype = "Classic" Then
                        For Each row In .DataGridViewOrders.Rows
                            If .TextBoxNAME.Text = row.Cells("Column1").value Then
                                If row.Cells("Column17").value = .DataGridViewOrders.SelectedRows(0).Cells(5).Value Then
                                    AddOnExist = True
                                    Exit For
                                End If
                            End If
                        Next
                    Else
                        For Each row In .DataGridViewOrders.Rows
                            If .TextBoxNAME.Text = row.Cells("Column1").value Then
                                AddOnExist = True
                                Exit For
                            End If
                        Next
                    End If
                    If AddOnExist = False Then
                        If .DataGridViewOrders.Rows.Count > 0 Then
                            If Addontype = "Classic" Then
                                If .DataGridViewOrders.SelectedRows(0).Cells(7).Value = "Add-Ons" Then
                                    MsgBox("Select product")
                                ElseIf .DataGridViewOrders.SelectedRows(0).Cells(7).Value = "Others" Then
                                    MsgBox("Add-Ons are exclusive for fbw waffles only")
                                Else
                                    ThisIsMyInventoryID = .TextBoxINC.Text
                                    .DataGridViewOrders.Rows.Insert(DatagridviewRowIndex + 1, Name, TotalQuantity, ProductOriginalPrice, TotalAddOnPrice, .TextBoxINC.Text, ProductID, Code, Category, .DataGridViewOrders.SelectedRows(0).Cells(5).Value.ToString, "AOCLASSIC", InventoryID, 0, Origin, Addontype)
                                End If
                            Else
                                ThisIsMyInventoryID = .TextBoxINC.Text
                                .DataGridViewOrders.Rows.Add(Name, TotalQuantity, ProductOriginalPrice, TotalAddOnPrice, .TextBoxINC.Text, ProductID, Code, Category, ProductID, "AOPREMIUM", InventoryID, 0, Origin, Addontype)
                            End If
                        Else
                            If Addontype = "Classic" Then
                                MsgBox("Select product first")
                            Else
                                ThisIsMyInventoryID = .TextBoxINC.Text
                                .DataGridViewOrders.Rows.Add(Name, TotalQuantity, ProductOriginalPrice, TotalAddOnPrice, .TextBoxINC.Text, ProductID, Code, Category, ProductID, "AOPREMIUM", InventoryID, 0, Origin, Addontype)
                            End If
                        End If
                    Else
                        For i As Integer = 0 To POS.DataGridViewOrders.Rows.Count - 1 Step +1
                            If .TextBoxNAME.Text = .DataGridViewOrders.Rows(i).Cells(0).Value.ToString() Then
                                If Addontype = "Classic" Then
                                    If .DataGridViewOrders.SelectedRows(0).Cells(5).Value = .DataGridViewOrders.Rows(i).Cells(8).Value Then
                                        If Val(.TextBoxQTY.Text) > 0 Then
                                            If S_ZeroRated = "0" Then
                                                .DataGridViewOrders.Rows(i).Cells(1).Value = .TextBoxQTY.Text
                                                .DataGridViewOrders.Rows(i).Cells(3).Value = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            Else
                                                .DataGridViewOrders.Rows(i).Cells(1).Value = .TextBoxQTY.Text
                                                Dim TotalAddonPrice1 As Double = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                                .DataGridViewOrders.Rows(i).Cells(3).Value = TwoDecimalPlaces(TotalAddonPrice1 / Tax)
                                            End If
                                        Else
                                            If S_ZeroRated = "0" Then
                                                .DataGridViewOrders.Rows(i).Cells(1).Value += 1
                                                .DataGridViewOrders.Rows(i).Cells(3).Value = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            Else
                                                .DataGridViewOrders.Rows(i).Cells(1).Value += 1
                                                Dim TotalAddonPrice1 As Double = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                                .DataGridViewOrders.Rows(i).Cells(3).Value = TwoDecimalPlaces(TotalAddonPrice1 / Tax)
                                            End If
                                        End If
                                    End If
                                    ThisIsMyInventoryID = .DataGridViewOrders.Rows(i).Cells(4).Value
                                Else
                                    If Val(.TextBoxQTY.Text) > 0 Then
                                        If S_ZeroRated = "0" Then
                                            .DataGridViewOrders.Rows(i).Cells(1).Value = .TextBoxQTY.Text
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                        Else
                                            .DataGridViewOrders.Rows(i).Cells(1).Value = .TextBoxQTY.Text
                                            Dim TotalAddonPrice1 As Double = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = TwoDecimalPlaces(TotalAddonPrice1 / Tax)
                                        End If
                                    Else
                                        If S_ZeroRated = "0" Then
                                            .DataGridViewOrders.Rows(i).Cells(1).Value += 1
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                        Else
                                            .DataGridViewOrders.Rows(i).Cells(1).Value += 1
                                            Dim TotalAddonPrice1 As Double = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = TwoDecimalPlaces(TotalAddonPrice1 / Tax)
                                        End If
                                    End If
                                    ThisIsMyInventoryID = .DataGridViewOrders.Rows(i).Cells(4).Value
                                End If
                            End If
                        Next

                        .ButtonPayMent.Enabled = True
                        .Buttonholdoder.Enabled = True
                        .ButtonPendingOrders.Enabled = False
                    End If
                Else
                    Dim ProductExist As Boolean = False
                    For Each row In .DataGridViewOrders.Rows
                        If Name = row.Cells("Column1").Value Then
                            ProductExist = True
                            Exit For
                        End If
                    Next
                    If ProductExist = False Then
                        Dim TotalQuantity As Integer = 0
                        Dim TotalUpgrade As Integer = 0

                        .ButtonClickCount += 1
                        .TextBoxINC.Text = POS.ButtonClickCount

                        If Val(.TextBoxQTY.Text) > 0 Then
                            TotalQuantity = .TextBoxQTY.Text
                            If .WaffleUpgrade Then
                                TotalUpgrade = .TextBoxQTY.Text
                            Else
                                TotalUpgrade = 0
                            End If
                        Else
                            TotalQuantity = 1
                            If .WaffleUpgrade Then
                                TotalUpgrade = 1
                            Else
                                TotalUpgrade = 0
                            End If
                        End If

                        If Category = "Famous Blends" Then
                            DISABLESERVEROTHERSPRODUCT = True
                            .DataGridViewOrders.Rows.Add(Name, TotalQuantity, ProductOriginalPrice, ProductTotalPrice, .TextBoxINC.Text, ProductID, Code, Category, ProductID, "DRINKS", InventoryID, 0, Origin, Addontype)
                        ElseIf Category = "Others" Then
                            If Origin = "Server" Then
                                If DISABLESERVEROTHERSPRODUCT = False Then
                                    For Each row In .DataGridViewOrders.Rows
                                        If row.Cells("Column42").Value = "Local" Then
                                            HASOTHERSLOCALPRODUCT = True
                                            Exit For
                                        End If
                                    Next
                                    If HASOTHERSLOCALPRODUCT = False Then
                                        'Name, ProductID, Code, Category, Origin, InventoryID, Addontype
                                        .DataGridViewOrders.Rows.Add(Name, TotalQuantity, ProductOriginalPrice, ProductTotalPrice, .TextBoxINC.Text, ProductID, Code, Category, ProductID, "OTHERS", InventoryID, 0, Origin, Addontype)
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
                                    .DataGridViewOrders.Rows.Add(Name, TotalQuantity, ProductOriginalPrice, ProductTotalPrice, .TextBoxINC.Text, ProductID, Code, Category, ProductID, "OTHERS", InventoryID, 0, Origin, Addontype)
                                    .ButtonTransactionMode.Enabled = True
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
                            'If Product is waffle 
                            DISABLESERVEROTHERSPRODUCT = True
                            'Name, ProductID, Code, Category, Origin, InventoryID, Addontype
                            .DataGridViewOrders.Rows.Add(Name, TotalQuantity, ProductOriginalPrice, ProductTotalPrice, .TextBoxINC.Text, ProductID, Code, Category, ProductID, "WAFFLE", InventoryID, TotalUpgrade, Origin, Addontype)
                        End If
                        .ButtonPayMent.Enabled = True
                    Else
                        Dim TotalProductPrice As Double = 0
                        Dim Tax = 1 + Val(S_Tax)
                        For i As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                            If .DataGridViewOrders.Rows(i).Cells(0).Value.ToString() = Name Then
                                If S_ZeroRated = "0" Then
                                    If Val(.TextBoxQTY.Text) > 0 Then
                                        If .WaffleUpgrade Then
                                            If Category = "Others" Then
                                                .WaffleUpgrade = False
                                                .ButtonWaffleUpgrade.Text = "Waffle Upgrade"
                                            End If
                                            .DataGridViewOrders.Rows(i).Cells(11).Value = Val(.TextBoxQTY.Text)
                                            Dim TotalUpgrade As Double = .DataGridViewOrders.Rows(i).Cells(11).Value * Val(S_Upgrade_Price)
                                            .DataGridViewOrders.Rows(i).Cells(1).Value = Val(.TextBoxQTY.Text)
                                            Dim PriceXQuantity As Double = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            TotalProductPrice = PriceXQuantity + TotalUpgrade
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = TwoDecimalPlaces(TotalProductPrice)
                                        Else
                                            .DataGridViewOrders.Rows(i).Cells(1).Value = Val(.TextBoxQTY.Text)
                                            TotalProductPrice = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = TwoDecimalPlaces(TotalProductPrice)
                                        End If
                                    Else
                                        If .WaffleUpgrade Then
                                            If Category = "Others" Then
                                                .WaffleUpgrade = False
                                                .ButtonWaffleUpgrade.Text = "Waffle Upgrade"
                                            End If
                                            If .DataGridViewOrders.Rows(i).Cells(9).Value = "WAFFLE" Then
                                                .DataGridViewOrders.Rows(i).Cells(11).Value += 1
                                            End If
                                            Dim TotalUpgrade As Double = .DataGridViewOrders.Rows(i).Cells(11).Value * Val(S_Upgrade_Price)
                                            .DataGridViewOrders.Rows(i).Cells(1).Value += 1
                                            Dim PriceXQuantity As Double = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            TotalProductPrice = PriceXQuantity + TotalUpgrade
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = TwoDecimalPlaces(TotalProductPrice)
                                        Else
                                            Dim TotalUpgrade As Double = .DataGridViewOrders.Rows(i).Cells(11).Value * Val(S_Upgrade_Price)
                                            .DataGridViewOrders.Rows(i).Cells(1).Value += 1
                                            Dim PriceXQuantity As Double = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            TotalProductPrice = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            TotalProductPrice = PriceXQuantity + TotalUpgrade
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = TwoDecimalPlaces(TotalProductPrice)
                                        End If
                                    End If
                                Else
                                    If Val(.TextBoxQTY.Text) > 0 Then
                                        If .WaffleUpgrade Then
                                            If Category = "Others" Then
                                                .WaffleUpgrade = False
                                                .ButtonWaffleUpgrade.Text = "Waffle Upgrade"
                                            End If
                                            If .DataGridViewOrders.Rows(i).Cells(9).Value = "WAFFLE" Then
                                                .DataGridViewOrders.Rows(i).Cells(11).Value = Val(.TextBoxQTY.Text)
                                            End If
                                            Dim TotalUpgrade As Double = .DataGridViewOrders.Rows(i).Cells(11).Value * Val(S_Upgrade_Price)
                                            .DataGridViewOrders.Rows(i).Cells(1).Value = Val(.TextBoxQTY.Text)
                                            Dim PriceXQuantity As Double = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            TotalProductPrice = PriceXQuantity + TotalUpgrade
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = TwoDecimalPlaces(TotalProductPrice / Tax)
                                        Else
                                            .DataGridViewOrders.Rows(i).Cells(1).Value = Val(.TextBoxQTY.Text)
                                            TotalProductPrice = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = TwoDecimalPlaces(TotalProductPrice / Tax)
                                        End If
                                    Else
                                        If .WaffleUpgrade Then
                                            If Category = "Others" Then
                                                .WaffleUpgrade = False
                                                .ButtonWaffleUpgrade.Text = "Waffle Upgrade"
                                            End If
                                            If .DataGridViewOrders.Rows(i).Cells(9).Value = "WAFFLE" Then
                                                .DataGridViewOrders.Rows(i).Cells(11).Value += 1
                                            End If
                                            Dim TotalUpgrade As Double = .DataGridViewOrders.Rows(i).Cells(11).Value * Val(S_Upgrade_Price)
                                            .DataGridViewOrders.Rows(i).Cells(1).Value += 1
                                            Dim PriceXQuantity As Double = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            TotalProductPrice = PriceXQuantity + TotalUpgrade
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = TwoDecimalPlaces(TotalProductPrice / Tax)
                                        Else
                                            Dim TotalUpgrade As Double = .DataGridViewOrders.Rows(i).Cells(11).Value * Val(S_Upgrade_Price)
                                            .DataGridViewOrders.Rows(i).Cells(1).Value += 1
                                            Dim PriceXQuantity As Double = .DataGridViewOrders.Rows(i).Cells(1).Value * .DataGridViewOrders.Rows(i).Cells(2).Value
                                            TotalProductPrice = PriceXQuantity + TotalUpgrade
                                            .DataGridViewOrders.Rows(i).Cells(3).Value = TwoDecimalPlaces(TotalProductPrice / Tax)
                                        End If
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
    Public Sub AddInventoryToDatagridviewInv(ByVal formulaID, ByVal Cat, ByVal Origin, ByVal addontype, ByVal ProductName)
        Try
            Dim DtInventory As DataTable = New DataTable
            If Origin = "Server" Then
                DtInventory = New DataTable
                DtInventory.Columns.Add("serving_value")
                DtInventory.Columns.Add("server_formula_id")
                DtInventory.Columns.Add("unit_cost")

                Dim splitformulaID As String = formulaID
                Dim words As String() = splitformulaID.Split(New Char() {","c})
                Dim word As String
                For Each word In words
                    Dim sql = "SELECT serving_value, server_formula_id, unit_cost FROM `loc_product_formula` WHERE server_formula_id = " & word
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                    Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                    Dim dtfill As DataTable = New DataTable
                    da.Fill(dtfill)
                    Dim FRID As DataRow = DtInventory.NewRow
                    FRID("serving_value") = dtfill(0)(0)
                    FRID("server_formula_id") = dtfill(0)(1)
                    FRID("unit_cost") = dtfill(0)(2)
                    DtInventory.Rows.Add(FRID)
                    LocalhostConn.Close()
                Next
            Else
                DtInventory = New DataTable
                DtInventory.Columns.Add("serving_value")
                DtInventory.Columns.Add("formula_id")
                DtInventory.Columns.Add("unit_cost")

                Dim splitformulaID As String = formulaID
                Dim words As String() = splitformulaID.Split(New Char() {","c})
                Dim word As String
                For Each word In words
                    Dim sql = "SELECT serving_value, formula_id, unit_cost FROM `loc_product_formula` WHERE formula_id = " & word
                    Console.Write(sql & "; ")
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                    Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                    Dim dtfill As DataTable = New DataTable
                    da.Fill(dtfill)
                    Dim FRID As DataRow = DtInventory.NewRow
                    FRID("serving_value") = dtfill(0)(0)
                    FRID("formula_id") = dtfill(0)(1)
                    FRID("unit_cost") = dtfill(0)(2)
                    DtInventory.Rows.Add(FRID)
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
                                If .DataGridViewOrders.SelectedRows(0).Cells(9).Value.ToString = "WAFFLE" Then
                                    For Each row As DataRow In DtInventory.Rows
                                        If Deleteitem = False Then
                                            If Val(.TextBoxQTY.Text) <> 0 Then
                                                servingtotal = Val(row("serving_value")) * Val(.TextBoxQTY.Text)
                                                If Origin = "Server" Then
                                                    .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), ID, Origin)
                                                Else
                                                    .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), ID, Origin)
                                                End If
                                            Else
                                                If Origin = "Server" Then
                                                    .DataGridViewInv.Rows.Add(row("serving_value"), row("server_formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost"), row("unit_cost"), ID, Origin)
                                                Else
                                                    .DataGridViewInv.Rows.Add(row("serving_value"), row("formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost"), row("unit_cost"), ID, Origin)
                                                End If
                                            End If
                                        End If
                                    Next row
                                End If
                            End If
                        Else
                            'Premium
                            For Each row As DataRow In DtInventory.Rows
                                If Deleteitem = False Then
                                    If .TextBoxQTY.Text > 0 Then
                                        servingtotal = Val(row("serving_value")) * Val(.TextBoxQTY.Text)
                                        If Origin = "Server" Then
                                            .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                        Else
                                            .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                        End If
                                    Else
                                        If .DataGridViewOrders.Rows.Count > 0 Then
                                            If Origin = "Server" Then
                                                .DataGridViewInv.Rows.Add(row("serving_value"), row("server_formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost"), row("unit_cost"), 0, Origin)
                                            Else
                                                .DataGridViewInv.Rows.Add(row("serving_value"), row("formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost"), row("unit_cost"), 0, Origin)
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
                                Dim ProductOthersLocal As Boolean = False
                                For Each row In .DataGridViewInv.Rows
                                    If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                                        ProductOthersLocal = True
                                        Exit For
                                    End If
                                Next
                                If ProductOthersLocal = False Then
                                    For Each row As DataRow In DtInventory.Rows
                                        If Val(.TextBoxQTY.Text) > 0 Then
                                            servingtotal = Val(row("serving_value")) * Val(.TextBoxQTY.Text)
                                            If Origin = "Server" Then
                                                .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                            Else
                                                .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                            End If
                                        Else
                                            servingtotal = Val(row("serving_value"))
                                            If Origin = "Server" Then
                                                .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), 0, Origin)
                                            Else
                                                .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), 0, Origin)
                                            End If
                                        End If
                                    Next row
                                Else
                                    Dim TotalQty As Integer = 0
                                    For i As Integer = 0 To .DataGridViewInv.Rows.Count - 1 Step +1
                                        If .TextBoxNAME.Text = .DataGridViewInv.Rows(i).Cells(4).Value.ToString() Then
                                            For a As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                                                If Val(.TextBoxQTY.Text) > 0 Then
                                                    TotalQty = .TextBoxQTY.Text
                                                    .DataGridViewInv.Rows(i).Cells(2).Value = TotalQty
                                                    .DataGridViewInv.Rows(i).Cells(0).Value = TotalQty * Val(.DataGridViewInv.Rows(i).Cells(5).Value)
                                                    .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                                Else
                                                    TotalQty = .DataGridViewInv.Rows(i).Cells(2).Value + 1
                                                    .DataGridViewInv.Rows(i).Cells(2).Value = TotalQty
                                                    .DataGridViewInv.Rows(i).Cells(0).Value = TotalQty * Val(.DataGridViewInv.Rows(i).Cells(5).Value)
                                                    .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                                End If
                                            Next
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Else
                        If HASOTHERSSERVERPRODUCT = False Then
                            Dim OthersLocalProduct As Boolean = False
                            For Each row In .DataGridViewInv.Rows
                                If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                                    OthersLocalProduct = True
                                    Exit For
                                End If
                            Next
                            If OthersLocalProduct = False Then
                                For Each row As DataRow In DtInventory.Rows
                                    If .TextBoxQTY.Text > 0 Then
                                        servingtotal = Val(row("serving_value")) * Val(.TextBoxQTY.Text)
                                        If Origin = "Server" Then
                                            .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                        Else
                                            .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                        End If
                                    Else
                                        servingtotal = Val(row("serving_value"))
                                        If Origin = "Server" Then
                                            .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost"), row("unit_cost"), 0, Origin)
                                        Else
                                            .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost"), row("unit_cost"), 0, Origin)
                                        End If
                                    End If
                                Next row
                            Else
                                Dim TotalQty As Integer = 0
                                For i As Integer = 0 To .DataGridViewInv.Rows.Count - 1 Step +1
                                    If .TextBoxNAME.Text = .DataGridViewInv.Rows(i).Cells(4).Value.ToString() Then
                                        If Val(.TextBoxQTY.Text) > 0 Then
                                            TotalQty = .TextBoxQTY.Text
                                            .DataGridViewInv.Rows(i).Cells(2).Value = TotalQty
                                            .DataGridViewInv.Rows(i).Cells(0).Value = TotalQty * Val(.DataGridViewInv.Rows(i).Cells(5).Value)
                                            .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                        Else
                                            TotalQty = .DataGridViewInv.Rows(i).Cells(2).Value + 1
                                            .DataGridViewInv.Rows(i).Cells(2).Value = TotalQty
                                            .DataGridViewInv.Rows(i).Cells(0).Value = TotalQty * Val(.DataGridViewInv.Rows(i).Cells(5).Value)
                                            .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End If
                Else
                    ' Waffle
                    Dim ProductInventoryExist As Boolean = False
                    For Each row In .DataGridViewInv.Rows
                        If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                            ProductInventoryExist = True
                            Exit For
                        End If
                    Next
                    If ProductInventoryExist = False Then
                        For Each row As DataRow In DtInventory.Rows
                            If Val(.TextBoxQTY.Text) > 0 Then
                                servingtotal = Val(row("serving_value")) * Val(.TextBoxQTY.Text)
                                If Origin = "Server" Then
                                    .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                Else
                                    .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                End If
                            Else
                                servingtotal = Val(row("serving_value"))
                                If Origin = "Server" Then
                                    .DataGridViewInv.Rows.Add(servingtotal, row("server_formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                Else
                                    .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0, Origin)
                                End If
                            End If
                        Next row
                    Else
                        'UPGRADE FUNCTION
                        If .WaffleUpgrade Then
                            Dim BrownieExist As Boolean = False
                            For Each row In .DataGridViewInv.Rows
                                If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                                    If S_Brownie_Mix = row.cells("Column6").value Then
                                        BrownieExist = True
                                        Exit For
                                    End If
                                End If
                            Next
                              Dim TotalQty As Integer = 0
                            If BrownieExist = False Then
                                Dim query
                                If Origin = "Server" Then
                                    query = "SELECT serving_value, server_formula_id, unit_cost FROM `loc_product_formula` WHERE server_formula_id = " & S_Brownie_Mix
                                Else
                                    query = "SELECT serving_value, formula_id, unit_cost FROM `loc_product_formula` WHERE formula_id = " & S_Brownie_Mix
                                End If

                                Dim cmd As MySqlCommand = New MySqlCommand(query, LocalhostConn)
                                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                                Dim dtt As DataTable = New DataTable
                                da.Fill(dtt)

                                If Val(.TextBoxQTY.Text) > 0 Then
                                    .DataGridViewInv.Rows.Add(dtt(0)(0), dtt(0)(1), Val(.TextBoxQTY.Text), .TextBoxINC.Text, .TextBoxNAME.Text, dtt(0)(0), dtt(0)(2) * Val(.TextBoxQTY.Text), dtt(0)(2), 0, Origin)
                                    'For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                    '    TotalQty = .TextBoxQTY.Text
                                    '    If row.Cells("Column6").Value <> S_Batter Then
                                    '        If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                    '            row.Cells("Column7").Value = TotalQty
                                    '            row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                    '            row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                    '        End If
                                    '    End If
                                    'Next
                                Else
                                    .DataGridViewInv.Rows.Add(dtt(0)(0), dtt(0)(1), 1, .TextBoxINC.Text, .TextBoxNAME.Text, dtt(0)(0), dtt(0)(2) * 1, dtt(0)(2), 0, Origin)
                                    'For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                    '    If row.Cells("Column6").Value <> S_Batter Then
                                    '        If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                    '            If row.Cells("Column10").Value = ProductName Then
                                    '                TotalQty = row.Cells("Column7").Value + 1
                                    '                row.Cells("Column7").Value = TotalQty
                                    '                row.Cells("Column5").Value = TotalQty * row.Cells("Column11").Value
                                    '                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                    '            End If
                                    '        End If
                                    '    End If
                                    'Next row
                                End If
                            Else
                                If Val(.TextBoxQTY.Text) > 0 Then
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Batter Then
                                            If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                                If row.Cells("Column10").Value = ProductName Then
                                                    TotalQty = .TextBoxQTY.Text
                                                    row.Cells("Column7").Value = TotalQty
                                                    row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                    row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                                End If
                                            Else
                                                If row.Cells("Column10").Value = ProductName Then
                                                    row.Cells("Column7").Value += Val(.TextBoxQTY.Text)
                                                    row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                    row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                                End If
                                            End If
                                        End If
                                    Next
                                Else
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Batter Then
                                            If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                                If row.Cells("Column10").Value = ProductName Then
                                                    TotalQty = row.Cells("Column7").Value + 1
                                                    row.Cells("Column7").Value = TotalQty
                                                    row.Cells("Column5").Value = TotalQty * row.Cells("Column11").Value
                                                    row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                                End If
                                            Else
                                                If row.Cells("Column10").Value = ProductName Then
                                                    TotalQty = row.Cells("Column7").Value + 1
                                                    row.Cells("Column7").Value = TotalQty
                                                    row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                    row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                                End If
                                            End If
                                        End If
                                    Next

                                End If
                            End If
                        Else
                            'FAMOUS BATTER
                            Dim BatterExist As Boolean = False
                            For Each row In .DataGridViewInv.Rows
                                If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                                    If S_Batter = row.cells("Column6").value Then
                                        BatterExist = True
                                        Exit For
                                    End If
                                End If
                            Next
                            Dim TotalQty As Integer = 0
                            If BatterExist = False Then
                                Dim query = "SELECT serving_value, server_formula_id, unit_cost FROM `loc_product_formula` WHERE server_formula_id = " & S_Batter
                                Dim cmd As MySqlCommand = New MySqlCommand(query, LocalhostConn)
                                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                                Dim dt1 As DataTable = New DataTable
                                da.Fill(dt1)
                                If Val(.TextBoxQTY.Text) > 0 Then
                                    TotalQty = .TextBoxQTY.Text
                                    .DataGridViewInv.Rows.Add(dt1(0)(0), dt1(0)(1), Val(.TextBoxQTY.Text), .TextBoxINC.Text, .TextBoxNAME.Text, dt1(0)(0), dt1(0)(2) * Val(.TextBoxQTY.Text), dt1(0)(2), 0, Origin)
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                            If row.Cells("Column6").Value <> S_Batter Then
                                                row.Cells("Column7").Value = TotalQty
                                                row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            End If
                                        End If
                                    Next
                                Else
                                    .DataGridViewInv.Rows.Add(dt1(0)(0), dt1(0)(1), 1, .TextBoxINC.Text, .TextBoxNAME.Text, dt1(0)(0), dt1(0)(2) * 1, dt1(0)(2), 0, Origin)
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                            TotalQty = row.Cells("Column7").Value + 1
                                            If row.Cells("Column6").Value <> S_Batter Then
                                                row.Cells("Column7").Value = TotalQty
                                                row.Cells("Column5").Value = TotalQty * row.Cells("Column11").Value
                                                row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                            End If
                                        End If
                                    Next
                                End If
                            Else
                                If Val(.TextBoxQTY.Text) > 0 Then
                                    TotalQty = .TextBoxQTY.Text
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                            If row.Cells("Column6").Value <> S_Batter Then
                                                If row.Cells("Column10").Value = ProductName Then
                                                    row.Cells("Column7").Value = TotalQty
                                                    row.Cells("Column5").Value = TotalQty * row.Cells("Column11").Value
                                                    row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                                End If
                                            Else
                                                If row.Cells("Column10").Value = ProductName Then
                                                    row.Cells("Column7").Value = Val(.TextBoxQTY.Text)
                                                    row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                    row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                                End If
                                            End If
                                        End If
                                    Next
                                Else
                                    For Each row As DataGridViewRow In .DataGridViewInv.Rows
                                        If row.Cells("Column6").Value <> S_Brownie_Mix Then
                                            TotalQty = row.Cells("Column7").Value + 1
                                            If row.Cells("Column6").Value <> S_Batter Then
                                                If row.Cells("Column10").Value = ProductName Then
                                                    row.Cells("Column7").Value = TotalQty
                                                    row.Cells("Column5").Value = TotalQty * row.Cells("Column11").Value
                                                    row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                                End If
                                            Else
                                                If row.Cells("Column10").Value = ProductName Then
                                                    row.Cells("Column7").Value = TotalQty
                                                    row.Cells("Column5").Value = row.Cells("Column7").Value * row.Cells("Column11").Value
                                                    row.Cells("Column14").Value = row.Cells("Column7").Value * row.Cells("Column15").Value
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    End If
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