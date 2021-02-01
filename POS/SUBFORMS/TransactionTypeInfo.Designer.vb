<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TransactionTypeInfo
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TransactionTypeInfo))
        Me.TextBoxFULLNAME = New System.Windows.Forms.TextBox()
        Me.TextBoxREFERENCE = New System.Windows.Forms.TextBox()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBoxMARKUP = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ButtonKeyboard = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TextBoxFULLNAME
        '
        Me.TextBoxFULLNAME.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.TextBoxFULLNAME.Location = New System.Drawing.Point(8, 26)
        Me.TextBoxFULLNAME.Name = "TextBoxFULLNAME"
        Me.TextBoxFULLNAME.Size = New System.Drawing.Size(348, 26)
        Me.TextBoxFULLNAME.TabIndex = 0
        '
        'TextBoxREFERENCE
        '
        Me.TextBoxREFERENCE.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.TextBoxREFERENCE.Location = New System.Drawing.Point(8, 76)
        Me.TextBoxREFERENCE.Name = "TextBoxREFERENCE"
        Me.TextBoxREFERENCE.Size = New System.Drawing.Size(265, 26)
        Me.TextBoxREFERENCE.TabIndex = 1
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(46, Byte), Integer))
        Me.Button2.FlatAppearance.BorderSize = 0
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(8, 108)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(263, 40)
        Me.Button2.TabIndex = 103
        Me.Button2.Text = "Submit"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.Label1.Location = New System.Drawing.Point(5, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 18)
        Me.Label1.TabIndex = 104
        Me.Label1.Text = "Full Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.Label2.Location = New System.Drawing.Point(5, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(131, 18)
        Me.Label2.TabIndex = 105
        Me.Label2.Text = "Reference Number"
        '
        'TextBoxMARKUP
        '
        Me.TextBoxMARKUP.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.TextBoxMARKUP.Location = New System.Drawing.Point(279, 76)
        Me.TextBoxMARKUP.Name = "TextBoxMARKUP"
        Me.TextBoxMARKUP.ReadOnly = True
        Me.TextBoxMARKUP.Size = New System.Drawing.Size(77, 26)
        Me.TextBoxMARKUP.TabIndex = 107
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.Label3.Location = New System.Drawing.Point(276, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 18)
        Me.Label3.TabIndex = 108
        Me.Label3.Text = "Mark Up"
        '
        'ButtonKeyboard
        '
        Me.ButtonKeyboard.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonKeyboard.BackgroundImage = CType(resources.GetObject("ButtonKeyboard.BackgroundImage"), System.Drawing.Image)
        Me.ButtonKeyboard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ButtonKeyboard.FlatAppearance.BorderSize = 0
        Me.ButtonKeyboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonKeyboard.Location = New System.Drawing.Point(277, 109)
        Me.ButtonKeyboard.Name = "ButtonKeyboard"
        Me.ButtonKeyboard.Size = New System.Drawing.Size(79, 40)
        Me.ButtonKeyboard.TabIndex = 240
        Me.ButtonKeyboard.UseVisualStyleBackColor = False
        '
        'TransactionTypeInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(368, 162)
        Me.Controls.Add(Me.ButtonKeyboard)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBoxFULLNAME)
        Me.Controls.Add(Me.TextBoxMARKUP)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TextBoxREFERENCE)
        Me.Controls.Add(Me.Label2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TransactionTypeInfo"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBoxFULLNAME As TextBox
    Friend WithEvents TextBoxREFERENCE As TextBox
    Friend WithEvents Button2 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBoxMARKUP As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents ButtonKeyboard As Button
End Class
