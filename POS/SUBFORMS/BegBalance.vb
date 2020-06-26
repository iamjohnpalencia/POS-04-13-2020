Imports MySql.Data.MySqlClient
Public Class BegBalance
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged, TextBox2.TextChanged, TextBox3.TextChanged, TextBox4.TextChanged, TextBox5.TextChanged, TextBox6.TextChanged, TextBox7.TextChanged, TextBox8.TextChanged, TextBox9.TextChanged, TextBox10.TextChanged
        Dim OneThousand As Decimal = Val(TextBox1.Text) * Val(Label1.Text)
        Dim FiveHundred As Decimal = Val(TextBox2.Text) * Val(Label2.Text)
        Dim TwoHundred As Decimal = Val(TextBox3.Text) * Val(Label3.Text)
        Dim OneHundred As Decimal = Val(TextBox4.Text) * Val(Label4.Text)
        Dim Fifty As Decimal = Val(TextBox5.Text) * Val(Label5.Text)
        Dim Twenty As Decimal = Val(TextBox6.Text) * Val(Label6.Text)
        Dim Ten As Decimal = Val(TextBox7.Text) * Val(Label7.Text)
        Dim Five As Decimal = Val(TextBox8.Text) * Val(Label8.Text)
        Dim One As Decimal = Val(TextBox9.Text) * Val(Label9.Text)
        Dim TwentyFiveCents As Decimal = Val(TextBox10.Text) * Val(Label10.Text)
        Label12.Text = OneThousand + FiveHundred + TwoHundred + OneHundred + Fifty + Twenty + Ten + Five + One + TwentyFiveCents
    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress, TextBox2.KeyPress, TextBox3.KeyPress, TextBox4.KeyPress, TextBox5.KeyPress, TextBox6.KeyPress, TextBox7.KeyPress, TextBox8.KeyPress, TextBox9.KeyPress, TextBox10.KeyPress
        Numeric(sender:=sender, e:=e)
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If ComboBox1.Text <> "" Then
                If Val(Label12.Text) <> 0 Then
                    InsertBeginningBalance()
                    With POS
                        If .DataGridViewPRODUCTUPDATE.Rows.Count > 0 Or .DataGridViewFORMULAUPDATE.Rows.Count > 0 Or .DataGridViewCATEGORYUPDATE.Rows.Count > 0 Or .DataGridViewINVENTORYUPDATE.Rows.Count > 0 Then
                            Dim updatemessage = MessageBox.Show("New Updates are available. Would you like to update now ?", "New Updates", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                            If updatemessage = DialogResult.Yes Then
                                InstallUpdatesFormula()
                                InstallUpdatesInventory()
                                InstallUpdatesCategory()
                                InstallUpdatesProducts()
                                listviewproductsshow(where:="Simply Perfect")
                            End If
                        End If
                    End With
                Else
                    Dim message As Integer = MessageBox.Show("Cash drawer has zero balance. Do you want to proceed ?", "Zero Balance", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    If message = DialogResult.Yes Then
                        InsertBeginningBalance()
                        With POS
                            If .DataGridViewPRODUCTUPDATE.Rows.Count > 0 Or .DataGridViewFORMULAUPDATE.Rows.Count > 0 Or .DataGridViewCATEGORYUPDATE.Rows.Count > 0 Or .DataGridViewINVENTORYUPDATE.Rows.Count > 0 Then
                                Dim updatemessage = MessageBox.Show("New Updates are available. Would you like to update now ?", "New Updates", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                                If updatemessage = DialogResult.Yes Then
                                    InstallUpdatesFormula()
                                    InstallUpdatesInventory()
                                    InstallUpdatesCategory()
                                    InstallUpdatesProducts()
                                    listviewproductsshow(where:="Simply Perfect")
                                End If
                            End If
                        End With
                    End If
                End If
            Else
                MessageBox.Show("Select shift first.", "Select shift", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertBeginningBalance()
        If ComboBox1.Text = "First Shift" Then
            SystemLogType = "BG-1"
        ElseIf ComboBox1.Text = "Second Shift" Then
            SystemLogType = "BG-2"
        ElseIf ComboBox1.Text = "Third Shift" Then
            SystemLogType = "BG-3"
        Else
            SystemLogType = "BG-4"
        End If
        Shift = ComboBox1.Text
        BeginningBalance = Val(Label12.Text)
        SystemLogDesc = Label12.Text
        GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
        Dim message As String = GetMonthName(Format(Now(), "yyyy-MM-dd")) & " " & Format(Now(), "dd yyyy") & vbNewLine & Format(Now(), "hh:mm tt")
        'MessageBox.Show(message, "Set Cash Datetime:", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'Settings.Button8.Visible = False
        Me.Close()
    End Sub
    Private Sub BegBalance_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        POS.Enabled = True
    End Sub
    Private Sub BegBalance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
    End Sub
    Private Sub InstallUpdatesCategory()
        Try
            Dim cmdlocal As MySqlCommand
            With POS.DataGridViewCATEGORYUPDATE
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim sql = "SELECT category_id FROM loc_admin_category WHERE category_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                    If result = 0 Then
                        Dim sqlinsert = "INSERT INTO `loc_admin_category`(`category_name`, `brand_name`, `updated_at`, `origin`, `status`) VALUES (@0,@1,@2,@3,@4)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    Else
                        Dim sqlupdate = "UPDATE `loc_admin_category` SET `category_name`=@0,`brand_name`=@1,`updated_at`=@2,`origin`=@3,`status`=@4 WHERE category_id = " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlupdate, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InstallUpdatesFormula()
        Try
            Dim cmdlocal As MySqlCommand
            With POS.DataGridViewFORMULAUPDATE
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim sql = "SELECT formula_id FROM loc_product_formula WHERE formula_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                    If result = 0 Then
                        Dim sqlinsert = "INSERT INTO loc_product_formula (`server_formula_id`,`product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin`, `store_id`, `guid`, `crew_id`, `server_date_modified`) VALUES
                                        (@0 ,@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11 , @12 , @13 , @14, @15, @16)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Int64).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.Decimal).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@16", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    Else
                        Dim sqlupdate = "UPDATE `loc_product_formula` SET 
`server_formula_id`= @0
,`product_ingredients`= @1
,`primary_unit`= @2
,`primary_value`= @3
,`secondary_unit`= @4
,`secondary_value`=@5
,`serving_unit`=@6
,`serving_value`=@7
,`no_servings`=@8
,`status`=@9
,`date_modified`=@10
,`unit_cost`=@11
,`origin`=@12
,`store_id`=@13
,`guid`=@14
,`server_date_modified`=@15 WHERE server_formula_id =  " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlupdate, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Int64).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.Decimal).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                        cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                        cmdlocal.ExecuteNonQuery()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InstallUpdatesInventory()
        Try
            Dim cmdlocal As MySqlCommand
            With POS.DataGridViewINVENTORYUPDATE
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim sql = "SELECT inventory_id FROM loc_pos_inventory WHERE inventory_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                    If result = 0 Then
                        Dim sqlinsert = "INSERT INTO loc_pos_inventory (`server_inventory_id`=,`formula_id`,`product_ingredients`,`sku`,`stock_primary`,`stock_secondary`,`stock_no_of_servings`,`stock_status`,`critical_limit`,`created_at`,`server_date_modified`) VALUES
                                        (@0 ,@1, @2, @3, @4, @5, @6, @7, @8, @9, @10)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.Int64).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.Int64).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.Int64).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    Else

                        Dim sqlUpdate = "UPDATE `loc_pos_inventory` SET `server_inventory_id`= @0,`formula_id`=@1,`product_ingredients`=@2,`sku`=@3,`stock_primary`=@4,`stock_secondary`=@5,`stock_no_of_servings`=@6,`stock_status`=@7,`critical_limit`=@8,`created_at`=@9,`server_date_modified`=@10 WHERE `server_inventory_id`= " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlUpdate, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.Int64).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.Int64).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.Int64).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InstallUpdatesProducts()
        Try
            Dim cmdlocal As MySqlCommand
            With POS.DataGridViewPRODUCTUPDATE
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    Dim sql = "SELECT product_id FROM loc_admin_products WHERE product_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                    If result = 0 Then
                        Dim sqlinsert = "INSERT INTO loc_admin_products (`server_product_id`, `product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `guid`, `store_id`, `crew_id`, `synced`) VALUES
                                        (@0 ,@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.Int64).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@13", MySqlDbType.Int64).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = "Synced"
                        cmdlocal.ExecuteNonQuery()
                    Else
                        Dim sqlupdate = "UPDATE `loc_admin_products` SET `server_product_id`=@0,`product_sku`=@1,`product_name`=@2,`formula_id`=@3,`product_barcode`=@4,`product_category`=@5,`product_price`=@6,`product_desc`=@7,`product_image`=@8,`product_status`=@9,`origin`=@10,`date_modified`=@11,`guid`=@12,`store_id`=@13,`crew_id`=@14,`synced`=@15 WHERE server_product_id =  " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlupdate, LocalhostConn())
                        cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                        cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.Int64).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = ClientGuid
                        cmdlocal.Parameters.Add("@13", MySqlDbType.Int64).Value = ClientStoreID
                        cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = "0"
                        cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = "Synced"
                        cmdlocal.ExecuteNonQuery()
                    End If
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class
