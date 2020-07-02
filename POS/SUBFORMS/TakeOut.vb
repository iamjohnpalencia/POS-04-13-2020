Imports MySql.Data.MySqlClient
Public Class TakeOut
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
End Class