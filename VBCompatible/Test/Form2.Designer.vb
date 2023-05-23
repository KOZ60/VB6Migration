<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form2

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.cmd01 = New VBCompatible.VBCommandButton()
        Me.cmd = New VBCompatible.VB6.VBCommandButtonArray(Me.components)
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.VbFrame1 = New VBCompatible.VBFrame()
        Me.VbCommandButton2 = New VBCompatible.VBCommandButton()
        Me.VbLabel1 = New VBCompatible.VBLabel()
        CType(Me.cmd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.VbFrame1.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmd01
        '
        Me.cmd.SetIndex(Me.cmd01, 0)
        Me.cmd01.Location = New System.Drawing.Point(17, 34)
        Me.cmd01.Name = "cmd01"
        Me.cmd01.Size = New System.Drawing.Size(162, 36)
        Me.cmd01.TabIndex = 0
        Me.cmd01.Text = "cmd00"
        Me.ToolTip1.SetToolTip(Me.cmd01, "ほげほげ")
        Me.cmd01.UseVisualStyleBackColor = True
        '
        'cmd
        '
        Me.cmd.Name = "cmd"
        '
        'VbFrame1
        '
        Me.VbFrame1.Controls.Add(Me.cmd01)
        Me.VbFrame1.Location = New System.Drawing.Point(12, 12)
        Me.VbFrame1.Name = "VbFrame1"
        Me.VbFrame1.Size = New System.Drawing.Size(205, 458)
        Me.VbFrame1.TabIndex = 2
        Me.VbFrame1.TabStop = False
        Me.VbFrame1.Text = "VbFrame1"
        '
        'VbCommandButton2
        '
        Me.VbCommandButton2.Location = New System.Drawing.Point(243, 46)
        Me.VbCommandButton2.Name = "VbCommandButton2"
        Me.VbCommandButton2.Size = New System.Drawing.Size(162, 36)
        Me.VbCommandButton2.TabIndex = 3
        Me.VbCommandButton2.Text = "cmdRemove"
        Me.VbCommandButton2.UseVisualStyleBackColor = True
        '
        'VbLabel1
        '
        Me.VbLabel1.Location = New System.Drawing.Point(246, 105)
        Me.VbLabel1.Name = "VbLabel1"
        Me.VbLabel1.Size = New System.Drawing.Size(158, 36)
        Me.VbLabel1.TabIndex = 4
        Me.VbLabel1.Text = "VbLabel1"
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(431, 479)
        Me.Controls.Add(Me.VbLabel1)
        Me.Controls.Add(Me.VbCommandButton2)
        Me.Controls.Add(Me.VbFrame1)
        Me.Name = "Form2"
        Me.Text = "TabIndex Demo"
        CType(Me.cmd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.VbFrame1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cmd01 As VBCompatible.VBCommandButton
    Friend WithEvents cmd As VBCompatible.VB6.VBCommandButtonArray
    Public WithEvents ToolTip1 As ToolTip
    Friend WithEvents VbFrame1 As VBCompatible.VBFrame
    Friend WithEvents VbCommandButton2 As VBCompatible.VBCommandButton
    Friend WithEvents VbLabel1 As VBCompatible.VBLabel
End Class
