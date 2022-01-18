<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TabIndexTextBox1 = New Test.TabIndexTextBox()
        Me.TabIndexTextBox2 = New Test.TabIndexTextBox()
        Me.TabIndexTextBox3 = New Test.TabIndexTextBox()
        Me.TabIndexTextBox4 = New Test.TabIndexTextBox()
        Me.TabIndexTextBox5 = New Test.TabIndexTextBox()
        Me.TabIndexTextBox6 = New Test.TabIndexTextBox()
        Me.VbPanel1 = New VBCompatible.VBPanel()
        Me.VbPanel2 = New VBCompatible.VBPanel()
        Me.VbPanel1.SuspendLayout()
        Me.VbPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabIndexTextBox1
        '
        Me.TabIndexTextBox1.Location = New System.Drawing.Point(16, 12)
        Me.TabIndexTextBox1.Name = "TabIndexTextBox1"
        Me.TabIndexTextBox1.Size = New System.Drawing.Size(141, 19)
        Me.TabIndexTextBox1.TabIndex = 0
        Me.TabIndexTextBox1.Text = "0"
        '
        'TabIndexTextBox2
        '
        Me.TabIndexTextBox2.Location = New System.Drawing.Point(15, 12)
        Me.TabIndexTextBox2.Name = "TabIndexTextBox2"
        Me.TabIndexTextBox2.Size = New System.Drawing.Size(141, 19)
        Me.TabIndexTextBox2.TabIndex = 1
        Me.TabIndexTextBox2.Text = "1"
        '
        'TabIndexTextBox3
        '
        Me.TabIndexTextBox3.Location = New System.Drawing.Point(16, 51)
        Me.TabIndexTextBox3.Name = "TabIndexTextBox3"
        Me.TabIndexTextBox3.Size = New System.Drawing.Size(141, 19)
        Me.TabIndexTextBox3.TabIndex = 2
        Me.TabIndexTextBox3.Text = "2"
        '
        'TabIndexTextBox4
        '
        Me.TabIndexTextBox4.Location = New System.Drawing.Point(15, 51)
        Me.TabIndexTextBox4.Name = "TabIndexTextBox4"
        Me.TabIndexTextBox4.Size = New System.Drawing.Size(141, 19)
        Me.TabIndexTextBox4.TabIndex = 3
        Me.TabIndexTextBox4.Text = "3"
        '
        'TabIndexTextBox5
        '
        Me.TabIndexTextBox5.Location = New System.Drawing.Point(16, 94)
        Me.TabIndexTextBox5.Name = "TabIndexTextBox5"
        Me.TabIndexTextBox5.Size = New System.Drawing.Size(141, 19)
        Me.TabIndexTextBox5.TabIndex = 4
        Me.TabIndexTextBox5.Text = "4"
        '
        'TabIndexTextBox6
        '
        Me.TabIndexTextBox6.Location = New System.Drawing.Point(15, 94)
        Me.TabIndexTextBox6.Name = "TabIndexTextBox6"
        Me.TabIndexTextBox6.Size = New System.Drawing.Size(141, 19)
        Me.TabIndexTextBox6.TabIndex = 5
        Me.TabIndexTextBox6.Text = "5"
        '
        'VbPanel1
        '
        Me.VbPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.VbPanel1.Controls.Add(Me.TabIndexTextBox5)
        Me.VbPanel1.Controls.Add(Me.TabIndexTextBox3)
        Me.VbPanel1.Controls.Add(Me.TabIndexTextBox1)
        Me.VbPanel1.Location = New System.Drawing.Point(29, 44)
        Me.VbPanel1.Name = "VbPanel1"
        Me.VbPanel1.Size = New System.Drawing.Size(178, 128)
        Me.VbPanel1.TabIndex = 6
        '
        'VbPanel2
        '
        Me.VbPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.VbPanel2.Controls.Add(Me.TabIndexTextBox6)
        Me.VbPanel2.Controls.Add(Me.TabIndexTextBox4)
        Me.VbPanel2.Controls.Add(Me.TabIndexTextBox2)
        Me.VbPanel2.Location = New System.Drawing.Point(226, 44)
        Me.VbPanel2.Name = "VbPanel2"
        Me.VbPanel2.Size = New System.Drawing.Size(182, 128)
        Me.VbPanel2.TabIndex = 7
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(440, 207)
        Me.Controls.Add(Me.VbPanel2)
        Me.Controls.Add(Me.VbPanel1)
        Me.Name = "Form2"
        Me.Text = "TabIndex Demo"
        Me.VbPanel1.ResumeLayout(False)
        Me.VbPanel1.PerformLayout()
        Me.VbPanel2.ResumeLayout(False)
        Me.VbPanel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabIndexTextBox1 As TabIndexTextBox
    Friend WithEvents TabIndexTextBox2 As TabIndexTextBox
    Friend WithEvents TabIndexTextBox3 As TabIndexTextBox
    Friend WithEvents TabIndexTextBox4 As TabIndexTextBox
    Friend WithEvents TabIndexTextBox5 As TabIndexTextBox
    Friend WithEvents TabIndexTextBox6 As TabIndexTextBox
    Friend WithEvents VbPanel1 As VBCompatible.VBPanel
    Friend WithEvents VbPanel2 As VBCompatible.VBPanel
End Class
