<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewStockEntry
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NewStockEntry))
        Me.Label23 = New System.Windows.Forms.Label()
        Me.ButtonENTRYADDSTOCK = New System.Windows.Forms.Button()
        Me.ComboBoxDESC = New System.Windows.Forms.ComboBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBoxEPrimary = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.TextBoxESecondary = New System.Windows.Forms.TextBox()
        Me.TextBoxEQuantity = New System.Windows.Forms.TextBox()
        Me.TextBoxEFPUnit = New System.Windows.Forms.TextBox()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.TextBoxEFPrimaryVal = New System.Windows.Forms.TextBox()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.TextBoxEFSUnit = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.TextBoxEFSecondVal = New System.Windows.Forms.TextBox()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.TextBoxEServingValue = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.TextBoxEServingVal = New System.Windows.Forms.TextBox()
        Me.TextBoxENoServings = New System.Windows.Forms.TextBox()
        Me.ButtonKeyboard = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.Label23.Location = New System.Drawing.Point(9, 9)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(76, 16)
        Me.Label23.TabIndex = 19
        Me.Label23.Text = "Description:"
        '
        'ButtonENTRYADDSTOCK
        '
        Me.ButtonENTRYADDSTOCK.BackColor = System.Drawing.Color.FromArgb(CType(CType(221, Byte), Integer), CType(CType(114, Byte), Integer), CType(CType(46, Byte), Integer))
        Me.ButtonENTRYADDSTOCK.FlatAppearance.BorderSize = 0
        Me.ButtonENTRYADDSTOCK.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonENTRYADDSTOCK.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.ButtonENTRYADDSTOCK.ForeColor = System.Drawing.Color.White
        Me.ButtonENTRYADDSTOCK.Location = New System.Drawing.Point(12, 235)
        Me.ButtonENTRYADDSTOCK.Name = "ButtonENTRYADDSTOCK"
        Me.ButtonENTRYADDSTOCK.Size = New System.Drawing.Size(258, 23)
        Me.ButtonENTRYADDSTOCK.TabIndex = 27
        Me.ButtonENTRYADDSTOCK.Text = "Submit"
        Me.ButtonENTRYADDSTOCK.UseVisualStyleBackColor = False
        '
        'ComboBoxDESC
        '
        Me.ComboBoxDESC.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ComboBoxDESC.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBoxDESC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxDESC.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboBoxDESC.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.ComboBoxDESC.FormattingEnabled = True
        Me.ComboBoxDESC.Location = New System.Drawing.Point(12, 27)
        Me.ComboBoxDESC.Name = "ComboBoxDESC"
        Me.ComboBoxDESC.Size = New System.Drawing.Size(326, 24)
        Me.ComboBoxDESC.TabIndex = 32
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.749999!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(26, 236)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(192, 22)
        Me.TextBox1.TabIndex = 53
        '
        'TextBoxEPrimary
        '
        Me.TextBoxEPrimary.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.TextBoxEPrimary.Location = New System.Drawing.Point(12, 72)
        Me.TextBoxEPrimary.Name = "TextBoxEPrimary"
        Me.TextBoxEPrimary.ReadOnly = True
        Me.TextBoxEPrimary.Size = New System.Drawing.Size(160, 23)
        Me.TextBoxEPrimary.TabIndex = 37
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.Label20.Location = New System.Drawing.Point(174, 187)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(109, 16)
        Me.Label20.TabIndex = 52
        Me.Label20.Text = "Quantity(Primary)"
        '
        'TextBoxESecondary
        '
        Me.TextBoxESecondary.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.TextBoxESecondary.Location = New System.Drawing.Point(12, 116)
        Me.TextBoxESecondary.Name = "TextBoxESecondary"
        Me.TextBoxESecondary.ReadOnly = True
        Me.TextBoxESecondary.Size = New System.Drawing.Size(160, 23)
        Me.TextBoxESecondary.TabIndex = 38
        '
        'TextBoxEQuantity
        '
        Me.TextBoxEQuantity.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.TextBoxEQuantity.Location = New System.Drawing.Point(178, 206)
        Me.TextBoxEQuantity.Name = "TextBoxEQuantity"
        Me.TextBoxEQuantity.Size = New System.Drawing.Size(159, 23)
        Me.TextBoxEQuantity.TabIndex = 51
        '
        'TextBoxEFPUnit
        '
        Me.TextBoxEFPUnit.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.TextBoxEFPUnit.Location = New System.Drawing.Point(261, 72)
        Me.TextBoxEFPUnit.Name = "TextBoxEFPUnit"
        Me.TextBoxEFPUnit.ReadOnly = True
        Me.TextBoxEFPUnit.Size = New System.Drawing.Size(77, 23)
        Me.TextBoxEFPUnit.TabIndex = 39
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.Label21.Location = New System.Drawing.Point(9, 187)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(105, 16)
        Me.Label21.TabIndex = 50
        Me.Label21.Text = "No. of Serving(s)"
        '
        'TextBoxEFPrimaryVal
        '
        Me.TextBoxEFPrimaryVal.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.TextBoxEFPrimaryVal.Location = New System.Drawing.Point(177, 72)
        Me.TextBoxEFPrimaryVal.Name = "TextBoxEFPrimaryVal"
        Me.TextBoxEFPrimaryVal.ReadOnly = True
        Me.TextBoxEFPrimaryVal.Size = New System.Drawing.Size(77, 23)
        Me.TextBoxEFPrimaryVal.TabIndex = 40
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.Label22.Location = New System.Drawing.Point(9, 142)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(87, 16)
        Me.Label22.TabIndex = 49
        Me.Label22.Text = "Serving Value"
        '
        'TextBoxEFSUnit
        '
        Me.TextBoxEFSUnit.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.TextBoxEFSUnit.Location = New System.Drawing.Point(261, 116)
        Me.TextBoxEFSUnit.Name = "TextBoxEFSUnit"
        Me.TextBoxEFSUnit.ReadOnly = True
        Me.TextBoxEFSUnit.Size = New System.Drawing.Size(77, 23)
        Me.TextBoxEFSUnit.TabIndex = 41
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.Label24.Location = New System.Drawing.Point(174, 145)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(77, 16)
        Me.Label24.TabIndex = 48
        Me.Label24.Text = "Serving Unit"
        '
        'TextBoxEFSecondVal
        '
        Me.TextBoxEFSecondVal.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.TextBoxEFSecondVal.Location = New System.Drawing.Point(177, 116)
        Me.TextBoxEFSecondVal.Name = "TextBoxEFSecondVal"
        Me.TextBoxEFSecondVal.ReadOnly = True
        Me.TextBoxEFSecondVal.Size = New System.Drawing.Size(77, 23)
        Me.TextBoxEFSecondVal.TabIndex = 42
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.Label25.Location = New System.Drawing.Point(9, 98)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(101, 16)
        Me.Label25.TabIndex = 47
        Me.Label25.Text = "Secondary Total"
        '
        'TextBoxEServingValue
        '
        Me.TextBoxEServingValue.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.TextBoxEServingValue.Location = New System.Drawing.Point(177, 161)
        Me.TextBoxEServingValue.Name = "TextBoxEServingValue"
        Me.TextBoxEServingValue.ReadOnly = True
        Me.TextBoxEServingValue.Size = New System.Drawing.Size(160, 23)
        Me.TextBoxEServingValue.TabIndex = 43
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.Label26.Location = New System.Drawing.Point(9, 54)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(85, 16)
        Me.Label26.TabIndex = 46
        Me.Label26.Text = "Primary Total"
        '
        'TextBoxEServingVal
        '
        Me.TextBoxEServingVal.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.TextBoxEServingVal.Location = New System.Drawing.Point(12, 161)
        Me.TextBoxEServingVal.Name = "TextBoxEServingVal"
        Me.TextBoxEServingVal.ReadOnly = True
        Me.TextBoxEServingVal.Size = New System.Drawing.Size(160, 23)
        Me.TextBoxEServingVal.TabIndex = 44
        '
        'TextBoxENoServings
        '
        Me.TextBoxENoServings.Font = New System.Drawing.Font("Tahoma", 9.75!)
        Me.TextBoxENoServings.Location = New System.Drawing.Point(12, 206)
        Me.TextBoxENoServings.Name = "TextBoxENoServings"
        Me.TextBoxENoServings.ReadOnly = True
        Me.TextBoxENoServings.Size = New System.Drawing.Size(160, 23)
        Me.TextBoxENoServings.TabIndex = 45
        '
        'ButtonKeyboard
        '
        Me.ButtonKeyboard.BackColor = System.Drawing.SystemColors.Control
        Me.ButtonKeyboard.BackgroundImage = CType(resources.GetObject("ButtonKeyboard.BackgroundImage"), System.Drawing.Image)
        Me.ButtonKeyboard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ButtonKeyboard.FlatAppearance.BorderSize = 0
        Me.ButtonKeyboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonKeyboard.Location = New System.Drawing.Point(276, 235)
        Me.ButtonKeyboard.Name = "ButtonKeyboard"
        Me.ButtonKeyboard.Size = New System.Drawing.Size(61, 23)
        Me.ButtonKeyboard.TabIndex = 232
        Me.ButtonKeyboard.UseVisualStyleBackColor = False
        '
        'NewStockEntry
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(351, 270)
        Me.Controls.Add(Me.ButtonKeyboard)
        Me.Controls.Add(Me.Label23)
        Me.Controls.Add(Me.ButtonENTRYADDSTOCK)
        Me.Controls.Add(Me.TextBoxENoServings)
        Me.Controls.Add(Me.ComboBoxDESC)
        Me.Controls.Add(Me.TextBoxEServingVal)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label26)
        Me.Controls.Add(Me.TextBoxEPrimary)
        Me.Controls.Add(Me.TextBoxEServingValue)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label25)
        Me.Controls.Add(Me.TextBoxESecondary)
        Me.Controls.Add(Me.TextBoxEFSecondVal)
        Me.Controls.Add(Me.TextBoxEQuantity)
        Me.Controls.Add(Me.Label24)
        Me.Controls.Add(Me.TextBoxEFPUnit)
        Me.Controls.Add(Me.TextBoxEFSUnit)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.TextBoxEFPrimaryVal)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewStockEntry"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Stock Entry"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label23 As Label
    Friend WithEvents ButtonENTRYADDSTOCK As Button
    Friend WithEvents ComboBoxDESC As ComboBox
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents TextBoxEPrimary As TextBox
    Friend WithEvents Label20 As Label
    Friend WithEvents TextBoxESecondary As TextBox
    Friend WithEvents TextBoxEQuantity As TextBox
    Friend WithEvents TextBoxEFPUnit As TextBox
    Friend WithEvents Label21 As Label
    Friend WithEvents TextBoxEFPrimaryVal As TextBox
    Friend WithEvents Label22 As Label
    Friend WithEvents TextBoxEFSUnit As TextBox
    Friend WithEvents Label24 As Label
    Friend WithEvents TextBoxEFSecondVal As TextBox
    Friend WithEvents Label25 As Label
    Friend WithEvents TextBoxEServingValue As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents TextBoxEServingVal As TextBox
    Friend WithEvents TextBoxENoServings As TextBox
    Friend WithEvents ButtonKeyboard As Button
End Class
