Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Net
Imports MySql.Data.MySqlClient
Imports System.Drawing.Printing
Imports System.Text.RegularExpressions
Imports System.Globalization

Module publicfunctions
    Public drasd
    Dim dr2
    Dim hashable As String
    Dim dateformat
    Dim timeformat
    Declare Function Wow64DisableWow64FsRedirection Lib "kernel32" (ByRef oldvalue As Long) As Boolean
    Private osk As String = "C:\Windows\System32\osk.exe"
    Public Sub ShowKeyboard()
        Try
            Wow64DisableWow64FsRedirection(0)
            Process.Start(osk)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub ButtonEnableability(ByVal root As Control, ENB As Boolean)
        For Each ctrl As Control In root.Controls
            ButtonEnableability(ctrl, ENB)
            If TypeOf ctrl Is Button Then
                CType(ctrl, Button).Enabled = ENB
            End If
        Next ctrl
    End Sub
    Public Function TextboxIsEmpty(ByVal root As Control)
        Dim ReturnThisThing As Boolean
        For Each tb As TextBox In root.Controls.OfType(Of TextBox)()
            If tb.Text = String.Empty Then

                ReturnThisThing = False
                Exit For
            Else
                ReturnThisThing = True
            End If
        Next
        Return ReturnThisThing
    End Function
    Public Sub TextboxEnableability(ByVal root As Control, ENB As Boolean)
        Try
            For Each ctrl As Control In root.Controls
                TextboxEnableability(ctrl, ENB)
                If TypeOf ctrl Is TextBox Then
                    CType(ctrl, TextBox).Enabled = ENB
                End If
            Next ctrl
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub ClearTextBox(ByVal root As Control)
        Try
            For Each ctrl As Control In root.Controls
                ClearTextBox(ctrl)
                If TypeOf ctrl Is TextBox Then
                    CType(ctrl, TextBox).Text = String.Empty
                End If
            Next ctrl
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub ClearDataGridViewRows(ByVal root As Control)
        Try
            For Each ctrl As Control In root.Controls
                ClearDataGridViewRows(ctrl)
                If TypeOf ctrl Is DataGridView Then
                    CType(ctrl, DataGridView).DataSource = Nothing
                    CType(ctrl, DataGridView).Rows.Clear()
                End If
            Next ctrl
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub SpecialCharRestriction(ByVal root As Control, ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Try
            For Each ctrl As Control In root.Controls
                SpecialCharRestriction(ctrl, sender, e)
                If TypeOf ctrl Is TextBox Then
                    Dim allowedChars As String = "[`~!@#\$%\^&\*\(\)_\-\+=\{\}\[\]\\\|:;""'<>,\.\?/"
                    If Not allowedChars.IndexOf(e.KeyChar) = -1 Then
                        e.Handled = True
                    End If
                End If
            Next ctrl
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub buttonpressedenter(ByVal btntext As String)
        If Val(POS.TextBoxQTY.Text) <> 0 Then
            POS.TextBoxQTY.Text += btntext
        Else
            POS.TextBoxQTY.Text = btntext
        End If
    End Sub
    Public Sub buttonpressedenterpayment(ByVal btntext As String)
        If Val(PaymentForm.TextBoxMONEY.Text) <> 0 Then
            PaymentForm.TextBoxMONEY.Text += btntext
        Else
            PaymentForm.TextBoxMONEY.Text = btntext
        End If
    End Sub
    Public Sub btnformcolor(ByVal changecolor As Button)
        changecolor.BackColor = Color.FromArgb(23, 162, 184)
    End Sub
    Public Sub btndefaut(ByVal defaultcolor As Button, ByVal form As Form)
        For Each P As Control In form.Controls
            If TypeOf P Is Panel Then
                For Each ctrl As Control In P.Controls
                    If TypeOf ctrl Is Button Then
                        If ctrl.Name <> defaultcolor.Name Then
                            CType(ctrl, Button).BackColor = Color.FromArgb(41, 39, 40)
                        End If
                    End If
                Next
            End If
        Next
    End Sub
    Public Function ConvertToBase64(str As String)
        Dim byt As Byte() = System.Text.Encoding.UTF8.GetBytes(str)
        Dim byt2 = Convert.ToBase64String(byt)
        Return byt2
    End Function
    Public Function ConvertB64ToString(str As String)
        Dim b As Byte() = Convert.FromBase64String(str)
        Dim byt2 = System.Text.Encoding.UTF8.GetString(b)
        Return byt2
    End Function
    'Dim MyPublicIpAddress As String
    'Public Function PublicIpAddress(myform As Form)
    '    Dim client As New WebClient
    '    '// Add a user agent header in case the requested URI contains a query.
    '    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR1.0.3705;)")
    '    Dim baseurl As String = "http://localhost/ipadd/"
    '    ' with proxy server only:
    '    Dim proxy As IWebProxy = WebRequest.GetSystemWebProxy()
    '    proxy.Credentials = CredentialCache.DefaultNetworkCredentials
    '    client.Proxy = proxy
    '    Dim data As Stream
    '    Try
    '        data = client.OpenRead(baseurl)
    '    Catch ex As Exception
    '        MsgBox("open url " & ex.Message)
    '        Exit Function
    '    End Try
    '    Dim reader As StreamReader = New StreamReader(data)
    '    MyPublicIpAddress = reader.ReadToEnd()
    '    data.Close()
    '    reader.Close()
    '    MyPublicIpAddress = MyPublicIpAddress.Replace("<html><head><title>Current IP Check</title></head><body>", "").Replace("</body></html>", "").ToString()
    '    myform.Text = MyPublicIpAddress
    '    Return MyPublicIpAddress
    'End Function

    Public Function RemoveCharacter(ByVal stringToCleanUp, ByVal characterToRemove)
        ' replace the target with nothing
        ' Replace() returns a new String and does not modify the current one
        Return stringToCleanUp.Replace(characterToRemove, "")
    End Function
    '_________________________________________________________________________________________________________________
    'IMAGE TO TEXT
    Public Function ImageToBase64(ByVal image As Image, ByVal format As System.Drawing.Imaging.ImageFormat) As String
        Using ms As New MemoryStream()
            ' Convert Image to byte[]
            image.Save(ms, format)
            Dim imageBytes As Byte() = ms.ToArray()
            ' Convert byte[] to Base64 String
            Dim base64String As String = Convert.ToBase64String(imageBytes)
            Return base64String
        End Using
    End Function
    'TEXT TO IMAGE
    Public Function Base64ToImage(ByVal base64String As String) As Image
        ' Convert Base64 String to byte[]
        Dim imageBytes As Byte() = Convert.FromBase64String(base64String)
        Dim ms As New MemoryStream(imageBytes, 0, imageBytes.Length)
        ' Convert byte[] to Image
        ms.Write(imageBytes, 0, imageBytes.Length)
        Dim ConvertedBase64Image As Image = Image.FromStream(ms, True)
        Return ConvertedBase64Image
    End Function
    Private ImagePath As String = ""
    '_________________________________________________________________________________________________________________
    'POS FUNCTIONS
    Public Function GetHash(theInput As String) As String
        Using hasher As MD5 = MD5.Create()    ' create hash object
            ' Convert to byte array and get hash
            Dim dbytes As Byte() =
             hasher.ComputeHash(Encoding.UTF8.GetBytes(theInput))
            ' sb to create string from bytes
            Dim sBuilder As New StringBuilder()
            ' convert byte data to hex string
            For n As Integer = 0 To dbytes.Length - 1
                sBuilder.Append(dbytes(n).ToString("x2"))
            Next n
            Return sBuilder.ToString()
        End Using
    End Function
    Public Function ConvertPassword(ByVal SourceString As String)
        Dim ConvertedString As String
        Dim byt As Byte() = System.Text.Encoding.UTF8.GetBytes(SourceString)
        ConvertedString = Convert.ToBase64String(byt)
        Using md5Hash As MD5 = MD5.Create()
            hashable = GetHash(ConvertedString)
        End Using
        Return hashable
    End Function
    Public Function CheckForInternetConnection() As Boolean
        Try
            Using client = New WebClient()
                Using stream = client.OpenRead("http://www.google.com")
                    Return True
                End Using
            End Using
        Catch
            Return False
        End Try
    End Function
    Public Function GetMonthName(dat As Date) As String
        Dim iMonth As Integer = Month(dat)
        GetMonthName = MonthName(iMonth)
    End Function

    Public resetinventory As Boolean
    Public Function FirstDayOfMonth(ByVal sourceDate As DateTime) As DateTime
        Return New DateTime(sourceDate.Year, sourceDate.Month, 1)
    End Function
    Dim dtRESET As DataTable
    Public Function CheckIfNeedToReset() As Boolean
        Try
            Dim cmd As MySqlCommand
            Dim da As MySqlDataAdapter
            Dim firstday = Format(FirstDayOfMonth(Date.Now), "yyyy-MM-dd")
            Try
                Dim sql = "SELECT * FROM loc_inv_temp_data WHERE created_at = '" & firstday & "'"
                cmd = New MySqlCommand(sql, LocalhostConn)
                da = New MySqlDataAdapter(cmd)
                dtRESET = New DataTable
                da.Fill(dtRESET)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try

        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
        If dtRESET.Rows.Count = 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Dim DateNow
    Public Function FullDate24HR()
        Try
            DateNow = Format(Now(), "yyyy-MM-dd HH:mm:ss")
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
        Return DateNow
    End Function
    Public Sub EndBalance()
        Try
            If Shift = "First Shift" Then
                SystemLogType = "END-1"
                Dim DailySales = sum(table:="loc_daily_transaction_details WHERE created_at = '" & Format(Now(), "yyyy-MM-dd") & "' AND active = 1 AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' ", tototal:="total")
                EndingBalance = BeginningBalance + Val(DailySales)
            ElseIf Shift = "Second Shift" Then
                SystemLogType = "END-2"
                Dim DailySales = sum(table:="loc_daily_transaction_details WHERE created_at = '" & Format(Now(), "yyyy-MM-dd") & "' AND active = 1 AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' ", tototal:="total")
                EndingBalance = BeginningBalance + Val(DailySales)
            ElseIf Shift = "Third Shift" Then
                SystemLogType = "END-3"
                Dim DailySales = sum(table:="loc_daily_transaction_details WHERE created_at = '" & Format(Now(), "yyyy-MM-dd") & "' AND active = 1 AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' ", tototal:="total")
                EndingBalance = BeginningBalance + Val(DailySales)
            Else
                SystemLogType = "END-4"
                Dim DailySales = sum(table:="loc_daily_transaction_details WHERE created_at = '" & Format(Now(), "yyyy-MM-dd") & "' AND active = 1 AND store_id = '" & ClientStoreID & "' AND guid = '" & ClientGuid & "' ", tototal:="total")
                EndingBalance = BeginningBalance + Val(DailySales)
            End If
            SystemLogDesc = EndingBalance
            GLOBAL_SYSTEM_LOGS(SystemLogType, SystemLogDesc)
            Shift = ""
            BeginningBalance = 0
            EndingBalance = 0
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Dim total
    Public Function SumOfColumnsToDecimal(ByVal datagrid As DataGridView, ByVal celltocompute As Integer)
        With datagrid
            Dim sum As Decimal
            For i As Integer = 0 To .Rows.Count() - 1 Step +1
                sum = sum + .Rows(i).Cells(celltocompute).Value
            Next
            Return Format(sum, "##,##0.00")
        End With
    End Function
    Public Function SumOfColumnsToInt(ByVal datagrid As DataGridView, ByVal celltocompute As Integer)
        Try
            With datagrid
                total = (From row As DataGridViewRow In .Rows
                         Where row.Cells(celltocompute).FormattedValue.ToString() <> String.Empty
                         Select Convert.ToInt32(row.Cells(celltocompute).FormattedValue)).Sum.ToString()
            End With
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
        Return total
    End Function
    Public Sub Numeric(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If e.KeyChar <> ControlChars.Back Then
            e.Handled = Not (Char.IsDigit(e.KeyChar) Or e.KeyChar = ".")
        End If
    End Sub
    Dim ReturnRowIndex
    Public Function getCurrentCellButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        With POS
            If .DataGridViewOrders.Rows.Count > 0 Then
                ReturnRowIndex = .DataGridViewOrders.CurrentCell.RowIndex
            End If
        End With
        Return ReturnRowIndex
    End Function
    Public Sub RightToLeftDisplay(sender As Object, e As PrintPageEventArgs, position As Integer, lefttext As String, righttext As String, myfont As Font, wth As Single, frompoint As Single)
        Dim format As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
        Dim rect3 As RectangleF = New RectangleF(10.0F + frompoint, position, 173.0F + wth, 100.0F)
        e.Graphics.DrawString(lefttext, myfont, Brushes.Black, rect3)
        e.Graphics.DrawString(righttext, myfont, Brushes.Black, rect3, format)
    End Sub
    Public Sub RightDisplay1(sender As Object, e As PrintPageEventArgs, position As Integer, lefttext As String, righttext As String, myfont As Font, wth As Single, frompoint As Single)
        Dim format As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
        Dim rect3 As RectangleF = New RectangleF(10.0F + frompoint, position, 0 + wth, 0)
        e.Graphics.DrawString(lefttext, myfont, Brushes.Black, rect3)
        e.Graphics.DrawString(righttext, myfont, Brushes.Black, rect3, format)
    End Sub
    Public Sub RightDisplay(sender As Object, e As PrintPageEventArgs, position As Integer, righttext As String, myfont As Font, wth As Single, frompoint As Single)
        Dim format As StringFormat = New StringFormat(StringFormatFlags.DirectionRightToLeft)
        Dim rect3 As RectangleF = New RectangleF(10.0F + frompoint, position, 120.0F + wth, 100.0F)
        e.Graphics.DrawString(righttext, myfont, Brushes.Black, rect3, format)
    End Sub
    Public Sub CenterTextDisplay(sender As Object, e As PrintPageEventArgs, myText As String, myFont As Font, myPosition As Integer)
        Dim sngCenterPagebrand As Single
        sngCenterPagebrand = Convert.ToSingle(e.PageBounds.Width / 2 - e.Graphics.MeasureString(myText, myFont).Width / 2)
        e.Graphics.DrawString(myText, myFont, Brushes.Black, sngCenterPagebrand, myPosition)
    End Sub
    Public Sub SimpleTextDisplay(sender As Object, e As PrintPageEventArgs, myText As String, myFont As Font, ShopX As Integer, ShopY As Integer)
        Dim shopnameX As Integer = 10, shopnameY As Integer = 20
        e.Graphics.DrawString(myText, myFont, Brushes.Black, New PointF(shopnameX + ShopX, shopnameY + ShopY))
    End Sub
    Public Sub FormIsOpen()
        If Application.OpenForms().OfType(Of SynctoCloud).Any Then
            SynctoCloud.Close()
        End If
    End Sub
    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
    Enum ProgressBarColor
        Green = &H1
        Red = &H2
        Yellow = &H3
    End Enum
    Public Sub ChangeProgBarColor(ByVal ProgressBar_Name As System.Windows.Forms.ProgressBar, ByVal ProgressBar_Color As ProgressBarColor)
        SendMessage(ProgressBar_Name.Handle, &H410, ProgressBar_Color, 0)
    End Sub
    Public Sub ReceiptHeader(sender As Object, e As PrintPageEventArgs)
        Try
            Dim brandfont As New Font("Tahoma", 7, FontStyle.Bold)
            Dim font As New Font("Tahoma", 6)
            Dim brand = ClientBrand.ToUpper
            CenterTextDisplay(sender, e, brand, brandfont, 10)
            CenterTextDisplay(sender, e, "VAT REG TIN " & ClientTin, font, 21)
            CenterTextDisplay(sender, e, ClientAddress & ", Brgy." & ClientBrgy, font, 31)
            CenterTextDisplay(sender, e, getmunicipality & ", " & getprovince, font, 41)
            CenterTextDisplay(sender, e, "TEL. NO.: " & ClientTel, font, 51)
            SimpleTextDisplay(sender, e, "Name:", font, 0, 50)
            If SENIORDETAILSBOOL = True Then
                SimpleTextDisplay(sender, e, SeniorDetailsName, font, 30, 45)
            End If
            e.Graphics.DrawLine(Pens.Black, 40, 77, 180, 77)
            SimpleTextDisplay(sender, e, "Tin:", font, 0, 60)
            e.Graphics.DrawLine(Pens.Black, 28, 87, 180, 87)
            SimpleTextDisplay(sender, e, "Address:", font, 0, 70)
            e.Graphics.DrawLine(Pens.Black, 49, 97, 180, 97)
            SimpleTextDisplay(sender, e, "Business Style:", font, 0, 80)
            e.Graphics.DrawLine(Pens.Black, 75, 107, 180, 107)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
    Public Sub ReceiptFooter(sender As Object, e As PrintPageEventArgs, a As Integer)
        Try
            Dim sql As String = "SELECT `Dev_Company_Name`, `Dev_Address`, `Dev_Tin`, `Dev_Accr_No`, `Dev_Accr_Date_Issued`, `Dev_Accr_Valid_Until`, `Dev_PTU_No`, `Dev_PTU_Date_Issued`, `Dev_PTU_Valid_Until` FROM loc_settings WHERE settings_id = 1"
            Dim cmd As MySqlCommand = New MySqlCommand(sql, LocalhostConn())
            Dim da As MySqlDataAdapter = New MySqlDataAdapter(cmd)
            Dim dt As DataTable = New DataTable
            da.Fill(dt)
            Dim brandfont As New Font("Tahoma", 7, FontStyle.Bold)
            Dim font As New Font("Tahoma", 6)
            CenterTextDisplay(sender, e, dt(0)(0).ToUpper, brandfont, a + 195)
            CenterTextDisplay(sender, e, "VAT REG TIN : " & dt(0)(2).ToString, font, a + 205)
            CenterTextDisplay(sender, e, dt(0)(1), font, a + 215)
            CenterTextDisplay(sender, e, "ACCR # : " & dt(0)(3), font, a + 225)
            CenterTextDisplay(sender, e, "DATE ISSUED : " & dt(0)(4), font, a + 235)
            CenterTextDisplay(sender, e, "VALID UNTIL : " & dt(0)(5), font, a + 245)
            CenterTextDisplay(sender, e, "PERMIT TO OPERATE : " & dt(0)(6), font, a + 255)
            CenterTextDisplay(sender, e, "DATE ISSUED : " & dt(0)(7), font, a + 265)
            CenterTextDisplay(sender, e, "VALID UNTIL : " & dt(0)(8), font, a + 275)
        Catch ex As Exception
            MsgBox(ex.ToString)
            SendErrorReport(ex.ToString)
        End Try
    End Sub
End Module
