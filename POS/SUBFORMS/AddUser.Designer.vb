<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddUser
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
        Me.ButtonUser = New System.Windows.Forms.Button()
        Me.RadioButtonFEMALE = New System.Windows.Forms.RadioButton()
        Me.RadioButtonMALE = New System.Windows.Forms.RadioButton()
        Me.TextBoxFULLNAME = New System.Windows.Forms.TextBox()
        Me.TextBoxUSERNAME = New System.Windows.Forms.TextBox()
        Me.TextBoxEMAIL = New System.Windows.Forms.TextBox()
        Me.TextBoxPASS = New System.Windows.Forms.TextBox()
        Me.TextBoxCONPASS = New System.Windows.Forms.TextBox()
        Me.TextBoxCONTACT = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.SuspendLayout()
        '
        'ButtonUser
        '
        Me.ButtonUser.BackColor = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(46, Byte), Integer))
        Me.ButtonUser.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonUser.FlatAppearance.BorderSize = 0
        Me.ButtonUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonUser.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.ButtonUser.ForeColor = System.Drawing.Color.White
        Me.ButtonUser.Location = New System.Drawing.Point(135, 206)
        Me.ButtonUser.Name = "ButtonUser"
        Me.ButtonUser.Size = New System.Drawing.Size(275, 29)
        Me.ButtonUser.TabIndex = 193
        Me.ButtonUser.Text = "Submit"
        Me.ButtonUser.UseVisualStyleBackColor = False
        '
        'RadioButtonFEMALE
        '
        Me.RadioButtonFEMALE.AutoSize = True
        Me.RadioButtonFEMALE.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButtonFEMALE.Location = New System.Drawing.Point(194, 180)
        Me.RadioButtonFEMALE.Name = "RadioButtonFEMALE"
        Me.RadioButtonFEMALE.Size = New System.Drawing.Size(68, 20)
        Me.RadioButtonFEMALE.TabIndex = 192
        Me.RadioButtonFEMALE.Text = "Female"
        Me.RadioButtonFEMALE.UseVisualStyleBackColor = True
        '
        'RadioButtonMALE
        '
        Me.RadioButtonMALE.AutoSize = True
        Me.RadioButtonMALE.Checked = True
        Me.RadioButtonMALE.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioButtonMALE.Location = New System.Drawing.Point(135, 180)
        Me.RadioButtonMALE.Name = "RadioButtonMALE"
        Me.RadioButtonMALE.Size = New System.Drawing.Size(53, 20)
        Me.RadioButtonMALE.TabIndex = 191
        Me.RadioButtonMALE.TabStop = True
        Me.RadioButtonMALE.Text = "Male"
        Me.RadioButtonMALE.UseVisualStyleBackColor = True
        '
        'TextBoxFULLNAME
        '
        Me.TextBoxFULLNAME.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxFULLNAME.Location = New System.Drawing.Point(135, 6)
        Me.TextBoxFULLNAME.Name = "TextBoxFULLNAME"
        Me.TextBoxFULLNAME.Size = New System.Drawing.Size(275, 23)
        Me.TextBoxFULLNAME.TabIndex = 185
        '
        'TextBoxUSERNAME
        '
        Me.TextBoxUSERNAME.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxUSERNAME.Location = New System.Drawing.Point(135, 35)
        Me.TextBoxUSERNAME.Name = "TextBoxUSERNAME"
        Me.TextBoxUSERNAME.Size = New System.Drawing.Size(275, 23)
        Me.TextBoxUSERNAME.TabIndex = 186
        '
        'TextBoxEMAIL
        '
        Me.TextBoxEMAIL.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxEMAIL.Location = New System.Drawing.Point(135, 64)
        Me.TextBoxEMAIL.Name = "TextBoxEMAIL"
        Me.TextBoxEMAIL.Size = New System.Drawing.Size(275, 23)
        Me.TextBoxEMAIL.TabIndex = 187
        '
        'TextBoxPASS
        '
        Me.TextBoxPASS.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxPASS.Location = New System.Drawing.Point(135, 93)
        Me.TextBoxPASS.Name = "TextBoxPASS"
        Me.TextBoxPASS.Size = New System.Drawing.Size(275, 23)
        Me.TextBoxPASS.TabIndex = 188
        Me.TextBoxPASS.UseSystemPasswordChar = True
        '
        'TextBoxCONPASS
        '
        Me.TextBoxCONPASS.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxCONPASS.Location = New System.Drawing.Point(135, 122)
        Me.TextBoxCONPASS.Name = "TextBoxCONPASS"
        Me.TextBoxCONPASS.Size = New System.Drawing.Size(275, 23)
        Me.TextBoxCONPASS.TabIndex = 189
        Me.TextBoxCONPASS.UseSystemPasswordChar = True
        '
        'TextBoxCONTACT
        '
        Me.TextBoxCONTACT.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxCONTACT.Location = New System.Drawing.Point(135, 151)
        Me.TextBoxCONTACT.Name = "TextBoxCONTACT"
        Me.TextBoxCONTACT.Size = New System.Drawing.Size(275, 23)
        Me.TextBoxCONTACT.TabIndex = 190
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(12, 67)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(44, 16)
        Me.Label8.TabIndex = 200
        Me.Label8.Text = "Email:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 16)
        Me.Label2.TabIndex = 195
        Me.Label2.Text = "Full Name:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(12, 154)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(105, 16)
        Me.Label7.TabIndex = 199
        Me.Label7.Text = "Contact Number:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(12, 125)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(117, 16)
        Me.Label6.TabIndex = 198
        Me.Label6.Text = "Confirm Password:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 96)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(68, 16)
        Me.Label5.TabIndex = 197
        Me.Label5.Text = "Password:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(71, 16)
        Me.Label4.TabIndex = 196
        Me.Label4.Text = "Username:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 182)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 16)
        Me.Label1.TabIndex = 201
        Me.Label1.Text = "Gender:"
        '
        'AddUser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(419, 245)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ButtonUser)
        Me.Controls.Add(Me.RadioButtonFEMALE)
        Me.Controls.Add(Me.RadioButtonMALE)
        Me.Controls.Add(Me.TextBoxFULLNAME)
        Me.Controls.Add(Me.TextBoxUSERNAME)
        Me.Controls.Add(Me.TextBoxEMAIL)
        Me.Controls.Add(Me.TextBoxPASS)
        Me.Controls.Add(Me.TextBoxCONPASS)
        Me.Controls.Add(Me.TextBoxCONTACT)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AddUser"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonUser As Button
    Friend WithEvents RadioButtonFEMALE As RadioButton
    Friend WithEvents RadioButtonMALE As RadioButton
    Friend WithEvents TextBoxFULLNAME As TextBox
    Friend WithEvents TextBoxUSERNAME As TextBox
    Friend WithEvents TextBoxEMAIL As TextBox
    Friend WithEvents TextBoxPASS As TextBox
    Friend WithEvents TextBoxCONPASS As TextBox
    Friend WithEvents TextBoxCONTACT As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
End Class
