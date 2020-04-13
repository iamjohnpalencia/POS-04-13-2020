Module LoadModule
    Public Sub configurationmanagerload()
        '    With Configurationmanager
        '        .TabControl1.TabPages(0).Text = "General Settings"
        '        .TabControl1.TabPages(1).Text = "License And Activation"
        '        .TabControl2.TabPages(0).Text = "Connection Settings"
        '        .TabControl2.TabPages(1).Text = "Additional Settings"
        '        '===================================================================================TabControl1 Page 1
        '        .TextBoxSchemaLocal.Text = My.Settings.localname
        '        .TextBoxPortLocal.Text = My.Settings.localport
        '        .TextBoxUsernameLocal.Text = My.Settings.localuser
        '        .TextBoxServerLocal.Text = My.Settings.localserver
        '        .TextBoxPasswordLocal.Text = My.Settings.localpass
        '        .TextBoxSchemaCloud.Text = My.Settings.cloudname
        '        .TextBoxPortCloud.Text = My.Settings.cloudport
        '        .TextBoxUsernameCloud.Text = My.Settings.clouduser
        '        .TextBoxServerCloud.Text = My.Settings.cloudserver
        '        .TextBoxPasswordCloud.Text = My.Settings.cloudpass
        '        '<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        '        '==================TabControl1 Page 2
        '        'Group Box 1
        '        .TextBoxExportPath.Text = My.Settings.exportpath
        '        .TextBoxTax.Text = My.Settings.Tax * 100
        '        .TextBoxSINumber.Text = My.Settings.SINumber
        '        .TextBoxTerminalNo.Text = My.Settings.TerminalNumber
        '        If My.Settings.ZeroOutRated = False Then
        '            .RadioButtonNO.Checked = True
        '        Else
        '            .RadioButtonYES.Checked = True
        '        End If
        '        'Group Box 2

        '        'Group Box 3
        '        .TextBoxDevname.Text = My.Settings.DevCompanyName
        '        .TextBoxDevAdd.Text = My.Settings.DevAddress
        '        .TextBoxDevTIN.Text = My.Settings.DevTinNo
        '        .TextBoxDevAccr.Text = My.Settings.DevAccreditation
        '        If My.Settings.DevAccrDateIssued <> "" Then
        '            .DateTimePicker1ACCRDI.Value = My.Settings.DevAccrDateIssued
        '        End If
        '        If My.Settings.DevAccrValidUntil <> "" Then
        '            .DateTimePicker2ACCRVU.Value = My.Settings.DevAccrValidUntil
        '        End If
        '        If My.Settings.DevPTUDateIssued <> "" Then
        '            .DateTimePicker4PTUDI.Value = My.Settings.DevPTUDateIssued
        '        End If
        '        If My.Settings.DevPTUValidUntil <> "" Then
        '            .DateTimePickerPTUVU.Value = My.Settings.DevPTUValidUntil
        '        End If
        '        .TextBoxDEVPTU.Text = My.Settings.DevPermittoOperate
        '        .DataGridViewOutlets.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
        '        .DataGridViewOutlets.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
        '        .DataGridViewOutlets.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        '        '<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        '    End With
    End Sub
End Module
