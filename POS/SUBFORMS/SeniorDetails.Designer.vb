<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SeniorDetails
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBoxSENIORID = New System.Windows.Forms.TextBox()
        Me.TextBoxSENIORNAME = New System.Windows.Forms.TextBox()
        Me.ButtonCANCEL = New System.Windows.Forms.Button()
        Me.ButtonSubmit = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.TextBoxSENIORID)
        Me.Panel1.Controls.Add(Me.TextBoxSENIORNAME)
        Me.Panel1.Controls.Add(Me.ButtonCANCEL)
        Me.Panel1.Controls.Add(Me.ButtonSubmit)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(374, 156)
        Me.Panel1.TabIndex = 110
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 5)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(128, 19)
        Me.Label1.TabIndex = 104
        Me.Label1.Text = "Senior Citizen ID"
        '
        'TextBoxSENIORID
        '
        Me.TextBoxSENIORID.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxSENIORID.Location = New System.Drawing.Point(7, 27)
        Me.TextBoxSENIORID.Name = "TextBoxSENIORID"
        Me.TextBoxSENIORID.Size = New System.Drawing.Size(360, 27)
        Me.TextBoxSENIORID.TabIndex = 0
        '
        'TextBoxSENIORNAME
        '
        Me.TextBoxSENIORNAME.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxSENIORNAME.Location = New System.Drawing.Point(7, 79)
        Me.TextBoxSENIORNAME.Name = "TextBoxSENIORNAME"
        Me.TextBoxSENIORNAME.Size = New System.Drawing.Size(360, 27)
        Me.TextBoxSENIORNAME.TabIndex = 1
        '
        'ButtonCANCEL
        '
        Me.ButtonCANCEL.BackColor = System.Drawing.Color.Firebrick
        Me.ButtonCANCEL.FlatAppearance.BorderSize = 0
        Me.ButtonCANCEL.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonCANCEL.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonCANCEL.ForeColor = System.Drawing.Color.White
        Me.ButtonCANCEL.Location = New System.Drawing.Point(254, 112)
        Me.ButtonCANCEL.Name = "ButtonCANCEL"
        Me.ButtonCANCEL.Size = New System.Drawing.Size(112, 36)
        Me.ButtonCANCEL.TabIndex = 106
        Me.ButtonCANCEL.Text = "Cancel"
        Me.ButtonCANCEL.UseVisualStyleBackColor = False
        '
        'ButtonSubmit
        '
        Me.ButtonSubmit.BackColor = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(46, Byte), Integer))
        Me.ButtonSubmit.FlatAppearance.BorderSize = 0
        Me.ButtonSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonSubmit.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonSubmit.ForeColor = System.Drawing.Color.White
        Me.ButtonSubmit.Location = New System.Drawing.Point(7, 112)
        Me.ButtonSubmit.Name = "ButtonSubmit"
        Me.ButtonSubmit.Size = New System.Drawing.Size(242, 36)
        Me.ButtonSubmit.TabIndex = 103
        Me.ButtonSubmit.Text = "Submit"
        Me.ButtonSubmit.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 57)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(152, 19)
        Me.Label2.TabIndex = 105
        Me.Label2.Text = "Senior Citizen Name"
        '
        'SeniorDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(374, 156)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "SeniorDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "SeniorDetails"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBoxSENIORID As TextBox
    Friend WithEvents TextBoxSENIORNAME As TextBox
    Friend WithEvents ButtonCANCEL As Button
    Friend WithEvents ButtonSubmit As Button
    Friend WithEvents Label2 As Label
End Class
