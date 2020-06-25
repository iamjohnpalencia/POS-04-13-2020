Imports System.Net.Mail
Imports MySql.Data.MySqlClient
Public Class Refund
    Private Sub ItemReturn_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label12.Text = Date.Now.ToString("yyyy-MM-dd hh:mm:ss tt")
        Timer1.Start()
        loaditemreturn(True)
        loadindexdgv()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label12.Text = Date.Now.ToString("yyyy-MM-dd hh:mm:ss tt")
    End Sub

    Private Sub loaditemreturn(justload As Boolean)
        fields = "transaction_number, amounttendered, discount, moneychange, crew_id, time, vatable, vat_exempt, zero_rated, vat, transaction_type"
        If justload = True Then
            GLOBAL_SELECT_ALL_FUNCTION(table:="loc_daily_transaction WHERE date = CURDATE() AND active = 1", datagrid:=DataGridView1, fields:=fields)
        Else
            If String.IsNullOrWhiteSpace(TextBox1.Text) Then
                FlowLayoutPanel1.Controls.Clear()
                GLOBAL_SELECT_ALL_FUNCTION(table:="loc_daily_transaction WHERE date = CURDATE() AND active = 1", datagrid:=DataGridView1, fields:=fields)
            Else
                FlowLayoutPanel1.Controls.Clear()
                GLOBAL_SELECT_ALL_FUNCTION(table:="loc_daily_transaction WHERE transaction_number LIKE '%" & TextBox1.Text & "%'  AND date = CURDATE() AND active = 1", datagrid:=DataGridView1, fields:=fields)
            End If
        End If
        With DataGridView1
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
    Private Sub rowindex()
        Label2.Text = DataGridView1.CurrentCell.RowIndex
    End Sub
    Dim productimage As String
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        rowindex()
        loadtransactions()
    End Sub
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        loaditemreturn(False)
        If DataGridView1.Rows.Count > 0 Then
            loadtransactions()
        End If
    End Sub
    Dim grandtotal As Decimal
    Private Sub loadtransactions()
        Try
            Dim countrow As Integer = 0
            FlowLayoutPanel1.Controls.Clear()
            sql = "SELECT product_id, product_name, quantity, price, total, product_sku FROM loc_daily_transaction_details WHERE transaction_number = '" & DataGridView1.SelectedRows(0).Cells(0).Value.ToString & "' AND active = 1"
            Dim query As String = "SELECT SUM(TOTAL) FROM loc_daily_transaction_details WHERE transaction_number = '" & DataGridView1.SelectedRows(0).Cells(0).Value.ToString & "'"
            Dim cmdquery As MySqlCommand
            Dim queryda As MySqlDataAdapter
            Dim dt1 As DataTable
            cmdquery = New MySqlCommand(query, LocalhostConn)
            queryda = New MySqlDataAdapter(cmd)
            dt1 = New DataTable
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
                        .Font = New Font("Century Gothic", 11.25)
                        .Width = 345
                        .Height = 140
                        .Cursor = Cursors.Hand
                        With Myname
                            .Location = New Point(200, 10)
                            .Text = row("product_sku")
                            .ForeColor = Color.Black
                            .Font = New Font("Century Gothic", 11.25, FontStyle.Bold)
                            .Width = 200
                        End With
                        With MyQty
                            .Location = New Point(200, 30)
                            .Text = "Quantity: " & row("quantity")
                            .ForeColor = Color.Black
                            .Width = 200
                        End With
                        With MyPrice
                            .Location = New Point(200, 50)
                            .Text = "Price: " & row("price")
                            .ForeColor = Color.Black
                            .Width = 200
                        End With
                        With MyTotal
                            .Location = New Point(200, 70)
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
                With DataGridView1
                    Label30.Text = .SelectedRows(0).Cells(0).Value.ToString
                    Label9.Text = countrow & " item(s)"
                    Label28.Text = .SelectedRows(0).Cells(10).Value.ToString
                    Label13.Text = grandtotal
                    Label15.Text = .SelectedRows(0).Cells(1).Value.ToString
                    Label16.Text = .SelectedRows(0).Cells(3).Value.ToString
                    Label24.Text = .SelectedRows(0).Cells(2).Value.ToString
                    Label18.Text = .SelectedRows(0).Cells(6).Value.ToString
                    Label20.Text = .SelectedRows(0).Cells(7).Value.ToString
                    Label22.Text = .SelectedRows(0).Cells(8).Value.ToString
                    Label26.Text = .SelectedRows(0).Cells(9).Value.ToString
                    ButtonRefund.Text = "Refund PHP " & grandtotal
                End With
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub loadindexdgv()
        If DataGridView1.RowCount > 0 Then
            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = DataGridView1.Rows(Val(Label2.Text)).Cells(0)
            DataGridView1.Rows(Val(Label2.Text)).Selected = True
            loadtransactions()
        Else
            FlowLayoutPanel1.Controls.Clear()
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ButtonRefund.Click
        Try
            Dim transaction_number = DataGridView1.SelectedRows(0).Cells(0).Value.ToString
            If String.IsNullOrWhiteSpace(TextBox2.Text) Then
                MessageBox.Show("Reason for refund is required!", "Refund", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                sql = "SELECT * FROM loc_daily_transaction WHERE date = CURDATE() AND time >= Now() - INTERVAL 10 MINUTE AND transaction_number = '" & transaction_number & "'"
                cmd = New MySqlCommand(sql, LocalhostConn)
                da = New MySqlDataAdapter(cmd)
                dt = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Dim refund As Integer = MessageBox.Show("Are you sure do you want to refund this transaction?", "Return and Refund", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    If refund = DialogResult.Yes Then
                        fields = "(`transaction_number`, `reason`, `total`, `guid`, `store_id`, `crew_id`, `synced`, `zreading`)"
                        value = "('" & transaction_number & "'
                            , '" & TextBox2.Text & "'
                            , " & grandtotal & "
                            , '" & ClientGuid & "'
                            , '" & ClientStoreID & "'
                            , '" & ClientCrewID & "'
                            , 'Unsynced'
                            , '" & S_Zreading & "')"
                        GLOBAL_INSERT_FUNCTION(table:="loc_refund_return_details", fields:=fields, values:=value)
                        table = "loc_daily_transaction"
                        fields = " `active`= 2 AND synced = 'Unsynced' "
                        where = " transaction_number = '" & transaction_number & "'"
                        GLOBAL_FUNCTION_UPDATE(table, fields, where)
                        table = "loc_daily_transaction_details"
                        GLOBAL_FUNCTION_UPDATE(table, fields, where)

                        Label2.Text = 0
                        loaditemreturn(True)
                        loadindexdgv()

                        'ClearTextBox(Panel3)
                        'ClearTextBox(Panel17)
                    End If
                Else
                    MessageBox.Show("This transaction is non refundable or exceed's the given period of time.", "Refund", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class