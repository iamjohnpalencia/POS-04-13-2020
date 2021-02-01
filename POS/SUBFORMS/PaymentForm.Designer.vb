<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PaymentForm
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
        Me.components = New System.ComponentModel.Container()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.ButtonClear = New System.Windows.Forms.Button()
        Me.TextBoxTransactionType = New System.Windows.Forms.TextBox()
        Me.ButtonNo00 = New System.Windows.Forms.Button()
        Me.Buttondot = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ButtonNo0 = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ButtonNo3 = New System.Windows.Forms.Button()
        Me.ButtonNo2 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ButtonNo1 = New System.Windows.Forms.Button()
        Me.ButtonESC = New System.Windows.Forms.Button()
        Me.ButtonNo5 = New System.Windows.Forms.Button()
        Me.ButtonNo6 = New System.Windows.Forms.Button()
        Me.ButtonSubmitPayment = New System.Windows.Forms.Button()
        Me.ButtonNo4 = New System.Windows.Forms.Button()
        Me.TextBoxCHANGE = New System.Windows.Forms.TextBox()
        Me.ButtonNo8 = New System.Windows.Forms.Button()
        Me.ButtonNo9 = New System.Windows.Forms.Button()
        Me.TextBoxTOTALPAY = New System.Windows.Forms.TextBox()
        Me.ButtonNo7 = New System.Windows.Forms.Button()
        Me.TextBoxMONEY = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.ButtonClear)
        Me.Panel1.Controls.Add(Me.TextBoxTransactionType)
        Me.Panel1.Controls.Add(Me.ButtonNo00)
        Me.Panel1.Controls.Add(Me.Buttondot)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.ButtonNo0)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ButtonNo3)
        Me.Panel1.Controls.Add(Me.ButtonNo2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.ButtonNo1)
        Me.Panel1.Controls.Add(Me.ButtonESC)
        Me.Panel1.Controls.Add(Me.ButtonNo5)
        Me.Panel1.Controls.Add(Me.ButtonNo6)
        Me.Panel1.Controls.Add(Me.ButtonSubmitPayment)
        Me.Panel1.Controls.Add(Me.ButtonNo4)
        Me.Panel1.Controls.Add(Me.TextBoxCHANGE)
        Me.Panel1.Controls.Add(Me.ButtonNo8)
        Me.Panel1.Controls.Add(Me.ButtonNo9)
        Me.Panel1.Controls.Add(Me.TextBoxTOTALPAY)
        Me.Panel1.Controls.Add(Me.ButtonNo7)
        Me.Panel1.Controls.Add(Me.TextBoxMONEY)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(372, 477)
        Me.Panel1.TabIndex = 0
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.White
        Me.Button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button2.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.Button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button2.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.Button2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.Button2.Location = New System.Drawing.Point(181, 187)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(180, 51)
        Me.Button2.TabIndex = 111
        Me.Button2.Text = "⟵Backspace"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'ButtonClear
        '
        Me.ButtonClear.BackColor = System.Drawing.Color.White
        Me.ButtonClear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonClear.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonClear.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonClear.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.ButtonClear.Location = New System.Drawing.Point(9, 187)
        Me.ButtonClear.Name = "ButtonClear"
        Me.ButtonClear.Size = New System.Drawing.Size(166, 51)
        Me.ButtonClear.TabIndex = 110
        Me.ButtonClear.Text = "Clear"
        Me.ButtonClear.UseVisualStyleBackColor = False
        '
        'TextBoxTransactionType
        '
        Me.TextBoxTransactionType.BackColor = System.Drawing.Color.White
        Me.TextBoxTransactionType.Enabled = False
        Me.TextBoxTransactionType.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxTransactionType.ForeColor = System.Drawing.Color.DimGray
        Me.TextBoxTransactionType.Location = New System.Drawing.Point(9, 91)
        Me.TextBoxTransactionType.Name = "TextBoxTransactionType"
        Me.TextBoxTransactionType.ReadOnly = True
        Me.TextBoxTransactionType.Size = New System.Drawing.Size(352, 27)
        Me.TextBoxTransactionType.TabIndex = 109
        Me.TextBoxTransactionType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ButtonNo00
        '
        Me.ButtonNo00.BackColor = System.Drawing.Color.White
        Me.ButtonNo00.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonNo00.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonNo00.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNo00.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonNo00.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.ButtonNo00.Location = New System.Drawing.Point(181, 415)
        Me.ButtonNo00.Name = "ButtonNo00"
        Me.ButtonNo00.Size = New System.Drawing.Size(80, 51)
        Me.ButtonNo00.TabIndex = 82
        Me.ButtonNo00.Text = "00"
        Me.ButtonNo00.UseVisualStyleBackColor = False
        '
        'Buttondot
        '
        Me.Buttondot.BackColor = System.Drawing.Color.White
        Me.Buttondot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Buttondot.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.Buttondot.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Buttondot.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.Buttondot.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.Buttondot.Location = New System.Drawing.Point(9, 415)
        Me.Buttondot.Name = "Buttondot"
        Me.Buttondot.Size = New System.Drawing.Size(80, 51)
        Me.Buttondot.TabIndex = 84
        Me.Buttondot.Text = "."
        Me.Buttondot.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 67)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(123, 18)
        Me.Label4.TabIndex = 108
        Me.Label4.Text = "Transaction Type"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(264, 121)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 18)
        Me.Label3.TabIndex = 107
        Me.Label3.Text = "Change"
        '
        'ButtonNo0
        '
        Me.ButtonNo0.BackColor = System.Drawing.Color.White
        Me.ButtonNo0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonNo0.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonNo0.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNo0.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonNo0.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.ButtonNo0.Location = New System.Drawing.Point(95, 415)
        Me.ButtonNo0.Name = "ButtonNo0"
        Me.ButtonNo0.Size = New System.Drawing.Size(80, 51)
        Me.ButtonNo0.TabIndex = 81
        Me.ButtonNo0.Text = "0"
        Me.ButtonNo0.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(114, 18)
        Me.Label2.TabIndex = 106
        Me.Label2.Text = "Amount Payable"
        '
        'ButtonNo3
        '
        Me.ButtonNo3.BackColor = System.Drawing.Color.White
        Me.ButtonNo3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonNo3.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonNo3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNo3.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonNo3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.ButtonNo3.Location = New System.Drawing.Point(181, 358)
        Me.ButtonNo3.Name = "ButtonNo3"
        Me.ButtonNo3.Size = New System.Drawing.Size(80, 51)
        Me.ButtonNo3.TabIndex = 80
        Me.ButtonNo3.Text = "3"
        Me.ButtonNo3.UseVisualStyleBackColor = False
        '
        'ButtonNo2
        '
        Me.ButtonNo2.BackColor = System.Drawing.Color.White
        Me.ButtonNo2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonNo2.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonNo2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNo2.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonNo2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.ButtonNo2.Location = New System.Drawing.Point(95, 358)
        Me.ButtonNo2.Name = "ButtonNo2"
        Me.ButtonNo2.Size = New System.Drawing.Size(80, 51)
        Me.ButtonNo2.TabIndex = 79
        Me.ButtonNo2.Text = "2"
        Me.ButtonNo2.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 121)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(127, 18)
        Me.Label1.TabIndex = 105
        Me.Label1.Text = "Amount Tendered"
        '
        'ButtonNo1
        '
        Me.ButtonNo1.BackColor = System.Drawing.Color.White
        Me.ButtonNo1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonNo1.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonNo1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNo1.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonNo1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.ButtonNo1.Location = New System.Drawing.Point(9, 358)
        Me.ButtonNo1.Name = "ButtonNo1"
        Me.ButtonNo1.Size = New System.Drawing.Size(80, 51)
        Me.ButtonNo1.TabIndex = 78
        Me.ButtonNo1.Text = "1"
        Me.ButtonNo1.UseVisualStyleBackColor = False
        '
        'ButtonESC
        '
        Me.ButtonESC.BackColor = System.Drawing.Color.Firebrick
        Me.ButtonESC.FlatAppearance.BorderSize = 0
        Me.ButtonESC.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonESC.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonESC.ForeColor = System.Drawing.Color.White
        Me.ButtonESC.Location = New System.Drawing.Point(267, 415)
        Me.ButtonESC.Name = "ButtonESC"
        Me.ButtonESC.Size = New System.Drawing.Size(94, 51)
        Me.ButtonESC.TabIndex = 98
        Me.ButtonESC.Text = "Cancel"
        Me.ButtonESC.UseVisualStyleBackColor = False
        '
        'ButtonNo5
        '
        Me.ButtonNo5.BackColor = System.Drawing.Color.White
        Me.ButtonNo5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonNo5.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonNo5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNo5.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonNo5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.ButtonNo5.Location = New System.Drawing.Point(95, 301)
        Me.ButtonNo5.Name = "ButtonNo5"
        Me.ButtonNo5.Size = New System.Drawing.Size(80, 51)
        Me.ButtonNo5.TabIndex = 76
        Me.ButtonNo5.Text = "5"
        Me.ButtonNo5.UseVisualStyleBackColor = False
        '
        'ButtonNo6
        '
        Me.ButtonNo6.BackColor = System.Drawing.Color.White
        Me.ButtonNo6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonNo6.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonNo6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNo6.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonNo6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.ButtonNo6.Location = New System.Drawing.Point(181, 301)
        Me.ButtonNo6.Name = "ButtonNo6"
        Me.ButtonNo6.Size = New System.Drawing.Size(80, 51)
        Me.ButtonNo6.TabIndex = 74
        Me.ButtonNo6.Text = "6"
        Me.ButtonNo6.UseVisualStyleBackColor = False
        '
        'ButtonSubmitPayment
        '
        Me.ButtonSubmitPayment.BackColor = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(46, Byte), Integer))
        Me.ButtonSubmitPayment.FlatAppearance.BorderSize = 0
        Me.ButtonSubmitPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonSubmitPayment.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonSubmitPayment.ForeColor = System.Drawing.Color.White
        Me.ButtonSubmitPayment.Location = New System.Drawing.Point(267, 244)
        Me.ButtonSubmitPayment.Name = "ButtonSubmitPayment"
        Me.ButtonSubmitPayment.Size = New System.Drawing.Size(94, 165)
        Me.ButtonSubmitPayment.TabIndex = 97
        Me.ButtonSubmitPayment.Text = "Check Out"
        Me.ButtonSubmitPayment.UseVisualStyleBackColor = False
        '
        'ButtonNo4
        '
        Me.ButtonNo4.BackColor = System.Drawing.Color.White
        Me.ButtonNo4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonNo4.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonNo4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNo4.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonNo4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.ButtonNo4.Location = New System.Drawing.Point(9, 301)
        Me.ButtonNo4.Name = "ButtonNo4"
        Me.ButtonNo4.Size = New System.Drawing.Size(80, 51)
        Me.ButtonNo4.TabIndex = 75
        Me.ButtonNo4.Text = "4"
        Me.ButtonNo4.UseVisualStyleBackColor = False
        '
        'TextBoxCHANGE
        '
        Me.TextBoxCHANGE.BackColor = System.Drawing.Color.White
        Me.TextBoxCHANGE.Enabled = False
        Me.TextBoxCHANGE.Font = New System.Drawing.Font("Tahoma", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxCHANGE.ForeColor = System.Drawing.Color.DimGray
        Me.TextBoxCHANGE.Location = New System.Drawing.Point(231, 145)
        Me.TextBoxCHANGE.Name = "TextBoxCHANGE"
        Me.TextBoxCHANGE.ReadOnly = True
        Me.TextBoxCHANGE.Size = New System.Drawing.Size(130, 36)
        Me.TextBoxCHANGE.TabIndex = 96
        Me.TextBoxCHANGE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ButtonNo8
        '
        Me.ButtonNo8.BackColor = System.Drawing.Color.White
        Me.ButtonNo8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonNo8.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonNo8.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNo8.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonNo8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.ButtonNo8.Location = New System.Drawing.Point(95, 244)
        Me.ButtonNo8.Name = "ButtonNo8"
        Me.ButtonNo8.Size = New System.Drawing.Size(80, 51)
        Me.ButtonNo8.TabIndex = 73
        Me.ButtonNo8.Text = "8"
        Me.ButtonNo8.UseVisualStyleBackColor = False
        '
        'ButtonNo9
        '
        Me.ButtonNo9.BackColor = System.Drawing.Color.White
        Me.ButtonNo9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonNo9.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonNo9.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNo9.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonNo9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.ButtonNo9.Location = New System.Drawing.Point(181, 244)
        Me.ButtonNo9.Name = "ButtonNo9"
        Me.ButtonNo9.Size = New System.Drawing.Size(80, 51)
        Me.ButtonNo9.TabIndex = 77
        Me.ButtonNo9.Text = "9"
        Me.ButtonNo9.UseVisualStyleBackColor = False
        '
        'TextBoxTOTALPAY
        '
        Me.TextBoxTOTALPAY.BackColor = System.Drawing.Color.White
        Me.TextBoxTOTALPAY.Enabled = False
        Me.TextBoxTOTALPAY.Font = New System.Drawing.Font("Tahoma", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxTOTALPAY.ForeColor = System.Drawing.Color.DimGray
        Me.TextBoxTOTALPAY.Location = New System.Drawing.Point(9, 28)
        Me.TextBoxTOTALPAY.Name = "TextBoxTOTALPAY"
        Me.TextBoxTOTALPAY.ReadOnly = True
        Me.TextBoxTOTALPAY.Size = New System.Drawing.Size(352, 36)
        Me.TextBoxTOTALPAY.TabIndex = 94
        Me.TextBoxTOTALPAY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ButtonNo7
        '
        Me.ButtonNo7.BackColor = System.Drawing.Color.White
        Me.ButtonNo7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonNo7.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonNo7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNo7.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonNo7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.ButtonNo7.Location = New System.Drawing.Point(9, 244)
        Me.ButtonNo7.Name = "ButtonNo7"
        Me.ButtonNo7.Size = New System.Drawing.Size(80, 51)
        Me.ButtonNo7.TabIndex = 72
        Me.ButtonNo7.Text = "7"
        Me.ButtonNo7.UseVisualStyleBackColor = False
        '
        'TextBoxMONEY
        '
        Me.TextBoxMONEY.BackColor = System.Drawing.Color.White
        Me.TextBoxMONEY.Font = New System.Drawing.Font("Tahoma", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxMONEY.ForeColor = System.Drawing.Color.DimGray
        Me.TextBoxMONEY.Location = New System.Drawing.Point(9, 145)
        Me.TextBoxMONEY.MaxLength = 10
        Me.TextBoxMONEY.Name = "TextBoxMONEY"
        Me.TextBoxMONEY.Size = New System.Drawing.Size(216, 36)
        Me.TextBoxMONEY.TabIndex = 95
        Me.TextBoxMONEY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PaymentForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(372, 477)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "PaymentForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PaymentForm"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents TextBoxCHANGE As TextBox
    Friend WithEvents TextBoxTOTALPAY As TextBox
    Friend WithEvents TextBoxMONEY As TextBox
    Friend WithEvents ButtonESC As Button
    Friend WithEvents ButtonSubmitPayment As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBoxTransactionType As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Buttondot As Button
    Friend WithEvents ButtonNo00 As Button
    Friend WithEvents ButtonNo0 As Button
    Friend WithEvents ButtonNo2 As Button
    Friend WithEvents ButtonNo1 As Button
    Friend WithEvents ButtonNo5 As Button
    Friend WithEvents ButtonNo4 As Button
    Friend WithEvents ButtonNo8 As Button
    Friend WithEvents ButtonNo7 As Button
    Friend WithEvents ButtonNo3 As Button
    Friend WithEvents ButtonNo9 As Button
    Friend WithEvents ButtonNo6 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents ButtonClear As Button
End Class
