Imports System.Threading
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Drawing.Printing

Public Class ConfigurationSettings
    Private WithEvents printdoc As PrintDocument = New PrintDocument
    Private PrintPreviewDialog1 As New PrintPreviewDialog

    Dim bat As String
    Dim optimizeorrepair As Integer = 0
    Dim thread As Thread
    Dim threadList As List(Of Thread) = New List(Of Thread)


    Private Sub DatabaseReset_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            TabControl1.TabPages(0).Text = "Connection"
            TabControl1.TabPages(1).Text = "Settings"
            TabControl1.TabPages(2).Text = "POS details"
            TabControl2.TabPages(0).Text = "Database Setup for Local / Cloud"
            Label1.Text = ClientProductKey
            Label32.Text = ClientBrand
            Label33.Text = ClientLocation
            Label34.Text = ClientAddress
            Label35.Text = ClientMunicipality
            Label36.Text = ClientProvince
            Label37.Text = ClientTin
            'Label38.Text = ClientIPAdd
            Label39.Text = ClientTel
            TextBoxMIN.Text = ClientMIN
            TextBoxPTU.Text = ClientPTUN
            'If My.Settings.DevAccrDateIssued <> "" Then
            '    DateTimePickerIssued.Value = My.Settings.DevAccrDateIssued
            'End If
            'If My.Settings.DevAccrValidUntil <> "" Then
            '    DateTimePickerValidUntil.Value = My.Settings.DevAccrValidUntil
            'End If
            'TextBoxMIN.Text = My.Settings.Accreditation
            'TextBoxSerialNum.Text = My.Settings.SerialNumber
            'TextBoxLocalServer.Text = My.Settings.localserver
            'TextBoxLocalUsername.Text = My.Settings.localuser
            'TextBoxLocalPassword.Text = My.Settings.localpass
            'TextBoxLocalPort.Text = My.Settings.localport
            'TextBoxLocalSchema.Text = My.Settings.localname
            'TextBoxServerName.Text = My.Settings.cloudserver
            'TextBoxServerUsername.Text = My.Settings.clouduser
            'TextBoxServerPassword.Text = My.Settings.cloudpass
            'TextBoxServerSchema.Text = My.Settings.cloudname
            'TextBoxServerPort.Text = My.Settings.cloudport
            'TextBoxExportPath.Text = My.Settings.exportpath
            'TextBoxSINumber.Text = My.Settings.SINumber
            TextBoxTax.Text = Val(S_Tax) * 100
            If S_ZeroRated = "0" Then
                RadioButtonNO.Checked = True
            Else
                RadioButtonYES.Checked = True
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub ButtonImport_Click(sender As Object, e As EventArgs) Handles ButtonImport.Click
        OpenFileDialog1.Title = "Please Select a File"
        OpenFileDialog1.InitialDirectory = "C:\Users\I.T\Desktop\DBBACKUP"
        OpenFileDialog1.ShowDialog()
    End Sub

    Private Sub ButtonMaintenance_Click(sender As Object, e As EventArgs) Handles ButtonMaintenance.Click
        optimizeorrepair = 0
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.RunWorkerAsync()
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        optimizeorrepair = 1
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.RunWorkerAsync()
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
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            If optimizeorrepair = 0 Then
                bat = "repair.bat"
                thread = New Thread(AddressOf StartCommandLine)
                thread.Start()
            ElseIf optimizeorrepair = 1 Then
                bat = "optimize.bat"
                thread = New Thread(AddressOf StartCommandLine)
                thread.Start()
            ElseIf optimizeorrepair = 2 Then
                bat = "backup.bat"
                thread = New Thread(AddressOf StartCommandLine)
                thread.Start()
            End If
            For Each t In threadList
                t.Join()
            Next
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        TextBoxLocalServer.ReadOnly = False
        TextBoxLocalUsername.ReadOnly = False
        TextBoxLocalPassword.ReadOnly = False
        TextBoxLocalPort.ReadOnly = False
        TextBoxLocalSchema.ReadOnly = False
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            localconn = New MySqlConnection
            localconn.ConnectionString = "server=" & TextBoxLocalServer.Text &
                ";user id=" & TextBoxLocalUsername.Text &
                ";password=" & TextBoxLocalPassword.Text &
                ";database=" & TextBoxLocalSchema.Text &
                ";port=" & TextBoxLocalPort.Text
            localconn.Open()
            If localconn.State = ConnectionState.Open Then
                MsgBox("Connected Successfully!")
            End If
        Catch ex As Exception
            MsgBox("Connection is invalid!")
        End Try
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            cloudconn = New MySqlConnection
            cloudconn.ConnectionString = "server=" & TextBoxServerName.Text &
                ";user id=" & TextBoxServerUsername.Text &
                ";password=" & TextBoxServerPassword.Text &
                ";database=" & TextBoxServerSchema.Text &
                ";port=" & TextBoxServerPort.Text
            cloudconn.Open()
            If cloudconn.State = ConnectionState.Open Then
                MsgBox("Connected Successfully!")
            End If
        Catch ex As Exception
            MsgBox("Connection is invalid!")
        End Try
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        TextBoxServerName.ReadOnly = False
        TextBoxServerUsername.ReadOnly = False
        TextBoxServerPassword.ReadOnly = False
        TextBoxServerPort.ReadOnly = False
        TextBoxServerSchema.ReadOnly = False
    End Sub
    Dim DatabaseName As String
    Private Sub ButtonExport_Click(sender As Object, e As EventArgs) Handles ButtonExport.Click
        'DatabaseName = "POS" + Date.Now.ToString("dd-MM-yyyy-HH-mm-ss")
        'FolderBrowserDialog1.ShowDialog()
        'BackupPath = FolderBrowserDialog1.SelectedPath.ToString()
        'Dim myProcess As New Process()
        'myProcess.StartInfo.FileName = "cmd.exe"
        'myProcess.StartInfo.UseShellExecute = False
        'myProcess.StartInfo.WorkingDirectory = "C:\xampp\mysql\bin\"
        'myProcess.StartInfo.RedirectStandardInput = True
        'myProcess.StartInfo.RedirectStandardOutput = True
        'myProcess.Start()
        'Dim myStreamerWriter As StreamWriter = myProcess.StandardInput
        'Dim myStreamerReader As StreamReader = myProcess.StandardOutput
        'myStreamerWriter.WriteLine("mysqldump -u" & My.Settings.localuser & " -p" & My.Settings.localpass & " " & My.Settings.localname & "  > " & BackupPath & "\" & DatabaseName & ".sql")
        'myStreamerWriter.Close()
        'myProcess.WaitForExit()
        'myProcess.Close()
    End Sub
    Dim BackupPath As String
    Dim Path As String
    Sub Restore()
        '    Dim myProcess As New Process()
        '    myProcess.StartInfo.FileName = "cmd.exe"
        '    myProcess.StartInfo.UseShellExecute = False
        '    myProcess.StartInfo.WorkingDirectory = "C:\xampp\mysql\bin\"
        '    myProcess.StartInfo.RedirectStandardInput = True
        '    myProcess.StartInfo.RedirectStandardOutput = True
        '    myProcess.Start()
        '    Dim myStreamerWriter As StreamWriter = myProcess.StandardInput
        '    Dim myStreamerReader As StreamReader = myProcess.StandardOutput
        '    myStreamerWriter.WriteLine("mysql -h" & My.Settings.localserver & " -u" & My.Settings.localuser & " -p" & My.Settings.localpass & " test1 < " & Path & " ")
        '    myStreamerWriter.Close()
        '    myProcess.WaitForExit()
        '    myProcess.Close()
        '    MsgBox("Database Restoration Successfully!", MsgBoxStyle.Information, "Restore")
    End Sub
    Private Sub OpenFileDialog1_FileOk(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        Path = OpenFileDialog1.FileName.ToString()
        Restore()
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        'My.Settings.SINumber = TextBoxSINumber.Text
        'S_Tax = Val(TextBoxTax.Text) / 100
        'My.Settings.Save()
        'Application.Exit()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            'My.Settings.DevAccreditation = TextBoxMIN.Text
            'My.Settings.SerialNumber = TextBoxSerialNum.Text
            ''TextBoxPTU.Text
            'My.Settings.DevAccrDateIssued = DateTimePickerIssued.Value
            'My.Settings.DevAccrValidUntil = DateTimePickerValidUntil.Value
            My.Settings.Save()
            Application.Exit()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub
    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        printdoc.DefaultPageSettings.PaperSize = New PaperSize("Custom", 200, 800)
        PrintPreviewDialog1.Document = printdoc
        PrintPreviewDialog1.ShowDialog()
    End Sub
    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printdoc.PrintPage
        Dim brandfont As New Font("Kelson Sans Normal", 9)
        Dim font As New Font("Kelson Sans Normal", 7)
        CenterTextDisplay(sender, e, ClientBrand.ToUpper, brandfont, 10)
        '============================================================================================================================
        CenterTextDisplay(sender, e, "Opt by : Innovention Food Asia Co.", font, 21)
        '============================================================================================================================
        CenterTextDisplay(sender, e, ClientAddress & ", Brgy. " & ClientBrgy, font, 31)
        '============================================================================================================================
        CenterTextDisplay(sender, e, getmunicipality & ", " & getprovince, font, 41)
        '============================================================================================================================
        CenterTextDisplay(sender, e, "VAT REG TIN : " & ClientTin, font, 51)
        '============================================================================================================================
        CenterTextDisplay(sender, e, "MSN : T500114100140", font, 61)
        '============================================================================================================================
        CenterTextDisplay(sender, e, "MIN : 140351765", font, 71)
        '============================================================================================================================
        CenterTextDisplay(sender, e, "PTUN : 0414-038-184993-000", font, 81)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 100, "TERMINAL REPORT", "X-READ", font)
        '============================================================================================================================
        SimpleTextDisplay(sender, e, "XT0000002110", font, 0, 90)
        SimpleTextDisplay(sender, e, "----------------------------------------", font, 0, 95)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 123, "DESCRIPTION", "QTY/AMOUNT", font)
        '============================================================================================================================
        SimpleTextDisplay(sender, e, "----------------------------------------", font, 0, 110)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 140, "TERMINAL N0.", "1", font)
        RightToLeftDisplay(sender, e, 155, "GROSS", "3000.00", font)
        RightToLeftDisplay(sender, e, 165, "LESS VAT (VE)", "3000.00", font)
        RightToLeftDisplay(sender, e, 175, "LESS VAT DIPLOMAT", "3000.00", font)
        RightToLeftDisplay(sender, e, 185, "LESS VAT (OTHER)", "3000.00", font)
        RightToLeftDisplay(sender, e, 195, "ADD VAT", "3000.00", font)
        RightToLeftDisplay(sender, e, 205, "DAILY SALES", "3000.00", font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 220, "VAT AMOUNT", "1", font)
        RightToLeftDisplay(sender, e, 230, "LOCAL GOV'T TAX", "3000.00", font)
        RightToLeftDisplay(sender, e, 240, "VATABLE SALES", "3000.00", font)
        RightToLeftDisplay(sender, e, 250, "ZERO RATED SALES", "3000.00", font)
        RightToLeftDisplay(sender, e, 260, "VAT EXEMPT SALES", "3000.00", font)
        RightToLeftDisplay(sender, e, 270, "LESS DISC (VE)", "3000.00", font)
        RightToLeftDisplay(sender, e, 280, "NET SALES", "3000.00", font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 295, "CASH TOTAL", "1", font)
        RightToLeftDisplay(sender, e, 305, "CREDIT CARD", "3000.00", font)
        RightToLeftDisplay(sender, e, 315, "DEBIT CARD", "3000.00", font)
        RightToLeftDisplay(sender, e, 325, "MISC/CHEQUES", "3000.00", font)
        RightToLeftDisplay(sender, e, 335, "   EXCESS CHK", "3000.00", font)
        RightToLeftDisplay(sender, e, 345, "IN-HOUSE CHARGE", "3000.00", font)
        RightToLeftDisplay(sender, e, 355, "   EXCESS GC", "3000.00", font)
        RightToLeftDisplay(sender, e, 365, "A/R", "3000.00", font)
        RightToLeftDisplay(sender, e, 375, "OTHERS", "3000.00", font)
        RightToLeftDisplay(sender, e, 385, "DEPOSIT", "3000.00", font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 400, "CASH IN DRAWER", "3000.00", font)
        RightToLeftDisplay(sender, e, 410, "PICK-UP", "3000.00", font)
        RightToLeftDisplay(sender, e, 420, "RCVD-ON-ACCOUNT", "3000.00", font)
        RightToLeftDisplay(sender, e, 430, "PAID-OUT", "3000.00", font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 445, "ITEM VOID/E.C", "1", font)
        RightToLeftDisplay(sender, e, 455, "TRANS. VOID", "3000.00", font)
        RightToLeftDisplay(sender, e, 465, "TRANS. CANCEL", "3000.00", font)
        RightToLeftDisplay(sender, e, 475, "DIPLOMAT", "3000.00", font)
        RightToLeftDisplay(sender, e, 485, "TOTAL DISCOUNTS", "3000.00", font)
        RightToLeftDisplay(sender, e, 495, "   SENIOR CITIZEN", "3000.00", font)
        RightToLeftDisplay(sender, e, 505, "ITEM DISCOUNTS", "3000.00", font)
        RightToLeftDisplay(sender, e, 515, "S. CHARGE", "3000.00", font)
        RightToLeftDisplay(sender, e, 525, "CORKAGE", "3000.00", font)
        RightToLeftDisplay(sender, e, 535, "TOTAL SURCHARGE", "3000.00", font)
        RightToLeftDisplay(sender, e, 545, "TAKE OUT CHARGE", "3000.00", font)
        RightToLeftDisplay(sender, e, 555, "DELIVERY CHARGE", "3000.00", font)
        RightToLeftDisplay(sender, e, 565, "UNCONSUMABLE", "3000.00", font)
        RightToLeftDisplay(sender, e, 575, "RETURNS EXCHANGE", "3000.00", font)
        RightToLeftDisplay(sender, e, 585, "RETURNS REFUND", "3000.00", font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 600, "TOTAL QTY. SOLD", "3000.00", font)
        RightToLeftDisplay(sender, e, 610, "TRANSACTION COUNT", "3000.00", font)
        RightToLeftDisplay(sender, e, 620, "TOTAL GUEST", "3000.00", font)
        RightToLeftDisplay(sender, e, 630, "BEG. OR NO.", "3000.00", font)
        RightToLeftDisplay(sender, e, 640, "END OR NO.", "3000.00", font)
        '============================================================================================================================
        RightToLeftDisplay(sender, e, 655, "CURRENT TOTAL SALES", "3000.00", font)
        RightToLeftDisplay(sender, e, 665, "OLD GRAND TOTAL", "3000.00", font)
        RightToLeftDisplay(sender, e, 675, "NEW GRAND TOTAL", "3000.00", font)
        '============================================================================================================================
        SimpleTextDisplay(sender, e, "----------------------------------------", font, 0, 665)
        '============================================================================================================================
        CenterTextDisplay(sender, e, Format(Now, "MM/dd/yyyy hh:mm:ss tt"), font, 700)
    End Sub
End Class