Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.IO
Imports System.Text
Imports System.Globalization
Imports System.Drawing.Imaging
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
    Dim Autobackup As Boolean = False
    Dim ConfirmAdditionalSettings As Boolean = False
    Dim ConfirmDevInfoSettings As Boolean = False
    Dim POSVersion As String = ""
    Dim LOCALCONNDATA As Boolean = False
    Dim CLOUDCONDATA As Boolean = False
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
            ValidCloudConnection = False
            ValidLocalConnection = False
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
                ValidLocalConnection = True
                LOCALCONNDATA = True
            End If
        Catch ex As Exception
            ValidLocalConnection = False
            LOCALCONNDATA = False
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
                ValidCloudConnection = True
                CLOUDCONDATA = True
            End If
        Catch ex As Exception
            ValidCloudConnection = False
            CLOUDCONDATA = False
        End Try
        Return cloudconn
    End Function
    Private Sub CreateFolder(Path As String, FolderName As String, Optional ByVal Attributes As System.IO.FileAttributes = IO.FileAttributes.Normal)
        Try
            My.Computer.FileSystem.CreateDirectory(Path & "\" & FolderName)
            If Not Attributes = IO.FileAttributes.Normal Then
                My.Computer.FileSystem.GetDirectoryInfo(Path & "\" & FolderName).Attributes = Attributes
            End If
            CreateUserConfig(Path, "user.config", FolderName)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub CreateUserConfig(path As String, FileName As String, FolderName As String, Optional ByVal Attributes As System.IO.FileAttributes = IO.FileAttributes.Normal)
        Try
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
            If LOCALCONNDATA = False Then
                MsgBox("Saved")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
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
        ValidLocalConnection = False
    End Sub
    Private Sub Button8_Click_1(sender As Object, e As EventArgs) Handles ButtonEditLocal.Click
        Try
            BTNSaveLocalConn = False
            TextboxEnableability(Panel5, True)
            ButtonClearLocal.Enabled = True
            ButtonTestLocConn.Enabled = True
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
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
        If ValidLocalConnection = False Then
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
        If ValidCloudConnection = False Then
            ChangeProgBarColor(ProgressBar2, ProgressBarColor.Yellow)
            LabelCloud.Text = "Invalid connection please try again."
        Else
            ChangeProgBarColor(ProgressBar2, ProgressBarColor.Green)
            LabelCloud.Text = "Connected successfully!"
        End If
        TextboxEnableability(Panel9, True)
        ButtonEnableability(Panel9, True)
    End Sub
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles ButtonEditCloud.Click
        Try
            TextboxEnableability(Panel9, True)
            ButtonClearCloud.Enabled = True
            ButtonTestCloudConn.Enabled = True
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim FillUp As Boolean = False
    Private Sub ButtonSaveCloudConn_Click(sender As Object, e As EventArgs) Handles ButtonSaveCloudConn.Click
        Try
            table = "loc_settings"
            where = "settings_id = 1"
            If ValidLocalConnection = True Then
                If ValidCloudConnection = True Then
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
                        If CLOUDCONDATA = False Then
                            MsgBox("Saved!")
                        End If
                    Else
                        fields = "(C_Server, C_Username, C_Password, C_Database, C_Port, S_Zreading)"
                        value = "('" & ConvertToBase64(Trim(TextBoxCloudServer.Text)) & "'
                     ,'" & ConvertToBase64(Trim(TextBoxCloudUsername.Text)) & "'
                     ,'" & ConvertToBase64(Trim(TextBoxCloudPassword.Text)) & "'
                     ,'" & ConvertToBase64(Trim(TextBoxCloudDatabase.Text)) & "'
                     ,'" & ConvertToBase64(Trim(TextBoxCloudPort.Text)) & "'
                     ,'" & Format(Now(), "yyyy-MM-dd") & "')"
                        sql = "INSERT INTO " & table & " " & fields & " VALUES " & value
                        cmd = New MySqlCommand(sql, TestLocalConnection)
                        cmd.ExecuteNonQuery()
                        If CLOUDCONDATA = False Then
                            MsgBox("Saved!")
                        End If
                    End If
                    LoadDefaultSettingsAdd()
                    LoadDefaultSettingsDev()
                    TextboxEnableability(Panel9, False)
                    BTNSaveCloudConn = True
                    ButtonClearCloud.Enabled = False
                    ButtonTestCloudConn.Enabled = False
                    FillUp = True
                    SaveDevInfo()
                    SaveAddSettings()
                Else
                    FillUp = False
                    BTNSaveCloudConn = False
                    MsgBox("Connection must be valid")
                End If
            Else
                MsgBox("Local connection must be valid first.")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim FooterInfo As String = ""
    Private Sub LoadDefaultSettingsAdd()
        Try
            If ValidCloudConnection = True And ValidLocalConnection = True Then
                If System.IO.File.Exists(My.Settings.LocalConnectionPath) Then
                    Dim EXPORTPATH = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Innovention"
                    sql = "SELECT `A_Tax`, `A_SIFormat`, `A_Terminal_No`, `A_ZeroRated`, `S_Batter`, `S_Brownie_Mix`, `S_Upgrade_Price_Add` , `S_Update_Version` , `S_Waffle_Bag`, `S_Packets`, `P_Footer_Info` FROM admin_settings_org WHERE settings_id = 1"
                    Dim cmd As MySqlCommand = New MySqlCommand(sql, TestCloudConnection)
                    Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                    Dim dt As DataTable = New DataTable
                    da.Fill(dt)
                    If dt.Rows.Count > 0 Then
                        TextBoxExportPath.Text = EXPORTPATH
                        TextBoxTax.Text = dt(0)(0)
                        TextBoxSINumber.Text = dt(0)(1)
                        TextBoxTerminalNo.Text = dt(0)(2)
                        If dt(0)(3) = "0" Then
                            RadioButtonNO.Checked = True
                        ElseIf dt(0)(3) = "1" Then
                            RadioButtonYES.Checked = False
                        End If
                        TextBoxBATTERID.Text = dt(0)(4)
                        TextBoxBROWNIEID.Text = dt(0)(5)
                        TextBoxBROWNIEPRICE.Text = dt(0)(6)
                        My.Settings.Version = dt(0)(7)
                        My.Settings.Save()
                        POSVersion = dt(0)(7)
                        TextBoxWaffleBag.Text = dt(0)(8)
                        TextBoxSugarPackets.Text = dt(0)(9)
                        ConfirmAdditionalSettings = True
                        FooterInfo = dt(0)(10)
                    Else
                        ConfirmAdditionalSettings = False
                    End If
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadDefaultSettingsDev()
        Try
            If ValidCloudConnection = True And ValidLocalConnection = True Then
                sql = "SELECT `Dev_Company_Name`, `Dev_Address`, `Dev_Tin`, `Dev_Accr_No`, `Dev_Accr_Date_Issued`, `Dev_Accr_Valid_Until`, `Dev_PTU_No`, `Dev_PTU_Date_Issued`, `Dev_PTU_Valid_Until` FROM admin_settings_org WHERE settings_id = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, TestCloudConnection)
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    TextBoxDevname.Text = dt(0)(0)
                    TextBoxDevAdd.Text = dt(0)(1)
                    TextBoxDevTIN.Text = dt(0)(2)
                    TextBoxDevAccr.Text = dt(0)(3)
                    DateTimePicker1ACCRDI.Text = dt(0)(4)
                    DateTimePicker2ACCRVU.Text = dt(0)(5)
                    TextBoxDEVPTU.Text = dt(0)(6)
                    DateTimePicker4PTUDI.Text = dt(0)(7)
                    DateTimePickerPTUVU.Text = dt(0)(8)
                    ConfirmDevInfoSettings = True
                Else
                    ConfirmDevInfoSettings = False
                End If
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
            If ValidLocalConnection = True Then
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
                    ValidCloudConnection = True
                    CLOUDCONDATA = True
                Else
                    ValidCloudConnection = False
                    CLOUDCONDATA = False
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadAutoBackup()
        Try
            If ValidLocalConnection = True Then
                Dim sql = "SELECT `S_BackupInterval`, `S_BackupDate` FROM loc_settings WHERE settings_id = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection)
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                For Each row As DataRow In dt.Rows
                    If row("S_BackupInterval") <> "" Then
                        If row("S_BackupDate") <> "" Then
                            Autobackup = True
                            Dim interval = row("S_BackupInterval")
                            If interval = "1" Then
                                RadioButtonDaily.Checked = True
                            ElseIf interval = "2" Then
                                RadioButtonWeekly.Checked = True
                            ElseIf interval = "3" Then
                                RadioButtonMonthly.Checked = True
                            ElseIf interval = "4" Then
                                RadioButtonYearly.Checked = True
                            End If
                        Else
                            Autobackup = False
                            Exit For
                        End If
                    Else
                        Autobackup = False
                        Exit For
                    End If
                Next
            Else
                Autobackup = False
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadAdditionalSettings()
        Try
            If ValidLocalConnection = True Then
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
                                        ConfirmAdditionalSettings = True
                                    Else
                                        ConfirmAdditionalSettings = False
                                        Exit For
                                    End If
                                Else
                                    ConfirmAdditionalSettings = False
                                    Exit For
                                End If
                            Else
                                ConfirmAdditionalSettings = False
                                Exit For
                            End If
                        Else
                            ConfirmAdditionalSettings = False
                            Exit For
                        End If
                    Else
                        ConfirmAdditionalSettings = False
                        Exit For
                    End If
                Next
            Else
                ConfirmAdditionalSettings = False
            End If
            My.Settings.Save()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub LoadDevInfo()
        Try
            If ValidLocalConnection = True Then
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
                                                        ConfirmDevInfoSettings = True
                                                    Else
                                                        ConfirmDevInfoSettings = False
                                                        Exit For
                                                    End If
                                                Else
                                                    ConfirmDevInfoSettings = False
                                                    Exit For
                                                End If
                                            Else
                                                ConfirmDevInfoSettings = False
                                                Exit For
                                            End If
                                        Else
                                            ConfirmDevInfoSettings = False
                                            Exit For
                                        End If
                                    Else
                                        ConfirmDevInfoSettings = False
                                        Exit For
                                    End If
                                Else
                                    ConfirmDevInfoSettings = False
                                    Exit For
                                End If
                            Else
                                ConfirmDevInfoSettings = False
                                Exit For
                            End If
                        Else
                            ConfirmDevInfoSettings = False
                            Exit For
                        End If
                    Else
                        ConfirmDevInfoSettings = False
                        Exit For
                    End If
                Next
            Else
                ConfirmDevInfoSettings = False
            End If
            My.Settings.Save()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Function LoadOutlets() As DataTable
        Dim CloudDT As DataTable = New DataTable
        Try
            Dim sql = "SELECT * FROM admin_outlets WHERE user_guid = '" & UserGUID & "' AND active = 1"
            Dim CloudCmd As MySqlCommand = New MySqlCommand(sql, TestCloudConnection)
            Dim CloudDa As MySqlDataAdapter = New MySqlDataAdapter(CloudCmd)
            CloudDa.Fill(CloudDT)

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return CloudDT
    End Function
    Private Sub GetLogo(BrandName)
        Try
            Dim Logo
            Dim sql = "SELECT brand_logo FROM admin_brand WHERE brand_name = '" & BrandName & "' "
            Dim CloudCmd As MySqlCommand = New MySqlCommand(sql, TestCloudConnection)
            Logo = CloudCmd.ExecuteScalar()
            RichTextBoxLogo.Text = Logo
            PictureBoxLogo.BackgroundImage = Base64ToImage(Logo)
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
        Application.Exit()

    End Sub
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles ButtonValidateAccount.Click
        If ValidCloudConnection = True Then
            If String.IsNullOrWhiteSpace(TextBoxFrancUser.Text) Then
                MsgBox("All fields are required")
            ElseIf String.IsNullOrWhiteSpace(TextBoxFrancPass.Text) Then
                MsgBox("All fields are required")
            Else
                If CheckForInternetConnection() = True Then
                    TextboxEnableability(Panel14, False)
                    ButtonEnableability(Panel14, False)
                    ClearTextBox(Panel15)
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

    Dim AccountExist As Boolean
    Dim UserGUID As String
    Dim UserStoreID As Integer
    Dim CloudConn2 As MySqlConnection
    Public Sub checkacc()
        Try
            AccountExist = False
            FranchiseeStoreValidation = False
            sql = "SELECT user_guid, user_id FROM admin_user WHERE user_name = '" & TextBoxFrancUser.Text & "' AND user_pass = '" & ConvertPassword(TextBoxFrancPass.Text) & "' AND user_role = 'Client' AND status = 1; "
            cmd = New MySqlCommand(sql, TestCloudConnection())
            Dim DataadapterCheckAcc As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            Dim DatatableCheckAcc As DataTable = New DataTable
            DataadapterCheckAcc.Fill(DatatableCheckAcc)
            For Each row As DataRow In DatatableCheckAcc.Rows
                If row("user_guid") <> "" Then
                    If row("user_id") <> 0 Then
                        AccountExist = True
                        UserGUID = row("user_guid")
                        UserID = row("user_id")
                    Else
                        AccountExist = False
                        UserGUID = ""
                        Exit For
                    End If
                Else
                    AccountExist = False
                    UserGUID = ""
                    Exit For

                End If
            Next
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
            TextboxEnableability(Panel14, False)
            ButtonEnableability(Panel14, True)
            ChangeProgBarColor(ProgressBar3, ProgressBarColor.Green)
            LabelAccCheck.Text = "Complete!"
        Else
            ChangeProgBarColor(ProgressBar3, ProgressBarColor.Yellow)
            TextboxEnableability(Panel14, True)
            ButtonEnableability(Panel14, True)
            LabelAccCheck.Text = "Invalid franchisee's Account."
        End If
    End Sub
    Private Sub BackgroundWorker4_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker4.DoWork
        Try
            For i = 0 To 100
                BackgroundWorker4.ReportProgress(i)
                LabelAccCheck.Text = "Getting Account information " & i & " %"
                If i = 0 Then
                    thread1 = New Thread(AddressOf LoadOutlets)
                    thread1.Start()
                    threadList.Add(thread1)
                End If
                Thread.Sleep(20)
            Next
            For Each t In threadList
                t.Join()
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub BackgroundWorker4_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker4.ProgressChanged
        ProgressBar3.Value = e.ProgressPercentage
    End Sub
    Private Sub BackgroundWorker4_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker4.RunWorkerCompleted
        With DataGridViewOutlets
            .DataSource = LoadOutlets()
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
            LabelAccCheck.Text = "Complete!"
        End With
    End Sub
    Private Sub DataGridViewOutlets_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewOutlets.CellClick
        ButtonSelectOutlet.PerformClick()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ButtonSelectOutlet.Click
        Try
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
                GetLogo(TextBoxBrandname.Text)
            Else
                FranchiseeStoreValidation = False
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub RDButtons(tf As Boolean)
        Try
            RadioButtonNO.Enabled = tf
            RadioButtonYES.Enabled = tf
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub ButtonEdit_Click(sender As Object, e As EventArgs) Handles ButtonEditAddSettings.Click
        TextboxEnableability(GroupBox10, True)
        RDButtons(True)
        ButtonGetExportPath.Enabled = True
        FillUp = False
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles ButtonSaveAddSettings.Click
        FillUp = False
        SaveAddSettings()
    End Sub
    Private Sub SaveAddSettings()
        Try
            Dim RButton As Integer
            Dim Tax = Val(TextBoxTax.Text) / 100
            If TextboxIsEmpty(GroupBox10) = True Then
                If ValidLocalConnection = True Then
                    Dim table = "loc_settings"
                    Dim fields = "A_Export_Path, A_Tax, A_SIFormat, A_Terminal_No, A_ZeroRated, S_Zreading, S_Batter, S_Brownie_Mix, S_Upgrade_Price_Add, S_Update_Version, S_Waffle_Bag , S_Packets , P_Footer_Info"
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
                        Dim fields1 = "A_Export_Path = '" & ConvertToBase64(Trim(TextBoxExportPath.Text)) & "', A_Tax = '" & Tax & "' , A_SIFormat = '" & Trim(TextBoxSINumber.Text) & "' , A_Terminal_No = '" & Trim(TextBoxTerminalNo.Text) & "' , A_ZeroRated = '" & RButton & "', S_Zreading = '" & Format(Now(), "yyyy-MM-dd") & "' , S_Batter = '" & Trim(TextBoxBATTERID.Text) & "', S_Brownie_Mix = '" & Trim(TextBoxBROWNIEID.Text) & "', S_Upgrade_Price_Add = '" & Trim(TextBoxBROWNIEPRICE.Text) & "' , `S_Waffle_Bag` = '" & Trim(TextBoxWaffleBag.Text) & "' , `S_Packets` = '" & Trim(TextBoxSugarPackets.Text) & "' , S_Update_Version = '" & POSVersion & "', P_Footer_Info = '" & FooterInfo & "'"
                        sql = "UPDATE " & table & " SET " & fields1 & " WHERE " & where
                        cmd = New MySqlCommand(sql, TestLocalConnection)
                        cmd.ExecuteNonQuery()
                        If FillUp = True Then
                        Else
                            MsgBox("Saved!")
                        End If
                    Else
                        Dim fields2 = "(A_Export_Path, A_Tax, A_SIFormat, A_Terminal_No, A_ZeroRated, S_Zreading, S_Batter, S_Brownie_Mix, S_Upgrade_Price_Add , S_Update_Version , S_Waffle_Bag , S_Packets, P_Footer_Info)"
                        Dim value = "('" & ConvertToBase64(Trim(TextBoxExportPath.Text)) & "'
                     ,'" & Tax & "'
                     ,'" & Trim(TextBoxSINumber.Text) & "'
                     ,'" & Trim(TextBoxTerminalNo.Text) & "'
                     ,'" & RButton & "'
                     ,'" & Format(Now(), "yyyy-MM-dd") & "'
                     ,'" & Trim(TextBoxBATTERID.Text) & "'
                     ,'" & Trim(TextBoxBROWNIEID.Text) & "'
                     ,'" & Trim(TextBoxBROWNIEPRICE.Text) & "'
                     ,'" & POSVersion & "'
                     ,'" & Trim(TextBoxWaffleBag.Text) & "'
                     ,'" & Trim(TextBoxSugarPackets.Text) & "')
                     ,'" & FooterInfo & "')"

                        sql = "INSERT INTO " & table & " " & fields2 & " VALUES " & value
                        cmd = New MySqlCommand(sql, TestLocalConnection)
                        cmd.ExecuteNonQuery()
                        If FillUp = True Then
                        Else
                            MsgBox("Saved!")
                        End If
                    End If
                    ConfirmAdditionalSettings = True
                    TextboxEnableability(GroupBox10, False)
                    RDButtons(False)
                    ButtonGetExportPath.Enabled = False
                Else
                    ConfirmAdditionalSettings = False
                    MsgBox("Invalid Local Connection.")
                End If
            Else
                ConfirmAdditionalSettings = False
                MsgBox("All fields are required.")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles ButtonGetExportPath.Click
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
                    thread1 = New Thread(AddressOf TestLocalConnection)
                    thread1.Start()
                    threadList.Add(thread1)
                    For Each t In threadList
                        t.Join()
                    Next
                    thread1 = New Thread(AddressOf LoadCloudConn)
                    thread1.Start()
                    threadList.Add(thread1)
                    thread1 = New Thread(AddressOf LoadAutoBackup)
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
    Private Sub BackgroundWorkerLOAD_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorkerLOAD.RunWorkerCompleted
        If ValidLocalConnection = True Then
            ButtonSaveLocalCon.PerformClick()
            LOCALCONNDATA = False
        End If

        If ValidCloudConnection = True Then
            ButtonSaveCloudConn.PerformClick()
            CLOUDCONDATA = False
        End If
    End Sub
    Private Sub DatePickerState(tf As Boolean)
        Try
            DateTimePicker1ACCRDI.Enabled = tf
            DateTimePicker2ACCRVU.Enabled = tf
            DateTimePicker4PTUDI.Enabled = tf
            DateTimePickerPTUVU.Enabled = tf
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub ButtonEditDevSet_Click(sender As Object, e As EventArgs) Handles ButtonEditDevSet.Click
        TextboxEnableability(GroupBox11, True)
        DatePickerState(True)
        FillUp = False
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles ButtonSaveDevSettings.Click
        FillUp = False
        SaveDevInfo()
    End Sub
    Private Sub SaveDevInfo()
        Dim table = "loc_settings"
        Dim where = "settings_id = 1"
        If TextboxIsEmpty(GroupBox11) = True Then
            If ValidLocalConnection = True Then
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
                `Dev_Accr_Date_Issued`= '" & Format(DateTimePicker1ACCRDI.Value, "yyy-MM-dd") & "',
                `Dev_Accr_Valid_Until`= '" & Format(DateTimePicker2ACCRVU.Value, "yyyy-MM-dd") & "',
                `Dev_PTU_No`= '" & Trim(TextBoxDEVPTU.Text) & "',
                `Dev_PTU_Date_Issued`= '" & Format(DateTimePickerPTUVU.Value, "yyyy-MM-dd") & "',
                `Dev_PTU_Valid_Until`= '" & Format(DateTimePicker4PTUDI.Value, "yyyy-MM-dd") & "'"
                    sql = "UPDATE " & table & " SET " & fields1 & " WHERE " & where
                    cmd = New MySqlCommand(sql, TestLocalConnection)
                    cmd.ExecuteNonQuery()
                    ConfirmDevInfoSettings = True
                    If FillUp = True Then
                    Else
                        MsgBox("Saved!")
                    End If

                Else
                    Dim fields2 = "(Dev_Company_Name, Dev_Address, Dev_Tin, Dev_Accr_No, Dev_Accr_Date_Issued, Dev_Accr_Valid_Until, Dev_PTU_No, Dev_PTU_Date_Issued, Dev_PTU_Valid_Until)"
                    Dim value = "('" & Trim(TextBoxDevname.Text) & "'
                ,'" & Trim(TextBoxDevAdd.Text) & "'
                ,'" & Trim(TextBoxDevTIN.Text) & "'
                ,'" & Trim(TextBoxDevAccr.Text) & "'
                ,'" & Format(DateTimePicker1ACCRDI.Value, "yyyy-MM-dd") & "'
                ,'" & Format(DateTimePicker2ACCRVU.Value, "yyyy-MM-dd") & "'
                ,'" & Trim(TextBoxDEVPTU.Text) & "'
                ,'" & Format(DateTimePickerPTUVU.Value, "yyyy-MM-dd") & "'
                ,'" & Format(DateTimePicker4PTUDI.Value, "yyyy-MM-dd") & "')"
                    sql = "INSERT INTO " & table & " " & fields2 & " VALUES " & value
                    cmd = New MySqlCommand(sql, TestLocalConnection)
                    cmd.ExecuteNonQuery()
                    ConfirmDevInfoSettings = True
                    If FillUp = True Then
                    Else
                        MsgBox("Saved!")
                    End If

                End If
                TextboxEnableability(GroupBox11, False)
                DatePickerState(False)
            Else
                MsgBox("Invalid local connection")
                ConfirmDevInfoSettings = False
            End If
        Else
            MsgBox("All fields are required")
            ConfirmDevInfoSettings = False
        End If
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles ButtonActivate.Click
        If ValidLocalConnection = True Then
            If BTNSaveLocalConn = True Then
                If ValidCloudConnection = True Then
                    If Autobackup = True Then
                        If BTNSaveCloudConn = True Then
                            If ConfirmAdditionalSettings = True Then
                                If ConfirmDevInfoSettings = True Then
                                    If AccountExist = True Then
                                        If FranchiseeStoreValidation = True Then
                                            If Not String.IsNullOrWhiteSpace(TextBoxProdKey.Text) Then
                                                DataGridViewOutlets.Enabled = False
                                                TextboxEnableability(GroupBox12, False)
                                                ButtonEnableability(GroupBox12, False)
                                                BackgroundWorkerACTIVATION.WorkerReportsProgress = True
                                                BackgroundWorkerACTIVATION.WorkerSupportsCancellation = True
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
                        MsgBox("Automatic backup interval has not been defined")
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

    Dim threadListActivationProduct As List(Of Thread) = New List(Of Thread)
    Dim threadListActivationCategory As List(Of Thread) = New List(Of Thread)
    Dim threadListActivationFormula As List(Of Thread) = New List(Of Thread)
    Dim threadListActivationInventory As List(Of Thread) = New List(Of Thread)
    Dim threadListActivationPartners As List(Of Thread) = New List(Of Thread)
    Dim threadListActivationCoupons As List(Of Thread) = New List(Of Thread)

    Dim ThreadActivationProduct As Thread
    Dim ThreadActivationCategory As Thread
    Dim ThreadActivationFormula As Thread
    Dim ThreadActivationInventory As Thread
    Dim ThreadActivationPartners As Thread
    Dim ThreadActivationCoupons As Thread


    Private Sub BackgroundWorkerACTIVATION_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerACTIVATION.DoWork
        Try
            For i = 0 To 100
                Thread.Sleep(50)
                BackgroundWorkerACTIVATION.ReportProgress(i)
                If i = 0 Then
                    ThreadActivation = New Thread(AddressOf SerialKey)
                    ThreadActivation.Start()
                    threadListActivation.Add(ThreadActivation)
                    For Each t In threadListActivation
                        t.Join()
                    Next

                End If
                If i = 20 Then
                    If ValidProductKey = True Then
                        ThreadActivation = New Thread(AddressOf adminserialkey)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                        For Each t In threadListActivation
                            t.Join()
                        Next

                    End If
                End If
                If i = 40 Then
                    If ValidProductKey = True Then
                        ThreadActivation = New Thread(AddressOf adminoutlets)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                        For Each t In threadListActivation
                            t.Join()
                        Next
                    End If

                End If
                If i = 60 Then
                    If ValidProductKey = True Then
                        ThreadActivation = New Thread(AddressOf insertintocloud)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                        For Each t In threadListActivation
                            t.Join()
                        Next
                    End If

                End If
                If i = 80 Then
                    If ValidProductKey = True Then
                        ThreadActivation = New Thread(AddressOf insertintolocaloutlets)
                        ThreadActivation.Start()
                        threadListActivation.Add(ThreadActivation)
                        For Each t In threadListActivation
                            t.Join()
                        Next
                    End If
                End If
                If i = 100 Then
                    ThreadActivation = New Thread(AddressOf InsertLocalMasterList)
                    ThreadActivation.Start()
                    threadListActivation.Add(ThreadActivation)
                    For Each t In threadListActivation
                        t.Join()
                    Next
                    ThreadActivation = New Thread(AddressOf SaveLogo)
                    ThreadActivation.Start()
                    threadListActivation.Add(ThreadActivation)
                    For Each t In threadListActivation
                        t.Join()
                    Next
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
            TextboxEnableability(GroupBox12, True)
            ButtonEnableability(GroupBox12, True)
        End If

    End Sub
    Private Sub adminserialkey()
        Try
            TextBox1.Text += FullDate24HR() & " :    Updating cloud server's table(Product Key)." & vbNewLine
            Dim sql = "UPDATE admin_serialkeys SET active = @1 , date_used = @2 WHERE serial_key = @3"
            Dim cloudcmd As MySqlCommand = New MySqlCommand(sql, TestCloudConnection)
            cloudcmd.Parameters.Add("@1", MySqlDbType.Int64).Value = 1
            cloudcmd.Parameters.Add("@2", MySqlDbType.Text).Value = FullDate24HR()
            cloudcmd.Parameters.Add("@3", MySqlDbType.VarChar).Value = Trim(TextBoxProdKey.Text)
            cloudcmd.ExecuteNonQuery()
            TextBox1.Text += FullDate24HR() & " :    Complete(Product key table updated)." & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Updating of product key)." & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub adminoutlets()
        Try
            TextBox1.Text += FullDate24HR() & " :    Updating cloud server's table(Outlets)." & vbNewLine
            Dim table = "admin_outlets"
            Dim fields = " active = @1 "
            Dim where = " store_id = " & DataGridViewOutlets.SelectedRows(0).Cells(0).Value.ToString
            Dim sql = "UPDATE " + table + " SET " + fields + " WHERE " & where
            Dim cloudcmd As MySqlCommand = New MySqlCommand(sql, TestCloudConnection)
            cloudcmd.Parameters.Add("@1", MySqlDbType.Int64).Value = 2
            cloudcmd.ExecuteNonQuery()
            TextBox1.Text += FullDate24HR() & " :    Complete(Outlets table updated)." & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Updating of outlet)." & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim Datenow
    Public Sub insertintocloud()
        Try
            TextBox1.Text += FullDate24HR() & " :    Inserting data to cloud server's table(Masterlist)." & vbNewLine
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
            TextBox1.Text += FullDate24HR() & " :    Complete(Masterlist data inserted)." & vbNewLine
        Catch ex As Exception
            MsgBox(ex.ToString)
            TextBox1.Text += FullDate24HR() & " :    Failed(Masterlist data insertion(Cloud))." & vbNewLine
        End Try
    End Sub
    Private Sub insertintolocaloutlets()
        Try
            TextBox1.Text += FullDate24HR() & " :    Getting cloud server's outlet data." & vbNewLine
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
                TextBox1.Text += FullDate24HR() & " :    Complete(Fetching of outlet data)." & vbNewLine
                Dim table = "admin_outlets"
                Dim fields = "(`store_id`, `brand_name`, `store_name`, `user_guid`, `location_name`, `postal_code`, `address`, `Barangay`, `municipality`, `municipality_name`, `province`, `province_name`, `tin_no`, `tel_no`, `active`, `MIN`, `MSN`, `PTUN`, `created_at`)"
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
                        ,'" & .Rows(0).Cells(12).Value.ToString & "'
                        ,'" & .Rows(0).Cells(13).Value.ToString & "'
                        ,'" & .Rows(0).Cells(14).Value.ToString & "'
                        ,'" & FullDate24HR() & "')"
                TextBox1.Text += FullDate24HR() & " :    Inserting outlet data." & vbNewLine
                Dim sql = "INSERT INTO " & table & fields & " VALUES " & value
                Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection)
                cmd.ExecuteNonQuery()
                TextBox1.Text += FullDate24HR() & " :    Complete(Outlet data inserted)." & vbNewLine
            End With
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Outlet data insertion(Local))." & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub SaveLogo()
        Try
            Dim sql = "UPDATE loc_settings SET S_logo = '" & RichTextBoxLogo.Text & "' WHERE settings_id = 1"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox("Contact Administrator Error Code: 3.0")
        End Try
    End Sub
    Private Sub InsertLocalMasterList()
        Try
            TextBox1.Text += FullDate24HR() & " :    Inserting masterlist data." & vbNewLine
            messageboxappearance = False
            Dim table1 = "admin_masterlist"
            Dim fields1 = " (`masterlist_username`,`masterlist_password`,`client_guid`,`client_product_key`,`user_id`,`active`,`client_store_id`,`created_at`)"
            Dim value1 = "('" & TextBoxFrancUser.Text & "'
                     ,'" & TextBoxFrancPass.Text & "'
                     ,'" & UserGUID & "'
                     ,'" & TextBoxProdKey.Text & "'
                     ,'" & UserID & "'
                     ," & 1 & "
                     ,'" & DataGridViewOutlets.SelectedRows(0).Cells(0).Value.ToString & "'
                     ,'" & FullDate24HR() & "')"
            Dim sql = "INSERT INTO " & table1 & fields1 & " VALUES " & value1
            Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection)
            cmd.ExecuteNonQuery()
            TextBox1.Text += FullDate24HR() & " :    Complete(Masterlist data inserted)." & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Masterlist data insertion)." & vbNewLine
            MsgBox("Contact Administrator Error Code: 3.0")
        End Try
    End Sub
    Private Function GLOBAL_SELECT_ALL_FUNCTION_CLOUD(tbl As String, flds As String, datagrid As DataGridView) As DataTable
        datagrid.Rows.Clear()
        Dim dt As DataTable = New DataTable()
        Try
            Dim sql = "SELECT " & flds & " FROM " & table
            Dim cmd As MySqlCommand = New MySqlCommand(sql, TestCloudConnection())
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            da.Fill(dt)
            datagrid.ReadOnly = True
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        Return dt
    End Function

    Private Sub FillDgvProd()
        Try
            DataGridViewPRODUCTS.DataSource = DtCount
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim threadLISTINSERPROD As List(Of Thread) = New List(Of Thread)
    Private Sub BackgroundWorker5_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker5.DoWork
        Try
            For i = 0 To 100
                Label22.Text = "Please wait " & i & " %"
                BackgroundWorker5.ReportProgress(i)
                Thread.Sleep(20)
                If i = 0 Then
                    ThreadActivationCategory = New Thread(AddressOf GetCategories)
                    ThreadActivationCategory.Start()
                    threadListActivationCategory.Add(ThreadActivationCategory)
                    For Each t In threadListActivationCategory
                        t.Join()
                    Next
                End If
                If i = 10 Then
                    ThreadActivationInventory = New Thread(AddressOf GetInventory)
                    ThreadActivationInventory.Start()
                    threadListActivationInventory.Add(ThreadActivationInventory)
                    For Each t In threadListActivationInventory
                        t.Join()
                    Next
                End If
                If i = 20 Then
                    ThreadActivationFormula = New Thread(AddressOf GetFormula)
                    ThreadActivationFormula.Start()
                    threadListActivationFormula.Add(ThreadActivationFormula)
                    For Each t In threadListActivationFormula
                        t.Join()
                    Next
                End If

                If i = 30 Then
                    ThreadActivationCoupons = New Thread(AddressOf GetCoupons)
                    ThreadActivationCoupons.Start()
                    threadListActivationCoupons.Add(ThreadActivationCoupons)
                    For Each t In threadListActivationCoupons
                        t.Join()
                    Next
                End If
                If i = 40 Then
                    ThreadActivationPartners = New Thread(AddressOf GetPartners)
                    ThreadActivationPartners.Start()
                    threadListActivationPartners.Add(ThreadActivationPartners)
                    For Each t In threadListActivationPartners
                        t.Join()
                    Next
                End If
                If i = 50 Then
                    ThreadActivationProduct = New Thread(AddressOf GetProducts)
                    ThreadActivationProduct.Start()
                    threadListActivationProduct.Add(ThreadActivationProduct)
                    For Each t In threadListActivationProduct
                        t.Join()
                    Next
                End If
                If i = 60 Then
                    thread1 = New System.Threading.Thread(AddressOf FillDgvProd)
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
                    thread1 = New System.Threading.Thread(AddressOf InsertPartnersTransacton)
                    thread1.Start()
                    threadLISTINSERPROD.Add(thread1)
                    For Each t In threadLISTINSERPROD
                        t.Join()
                    Next
                    thread1 = New System.Threading.Thread(AddressOf InsertCoupons)
                    thread1.Start()
                    threadLISTINSERPROD.Add(thread1)
                    For Each t In threadLISTINSERPROD
                        t.Join()
                    Next
                    thread1 = New System.Threading.Thread(AddressOf InsertToProducts)
                    thread1.Start()
                    threadLISTINSERPROD.Add(thread1)
                    For Each t In threadLISTINSERPROD
                        t.Join()
                    Next
                End If
            Next
        Catch ex As Exception
            'MsgBox(ex.ToString)
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
        Dim logPath = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        Dim file As System.IO.StreamWriter
        file = My.Computer.FileSystem.OpenTextFileWriter(logPath & "\configmanagerslogs.txt", True)
        file.WriteLine(TextBox1.Text)
        file.Close()
        Close()
        ChooseLayout.Show()
    End Sub
    Public Sub GetCategories()
        Try
            TextBox1.Text += FullDate24HR() & " :    Getting cloud server's categories table." & vbNewLine
            table = "admin_category"
            fields = "*"
            Dim Datatablecat = GLOBAL_SELECT_ALL_FUNCTION_CLOUD(table, fields, DataGridViewCATEGORIES)
            For Each row As DataRow In Datatablecat.Rows
                DataGridViewCATEGORIES.Rows.Add(row("category_name"), row("brand_name"), row("updated_at"), row("origin"), row("status"))
            Next
            TextBox1.Text += FullDate24HR() & " :    Complete(Fetching of categories data)" & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Fetching of categories data)" & vbNewLine
            'MsgBox(ex.ToString)
        End Try
    End Sub
    Dim DtCount As DataTable
    Private Sub GetProducts()
        Try
            Try
                TextBox1.Text += FullDate24HR() & " :    Getting cloud server's products data." & vbNewLine
                Dim Connection As MySqlConnection = TestCloudConnection()
                Dim SqlCount = "SELECT COUNT(product_id) FROM admin_products_org"
                Dim CmdCount As MySqlCommand = New MySqlCommand(SqlCount, Connection)
                Dim result As Integer = CmdCount.ExecuteScalar
                DtCount = New DataTable
                DtCount.Columns.Add("product_id")
                DtCount.Columns.Add("product_sku")
                DtCount.Columns.Add("product_name")
                DtCount.Columns.Add("formula_id")
                DtCount.Columns.Add("product_barcode")
                DtCount.Columns.Add("product_category")
                DtCount.Columns.Add("product_price")
                DtCount.Columns.Add("product_desc")
                DtCount.Columns.Add("product_image")
                DtCount.Columns.Add("product_status")
                DtCount.Columns.Add("origin")
                DtCount.Columns.Add("date_modified")
                DtCount.Columns.Add("inventory_id")
                DtCount.Columns.Add("addontype")
                Dim DaCount As MySqlDataAdapter
                Dim FillDt As DataTable = New DataTable
                For a = 1 To result
                    Dim Query As String = "SELECT * FROM admin_products_org WHERE product_id = " & a
                    cmd = New MySqlCommand(Query, Connection)
                    DaCount = New MySqlDataAdapter(cmd)
                    FillDt = New DataTable
                    DaCount.Fill(FillDt)
                    For i As Integer = 0 To FillDt.Rows.Count - 1 Step +1
                        Dim Prod As DataRow = DtCount.NewRow
                        Prod("product_id") = FillDt(i)(0)
                        Prod("product_sku") = FillDt(i)(1)
                        Prod("product_name") = FillDt(i)(2)
                        Prod("formula_id") = FillDt(i)(3)
                        Prod("product_barcode") = FillDt(i)(4)
                        Prod("product_category") = FillDt(i)(5)
                        Prod("product_price") = FillDt(i)(6)
                        Prod("product_desc") = FillDt(i)(7)
                        Prod("product_image") = FillDt(i)(8)
                        Prod("product_status") = FillDt(i)(9)
                        Prod("origin") = FillDt(i)(10)
                        Prod("date_modified") = FillDt(i)(11)
                        Prod("inventory_id") = FillDt(i)(12)
                        Prod("addontype") = FillDt(i)(13)
                        DtCount.Rows.Add(Prod)
                    Next
                Next
                TextBox1.Text += FullDate24HR() & " :    Complete(Fetching of products data)" & vbNewLine
            Catch ex As Exception
                TextBox1.Text += FullDate24HR() & " :    Failed(Fetching of products data)" & vbNewLine
                MsgBox(ex.ToString)
            End Try
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub GetCoupons()
        Try
            TextBox1.Text += FullDate24HR() & " :    Getting cloud server's coupons data." & vbNewLine
            table = "admin_coupon"
            fields = "*"
            Dim DatatableCoupons = GLOBAL_SELECT_ALL_FUNCTION_CLOUD(table, fields, DataGridViewCoupons)
            For Each row As DataRow In DatatableCoupons.Rows
                DataGridViewCoupons.Rows.Add(row("Couponname_"), row("Desc_"), row("Discountvalue_"), row("Referencevalue_"), row("Type"), row("Bundlebase_"), row("BBValue_"), row("Bundlepromo_"), row("BPValue_"), row("Effectivedate"), row("Expirydate"))
            Next
            TextBox1.Text += FullDate24HR() & " :    Complete(Fetching of coupons data)" & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Fetching of coupons data)" & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub GetPartners()
        Try
            TextBox1.Text += FullDate24HR() & " :    Getting cloud server's partners data." & vbNewLine
            table = "admin_partners_transaction_org"
            fields = "*"
            Dim DatatablePartners = GLOBAL_SELECT_ALL_FUNCTION_CLOUD(table, fields, DataGridViewPartners)
            For Each row As DataRow In DatatablePartners.Rows
                DataGridViewPartners.Rows.Add(row("arrid"), row("bankname"), row("date_modified"), row("active"))
            Next
            TextBox1.Text += FullDate24HR() & " :    Complete(Fetching of partners data)" & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Fetching of partners data)" & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub GetInventory()
        Try
            TextBox1.Text += FullDate24HR() & " :    Getting cloud server's inventories data." & vbNewLine
            table = "admin_pos_inventory_org"
            fields = "*"
            Dim DatatableInv = GLOBAL_SELECT_ALL_FUNCTION_CLOUD(table, fields, DataGridViewINVENTORY)
            For Each row As DataRow In DatatableInv.Rows
                DataGridViewINVENTORY.Rows.Add(row("server_inventory_id"), row("server_formula_id"), row("product_ingredients"), row("sku"), row("stock_primary"), row("stock_secondary"), row("stock_no_of_servings"), row("stock_status"), row("critical_limit"), row("date_modified"), row("main_inventory_id"), row("origin"))
            Next
            TextBox1.Text += FullDate24HR() & " :    Complete(Fetching of inventories data)" & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Fetching of inventories data)" & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Public Sub GetFormula()
        Try
            TextBox1.Text += FullDate24HR() & " :    Getting cloud server's formulas data." & vbNewLine
            table = "admin_product_formula_org"
            fields = "*"
            Dim DatatableForm = GLOBAL_SELECT_ALL_FUNCTION_CLOUD(table, fields, DataGridViewFORMULA)
            For Each row As DataRow In DatatableForm.Rows
                DataGridViewFORMULA.Rows.Add(row("server_formula_id"), row("product_ingredients"), row("primary_unit"), row("primary_value"), row("secondary_unit"), row("secondary_value"), row("serving_unit"), row("serving_value"), row("no_servings"), row("status"), row("date_modified"), row("unit_cost"), row("origin"))
            Next
            TextBox1.Text += FullDate24HR() & " :    Complete(Fetching of formulas data)" & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Fetching of formulas data)" & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertToProducts()
        Try
            TextBox1.Text += FullDate24HR() & " :    Inserting data to local server's table(Products)" & vbNewLine
            With DataGridViewPRODUCTS
                Dim cmdlocal As MySqlCommand
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmdlocal = New MySqlCommand("INSERT INTO loc_admin_products(`server_product_id`,`product_sku`, `product_name`, `formula_id`, `product_barcode`, `product_category`, `product_price`, `product_desc`, `product_image`, `product_status`, `origin`, `date_modified`, `server_inventory_id`, `guid`, `store_id`, `synced`, `addontype`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15 ,@16)", TestLocalConnection())
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
                    cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = .Rows(i).Cells(11).Value
                    cmdlocal.Parameters.Add("@12", MySqlDbType.Int64).Value = .Rows(i).Cells(12).Value
                    cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = UserGUID
                    cmdlocal.Parameters.Add("@14", MySqlDbType.Int32).Value = DataGridViewOutlets.SelectedRows(0).Cells(0).Value
                    cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = "Synced"
                    cmdlocal.Parameters.Add("@16", MySqlDbType.Text).Value = .Rows(i).Cells(13).Value

                    cmdlocal.ExecuteNonQuery()
                Next
            End With
            TextBox1.Text += FullDate24HR() & " :    Complete(Products data insertion)" & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Products data insertion)" & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertToInventory()
        Try
            TextBox1.Text += FullDate24HR() & " :    Inserting data to local server's table(Inventories)." & vbNewLine
            With DataGridViewINVENTORY
                Dim cmdlocal As MySqlCommand
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmdlocal = New MySqlCommand("INSERT INTO loc_pos_inventory(`server_inventory_id`, `formula_id`, `product_ingredients`, `sku`, `stock_primary`, `stock_secondary`, `stock_no_of_servings`, `stock_status`, `critical_limit`, `server_date_modified`, `store_id`, `guid`, `date_modified`, `crew_id`, `synced`, `main_inventory_id`, `origin`)
                                             VALUES (@0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11, @12, @13, @14, @15, @16)", TestLocalConnection())
                    cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmdlocal.Parameters.Add("@1", MySqlDbType.Int64).Value = 0
                    cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString()
                    cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmdlocal.Parameters.Add("@4", MySqlDbType.Decimal).Value = .Rows(i).Cells(4).Value.ToString()
                    cmdlocal.Parameters.Add("@5", MySqlDbType.Decimal).Value = .Rows(i).Cells(5).Value.ToString()
                    cmdlocal.Parameters.Add("@6", MySqlDbType.Decimal).Value = .Rows(i).Cells(6).Value.ToString()
                    cmdlocal.Parameters.Add("@7", MySqlDbType.Int64).Value = .Rows(i).Cells(7).Value.ToString()
                    cmdlocal.Parameters.Add("@8", MySqlDbType.Int64).Value = .Rows(i).Cells(8).Value
                    cmdlocal.Parameters.Add("@9", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value
                    cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = DataGridViewOutlets.SelectedRows(0).Cells(0).Value
                    cmdlocal.Parameters.Add("@11", MySqlDbType.VarChar).Value = UserGUID
                    cmdlocal.Parameters.Add("@12", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value
                    cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = "0"
                    cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = "Synced"
                    cmdlocal.Parameters.Add("@15", MySqlDbType.Text).Value = .Rows(i).Cells(10).Value
                    cmdlocal.Parameters.Add("@16", MySqlDbType.Text).Value = .Rows(i).Cells(11).Value

                    cmdlocal.ExecuteNonQuery()
                Next
            End With
            TextBox1.Text += FullDate24HR() & " :    Complete(Inventories data insertion)" & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Inventories data insertion)" & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertToCategories()
        Try
            TextBox1.Text += FullDate24HR() & " :    Inserting data to local server's table(Categories)." & vbNewLine
            With DataGridViewCATEGORIES
                Dim cmdlocal As MySqlCommand
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmdlocal = New MySqlCommand("INSERT INTO loc_admin_category( `category_name`, `brand_name`, `updated_at`, `origin`, `status`)
                                             VALUES (@0, @1, @2, @3, @4)", TestLocalConnection())
                    cmdlocal.Parameters.Add("@0", MySqlDbType.VarChar).Value = .Rows(i).Cells(0).Value.ToString()
                    cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmdlocal.Parameters.Add("@2", MySqlDbType.VarChar).Value = .Rows(i).Cells(2).Value.ToString
                    cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = .Rows(i).Cells(3).Value.ToString()
                    cmdlocal.Parameters.Add("@4", MySqlDbType.Int64).Value = .Rows(i).Cells(4).Value.ToString()
                    cmdlocal.ExecuteNonQuery()
                Next
            End With
            TextBox1.Text += FullDate24HR() & " :    Complete(Categories data insertion)" & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Categories data insertion)" & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertPartnersTransacton()
        Try
            TextBox1.Text += FullDate24HR() & " :    Inserting data to local server's table(Partners)." & vbNewLine
            With DataGridViewPartners
                Dim cmdlocal As MySqlCommand
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmdlocal = New MySqlCommand("INSERT INTO loc_partners_transaction(`arrid`, `bankname`, `date_modified`, `crew_id`, `store_id`, `guid`, `active`, `synced`)
                                             VALUES (@0, @1, @2, @3, @4 ,@5 ,@6 ,@7)", TestLocalConnection())
                    cmdlocal.Parameters.Add("@0", MySqlDbType.Int64).Value = .Rows(i).Cells(0).Value.ToString()
                    cmdlocal.Parameters.Add("@1", MySqlDbType.VarChar).Value = .Rows(i).Cells(1).Value.ToString()
                    cmdlocal.Parameters.Add("@2", MySqlDbType.Text).Value = .Rows(i).Cells(2).Value.ToString
                    cmdlocal.Parameters.Add("@3", MySqlDbType.VarChar).Value = ""
                    cmdlocal.Parameters.Add("@4", MySqlDbType.VarChar).Value = DataGridViewOutlets.SelectedRows(0).Cells(0).Value
                    cmdlocal.Parameters.Add("@5", MySqlDbType.VarChar).Value = UserGUID
                    cmdlocal.Parameters.Add("@6", MySqlDbType.Int64).Value = .Rows(i).Cells(3).Value.ToString()
                    cmdlocal.Parameters.Add("@7", MySqlDbType.VarChar).Value = "Synced"
                    cmdlocal.ExecuteNonQuery()
                Next
            End With
            TextBox1.Text += FullDate24HR() & " :    Complete(Partners data insertion)" & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Partners data insertion)" & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertCoupons()
        Try
            TextBox1.Text += FullDate24HR() & " :    Inserting data to local server's table(Coupons)." & vbNewLine
            With DataGridViewCoupons
                Dim cmdlocal As MySqlCommand
                For i As Integer = 0 To .Rows.Count - 1 Step +1
                    cmdlocal = New MySqlCommand("INSERT INTO tbcoupon(`Couponname_`, `Desc_`, `Discountvalue_`, `Referencevalue_`, `Type`, `Bundlebase_`, `BBValue_`, `Bundlepromo_`, `BPValue_`, `Effectivedate`, `Expirydate`, `store_id`, `crew_id`, `guid`, `origin`, `synced`, `active`)
                                             VALUES (@0, @1, @2, @3, @4 ,@5 ,@6 ,@7 ,@8 ,@9 ,@10 ,@11 ,@12 ,@13, @14, @15, @16)", TestLocalConnection())
                    cmdlocal.Parameters.Add("@0", MySqlDbType.Text).Value = .Rows(i).Cells(0).Value.ToString()
                    cmdlocal.Parameters.Add("@1", MySqlDbType.Text).Value = .Rows(i).Cells(1).Value.ToString()
                    cmdlocal.Parameters.Add("@2", MySqlDbType.Text).Value = .Rows(i).Cells(2).Value.ToString
                    cmdlocal.Parameters.Add("@3", MySqlDbType.Text).Value = .Rows(i).Cells(3).Value.ToString
                    cmdlocal.Parameters.Add("@4", MySqlDbType.Text).Value = .Rows(i).Cells(4).Value.ToString
                    cmdlocal.Parameters.Add("@5", MySqlDbType.Text).Value = .Rows(i).Cells(5).Value.ToString
                    cmdlocal.Parameters.Add("@6", MySqlDbType.Text).Value = .Rows(i).Cells(6).Value.ToString
                    cmdlocal.Parameters.Add("@7", MySqlDbType.Text).Value = .Rows(i).Cells(7).Value.ToString
                    cmdlocal.Parameters.Add("@8", MySqlDbType.Text).Value = .Rows(i).Cells(8).Value.ToString
                    cmdlocal.Parameters.Add("@9", MySqlDbType.Text).Value = .Rows(i).Cells(9).Value.ToString
                    cmdlocal.Parameters.Add("@10", MySqlDbType.Text).Value = .Rows(i).Cells(10).Value.ToString
                    cmdlocal.Parameters.Add("@11", MySqlDbType.Text).Value = DataGridViewOutlets.SelectedRows(0).Cells(0).Value
                    cmdlocal.Parameters.Add("@12", MySqlDbType.Text).Value = ""
                    cmdlocal.Parameters.Add("@13", MySqlDbType.Text).Value = UserGUID
                    cmdlocal.Parameters.Add("@14", MySqlDbType.Text).Value = "Server"
                    cmdlocal.Parameters.Add("@15", MySqlDbType.Text).Value = "Synced"
                    cmdlocal.Parameters.Add("@16", MySqlDbType.Text).Value = 1
                    cmdlocal.ExecuteNonQuery()
                Next
            End With
            TextBox1.Text += FullDate24HR() & " :    Complete(Coupons data insertion)" & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Coupons data insertion)" & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub InsertToFormula()
        Try
            TextBox1.Text += FullDate24HR() & " :    Inserting data to local server's table(Formulas)." & vbNewLine
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
                    cmdlocal.Parameters.Add("@10", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value
                    cmdlocal.Parameters.Add("@11", MySqlDbType.Decimal).Value = .Rows(i).Cells(11).Value.ToString()
                    cmdlocal.Parameters.Add("@12", MySqlDbType.VarChar).Value = .Rows(i).Cells(12).Value.ToString()
                    cmdlocal.Parameters.Add("@13", MySqlDbType.VarChar).Value = .Rows(i).Cells(10).Value
                    cmdlocal.Parameters.Add("@14", MySqlDbType.VarChar).Value = DataGridViewOutlets.SelectedRows(0).Cells(0).Value
                    cmdlocal.Parameters.Add("@15", MySqlDbType.VarChar).Value = UserGUID
                    cmdlocal.ExecuteNonQuery()
                Next
            End With
            TextBox1.Text += FullDate24HR() & " :    Complete(Formulas data insertion)" & vbNewLine
        Catch ex As Exception
            TextBox1.Text += FullDate24HR() & " :    Failed(Formulas data insertion)" & vbNewLine
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles ButtonEditAccount.Click
        TextboxEnableability(Panel14, True)
        AccountExist = False
        FranchiseeStoreValidation = False
        DataGridViewOutlets.DataSource = Nothing
        DataGridViewOutletDetails.Rows.Clear()
        ClearTextBox(Panel15)
    End Sub
    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles ButtonSaveLocalCon.Click
        SaveLocalConnection()
    End Sub

    Private Sub SaveLocalConnection()
        Try
            If ValidLocalConnection = True Then
                Dim FolderName As String = "Innovention"
                Dim path = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                CreateFolder(path, FolderName)
                BTNSaveLocalConn = True
                TextboxEnableability(Panel5, False)
                ButtonClearLocal.Enabled = False
                ButtonTestLocConn.Enabled = False
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
    Private Sub TextBoxLocalServer_TextChanged(sender As Object, e As EventArgs) Handles TextBoxLocalUsername.TextChanged, TextBoxLocalServer.TextChanged, TextBoxLocalPort.TextChanged, TextBoxLocalPassword.TextChanged, TextBoxLocalDatabase.TextChanged
        Try
            BTNSaveLocalConn = False
            ValidLocalConnection = False
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub TextBoxCloudServer_TextChanged(sender As Object, e As EventArgs) Handles TextBoxCloudUsername.TextChanged, TextBoxCloudServer.TextChanged, TextBoxCloudPort.TextChanged, TextBoxCloudPassword.TextChanged, TextBoxCloudDatabase.TextChanged
        Try
            BTNSaveCloudConn = False
            ValidCloudConnection = False
            My.Settings.Save()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub TextBoxDEVPTU_TextChanged(sender As Object, e As EventArgs) Handles TextBoxDevTIN.TextChanged, TextBoxDEVPTU.TextChanged, TextBoxDevname.TextChanged, TextBoxDevAdd.TextChanged, TextBoxDevAccr.TextChanged
        Try
            ConfirmDevInfoSettings = False
            My.Settings.Save()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub DateTimePicker1ACCRDI_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePickerPTUVU.ValueChanged, DateTimePicker4PTUDI.ValueChanged, DateTimePicker2ACCRVU.ValueChanged, DateTimePicker1ACCRDI.ValueChanged
        Try
            ConfirmDevInfoSettings = False
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub TextBoxExportPath_TextChanged(sender As Object, e As EventArgs) Handles TextBoxTerminalNo.TextChanged, TextBoxTax.TextChanged, TextBoxSINumber.TextChanged, TextBoxExportPath.TextChanged
        Try
            ConfirmAdditionalSettings = False
            My.Settings.Save()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub RadioButtonYES_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonYES.CheckedChanged, RadioButtonNO.CheckedChanged
        Try
            ConfirmAdditionalSettings = False
            My.Settings.Save()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub RadioButtonDaily_Click(sender As Object, e As EventArgs) Handles RadioButtonYearly.Click, RadioButtonWeekly.Click, RadioButtonMonthly.Click, RadioButtonDaily.Click
        Try
            If ValidLocalConnection = True Then
                Dim Interval As Integer = 0
                Dim IntervalName As String = ""
                If RadioButtonDaily.Checked = True Then
                    Interval = 1
                    IntervalName = "Daily"
                ElseIf RadioButtonWeekly.Checked = True Then
                    Interval = 2
                    IntervalName = "Weekly"
                ElseIf RadioButtonMonthly.Checked = True Then
                    Interval = 3
                    IntervalName = "Monthly"
                ElseIf RadioButtonYearly.Checked = True Then
                    Interval = 4
                    IntervalName = "Yearly"
                End If
                Dim sql = "SELECT `S_BackupInterval` , `S_BackupDate` FROM loc_settings WHERE settings_id = 1"
                Dim cmd As MySqlCommand = New MySqlCommand(sql, TestLocalConnection)
                Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
                Dim dt As DataTable = New DataTable
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    sql = "UPDATE loc_settings SET `S_BackupInterval` = " & Interval & " , `S_BackupDate` = '" & Format(Now(), "yyyy-MM-dd") & "'"
                    cmd = New MySqlCommand(sql, TestLocalConnection)
                    cmd.ExecuteNonQuery()
                    Autobackup = True
                Else
                    sql = "INSERT INTO loc_settings (`S_BackupInterval` , `S_BackupDate`) VALUES ('" & Interval & "','" & Format(Now(), "yyyy-MM-dd") & "')"
                    cmd = New MySqlCommand(sql, TestLocalConnection)
                    cmd.ExecuteNonQuery()
                    Autobackup = True
                End If
                MsgBox("Automatic system backup set to " & IntervalName & " backup")
            Else
                Autobackup = False
                MsgBox("Local connection must be valid first.")
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub TextBoxLocalDatabase_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBoxLocalUsername.KeyPress, TextBoxLocalServer.KeyPress, TextBoxLocalPort.KeyPress, TextBoxLocalPassword.KeyPress, TextBoxLocalDatabase.KeyPress, TextBoxCloudUsername.KeyPress, TextBoxCloudServer.KeyPress, TextBoxCloudPort.KeyPress, TextBoxCloudPassword.KeyPress, TextBoxCloudDatabase.KeyPress, TextBoxTerminalNo.KeyPress, TextBoxTax.KeyPress, TextBoxSINumber.KeyPress, TextBoxExportPath.KeyPress, TextBoxDevTIN.KeyPress, TextBoxDEVPTU.KeyPress, TextBoxDevname.KeyPress, TextBoxDevAdd.KeyPress, TextBoxDevAccr.KeyPress, TextBoxFrancUser.KeyPress, TextBoxFrancPass.KeyPress, TextBoxProdKey.KeyPress
        Try
            If InStr(DisallowedCharacters, e.KeyChar) > 0 Then
                e.Handled = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub

    Private Sub ButtonImportCloudConn_Click(sender As Object, e As EventArgs) Handles ButtonImportCloudConn.Click
        Try
            Dim myOpenFileDialog As New OpenFileDialog()

            myOpenFileDialog.CheckFileExists = True
            myOpenFileDialog.DefaultExt = "txt"
            myOpenFileDialog.InitialDirectory = "C:\"
            myOpenFileDialog.Multiselect = False
            If myOpenFileDialog.ShowDialog = DialogResult.OK Then
                Dim ImportPath = myOpenFileDialog.FileName
                Dim TextLine As String = ""
                Dim objReader As New System.IO.StreamReader(ImportPath)
                Dim lineCount As Integer
                Do While objReader.Peek() <> -1
                    TextLine = objReader.ReadLine()
                    If lineCount = 0 Then
                        TextBoxCloudServer.Text = RemoveCharacter(TextLine, "server=")
                    End If
                    If lineCount = 1 Then
                        TextBoxCloudUsername.Text = RemoveCharacter(TextLine, "user id=")
                    End If
                    If lineCount = 2 Then
                        TextBoxCloudPassword.Text = RemoveCharacter(TextLine, "password=")
                    End If
                    If lineCount = 3 Then
                        TextBoxCloudDatabase.Text = RemoveCharacter(TextLine, "database=")
                    End If
                    If lineCount = 4 Then
                        TextBoxCloudPort.Text = RemoveCharacter(TextLine, "port=")
                    End If
                    lineCount = lineCount + 1
                Loop
                objReader.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Dim ImagePath As String
    Private Sub ButtonBrowseLogo_Click(sender As Object, e As EventArgs) Handles ButtonBrowseLogo.Click
        Try
            With OpenFileDialog1
                .Filter = ("Images | *.png; *.bmp; *.jpg; *.jpeg; *.gif; *.ico;")
                .FilterIndex = 4
            End With
            OpenFileDialog1.FileName = ""
            If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
                If My.Computer.FileSystem.FileExists(ImagePath) Then
                    convertimage()
                End If
                PictureBoxLogo.Image = Image.FromFile(OpenFileDialog1.FileName)
                PictureBoxLogo.SizeMode = PictureBoxSizeMode.StretchImage
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        ImagePath = OpenFileDialog1.FileName
    End Sub
    Dim encodeType As ImageFormat = ImageFormat.Jpeg
    Dim decodingstring As String = String.Empty
    Private Sub convertimage()
        Try
            RichTextBoxLogo.Clear()
            Dim ImageToConvert As Bitmap = Bitmap.FromFile(ImagePath)
            ImageToConvert.MakeTransparent()
            Dim encoding As String = String.Empty
            If ImagePath.ToLower.EndsWith(".jpg") Then
                encodeType = ImageFormat.Jpeg
            ElseIf ImagePath.ToLower.EndsWith(".png") Then
                encodeType = ImageFormat.Png
            ElseIf ImagePath.ToLower.EndsWith(".gif") Then
                encodeType = ImageFormat.Gif
            ElseIf ImagePath.ToLower.EndsWith(".bmp") Then
                encodeType = ImageFormat.Bmp
            End If
            decodingstring = encoding
            RichTextBoxLogo.Text = ImageToBase64(ImageToConvert, encodeType)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
#Region "Test Insert"
    'Private Sub button734_click(sender As Object, e As EventArgs) Handles Button4.Click
    '    InsertToProducts()
    '    InsertToInventory()
    '    InsertToCategories()
    '    InsertToFormula()
    'End Sub

    'Private Sub button8_click_123(sender As Object, e As EventArgs) Handles Button8.Click
    '    GetCategories()
    '    GetProducts()
    '    GetInventory()
    '    GetFormula()
    'End Sub


#End Region
End Class