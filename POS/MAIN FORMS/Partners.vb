Imports MySql.Data.MySqlClient

Public Class Partners
    Dim AddOrEdit As Boolean
    Private Sub Partners_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TabControl1.TabPages(0).Text = "Bank List"
        LoadPartners()
    End Sub
    Private Sub LoadPartners()
        GLOBAL_SELECT_ALL_FUNCTION("loc_partners_transaction ORDER BY arrid ASC", "*", DataGridViewPartners)
        With DataGridViewPartners
            .Columns(0).Visible = False
            .Columns(1).Visible = False
            .Columns(2).HeaderText = "Bank Name"
            .Columns(3).HeaderText = "Date Modified"
            .Columns(4).HeaderText = "Service Crew ID"
            .Columns(5).Visible = False
            .Columns(6).Visible = False
            .Columns(7).HeaderText = "Status"
            .Columns(8).Visible = False
        End With
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        AddOrEdit = True
        PanelAddBank.Top = (Me.Height - PanelAddBank.Height) / 4
        PanelAddBank.Left = (Me.Width - PanelAddBank.Width) / 3
        PanelAddBank.Visible = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        PanelAddBank.Visible = False
        ClearTextBox(PanelAddBank)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            table = "loc_partners_transaction"
            If AddOrEdit = True Then

                Dim arrid = count("arrid", table)
                fields = "(`arrid`, `bankname`,  `active`, `crew_id`, `synced`, `store_id`, `guid`)"
                value = "(" & arrid + 1 & ", '" & TextBoxBankName.Text & "', " & 1 & ", '" & ClientCrewID & "', 'Unsynced', '" & ClientStoreID & "', '" & ClientGuid & "')"
                GLOBAL_INSERT_FUNCTION(table, fields, value)
                LoadPartners()
            Else
                fields = "bankname = '" & TextBoxBankName.Text & "'"
                where = "id = " & DataGridViewPartners.SelectedRows(0).Cells(0).Value & ""
                GLOBAL_FUNCTION_UPDATE(table, fields, where)
                LoadPartners()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If DataGridViewPartners.SelectedRows.Count > 1 Then
            MsgBox("SELECT ONE ONLY")
        Else
            AddOrEdit = False
            PanelAddBank.Top = (Me.Height - PanelAddBank.Height) / 4
            PanelAddBank.Left = (Me.Width - PanelAddBank.Width) / 3
            PanelAddBank.Visible = True
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Dim ID = GLOBAL_RETURN_FUNCTION("loc_partners_transaction WHERE arrid = 1", "bankname", "bankname", True)
            Dim Arrid = GLOBAL_RETURN_FUNCTION("loc_partners_transaction WHERE id = " & DataGridViewPartners.SelectedRows(0).Cells(0).Value, "bankname", "bankname", True)
            table = "loc_partners_transaction"
            fields = "arrid = 1, synced = 'Unsynced'"
            where = "bankname = '" & Arrid & "'"
            GLOBAL_FUNCTION_UPDATE(table, fields, where)
            fields = "arrid = " & DataGridViewPartners.SelectedRows(0).Cells(1).Value & ", synced = 'Unsynced'"
            where = "bankname = '" & ID & "'"
            GLOBAL_FUNCTION_UPDATE(table, fields, where)
            LoadPartners()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub ButtonDeleteProducts_Click(sender As Object, e As EventArgs) Handles ButtonDeleteProducts.Click

    End Sub
End Class