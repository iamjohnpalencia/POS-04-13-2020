Imports MySql.Data.MySqlClient
Public Class UserSettings
    'Dim gender As String
    Dim userid As String
    Dim fullname As String
    'Dim result As Integer
    'Dim r As Random = New Random(Guid.NewGuid().GetHashCode())
    'Dim uniqid As String
    Private Sub UserSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TabControl1.TabPages(0).Text = "User Accounts"
        Usersloadusers()
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Enabled = False
        AddUser.AddUserText = "ADD USER"
        AddUser.Show()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Enabled = False
        AddUser.AddUserText = "EDIT USER"
        AddUser.userid = DataGridViewUserSettings.SelectedRows(0).Cells(14).Value.ToString
        AddUser.Show()
    End Sub
    Public Sub Usersloadusers()
        Try
            If ClientRole = "Crew" Then
                GLOBAL_SELECT_ALL_FUNCTION("loc_users WHERE user_level <> 'Admin' AND user_id = " & ClientCrewID & " AND store_id= " & ClientStoreID & " AND guid='" & ClientGuid & "' AND active = 1 ", "*", DataGridViewUserSettings)
            Else
                GLOBAL_SELECT_ALL_FUNCTION("loc_users WHERE user_level <> 'Admin' AND store_id= " & ClientStoreID & " AND guid='" & ClientGuid & "' AND active = 1 ", "*", DataGridViewUserSettings)
            End If

            With DataGridViewUserSettings
                .AllowUserToAddRows = False
                .DataSource = dt
                .RowHeadersVisible = False
                .CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
                .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                .Columns(0).Visible = False
                .Columns(1).Visible = False
                .Columns(4).Visible = False
                .Columns(7).Visible = False
                .Columns(9).Visible = False
                .Columns(10).Visible = False
                .Columns(11).Visible = False
                .Columns(12).Visible = False
                .Columns(13).Visible = False
                .Columns(15).Visible = False
                .Columns(2).HeaderText = "Full Name"
                .Columns(3).HeaderText = "Username"
                .Columns(5).HeaderText = "Contact Number"
                .Columns(6).HeaderText = "Email Address"
                .Columns(8).HeaderText = "Gender"
                .Columns(14).HeaderText = "Crew ID"
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub ButtonDeleteProducts_Click(sender As Object, e As EventArgs) Handles ButtonDeleteProducts.Click
        If ClientRole = "Head Crew" Then
            deactivateuser()
        Else
            MsgBox("You do not have permission to perform this task" & vbNewLine & "Please contact your administrator for help.")
        End If
    End Sub
    Private Sub deactivateuser()
        userid = DataGridViewUserSettings.SelectedRows(0).Cells(0).Value.ToString()
        fullname = DataGridViewUserSettings.SelectedRows(0).Cells(2).Value.ToString()
        Dim deactivation = MessageBox.Show("Are you sure you want to deactivate ( " & fullname & " ) account?", "Deactivation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
        If deactivation = DialogResult.Yes Then
            Try
                Dim sql = "UPDATE loc_users SET active = 0 WHERE user_id =" & userid
                Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn)
                Dim result = cmd.ExecuteNonQuery()
                If result = 1 Then
                    MsgBox("Account Deactivated")
                    Usersloadusers()
                    messageboxappearance = False
                    SystemLogType = "USER DEACTIVATION"
                    SystemLogDesc = "Deactivated by :" & returnfullname(ClientCrewID) & " : " & ClientRole
                    GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
                End If
            Catch ex As Exception
                MsgBox(ex.ToString)
                SendErrorReport(ex.ToString)
            End Try
        End If
    End Sub
    Private Sub ButtonKeyboard_Click(sender As Object, e As EventArgs) Handles ButtonKeyboard.Click
        ShowKeyboard()
    End Sub

    Private Sub UserSettings_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If Application.OpenForms().OfType(Of AddUser).Any Then
            AddUser.Close()
        End If
    End Sub
End Class