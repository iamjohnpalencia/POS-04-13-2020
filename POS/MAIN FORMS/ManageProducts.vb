Imports MySql.Data.MySqlClient
Imports System.Drawing.Imaging
Public Class ManageProducts
    Private ImagePath As String = ""
    Dim productid
    Dim customproductname
    Dim result As Integer
    Private Sub Reports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TabControl1.TabPages(0).Text = "Product List"
        TabControl1.TabPages(1).Text = "Custom Product List(Approved)"
        TabControl1.TabPages(2).Text = "Custom Product List(Unapproved)"
        serverloadproducts()
        clientloadproducts()
        clientloadproductsunapproved()
        'loadindexdgv()
    End Sub
    'Public Sub loadindexdgv()
    '    If DataGridViewServerProducts.RowCount > 0 Then
    '        DataGridViewServerProducts.ClearSelection()
    '        DataGridViewServerProducts.CurrentCell = DataGridViewServerProducts.Rows(Val(Label10.Text)).Cells(1)
    '        DataGridViewServerProducts.Rows(Val(Label10.Text)).Selected = True
    '        selectimage()
    '    End If
    '    If DataGridViewClientProducts.RowCount > 0 Then
    '        DataGridViewClientProducts.ClearSelection()
    '        DataGridViewClientProducts.CurrentCell = DataGridViewClientProducts.Rows(Val(Label12.Text)).Cells(1)
    '        DataGridViewClientProducts.Rows(Val(Label12.Text)).Selected = True
    '        selectimagecustom()
    '    End If
    'End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Label11.Text = "ADD PRODUCT"
        ButtonCustomProducts.Text = "Submit"
        ButtonCustomProducts.BackColor = Color.FromArgb(84, 177, 140)
        PanelAddCustomProducts.Top = (Me.Height - PanelAddCustomProducts.Height) / 4
        PanelAddCustomProducts.Left = (Me.Width - PanelAddCustomProducts.Width) / 3
        PanelAddCustomProducts.Visible = True
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridViewClientProducts.SelectedRows.Count = 1 Then
            DataGridViewClientProducts.Enabled = False
            Label11.Text = "EDIT PRODUCT"
            ButtonCustomProducts.Text = "Update"
            ButtonCustomProducts.BackColor = Color.FromArgb(221, 114, 46)
            PanelAddCustomProducts.Top = (Me.Height - PanelAddCustomProducts.Height) / 4
            PanelAddCustomProducts.Left = (Me.Width - PanelAddCustomProducts.Width) / 3
            PanelAddCustomProducts.Visible = True
            fillcustomproducts(True)
        End If
    End Sub
    Private Sub OfdImage_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        ImagePath = OpenFileDialog1.FileName
    End Sub
    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        PanelAddCustomProducts.Visible = False
        ClearTextBox(PanelAddCustomProducts)
        PictureBoxCustomAdd.Image = Nothing
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
                    PictureBoxCustomAdd.Image = Image.FromFile(OpenFileDialog1.FileName)
                    PictureBoxCustomAdd.SizeMode = PictureBoxSizeMode.StretchImage
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
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
            End Try
        End If
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        MDIFORM.Button2.PerformClick()
    End Sub
    Public Sub fillcustomproducts(ByVal iftrueorfalse As Boolean)
        If iftrueorfalse = True Then
            If DataGridViewClientProducts.Rows.Count > 0 Then
                productid = DataGridViewClientProducts.SelectedRows(0).Cells(0).Value.ToString()
                Try
                    dbconnection()
                    sql = "SELECT * FROM loc_admin_products WHERE product_id =" & productid
                    da = New MySqlDataAdapter(sql, localconn)
                    dt = New DataTable
                    da.Fill(dt)
                    For Each row As DataRow In dt.Rows
                        TextBoxPRCODE.Text = row("product_sku")
                        TextBoxBCODE.Text = row("product_barcode")
                        TextBoxNAME.Text = row("product_name")
                        TextBoxCustomDesc.Text = row("product_desc")
                        TextBoxPRICE.Text = row("product_price")
                        TxtBase64.Text = row("product_image")
                        PictureBoxCustomAdd.Image = Base64ToImage(row("product_image"))
                        PictureBoxCustomAdd.SizeMode = PictureBoxSizeMode.StretchImage
                    Next
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
            End If
        Else
            If DataGridViewUNAPPROVED.Rows.Count > 0 Then
                productid = DataGridViewUNAPPROVED.SelectedRows(0).Cells(0).Value.ToString()
                Try
                    dbconnection()
                    sql = "SELECT * FROM loc_admin_products WHERE product_id =" & productid
                    da = New MySqlDataAdapter(sql, localconn)
                    dt = New DataTable
                    da.Fill(dt)
                    For Each row As DataRow In dt.Rows
                        TextBox1.Text = row("product_sku")
                        TextBox4.Text = row("product_barcode")
                        TextBox5.Text = row("product_name")
                        TextBox3.Text = row("product_desc")
                        TextBox2.Text = row("product_price")
                        RichTextBox1.Text = row("product_image")
                        PictureBox2.Image = Base64ToImage(row("product_image"))
                        PictureBox2.SizeMode = PictureBoxSizeMode.StretchImage
                    Next
                Catch ex As Exception
                    MsgBox(ex.ToString)
                End Try
            End If
        End If

    End Sub
    Public Sub serverloadproducts()
        Try
            table = "loc_admin_products"
            fields = "product_id, product_sku, product_name  , product_barcode, product_category, product_price, product_status, origin"
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
                .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                .Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
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
        End Try
    End Sub
    Private Sub addcustomproducts()
        Try
            messageboxappearance = False
            table = "loc_product_formula"
            fields = "(`product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `created_at`, `store_id`, `guid`)"
            value = "('" & TextBoxNAME.Text & "'
                , 'piece(s)'
                , " & 1 & "
                , 'N/A'
                , " & 0 & "
                , 'piece(s)'
                , " & 1 & "
                , " & 1 & "
                , " & 0 & "
                , '" & Format(Now, ("yyyy-MM-dd HH:mm:ss")) & "'
                , '" & ClientStoreID & "'
                , '" & ClientGuid & "')"
            GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, errormessage:="", successmessage:="")
        Catch ex As Exception
            MsgBox("Error 2.0")
            SystemLogType = "ERROR"
            SystemLogDesc = "Error 2.0 ADDING FORMULA" & ex.ToString
            GLOBAL_SYSTEM_LOGS(systemlogtype, SystemLogDesc)
        End Try
        Try
            messageboxappearance = False
            table = "loc_pos_inventory"
            fields = "(`store_id`, `formula_id`, `product_ingredients`, `stock_status`, `guid`, `date_modified`, `synced`, `critical_limit`)"
            value = "('" & ClientStoreID & "'
                    , " & selectmaxformula(whatid:="formula_id", fromtable:="loc_product_formula", flds:="formula_id") & "
                    , '" & Me.TextBoxNAME.Text & "' 
                    , " & 0 & "
                    , '" & ClientGuid & "'
                    , '" & Format(Now, ("yyyy-MM-dd HH:mm:ss")) & "'
                    , 'Unsynced'
                    , '" & 10 & "')"
            successmessage = "Successfully Added!"
            errormessage = "error manageproducts(loc_admin_products)"
            GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, errormessage:=errormessage, successmessage:=successmessage)
        Catch ex As Exception
            MsgBox("Error 2.0")
            messageboxappearance = False
            SystemLogType = "Error Adding Inventory"
            SystemLogDesc = ex.ToString
            GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
        End Try
        Try
            messageboxappearance = True
            table = "loc_admin_products"
            fields = "(`product_sku`,`product_name`,`formula_id`,`product_barcode`,`product_category`,`product_price`,`product_desc`,`product_image`,`origin`,`product_status`,`guid`,`store_id`,`crew_id`,`synced`)"
            value = "('" & Me.TextBoxPRCODE.Text & "'
                    , '" & Me.TextBoxNAME.Text & "'
                    , " & selectmaxformula(whatid:="formula_id", fromtable:="loc_product_formula", flds:="formula_id") & "
                    , '" & Me.TextBoxBCODE.Text & "'
                    , 'Others'
                    , '" & Me.TextBoxPRICE.Text & "'
                    , '" & Me.TextBoxCustomDesc.Text & "'
                    , '" & Me.TxtBase64.Text & "'
                    , 'Local'
                    , " & 0 & "
                    , '" & ClientGuid & "'
                    , " & ClientStoreID & "
                    , '" & ClientCrewID & "'
                    , 'Unsynced')"
            successmessage = "Successfully Added!"
            errormessage = "error manageproducts(loc_admin_products)"
            GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, errormessage:=errormessage, successmessage:=successmessage)

            SystemLogType = "NEW CUSTOM PRODUCT"
            SystemLogDesc = "Added by :" & returnfullname(ClientCrewID) & " : " & ClientRole & " : product name: " & TextBoxNAME.Text
            GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
        Catch ex As Exception
            MsgBox("Error 2.0")
            messageboxappearance = False
            SystemLogType = "ERROR"
            SystemLogDesc = "Error 2.0 ADDING OF CUSTOM PRODUCTS" & ex.ToString
            GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
        End Try

        'ClearTextBox(PanelAddCustomProducts)
    End Sub
    Private Sub updatecustomproduct(ByVal iftrueorfalse As Boolean)
        If iftrueorfalse = True Then
            Dim formula_id = DataGridViewClientProducts.SelectedRows(0).Cells(8).Value.ToString()
            Try

                table = " loc_admin_products "
                fields = " `product_sku`='" & Me.TextBoxPRCODE.Text & "'
                     ,`product_name`='" & Me.TextBoxNAME.Text & "'
                      ,`product_barcode`='" & Me.TextBoxBCODE.Text & "'
                      ,`product_category`='Others'
                      ,`product_price`='" & Me.TextBoxPRICE.Text & "'
                      ,`product_desc`='" & Me.TextBoxCustomDesc.Text & "'
                      ,`product_image`='" & Me.TxtBase64.Text & "' "
                where = " product_id = " & productid
                GLOBAL_FUNCTION_UPDATE(table, fields, where)
            Catch ex As Exception
                messageboxappearance = False
                SystemLogType = "UPDATING CUSTOM PRODUCT"
                SystemLogType = ex.ToString
                GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogType)
            End Try
            Try

                table = " loc_pos_inventory "
                fields = "`product_ingredients`='" & Me.TextBoxNAME.Text & "'"
                where = " formula_id = " & formula_id & ""
                GLOBAL_FUNCTION_UPDATE(table, fields, where)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            Try
                table = " loc_product_formula "
                fields = "`product_ingredients`='" & Me.TextBoxNAME.Text & "' "
                where = " formula_id = " & formula_id & ""
                GLOBAL_FUNCTION_UPDATE(table, fields, where)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            DataGridViewClientProducts.Enabled = True
        Else
            productid = DataGridViewUNAPPROVED.SelectedRows(0).Cells(0).Value
            Dim formula_id = DataGridViewUNAPPROVED.SelectedRows(0).Cells(8).Value.ToString()
            Try
                table = " loc_admin_products "
                fields = " `product_sku`='" & Me.TextBox1.Text & "'
                          ,`product_name`='" & Me.TextBox5.Text & "'
                          ,`product_barcode`='" & Me.TextBox4.Text & "'
                          ,`product_category`='Others'
                          ,`product_price`='" & Me.TextBox2.Text & "'
                          ,`product_desc`='" & Me.TextBox3.Text & "'
                          ,`product_image`='" & Me.RichTextBox1.Text & "' "
                where = " product_id = " & productid
                GLOBAL_FUNCTION_UPDATE(table, fields, where)
            Catch ex As Exception
                messageboxappearance = False
                SystemLogType = "UPDATING CUSTOM PRODUCT"
                SystemLogType = ex.ToString
                GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogType)
            End Try
            Try
                table = " loc_pos_inventory "
                fields = "`product_ingredients`='" & Me.TextBox5.Text & "'"
                where = " formula_id = " & formula_id & ""
                GLOBAL_FUNCTION_UPDATE(table, fields, where)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            Try
                table = " loc_product_formula "
                fields = "`product_ingredients`='" & Me.TextBox5.Text & "' "
                where = " formula_id = " & formula_id & ""
                GLOBAL_FUNCTION_UPDATE(table, fields, where)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
            DataGridViewUNAPPROVED.Enabled = True
        End If
        messageboxappearance = False
        SystemLogType = "CUSTOM PRODUCT UPDATE"
        SystemLogDesc = "Updated by :" & returnfullname(ClientCrewID) & " : " & ClientRole
        GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
        ClearTextBox(PanelAddCustomProducts)

    End Sub
    'Public Sub selectimage()
    '    Try
    '        Dim data As String = DataGridViewServerProducts.SelectedRows(0).Cells(0).Value.ToString()
    '        dbconnection()
    '        cmd = New MySqlCommand("SELECT * FROM `loc_admin_products` WHERE product_id = " & data, localconn)
    '        da = New MySqlDataAdapter(cmd)
    '        dr = cmd.ExecuteReader
    '        If dr.HasRows Then
    '            While dr.Read()
    '                TextBoxDES.Text = dr("product_desc")
    '                PictureBox1.BackgroundImage = Base64ToImage(dr("product_image"))
    '                PictureBox1.BackgroundImageLayout = ImageLayout.Center
    '            End While
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.ToString)
    '    End Try
    'End Sub
    'Public Sub selectimagecustom()
    '    Try
    '        dbconnection()
    '        productid = DataGridViewClientProducts.SelectedRows(0).Cells(0).Value
    '        cmd = New MySqlCommand("SELECT * FROM `loc_admin_products` WHERE product_id = " & productid, localconn)
    '        da = New MySqlDataAdapter(cmd)
    '        dr = cmd.ExecuteReader
    '        If dr.HasRows Then
    '            While dr.Read()
    '                TextBoxdesccust.Text = dr("product_desc")
    '                PictureBoxCustomImage.BackgroundImage = Base64ToImage(dr("product_image"))
    '                PictureBoxCustomImage.BackgroundImageLayout = ImageLayout.Center
    '            End While
    '        End If
    '    Catch ex As Exception
    '        MsgBox(ex.ToString)
    '    End Try
    'End Sub
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
        'selectimage()
    End Sub

    '<================================================================================= ADD CUSTOM PRODUCTS 
    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonCustomProducts.Click
        addcustomprod(True)
    End Sub
    Sub addcustomprod(ByVal iftrueorfalse As Boolean)
        If iftrueorfalse = True Then
            If ButtonCustomProducts.Text = "Submit" Then
                If String.IsNullOrEmpty(TextBoxPRCODE.Text.Trim) Then
                    TextBoxPRCODE.Clear()
                    MessageBox.Show("Product Code is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(TextBoxBCODE.Text) Then
                    MessageBox.Show("Barcode is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(TextBoxNAME.Text) Then
                    MessageBox.Show("Product Name is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(TextBoxCustomDesc.Text) Then
                    MessageBox.Show("Description is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(TextBoxPRICE.Text) Then
                    MessageBox.Show("Product Price is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    addcustomproducts()
                    clientloadproducts()
                    PictureBoxCustomAdd.Image = Nothing
                    ClearTextBox(Me)
                    TxtBase64.Text = ""
                    clientloadproducts()
                    clientloadproductsunapproved()
                End If
            ElseIf ButtonCustomProducts.Text = "Update" Then
                If String.IsNullOrEmpty(TextBoxPRCODE.Text.Trim) Then
                    TextBoxPRCODE.Clear()
                    MessageBox.Show("Product Code is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(TextBoxBCODE.Text) Then
                    MessageBox.Show("Barcode is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(TextBoxNAME.Text) Then
                    MessageBox.Show("Product Name is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(TextBoxCustomDesc.Text) Then
                    MessageBox.Show("Description is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(TextBoxPRICE.Text) Then
                    MessageBox.Show("Product Price is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    updatecustomproduct(True)
                    clientloadproducts()
                    PictureBoxCustomAdd.Image = Nothing
                    ClearTextBox(Me)
                    TxtBase64.Text = ""
                    clientloadproducts()
                    clientloadproductsunapproved()
                End If
            End If
        Else
            If Button10.Text = "Submit" Then
                If String.IsNullOrEmpty(TextBox1.Text.Trim) Then
                    TextBox1.Clear()
                    MessageBox.Show("Product Code is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(textbox4.Text) Then
                    MessageBox.Show("Barcode is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(textbox5.Text) Then
                    MessageBox.Show("Product Name is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(textbox3.Text) Then
                    MessageBox.Show("Description is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(textbox2.Text) Then
                    MessageBox.Show("Product Price is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    addcustomproducts()
                    clientloadproducts()
                    PictureBoxCustomAdd.Image = Nothing
                    ClearTextBox(Me)
                    TxtBase64.Text = ""
                    clientloadproducts()
                    clientloadproductsunapproved()
                End If
            ElseIf Button10.Text = "Update" Then
                If String.IsNullOrEmpty(TextBox1.Text.Trim) Then
                    TextBox1.Clear()
                    MessageBox.Show("Product Code is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(TextBox4.Text) Then
                    MessageBox.Show("Barcode is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(TextBox5.Text) Then
                    MessageBox.Show("Product Name is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(TextBox3.Text) Then
                    MessageBox.Show("Description is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf String.IsNullOrEmpty(TextBox2.Text) Then
                    MessageBox.Show("Product Price is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    updatecustomproduct(False)
                    clientloadproducts()
                    PictureBoxCustomAdd.Image = Nothing
                    ClearTextBox(Me)
                    TxtBase64.Text = ""
                    clientloadproducts()
                    clientloadproductsunapproved()
                End If
            End If
        End If

    End Sub
    Private Sub DataGridViewClientProducts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewClientProducts.CellClick
        rowindexclient()
        'selectimagecustom()
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
                            dbconnection()
                            sql = "DELETE FROM `loc_admin_products` WHERE product_id =" & productid
                            cmd = New MySqlCommand
                            With cmd
                                .CommandText = sql
                                .Connection = localconn
                                result = .ExecuteNonQuery()
                            End With
                            sql = "DELETE FROM `loc_pos_inventory` WHERE formula_id = " & formula_id
                            cmd = New MySqlCommand
                            With cmd
                                .CommandText = sql
                                .Connection = localconn
                                result = .ExecuteNonQuery()
                            End With
                            sql = "DELETE FROM `loc_product_formula` WHERE formula_id = " & formula_id
                            cmd = New MySqlCommand
                            With cmd
                                .CommandText = sql
                                .Connection = localconn
                                result = .ExecuteNonQuery()
                            End With
                            localconn.Close()

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
                            dbconnection()
                            sql = "DELETE FROM `loc_admin_products`WHERE product_id =" & productid
                            cmd = New MySqlCommand
                            With cmd
                                .CommandText = sql
                                .Connection = localconn
                                result = .ExecuteNonQuery()
                            End With
                            sql = "DELETE FROM `loc_pos_inventory` WHERE formula_id = " & formula_id
                            cmd = New MySqlCommand
                            With cmd
                                .CommandText = sql
                                .Connection = localconn
                                result = .ExecuteNonQuery()
                            End With
                            sql = "DELETE FROM `loc_product_formula` WHERE formula_id = " & formula_id
                            cmd = New MySqlCommand
                            With cmd
                                .CommandText = sql
                                .Connection = localconn
                                result = .ExecuteNonQuery()
                            End With
                            localconn.Close()
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
            Label18.Text = "EDIT PRODUCT"
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
        addcustomprod(False)
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        deactivateuser(False)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

    End Sub
    'ADD CUSTOM CATEGORY =================================================================================>
End Class

