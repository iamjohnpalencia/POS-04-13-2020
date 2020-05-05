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
                        End If
                    End With
                Else
                    Dim message As Integer = MessageBox.Show("Cash drawer has zero balance. Do you want to proceed ?", "Zero Balance", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    If message = DialogResult.Yes Then
                        InsertBeginningBalance()
                        With POS
                            If .DataGridViewPRODUCTUPDATE.Rows.Count > 0 Or .DataGridViewFORMULAUPDATE.Rows.Count > 0 Or .DataGridViewCATEGORYUPDATE.Rows.Count > 0 Or .DataGridViewINVENTORYUPDATE.Rows.Count > 0 Then
                                Dim updatemessage = MessageBox.Show("New Updates are available. Would you like to update now ?", "New Updates", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
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
                        Dim sqlinsert = "INSERT INTO loc_admin_category (`category_name`, `brand_name`, `updated_at`, `origin`, `status`) VALUES
                                        ('" & .Rows(i).Cells(1).Value & "','" & .Rows(i).Cells(2).Value & "','" & returndateformat(.Rows(i).Cells(3).Value) & "','Server',1)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.ExecuteNonQuery()
                    Else
                        Dim sqlupdate = "UPDATE loc_admin_category SET category_name = '" & .Rows(i).Cells(1).Value & "', brand_name = '" & .Rows(i).Cells(2).Value & "', updated_at = '" & returndateformat(.Rows(i).Cells(3).Value) & "', status = " & .Rows(i).Cells(4).Value & " WHERE category_id = " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlupdate, LocalhostConn())
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
                        Dim sqlinsert = "INSERT INTO loc_product_formula (`product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `store_id`, `guid`, `crew_id`, `origin`) VALUES
                                        (@1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11 , @12 , @13 , @14, @15, @16)"
                        cmdlocal = New MySqlCommand(sqlinsert, LocalhostConn())
                        cmdlocal.Parameters.Add("@1", MySqlDbType.Int64).Value = .Rows(i).Cells(1).Value.ToString()
                        cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                        cmdlocal.Parameters.Add("@3", MySqlDbType.Decimal).Value = .Rows(i).Cells(3).Value.ToString()
                        cmdlocal.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                        cmdlocal.Parameters.Add("@5", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                        cmdlocal.Parameters.Add("@6", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                        cmdlocal.Parameters.Add("@7", MySqlDbType.Decimal).Value = .Rows(i).Cells(7).Value.ToString()
                        cmdlocal.Parameters.Add("@8", MySqlDbType.Decimal).Value = .Rows(i).Cells(8).Value.ToString()
                        cmdlocal.Parameters.Add("@9", MySqlDbType.Decimal).Value = .Rows(i).Cells(9).Value.ToString()
                        cmdlocal.Parameters.Add("@10", MySqlDbType.Decimal).Value = .Rows(i).Cells(10).Value.ToString()
                        cmdlocal.Parameters.Add("@11", MySqlDbType.Decimal).Value = .Rows(i).Cells(11).Value.ToString()
                        cmdlocal.Parameters.Add("@12", MySqlDbType.Decimal).Value = .Rows(i).Cells(12).Value.ToString()
                        cmdlocal.Parameters.Add("@13", MySqlDbType.Decimal).Value = .Rows(i).Cells(13).Value.ToString()
                        cmdlocal.Parameters.Add("@14", MySqlDbType.Decimal).Value = .Rows(i).Cells(14).Value.ToString()
                        cmdlocal.Parameters.Add("@15", MySqlDbType.Decimal).Value = .Rows(i).Cells(15).Value.ToString()
                        cmdlocal.Parameters.Add("@16", MySqlDbType.Decimal).Value = .Rows(i).Cells(15).Value.ToString()
                        cmdlocal.ExecuteNonQuery()
                    Else
                        Dim sqlupdate = "UPDATE loc_admin_category SET category_name = '" & .Rows(i).Cells(1).Value & "', brand_name = '" & .Rows(i).Cells(2).Value & "', updated_at = '" & returndateformat(.Rows(i).Cells(3).Value) & "', status = " & .Rows(i).Cells(4).Value & " WHERE category_id = " & .Rows(i).Cells(0).Value
                        cmdlocal = New MySqlCommand(sqlupdate, LocalhostConn())
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
                    Dim sql = "SELECT category_id FROM loc_admin_category WHERE category_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
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
                    Dim sql = "SELECT category_id FROM loc_admin_category WHERE category_id = " & .Rows(i).Cells(0).Value
                    cmdlocal = New MySqlCommand(sql, LocalhostConn())
                    Dim result As Integer = cmdlocal.ExecuteScalar
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
End Class