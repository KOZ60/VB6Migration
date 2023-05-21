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
        Me.components = New System.ComponentModel.Container()
        Me.cmd01 = New VBCompatible.VBCommandButton()
        Me.cmd = New VBCompatible.ControlArray.VBCommandButtonArray(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.VbCommandButton1 = New VBCompatible.VBCommandButton()
        CType(Me.cmd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmd01
        '
        Me.cmd.SetIndex(Me.cmd01, 0)
        Me.cmd01.Location = New System.Drawing.Point(12, 12)
        Me.cmd01.Name = "cmd01"
        Me.cmd01.Size = New System.Drawing.Size(162, 36)
        Me.cmd01.TabIndex = 0
        Me.cmd01.Text = "VbCommandButton1"
        Me.ToolTip1.SetToolTip(Me.cmd01, "ほげほげ")
        Me.cmd01.UseVisualStyleBackColor = True
        '
        'cmd
        '
        '
        'VbCommandButton1
        '
        Me.VbCommandButton1.Location = New System.Drawing.Point(229, 54)
        Me.VbCommandButton1.Name = "VbCommandButton1"
        Me.VbCommandButton1.Size = New System.Drawing.Size(125, 29)
        Me.VbCommandButton1.TabIndex = 1
        Me.VbCommandButton1.Text = "VbCommandButton1"
        Me.VbCommandButton1.UseVisualStyleBackColor = True
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(403, 178)
        Me.Controls.Add(Me.VbCommandButton1)
        Me.Controls.Add(Me.cmd01)
        Me.Name = "Form2"
        Me.Text = "TabIndex Demo"
        CType(Me.cmd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cmd01 As VBCompatible.VBCommandButton
    Friend WithEvents cmd As VBCompatible.ControlArray.VBCommandButtonArray
    Public WithEvents ToolTip1 As ToolTip
    Friend WithEvents VbCommandButton1 As VBCompatible.VBCommandButton
End Class
