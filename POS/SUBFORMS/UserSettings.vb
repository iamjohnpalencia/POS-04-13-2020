Imports MySql.Data.MySqlClient
Public Class UserSettings
    Dim gender As String
    Dim userid As String
    Dim fullname As String
    Dim result As Integer
    Dim r As Random = New Random(Guid.NewGuid().GetHashCode())
    Dim uniqid As String
    Private Sub UserSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TabControl1.TabPages(0).Text = "User Accounts"
        Usersloadusers()
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Panel6.Top = (Me.Height - Panel6.Height) / 4
        Panel6.Left = (Me.Width - Panel6.Width) / 3
        Panel6.Visible = True
        Label3.Text = "ADD USER"
        ButtonUser.Text = "Add User"
        TextBoxCONPASS.Enabled = True
    End Sub
    Public Sub Usersloadusers()
        Try
            GLOBAL_SELECT_ALL_FUNCTION("loc_users WHERE store_id= " & ClientStoreID & " AND guid='" & ClientGuid & "' AND active = 1 ", "*", "", "", DataGridViewUserSettings)
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
        End Try
    End Sub
    Private Sub ButtonExit_Click(sender As Object, e As EventArgs) Handles ButtonExit.Click
        MDIFORM.Button2.PerformClick()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Panel6.Visible = False
        ClearTextBox(Me)
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles ButtonUser.Click
        If ButtonUser.Text = "Add User" Then
            adduser()
        ElseIf ButtonUser.Text = "Update" Then
            updateuser()
        End If
        Usersloadusers()
        messageboxappearance = False
        SystemLogType = "NEW USER"
        SystemLogDesc = "Added by :" & returnfullname(ClientCrewID)
        GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
    End Sub
    Private Sub adduser()
        If String.IsNullOrEmpty(TextBoxFULLNAME.Text.Trim) Then
            TextBoxFULLNAME.Clear()
            MessageBox.Show("Full name is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf TextBoxEMAIL.Text.Trim.Length = 0 Then
            MessageBox.Show("Email is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf TextBoxUSERNAME.Text.Trim.Length = 0 Then
            MessageBox.Show("Username is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf TextBoxPASS.Text.Trim.Length = 0 Then
            MessageBox.Show("Password is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf TextBoxCONTACT.Text.Trim.Length = 0 Then
            MessageBox.Show("Contact number is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            If TextBoxPASS.Text.Trim <> TextBoxCONPASS.Text.Trim Then
                MessageBox.Show("Password did not match!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim cipherText As String = ConvertPassword(SourceString:=TextBoxPASS.Text)
                Try
                    messageboxappearance = True
                    If RadioButtonMALE.Checked = True Then
                        gender = "Male"
                    ElseIf RadioButtonFEMALE.Checked = True Then
                        gender = "Female"
                    End If
                    uniqid = ClientStorename & "-" & r.[Next](1000, 10000)
                    table = "loc_users"
                    fields = " (`uniq_id`,`user_level`,`full_name`,`username`,`password`,`contact_number`,`email`,`position`,`store_id`,`gender`,`active`,`guid`,`synced`)"
                    value = "('" & uniqid & "'
                            ,'Crew'
                            , '" & TextBoxFULLNAME.Text & "'
                            , '" & TextBoxUSERNAME.Text & "'
                            , '" & cipherText & "'
                            , '" & TextBoxCONTACT.Text & "'
                            , '" & TextBoxEMAIL.Text & "'
                            , 'Crew'
                            , " & ClientStoreID & "  
                            , '" & gender & "'
                            , " & 1 & "
                            , '" & ClientGuid & "'
                            , 'Unsynced')"
                    successmessage = "Successfully Registered!"
                    errormessage = "error registrationvb(loc_users)"
                    GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value, successmessage:=successmessage, errormessage:=errormessage)
                Catch ex As Exception
                End Try
                ClearTextBox(Me)
                selectmax(whatform:=3)
                messageboxappearance = False
                SystemLogType = "NEW USER"
                SystemLogDesc = "Added by :" & returnfullname(ClientCrewID) & " : " & ClientRole
                GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
            End If
        End If
    End Sub
    Private Sub updateuser()
        uniqid = DataGridViewUserSettings.SelectedRows(0).Cells(14).Value.ToString()
        If String.IsNullOrEmpty(TextBoxFULLNAME.Text.Trim) Then
            TextBoxFULLNAME.Clear()
            MessageBox.Show("Full name is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf TextBoxEMAIL.Text.Trim.Length = 0 Then
            MessageBox.Show("Email is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf TextBoxUSERNAME.Text.Trim.Length = 0 Then
            MessageBox.Show("Username is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf TextBoxCONTACT.Text.Trim.Length = 0 Then
            MessageBox.Show("Contact number is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Dim cipherText As String = ConvertPassword(SourceString:=TextBoxPASS.Text)

            Try
                messageboxappearance = False
                If RadioButtonMALE.Checked = True Then
                    gender = "Male"
                ElseIf RadioButtonFEMALE.Checked = True Then
                    gender = "Female"
                End If
                table = " loc_users "
                If String.IsNullOrWhiteSpace(TextBoxPASS.Text) Then
                    fields = "`full_name`='" & TextBoxFULLNAME.Text & "',`username`='" & TextBoxUSERNAME.Text & "',`contact_number`= '" & TextBoxCONTACT.Text & "',`email`='" & TextBoxEMAIL.Text & "',`gender`='" & gender & "',`active`=" & 1 & ", synced = 'Unsynced' "
                Else
                    fields = "`full_name`='" & TextBoxFULLNAME.Text & "',`username`='" & TextBoxUSERNAME.Text & "',`password`='" & cipherText & "',`contact_number`= '" & TextBoxCONTACT.Text & "',`email`='" & TextBoxEMAIL.Text & "',`gender`='" & gender & "',`active`=" & 1 & ", synced = 'Unsynced' "
                End If
                Dim where = " uniq_id = '" & uniqid & "'"
                GLOBAL_FUNCTION_UPDATE(table, fields, where)
            Catch ex As Exception
            End Try
            ClearTextBox(Me)
            selectmax(whatform:=3)
            messageboxappearance = False
            SystemLogType = "USER UPDATE"
            SystemLogDesc = "Updated by :" & returnfullname(ClientCrewID) & " : " & ClientRole
            GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ButtonUser.Text = "Update"
        Panel6.Top = (Me.Height - Panel6.Height) / 4
        Panel6.Left = (Me.Width - Panel6.Width) / 3
        Panel6.Visible = True
        edituser()
    End Sub
    Private Sub edituser()
        Label3.Text = "EDIT USER"
        userid = DataGridViewUserSettings.SelectedRows(0).Cells(14).Value.ToString()
        TextBoxCONPASS.Enabled = False
        Try
            sql = "SELECT * FROM loc_users WHERE uniq_id = '" & userid & "' "
            cmd = New MySqlCommand(sql, LocalhostConn)
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            For Each row As DataRow In dt.Rows
                TextBoxFULLNAME.Text = row("full_name")
                TextBoxUSERNAME.Text = row("username")
                TextBoxEMAIL.Text = row("email")
                TextBoxCONTACT.Text = row("contact_number")
                gender = row("gender")
            Next
            If gender = "Male" Then
                RadioButtonMALE.Checked = True
            Else
                RadioButtonFEMALE.Checked = True
            End If
            Panel6.Visible = True
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub ButtonDeleteProducts_Click(sender As Object, e As EventArgs) Handles ButtonDeleteProducts.Click
        deactivateuser()
    End Sub
    Private Sub deactivateuser()
        userid = DataGridViewUserSettings.SelectedRows(0).Cells(0).Value.ToString()
        fullname = DataGridViewUserSettings.SelectedRows(0).Cells(2).Value.ToString()
        Dim deactivation = MessageBox.Show("Are you sure you want to deactivate ( " & fullname & " ) account?", "Deactivation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
        If deactivation = DialogResult.Yes Then
            Try
                sql = "UPDATE loc_users SET active = 0 WHERE user_id =" & userid
                cmd = New MySqlCommand(sql, LocalhostConn)
                result = cmd.ExecuteNonQuery()
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
            End Try
        End If
    End Sub
End Class