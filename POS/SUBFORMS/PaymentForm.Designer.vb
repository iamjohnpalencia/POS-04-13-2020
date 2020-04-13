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
        Me.TextBoxTransactionType = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ButtonESC = New System.Windows.Forms.Button()
        Me.ButtonSubmitPayment = New System.Windows.Forms.Button()
        Me.TextBoxCHANGE = New System.Windows.Forms.TextBox()
        Me.TextBoxTOTALPAY = New System.Windows.Forms.TextBox()
        Me.TextBoxMONEY = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TextBoxTransactionType)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.ButtonESC)
        Me.Panel1.Controls.Add(Me.ButtonSubmitPayment)
        Me.Panel1.Controls.Add(Me.TextBoxCHANGE)
        Me.Panel1.Controls.Add(Me.TextBoxTOTALPAY)
        Me.Panel1.Controls.Add(Me.TextBoxMONEY)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(457, 233)
        Me.Panel1.TabIndex = 0
        '
        'TextBoxTransactionType
        '
        Me.TextBoxTransactionType.BackColor = System.Drawing.Color.White
        Me.TextBoxTransactionType.Enabled = False
        Me.TextBoxTransactionType.Font = New System.Drawing.Font("Kelson Sans Normal", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxTransactionType.ForeColor = System.Drawing.Color.DimGray
        Me.TextBoxTransactionType.Location = New System.Drawing.Point(7, 91)
        Me.TextBoxTransactionType.Name = "TextBoxTransactionType"
        Me.TextBoxTransactionType.ReadOnly = True
        Me.TextBoxTransactionType.Size = New System.Drawing.Size(440, 27)
        Me.TextBoxTransactionType.TabIndex = 109
        Me.TextBoxTransactionType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Kelson Sans Normal", 11.25!)
        Me.Label4.Location = New System.Drawing.Point(4, 67)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(114, 18)
        Me.Label4.TabIndex = 108
        Me.Label4.Text = "Transaction Type"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Kelson Sans Normal", 11.25!)
        Me.Label3.Location = New System.Drawing.Point(271, 121)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 18)
        Me.Label3.TabIndex = 107
        Me.Label3.Text = "Change"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Kelson Sans Normal", 11.25!)
        Me.Label2.Location = New System.Drawing.Point(4, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(115, 18)
        Me.Label2.TabIndex = 106
        Me.Label2.Text = "Amount Payable"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Kelson Sans Normal", 11.25!)
        Me.Label1.Location = New System.Drawing.Point(4, 121)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(122, 18)
        Me.Label1.TabIndex = 105
        Me.Label1.Text = "Amount Tendered"
        '
        'ButtonESC
        '
        Me.ButtonESC.BackColor = System.Drawing.Color.Firebrick
        Me.ButtonESC.FlatAppearance.BorderSize = 0
        Me.ButtonESC.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonESC.Font = New System.Drawing.Font("Kelson Sans Normal", 11.25!)
        Me.ButtonESC.ForeColor = System.Drawing.Color.White
        Me.ButtonESC.Location = New System.Drawing.Point(313, 188)
        Me.ButtonESC.Name = "ButtonESC"
        Me.ButtonESC.Size = New System.Drawing.Size(134, 37)
        Me.ButtonESC.TabIndex = 98
        Me.ButtonESC.Text = "Cancel"
        Me.ButtonESC.UseVisualStyleBackColor = False
        '
        'ButtonSubmitPayment
        '
        Me.ButtonSubmitPayment.BackColor = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(46, Byte), Integer))
        Me.ButtonSubmitPayment.FlatAppearance.BorderSize = 0
        Me.ButtonSubmitPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonSubmitPayment.Font = New System.Drawing.Font("Kelson Sans Normal", 11.25!)
        Me.ButtonSubmitPayment.ForeColor = System.Drawing.Color.White
        Me.ButtonSubmitPayment.Location = New System.Drawing.Point(7, 188)
        Me.ButtonSubmitPayment.Name = "ButtonSubmitPayment"
        Me.ButtonSubmitPayment.Size = New System.Drawing.Size(300, 37)
        Me.ButtonSubmitPayment.TabIndex = 97
        Me.ButtonSubmitPayment.Text = "Check Out"
        Me.ButtonSubmitPayment.UseVisualStyleBackColor = False
        '
        'TextBoxCHANGE
        '
        Me.TextBoxCHANGE.BackColor = System.Drawing.Color.White
        Me.TextBoxCHANGE.Enabled = False
        Me.TextBoxCHANGE.Font = New System.Drawing.Font("Kelson Sans", 18.0!, System.Drawing.FontStyle.Bold)
        Me.TextBoxCHANGE.ForeColor = System.Drawing.Color.DimGray
        Me.TextBoxCHANGE.Location = New System.Drawing.Point(274, 145)
        Me.TextBoxCHANGE.Name = "TextBoxCHANGE"
        Me.TextBoxCHANGE.ReadOnly = True
        Me.TextBoxCHANGE.Size = New System.Drawing.Size(173, 36)
        Me.TextBoxCHANGE.TabIndex = 96
        Me.TextBoxCHANGE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBoxTOTALPAY
        '
        Me.TextBoxTOTALPAY.BackColor = System.Drawing.Color.White
        Me.TextBoxTOTALPAY.Enabled = False
        Me.TextBoxTOTALPAY.Font = New System.Drawing.Font("Kelson Sans", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxTOTALPAY.ForeColor = System.Drawing.Color.DimGray
        Me.TextBoxTOTALPAY.Location = New System.Drawing.Point(7, 28)
        Me.TextBoxTOTALPAY.Name = "TextBoxTOTALPAY"
        Me.TextBoxTOTALPAY.ReadOnly = True
        Me.TextBoxTOTALPAY.Size = New System.Drawing.Size(440, 36)
        Me.TextBoxTOTALPAY.TabIndex = 94
        Me.TextBoxTOTALPAY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextBoxMONEY
        '
        Me.TextBoxMONEY.BackColor = System.Drawing.Color.White
        Me.TextBoxMONEY.Font = New System.Drawing.Font("Kelson Sans", 18.0!, System.Drawing.FontStyle.Bold)
        Me.TextBoxMONEY.ForeColor = System.Drawing.Color.DimGray
        Me.TextBoxMONEY.Location = New System.Drawing.Point(7, 145)
        Me.TextBoxMONEY.MaxLength = 10
        Me.TextBoxMONEY.Name = "TextBoxMONEY"
        Me.TextBoxMONEY.Size = New System.Drawing.Size(261, 36)
        Me.TextBoxMONEY.TabIndex = 95
        Me.TextBoxMONEY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PaymentForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(457, 233)
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
End Class
