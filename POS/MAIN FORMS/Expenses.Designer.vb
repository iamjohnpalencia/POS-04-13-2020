<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Expenses
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Expenses))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.TextBoxTOTAL = New System.Windows.Forms.TextBox()
        Me.TextBoxPRICE = New System.Windows.Forms.TextBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.TextBoxITEMINF = New System.Windows.Forms.TextBox()
        Me.ComboBoxType = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBoxQTY = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.PictureBoxAttachment = New System.Windows.Forms.PictureBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBoxAttatchment = New System.Windows.Forms.TextBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.Panel1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.PictureBoxAttachment, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TableLayoutPanel1)
        Me.Panel1.Controls.Add(Me.StatusStrip1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(648, 249)
        Me.Panel1.TabIndex = 0
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.GroupBox2, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(646, 225)
        Me.TableLayoutPanel1.TabIndex = 212
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.White
        Me.GroupBox1.Controls.Add(Me.TextBoxTOTAL)
        Me.GroupBox1.Controls.Add(Me.TextBoxPRICE)
        Me.GroupBox1.Controls.Add(Me.Button5)
        Me.GroupBox1.Controls.Add(Me.Button4)
        Me.GroupBox1.Controls.Add(Me.TextBoxITEMINF)
        Me.GroupBox1.Controls.Add(Me.ComboBoxType)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.TextBoxQTY)
        Me.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.GroupBox1.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(390, 219)
        Me.GroupBox1.TabIndex = 25
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Expense"
        '
        'TextBoxTOTAL
        '
        Me.TextBoxTOTAL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTOTAL.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.TextBoxTOTAL.ForeColor = System.Drawing.SystemColors.WindowFrame
        Me.TextBoxTOTAL.Location = New System.Drawing.Point(106, 154)
        Me.TextBoxTOTAL.Name = "TextBoxTOTAL"
        Me.TextBoxTOTAL.Size = New System.Drawing.Size(146, 23)
        Me.TextBoxTOTAL.TabIndex = 24
        '
        'TextBoxPRICE
        '
        Me.TextBoxPRICE.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxPRICE.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.TextBoxPRICE.ForeColor = System.Drawing.SystemColors.WindowFrame
        Me.TextBoxPRICE.Location = New System.Drawing.Point(106, 125)
        Me.TextBoxPRICE.Name = "TextBoxPRICE"
        Me.TextBoxPRICE.Size = New System.Drawing.Size(146, 23)
        Me.TextBoxPRICE.TabIndex = 23
        '
        'Button5
        '
        Me.Button5.BackColor = System.Drawing.Color.FromArgb(CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer), CType(CType(77, Byte), Integer))
        Me.Button5.FlatAppearance.BorderSize = 0
        Me.Button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button5.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.Button5.ForeColor = System.Drawing.Color.White
        Me.Button5.Location = New System.Drawing.Point(295, 183)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(89, 33)
        Me.Button5.TabIndex = 31
        Me.Button5.Text = "BACK"
        Me.Button5.UseVisualStyleBackColor = False
        '
        'Button4
        '
        Me.Button4.BackColor = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(46, Byte), Integer))
        Me.Button4.FlatAppearance.BorderSize = 0
        Me.Button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button4.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.Button4.ForeColor = System.Drawing.Color.White
        Me.Button4.Location = New System.Drawing.Point(106, 183)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(183, 33)
        Me.Button4.TabIndex = 25
        Me.Button4.Text = "SAVE"
        Me.Button4.UseVisualStyleBackColor = False
        '
        'TextBoxITEMINF
        '
        Me.TextBoxITEMINF.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxITEMINF.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.TextBoxITEMINF.ForeColor = System.Drawing.SystemColors.WindowFrame
        Me.TextBoxITEMINF.Location = New System.Drawing.Point(106, 67)
        Me.TextBoxITEMINF.Name = "TextBoxITEMINF"
        Me.TextBoxITEMINF.Size = New System.Drawing.Size(278, 23)
        Me.TextBoxITEMINF.TabIndex = 21
        '
        'ComboBoxType
        '
        Me.ComboBoxType.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.ComboBoxType.ForeColor = System.Drawing.SystemColors.WindowFrame
        Me.ComboBoxType.FormattingEnabled = True
        Me.ComboBoxType.Location = New System.Drawing.Point(106, 38)
        Me.ComboBoxType.Name = "ComboBoxType"
        Me.ComboBoxType.Size = New System.Drawing.Size(278, 23)
        Me.ComboBoxType.TabIndex = 32
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.Label9.Location = New System.Drawing.Point(11, 156)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(83, 15)
        Me.Label9.TabIndex = 30
        Me.Label9.Text = "Total Amount:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.Label6.Location = New System.Drawing.Point(11, 41)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(86, 15)
        Me.Label6.TabIndex = 29
        Me.Label6.Text = "Expense Type:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.Label4.Location = New System.Drawing.Point(11, 127)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 15)
        Me.Label4.TabIndex = 28
        Me.Label4.Text = "Price:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.Label3.Location = New System.Drawing.Point(11, 98)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 15)
        Me.Label3.TabIndex = 27
        Me.Label3.Text = "Quantity:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.Label1.Location = New System.Drawing.Point(11, 69)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(36, 15)
        Me.Label1.TabIndex = 26
        Me.Label1.Text = "Item:"
        '
        'TextBoxQTY
        '
        Me.TextBoxQTY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxQTY.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.TextBoxQTY.ForeColor = System.Drawing.SystemColors.WindowFrame
        Me.TextBoxQTY.Location = New System.Drawing.Point(106, 96)
        Me.TextBoxQTY.Name = "TextBoxQTY"
        Me.TextBoxQTY.Size = New System.Drawing.Size(146, 23)
        Me.TextBoxQTY.TabIndex = 22
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.White
        Me.GroupBox2.Controls.Add(Me.PictureBoxAttachment)
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.Controls.Add(Me.TextBoxAttatchment)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.GroupBox2.Location = New System.Drawing.Point(399, 3)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(244, 219)
        Me.GroupBox2.TabIndex = 26
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Image"
        '
        'PictureBoxAttachment
        '
        Me.PictureBoxAttachment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.PictureBoxAttachment.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PictureBoxAttachment.Image = CType(resources.GetObject("PictureBoxAttachment.Image"), System.Drawing.Image)
        Me.PictureBoxAttachment.Location = New System.Drawing.Point(3, 19)
        Me.PictureBoxAttachment.Name = "PictureBoxAttachment"
        Me.PictureBoxAttachment.Size = New System.Drawing.Size(238, 164)
        Me.PictureBoxAttachment.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBoxAttachment.TabIndex = 0
        Me.PictureBoxAttachment.TabStop = False
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(46, Byte), Integer))
        Me.Button1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Button1.FlatAppearance.BorderSize = 0
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(3, 183)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(238, 33)
        Me.Button1.TabIndex = 26
        Me.Button1.Text = "SELECT ATTACHMENT"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'TextBoxAttatchment
        '
        Me.TextBoxAttatchment.Enabled = False
        Me.TextBoxAttatchment.Location = New System.Drawing.Point(54, 82)
        Me.TextBoxAttatchment.Multiline = True
        Me.TextBoxAttatchment.Name = "TextBoxAttatchment"
        Me.TextBoxAttatchment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxAttatchment.Size = New System.Drawing.Size(100, 75)
        Me.TextBoxAttatchment.TabIndex = 1
        Me.TextBoxAttatchment.Text = resources.GetString("TextBoxAttatchment.Text")
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel1, Me.ToolStripProgressBar1})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 225)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(646, 22)
        Me.StatusStrip1.TabIndex = 211
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Font = New System.Drawing.Font("Kelson Sans Normal", 9.749999!)
        Me.ToolStripStatusLabel1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer), CType(CType(54, Byte), Integer))
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(42, 17)
        Me.ToolStripStatusLabel1.Text = "Status"
        '
        'ToolStripProgressBar1
        '
        Me.ToolStripProgressBar1.Name = "ToolStripProgressBar1"
        Me.ToolStripProgressBar1.Size = New System.Drawing.Size(540, 16)
        '
        'BackgroundWorker1
        '
        '
        'Timer1
        '
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'Expenses
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(648, 249)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Expenses"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.PictureBoxAttachment, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As Panel
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents PictureBoxAttachment As PictureBox
    Friend WithEvents Button1 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents ComboBoxType As ComboBox
    Friend WithEvents Button5 As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Button4 As Button
    Friend WithEvents TextBoxTOTAL As TextBox
    Friend WithEvents TextBoxPRICE As TextBox
    Friend WithEvents TextBoxQTY As TextBox
    Friend WithEvents TextBoxITEMINF As TextBox
    Friend WithEvents TextBoxAttatchment As TextBox
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Timer1 As Timer
    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents ToolStripProgressBar1 As ToolStripProgressBar
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
End Class
