Option Explicit On
Imports MySql.Data.MySqlClient
Imports System.Drawing.Imaging
Public Class ManageProducts

    Private Shared _instance As ManageProducts
    Public ReadOnly Property Instance As ManageProducts
        Get
            Return _instance
        End Get
    End Property
    Private Sub ManageProducts_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _instance = Me
        Try
            TabControl1.TabPages(0).Text = "Product List"
            TabControl1.TabPages(1).Text = "Custom Products(Approved)"
            TabControl1.TabPages(2).Text = "Custom Products(Pending)"
            TabControl1.TabPages(3).Text = "Product Price Change"
            LoadProductList()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub LoadProductList()
        Try
            GLOBAL_SELECT_ALL_FUNCTION("loc_admin_products WHERE `product_category` <> 'Others' AND product_status = 1 ORDER BY `product_category` ASC ", "`product_id`, `server_product_id`, `product_sku`, `product_name`, `product_barcode`, `product_category`, `product_price`, `product_desc`,`product_status`, `origin`, `date_modified`", DataGridViewProductList)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub ButtonPriceChange_Click(sender As Object, e As EventArgs) Handles ButtonPriceChange.Click
        Try
            If DataGridViewProductList.SelectedRows.Count = 1 Then
                If DataGridViewProductList.SelectedRows(0).Cells(9).Value.ToString = "Server" Then
                    Enabled = False
                    With ChangePrice
                        .ProductID = DataGridViewProductList.SelectedRows(0).Cells(1).Value
                        .PriceFrom = DataGridViewProductList.SelectedRows(0).Cells(6).Value
                        .Product = DataGridViewProductList.SelectedRows(0).Cells(3).Value
                        .Show()
                        MDIFORM.Button5.Focus()
                    End With
                Else
                    MsgBox("This feature is for server product only")
                End If
            Else
                MsgBox("Select product first")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub LoadOthersApprove()
        Try
            GLOBAL_SELECT_ALL_FUNCTION("loc_admin_products WHERE `product_category` = 'Others' AND product_status = 1 ORDER BY `product_category` ASC ", "`product_id`, `server_product_id`, `product_sku`, `product_name`, `product_barcode`, `product_category`, `product_price`, `product_desc`,`product_status`, `origin`, `date_modified`", DataGridViewOthersApproved)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub LoadOthersPending()
        Try
            GLOBAL_SELECT_ALL_FUNCTION("loc_admin_products WHERE `product_category` = 'Others' AND product_status = 0 ORDER BY `product_category` ASC ", "`product_id`, `server_product_id`, `product_sku`, `product_name`, `product_barcode`, `product_category`, `product_price`, `product_desc`,`product_status`, `origin`, `date_modified`", DataGridViewOthersPending)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub LoadPriceChange()
        Try
            GLOBAL_SELECT_ALL_FUNCTION("loc_price_request_change", "*", DataGridViewPriceRequest)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub ManageProducts_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            Dim newMDIchild As New ChangePrice()
            If Application.OpenForms().OfType(Of ChangePrice).Any Then
                ChangePrice.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        Try
            If TabControl1.SelectedIndex = 1 Then
                Dim Loaded As Boolean = False
                If Loaded = False Then
                    LoadOthersApprove()
                    Loaded = True
                End If
            ElseIf TabControl1.SelectedIndex = 2 Then
                Dim Loaded As Boolean = False
                If Loaded = False Then
                    LoadOthersPending()
                    Loaded = True
                End If
            ElseIf TabControl1.SelectedIndex = 3 Then
                Dim Loaded As Boolean = False
                If Loaded = False Then
                    LoadPriceChange()
                    Loaded = True
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles ButtonAddCustomProduct.Click
        Try
            AddEditProducts.AddNewProduct = True
            AddEditProducts.TopMost = True
            AddEditProducts.Show()
            Enabled = False
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles ButtonEditApprovedProducts.Click
        Try
            With DataGridViewOthersApproved
                If .SelectedRows.Count > 0 Then
                    If .SelectedRows(0).Cells(9).Value.ToString <> "Server" Then
                        AddEditProducts.TopMost = True
                        AddEditProducts.productcode = .SelectedRows(0).Cells(2).Value.ToString
                        AddEditProducts.product = .SelectedRows(0).Cells(3).Value.ToString
                        AddEditProducts.productbarcode = .SelectedRows(0).Cells(4).Value.ToString
                        AddEditProducts.productprice = .SelectedRows(0).Cells(6).Value.ToString
                        AddEditProducts.productdesc = .SelectedRows(0).Cells(7).Value.ToString
                        AddEditProducts.productid = .SelectedRows(0).Cells(0).Value.ToString
                        AddEditProducts.AddNewProduct = False
                        AddEditProducts.Show()
                        Enabled = False
                    Else
                        MsgBox("Server products cannot be edited")
                    End If
                Else
                    MsgBox("Select product first")
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles ButtonEditCustomProduct.Click
        Try
            With DataGridViewOthersPending
                If .SelectedRows.Count > 0 Then
                    If .SelectedRows(0).Cells(9).Value.ToString <> "Server" Then
                        AddEditProducts.TopMost = True
                        AddEditProducts.productcode = .SelectedRows(0).Cells(2).Value.ToString
                        AddEditProducts.product = .SelectedRows(0).Cells(3).Value.ToString
                        AddEditProducts.productbarcode = .SelectedRows(0).Cells(4).Value.ToString
                        AddEditProducts.productprice = .SelectedRows(0).Cells(6).Value.ToString
                        AddEditProducts.productdesc = .SelectedRows(0).Cells(7).Value.ToString
                        AddEditProducts.productid = .SelectedRows(0).Cells(0).Value.ToString
                        AddEditProducts.AddNewProduct = False
                        AddEditProducts.Show()
                        Enabled = False
                    Else
                        MsgBox("Server products cannot be edited")
                    End If
                Else
                    MsgBox("Select product first")
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles ButtonDeactivateApprovedProduct.Click
        Try
            If DataGridViewOthersApproved.SelectedRows.Count <> 0 Then
                DeactivateProduct(DataGridViewOthersApproved)
            Else
                MsgBox("Select product first")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub ButtonDeletePending_Click(sender As Object, e As EventArgs) Handles ButtonDeletePending.Click
        Try
            If DataGridViewOthersPending.SelectedRows.Count <> 0 Then
                DeactivateProduct(DataGridViewOthersPending)
            Else
                MsgBox("Select product first")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub DeactivateProduct(Datagrid As DataGridView)
        Try
            With Datagrid
                If .SelectedRows.Count > 0 Then
                    If .SelectedRows(0).Cells(9).Value.ToString <> "Server" Then
                        Dim msg = MessageBox.Show("Are you sure you want to deactivate this product?", "Product Deactivation", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                        If msg = DialogResult.Yes Then
                            Dim productid = .SelectedRows(0).Cells(0).Value.ToString
                            Dim ProductFormulaID = returnselect("formula_id", "loc_admin_products WHERE product_id = " & productid)

                            Dim sql = "UPDATE loc_admin_products SET product_status = 2 WHERE product_id = " & productID
                            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                            cmd.ExecuteNonQuery()

                            sql = "UPDATE loc_pos_inventory SET stock_status = 2 WHERE formula_id = " & ProductFormulaID
                            cmd = New MySqlCommand(sql, LocalhostConn)
                            cmd.ExecuteNonQuery()

                            sql = "UPDATE loc_product_formula SET status = 2 WHERE formula_id = " & ProductFormulaID
                            cmd = New MySqlCommand(sql, LocalhostConn)
                            cmd.ExecuteNonQuery()
                            MsgBox("Product deactivated.")
                            LoadOthersApprove()
                            LoadOthersPending()
                        End If
                    Else
                        MsgBox("Server products cannot be deleted")
                    End If
                Else
                    MsgBox("Select product first")
                End If
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
End Class

