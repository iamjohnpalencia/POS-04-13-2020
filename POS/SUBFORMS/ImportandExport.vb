Imports System.IO
Imports System.Text
Imports MySql.Data.MySqlClient
Public Class ImportandExport
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Backuptable()
    End Sub
    Sub Backuptable()
        Dim DatabaseName As String = "BACKUP " + Date.Now.ToString("dd-MM-yyyy HH-mm-ss")
        Try
            Dim asdasd = "mysqldump -uposuser -ppassword2020 pos --tables loc_daily_transaction_details --where=""synced = 'Unsynced'"" >  C:\Users\I.T\Desktop\Export-Path\try.sql"
            'Dim abasdasd = " ""123123""  "
            '  Process.Start("C:\xampp\mysql\bin\mysqldump.exe", " -uposuser -ppassword2020 pos  --tables loc_daily_transaction_details --where="" synced = ""Unsynced""  "" > " & My.Settings.exportpath & "\" & DatabaseName & ".innov")
            'Process.Start("C:\xampp\mysql\bin\mysqldump.exe", "-h localhost -u posuser -p password2020 pos loc_daily_transaction_details --where='synced = 'Unsynced''> """ & My.Settings.exportpath & "\" & DatabaseName & ".innov""")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        table = " `Triggers_admin_daily_transaction_details`"
        fields = "(`loc_details_id`, `product_id`, `product_sku`, `product_name`, `quantity`, `price`, `total`, `crew_id`, `transaction_number`, `active`, `created_at`, `timenow`, `guid`, `store_id`)"
        backuptable2(table:=table, fields:=fields, whatnumber:=0)
    End Sub
    Private Sub backuptable2(ByVal table, ByVal fields, ByVal whatnumber)
        'Try
        '    Dim iamsql
        '    Dim Mypath As String = Path.Combine(My.Settings.exportpath & "\" & "FBWLDT" & Date.Now.ToString("yyyyMMdd-HHmmss") & ".inno")
        '    Dim myWriter As New StreamWriter(Mypath)
        '    Dim myString As New StringBuilder()
        '    With DataGridView1
        '        iamsql = "INSERT INTO " & table & " " & fields & " VALUES "
        '        For i As Integer = 0 To .Rows.Count - 1 Step +1
        '            If whatnumber = 0 Then
        '                iamsql += "(" & .Rows(i).Cells(0).Value & ", " & .Rows(i).Cells(1).Value & ", '" & .Rows(i).Cells(2).Value & "', '" & .Rows(i).Cells(3).Value & "', " & .Rows(i).Cells(4).Value & ", " & .Rows(i).Cells(5).Value & ", " & .Rows(i).Cells(6).Value & ", '" & .Rows(i).Cells(7).Value & "', '" & .Rows(i).Cells(8).Value & "', " & .Rows(i).Cells(9).Value & ",'" & returndateformat(.Rows(i).Cells(10).Value.ToString) & "', '" & returntimeformat(.Rows(i).Cells(11).Value.ToString) & "','" & .Rows(i).Cells(12).Value & "', '" & .Rows(i).Cells(13).Value & "'),"
        '            End If
        '        Next
        '    End With
        '    Dim trimme = iamsql.ToString.Substring(0, iamsql.ToString.Length - 1)
        '    trimme = trimme & ";"
        '    myString = myString.Append(trimme)
        '    myWriter.WriteLine(myString)
        '    myWriter.Close()
        'Catch ex As Exception
        '    MsgBox(ex.ToString)
        'End Try
    End Sub
    Private Sub loaddata()
        Try
            sql = "SELECT `details_id`, `product_id`, `product_sku`, `product_name`, `quantity`, `price`, `total`, `crew_id`, `transaction_number`, `active`, `created_at`, `timenow`, `guid`, `store_id` FROM loc_daily_transaction_details WHERE synced = 'Unsynced'"
            cmd = New MySqlCommand(sql, LocalhostConn())
            da = New MySqlDataAdapter(cmd)
            dt = New DataTable
            da.Fill(dt)
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub ImportandExport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        loaddata()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim psi As New ProcessStartInfo("C:\Users\I.T\Desktop\Export-Path\BatchFiles\backup.bat")
        psi.RedirectStandardError = True
        psi.RedirectStandardOutput = True
        psi.CreateNoWindow = False
        psi.WindowStyle = ProcessWindowStyle.Hidden
        psi.UseShellExecute = False
        Dim process As Process = Process.Start(psi)
    End Sub
End Class