Imports MySql.Data.MySqlClient
Public Class Test
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Try
        '    serverconn()
        '    sql = "SELECT * FROM admin_products"
        '    cmd = New mys
        '    da = New MySqlDataAdapter(sql, cloudconn)
        '    dt = New DataTable
        '    da.Fill(dt)
        '    DataGridView1.DataSource = dt
        'Catch ex As Exception
        '    MsgBox(ex.ToString)
        'End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            For i As Integer = 0 To DataGridView1.Rows.Count - 1 Step +1
                table = "triggers_loc_admin_products"
                fields = "(`product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `guid`, `ip_address`, `store_id`, `crew_id`, `synced`)"
                value = "('" & DataGridView1.Rows(i).Cells(1).Value & "'
                        ,'" & DataGridView1.Rows(i).Cells(2).Value & "'
                        ,'" & DataGridView1.Rows(i).Cells(3).Value & "'
                        ,'" & DataGridView1.Rows(i).Cells(4).Value & "'
                        ,'" & DataGridView1.Rows(i).Cells(5).Value & "'
                        ," & DataGridView1.Rows(i).Cells(6).Value & "
                        ,'" & DataGridView1.Rows(i).Cells(7).Value & "'
                        ,'" & DataGridView1.Rows(i).Cells(8).Value & "'
                        ,'" & DataGridView1.Rows(i).Cells(9).Value & "'
                        ,'" & DataGridView1.Rows(i).Cells(10).Value & "'
                        ,'" & returndatetimeformat(DataGridView1.Rows(i).Cells(11).Value) & "'
                        ,'" & DataGridView1.Rows(i).Cells(12).Value & "'
                        ,''
                        ,''
                        ,''
                        ,'Synced')"
                GLOBAL_INSERT_FUNCTION(table:=table, errormessage:="", fields:=fields, successmessage:="", values:=value)
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class