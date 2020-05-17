Imports MySql.Data.MySqlClient
Public Class Promo
    Private Sub Promo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loaddatagrid1()
        loaddatagrid2()
    End Sub
    Private Sub loaddatagrid1()
        DataGridView1.Columns.Clear()

        Dim mysqlcon As New MySqlConnection(localconn.ConnectionString)
        Dim dt As New DataTable
        Dim sda As New MySqlDataAdapter
        Dim bsource As New BindingSource
        Try
            cmd = New MySqlCommand("SELECT * FROM loc_admin_products", LocalhostConn)
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            DataGridView1.DataSource = dt
            'DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            'DataGridView1.AutoGenerateColumns = False

            'Dim index As Integer
            'index = DataGridView1.Columns.Add("ID", "ID")
            'DataGridView1.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            'DataGridView1.Columns(0).Visible = True
            'DataGridView1.Columns(index).DataPropertyName = "ProductID_"

            'index = DataGridView1.Columns.Add("Product", "Product")
            'DataGridView1.Columns(1).Visible = True
            'DataGridView1.Columns(index).DataPropertyName = "Productname_"

            'index = DataGridView1.Columns.Add("Category", "Category")
            'DataGridView1.Columns(2).Visible = True
            'DataGridView1.Columns(index).DataPropertyName = "Category_"

            'index = DataGridView1.Columns.Add("Price", "Price")
            'DataGridView1.Columns(3).Visible = True
            'DataGridView1.Columns(index).DataPropertyName = "Price_"

            'da.SelectCommand = cmd
            'da.Fill(dt)
            'bsource.DataSource = dt
            'DataGridView1.DataSource = bsource
            'da.Update(dt)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
    Private Sub loaddatagrid2()
        DataGridView2.Columns.Clear()

        Dim mysqlcon As New MySqlConnection(localconn.ConnectionString)
        Dim dt As New DataTable
        Dim sda As New MySqlDataAdapter
        Dim bsource As New BindingSource
        Try
            cmd = New MySqlCommand("SELECT * FROM tbcoupon", LocalhostConn())
            DataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
            DataGridView2.AutoGenerateColumns = False
            Dim index As Integer
            index = DataGridView2.Columns.Add("ID", "ID")
            DataGridView2.Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            DataGridView2.Columns(0).Visible = True
            DataGridView2.Columns(index).DataPropertyName = "ID"

            index = DataGridView2.Columns.Add("Coupon", "Coupon")
            DataGridView2.Columns(1).Visible = True
            DataGridView2.Columns(index).DataPropertyName = "Couponname_"

            index = DataGridView2.Columns.Add("Description", "Description")
            DataGridView2.Columns(2).Visible = True
            DataGridView2.Columns(index).DataPropertyName = "Desc_"

            index = DataGridView2.Columns.Add("Discount", "Discount")
            DataGridView2.Columns(3).Visible = True
            DataGridView2.Columns(index).DataPropertyName = "Discountvalue_"

            index = DataGridView2.Columns.Add("Minimum", "Minimum")
            DataGridView2.Columns(4).Visible = True
            DataGridView2.Columns(index).DataPropertyName = "Referencevalue_"

            index = DataGridView2.Columns.Add("Coupon Type", "Coupon Type")
            DataGridView2.Columns(5).Visible = True
            DataGridView2.Columns(index).DataPropertyName = "Type"

            index = DataGridView2.Columns.Add("Bundle Reference", "Bundle Reference")
            DataGridView2.Columns(6).Visible = True
            DataGridView2.Columns(index).DataPropertyName = "Bundlebase_"

            index = DataGridView2.Columns.Add("Bundle Value", "Bundle Value")
            DataGridView2.Columns(7).Visible = True
            DataGridView2.Columns(index).DataPropertyName = "BBValue_"

            index = DataGridView2.Columns.Add("Bundle Promo Item", "Bundle Promo Item")
            DataGridView2.Columns(8).Visible = True
            DataGridView2.Columns(index).DataPropertyName = "Bundlepromo_"

            index = DataGridView2.Columns.Add("Bundle Promo Qty", "Bundle Promo Qty")
            DataGridView2.Columns(9).Visible = True
            DataGridView2.Columns(index).DataPropertyName = "BPValue_"

            index = DataGridView2.Columns.Add("Effective", "Effective")
            DataGridView2.Columns(10).Visible = True
            DataGridView2.Columns(index).DataPropertyName = "Effectivedate"

            index = DataGridView2.Columns.Add("Expiry", "Expiry")
            DataGridView2.Columns(11).Visible = True
            DataGridView2.Columns(index).DataPropertyName = "Expirydate"

            sda.SelectCommand = cmd
            sda.Fill(dt)
            bsource.DataSource = dt
            DataGridView2.DataSource = bsource
            sda.Update(dt)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If TextBox5.Enabled = False Then
            Exit Sub
        Else
            If TextBox5.Text = String.Empty Then
                TextBox5.Text = Me.DataGridView1.Item(0, Me.DataGridView1.CurrentRow.Index).Value
                '  Dim newString As String
                '   newString = deleteDup(TextBox5.Text, TextBox5.Text)

                '   TextBox5.Text = newString
            Else
                TextBox5.Text = TextBox5.Text & "," & Me.DataGridView1.Item(0, Me.DataGridView1.CurrentRow.Index).Value
                ' Dim newString As String
                '  newString = deleteDup(TextBox5.Text, TextBox5.Text)

                ' TextBox5.Text = newString
            End If
        End If

        If TextBox7.Enabled = False Then
            Exit Sub
        Else
            If TextBox7.Text = String.Empty Then
                TextBox7.Text = Me.DataGridView1.Item(0, Me.DataGridView1.CurrentRow.Index).Value
            Else
                TextBox7.Text = TextBox7.Text & "," & Me.DataGridView1.Item(0, Me.DataGridView1.CurrentRow.Index).Value
            End If
        End If




    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        save()
    End Sub
    Private Sub save()

        'mysqlcon = New MySqlConnection
        'mysqlcon.ConnectionString = cs2

        If ComboBox1.Text = "Percentage" Then
            If TextBox1.Text = Trim(String.Empty) Then
                TextBox1.Focus()
                Exit Sub
            ElseIf TextBox2.Text = Trim(String.Empty) Then
                TextBox2.Focus()
                Exit Sub
            ElseIf TextBox3.Text = Trim(String.Empty) Then
                TextBox3.Focus()
                Exit Sub
            Else
                Dim command As New MySqlCommand
                command.Connection = LocalhostConn()
                command.CommandText = "INSERT INTO tbcoupon (Couponname_,Desc_,Discountvalue_,Type,Effectivedate,Expirydate) Values
                                    ('" & Trim(TextBox1.Text) & "',
                                     '" & Trim(TextBox2.Text) & "',
                                     '" & Trim(TextBox3.Text) & "',
                                     '" & Trim(ComboBox1.Text) & "',
                                     '" & CDate(DateTimePicker1.Text).ToShortDateString & "',
                                     '" & CDate(DateTimePicker2.Text).ToShortDateString & "')"

                command.ExecuteReader()
                loaddatagrid2()
            End If

        ElseIf ComboBox1.Text = "Fix-1" Then
            If TextBox1.Text = Trim(String.Empty) Then
                TextBox1.Focus()
                Exit Sub
            ElseIf TextBox2.Text = Trim(String.Empty) Then
                TextBox2.Focus()
                Exit Sub
            ElseIf TextBox3.Text = Trim(String.Empty) Then
                TextBox3.Focus()
                Exit Sub
            Else
                Dim command As New MySqlCommand
                command.Connection = LocalhostConn()
                command.CommandText = "INSERT INTO tbcoupon (Couponname_,Desc_,Discountvalue_,Type,Effectivedate,Expirydate) Values
                                    ('" & Trim(TextBox1.Text) & "',
                                     '" & Trim(TextBox2.Text) & "',
                                     '" & Trim(TextBox3.Text) & "',
                                     '" & Trim(ComboBox1.Text) & "',
                                     '" & CDate(DateTimePicker1.Text).ToShortDateString & "',
                                     '" & CDate(DateTimePicker2.Text).ToShortDateString & "')"

                command.ExecuteReader()
                loaddatagrid2()
            End If
        ElseIf ComboBox1.Text = "Fix-2" Then
            If TextBox1.Text = Trim(String.Empty) Then
                TextBox1.Focus()
                Exit Sub
            ElseIf TextBox2.Text = Trim(String.Empty) Then
                TextBox2.Focus()
                Exit Sub
            ElseIf TextBox3.Text = Trim(String.Empty) Then
                TextBox3.Focus()
                Exit Sub
            ElseIf TextBox4.Text = Trim(String.Empty) Then
                TextBox4.Focus()
                Exit Sub
            Else
                Dim command As New MySqlCommand
                command.Connection = LocalhostConn()
                command.CommandText = "INSERT INTO tbcoupon (Couponname_,Desc_,Discountvalue_,Referencevalue_,Type,Effectivedate,Expirydate) Values
                                    ('" & Trim(TextBox1.Text) & "',
                                     '" & Trim(TextBox2.Text) & "',
                                     '" & Trim(TextBox3.Text) & "',
                                     '" & Trim(TextBox4.Text) & "',
                                     '" & Trim(ComboBox1.Text) & "',
                                     '" & CDate(DateTimePicker1.Text).ToShortDateString & "',
                                     '" & CDate(DateTimePicker2.Text).ToShortDateString & "')"
                command.ExecuteReader()
                loaddatagrid2()
            End If
        ElseIf ComboBox1.Text = "Bundle-1(Fix)" Then
            If TextBox1.Text = Trim(String.Empty) Then
                TextBox1.Focus()
                Exit Sub
            ElseIf TextBox2.Text = Trim(String.Empty) Then
                TextBox2.Focus()
                Exit Sub
            ElseIf TextBox5.Text = Trim(String.Empty) Then
                TextBox5.Focus()
                Exit Sub
            ElseIf TextBox6.Text = Trim(String.Empty) Then
                TextBox6.Focus()
                Exit Sub
            ElseIf TextBox7.Text = Trim(String.Empty) Then
                TextBox7.Focus()
                Exit Sub
            ElseIf TextBox8.Text = Trim(String.Empty) Then
                TextBox8.Focus()
                Exit Sub
            Else
                Dim command As New MySqlCommand
                command.Connection = LocalhostConn()
                command.CommandText = "INSERT INTO tbcoupon (Couponname_,Desc_,Bundlebase_,BBValue_,Bundlepromo_,BPValue_,Type,Effectivedate,Expirydate) Values
                                    ('" & Trim(TextBox1.Text) & "',
                                     '" & Trim(TextBox2.Text) & "',
                                     '" & Trim(TextBox5.Text) & "',
                                     '" & Trim(TextBox6.Text) & "',
                                     '" & Trim(TextBox7.Text) & "',
                                     '" & Trim(TextBox8.Text) & "',
                                     '" & Trim(ComboBox1.Text) & "',
                                     '" & CDate(DateTimePicker1.Text).ToShortDateString & "',
                                     '" & CDate(DateTimePicker2.Text).ToShortDateString & "')"
                command.ExecuteReader()
                loaddatagrid2()
            End If
        ElseIf ComboBox1.Text = "Bundle-2(Fix)" Then
            If TextBox1.Text = Trim(String.Empty) Then
                TextBox1.Focus()
                Exit Sub
            ElseIf TextBox2.Text = Trim(String.Empty) Then
                TextBox2.Focus()
                Exit Sub
            ElseIf TextBox3.Text = Trim(String.Empty) Then
                TextBox3.Focus()
                Exit Sub
            ElseIf TextBox5.Text = Trim(String.Empty) Then
                TextBox5.Focus()
                Exit Sub
            ElseIf TextBox6.Text = Trim(String.Empty) Then
                TextBox6.Focus()
                Exit Sub
            ElseIf TextBox7.Text = Trim(String.Empty) Then
                TextBox7.Focus()
                Exit Sub
            ElseIf TextBox8.Text = Trim(String.Empty) Then
                TextBox8.Focus()
                Exit Sub
            Else
                Dim command As New MySqlCommand
                command.Connection = LocalhostConn()
                command.CommandText = "INSERT INTO tbcoupon (Couponname_,Desc_,Discountvalue_,Bundlebase_,BBValue_,Bundlepromo_,BPValue_,Type,Effectivedate,Expirydate) Values
                                    ('" & Trim(TextBox1.Text) & "',
                                     '" & Trim(TextBox2.Text) & "',
                                    '" & Trim(TextBox3.Text) & "',
                                     '" & Trim(TextBox5.Text) & "',
                                     '" & Trim(TextBox6.Text) & "',
                                     '" & Trim(TextBox7.Text) & "',
                                     '" & Trim(TextBox8.Text) & "',
                                     '" & Trim(ComboBox1.Text) & "',
                                     '" & CDate(DateTimePicker1.Text).ToShortDateString & "',
                                     '" & CDate(DateTimePicker2.Text).ToShortDateString & "')"
                command.ExecuteReader()
                loaddatagrid2()
            End If
        ElseIf ComboBox1.Text = "Bundle-3(%)" Then
            If TextBox1.Text = Trim(String.Empty) Then
                TextBox1.Focus()
                Exit Sub
            ElseIf TextBox2.Text = Trim(String.Empty) Then
                TextBox2.Focus()
                Exit Sub
            ElseIf TextBox3.Text = Trim(String.Empty) Then
                TextBox3.Focus()
                Exit Sub
            ElseIf TextBox5.Text = Trim(String.Empty) Then
                TextBox5.Focus()
                Exit Sub
            ElseIf TextBox6.Text = Trim(String.Empty) Then
                TextBox6.Focus()
                Exit Sub
            ElseIf TextBox7.Text = Trim(String.Empty) Then
                TextBox7.Focus()
                Exit Sub
            ElseIf TextBox8.Text = Trim(String.Empty) Then
                TextBox8.Focus()
                Exit Sub
            Else
                Dim command As New MySqlCommand
                command.Connection = LocalhostConn()
                command.CommandText = "INSERT INTO tbcoupon (Couponname_,Desc_,Discountvalue_,Bundlebase_,BBValue_,Bundlepromo_,BPValue_,Type,Effectivedate,Expirydate) Values
                                    ('" & Trim(TextBox1.Text) & "',
                                     '" & Trim(TextBox2.Text) & "',
                                    '" & Trim(TextBox3.Text) & "',
                                     '" & Trim(TextBox5.Text) & "',
                                     '" & Trim(TextBox6.Text) & "',
                                     '" & Trim(TextBox7.Text) & "',
                                     '" & Trim(TextBox8.Text) & "',
                                     '" & Trim(ComboBox1.Text) & "',
                                     '" & CDate(DateTimePicker1.Text).ToShortDateString & "',
                                     '" & CDate(DateTimePicker2.Text).ToShortDateString & "')"
                command.ExecuteReader()
                loaddatagrid2()
            End If
        Else
            MsgBox("Please check details. Can't proceed due to missing information", vbInformation)
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For Each a In TabPage1.Controls
            If TypeOf a Is TextBox Then
                a.Text = Nothing
            End If
        Next
        ComboBox1.Text = ""
    End Sub

    Private Sub ComboBox1_TextChanged(sender As Object, e As EventArgs) Handles ComboBox1.TextChanged
        If ComboBox1.Text = "Percentage" Then
            TextBox3.Enabled = True
            TextBox4.Enabled = False
            TextBox5.Enabled = False
            TextBox6.Enabled = False
            TextBox7.Enabled = False
            TextBox8.Enabled = False
            For Each a In Panel1.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        ElseIf ComboBox1.Text = "Fix-1" Then
            TextBox3.Enabled = True
            TextBox4.Enabled = False
            TextBox5.Enabled = False
            TextBox6.Enabled = False
            TextBox7.Enabled = False
            TextBox8.Enabled = False
            For Each a In Panel1.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        ElseIf ComboBox1.Text = "Fix-2" Then
            TextBox3.Enabled = True
            TextBox4.Enabled = True
            TextBox5.Enabled = False
            TextBox6.Enabled = False
            TextBox7.Enabled = False
            TextBox8.Enabled = False
            For Each a In Panel1.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        ElseIf ComboBox1.Text = "Bundle-1(Fix)" Then
            TextBox3.Enabled = False
            TextBox4.Enabled = False
            TextBox5.Enabled = True
            TextBox6.Enabled = True
            TextBox7.Enabled = True
            TextBox8.Enabled = True
            For Each a In Panel1.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        ElseIf ComboBox1.Text = "Bundle-2(Fix)" Then
            TextBox3.Enabled = True
            TextBox4.Enabled = False
            TextBox5.Enabled = True
            TextBox6.Enabled = True
            TextBox7.Enabled = True
            TextBox8.Enabled = True
            For Each a In Panel1.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        ElseIf ComboBox1.Text = "Bundle-3(%)" Then
            TextBox3.Enabled = True
            TextBox4.Enabled = False
            TextBox5.Enabled = True
            TextBox6.Enabled = True
            TextBox7.Enabled = True
            TextBox8.Enabled = True
            For Each a In Panel1.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        Else
            TextBox3.Enabled = True
            TextBox4.Enabled = True
            TextBox5.Enabled = True
            TextBox6.Enabled = True
            TextBox7.Enabled = True
            TextBox8.Enabled = True
            For Each a In Panel1.Controls
                If TypeOf a Is TextBox Then
                    If a.Enabled = False Then
                        a.Text = "N/A"
                    ElseIf a.Enabled = True Then
                        a.Text = ""
                    End If
                End If
            Next
        End If
    End Sub
End Class