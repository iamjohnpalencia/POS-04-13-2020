﻿Public Class AddBank
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Try
            table = "loc_partners_transaction"
            If SettingsForm.AddOrEdit = True Then
                Dim arrid = count("arrid", table)
                fields = "(`arrid`, `bankname`,  `active`, `crew_id`, `synced`, `store_id`, `guid`)"
                value = "(" & arrid + 1 & ", '" & Trim(TextBoxBankName.Text) & "', " & 1 & ", '" & ClientCrewID & "', 'Unsynced', '" & ClientStoreID & "', '" & ClientGuid & "')"
                GLOBAL_INSERT_FUNCTION(table, fields, value)
                SettingsForm.LoadPartners()
            Else
                fields = "bankname = '" & Trim(TextBoxBankName.Text) & "'"
                where = "id = " & SettingsForm.DataGridViewPartners.SelectedRows(0).Cells(0).Value & ""
                GLOBAL_FUNCTION_UPDATE(table, fields, where)
                SettingsForm.LoadPartners()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub AddBank_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        SettingsForm.Enabled = True
    End Sub
End Class