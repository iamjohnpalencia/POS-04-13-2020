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
                Else
                    Dim message As Integer = MessageBox.Show("Cash drawer has zero balance. Do you want to proceed ?", "Zero Balance", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    If message = DialogResult.Yes Then
                        InsertBeginningBalance()
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

        'If INSERTZREADINVENTORY = True Then
        '    XZreadingInventory(S_Zreading)
        'End If
        Me.Close()
    End Sub
    Private Sub BegBalance_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        POS.Enabled = True
    End Sub
    Dim INSERTZREADINVENTORY As Boolean = False
    Private Sub BegBalance_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        Try
            Dim SQL = "SELECT * FROM `loc_zread_inventory` WHERE zreading = '" & S_Zreading & "'"
            Dim Cmd As MySqlCommand = New MySqlCommand(SQL, LocalhostConn)
            Dim Da As MySqlDataAdapter = New MySqlDataAdapter(Cmd)
            Dim Dt As DataTable = New DataTable
            Da.Fill(Dt)
            If Dt.Rows.Count = 0 Then
                'FillZreadInv()
                INSERTZREADINVENTORY = True
            Else
                INSERTZREADINVENTORY = False
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    'Private Sub FillZreadInv()
    '    Try
    '        GLOBAL_SELECT_ALL_FUNCTION("loc_pos_inventory", "*", DataGridViewZreadInventory)
    '    Catch ex As Exception
    '        MsgBox(ex.ToString)
    '    End Try
    'End Sub
    'Private Sub XZreadingInventory(zreaddate)
    '    Try
    '        Dim Fields As String = "`inventory_id`, `store_id`, `formula_id`, `product_ingredients`, `sku`, `stock_primary`, `stock_secondary`, `stock_no_of_servings`, `stock_status`, `critical_limit`, `guid`, `created_at`, `crew_id`, `synced`, `server_date_modified`, `server_inventory_id`, `zreading`"
    '        Dim cmd As MySqlCommand
    '        With DataGridViewZreadInventory
    '            For i As Integer = 0 To .Rows.Count - 1 Step +1
    '                cmd = New MySqlCommand("INSERT INTO loc_zread_inventory (" & Fields & ") VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17)", LocalhostConn)
    '                cmd.Parameters.Add("@1", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString
    '                cmd.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString
    '                cmd.Parameters.Add("@3", MySqlDbType.Int64).Value = .Rows(i).Cells(2).Value.ToString
    '                cmd.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString
    '                cmd.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString
    '                cmd.Parameters.Add("@6", MySqlDbType.Double).Value = .Rows(i).Cells(5).Value.ToString
    '                cmd.Parameters.Add("@7", MySqlDbType.Double).Value = .Rows(i).Cells(6).Value.ToString
    '                cmd.Parameters.Add("@8", MySqlDbType.Double).Value = .Rows(i).Cells(7).Value.ToString
    '                cmd.Parameters.Add("@9", MySqlDbType.Int64).Value = .Rows(i).Cells(8).Value.ToString
    '                cmd.Parameters.Add("@10", MySqlDbType.Int64).Value = .Rows(i).Cells(9).Value.ToString
    '                cmd.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString
    '                cmd.Parameters.Add("@12", MySqlDbType.Text).Value = .Rows(i).Cells(11).Value.ToString
    '                cmd.Parameters.Add("@13", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString
    '                cmd.Parameters.Add("@14", MySqlDbType.VarChar).Value = .Rows(i).Cells(13).Value.ToString
    '                cmd.Parameters.Add("@15", MySqlDbType.Text).Value = .Rows(i).Cells(14).Value.ToString
    '                cmd.Parameters.Add("@16", MySqlDbType.Int64).Value = .Rows(i).Cells(15).Value.ToString
    '                cmd.Parameters.Add("@17", MySqlDbType.Text).Value = S_Zreading
    '                cmd.ExecuteNonQuery()
    '            Next
    '        End With
    '    Catch ex As Exception
    '        MsgBox(ex.ToString)
    '    End Try
    'End Sub

    'Private Sub Button2_Click(sender As Object, e As EventArgs)
    '    XZreadingInventory(S_Zreading)
    'End Sub
End Class
