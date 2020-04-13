<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddDepositSlip
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Button12 = New System.Windows.Forms.Button()
        Me.DateTimePickerDATE = New System.Windows.Forms.DateTimePicker()
        Me.ComboBoxBankName = New System.Windows.Forms.ComboBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.TextBoxAMT = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.TextBoxTRANNUM = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.TextBoxNAME = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 139)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(92, 13)
        Me.Label3.TabIndex = 257
        Me.Label3.Text = "Transaction Date:"
        '
        'Button12
        '
        Me.Button12.BackColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.Button12.FlatAppearance.BorderSize = 0
        Me.Button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button12.Font = New System.Drawing.Font("Kelson Sans Normal", 8.999999!)
        Me.Button12.ForeColor = System.Drawing.Color.White
        Me.Button12.Location = New System.Drawing.Point(135, 163)
        Me.Button12.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Button12.Name = "Button12"
        Me.Button12.Size = New System.Drawing.Size(233, 23)
        Me.Button12.TabIndex = 255
        Me.Button12.Text = "SUBMIT"
        Me.Button12.UseVisualStyleBackColor = False
        '
        'DateTimePickerDATE
        '
        Me.DateTimePickerDATE.Font = New System.Drawing.Font("Kelson Sans Normal", 8.999999!)
        Me.DateTimePickerDATE.Location = New System.Drawing.Point(135, 133)
        Me.DateTimePickerDATE.Name = "DateTimePickerDATE"
        Me.DateTimePickerDATE.Size = New System.Drawing.Size(233, 22)
        Me.DateTimePickerDATE.TabIndex = 256
        '
        'ComboBoxBankName
        '
        Me.ComboBoxBankName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxBankName.Font = New System.Drawing.Font("Kelson Sans Normal", 8.999999!)
        Me.ComboBoxBankName.FormattingEnabled = True
        Me.ComboBoxBankName.Location = New System.Drawing.Point(135, 103)
        Me.ComboBoxBankName.Name = "ComboBoxBankName"
        Me.ComboBoxBankName.Size = New System.Drawing.Size(233, 22)
        Me.ComboBoxBankName.TabIndex = 258
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(12, 45)
        Me.Label18.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(106, 13)
        Me.Label18.TabIndex = 252
        Me.Label18.Text = "Transaction Number:"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(12, 106)
        Me.Label16.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(35, 13)
        Me.Label16.TabIndex = 254
        Me.Label16.Text = "Bank:"
        '
        'TextBoxAMT
        '
        Me.TextBoxAMT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxAMT.Font = New System.Drawing.Font("Kelson Sans Normal", 8.999999!)
        Me.TextBoxAMT.Location = New System.Drawing.Point(135, 73)
        Me.TextBoxAMT.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TextBoxAMT.Name = "TextBoxAMT"
        Me.TextBoxAMT.Size = New System.Drawing.Size(233, 22)
        Me.TextBoxAMT.TabIndex = 250
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(12, 75)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(46, 13)
        Me.Label17.TabIndex = 253
        Me.Label17.Text = "Amount:"
        '
        'TextBoxTRANNUM
        '
        Me.TextBoxTRANNUM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTRANNUM.Font = New System.Drawing.Font("Kelson Sans Normal", 8.999999!)
        Me.TextBoxTRANNUM.Location = New System.Drawing.Point(135, 43)
        Me.TextBoxTRANNUM.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TextBoxTRANNUM.Name = "TextBoxTRANNUM"
        Me.TextBoxTRANNUM.Size = New System.Drawing.Size(233, 22)
        Me.TextBoxTRANNUM.TabIndex = 249
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(12, 15)
        Me.Label19.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(38, 13)
        Me.Label19.TabIndex = 251
        Me.Label19.Text = "Name:"
        '
        'TextBoxNAME
        '
        Me.TextBoxNAME.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxNAME.Font = New System.Drawing.Font("Kelson Sans Normal", 8.999999!)
        Me.TextBoxNAME.Location = New System.Drawing.Point(135, 13)
        Me.TextBoxNAME.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TextBoxNAME.Name = "TextBoxNAME"
        Me.TextBoxNAME.Size = New System.Drawing.Size(233, 22)
        Me.TextBoxNAME.TabIndex = 248
        '
        'AddDepositSlip
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(382, 198)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Button12)
        Me.Controls.Add(Me.DateTimePickerDATE)
        Me.Controls.Add(Me.ComboBoxBankName)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.TextBoxAMT)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.TextBoxTRANNUM)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.TextBoxNAME)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "AddDepositSlip"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AddDepositSlip"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label3 As Label
    Friend WithEvents Button12 As Button
    Friend WithEvents DateTimePickerDATE As DateTimePicker
    Friend WithEvents ComboBoxBankName As ComboBox
    Friend WithEvents Label18 As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents TextBoxAMT As TextBox
    Friend WithEvents Label17 As Label
    Friend WithEvents TextBoxTRANNUM As TextBox
    Friend WithEvents Label19 As Label
    Friend WithEvents TextBoxNAME As TextBox
End Class
