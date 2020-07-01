<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddBank
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.TextBoxBankName = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.999999!)
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(9, 15)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 15)
        Me.Label2.TabIndex = 168
        Me.Label2.Text = "Bank Name:"
        '
        'Button11
        '
        Me.Button11.BackColor = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(46, Byte), Integer))
        Me.Button11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button11.FlatAppearance.BorderSize = 0
        Me.Button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.999999!)
        Me.Button11.ForeColor = System.Drawing.Color.White
        Me.Button11.Location = New System.Drawing.Point(12, 40)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(296, 28)
        Me.Button11.TabIndex = 167
        Me.Button11.Text = "Submit"
        Me.Button11.UseVisualStyleBackColor = False
        '
        'TextBoxBankName
        '
        Me.TextBoxBankName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.999999!)
        Me.TextBoxBankName.Location = New System.Drawing.Point(94, 12)
        Me.TextBoxBankName.Name = "TextBoxBankName"
        Me.TextBoxBankName.Size = New System.Drawing.Size(214, 21)
        Me.TextBoxBankName.TabIndex = 166
        '
        'AddBank
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(320, 80)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Button11)
        Me.Controls.Add(Me.TextBoxBankName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "AddBank"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "AddBank"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As Label
    Friend WithEvents Button11 As Button
    Friend WithEvents TextBoxBankName As TextBox
End Class
