<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TransactionTypeInfo
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
        Me.TextBoxFULLNAME = New System.Windows.Forms.TextBox()
        Me.TextBoxREFERENCE = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ButtonESC = New System.Windows.Forms.Button()
        Me.TextBoxMARKUP = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextBoxFULLNAME
        '
        Me.TextBoxFULLNAME.Font = New System.Drawing.Font("Kelson Sans Normal", 12.0!)
        Me.TextBoxFULLNAME.Location = New System.Drawing.Point(7, 27)
        Me.TextBoxFULLNAME.Name = "TextBoxFULLNAME"
        Me.TextBoxFULLNAME.Size = New System.Drawing.Size(360, 27)
        Me.TextBoxFULLNAME.TabIndex = 0
        '
        'TextBoxREFERENCE
        '
        Me.TextBoxREFERENCE.Font = New System.Drawing.Font("Kelson Sans Normal", 12.0!)
        Me.TextBoxREFERENCE.Location = New System.Drawing.Point(7, 79)
        Me.TextBoxREFERENCE.Name = "TextBoxREFERENCE"
        Me.TextBoxREFERENCE.Size = New System.Drawing.Size(277, 27)
        Me.TextBoxREFERENCE.TabIndex = 1
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(46, Byte), Integer))
        Me.Button2.FlatAppearance.BorderSize = 0
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Kelson Sans Normal", 12.0!)
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(7, 112)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(242, 36)
        Me.Button2.TabIndex = 103
        Me.Button2.Text = "Submit"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Kelson Sans Normal", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 19)
        Me.Label1.TabIndex = 104
        Me.Label1.Text = "Full Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Kelson Sans Normal", 12.0!)
        Me.Label2.Location = New System.Drawing.Point(3, 57)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(143, 19)
        Me.Label2.TabIndex = 105
        Me.Label2.Text = "Reference Number"
        '
        'ButtonESC
        '
        Me.ButtonESC.BackColor = System.Drawing.Color.Firebrick
        Me.ButtonESC.FlatAppearance.BorderSize = 0
        Me.ButtonESC.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonESC.Font = New System.Drawing.Font("Kelson Sans Normal", 12.0!)
        Me.ButtonESC.ForeColor = System.Drawing.Color.White
        Me.ButtonESC.Location = New System.Drawing.Point(254, 112)
        Me.ButtonESC.Name = "ButtonESC"
        Me.ButtonESC.Size = New System.Drawing.Size(112, 36)
        Me.ButtonESC.TabIndex = 106
        Me.ButtonESC.Text = "Cancel"
        Me.ButtonESC.UseVisualStyleBackColor = False
        '
        'TextBoxMARKUP
        '
        Me.TextBoxMARKUP.Font = New System.Drawing.Font("Kelson Sans Normal", 12.0!)
        Me.TextBoxMARKUP.Location = New System.Drawing.Point(289, 79)
        Me.TextBoxMARKUP.Name = "TextBoxMARKUP"
        Me.TextBoxMARKUP.ReadOnly = True
        Me.TextBoxMARKUP.Size = New System.Drawing.Size(77, 27)
        Me.TextBoxMARKUP.TabIndex = 107
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Kelson Sans Normal", 12.0!)
        Me.Label3.Location = New System.Drawing.Point(285, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 19)
        Me.Label3.TabIndex = 108
        Me.Label3.Text = "Mark Up"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.TextBoxFULLNAME)
        Me.Panel1.Controls.Add(Me.TextBoxMARKUP)
        Me.Panel1.Controls.Add(Me.TextBoxREFERENCE)
        Me.Panel1.Controls.Add(Me.ButtonESC)
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(374, 156)
        Me.Panel1.TabIndex = 109
        '
        'TransactionTypeInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(374, 156)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "TransactionTypeInfo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TextBoxFULLNAME As TextBox
    Friend WithEvents TextBoxREFERENCE As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ButtonESC As Button
    Friend WithEvents TextBoxMARKUP As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Panel1 As Panel
End Class
