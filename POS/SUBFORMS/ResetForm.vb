Imports MySql.Data.MySqlClient
Imports System.Threading
Public Class ResetForm
    Dim Query As String = ""
    Private Sub ResetForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            CheckForIllegalCrossThreadCalls = False
            Panel1.Location = New Point((Me.Width - Panel1.Width) \ 2, (Me.Height - Panel1.Height) \ 2)
            Panel2.Location = New Point((Me.Width - Panel2.Width) \ 2, (Me.Height - Panel2.Height) \ 2)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub TruncateTable(ToTruncate)
        Try
            Query += "TRUNCATE TABLE " & ToTruncate & " ;"
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Query = ""
            If CheckBoxCategories.Checked Then
                TruncateTable("loc_admin_category")
            End If
            If CheckBoxProducts.Checked Then
                TruncateTable("loc_admin_products")
                TruncateTable("triggers_loc_admin_products")
            End If
            If CheckBoxCouponData.Checked Then
                TruncateTable("loc_coupon_data")
            End If
            If CheckBoxSales.Checked Then
                TruncateTable("loc_daily_transaction")
                TruncateTable("loc_daily_transaction_details")
                TruncateTable("loc_senior_details")
                TruncateTable("loc_transaction_mode_details")
            End If
            If CheckBoxDeposits.Checked Then
                TruncateTable("loc_deposit")
            End If
            If CheckBoxExpenses.Checked Then
                TruncateTable("loc_expense_details")
                TruncateTable("loc_expense_list")
            End If
            If CheckBoxFMStocks.Checked Then
                TruncateTable("loc_fm_stock")
            End If
            If CheckBoxMessage.Checked Then
                TruncateTable("loc_inbox_messages")
            End If
            If CheckBoxInvTempData.Checked Then
                TruncateTable("loc_inv_temp_data")
            End If
            If CheckBoxPartners.Checked Then
                TruncateTable("loc_partners_transaction")
            End If
            If CheckBoxPendingOrders.Checked Then
                TruncateTable("loc_pending_orders")
            End If
            If CheckBoxInventory.Checked Then
                TruncateTable("loc_pos_inventory")
            End If
            If CheckBoxPriceReq.Checked Then
                TruncateTable("loc_price_request_change")
            End If
            If CheckBoxFormula.Checked Then
                TruncateTable("loc_product_formula")
            End If
            If CheckBoxReturns.Checked Then
                TruncateTable("loc_refund_return_details")
            End If
            If CheckBoxErrorLogs.Checked Then
                TruncateTable("loc_send_bug_report")
            End If
            If CheckBoxStockAdjCat.Checked Then
                TruncateTable("loc_stockadjustment_cat")
            End If
            If CheckBoxSystemLogs.Checked Then
                TruncateTable("loc_system_logs")
            End If
            If CheckBoxTransferInventory.Checked Then
                TruncateTable("loc_transfer_data")
            End If
            If CheckBoxUsers.Checked Then
                TruncateTable("loc_users")
                TruncateTable("triggers_loc_users")
            End If
            If CheckBoxZreadInventory.Checked Then
                TruncateTable("loc_zread_inventory")
            End If
            If CheckBoxCoupons.Checked Then
                TruncateTable("tbcoupon")
            End If
            Dim msg = MessageBox.Show("Are you sure you want to truncate all selected table(s)?", "NOTICE", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If msg = DialogResult.Yes Then
                Dim ConnectionLocal As MySqlConnection = LocalhostConn()
                Dim cmd As MySqlCommand = New MySqlCommand(Query, ConnectionLocal)
                Dim res = cmd.ExecuteNonQuery()
                If res = 1 Then
                    MsgBox("Complete")
                Else
                    MsgBox("Error found")
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub TextBoxUsername_KeyPress(sender As Object, e As KeyPressEventArgs)
        Try
            If InStr(DisallowedCharacters, e.KeyChar) > 0 Then
                e.Handled = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub ButttonLogin_Click(sender As Object, e As EventArgs) Handles ButttonLogin.Click
        Try
            Label1.Text = "Validating account."
            Timer1.Start()
            BackgroundWorker1.WorkerReportsProgress = True
            BackgroundWorker1.WorkerSupportsCancellation = True
            BackgroundWorker1.RunWorkerAsync()
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub ButtonKeyboard_Click(sender As Object, e As EventArgs) Handles ButtonKeyboard.Click
        Try
            ShowKeyboard()
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Dim threadList As List(Of Thread) = New List(Of Thread)
    Dim thread As Thread
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Label1.Text = "Validating account." Then
            Label1.Text = "Validating account.."
        ElseIf Label1.Text = "Validating account.." Then
            Label1.Text = "Validating account..."
        ElseIf Label1.Text = "Validating account..." Then
            Label1.Text = "Validating account."
        End If
    End Sub
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            For i = 0 To 100
                BackgroundWorker1.ReportProgress(i)
                Thread.Sleep(50)
                If i = 0 Then
                    thread = New Thread(AddressOf CheckAdmin)
                    thread.Start()
                    threadList.Add(thread)
                End If
            Next
            For Each t In threadList
                t.Join()
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        Try
            ProgressBar1.Value = e.ProgressPercentage
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            Timer1.Stop()
            If ValidAccount Then
                Panel2.Visible = False
                Panel1.Visible = True
            Else
                Label1.Text = "Account is not valid."
                Panel2.Visible = True
                Panel1.Visible = False
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Dim ValidAccount As Boolean = False
    Private Sub CheckAdmin()
        Try
            Dim ConnectionServer As MySqlConnection = ServerCloudCon()
            Dim cipherText = ConvertPassword(txtpassword.Text)
            Dim sql = "SELECT user_role FROM admin_user WHERE user_name = '" & Trim(txtusername.Text) & "' AND user_pass = '" & cipherText & "' AND status = 1 AND user_role = 'Admin'"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, ConnectionServer)
            Using reader As MySqlDataReader = cmd.ExecuteReader
                If reader.HasRows Then
                    ValidAccount = True
                Else
                    ValidAccount = False
                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
End Class