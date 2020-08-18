﻿
Public Class Registration
    Dim gender As String
    Private ImagePath As String = ""
    Dim r As Random = New Random(Guid.NewGuid().GetHashCode())
    Dim uniqid As String
    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OfdImage.FileOk
        ImagePath = OfdImage.FileName
    End Sub
    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Login.Show()
        Me.Dispose()
    End Sub
    Private Sub ButtonSubmit_click(sender As Object, e As EventArgs) Handles ButtonSubmit.Click
        If String.IsNullOrWhiteSpace(TextBoxFN.Text.Trim) Then
            TextBoxFN.Clear()
            MessageBox.Show("Full name is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf String.IsNullOrWhiteSpace(TextBoxEMAIL.Text.Trim) Then
            MessageBox.Show("Email is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf String.IsNullOrWhiteSpace(TextBoxUN.Text.Trim) Then
            MessageBox.Show("Username is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf String.IsNullOrWhiteSpace(TextBoxP.Text.Trim) Then
            MessageBox.Show("Password is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf String.IsNullOrWhiteSpace(TextBoxCN.Text.Trim) Then
            MessageBox.Show("Contact number is required!", "Incomplete Fields", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            If TextBoxP.Text.Trim <> TextBoxCP.Text.Trim Then
                MessageBox.Show("Password did not match!", "", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim cipherText As String = ConvertPassword(SourceString:=TextBoxP.Text)
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
                            , 'Crew'
                            , '" & TextBoxFN.Text & "'
                            , '" & TextBoxUN.Text & "'
                            , '" & cipherText & "'
                            , '" & TextBoxCN.Text & "'
                            , '" & TextBoxEMAIL.Text & "'
                            , 'Crew'
                            , " & ClientStoreID & "  
                            , '" & gender & "'
                            , " & 1 & "
                            , '" & ClientGuid & "'
                            , 'Unsynced')"
                    successmessage = "Successfully Registered!"
                    errormessage = "error registrationvb(loc_users)"
                    GLOBAL_INSERT_FUNCTION(table:=table, fields:=fields, values:=value)
                Catch ex As Exception
                End Try
                SystemLogType = "USER REGISTRATION"
                SystemLogDesc = "Registration of: " & TextBoxFN.Text
                GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
                ClearTextBox(Me)
                selectmax(whatform:=3)
                MessageBox.Show("Success fully registered", "Registration", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs)

    End Sub
    Private Sub Registration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        selectmax(whatform:=3)
    End Sub
    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        If MessageBox.Show("Are you sure you really want to exit ?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            Application.Exit()
        End If
    End Sub
    Private Sub ButtonKeyboard_Click(sender As Object, e As EventArgs) Handles ButtonKeyboard.Click
        ShowKeyboard()
    End Sub
    Private Sub TextBoxFN_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxUN.KeyPress, TextBoxP.KeyPress, TextBoxFN.KeyPress, TextBoxEMAIL.KeyPress, TextBoxCP.KeyPress, TextBoxCN.KeyPress
        Try
            If InStr(DisallowedCharacters, e.KeyChar) > 0 Then
                e.Handled = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
End Class