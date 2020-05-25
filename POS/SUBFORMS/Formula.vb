Imports MySql.Data.MySqlClient
Public Class Formula
    Private Sub Formula_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loadformula()
    End Sub
    Public Sub loadformula()
        fields = "`product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`"
        GLOBAL_SELECT_ALL_FUNCTION(table:="loc_product_formula WHERE status = 1 AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' ", datagrid:=DataGridViewProductFormula, fields:=fields)
        With DataGridViewProductFormula
            .Columns(0).HeaderText = "Ingredients"
            .Columns(1).HeaderText = "Primary Unit"
            .Columns(2).HeaderText = "Primary (Value)"
            .Columns(3).HeaderText = "Secondary Unit"
            .Columns(4).HeaderText = "Secondary (Value)"
            .Columns(5).HeaderText = "Srv. Unit"
            .Columns(6).HeaderText = "Srv. (Value)"
            .Columns(7).HeaderText = "No. of Serving"
            .Columns(0).Width = 150
            .Columns(2).Width = 70
        End With
    End Sub
    Private Sub ButtonExit_Click(sender As Object, e As EventArgs) Handles ButtonExit.Click
        MDIFORM.Button2.PerformClick()
    End Sub
End Class