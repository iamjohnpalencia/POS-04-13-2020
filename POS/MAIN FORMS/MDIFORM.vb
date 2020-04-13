Public Class MDIFORM
    Private m_ChildFormNumber As Integer
    Private Sub MDIFORM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim newMDIchild As New Leaderboards()
            If Application.OpenForms().OfType(Of Leaderboards).Any Then
                Leaderboards.TopMost = True
            Else
                btncolor(changecolor:=Button2)
                btndefaut(defaultcolor:=Button2)
                formclose(closeform:=Leaderboards)
                newMDIchild.MdiParent = Me
                newMDIchild.ShowIcon = False
                newMDIchild.Show()
            End If
            LabelTotalProdLine.Text = count(table:="loc_admin_products WHERE store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'", tocount:="product_id")
            LabelTotalProdLine.Text = Val(LabelTotalProdLine.Text) + count(table:="loc_admin_products WHERE product_category <> 'Others'", tocount:="product_id")
            LabelTotalAvailStock.Text = sum(table:="loc_pos_inventory WHERE store_id = " & ClientStoreID & " AND guid = '" & ClientGuid & "'", tototal:="stock_quantity")
            LabelTotalSales.Text = sum(table:="loc_daily_transaction_details WHERE zreading = '" & returndateformat(Now) & "' AND active = 1 AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' ", tototal:="total")
            LabelTotalCrititems.Text = count(table:="loc_pos_inventory WHERE stock_status = 1 AND critical_limit >= stock_quantity AND store_id ='" & ClientStoreID & "' AND guid = '" & ClientGuid & "'", tocount:="inventory_id")
            If ClientRole = "Crew" Then
                Button8.Visible = False
                Button6.Visible = False
                Button5.Visible = False
                Button2.Location = New Point(20, 276)
                Button3.Location = New Point(20, 320)
                Button10.Location = New Point(20, 364)
                Button1.Location = New Point(20, 408)
                Button4.Location = New Point(20, 452)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim newMDIchild As New ManageProducts()
        If Application.OpenForms().OfType(Of ManageProducts).Any Then
        Else
            btndefaut(defaultcolor:=Button5)
            btncolor(changecolor:=Button5)
            formclose(closeform:=ManageProducts)
            newMDIchild.MdiParent = Me
            newMDIchild.ShowIcon = False
            newMDIchild.Show()
        End If
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim newMDIchild As New Inventory()
        If Application.OpenForms().OfType(Of Inventory).Any Then
        Else
            btncolor(changecolor:=Button6)
            btndefaut(defaultcolor:=Button6)
            formclose(closeform:=Inventory)
            newMDIchild.MdiParent = Me
            newMDIchild.ShowIcon = False
            newMDIchild.Show()
        End If
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim newMDIchild As New Reports()
        If Application.OpenForms().OfType(Of Reports).Any Then
        Else
            btncolor(changecolor:=Button3)
            btndefaut(defaultcolor:=Button3)
            formclose(closeform:=Reports)
            newMDIchild.MdiParent = Me
            newMDIchild.ShowIcon = False
            newMDIchild.Show()
        End If
    End Sub
    Private Sub Button8_Click_1(sender As Object, e As EventArgs) Handles Button8.Click
        Dim newMDIchild As New UserSettings()
        If Application.OpenForms().OfType(Of UserSettings).Any Then
        Else
            btncolor(changecolor:=Button8)
            btndefaut(defaultcolor:=Button8)
            formclose(closeform:=UserSettings)
            newMDIchild.MdiParent = Me
            newMDIchild.ShowIcon = False
            newMDIchild.Show()
        End If
    End Sub
    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        btncolor(changecolor:=Button10)
        btndefaut(defaultcolor:=Button10)
        formclose(closeform:=SynctoCloud)
        SynctoCloud.Show()
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim newMDIchild As New Leaderboards()
        If Application.OpenForms().OfType(Of Leaderboards).Any Then
            Leaderboards.TopMost = True
        Else
            btncolor(changecolor:=Button2)
            btndefaut(defaultcolor:=Button2)
            formclose(closeform:=Leaderboards)
            newMDIchild.MdiParent = Me
            newMDIchild.ShowIcon = False
            newMDIchild.Show()
        End If
    End Sub
    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        Dim newMDIchild As New About()
        If Application.OpenForms().OfType(Of About).Any Then
        Else
            btncolor(changecolor:=Button4)
            btndefaut(defaultcolor:=Button4)
            formclose(closeform:=About)
            newMDIchild.MdiParent = Me
            newMDIchild.ShowIcon = False
            newMDIchild.Show()
        End If
    End Sub
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Dim newMDIchild As New ImportandExport()
        If Application.OpenForms().OfType(Of ImportandExport).Any Then
        Else
            btncolor(changecolor:=Button11)
            btndefaut(defaultcolor:=Button11)
            formclose(closeform:=ImportandExport)
            newMDIchild.MdiParent = Me
            newMDIchild.ShowIcon = False
            newMDIchild.Show()
        End If
    End Sub
    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Dim newMDIchild As New DepositSlip()
        If Application.OpenForms().OfType(Of DepositSlip).Any Then
        Else
            btncolor(changecolor:=Button12)
            btndefaut(defaultcolor:=Button12)
            formclose(closeform:=DepositSlip)
            newMDIchild.MdiParent = Me
            newMDIchild.ShowIcon = False
            newMDIchild.Show()
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            iflogout = False
            Me.Close()
        Catch ex As Exception
            Dispose()
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub btncolor(ByVal changecolor As Button)
        changecolor.ForeColor = Color.White
        changecolor.BackColor = Color.FromArgb(23, 162, 184)
    End Sub
    Public Sub btndefaut(ByVal defaultcolor As Button)
        For Each P As Control In Controls
            If TypeOf P Is Panel Then
                For Each ctrl As Control In P.Controls
                    If TypeOf ctrl Is Button Then
                        If ctrl.Name <> defaultcolor.Name Then
                            CType(ctrl, Button).ForeColor = Color.Black
                            CType(ctrl, Button).BackColor = Color.White
                        End If
                    End If
                Next
            End If
        Next
    End Sub
    Public Sub formclose(ByVal closeform As Form)
        Try
            For Each P As Control In Controls
                For Each ctrl As Control In P.Controls
                    If TypeOf ctrl Is Form Then
                        If ctrl.Name <> closeform.Name Then
                            CType(ctrl, Form).FindForm.Hide()
                        End If
                    End If
                Next
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim iflogout As Boolean
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        If SyncIsOnProcess = True Then
            MessageBox.Show("Sync is on process please wait.", "Syncing", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            If MessageBox.Show("Are you sure you really want to Logout ?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
                FormIsOpen()
                EndBalance()
                iflogout = True
                Login.Show()
                Close()
                POS.Close()
            End If
        End If
    End Sub
    Private Sub MDIFORM_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If iflogout = False Then
            POS.Enabled = True
            POS.BringToFront()
        End If
    End Sub
End Class