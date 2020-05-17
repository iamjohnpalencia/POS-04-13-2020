Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.IO
Imports System.Text
'Requirements
'my.settings.validlocalconn/cloudconn = 1
'franchiseeacc = true/ accountexist = true
Public Class ConfigManager
    Dim BGWIdentifyer As Integer = 0
    Dim thread1 As Thread
    Dim FranchiseeStoreValidation As Boolean
    Dim UserID
    Dim BTNSaveLocalConn As Boolean = False
    Dim BTNSaveCloudConn As Boolean = False
    Private Sub ConfigManager_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        TabControl1.TabPages(0).Text = "General Settings"
        TabControl1.TabPages(1).Text = "License And Activation"
        TabControl2.TabPages(0).Text = "Connection Settings"
        TabControl2.TabPages(1).Text = "Additional Settings"
        TabControl3.TabPages(0).Text = "Account/ Store Settings"
        TabControl3.TabPages(1).Text = "Activation"
        If System.IO.File.Exists(My.Settings.LocalConnectionPath) Then
            BackgroundWorkerLOAD.WorkerReportsProgress = True
            BackgroundWorkerLOAD.WorkerReportsProgress = True
            BackgroundWorkerLOAD.RunWorkerAsync()
        Else
            My.Settings.ValidLocalConn = False
            My.Settings.ValidCloudConn = False
            My.Settings.Save()
        End If
    End Sub
    Private Function TestLocalConnection()
        Dim Conn As MySqlConnection = New MySqlConnection
        Try
            Conn.ConnectionString = "server=" & Trim(TextBoxLocalServer.Text) &
            ";user id= " & Trim(TextBoxLocalUsername.Text) &
            ";password=" & Trim(TextBoxLocalPassword.Text) &
            ";database=" & Trim(TextBoxLocalDatabase.Text) &
            ";port=" & Trim(TextBoxLocalPort.Text)
            Conn.Open()
            If Conn.State = ConnectionState.Open Then
                My.Settings.ValidLocalConn = True
                My.Settings.Save()
            End If
        Catch ex As Exception
            My.Settings.ValidLocalConn = False
            My.Settings.Save()
        End Try
        Return Conn
    End Function
    Private Function TestCloudConnection()
        Dim cloudconn As MySqlConnection = New MySqlConnection
        Try
            cloudconn.ConnectionString = "server=" & Trim(TextBoxCloudServer.Text) &
            ";user id= " & Trim(TextBoxCloudUsername.Text) &
            ";password=" & Trim(TextBoxCloudPassword.Text) &
            ";database=" & Trim(TextBoxCloudDatabase.Text) &
            ";port=" & Trim(TextBoxCloudPort.Text)
            cloudconn.Open()
            If cloudconn.State = ConnectionState.Open Then
                My.Settings.ValidCloudConn = True
                My.Settings.Save()
            End If
        Catch ex As Exception
            My.Settings.ValidCloudConn = False
            My.Settings.Save()
        End Try
        Return cloudconn
    End Function


    Private Sub CreateFolder(Path As String, FolderName As String, Optional ByVal Attributes As System.IO.FileAttributes = IO.FileAttributes.Normal)
        My.Computer.FileSystem.CreateDirectory(Path & "\" & FolderName)
        If Not Attributes = IO.FileAttributes.Normal Then
            My.Computer.FileSystem.GetDirectoryInfo(Path & "\" & FolderName).Attributes = Attributes
        End If
        CreateUserConfig(Path, "user.config", FolderName)
    End Sub
    Public Sub CreateUserConfig(path As String, FileName As String, FolderName As String, Optional ByVal Attributes As System.IO.FileAttributes = IO.FileAttributes.Normal)
        Dim CompletePath As String = path & "\" & FolderName & "\" & "user.config"
        My.Computer.FileSystem.CreateDirectory(path & "\" & FolderName)
        If Not Attributes = IO.FileAttributes.Normal Then
            My.Computer.FileSystem.GetDirectoryInfo(path & "\" & FolderName).Attributes = Attributes
        End If
        Dim ConnString(5) As String
        ConnString(0) = "server=" & ConvertToBase64(Trim(TextBoxLocalServer.Text))
        ConnString(1) = "user id=" & ConvertToBase64(Trim(TextBoxLocalUsername.Text))
        ConnString(2) = "password=" & ConvertToBase64(Trim(TextBoxLocalPassword.Text))
        ConnString(3) = "database=" & ConvertToBase64(Trim(TextBoxLocalDatabase.Text))
        ConnString(4) = "port=" & ConvertToBase64(Trim(TextBoxLocalPort.Text))
        ConnString(5) = "Allow Zero Datetime=True"
        File.WriteAllLines(CompletePath, ConnString, Encoding.UTF8)
        CreateConn(CompletePath)
        My.Settings.LocalConnectionPath = CompletePath
        My.Settings.Save()
        MsgBox("Saved")
        'MsgBox(My.Settings.LocalConnectionPath)
        'MsgBox(My.Settings.LocalConnectionString)
    End Sub
    Private Sub ButtonTestLocConn_Click(sender As Object, e As EventArgs) Handles ButtonTestLocConn.Click
        TextboxEnableability(Panel5, False)
        ButtonEnableability(Panel5, False)
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.RunWorkerAsync()
    End Sub
    Private Sub ButtonClearLocal_Click(sender As Object, e As EventArgs) Handles ButtonClearLocal.Click
        ClearTextBox(Panel5)
    End Sub
    Private Sub ButtonClearCloud_Click(sender As Object, e As EventArgs) Handles ButtonClearCloud.Click
        ClearTextBox(Panel9)
    End Sub
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            For i = 0 To 100
                LabelLocal.Text = "Checking Connection " & i & " %"
                BackgroundWorker1.ReportProgress(i)
                Thread.Sleep(50)
                If i = 10 Then
                    thread1 = New Thread(AddressOf TestLocalConnection)
                    thread1.Start()
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        If My.Settings.ValidLocalConn = False Then
            ChangeProgBarColor(ProgressBar1, ProgressBarColor.Yellow)
            LabelLocal.Text = "Invalid connection please try again."
        Else
            ChangeProgBarColor(ProgressBar1, ProgressBarColor.Green)
            LabelLocal.Text = "Connected successfully!"

            ButtonSaveLocalCon.PerformClick()
        End If
        TextboxEnableability(Panel5, True)
        ButtonEnableability(Panel5, True)
    End Sub

    Private Sub ButtonTestCloudConn_Click(sender As Object, e As EventArgs) Handles ButtonTestCloudConn.Click
        TextboxEnableability(Panel9, False)
        ButtonEnableability(Panel9, False)
        BackgroundWorker2.WorkerSupportsCancellation = True
        BackgroundWorker2.WorkerReportsProgress = True
        BackgroundWorker2.RunWorkerAsync()
    End Sub
    Private Sub BackgroundWorker2_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker2.DoWork
        Try
            For i = 0 To 100
                LabelCloud.Text = "Checking Connection " & i & " %"
                BackgroundWorker2.ReportProgress(i)
                Thread.Sleep(50)
                If i = 10 Then
                    thread1 = New Thread(AddressOf TestCloudConnection)
                    thread1.Start()
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorker2_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker2.ProgressChanged
        ProgressBar2.Value = e.ProgressPercentage
    End Sub
    Private Sub BackgroundWorker2_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker2.RunWorkerCompleted
        If My.Settings.ValidCloudConn = False Then
            ChangeProgBarColor(ProgressBar2, ProgressBarColor.Yellow)
            LabelCloud.Text = "Invalid connection please try again."
        Else
            ChangeProgBarColor(ProgressBar2, ProgressBarColor.Green)
            LabelCloud.Text = "Connected successfully!"
        End If
        TextboxEnableability(Panel9, True)
        ButtonEnableability(Panel9, True)
    End Sub
    Private Sub ButtonSaveCloudConn_Click(sender As Object, e As EventArgs) Handles ButtonSaveCloudConn.Click
        Try
            table = "loc_settings"
            where = "settings_id = 1"
            If My.Settings.ValidLocalConn = True Then
                If My.Settings.ValidCloudConn = True Then
                    fields = "C_Server, C_Username, C_Password, C_Database, C_Port"
                    sql = "Select " & fields & " FROM " & table & " WHERE " & where
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection())
                    Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                    Dim dt As DataTable = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        fields = "C_Server = '" & ConvertToBase64(Trim(TextBoxCloudServer.Text)) & "', C_Username = '" & ConvertToBase64(Trim(TextBoxCloudUsername.Text)) & "', C_Password = '" & ConvertToBase64(Trim(TextBoxCloudPassword.Text)) & "', C_Database = '" & ConvertToBase64(Trim(TextBoxCloudDatabase.Text)) & "', C_Port = '" & ConvertToBase64(Trim(TextBoxCloudPort.Text)) & "'"
                        sql = "UPDATE " & table & " SET " & fields & " WHERE " & where
                        cmd = New MySqlCommand(sql, TestLocalConnection())
                        cmd.ExecuteNonQuery()
                        MsgBox("Saved!")
                    Else
                        fields = "(C_Server, C_Username, C_Password, C_Database, C_Port, S_Zreading)"
                        value = "('" & ConvertToBase64(Trim(TextBoxCloudServer.Text)) & "'
                     ,'" & ConvertToBase64(Trim(TextBoxCloudUsername.Text)) & "'
                     ,'" & ConvertToBase64(Trim(TextBoxCloudPassword.Text)) & "'
                     ,'" & ConvertToBase64(Trim(TextBoxCloudDatabase.Text)) & "'
                     ,'" & ConvertToBase64(Trim(TextBoxCloudPort.Text)) & "'
                     ,'" & returndateformat(Now) & "')"
                        sql = "INSERT INTO " & table & " " & fields & " VALUES " & value
                        cmd = New MySqlCommand(sql, TestLocalConnection)
                        cmd.ExecuteNonQuery()
                        MsgBox("Saved!")
                    End If
                    BTNSaveCloudConn = True
                Else
                    MsgBox("Connection must be valid")
                End If
            Else
                MsgBox("Local connection must be valid first.")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadConn()
        Try
            If My.Settings.LocalConnectionPath <> "" Then
                If System.IO.File.Exists(My.Settings.LocalConnectionPath) Then
                    'The File exists 
                    Dim CreateConnString As String = ""
                    Dim filename As String = String.Empty
                    Dim TextLine As String = ""
                    Dim objReader As New System.IO.StreamReader(My.Settings.LocalConnectionPath)
                    Dim lineCount As Integer
                    Do While objReader.Peek() <> -1
                        TextLine = objReader.ReadLine()
                        If lineCount = 0 Then
                            TextBoxLocalServer.Text = ConvertB64ToString(RemoveCharacter(TextLine, "server="))
                        End If
                        If lineCount = 1 Then
                            TextBoxLocalUsername.Text = ConvertB64ToString(RemoveCharacter(TextLine, "user id="))
                        End If
                        If lineCount = 2 Then
                            TextBoxLocalPassword.Text = ConvertB64ToString(RemoveCharacter(TextLine, "password="))
                        End If
                        If lineCount = 3 Then
                            TextBoxLocalDatabase.Text = ConvertB64ToString(RemoveCharacter(TextLine, "database="))
                        End If
                        If lineCount = 4 Then
                            TextBoxLocalPort.Text = ConvertB64ToString(RemoveCharacter(TextLine, "port="))
                        End If
                        lineCount = lineCount + 1
                    Loop
                    objReader.Close()
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub LoadCloudConn()
        Try
            If My.Settings.ValidLocalConn = True Then
                sql = "SELECT C_Server, C_Username, C_Password, C_Database, C_Port FROM loc_settings WHERE settings_id = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection)
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    TextBoxCloudServer.Text = ConvertB64ToString(dt(0)(0))
                    TextBoxCloudUsername.Text = ConvertB64ToString(dt(0)(1))
                    TextBoxCloudPassword.Text = ConvertB64ToString(dt(0)(2))
                    TextBoxCloudDatabase.Text = ConvertB64ToString(dt(0)(3))
                    TextBoxCloudPort.Text = ConvertB64ToString(dt(0)(4))
                    My.Settings.ValidCloudConn = True
                    My.Settings.Save()
                Else
                    My.Settings.ValidCloudConn = False
                    My.Settings.Save()
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadAdditionalSettings()
        Try
            If My.Settings.ValidLocalConn = True Then
                Dim sql = "SELECT A_Export_Path, A_Tax, A_SIFormat, A_Terminal_No, A_ZeroRated FROM loc_settings WHERE settings_id = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection)
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                For Each row As DataRow In dt.Rows
                    If row("A_Export_Path") <> "" Then
                        If row("A_Tax") <> "" Then
                            If row("A_SIFormat") <> "" Then
                                If row("A_Terminal_No") <> "" Then
                                    If row("A_ZeroRated") <> "" Then
                                        TextBoxExportPath.Text = ConvertB64ToString(row("A_Export_Path"))
                                        TextBoxTax.Text = Val(row("A_Tax")) * 100
                                        TextBoxSINumber.Text = row("A_SIFormat")
                                        TextBoxTerminalNo.Text = row("A_Terminal_No")
                                        If Val(row("A_ZeroRated")) = 0 Then
                                            RadioButtonNO.Checked = True
                                        ElseIf dt(0)(4) = 1 Then
                                            RadioButtonYES.Checked = True
                                        End If
                                        My.Settings.ValidAddtionalSettings = True
                                    Else
                                        My.Settings.ValidAddtionalSettings = False
                                        Exit For
                                    End If
                                Else
                                    My.Settings.ValidAddtionalSettings = False
                                    Exit For
                                End If
                            Else
                                My.Settings.ValidAddtionalSettings = False
                                Exit For
                            End If
                        Else
                            My.Settings.ValidAddtionalSettings = False
                            Exit For
                        End If
                    Else
                        My.Settings.ValidAddtionalSettings = False
                        Exit For
                    End If
                Next
                My.Settings.Save()
            Else
                My.Settings.ValidAddtionalSettings = False
                My.Settings.Save()
            End If
            MsgBox(My.Settings.ValidAddtionalSettings)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadDevInfo()
        Try
            If My.Settings.ValidLocalConn = True Then
                sql = "SELECT Dev_Company_Name, Dev_Address, Dev_Tin, Dev_Accr_No, Dev_Accr_Date_Issued, Dev_Accr_Valid_Until, Dev_PTU_No, Dev_PTU_Date_Issued, Dev_PTU_Valid_Until FROM loc_settings WHERE settings_id = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection)
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt = New DataTable
                da.Fill(dt)
                For Each row As DataRow In dt.Rows
                    If row("Dev_Company_Name") <> "" Then
                        If row("Dev_Address") <> "" Then
                            If row("Dev_Tin") <> "" Then
                                If row("Dev_Accr_No") <> "" Then
                                    If row("Dev_Accr_Date_Issued") <> "" Then
                                        If row("Dev_Accr_Valid_Until") <> "" Then
                                            If row("Dev_PTU_No") <> "" Then
                                                If row("Dev_PTU_Date_Issued") <> "" Then
                                                    If row("Dev_PTU_Valid_Until") <> "" Then
                                                        TextBoxDevname.Text = row("Dev_Company_Name")
                                                        TextBoxDevAdd.Text = row("Dev_Address")
                                                        TextBoxDevTIN.Text = row("Dev_Tin")
                                                        TextBoxDevAccr.Text = row("Dev_Accr_No")
                                                        DateTimePicker1ACCRDI.Value = row("Dev_Accr_Date_Issued")
                                                        DateTimePicker2ACCRVU.Value = row("Dev_Accr_Valid_Until")
                                                        TextBoxDEVPTU.Text = row("Dev_PTU_No")
                                                        DateTimePicker4PTUDI.Value = row("Dev_PTU_Date_Issued")
                                                        DateTimePickerPTUVU.Value = row("Dev_PTU_Valid_Until")
                                                        My.Settings.ValidDevSettings = True
                                                        My.Settings.Save()
                                                    Else
                                                        My.Settings.ValidDevSettings = False
                                                        Exit For
                                                    End If
                                                Else
                                                    My.Settings.ValidDevSettings = False
                                                    Exit For
                                                End If
                                            Else
                                                My.Settings.ValidDevSettings = False
                                                Exit For
                                            End If
                                        Else
                                            My.Settings.ValidDevSettings = False
                                            Exit For
                                        End If
                                    Else
                                        My.Settings.ValidDevSettings = False
                                        Exit For
                                    End If
                                Else
                                    My.Settings.ValidDevSettings = False
                                    Exit For
                                End If
                            Else
                                My.Settings.ValidDevSettings = False
                                Exit For
                            End If
                        Else
                            My.Settings.ValidDevSettings = False
                            Exit For
                        End If
                    Else
                        My.Settings.ValidDevSettings = False
                        Exit For
                    End If
                Next
                My.Settings.Save()
            Else
                My.Settings.ValidDevSettings = False
                My.Settings.Save()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadOutlets()
        Try
            Dim CloudCmd As MySqlCommand
            Dim CloudDa As MySqlDataAdapter
            Dim CloudDT As DataTable
            sql = "SELECT * FROM admin_outlets WHERE user_guid = '" & UserGUID & "' AND active = 1"
            CloudCmd = New MySqlCommand(sql, CloudConn2)
            CloudDa = New MySqlDataAdapter(CloudCmd)
            CloudDT = New DataTable
            CloudDa.Fill(CloudDT)
            With DataGridViewOutlets
                .DataSource = Nothing
                .DataSource = CloudDT
                .Columns(0).Visible = False
                .Columns(3).Visible = False
                .Columns(4).Visible = False
                .Columns(5).Visible = False
                .Columns(6).Visible = False
                .Columns(7).Visible = False
                .Columns(8).Visible = False
                .Columns(9).Visible = False
                .Columns(10).Visible = False
                .Columns(11).Visible = False
                .Columns(12).Visible = False
                .Columns(13).Visible = False
                .Columns(14).Visible = False
                .Columns(15).Visible = False
                .Columns(16).Visible = False
                .Columns(17).Visible = False
                .Columns(18).Visible = False
                .ColumnHeadersVisible = False
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Public Sub CreateConn(path As String)
        Try
            Dim CreateConnString As String = ""
            Dim filename As String = String.Empty
            Dim TextLine As String = ""
            Dim objReader As New System.IO.StreamReader(path)
            Dim lineCount As Integer
            Do While objReader.Peek() <> -1
                TextLine = objReader.ReadLine()
                If lineCount = 0 Then
                    CreateConnString += TextLine & ";"
                End If
                If lineCount = 1 Then
                    CreateConnString += TextLine & ";"
                End If
                If lineCount = 2 Then
                    CreateConnString += TextLine & ";"
                End If
                If lineCount = 3 Then
                    CreateConnString += TextLine & ";"
                End If
                If lineCount = 4 Then
                    CreateConnString += TextLine & ";"
                End If
                If lineCount = 5 Then
                    CreateConnString += TextLine
                End If
                lineCount = lineCount + 1
            Loop
            objReader.Close()

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles ButtonExit.Click
        Close()
    End Sub
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        If My.Settings.ValidCloudConn = True Then
            If String.IsNullOrWhiteSpace(TextBoxFrancUser.Text) Then
                MsgBox("All fields are required")
            ElseIf String.IsNullOrWhiteSpace(TextBoxFrancPass.Text) Then
                MsgBox("All fields are required")
            Else
                If CheckForInternetConnection() = True Then
                    TextboxEnableability(Panel14, False)
                    ButtonEnableability(Panel14, False)
                    BackgroundWorker3.WorkerSupportsCancellation = True
                    BackgroundWorker3.WorkerReportsProgress = True
                    BackgroundWorker3.RunWorkerAsync()
                    DataGridViewOutlets.Focus()
                    DataGridViewOutlets.DataSource = Nothing
                Else
                    MsgBox("No Internet Connection")
                End If
            End If
        Else
            MsgBox("Cloud Server Connection must be valid first.")
        End If
    End Sub
    Dim threadList As List(Of Thread) = New List(Of Thread)
    Private Sub BackgroundWorker3_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker3.DoWork
        For i = 0 To 100
            LabelAccCheck.Text = "Checking Account " & i & " %"
            BackgroundWorker3.ReportProgress(i)
            If i = 0 Then
                thread1 = New System.Threading.Thread(AddressOf checkacc)
                thread1.Start()
                threadList.Add(thread1)
            End If
            Thread.Sleep(20)
        Next
        For Each t In threadList
            t.Join()
        Next
    End Sub
    Dim DataadapterCheckAcc As MySqlDataAdapter
    Dim DatatableCheckAcc As DataTable
    Dim AccountExist As Boolean
    Dim UserGUID As String
    Dim UserStoreID As Integer
    Dim CloudConn2 As MySqlConnection
    Public Sub checkacc()
        Try
            AccountExist = False
            FranchiseeStoreValidation = False
            CloudConn2 = New MySqlConnection
            CloudConn2.ConnectionString = "server=" & Trim(TextBoxCloudServer.Text) &
            ";user id= " & Trim(TextBoxCloudUsername.Text) &
            ";password=" & Trim(TextBoxCloudPassword.Text) &
            ";database=" & Trim(TextBoxCloudDatabase.Text) &
            ";port=" & Trim(TextBoxCloudPort.Text)
            CloudConn2.Open()
            sql = "SELECT user_guid, user_id FROM admin_user WHERE user_name = '" & TextBoxFrancUser.Text & "' AND user_pass = '" & ConvertPassword(TextBoxFrancPass.Text) & "' AND user_role = 'Client' AND status = 1; "
            cmd = New MySqlCommand(sql, CloudConn2)
            DataadapterCheckAcc = New MySqlDataAdapter(cmd)
            DatatableCheckAcc = New DataTable
            DataadapterCheckAcc.Fill(DatatableCheckAcc)
            If DatatableCheckAcc.Rows.Count > 0 Then
                AccountExist = True
                UserGUID = DatatableCheckAcc(0)(0)
                UserID = DatatableCheckAcc(0)(1)
            Else
                AccountExist = False
                UserGUID = ""
            End If
        Catch ex As MySqlException
            MsgBox(ex.ToString)
        Finally
        End Try
    End Sub
    Private Sub BackgroundWorker3_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker3.ProgressChanged
        ProgressBar3.Value = e.ProgressPercentage
    End Sub
    Private Sub BackgroundWorker3_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker3.RunWorkerCompleted
        If AccountExist = True Then
            BackgroundWorker4.WorkerSupportsCancellation = True
            BackgroundWorker4.WorkerReportsProgress = True
            BackgroundWorker4.RunWorkerAsync()
            MsgBox("Exist")
        Else
            MsgBox("Invalid account.")
        End If
        TextboxEnableability(Panel14, True)
        ButtonEnableability(Panel14, True)
    End Sub
    Private Sub BackgroundWorker4_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker4.DoWork
        Try
            thread1 = New Thread(AddressOf LoadOutlets)
            thread1.Start()
            threadList.Add(thread1)
            For Each t In threadList
                t.Join()
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub DataGridViewOutlets_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewOutlets.CellClick
        Button2.PerformClick()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridViewOutlets.Rows.Count > 0 Then
            With Me
                DataGridViewOutletDetails.Rows.Clear()
                TextBoxBrandname.Text = DataGridViewOutlets.SelectedRows(0).Cells(1).Value.ToString
                TextBoxLocation.Text = DataGridViewOutlets.SelectedRows(0).Cells(4).Value.ToString
                TextBoxPostalCode.Text = DataGridViewOutlets.SelectedRows(0).Cells(5).Value.ToString
                TextBoxAddress.Text = DataGridViewOutlets.SelectedRows(0).Cells(6).Value.ToString
                TextBoxMun.Text = DataGridViewOutlets.SelectedRows(0).Cells(8).Value.ToString
                TextBoxProv.Text = DataGridViewOutlets.SelectedRows(0).Cells(9).Value.ToString
                TextBoxTIN.Text = DataGridViewOutlets.SelectedRows(0).Cells(10).Value.ToString
                TextBoxTEL.Text = DataGridViewOutlets.SelectedRows(0).Cells(11).Value.ToString
                TextBoxPTUN.Text = DataGridViewOutlets.SelectedRows(0).Cells(17).Value.ToString
                TextBoxMIN.Text = DataGridViewOutlets.SelectedRows(0).Cells(15).Value.ToString
                TextBoxMSN.Text = DataGridViewOutlets.SelectedRows(0).Cells(16).Value.ToString
                DataGridViewOutletDetails.Rows.Add(DataGridViewOutlets.SelectedRows(0).Cells(0).Value.ToString, TextBoxBrandname.Text, DataGridViewOutlets.SelectedRows(0).Cells(2).Value.ToString, UserGUID, TextBoxLocation.Text, TextBoxPostalCode.Text, TextBoxAddress.Text, DataGridViewOutlets.SelectedRows(0).Cells(7).Value.ToString, TextBoxMun.Text, TextBoxProv.Text, TextBoxTIN.Text, TextBoxTEL.Text, TextBoxMIN.Text, TextBoxMSN.Text, TextBoxPTUN.Text)
                FranchiseeStoreValidation = True
            End With
        Else
            FranchiseeStoreValidation = False
        End If
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Dim RButton As Integer
            Dim Tax = Val(TextBoxTax.Text) / 100
            If TextboxIsEmpty(GroupBox10) = True Then
                If My.Settings.ValidLocalConn = True Then
                    Dim table = "loc_settings"
                    Dim fields = "A_Export_Path, A_Tax, A_SIFormat, A_Terminal_No, A_ZeroRated"
                    Dim where = "settings_id = 1"
                    Dim sql = "Select " & fields & " FROM " & table & " WHERE " & where
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection())
                    Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                    Dim dt As DataTable = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        If RadioButtonYES.Checked = True Then
                            RButton = 1
                        ElseIf RadioButtonNO.Checked = True Then
                            RButton = 0
                        End If
                        Dim fields1 = "A_Export_Path = '" & ConvertToBase64(Trim(TextBoxExportPath.Text)) & "', A_Tax = '" & Tax & "' , A_SIFormat = '" & Trim(TextBoxSINumber.Text) & "' , A_Terminal_No = '" & Trim(TextBoxTerminalNo.Text) & "' , A_ZeroRated = '" & RButton & "', S_Zreading = CURRENT_DATE()"
                        sql = "UPDATE " & table & " SET " & fields1 & " WHERE " & where
                        cmd = New MySqlCommand(sql, TestLocalConnection)
                        cmd.ExecuteNonQuery()
                        MsgBox("Saved!")
                        My.Settings.ValidAddtionalSettings = True
                        My.Settings.Save()
                    Else
                        Dim fields2 = "(A_Export_Path, A_Tax, A_SIFormat, A_Terminal_No, A_ZeroRated, S_Zreading)"
                        Dim value = "('" & ConvertToBase64(Trim(TextBoxExportPath.Text)) & "'
                     ,'" & Tax & "'
                     ,'" & Trim(TextBoxSINumber.Text) & "'
                     ,'" & Trim(TextBoxTerminalNo.Text) & "'
                     ,'" & RButton & "'
                     ,'" & returndateformat(Now()) & "')"
                        sql = "INSERT INTO " & table & " " & fields2 & " VALUES " & value
                        cmd = New MySqlCommand(sql, TestLocalConnection)
                        cmd.ExecuteNonQuery()
                        MsgBox("Saved!")
                        My.Settings.ValidAddtionalSettings = True
                        My.Settings.Save()
                    End If
                Else
                    My.Settings.ValidAddtionalSettings = False
                    My.Settings.Save()
                    MsgBox("Invalid Local Connection.")
                End If
            Else
                MsgBox("All fields are required.")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If (FolderBrowserDialog1.ShowDialog() = DialogResult.OK) Then
            TextBoxExportPath.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub
    Private Sub BackgroundWorkerLOAD_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerLOAD.DoWork
        Try
            Thread.Sleep(30)
            For i = 0 To 100
                BackgroundWorkerLOAD.ReportProgress(i)
                If i = 0 Then
                    thread1 = New Thread(AddressOf LoadConn)
                    thread1.Start()
                    threadList.Add(thread1)
                    For Each t In threadList
                        t.Join()
                    Next
                    thread1 = New Thread(AddressOf LoadCloudConn)
                    thread1.Start()
                    threadList.Add(thread1)
                    thread1 = New Thread(AddressOf LoadAdditionalSettings)
                    thread1.Start()
                    threadList.Add(thread1)
                    thread1 = New Thread(AddressOf LoadDevInfo)
                    thread1.Start()
                    threadList.Add(thread1)
                End If
            Next
            For Each t In threadList
                t.Join()
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorkerLOAD_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorkerLOAD.ProgressChanged
        ProgressBar4.Value = e.ProgressPercentage
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim table = "loc_settings"
        Dim where = "settings_id = 1"
        If TextboxIsEmpty(GroupBox11) = True Then
            If My.Settings.ValidLocalConn = True Then
                Dim fields = "Dev_Company_Name, Dev_Address, Dev_Tin, Dev_Accr_No, Dev_Accr_Date_Issued, Dev_Accr_Valid_Until, Dev_PTU_No, Dev_PTU_Date_Issued, Dev_PTU_Valid_Until"
                Dim sql = "Select " & fields & " FROM " & table & " WHERE " & where
                Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection())
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Dim fields1 = "`Dev_Company_Name`= '" & Trim(TextBoxDevname.Text) & "',
`Dev_Address`= '" & Trim(TextBoxDevAdd.Text) & "',
`Dev_Tin`= '" & Trim(TextBoxDevTIN.Text) & "',
`Dev_Accr_No`= '" & Trim(TextBoxDevAccr.Text) & "' ,
`Dev_Accr_Date_Issued`= '" & returndateformat(DateTimePicker1ACCRDI.Value) & "',
`Dev_Accr_Valid_Until`= '" & returndateformat(DateTimePicker2ACCRVU.Value) & "',
`Dev_PTU_No`= '" & Trim(TextBoxDEVPTU.Text) & "',
`Dev_PTU_Date_Issued`= '" & returndateformat(DateTimePickerPTUVU.Value) & "',
`Dev_PTU_Valid_Until`= '" & returndateformat(DateTimePicker4PTUDI.Value) & "'"
                    sql = "UPDATE " & table & " SET " & fields1 & " WHERE " & where
                    cmd = New MySqlCommand(sql, TestLocalConnection)
                    cmd.ExecuteNonQuery()
                    My.Settings.ValidDevSettings = True
                    My.Settings.Save()
                    MsgBox("Saved!")
                Else
                    Dim fields2 = "(Dev_Company_Name, Dev_Address, Dev_Tin, Dev_Accr_No, Dev_Accr_Date_Issued, Dev_Accr_Valid_Until, Dev_PTU_No, Dev_PTU_Date_Issued, Dev_PTU_Valid_Until)"
                    Dim value = "('" & Trim(TextBoxDevname.Text) & "'
,'" & Trim(TextBoxDevAdd.Text) & "'
,'" & Trim(TextBoxDevTIN.Text) & "'
,'" & Trim(TextBoxDevAccr.Text) & "'
,'" & returndateformat(DateTimePicker1ACCRDI.Value) & "'
,'" & returndateformat(DateTimePicker2ACCRVU.Value) & "'
,'" & Trim(TextBoxDEVPTU.Text) & "'
,'" & returndateformat(DateTimePickerPTUVU.Value) & "'
,'" & returndateformat(DateTimePicker4PTUDI.Value) & "')"
                    sql = "INSERT INTO " & table & " " & fields2 & " VALUES " & value
                    cmd = New MySqlCommand(sql, TestLocalConnection)
                    cmd.ExecuteNonQuery()
                    MsgBox("Saved!")
                    My.Settings.ValidAddtionalSettings = True
                    My.Settings.Save()
                End If
            Else
                MsgBox("Invalid local connection")
                My.Settings.ValidDevSettings = False
                My.Settings.Save()
            End If
        Else
            MsgBox("All fields are required")
        End If
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        If My.Settings.ValidLocalConn = True Then
            If BTNSaveLocalConn = True Then
                If My.Settings.ValidCloudConn = True Then
                    If BTNSaveCloudConn = True Then
                        If My.Settings.ValidAddtionalSettings = True Then
                            If My.Settings.ValidDevSettings = True Then
                                If AccountExist = True Then
                                    If FranchiseeStoreValidation = True Then
                                        If Not String.IsNullOrWhiteSpace(TextBoxProdKey.Text) Then
                                            TextboxEnableability(GroupBox12, False)
                                            ButtonEnableability(GroupBox12, False)
                                            BackgroundWorkerACTIVATION.WorkerReportsProgress = True
                                            BackgroundWorkerACTIVATION.WorkerReportsProgress = True
                                            BackgroundWorkerACTIVATION.RunWorkerAsync()
                                        Else
                                            MsgBox("Please input serial key")
                                        End If
                                    Else
                                        MsgBox("Please select store in Account and Store settings tab")
                                    End If
                                Else
                                    MsgBox("Franchisee's Account must be valid first")
                                End If
                            Else
                                MsgBox("Please fill up all fields in Developer Information Settings")
                            End If
                        Else
                            MsgBox("Please fill up all fields in Additional Settings")
                        End If
                    Else
                        MsgBox("Save Cloud connection first")
                    End If
                Else
                    MsgBox("Invalid Cloud Connection")
                End If
            Else
                MsgBox("Save local connection first")
            End If
        Else
            MsgBox("Invalid Local Connection")
        End If
    End Sub
    Dim ValidProductKey As Boolean
    Private Sub SerialKey()
        Try
            Dim sql = "SELECT serial_key FROM admin_serialkeys WHERE active = 0 AND serial_key = '" & Trim(TextBoxProdKey.Text) & "'"
            Dim cloudcmd As MySqlCommand = New MySqlCommand(sql, TestCloudConnection)
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cloudcmd)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                ValidProductKey = True
            Else
                ValidProductKey = False
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim threadListActivation As List(Of Thread) = New List(Of Thread)
    Dim ThreadActivation As Thread
    Private Sub BackgroundWorkerACTIVATION_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerACTIVATION.DoWork
        Try
            Thread.Sleep(50)
            For i = 0 To 20
                BackgroundWorkerACTIVATION.ReportProgress(i)
                If i = 0 Then
                    ThreadActivation = New Thread(AddressOf SerialKey)
                    ThreadActivation.Start()
                    threadListActivation.Add(ThreadActivation)
                End If
            Next
            For Each t In threadListActivation
                t.Join()
            Next
            For i = 20 To 100
                BackgroundWorkerACTIVATION.ReportProgress(i)
                If i = 20 Then
                    If ValidProductKey = True Then
                        ThreadActivation = New System.Threading.Thread(AddressOf adminserialkey)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                        'For Each t In threadListActivation
                        '    t.Join()
                        'Next
                        ThreadActivation = New System.Threading.Thread(AddressOf adminoutlets)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                        'For Each t In threadListActivation
                        '    t.Join()
                        'Next
                        ThreadActivation = New System.Threading.Thread(AddressOf insertintocloud)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                        'For Each t In threadListActivation
                        '    t.Join()
                        'Next
                        ThreadActivation = New System.Threading.Thread(AddressOf insertintolocaloutlets)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                        'For Each t In threadListActivation
                        '    t.Join()
                        'Next
                        ThreadActivation = New System.Threading.Thread(AddressOf InsertLocalMasterList)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                        'For Each t In threadListActivation
                        '    t.Join()
                        'Next
                        '===
                        ThreadActivation = New System.Threading.Thread(AddressOf GetCategories)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                        'For Each t In threadListActivation
                        '    t.Join()
                        'Next
                        ThreadActivation = New System.Threading.Thread(AddressOf GetProducts)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                        'For Each t In threadListActivation
                        '    t.Join()
                        'Next
                        ThreadActivation = New System.Threading.Thread(AddressOf GetInventory)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                        'For Each t In threadListActivation
                        '    t.Join()
                        'Next
                        ThreadActivation = New System.Threading.Thread(AddressOf GetFormula)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                    End If
                End If
            Next
            For Each t In threadListActivation
                t.Join()
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorkerACTIVATION_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorkerACTIVATION.ProgressChanged
        ProgressBar5.Value = e.ProgressPercentage
    End Sub
    Private Sub BackgroundWorkerACTIVATION_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorkerACTIVATION.RunWorkerCompleted
        If ValidProductKey = True Then
            BackgroundWorker5.WorkerReportsProgress = True
            BackgroundWorker5.WorkerSupportsCancellation = True
            BackgroundWorker5.RunWorkerAsync()
        Else
            MsgBox("Invalid Product key")
        End If
        TextboxEnableability(GroupBox12, True)
        ButtonEnableability(GroupBox12, True)
    End Sub
    Private Sub adminserialkey()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Updating server(serial key)...." & vbNewLine
            Dim table = "admin_serialkeys"
            Dim fields = " active = 1 "
            Dim where = " serial_key = '" & TextBoxProdKey.Text & "'"
            Dim sql = "UPDATE " + table + " SET " + fields + " WHERE " & where
            Dim cloudcmd As MySqlCommand = New MySqlCommand(sql, TestCloudConnection)
            cloudcmd.ExecuteNonQuery()
            RichTextBox1.Text = RichTextBox1.Text & "Done!..." & vbNewLine
        Catch ex As Exception
            RichTextBox1.Text = RichTextBox1.Text & "Failed to Update serial key...." & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub adminoutlets()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Updating server(outlet)...." & vbNewLine
            Dim table = "admin_outlets"
            Dim fields = " active = 2 "
            Dim where = " store_id = " & DataGridViewOutlets.SelectedRows(0).Cells(0).Value.ToString
            Dim sql = "UPDATE " + table + " SET " + fields + " WHERE " & where
            Dim cloudcmd As MySqlCommand = New MySqlCommand(sql, TestCloudConnection)
            cloudcmd.ExecuteNonQuery()
            RichTextBox1.Text = RichTextBox1.Text & "Done!..." & vbNewLine
        Catch ex As Exception
            RichTextBox1.Text = RichTextBox1.Text & "Failed to update outlets...." & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub insertintocloud()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Inserting server masterlist data...." & vbNewLine
            Dim table1 = "admin_masterlist"
            Dim fields1 = " (`masterlist_username`,`masterlist_password`,`client_guid`,`client_product_key`,`user_id`,`active`,`client_store_id`)"
            Dim value1 = "('" & TextBoxFrancUser.Text & "'
                     ,'" & TextBoxFrancPass.Text & "'
                     ,'" & UserGUID & "'
                     ,'" & TextBoxProdKey.Text & "'
                     ,'" & UserID & "'
                     ," & 1 & "
                     ,'" & DataGridViewOutlets.SelectedRows(0).Cells(0).Value & "')"
            Dim sql = "INSERT INTO " + table1 + fields1 + " VALUES " + value1
            Dim cloudcmd As MySqlCommand = New MySqlCommand(sql, TestCloudConnection)
            cloudcmd.ExecuteNonQuery()
            RichTextBox1.Text = RichTextBox1.Text & "Done!..." & vbNewLine
        Catch ex As Exception
            MsgBox(ex.ToString)
            RichTextBox1.Text = RichTextBox1.Text & "Failed to insert masterlist...." & vbNewLine
        End Try
    End Sub
    Private Sub insertintolocaloutlets()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Getting server's outlet data...." & vbNewLine
            Dim Municipalityname
            Dim ProvinceName
            With DataGridViewOutletDetails
                Dim sql1 As String = "SELECT mn_name FROM admin_municipality WHERE mn_id = " & .Rows(0).Cells(8).Value.ToString
                Dim cloudcmd1 As MySqlCommand = New MySqlCommand(sql1, TestCloudConnection)
                Dim da1 As MySqlDataAdapter = New MySqlDataAdapter(cloudcmd1)
                Dim dt1 As DataTable = New DataTable
                da1.Fill(dt1)
                Municipalityname = dt1(0)(0)
                '=======================================================
                Dim sql2 As String = "SELECT province FROM admin_province WHERE add_id = " & .Rows(0).Cells(9).Value.ToString
                Dim cloudcmd2 As MySqlCommand = New MySqlCommand(sql2, TestCloudConnection)
                Dim da2 As MySqlDataAdapter = New MySqlDataAdapter(cloudcmd2)
                Dim dt2 As DataTable = New DataTable
                da2.Fill(dt2)
                ProvinceName = dt2(0)(0)
                RichTextBox1.Text = RichTextBox1.Text & "Done!..." & vbNewLine
                Dim table = "admin_outlets"
                Dim fields = "(`store_id`, `brand_name`, `store_name`, `user_guid`, `location_name`, `postal_code`, `address`, `Barangay`, `municipality`, `municipality_name`, `province`, `province_name`, `tin_no`, `tel_no`, `active`, `created_at`, `MIN`, `MSN`, `PTUN`)"
                Dim value = "(" & .Rows(0).Cells(0).Value.ToString & "                       
                        ,'" & .Rows(0).Cells(1).Value.ToString & "'
                        ,'" & .Rows(0).Cells(2).Value.ToString & "'
                        ,'" & .Rows(0).Cells(3).Value.ToString & "'                    
                        ,'" & .Rows(0).Cells(4).Value.ToString & "'
                        ,'" & .Rows(0).Cells(5).Value.ToString & "'                         
                        ,'" & .Rows(0).Cells(6).Value.ToString & "'
                        ,'" & .Rows(0).Cells(7).Value.ToString & "'
                        ,'" & .Rows(0).Cells(8).Value.ToString & "'
                        ,'" & Municipalityname & "' 
                        ,'" & .Rows(0).Cells(9).Value.ToString & "'
                        ,'" & ProvinceName & "'               
                        ,'" & .Rows(0).Cells(10).Value.ToString & "'
                        ,'" & .Rows(0).Cells(11).Value.ToString & "'
                        ," & 1 & "
                        ,'" & returndatetimeformat(Now) & "'
                        ,'" & .Rows(0).Cells(12).Value.ToString & "'
                        ,'" & .Rows(0).Cells(13).Value.ToString & "'
                        ,'" & .Rows(0).Cells(14).Value.ToString & "')"
                RichTextBox1.Text = RichTextBox1.Text & "Inserting outlet data...." & vbNewLine
                Dim sql = "INSERT INTO " & table & fields & " VALUES " & value
                Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection)
                cmd.ExecuteNonQuery()
                RichTextBox1.Text = RichTextBox1.Text & "Done!..." & vbNewLine
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertLocalMasterList()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Inserting master List data...." & vbNewLine
            messageboxappearance = False
            Dim table1 = "admin_masterlist"
            Dim fields1 = " (`masterlist_username`,`masterlist_password`,`client_guid`,`client_product_key`,`user_id`,`active`,`client_store_id`)"
            Dim value1 = "('" & TextBoxFrancUser.Text & "'
                     ,'" & TextBoxFrancPass.Text & "'
                     ,'" & UserGUID & "'
                     ,'" & TextBoxProdKey.Text & "'
                     ,'" & UserID & "'
                     ," & 1 & "
                     ,'" & DataGridViewOutlets.SelectedRows(0).Cells(0).Value.ToString & "')"
            Dim sql = "INSERT INTO " & table1 & fields1 & " VALUES " & value1
            Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection)
            cmd.ExecuteNonQuery()
            RichTextBox1.Text = RichTextBox1.Text & "Done!..." & vbNewLine
        Catch ex As Exception
            MsgBox("Contact Administrator Error Code: 3.0")
        End Try
    End Sub
    Private Sub GLOBAL_SELECT_ALL_FUNCTION_CLOUD(tbl As String, flds As String, datagrid As DataGridView)
        Try
            Dim sql = "SELECT " & flds & " FROM " & table
            Dim cmd As MySqlCommand = New MySqlCommand(sql, TestCloudConnection())
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            datagrid.DataSource = dt
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim threadLISTINSERPROD As List(Of Thread) = New List(Of Thread)
    Private Sub BackgroundWorker5_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker5.DoWork
        Try
            For i = 0 To 100
                Label22.Text = "Please Wait " & i & " %"
                BackgroundWorker5.ReportProgress(i)
                Thread.Sleep(20)
                If i = 0 Then
                    thread1 = New System.Threading.Thread(AddressOf InsertToProducts)
                    thread1.Start()
                    threadLISTINSERPROD.Add(thread1)
                    For Each t In threadLISTINSERPROD
                        t.Join()
                    Next
                    thread1 = New System.Threading.Thread(AddressOf InsertToInventory)
                    thread1.Start()
                    threadLISTINSERPROD.Add(thread1)
                    For Each t In threadLISTINSERPROD
                        t.Join()
                    Next
                    thread1 = New System.Threading.Thread(AddressOf InsertToCategories)
                    thread1.Start()
                    threadLISTINSERPROD.Add(thread1)
                    For Each t In threadLISTINSERPROD
                        t.Join()
                    Next
                    thread1 = New System.Threading.Thread(AddressOf InsertToFormula)
                    thread1.Start()
                    threadLISTINSERPROD.Add(thread1)
                    For Each t In threadLISTINSERPROD
                        t.Join()
                    Next
                End If
            Next
            For Each t In threadLISTINSERPROD
                t.Join()
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim optimizeorrepair As Integer = 0
    Private Sub ButtonMaintenance_Click(sender As Object, e As EventArgs) Handles ButtonMaintenance.Click
        optimizeorrepair = 1
        BackgroundWorkerABTDB.WorkerReportsProgress = True
        BackgroundWorkerABTDB.WorkerSupportsCancellation = True
        BackgroundWorkerABTDB.RunWorkerAsync()
    End Sub
    Private Sub ButtonRepair_Click(sender As Object, e As EventArgs) Handles ButtonRepair.Click
        optimizeorrepair = 0
        BackgroundWorkerABTDB.WorkerReportsProgress = True
        BackgroundWorkerABTDB.WorkerSupportsCancellation = True
        BackgroundWorkerABTDB.RunWorkerAsync()
    End Sub
    Private Sub ButtonExport_Click(sender As Object, e As EventArgs) Handles ButtonExport.Click
        optimizeorrepair = 2
        BackgroundWorkerABTDB.WorkerReportsProgress = True
        BackgroundWorkerABTDB.WorkerSupportsCancellation = True
        BackgroundWorkerABTDB.RunWorkerAsync()
    End Sub
    Dim bat As String
    Dim threadABTDB As Thread
    Dim threadListABTDB As List(Of Thread) = New List(Of Thread)
    Private Sub BackgroundWorkerABTDB_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerABTDB.DoWork
        Try
            If optimizeorrepair = 0 Then
                bat = "repair.bat"
                threadABTDB = New Thread(AddressOf StartCommandLine)
                threadABTDB.Start()
            ElseIf optimizeorrepair = 1 Then
                bat = "optimize.bat"
                threadABTDB = New Thread(AddressOf StartCommandLine)
                threadABTDB.Start()
            ElseIf optimizeorrepair = 2 Then
                bat = "backup.bat"
                threadABTDB = New Thread(AddressOf StartCommandLine)
                threadABTDB.Start()
            End If
            For Each t In threadListABTDB
                t.Join()
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub StartCommandLine(ByVal batfile As String)
        Try
            batfile = bat
            Dim p As Process = New Process()
            Dim psi As ProcessStartInfo = New ProcessStartInfo()
            psi.FileName = "CMD.EXE"
            psi.Arguments = "/K " & batfile
            p.StartInfo = psi
            p.Start()
            p.WaitForExit()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorker5_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker5.RunWorkerCompleted
        Dim message As Integer = MessageBox.Show("Successfully Registered. Your system will automatically reboot after pressing OK button.", "Activated", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ValidProductKey = False
        TextboxEnableability(GroupBox12, True)
        ButtonEnableability(GroupBox12, True)
        Close()
        Loading.Show()
    End Sub
    Public Sub GetCategories()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Getting Categories...." & vbNewLine
            table = "admin_category"
            fields = "`category_name`, `brand_name`, `updated_at`, `origin`, `status`"
            GLOBAL_SELECT_ALL_FUNCTION_CLOUD(table, fields, DataGridViewCATEGORIES)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub GetProducts()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Getting Products...." & vbNewLine
            table = "admin_products_org"
            fields = "`product_id`, `product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`"
            GLOBAL_SELECT_ALL_FUNCTION_CLOUD(table, fields, DataGridViewPRODUCTS)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub GetInventory()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Getting Inventory...." & vbNewLine
            table = "admin_pos_inventory_org"
            fields = "`inventory_id`, `formula_id`, `product_ingredients`, `sku`, `stock_quantity`, `stock_total`, `stock_status`, `critical_limit`, `date_modified`"
            GLOBAL_SELECT_ALL_FUNCTION_CLOUD(table, fields, DataGridViewINVENTORY)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub GetFormula()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Getting Formula's...." & vbNewLine
            table = "admin_product_formula_org"
            fields = "`formula_id`, `product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin`"
            GLOBAL_SELECT_ALL_FUNCTION_CLOUD(table, fields, DataGridViewFORMULA)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertToProducts()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Inserting Productlist data...." & vbNewLine
            With DataGridViewPRODUCTS
                Dim cmdlocal As MySqlCommand
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmdlocal = New MySqlCommand("INSERT INTO loc_admin_products(`server_product_id`,`product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `guid`, `store_id`, `synced`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14)", TestLocalConnection())
                    cmdlocal.Parameters.Add("@0", MySqlDbType.Int32).Value = .Rows(i).Cells(0).Value.ToString()
                    cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                    cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    cmdlocal.Parameters.Add("@6", MySqlDbType.Int32).Value = .Rows(i).Cells(6).Value.ToString()
                    cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                    cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                    cmdlocal.Parameters.Add("@9", MySqlDbType.VarChar).Value = .Rows(i).Cells(9).Value.ToString()
                    cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value.ToString()
                    cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = returndatetimeformat(.Rows(i).Cells(11).Value.ToString())
                    cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = UserGUID
                    cmdlocal.Parameters.Add("@13", MySqlDbType.Int32).Value = DataGridViewOutlets.SelectedRows(0).Cells(0).Value
                    cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = "Synced"
                    cmdlocal.ExecuteNonQuery()
                Next
                RichTextBox1.Text = RichTextBox1.Text & "Done!..." & vbNewLine
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertToInventory()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Inserting Inventory list...." & vbNewLine
            With DataGridViewINVENTORY
                Dim cmdlocal As MySqlCommand
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmdlocal = New MySqlCommand("INSERT INTO loc_pos_inventory(`server_inventory_id`,`formula_id`, `product_ingredients`, `sku`, `stock_quantity`, `stock_total`, `stock_status`, `critical_limit`, `date_modified`, `guid`, `store_id`, `synced`, `server_date_modified`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12)", TestLocalConnection())
                    cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmdlocal.Parameters.Add("@1", MySqlDbType.Int64).Value = .Rows(i).Cells(1).Value.ToString()
                    cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmdlocal.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(4).Value.ToString()
                    cmdlocal.Parameters.Add("@5", MySqlDbType.Int64).Value = .Rows(i).Cells(5).Value.ToString()
                    cmdlocal.Parameters.Add("@6", MySqlDbType.Int64).Value = .Rows(i).Cells(6).Value.ToString()
                    cmdlocal.Parameters.Add("@7", MySqlDbType.Int64).Value = .Rows(i).Cells(7).Value.ToString()
                    cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = returndatetimeformat(.Rows(i).Cells(8).Value.ToString())
                    cmdlocal.Parameters.Add("@9", MySqlDbType.VarChar).Value = UserGUID
                    cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = DataGridViewOutlets.SelectedRows(0).Cells(0).Value
                    cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = "Synced"
                    cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = returndatetimeformat(.Rows(i).Cells(8).Value.ToString())
                    cmdlocal.ExecuteNonQuery()
                Next
            End With
            RichTextBox1.Text = RichTextBox1.Text & "Done!..." & vbNewLine
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertToCategories()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Inserting Category list...." & vbNewLine
            With DataGridViewCATEGORIES
                Dim cmdlocal As MySqlCommand
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmdlocal = New MySqlCommand("INSERT INTO loc_admin_category( `category_name`, `brand_name`, `updated_at`, `origin`, `status`)
                                             VALUES (@0, @1, @2, @3, @4)", TestLocalConnection())
                    cmdlocal.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(0).Value.ToString()
                    cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = returndatetimeformat(.Rows(i).Cells(2).Value.ToString())
                    cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmdlocal.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(4).Value.ToString()
                    cmdlocal.ExecuteNonQuery()
                Next
            End With
            RichTextBox1.Text = RichTextBox1.Text & "Done!..." & vbNewLine
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertToFormula()
        Try
            RichTextBox1.Text = RichTextBox1.Text & "Inserting Formula List...." & vbNewLine
            With DataGridViewFORMULA
                Dim cmdlocal As MySqlCommand
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmdlocal = New MySqlCommand("INSERT INTO loc_product_formula(`server_formula_id`, `product_ingredients`, `primary_unit`, `primary_value`, `secondary_unit`, `secondary_value`, `serving_unit`, `serving_value`, `no_servings`, `status`, `date_modified`, `unit_cost`, `origin`, `server_date_modified`, `store_id`, `guid`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15)", TestLocalConnection())
                    cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = .Rows(i).Cells(4).Value.ToString()
                    cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = .Rows(i).Cells(5).Value.ToString()
                    cmdlocal.Parameters.Add("@6", MySqlDbType.VarChar).Value = .Rows(i).Cells(6).Value.ToString()
                    cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = .Rows(i).Cells(7).Value.ToString()
                    cmdlocal.Parameters.Add("@8", MySqlDbType.VarChar).Value = .Rows(i).Cells(8).Value.ToString()
                    cmdlocal.Parameters.Add("@9", MySqlDbType.Int64).Value = .Rows(i).Cells(9).Value.ToString()
                    cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = returndatetimeformat(.Rows(i).Cells(10).Value.ToString())
                    cmdlocal.Parameters.Add("@11", MySqlDbType.Decimal).Value = .Rows(i).Cells(11).Value.ToString()
                    cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                    cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = returndatetimeformat(.Rows(i).Cells(10).Value.ToString())
                    cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = DataGridViewOutlets.SelectedRows(0).Cells(0).Value
                    cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = UserGUID
                    cmdlocal.ExecuteNonQuery()
                Next
                RichTextBox1.Text = RichTextBox1.Text & "Done!..." & vbNewLine
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles ButtonSaveLocalCon.Click
        Try
            If My.Settings.ValidLocalConn = True Then
                Dim FolderName As String = "Innovention"
                Dim path = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                CreateFolder(path, FolderName)
                BTNSaveLocalConn = True
                MsgBox(BTNSaveLocalConn)
            Else
                MsgBox("Connection must be valid")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub BackgroundWorker5_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker5.ProgressChanged
        ProgressBar6.Value = e.ProgressPercentage
    End Sub

    'Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
    '    InsertToProducts()
    '    InsertToInventory()
    '    InsertToCategories()
    '    InsertToFormula()
    'End Sub

    'Private Sub Button8_Click_1(sender As Object, e As EventArgs) Handles Button8.Click
    '    GetCategories()
    '    GetProducts()
    '    GetInventory()
    '    GetFormula()
    'End Sub
End Class