<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HoldOrder
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(HoldOrder))
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ButtonKeyboard = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ButtonHoldOrder = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TextBoxCustomerName = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Button1)
        Me.Panel2.Controls.Add(Me.ButtonKeyboard)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.ButtonHoldOrder)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.TextBoxCustomerName)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(413, 125)
        Me.Panel2.TabIndex = 202
        '
        'ButtonKeyboard
        '
        Me.ButtonKeyboard.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonKeyboard.BackgroundImage = CType(resources.GetObject("ButtonKeyboard.BackgroundImage"), System.Drawing.Image)
        Me.ButtonKeyboard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ButtonKeyboard.FlatAppearance.BorderSize = 0
        Me.ButtonKeyboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonKeyboard.Location = New System.Drawing.Point(342, 82)
        Me.ButtonKeyboard.Name = "ButtonKeyboard"
        Me.ButtonKeyboard.Size = New System.Drawing.Size(61, 35)
        Me.ButtonKeyboard.TabIndex = 230
        Me.ButtonKeyboard.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 21.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(52, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(298, 35)
        Me.Label1.TabIndex = 152
        Me.Label1.Text = "Hold Customers Order"
        '
        'ButtonHoldOrder
        '
        Me.ButtonHoldOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ButtonHoldOrder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonHoldOrder.FlatAppearance.BorderSize = 0
        Me.ButtonHoldOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonHoldOrder.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.ButtonHoldOrder.ForeColor = System.Drawing.Color.White
        Me.ButtonHoldOrder.Location = New System.Drawing.Point(9, 82)
        Me.ButtonHoldOrder.Name = "ButtonHoldOrder"
        Me.ButtonHoldOrder.Size = New System.Drawing.Size(221, 35)
        Me.ButtonHoldOrder.TabIndex = 151
        Me.ButtonHoldOrder.Text = "Hold Order"
        Me.ButtonHoldOrder.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 54)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 18)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Customer Name:"
        '
        'TextBoxCustomerName
        '
        Me.TextBoxCustomerName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBoxCustomerName.Font = New System.Drawing.Font("Tahoma", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxCustomerName.Location = New System.Drawing.Point(132, 53)
        Me.TextBoxCustomerName.Name = "TextBoxCustomerName"
        Me.TextBoxCustomerName.Size = New System.Drawing.Size(271, 19)
        Me.TextBoxCustomerName.TabIndex = 0
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.White
        Me.Label6.Font = New System.Drawing.Font("Century Gothic", 9.75!)
        Me.Label6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.Label6.Location = New System.Drawing.Point(129, 58)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(274, 17)
        Me.Label6.TabIndex = 231
        Me.Label6.Text = "______________________________________"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer), CType(CType(74, Byte), Integer))
        Me.Button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Tahoma", 11.25!)
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(233, 82)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(106, 35)
        Me.Button1.TabIndex = 232
        Me.Button1.Text = "Cancel"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'HoldOrder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(413, 125)
        Me.Controls.Add(Me.Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "HoldOrder"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HoldOrder"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As Panel
    Friend WithEvents ButtonHoldOrder As Button
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBoxCustomerName As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents ButtonKeyboard As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents Button1 As Button
End Class
