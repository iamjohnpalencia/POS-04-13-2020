Imports MySql.Data.MySqlClient
Public Class TakeOut
    Dim BAGORSYRUP As Boolean = False
    Private Sub TakeOut_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        POS.Enabled = True
    End Sub
    Private Sub DInventory()
        Try
            Dim Sql
            Dim Cmd As MySqlCommand
            Dim Da As MySqlDataAdapter
            Dim Dt As DataTable

            Dim EXTRAFPrimary As Double = 0
            Dim EXTRAFSecondary As Double = 0
            Dim EXTRAFNServing As Double = 0

            Dim IPrimary As Double = 0
            Dim ISecondary As Double = 0
            Dim INServing As Double = 0

            Dim SPrimary As Double = 0
            Dim SSecondary As Double = 0
            Dim SNServing As Double = 0

            If CheckBoxWaffleBag.Checked = True And CheckBoxSugarSyrup.Checked = True Then
                Dim message = MessageBox.Show("Do you want to add Extra packaging and Sugar Syrup?", "Extra packaging and Syrup", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                If message = DialogResult.Yes Then
                    If Double.Parse(TextBoxWaffleBag.Text) <> 0 And Double.Parse(TextBoxSugarSyrup.Text) <> 0 Then
                        Try
                            Sql = "SELECT primary_value ,secondary_value, no_servings FROM loc_product_formula WHERE product_ingredients = 'Extra Packaging'"
                            Cmd = New MySqlCommand(Sql, LocalhostConn)
                            Da = New MySqlDataAdapter(Cmd)
                            Dt = New DataTable
                            Da.Fill(Dt)

                            For Each row As DataRow In Dt.Rows
                                EXTRAFPrimary = row("primary_value")
                                EXTRAFSecondary = row("secondary_value")
                                EXTRAFNServing = row("no_servings")
                            Next

                            Dim TotalExtraP As Double = EXTRAFPrimary * Double.Parse(TextBoxWaffleBag.Text)
                            Dim TotalExtraS As Double = EXTRAFSecondary * Double.Parse(TextBoxWaffleBag.Text)
                            Dim TotalExtraN As Double = EXTRAFNServing * Double.Parse(TextBoxWaffleBag.Text)

                            Sql = "SELECT stock_primary ,stock_secondary, stock_no_of_servings FROM loc_pos_inventory WHERE product_ingredients = 'Extra Packaging'"
                            Cmd = New MySqlCommand(Sql, LocalhostConn)
                            Da = New MySqlDataAdapter(Cmd)
                            Dt = New DataTable
                            Da.Fill(Dt)

                            For Each row As DataRow In Dt.Rows
                                IPrimary = row("stock_primary")
                                ISecondary = row("stock_secondary")
                                INServing = row("stock_no_of_servings")
                            Next

                            SPrimary = IPrimary - TotalExtraP
                            SSecondary = ISecondary - TotalExtraS
                            SNServing = INServing - TotalExtraN

                            GLOBAL_FUNCTION_UPDATE("loc_pos_inventory", "stock_primary=" & SPrimary & ",stock_secondary=" & SSecondary & ",stock_no_of_servings=" & SNServing & "", "product_ingredients = 'Extra Packaging'")

                        Catch ex As Exception
                            MsgBox(ex.ToString)
                        End Try
                        '=====================================================================================================
                        Try
                            Sql = "SELECT primary_value ,secondary_value, no_servings FROM loc_product_formula WHERE formula_id = 53"
                            Cmd = New MySqlCommand(Sql, LocalhostConn)
                            Da = New MySqlDataAdapter(Cmd)
                            Dt = New DataTable
                            Da.Fill(Dt)

                            For Each row As DataRow In Dt.Rows
                                EXTRAFPrimary = row("primary_value")
                                EXTRAFSecondary = row("secondary_value")
                                EXTRAFNServing = row("no_servings")
                            Next

                            Dim TotalExtraP As Double = EXTRAFPrimary * Double.Parse(TextBoxSugarSyrup.Text)
                            Dim TotalExtraS As Double = EXTRAFSecondary * Double.Parse(TextBoxSugarSyrup.Text)
                            Dim TotalExtraN As Double = EXTRAFNServing * Double.Parse(TextBoxSugarSyrup.Text)

                            Sql = "SELECT stock_primary ,stock_secondary, stock_no_of_servings FROM loc_pos_inventory WHERE inventory_id = 53"
                            Cmd = New MySqlCommand(Sql, LocalhostConn)
                            Da = New MySqlDataAdapter(Cmd)
                            Dt = New DataTable
                            Da.Fill(Dt)

                            For Each row As DataRow In Dt.Rows
                                IPrimary = row("stock_primary")
                                ISecondary = row("stock_secondary")
                                INServing = row("stock_no_of_servings")
                            Next

                            SPrimary = IPrimary - TotalExtraP
                            SSecondary = ISecondary - TotalExtraS
                            SNServing = INServing - TotalExtraN

                            GLOBAL_FUNCTION_UPDATE("loc_pos_inventory", "stock_primary=" & SPrimary & ",stock_secondary=" & SSecondary & ",stock_no_of_servings=" & SNServing & "", "inventory_id = 53")

                        Catch ex As Exception
                            MsgBox(ex.ToString)
                        End Try
                    End If
                End If
            ElseIf CheckBoxSugarSyrup.Checked = True Then
                Dim message = MessageBox.Show("Do you want to add Extra Sugar Syrup?", "Extra Sugar", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                If message = DialogResult.Yes Then
                    Try
                        Sql = "SELECT primary_value ,secondary_value, no_servings FROM loc_product_formula WHERE formula_id = 53"
                        Cmd = New MySqlCommand(Sql, LocalhostConn)
                        Da = New MySqlDataAdapter(Cmd)
                        Dt = New DataTable
                        Da.Fill(Dt)

                        For Each row As DataRow In Dt.Rows
                            EXTRAFPrimary = row("primary_value")
                            EXTRAFSecondary = row("secondary_value")
                            EXTRAFNServing = row("no_servings")
                        Next

                        Dim TotalExtraP As Double = EXTRAFPrimary * Double.Parse(TextBoxSugarSyrup.Text)
                        Dim TotalExtraS As Double = EXTRAFSecondary * Double.Parse(TextBoxSugarSyrup.Text)
                        Dim TotalExtraN As Double = EXTRAFNServing * Double.Parse(TextBoxSugarSyrup.Text)

                        Sql = "SELECT stock_primary ,stock_secondary, stock_no_of_servings FROM loc_pos_inventory WHERE inventory_id = 53"
                        Cmd = New MySqlCommand(Sql, LocalhostConn)
                        Da = New MySqlDataAdapter(Cmd)
                        Dt = New DataTable
                        Da.Fill(Dt)

                        For Each row As DataRow In Dt.Rows
                            IPrimary = row("stock_primary")
                            ISecondary = row("stock_secondary")
                            INServing = row("stock_no_of_servings")
                        Next

                        SPrimary = IPrimary - TotalExtraP
                        SSecondary = ISecondary - TotalExtraS
                        SNServing = INServing - TotalExtraN


                        GLOBAL_FUNCTION_UPDATE("loc_pos_inventory", "stock_primary=" & SPrimary & ",stock_secondary=" & SSecondary & ",stock_no_of_servings=" & SNServing & "", "inventory_id = 53")

                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try
                End If
            ElseIf CheckBoxWaffleBag.Checked = True Then
                Dim message = MessageBox.Show("Do you want to add Extra packaging?", "Extra packaging", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                If message = DialogResult.Yes Then
                    Try
                        Sql = "SELECT primary_value ,secondary_value, no_servings FROM loc_product_formula WHERE product_ingredients = 'Extra Packaging'"
                        Cmd = New MySqlCommand(Sql, LocalhostConn)
                        Da = New MySqlDataAdapter(Cmd)
                        Dt = New DataTable
                        Da.Fill(Dt)

                        For Each row As DataRow In Dt.Rows
                            EXTRAFPrimary = row("primary_value")
                            EXTRAFSecondary = row("secondary_value")
                            EXTRAFNServing = row("no_servings")
                        Next

                        Dim TotalExtraP As Double = EXTRAFPrimary * Double.Parse(TextBoxWaffleBag.Text)
                        Dim TotalExtraS As Double = EXTRAFSecondary * Double.Parse(TextBoxWaffleBag.Text)
                        Dim TotalExtraN As Double = EXTRAFNServing * Double.Parse(TextBoxWaffleBag.Text)

                        Sql = "SELECT stock_primary ,stock_secondary, stock_no_of_servings FROM loc_pos_inventory WHERE product_ingredients = 'Extra Packaging'"
                        Cmd = New MySqlCommand(Sql, LocalhostConn)
                        Da = New MySqlDataAdapter(Cmd)
                        Dt = New DataTable
                        Da.Fill(Dt)

                        For Each row As DataRow In Dt.Rows
                            IPrimary = row("stock_primary")
                            ISecondary = row("stock_secondary")
                            INServing = row("stock_no_of_servings")
                        Next

                        SPrimary = IPrimary - TotalExtraP
                        SSecondary = ISecondary - TotalExtraS
                        SNServing = INServing - TotalExtraN

                        GLOBAL_FUNCTION_UPDATE("loc_pos_inventory", "stock_primary=" & SPrimary & ",stock_secondary=" & SSecondary & ",stock_no_of_servings=" & SNServing & "", "product_ingredients = 'Extra Packaging'")

                    Catch ex As Exception
                        MsgBox(ex.ToString)
                    End Try
                End If
            Else
                MsgBox("Select extra packaging/sugar syrup first")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub ButtonTakeOut_Click(sender As Object, e As EventArgs) Handles ButtonTakeOut.Click
        DInventory()
    End Sub

    Private Sub buttonpressedenter(ByVal btntext As String)
        Try
            If BAGORSYRUP = False Then
                TextBoxWaffleBag.Select()
                If Val(TextBoxWaffleBag.Text) <> 0 Then
                    TextBoxWaffleBag.Text += btntext
                Else
                    TextBoxWaffleBag.Text = btntext
                End If
            Else
                TextBoxSugarSyrup.Select()
                If Val(TextBoxSugarSyrup.Text) <> 0 Then
                    TextBoxSugarSyrup.Text += btntext
                Else
                    TextBoxSugarSyrup.Text = btntext
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub ButtonNo9_Click(sender As Object, e As EventArgs) Handles ButtonNo9.Click
        If TextBoxWaffleBag.Focus = True Then
            If TextBoxWaffleBag.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo9.Text)
            End If
        ElseIf TextBoxSugarSyrup.Focus = True Then
            If TextBoxSugarSyrup.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo9.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo8_Click(sender As Object, e As EventArgs) Handles ButtonNo8.Click
        If TextBoxWaffleBag.Focus = True Then
            If TextBoxWaffleBag.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo8.Text)
            End If
        ElseIf TextBoxSugarSyrup.Focus = True Then
            If TextBoxSugarSyrup.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo8.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo7_Click(sender As Object, e As EventArgs) Handles ButtonNo7.Click
        If TextBoxWaffleBag.Focus = True Then
            If TextBoxWaffleBag.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo7.Text)
            End If
        ElseIf TextBoxSugarSyrup.Focus = True Then
            If TextBoxSugarSyrup.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo7.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo6_Click(sender As Object, e As EventArgs) Handles ButtonNo6.Click
        If TextBoxWaffleBag.Focus = True Then
            If TextBoxWaffleBag.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo6.Text)
            End If
        ElseIf TextBoxSugarSyrup.Focus = True Then
            If TextBoxSugarSyrup.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo6.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo5_Click(sender As Object, e As EventArgs) Handles ButtonNo5.Click
        If TextBoxWaffleBag.Focus = True Then
            If TextBoxWaffleBag.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo5.Text)
            End If
        ElseIf TextBoxSugarSyrup.Focus = True Then
            If TextBoxSugarSyrup.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo5.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo4_Click(sender As Object, e As EventArgs) Handles ButtonNo4.Click
        If TextBoxWaffleBag.Focus = True Then
            If TextBoxWaffleBag.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo4.Text)
            End If
        ElseIf TextBoxSugarSyrup.Focus = True Then
            If TextBoxSugarSyrup.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo4.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo3_Click(sender As Object, e As EventArgs) Handles ButtonNo3.Click
        If TextBoxWaffleBag.Focus = True Then
            If TextBoxWaffleBag.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo3.Text)
            End If
        ElseIf TextBoxSugarSyrup.Focus = True Then
            If TextBoxSugarSyrup.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo3.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo2_Click(sender As Object, e As EventArgs) Handles ButtonNo2.Click
        If TextBoxWaffleBag.Focus = True Then
            If TextBoxWaffleBag.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo2.Text)
            End If
        ElseIf TextBoxSugarSyrup.Focus = True Then
            If TextBoxSugarSyrup.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo2.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo1_Click(sender As Object, e As EventArgs) Handles ButtonNo1.Click
        If TextBoxWaffleBag.Focus = True Then
            If TextBoxWaffleBag.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo1.Text)
            End If
        ElseIf TextBoxSugarSyrup.Focus = True Then
            If TextBoxSugarSyrup.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo1.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo0_Click(sender As Object, e As EventArgs) Handles ButtonNo0.Click
        If TextBoxWaffleBag.Focus = True Then
            If TextBoxWaffleBag.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo0.Text)
            End If
        ElseIf TextBoxSugarSyrup.Focus = True Then
            If TextBoxSugarSyrup.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo0.Text)
            End If
        End If
    End Sub
    Private Sub ButtonNo00_Click(sender As Object, e As EventArgs) Handles ButtonNo00.Click
        If TextBoxWaffleBag.Focus = True Then
            If TextBoxWaffleBag.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo00.Text)
            End If
        ElseIf TextBoxSugarSyrup.Focus = True Then
            If TextBoxSugarSyrup.Text.Length > 6 Then
            Else
                buttonpressedenter(btntext:=ButtonNo00.Text)
            End If
        End If
    End Sub

    Private Sub TakeOut_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TextBoxWaffleBag_Click(sender As Object, e As EventArgs) Handles TextBoxWaffleBag.Click
        BAGORSYRUP = False
    End Sub

    Private Sub TextBoxSugarSyrup_Click(sender As Object, e As EventArgs) Handles TextBoxSugarSyrup.Click
        BAGORSYRUP = True
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If BAGORSYRUP = False Then
            TextBoxWaffleBag.Text = 0
        Else
            TextBoxSugarSyrup.Text = 0
        End If
    End Sub
End Class