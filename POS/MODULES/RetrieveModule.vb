Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Text
Module RetrieveModule
    Dim user_id As String
    Dim acroo As String
    Dim fullname As String
    Dim Location_control As New Point(10, 10)
    Public Count_control As Integer = 0
    Dim result As Integer
    Dim dr
    Dim servingtotal
    Dim municipality
    Dim province
    Dim full_name
    Dim productcode As String
    Dim password As String = Login.txtpassword.Text
    Dim wrapper As New Simple3Des(password)
    Dim returnval
    Dim RowsReturned As Integer
    Dim product_line
    Dim available_stock
    Dim dailysales
    Dim critical_item
    Dim product
    Dim cipherText As String
    'FUNCTION FOR LOGGING-IN POS CLIENT/ CREW / POS ==================================================================================== 
    Public Sub retrieveLoginDetails()
        If Login.txtusername.Text = "" Then
            MessageBox.Show("Input username!", "Login Form", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Login.txtusername.Focus()
        ElseIf Login.txtpassword.Text = "" Then
            MessageBox.Show("Input password!", "Login Form", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Login.txtpassword.Focus()
        Else
            Try
                cipherText = ConvertPassword(SourceString:=Login.txtpassword.Text)
                sql = "SELECT * FROM loc_users WHERE username = @Username AND password = @Password AND guid = '" & ClientGuid & "' AND store_id = @StoreID AND active = 1;"
                cmd = New MySqlCommand(sql, LocalhostConn())
                With cmd
                    .Parameters.Clear()
                    .Parameters.AddWithValue("@Username", Login.txtusername.Text)
                    .Parameters.AddWithValue("@UserID", Login.txtusername.Text)
                    .Parameters.AddWithValue("@Password", cipherText)
                    .Parameters.AddWithValue("@StoreID", ClientStoreID)
                    Dim reader As MySqlDataReader
                    reader = .ExecuteReader()
                    While reader.Read()
                        user_id = reader("uniq_id")
                    End While
                    reader.Close()
                End With
                da = New MySqlDataAdapter
                dt = New DataTable
                da.SelectCommand = cmd
                da.Fill(dt)
            Catch ex As MySqlException
                MsgBox(ex.ToString)
            Finally
                da.Dispose()
                If dt.Rows.Count > 0 Then
                    Dim crew_id, username, password, fullname, userlevel, active, storeid, franguid, role As String
                    crew_id = dt.Rows(0).Item(0)
                    role = dt.Rows(0).Item(1)
                    username = dt.Rows(0).Item(3)
                    password = dt.Rows(0).Item(4)
                    userlevel = dt.Rows(0).Item(7)
                    fullname = dt.Rows(0).Item(2)
                    active = dt.Rows(0).Item(11)
                    franguid = dt.Rows(0).Item(12)
                    storeid = dt.Rows(0).Item(13)
                    ClientRole = role
                    If Login.txtusername.Text = username And cipherText = password And userlevel = "Crew" And ClientStoreID = storeid And active = 1 And franguid = ClientGuid Then
                        MessageBox.Show("Welcome " + fullname + "!", "Login Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Login.txtusername.Text = ""
                        Login.txtpassword.Text = ""
                        ClientCrewID = user_id
                        messageboxappearance = True
                        SystemLogType = "LOGIN"
                        SystemLogDesc = "User Login: " & username & " : " & ClientRole
                        GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
                        Shift = ""
                        Login.Close()
                        POS.Show()
                    ElseIf Login.txtusername.Text = username And cipherText = password And userlevel = "Head Crew" And ClientStoreID = storeid And active = 1 And franguid = ClientGuid Then
                        MessageBox.Show("Welcome " + fullname + "!", "Login Successfully(" & ClientRole & ")", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Login.txtusername.Text = ""
                        Login.txtpassword.Text = ""
                        ClientCrewID = user_id
                        messageboxappearance = True
                        SystemLogType = "LOGIN"
                        SystemLogDesc = "User Login: " & username & " : " & ClientRole
                        GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
                        Shift = ""
                        Login.Close()
                        POS.Show()
                    Else
                        MessageBox.Show("Incorrect username or password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        SystemLogType = "ERROR"
                        SystemLogDesc = "FAILED TO LOGIN: Username: " & Login.txtusername.Text & " Password: " & Login.txtpassword.Text
                        GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
                    End If
                Else
                    MessageBox.Show("Incorrect username or password!", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Login.txtusername.Focus()
                    messageboxappearance = True
                    SystemLogType = "ERROR"
                    SystemLogDesc = "FAILED TO LOGIN: Username and password input " & Login.txtusername.Text & " " & Login.txtpassword.Text
                    GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
                End If
            End Try
        End If
    End Sub
    'FUNCTION LOADING EXPENCES / POS ==================================================================================== 
    Public Sub listviewproductsshow(ByVal where As String)
        Try
            If where = "Others" Then
                cmd = New MySqlCommand("SELECT product_id, product_name, product_image, product_price, formula_id FROM loc_admin_products WHERE product_category ='" & where & "' AND product_status = 1 AND store_id = " & ClientStoreID, LocalhostConn())
            Else
                cmd = New MySqlCommand("SELECT product_id, product_name, product_image, product_price, formula_id FROM loc_admin_products WHERE product_category ='" & where & "' AND product_status = 1 ", LocalhostConn())
            End If
            With POS
                .PanelProducts.Controls.Clear()
                da = New MySqlDataAdapter(cmd)
                dt = New DataTable()
                da.Fill(dt)
                For Each row As DataRow In dt.Rows
                    Count_control += 1
                    Dim new_Button_product As New Button
                    Dim buttonname As String = row("product_name")
                    Dim newlabel As New Label
                    productprice = row("product_price")
                    productID = row("product_id")
                    With new_Button_product
                        .Name = buttonname
                        .Text = productprice
                        .TextImageRelation = TextImageRelation.ImageBeforeText
                        .TextAlign = ContentAlignment.TopLeft
                        If where = "Premium" Then
                            .ForeColor = Color.White
                        Else
                            .ForeColor = Color.Black
                        End If
                        .Font = New Font("Kelson Sans Normal", 10)
                        .BackgroundImage = Base64ToImage(row("product_image"))
                        .FlatStyle = FlatStyle.Flat
                        .FlatAppearance.BorderSize = 0
                        .BackgroundImageLayout = ImageLayout.Stretch
                        .Location = New Point(Location_control.X, Location_control.Y)
                        .Width = 148
                        .Height = 120
                        .Cursor = Cursors.Hand
                        With newlabel
                            .Text = buttonname
                            .Font = New Font("Kelson Sans Normal", 10)
                            .ForeColor = Color.White
                            .Width = 148
                            .Location = New Point(0, 100)
                            .BackColor = Color.SlateGray
                            .Parent = new_Button_product
                            .TextAlign = ContentAlignment.TopCenter
                        End With
                        .Controls.Add(newlabel)
                    End With
                    .PanelProducts.Controls.Add(new_Button_product)
                    AddHandler new_Button_product.Click, AddressOf new_product_button_click
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
        End Try
    End Sub
    'MANAGE PRODUCTS PANEL ========================================================
    Public Sub selectmax(ByVal whatform As Integer)
        If whatform = 1 Then
            POS.TextBoxMAXID.Text = Format(Now, "yyddMMHHmmssyy")
        ElseIf whatform = 2 Then
            Addexpense.TextBoxMAXID.Text = Format(Now, "yydd-MMHH-mmssyy")
        ElseIf whatform = 3 Then
            Registration.TextBoxMAXID.Text = Format(Now, "yydd-MMHH-mmssyy")
        End If
        cmd.Dispose()
    End Sub
    Dim formulaid
    Public Function selectmaxformula(ByVal whatid As String, ByVal fromtable As String, ByVal flds As String)
        Try
            sql = "Select " & flds & " FROM " & fromtable & " ORDER BY " & whatid & " DESC LIMIT 1"
            cmd = New MySqlCommand(sql, LocalhostConn())
            formulaid = cmd.ExecuteScalar()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return formulaid
        cmd.Dispose()
    End Function
    Public Function returnfullname(ByVal where As String)
        Try
            Dim cmd As MySqlCommand = New MySqlCommand("SELECT full_name FROM loc_users WHERE uniq_id = '" + where + "' ", LocalhostConn)
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            full_name = dt(0)(0)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            LocalhostConn.close()
            da.Dispose()
        End Try
        Return full_name
    End Function
    Dim valuetoreturn
    Dim MyLocalConnection As MySqlConnection
    Dim MyCloudConnection As MySqlConnection
    Dim MyCmd As MySqlCommand
    Public Function GLOBAL_RETURN_FUNCTION(tbl As String, flds As String, toreturn As String, thisislocalconn As Boolean)
        Try
            sql = "SELECT " & flds & " FROM " & tbl
            If thisislocalconn = True Then
                MyLocalConnection = New MySqlConnection
                MyLocalConnection.ConnectionString = LocalConnectionString
                MyLocalConnection.Open()
                MyCmd = New MySqlCommand(sql, MyLocalConnection)
            Else
                MyCloudConnection = New MySqlConnection
                MyCloudConnection.ConnectionString = CloudConnectionString
                MyCloudConnection.Open()
                MyCmd = New MySqlCommand(sql, MyCloudConnection)
            End If
            Using reader As MySqlDataReader = MyCmd.ExecuteReader()
                If reader.HasRows Then
                    While reader.Read
                        valuetoreturn = reader(toreturn)
                    End While
                End If
            End Using
            If thisislocalconn = True Then
                MyLocalConnection.Close()
            Else
                MyCloudConnection.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return valuetoreturn
    End Function
    Public Function returnuserid(ByVal full_name As String) As String
        Try
            sql = "Select uniq_id FROM loc_users WHERE full_name = '" & full_name & "'"
            cmd = New MySqlCommand
            With cmd
                .CommandText = sql
                .Connection = LocalhostConn()
                'This will loop through all returned records 
                Using readerObj As MySqlDataReader = cmd.ExecuteReader
                    While readerObj.Read
                        fullname = readerObj("uniq_id")
                        'handle returned value before next loop here
                    End While
                End Using
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return fullname
    End Function
    Public Function AsDatatable(table, fields, datagridd) As DataTable
        datagridd.rows.clear
        Dim dttable As DataTable = New DataTable
        Try
            Dim sql = "SELECT " & fields & " FROM " & table
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            da.Fill(dttable)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dttable
    End Function
    Public Sub retrieveformulaids()
        Try
            sql = "SELECT product_id, product_sku, formula_id, product_category, origin, server_inventory_id FROM `loc_admin_products` WHERE product_name = '" & POS.TextBoxNAME.Text & "'"
            cmd = New MySqlCommand
            With cmd
                .CommandText = sql
                .Connection = LocalhostConn()
                Using readerObj As MySqlDataReader = cmd.ExecuteReader
                    While readerObj.Read
                        Dim formula_id = readerObj("formula_id")
                        Dim product_id = readerObj("product_id").ToString
                        Dim product_sku = readerObj("product_sku").ToString
                        Dim product_category = readerObj("product_category").ToString
                        Dim origin = readerObj("origin").ToString
                        Dim inventoryid = readerObj("server_inventory_id").ToString
                        With POS
                            .TextBoxFormulaID.Text = formula_id
                            checkcriticallimit(formula_id:=formula_id, ID:=product_id, SKU:=product_sku, CAT:=product_category, ORIGIN:=origin, INVID:=inventoryid)
                        End With
                    End While
                End Using
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim DataTableCriticalLimit As New DataTable
    Dim DataAdapterCriticalLimit As MySqlDataAdapter
    Dim CmdCriticalLimit As MySqlCommand
    Dim ListOfIngredients As String
    Public Sub checkcriticallimit(ByVal formula_id, ByVal ID, ByVal SKU, ByVal CAT, ByVal ORIGIN, ByVal INVID)
        Try
            ListOfIngredients = ""
            sql = "SELECT product_ingredients, critical_limit, stock_primary, stock_secondary, stock_no_of_servings FROM `loc_pos_inventory` WHERE stock_primary <= critical_limit AND inventory_id IN (" & Trim(formula_id) & ");"
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
                    preventdgvordersdup(price:=POS.TextBoxPRICE.Text, name:=POS.TextBoxNAME.Text, ID:=ID, SKU:=SKU, CAT:=CAT, ORIGIN:=ORIGIN, INVID:=INVID)
                    retrieveanddeduct(formulaID:=POS.TextBoxFormulaID.Text, Cat:=CAT, Origin:=ORIGIN)
                End If
            Else
                preventdgvordersdup(price:=POS.TextBoxPRICE.Text, name:=POS.TextBoxNAME.Text, ID:=ID, SKU:=SKU, CAT:=CAT, ORIGIN:=ORIGIN, INVID:=INVID)
                retrieveanddeduct(formulaID:=POS.TextBoxFormulaID.Text, Cat:=CAT, Origin:=ORIGIN)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub retrieveanddeduct(ByVal formulaID, ByVal Cat, ByVal Origin)
        Try
            sql = "SELECT serving_value, formula_id, unit_cost FROM `loc_product_formula` WHERE formula_id IN (" & formulaID & ")"
            cmd = New MySqlCommand(sql, LocalhostConn())
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            With POS
                If Cat = "Add-Ons" Then
                    If .DataGridViewOrders.Rows.Count > 0 Then
                        If .DataGridViewOrders.SelectedRows(0).Cells(7).Value <> "Add-Ons" Then
                            Dim ID = .DataGridViewOrders.SelectedRows(0).Cells(5).Value
                            Dim test As Boolean = False
                            For Each row In .DataGridViewInv.Rows
                                If .TextBoxNAME.Text = row.Cells("Column10").Value Then
                                    If .DataGridViewOrders.SelectedRows(0).Cells(5).Value = row.cells("Column18").value Then
                                        test = True
                                        Exit For
                                    End If
                                End If
                            Next
                            If test = False Then
                                For Each row As DataRow In dt.Rows
                                    If deleteitem = False Then
                                        If .TextBoxQTY.Text <> 0 Then
                                            servingtotal = Val(row("serving_value")) * Val(.TextBoxQTY.Text)
                                            .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), ID)
                                        Else
                                            If .DataGridViewOrders.Rows.Count > 0 Then
                                                .DataGridViewInv.Rows.Add(row("serving_value"), row("formula_id"), 1, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), ID)
                                            End If
                                        End If
                                    End If
                                Next row
                            Else
                                For i As Integer = 0 To .DataGridViewInv.Rows.Count - 1 Step +1
                                    If .TextBoxNAME.Text = .DataGridViewInv.Rows(i).Cells(4).Value.ToString Then
                                        If .DataGridViewOrders.SelectedRows(0).Cells(5).Value = .DataGridViewInv.Rows(i).Cells(8).Value Then
                                            If enterpressorbuttonpress = True Then
                                                .DataGridViewInv.Rows(i).Cells(2).Value = .TextBoxPressQTY.Text
                                                .DataGridViewInv.Rows(i).Cells(0).Value = .DataGridViewInv.Rows(i).Cells(2).Value * .DataGridViewInv.Rows(i).Cells(5).Value
                                                .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                            Else
                                                .DataGridViewInv.Rows(i).Cells(2).Value = .DataGridViewInv.Rows(i).Cells(2).Value + 1
                                                .DataGridViewInv.Rows(i).Cells(0).Value = .DataGridViewInv.Rows(i).Cells(2).Value * .DataGridViewInv.Rows(i).Cells(5).Value
                                                .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End If
                ElseIf Cat = "Others" Then
                    If Origin = "Server" Then
                        If DISABLESERVEROTHERSPRODUCT = False Then
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
                                            .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0)
                                        Else
                                            servingtotal = Val(row("serving_value")) * Val(.TextBoxPressQTY.Text)
                                            .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxPressQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), 0)
                                        End If
                                    End If
                                Next row
                            Else
                                For i As Integer = 0 To .DataGridViewInv.Rows.Count - 1 Step +1
                                    If .TextBoxNAME.Text = .DataGridViewInv.Rows(i).Cells(4).Value.ToString() Then
                                        If enterpressorbuttonpress = True Then
                                            .DataGridViewInv.Rows(i).Cells(2).Value = .TextBoxPressQTY.Text
                                            .DataGridViewInv.Rows(i).Cells(0).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(5).Value)
                                            .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                        Else
                                            For a As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                                                .DataGridViewInv.Rows(i).Cells(2).Value = .TextBoxPressQTY.Text
                                                .DataGridViewInv.Rows(i).Cells(0).Value = Val(.TextBoxPressQTY.Text) * Val(.DataGridViewInv.Rows(i).Cells(5).Value)
                                                .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                            Next
                                        End If
                                    End If
                                Next
                            End If
                            .TextBoxQTY.Text = 0
                        End If
                    End If
                Else
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
                                    .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxQTY.Text), row("unit_cost"), 0)
                                Else
                                    servingtotal = Val(row("serving_value")) * Val(.TextBoxPressQTY.Text)
                                    .DataGridViewInv.Rows.Add(servingtotal, row("formula_id"), .TextBoxPressQTY.Text, .TextBoxINC.Text, .TextBoxNAME.Text, row("serving_value"), row("unit_cost") * Val(.TextBoxPressQTY.Text), row("unit_cost"), 0)
                                End If
                            End If
                        Next row
                    Else
                        For i As Integer = 0 To .DataGridViewInv.Rows.Count - 1 Step +1
                            If .TextBoxNAME.Text = .DataGridViewInv.Rows(i).Cells(4).Value.ToString() Then
                                If enterpressorbuttonpress = True Then
                                    .DataGridViewInv.Rows(i).Cells(2).Value = .TextBoxPressQTY.Text
                                    .DataGridViewInv.Rows(i).Cells(0).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(5).Value)
                                    .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                Else
                                    For a As Integer = 0 To .DataGridViewOrders.Rows.Count - 1 Step +1
                                        .DataGridViewInv.Rows(i).Cells(2).Value = .TextBoxPressQTY.Text
                                        .DataGridViewInv.Rows(i).Cells(0).Value = Val(.TextBoxPressQTY.Text) * Val(.DataGridViewInv.Rows(i).Cells(5).Value)
                                        .DataGridViewInv.Rows(i).Cells(6).Value = Val(.DataGridViewInv.Rows(i).Cells(2).Value) * Val(.DataGridViewInv.Rows(i).Cells(7).Value)
                                    Next
                                End If
                            End If
                        Next
                    End If
                    .TextBoxQTY.Text = 0
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
        End Try
    End Sub
    Public Sub GLOBAL_SELECT_ALL_FUNCTION_CLOUD(tbl As String, flds As String, datagrid As DataGridView)
        Try
            sql = "SELECT " & flds & " FROM " & table
            cmd = New MySqlCommand(sql, ServerCloudCon())
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            datagrid.DataSource = dt
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cloudconn.Close()
            da.Dispose()
        End Try
    End Sub
    Public Sub GLOBAL_SELECT_ALL_FUNCTION(ByVal table As String, ByVal fields As String, ByRef datagrid As DataGridView)
        Try
            sql = "SELECT " + fields + " FROM " + table
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            With datagrid
                .DataSource = Nothing
                .DataSource = dt
                .RowHeadersVisible = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .AllowUserToOrderColumns = False
                .AllowUserToResizeColumns = False
                .AllowUserToResizeRows = False
                .Font = New Font("Kelson Sans Normal", 10)
                .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
                .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            LocalhostConn.close
        End Try
    End Sub
    Public Sub GLOBAL_SELECT_ALL_FUNCTION_WHERE(ByVal table As String, ByVal fields As String, ByVal where As String, ByVal successmessage As String, ByVal errormessage As String, ByRef datagrid As DataGridView)
        Try
            sql = "SELECT " + fields + " FROM " + table + " WHERE " + where
            cmd = New MySqlCommand(sql, LocalhostConn)
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            With datagrid
                .DataSource = Nothing
                .DataSource = dt
                .RowHeadersVisible = False
                .AllowUserToAddRows = False
                .AllowUserToDeleteRows = False
                .AllowUserToOrderColumns = False
                .AllowUserToResizeColumns = False
                .AllowUserToResizeRows = False
                .Font = New Font("Kelson Sans Normal", 10)
                .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
                .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            LocalhostConn.close
        End Try
    End Sub
    Public Sub GLOBAL_SELECT_ALL_FUNCTION_COMBOBOX(table As String, fields As String, combobox As ComboBox, Loccon As Boolean)
        Try
            sql = "SELECT " + fields + " FROM " + table
            If Loccon = True Then
                cmd = New MySqlCommand(sql, LocalhostConn)
            Else
                cmd = New MySqlCommand(sql, ServerCloudCon)
            End If
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            With combobox
                .DataSource = Nothing
                .DataSource = dt
                .ValueMember = fields
                .DisplayMember = fields
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            da.Dispose()
        End Try
    End Sub
    Public Function GLOBAL_SELECT_FUNCTION_RETURN(ByVal table As String, ByVal fields As String, ByVal values As String, ByVal returnvalrow As String)
        Try
            sql = "SELECT " + fields + " FROM " + table + " WHERE " + values
            cmd = New MySqlCommand(sql, LocalhostConn)
            Using readerObj As MySqlDataReader = cmd.ExecuteReader
                While readerObj.Read
                    returnval = readerObj(returnvalrow).ToString
                End While
            End Using
            LocalhostConn.close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return returnval
    End Function
    '=================================================================MDI FORM
    Dim returncount As String
    Public Function count(ByVal tocount As String, ByVal table As String)
        Try
            sql = "SELECT COUNT(" & tocount & ") FROM " & table
            cmd = New MySqlCommand(sql, LocalhostConn)
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            For Each row As DataRow In dt.Rows
                returncount = row("COUNT(" & tocount & ")")
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return returncount
    End Function
    Dim returnsum
    Public Function roundsum(tototal As String, table As String, Columncall As String)
        Try
            sql = "SELECT SUM(ROUND(" & tototal & ",0)) AS " & Columncall & " FROM " & table
            cmd = New MySqlCommand(sql, LocalhostConn)
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            If IsDBNull(dt.Rows(0)(0)) Then
                returnsum = 0
            Else
                For Each row As DataRow In dt.Rows
                    returnsum = row(Columncall)
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return returnsum
    End Function
    Public Function sum(ByVal tototal As String, ByVal table As String)
        Try
            sql = "SELECT SUM(" & tototal & ") FROM " & table
            cmd = New MySqlCommand(sql, LocalhostConn)
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            If IsDBNull(dt.Rows(0)(0)) Then
                returnsum = 0
            Else
                For Each row As DataRow In dt.Rows
                    returnsum = row("SUM(" & tototal & ")")
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return returnsum
    End Function
    Dim RetunSel
    Public Function returnselect(toreturn As String, table As String)
        Try
            sql = "SELECT " & toreturn & " FROM " & table
            cmd = New MySqlCommand(sql, LocalhostConn)
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            For Each row As DataRow In dt.Rows
                RetunSel = row(toreturn)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return RetunSel
    End Function
End Module