﻿Imports MySql.Data.MySqlClient
Imports System.Drawing.Imaging
Public Class ManageProducts
    Private ImagePath As String = ""
    Dim productid
    Dim customproductname
    Dim result As Integer
    Private Sub Reports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            TabControl1.TabPages(0).Text = "Product List"
            TabControl1.TabPages(1).Text = "Custom Product List(Approved)"
            TabControl1.TabPages(2).Text = "Custom Product List(Unapproved)"
            TabControl1.TabPages(3).Text = "Price Change"
            serverloadproducts()
            clientloadproducts()
            clientloadproductsunapproved()
            LoadPriceChange()
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        GroupBox2.Text = "Add Product"
        ButtonCustomProducts.Text = "Submit"
        PanelAddCustomProducts.Top = (Me.Height - PanelAddCustomProducts.Height) / 4
        PanelAddCustomProducts.Left = (Me.Width - PanelAddCustomProducts.Width) / 3
        PanelAddCustomProducts.Visible = True
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            If DataGridViewClientProducts.SelectedRows.Count = 1 Then
                If DataGridViewClientProducts.SelectedRows(0).Cells(7).Value.ToString = "Server" Then
                    MsgBox("Products from server is not editable")
                Else
                    DataGridViewClientProducts.Enabled = False
                    GroupBox2.Text = "Edit Product"
                    ButtonCustomProducts.Text = "Update"
                    ButtonCustomProducts.BackColor = Color.FromArgb(221, 114, 46)
                    PanelAddCustomProducts.Top = (Me.Height - PanelAddCustomProducts.Height) / 4
                    PanelAddCustomProducts.Left = (Me.Width - PanelAddCustomProducts.Width) / 3
                    PanelAddCustomProducts.Visible = True
                    fillcustomproducts(True)
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub OfdImage_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        ImagePath = OpenFileDialog1.FileName
    End Sub
    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        PanelAddCustomProducts.Visible = False
        ClearTextBox(PanelAddCustomProducts)
        PictureBoxADDIMAGE.Image = Nothing
        DataGridViewClientProducts.Enabled = True
    End Sub
    Private Sub BtnConvertToBase64_Click(sender As Object, e As EventArgs) Handles BtnConvertToBase64.Click
        chooseimage(True)
    End Sub
    Sub chooseimage(ByVal trueorfalse As Boolean)
        If trueorfalse = True Then
            Try
                With OpenFileDialog1
                    .Filter = ("Images | *.png; *.bmp; *.jpg; *.jpeg; *.gif; *.ico;")
                    .FilterIndex = 4
                End With
                OpenFileDialog1.FileName = ""
                If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
                    If My.Computer.FileSystem.FileExists(ImagePath) Then
                        Dim ImageToConvert As Bitmap = Bitmap.FromFile(ImagePath)
                        ImageToConvert.MakeTransparent()
                        TxtBase64.Text = ImageToBase64(ImageToConvert, ImageFormat.Png)
                    End If
                    PictureBoxADDIMAGE.Image = Image.FromFile(OpenFileDialog1.FileName)
                    PictureBoxADDIMAGE.SizeMode = PictureBoxSizeMode.StretchImage
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
                SendErrorReport(ex.ToString)
            End Try
        Else
            Try
                With OpenFileDialog1
                    .Filter = ("Images | *.png; *.bmp; *.jpg; *.jpeg; *.gif; *.ico;")
                    .FilterIndex = 4
                End With
                OpenFileDialog1.FileName = ""
                If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
                    If My.Computer.FileSystem.FileExists(ImagePath) Then
                        Dim ImageToConvert As Bitmap = Bitmap.FromFile(ImagePath)
                        ImageToConvert.MakeTransparent()
                        RichTextBox1.Text = ImageToBase64(ImageToConvert, ImageFormat.Png)
                    End If
                    PictureBox2.Image = Image.FromFile(OpenFileDialog1.FileName)
                    PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
                SendErrorReport(ex.ToString)
            End Try
        End If
    End Sub
    Public Sub fillcustomproducts(ByVal iftrueorfalse As Boolean)
        If iftrueorfalse = True Then
            If DataGridViewClientProducts.Rows.Count > 0 Then
                productid = DataGridViewClientProducts.SelectedRows(0).Cells(0).Value.ToString()
                Try

                    sql = "SELECT * FROM loc_admin_products WHERE product_id =" & productid
                    cmd = New MySqlCommand(sql, LocalhostConn())
                    da = New MySqlDataAdapter(cmd)
                    dt = New DataTable
                    da.Fill(dt)
                    For Each row As DataRow In dt.Rows
                        TextBoxADDPRCODE.Text = row("product_sku")
                        TextBoxADDBCODE.Text = row("product_barcode")
                        TextBoxADDNAME.Text = row("product_name")
                        TextBoxADDDESC.Text = row("product_desc")
                        TextBoxADDPRICE.Text = row("product_price")
                        TxtBase64.Text = row("product_image")
                        PictureBoxADDIMAGE.Image = Base64ToImage(row("product_image"))
                        PictureBoxADDIMAGE.SizeMode = PictureBoxSizeMode.StretchImage
                    Next
                Catch ex As Exception
                    MsgBox(ex.ToString)
                    SendErrorReport(ex.ToString)
                End Try
            End If
        Else
            If DataGridViewUNAPPROVED.Rows.Count > 0 Then
                productid = DataGridViewUNAPPROVED.SelectedRows(0).Cells(0).Value.ToString()
                Try
                    sql = "SELECT * FROM loc_admin_products WHERE product_id =" & productid
                    cmd = New MySqlCommand(sql, LocalhostConn)
                    da = New MySqlDataAdapter(cmd)
                    dt = New DataTable
                    da.Fill(dt)
                    For Each row As DataRow In dt.Rows
                        TextBoxUPDATEPRCODE.Text = row("product_sku")
                        TextBoxUPDATEBCODE.Text = row("product_barcode")
                        TextBoxUPDATENAME.Text = row("product_name")
                        TextBoxUPDATEDESC.Text = row("product_desc")
                        TextBoxUPDATEPRICE.Text = row("product_price")
                        RichTextBox1.Text = row("product_image")
                        PictureBox2.Image = Base64ToImage(row("product_image"))
                        PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage
                    Next
                Catch ex As Exception
                    MsgBox(ex.ToString)
                    SendErrorReport(ex.ToString)
                End Try
            End If
        End If

    End Sub
    Public Sub serverloadproducts()
        Try
            table = "loc_admin_products"
            fields = "product_id, product_sku, product_name  , product_barcode, product_category, product_price, product_status, origin, server_product_id"
            where = "product_category <> 'Others' AND product_status = 1"
            GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewServerProducts, errormessage:="", successmessage:="", fields:=fields, where:=where)
            For Each row In dt.Rows
                If row("product_status") = "1" Then
                    row("product_status") = "Active"
                Else
                    row("product_status") = "Inactive"
                End If
            Next
            With DataGridViewServerProducts
                .ClearSelection()
                .Columns(0).Visible = False
                .Columns(1).HeaderCell.Value = "Code"
                .Columns(2).HeaderCell.Value = "Name"
                .Columns(3).HeaderCell.Value = "Barcode"
                .Columns(4).HeaderCell.Value = "Category"
                .Columns(5).HeaderCell.Value = "Price"
                .Columns(6).HeaderCell.Value = "State"
                .Columns(7).HeaderCell.Value = "Origin"
                .Columns(2).Width = 200
                .Columns(4).Width = 150
                .Columns.Item(5).DefaultCellStyle.Format = "n2"
                .Columns(8).Visible = False
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub clientloadproductsunapproved()
        Try
            table = "loc_admin_products"
            fields = "product_id, product_sku, product_name  , product_barcode, product_category, product_price, product_status, origin, formula_id"
            where = "product_category = 'Others' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'  AND product_status = 0"
            GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewUNAPPROVED, errormessage:="", successmessage:="", fields:=fields, where:=where)
            For Each row In dt.Rows
                If row("product_status") = 1 Then
                    row("product_status") = "Active"
                Else
                    row("product_status") = "Inactive"
                End If
            Next
            With DataGridViewUNAPPROVED
                .Columns(0).Visible = False
                .Columns(1).HeaderCell.Value = "Code"
                .Columns(2).HeaderCell.Value = "Name"
                .Columns(3).HeaderCell.Value = "Barcode"
                .Columns(4).HeaderCell.Value = "Category"
                .Columns(5).HeaderCell.Value = "Price"
                .Columns(6).HeaderCell.Value = "State"
                .Columns(7).HeaderCell.Value = "Origin"
                .Columns(8).Visible = False
                .Columns.Item(5).DefaultCellStyle.Format = "n2"
                .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub LoadPriceChange()
        Try
            Dim PriceChange = AsDatatable("loc_price_request_change", "*", DataGridViewPriceChange)
            For Each row As DataRow In PriceChange.Rows
                Dim sql = "SELECT product_name FROM loc_admin_products WHERE server_product_id = " & row("server_product_id")
                Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                Dim Productname = dt(0)(0)
                Dim Active
                If row("active") = 1 Then
                    Active = "Waiting For Approval"
                ElseIf row("active") = 2 Then
                    Active = "Approved"
                Else
                    Active = "Disapproved"
                End If
                DataGridViewPriceChange.Rows.Add(Productname, row("request_price"), row("created_at"), Active)
            Next
            With DataGridViewPriceChange
                .Columns(0).HeaderText = "Product Name"
                .Columns(1).HeaderText = "Price Requested"
                .Columns(2).HeaderText = "Date Created"
                .Columns(3).HeaderText = "Status"

            End With
            ' DataGridViewPriceChange
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub clientloadproducts()
        Try
            table = "loc_admin_products"
            fields = "product_id, product_sku, product_name  , product_barcode, product_category, product_price, product_status, origin, formula_id"
            where = "product_category = 'Others' AND store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'  AND product_status = 1"
            GLOBAL_SELECT_ALL_FUNCTION_WHERE(table:=table, datagrid:=DataGridViewClientProducts, errormessage:="", successmessage:="", fields:=fields, where:=where)
            For Each row In dt.Rows
                If row("product_status") = 1 Then
                    row("product_status") = "Active"
                Else
                    row("product_status") = "Inactive"
                End If
            Next
            With DataGridViewClientProducts

                .Columns(0).Visible = False
                .Columns(1).HeaderCell.Value = "Code"
                .Columns(2).HeaderCell.Value = "Name"
                .Columns(3).HeaderCell.Value = "Barcode"
                .Columns(4).HeaderCell.Value = "Category"
                .Columns(5).HeaderCell.Value = "Price"
                .Columns(6).HeaderCell.Value = "State"
                .Columns(7).HeaderCell.Value = "Origin"
                .Columns(8).Visible = False
                .Columns.Item(5).DefaultCellStyle.Format = "n2"
                .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub addcustomproducts()
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()
            Dim FormulaID = selectmaxformula("inventory_id", "loc_pos_inventory", "inventory_id")
            Dim Query = "INSERT INTO loc_product_formula (`product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `store_id`, `guid`, `crew_id`, `origin`, `server_formula_id`, `server_date_modified`) VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17)"
            Dim Cmd As MySqlCommand = New MySqlCommand(Query, ConnectionLocal)
            Cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = Trim(TextBoxADDNAME.Text)
            Cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = "piece(s)"
            Cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = 1
            Cmd.Parameters.Add("@4", MySqlDbType.VarChar).Value = "piece(s)"
            Cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = 1
            Cmd.Parameters.Add("@6", MySqlDbType.VarChar).Value = "piece(s)"
            Cmd.Parameters.Add("@7", MySqlDbType.VarChar).Value = 1
            Cmd.Parameters.Add("@8", MySqlDbType.VarChar).Value = 1
            Cmd.Parameters.Add("@9", MySqlDbType.Int64).Value = 0
            Cmd.Parameters.Add("@10", MySqlDbType.Text).Value = FullDate24HR()
            Cmd.Parameters.Add("@11", MySqlDbType.Decimal).Value = 0
            Cmd.Parameters.Add("@12", MySqlDbType.VarChar).Value = ClientStoreID
            Cmd.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientGuid
            Cmd.Parameters.Add("@14", MySqlDbType.VarChar).Value = ClientCrewID
            Cmd.Parameters.Add("@15", MySqlDbType.VarChar).Value = "Local"
            Cmd.Parameters.Add("@16", MySqlDbType.Int64).Value = 0
            Cmd.Parameters.Add("@17", MySqlDbType.Text).Value = "N/A"
            Cmd.ExecuteNonQuery()

            Query = "INSERT INTO loc_pos_inventory (`store_id`, `formula_id`, `product_ingredients`, `sku`, `stock_primary`, `stock_secondary`, `stock_no_of_servings`, `stock_status`, `critical_limit`, `guid`, `date_modified`, `crew_id`, `synced`, `server_date_modified`, `server_inventory_id`, `main_inventory_id`, `origin`) VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17)"
            Cmd = New MySqlCommand(Query, ConnectionLocal)
            Cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = ClientStoreID
            Cmd.Parameters.Add("@2", MySqlDbType.Int64).Value = FormulaID + 1
            Cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = Trim(TextBoxADDNAME.Text)
            Cmd.Parameters.Add("@4", MySqlDbType.VarChar).Value = Trim(TextBoxADDPRCODE.Text)
            Cmd.Parameters.Add("@5", MySqlDbType.Double).Value = 0
            Cmd.Parameters.Add("@6", MySqlDbType.Double).Value = 0
            Cmd.Parameters.Add("@7", MySqlDbType.Double).Value = 0
            Cmd.Parameters.Add("@8", MySqlDbType.Int64).Value = 0
            Cmd.Parameters.Add("@9", MySqlDbType.Int64).Value = 20
            Cmd.Parameters.Add("@10", MySqlDbType.VarChar).Value = ClientGuid
            Cmd.Parameters.Add("@11", MySqlDbType.Text).Value = FullDate24HR()
            Cmd.Parameters.Add("@12", MySqlDbType.VarChar).Value = ClientCrewID
            Cmd.Parameters.Add("@13", MySqlDbType.VarChar).Value = "Unsynced"
            Cmd.Parameters.Add("@14", MySqlDbType.Text).Value = "N/A"
            Cmd.Parameters.Add("@15", MySqlDbType.Int64).Value = 0
            Cmd.Parameters.Add("@16", MySqlDbType.Int64).Value = 0
            Cmd.Parameters.Add("@17", MySqlDbType.Text).Value = "Local"
            Cmd.ExecuteNonQuery()

            Query = "INSERT INTO loc_admin_products (`product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `guid`, `store_id`, `crew_id`, `synced`, `server_product_id`, `server_inventory_id`, `price_change`, `addontype`) VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19)"
            Cmd = New MySqlCommand(Query, ConnectionLocal)
            Cmd.Parameters.Add("@1", MySqlDbType.VarChar).Value = Trim(TextBoxADDPRCODE.Text)
            Cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = Trim(TextBoxADDNAME.Text)
            Cmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = FormulaID + 1
            Cmd.Parameters.Add("@4", MySqlDbType.VarChar).Value = Trim(TextBoxADDBCODE.Text)
            Cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = "Others"
            Cmd.Parameters.Add("@6", MySqlDbType.Int64).Value = Double.Parse(TextBoxADDPRICE.Text)
            Cmd.Parameters.Add("@7", MySqlDbType.VarChar).Value = Trim(TextBoxADDDESC.Text)
            Cmd.Parameters.Add("@8", MySqlDbType.LongText).Value = TxtBase64.Text
            Cmd.Parameters.Add("@9", MySqlDbType.VarChar).Value = 0
            Cmd.Parameters.Add("@10", MySqlDbType.VarChar).Value = "Local"
            Cmd.Parameters.Add("@11", MySqlDbType.Text).Value = FullDate24HR()
            Cmd.Parameters.Add("@12", MySqlDbType.VarChar).Value = ClientGuid
            Cmd.Parameters.Add("@13", MySqlDbType.Int64).Value = ClientStoreID
            Cmd.Parameters.Add("@14", MySqlDbType.VarChar).Value = ClientCrewID
            Cmd.Parameters.Add("@15", MySqlDbType.VarChar).Value = "Unsynced"
            Cmd.Parameters.Add("@16", MySqlDbType.Int64).Value = 0
            Cmd.Parameters.Add("@17", MySqlDbType.Int64).Value = 0
            Cmd.Parameters.Add("@18", MySqlDbType.Int64).Value = 0
            Cmd.Parameters.Add("@19", MySqlDbType.Text).Value = "N/A"
            Cmd.ExecuteNonQuery()

        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub updatecustomproduct()
        Try
            Dim ConnectionLocal As MySqlConnection = LocalhostConn()

            Dim formula_id = GLOBAL_SELECT_FUNCTION_RETURN("loc_pos_inventory", "formula_id", "`product_ingredients`= '" & Me.TextBoxUPDATENAME.Text & "'", "formula_id")

            table = " loc_admin_products "
            fields = " `product_sku`='" & Me.TextBoxUPDATEPRCODE.Text & "'
                     ,`product_name`='" & Me.TextBoxUPDATENAME.Text & "'
                      ,`product_barcode`='" & Me.TextBoxUPDATEBCODE.Text & "'
                      ,`product_category`='Others'
                      ,`product_price`='" & Me.TextBoxUPDATEPRICE.Text & "'
                      ,`product_desc`='" & Me.TextBoxUPDATEDESC.Text & "'
                      ,`date_modified`='" & FullDate24HR() & "'
                      ,`product_image`='" & Me.RichTextBox1.Text & "' "
            where = " product_id = " & productid
            Dim sql = "UPDATE " & table & " SET " & fields & " WHERE " & where

            Dim cmd As MySqlCommand = New MySqlCommand(sql, ConnectionLocal)
            cmd.ExecuteNonQuery()

            table = " loc_pos_inventory "
            fields = "`product_ingredients`='" & Me.TextBoxUPDATENAME.Text & "',`date_modified`='" & FullDate24HR() & "'"
            where = " formula_id = " & formula_id & ""
            sql = "UPDATE " & table & " SET " & fields & " WHERE " & where

            cmd = New MySqlCommand(sql, ConnectionLocal)
            cmd.ExecuteNonQuery()

            table = " loc_product_formula "
            fields = "`product_ingredients`='" & Me.TextBoxUPDATENAME.Text & "' ,`date_modified`='" & FullDate24HR() & "'"
            where = " formula_id = " & formula_id & ""
            sql = "UPDATE " & table & " SET " & fields & " WHERE " & where

            cmd = New MySqlCommand(sql, ConnectionLocal)
            cmd.ExecuteNonQuery()

            DataGridViewClientProducts.Enabled = True
            messageboxappearance = False
            SystemLogType = "CUSTOM PRODUCT UPDATE"
            SystemLogDesc = "Updated by :" & returnfullname(ClientCrewID) & " : " & ClientRole
            GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
            ClearTextBox(PanelAddCustomProducts)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub rowindex()
        If DataGridViewServerProducts.Rows.Count > 0 Then
            Label10.Text = DataGridViewServerProducts.CurrentCell.RowIndex
        End If
    End Sub
    Private Sub rowindexclient()
        If DataGridViewClientProducts.Rows.Count > 0 Then
            Label12.Text = DataGridViewClientProducts.CurrentCell.RowIndex
        End If
    End Sub
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewServerProducts.CellClick
        rowindex()
    End Sub
    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonCustomProducts.Click
        Try
            PanelAddCustomProducts.Visible = True
            If TextboxIsEmpty(GroupBox2) = True Then
                addcustomproducts()
                clientloadproducts()
                PictureBoxADDIMAGE.Image = Nothing
                ClearTextBox(Me)
                TxtBase64.Text = ""
                clientloadproducts()
                clientloadproductsunapproved()
                MsgBox("Complete")
            Else
                MsgBox("All fields are required")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Sub addcustomprod(ByVal iftrueorfalse As Boolean)
        'Try
        '    If iftrueorfalse = True Then
        '        If ButtonCustomProducts.Text = "Submit" Then
        '            If String.IsNullOrEmpty(TextBoxPRCODE.Text.Trim) Then
        '                TextBoxPRCODE.Clear()
        '                MessageBox.Show("Product Code is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBoxBCODE.Text) Then
        '                MessageBox.Show("Barcode is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBoxNAME.Text) Then
        '                MessageBox.Show("Product Name is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBoxCustomDesc.Text) Then
        '                MessageBox.Show("Description is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBoxPRICE.Text) Then
        '                MessageBox.Show("Product Price is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            Else
        '                addcustomproducts()
        '                clientloadproducts()
        '                PictureBoxCustomAdd.Image = Nothing
        '                ClearTextBox(Me)
        '                TxtBase64.Text = ""
        '                clientloadproducts()
        '                clientloadproductsunapproved()
        '                MsgBox("Complete")
        '            End If
        '        ElseIf ButtonCustomProducts.Text = "Update" Then
        '            If String.IsNullOrEmpty(TextBoxPRCODE.Text.Trim) Then
        '                TextBoxPRCODE.Clear()
        '                MessageBox.Show("Product Code is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBoxBCODE.Text) Then
        '                MessageBox.Show("Barcode is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBoxNAME.Text) Then
        '                MessageBox.Show("Product Name is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBoxCustomDesc.Text) Then
        '                MessageBox.Show("Description is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBoxPRICE.Text) Then
        '                MessageBox.Show("Product Price is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            Else
        '                updatecustomproduct(True)
        '                clientloadproducts()
        '                PictureBoxCustomAdd.Image = Nothing
        '                ClearTextBox(Me)
        '                TxtBase64.Text = ""
        '                clientloadproducts()
        '                clientloadproductsunapproved()
        '                MsgBox("Complete")
        '            End If
        '        End If
        '    Else
        '        If Button10.Text = "Submit" Then
        '            If String.IsNullOrEmpty(TextBox1.Text.Trim) Then
        '                TextBox1.Clear()
        '                MessageBox.Show("Product Code is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBox4.Text) Then
        '                MessageBox.Show("Barcode is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBox5.Text) Then
        '                MessageBox.Show("Product Name is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBox3.Text) Then
        '                MessageBox.Show("Description is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBox2.Text) Then
        '                MessageBox.Show("Product Price is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            Else
        '                addcustomproducts()
        '                clientloadproducts()
        '                PictureBoxCustomAdd.Image = Nothing
        '                ClearTextBox(Me)
        '                TxtBase64.Text = ""
        '                clientloadproducts()
        '                clientloadproductsunapproved()
        '                MsgBox("Complete")
        '            End If
        '        ElseIf Button10.Text = "Update" Then
        '            If String.IsNullOrEmpty(TextBox1.Text.Trim) Then
        '                TextBox1.Clear()
        '                MessageBox.Show("Product Code is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBox4.Text) Then
        '                MessageBox.Show("Barcode is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBox5.Text) Then
        '                MessageBox.Show("Product Name is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBox3.Text) Then
        '                MessageBox.Show("Description is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            ElseIf String.IsNullOrEmpty(TextBox2.Text) Then
        '                MessageBox.Show("Product Price is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '            Else
        '                updatecustomproduct(False)
        '                clientloadproducts()
        '                PictureBoxCustomAdd.Image = Nothing
        '                ClearTextBox(Me)
        '                TxtBase64.Text = ""
        '                clientloadproducts()
        '                clientloadproductsunapproved()
        '                MsgBox("Complete")
        '            End If
        '        End If
        '    End If
        'Catch ex As Exception
        '    MsgBox(ex.ToString)
        '    SendErrorReport(ex.ToString)
        'End Try
    End Sub
    Private Sub DataGridViewClientProducts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewClientProducts.CellClick
        rowindexclient()
    End Sub
    Private Sub ButtonDeleteProducts_Click(sender As Object, e As EventArgs) Handles ButtonDeleteProducts.Click
        deactivateuser(True)
    End Sub
    Private Sub deactivateuser(ByRef iftrueorfalse As Boolean)
        If iftrueorfalse = True Then
            Try
                If DataGridViewClientProducts.Rows.Count > 0 Then
                    productid = DataGridViewClientProducts.SelectedRows(0).Cells(0).Value.ToString()
                    customproductname = DataGridViewClientProducts.SelectedRows(0).Cells(2).Value.ToString()
                    Dim formula_id = DataGridViewClientProducts.SelectedRows(0).Cells(8).Value.ToString()
                    Dim deactivation = MessageBox.Show("Are you sure you want to deactivate ( " & customproductname & " ) product?", "Deactivation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                    If deactivation = DialogResult.Yes Then
                        Try

                            sql = "UPDATE `loc_admin_products` SET product_status = 0  WHERE product_id =" & productid
                            cmd = New MySqlCommand(sql, LocalhostConn)
                            result = cmd.ExecuteNonQuery()
                            sql = "UPDATE `loc_pos_inventory` SET stock_status = 0  WHERE formula_id = " & formula_id
                            cmd = New MySqlCommand(sql, LocalhostConn)
                            result = cmd.ExecuteNonQuery()
                            sql = "UPDATE `loc_product_formula` SET status = 0 WHERE formula_id = " & formula_id
                            cmd = New MySqlCommand(sql, LocalhostConn)
                            result = cmd.ExecuteNonQuery()
                            If result = 1 Then
                                MsgBox("Product Deactivated")
                                messageboxappearance = False
                                SystemLogType = "CUSTOM PRODUCT DEACTIVATION"
                                SystemLogDesc = "Deactivated by :" & returnfullname(ClientCrewID) & " : " & ClientRole
                                GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
                                clientloadproducts()
                                clientloadproductsunapproved()
                            End If
                        Catch ex As Exception
                            MsgBox(ex.ToString)
                        End Try
                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
                SendErrorReport(ex.ToString)
            End Try
        Else
            Try
                If DataGridViewUNAPPROVED.Rows.Count > 0 Then
                    productid = DataGridViewUNAPPROVED.SelectedRows(0).Cells(0).Value.ToString()
                    customproductname = DataGridViewUNAPPROVED.SelectedRows(0).Cells(2).Value.ToString()
                    Dim formula_id = DataGridViewUNAPPROVED.SelectedRows(0).Cells(8).Value.ToString()
                    Dim deactivation = MessageBox.Show("Are you sure you want to deactivate ( " & customproductname & " ) product?", "Deactivation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
                    If deactivation = DialogResult.Yes Then
                        Try
                            sql = "UPDATE `loc_admin_products` SET product_status = 0  WHERE product_id =" & productid
                            cmd = New MySqlCommand(sql, LocalhostConn)
                            result = cmd.ExecuteNonQuery()
                            sql = "UPDATE `loc_pos_inventory` SET stock_status = 0  WHERE formula_id = " & formula_id
                            cmd = New MySqlCommand
                            cmd = New MySqlCommand(sql, LocalhostConn)
                            result = cmd.ExecuteNonQuery()
                            sql = "UPDATE `loc_product_formula` SET status = 0 WHERE formula_id = " & formula_id
                            cmd = New MySqlCommand(sql, LocalhostConn)
                            result = cmd.ExecuteNonQuery()
                            If result = 1 Then
                                MsgBox("Product Deactivated")
                                messageboxappearance = False
                                SystemLogType = "CUSTOM PRODUCT DEACTIVATION"
                                SystemLogDesc = "Deactivated by :" & returnfullname(ClientCrewID) & " : " & ClientRole
                                GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
                                clientloadproducts()
                                clientloadproductsunapproved()
                            End If
                        Catch ex As Exception
                            MsgBox(ex.ToString)
                            SendErrorReport(ex.ToString)
                        End Try
                    End If
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
                SendErrorReport(ex.ToString)
            End Try
        End If
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Panel5.Top = (Me.Height - Panel5.Height) / 3
        Panel5.Left = (Me.Width - Panel5.Width) / 2
        DataGridViewServerProducts.Enabled = False
        Label6.Text = DataGridViewServerProducts.SelectedRows(0).Cells(0).Value.ToString
        TextBoxFROM.Text = DataGridViewServerProducts.SelectedRows(0).Cells(5).Value.ToString
        GroupBox1.Text = DataGridViewServerProducts.SelectedRows(0).Cells(1).Value.ToString & "-" & DataGridViewServerProducts.SelectedRows(0).Cells(2).Value.ToString
        Panel5.Visible = True
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Panel5.Visible = False
        DataGridViewServerProducts.Enabled = True
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        If DataGridViewUNAPPROVED.SelectedRows.Count = 1 Then
            'Label18.Text = "EDIT PRODUCT"
            Button10.Text = "Update"
            Button10.BackColor = Color.FromArgb(221, 114, 46)
            Panel10.Top = (Me.Height - Panel10.Height) / 4
            Panel10.Left = (Me.Width - Panel10.Width) / 3
            Panel10.Visible = True
            fillcustomproducts(False)
        End If

    End Sub
    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Panel10.Visible = False
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        chooseimage(False)
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Try
            Panel10.Visible = False
            If TextboxIsEmpty(GroupBox3) = True Then
                updatecustomproduct()
                clientloadproducts()
                'PictureBoxCustomAdd.Image = Nothing
                ClearTextBox(Me)
                TxtBase64.Text = ""
                clientloadproducts()
                clientloadproductsunapproved()
                MsgBox("Complete")
            Else
                MsgBox("All fields are required")
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        deactivateuser(False)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PriceRequest()
        LoadPriceChange()
    End Sub
    Private Sub PriceRequest()
        Try
            If Trim(TextBoxTO.Text) <> "" Then
                Dim server_product_id = DataGridViewServerProducts.SelectedRows(0).Cells(8).Value
                Dim sql = "INSERT INTO loc_price_request_change(`server_product_id`, `request_price`, `created_at`, `active`, `store_id`, `crew_id`, `guid`, `synced`) VALUES (@1, @2, @3, @4 ,@5 ,@6, @7, @8)"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                cmd.Parameters.Add("@1", MySqlDbType.Text).Value = server_product_id
                cmd.Parameters.Add("@2", MySqlDbType.Text).Value = TextBoxTO.Text
                cmd.Parameters.Add("@3", MySqlDbType.Text).Value = FullDate24HR()
                cmd.Parameters.Add("@4", MySqlDbType.Text).Value = 1
                cmd.Parameters.Add("@5", MySqlDbType.Text).Value = ClientStoreID
                cmd.Parameters.Add("@6", MySqlDbType.Text).Value = ClientCrewID
                cmd.Parameters.Add("@7", MySqlDbType.Text).Value = ClientGuid
                cmd.Parameters.Add("@8", MySqlDbType.Text).Value = "Unsynced"
                cmd.ExecuteNonQuery()
                MsgBox("Complete. Please wait for the admin approval")
                Panel5.Visible = False
            Else
                MsgBox("Fill up all required fields")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub TextBoxFROM_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxTO.KeyPress, TextBoxFROM.KeyPress
        Try
            Numeric(sender, e)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub TextBoxPRCODE_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxADDPRCODE.KeyPress, TextBoxADDNAME.KeyPress, TextBoxADDDESC.KeyPress, TextBoxADDBCODE.KeyPress
        Try
            If InStr(DisallowedCharacters, e.KeyChar) > 0 Then
                e.Handled = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub ButtonKeyboard_Click(sender As Object, e As EventArgs) Handles ButtonKeyboard.Click
        ShowKeyboard()
    End Sub

    Private Sub TextBoxPRICE_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxADDPRICE.KeyPress
        Try
            Numeric(sender, e)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxUPDATEPRICE.KeyPress
        Try
            Numeric(sender, e)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
End Class

