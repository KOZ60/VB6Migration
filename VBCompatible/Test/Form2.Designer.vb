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
        Me.cmd02 = New VBCompatible.VBCommandButton()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.VbCommandButton1 = New VBCompatible.VBCommandButton()
        Me.VbFrame1 = New VBCompatible.VBFrame()
        Me.VbCommandButton2 = New VBCompatible.VBCommandButton()
        Me.VbCheckBoxArray1 = New VBCompatible.ControlArray.VBCheckBoxArray(Me.components)
        CType(Me.cmd, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.VbFrame1.SuspendLayout()
        CType(Me.VbCheckBoxArray1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmd01
        '
        Me.cmd01.Location = New System.Drawing.Point(17, 34)
        Me.cmd01.Name = "cmd01"
        Me.cmd01.Size = New System.Drawing.Size(162, 36)
        Me.cmd01.TabIndex = 0
        Me.cmd01.Text = "VbCommandButton1"
        Me.ToolTip1.SetToolTip(Me.cmd01, "ほげほげ")
        Me.cmd01.UseVisualStyleBackColor = True
        '
        'cmd
        '
        Me.cmd.Name = "cmd"
        '
        'cmd02
        '
        Me.cmd02.Location = New System.Drawing.Point(244, 195)
        Me.cmd02.Name = "cmd02"
        Me.cmd02.Size = New System.Drawing.Size(162, 36)
        Me.cmd02.TabIndex = 1
        Me.cmd02.Text = "VbCommandButton1"
        Me.ToolTip1.SetToolTip(Me.cmd02, "ほげほげ")
        Me.cmd02.UseVisualStyleBackColor = True
        '
        'VbCommandButton1
        '
        Me.VbCommandButton1.Location = New System.Drawing.Point(244, 46)
        Me.VbCommandButton1.Name = "VbCommandButton1"
        Me.VbCommandButton1.Size = New System.Drawing.Size(162, 36)
        Me.VbCommandButton1.TabIndex = 1
        Me.VbCommandButton1.Text = "ShowEventEnum"
        Me.VbCommandButton1.UseVisualStyleBackColor = True
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
        Me.VbCommandButton2.Location = New System.Drawing.Point(244, 120)
        Me.VbCommandButton2.Name = "VbCommandButton2"
        Me.VbCommandButton2.Size = New System.Drawing.Size(162, 36)
        Me.VbCommandButton2.TabIndex = 3
        Me.VbCommandButton2.Text = "cmdRemove"
        Me.VbCommandButton2.UseVisualStyleBackColor = True
        '
        'VbCheckBoxArray1
        '
        Me.VbCheckBoxArray1.Name = "VbCheckBoxArray1"
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(431, 479)
        Me.Controls.Add(Me.cmd02)
        Me.Controls.Add(Me.VbCommandButton2)
        Me.Controls.Add(Me.VbFrame1)
        Me.Controls.Add(Me.VbCommandButton1)
        Me.Name = "Form2"
        Me.Text = "TabIndex Demo"
        CType(Me.cmd, System.ComponentModel.ISupportInitialize).EndInit()
        Me.VbFrame1.ResumeLayout(False)
        CType(Me.VbCheckBoxArray1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cmd01 As VBCompatible.VBCommandButton
    Friend WithEvents cmd As VBCompatible.ControlArray.VBCommandButtonArray
    Public WithEvents ToolTip1 As ToolTip
    Friend WithEvents VbCommandButton1 As VBCompatible.VBCommandButton
    Friend WithEvents VbFrame1 As VBCompatible.VBFrame
    Friend WithEvents VbCommandButton2 As VBCompatible.VBCommandButton
    Friend WithEvents cmd02 As VBCompatible.VBCommandButton
    Friend WithEvents VbCheckBoxArray1 As VBCompatible.ControlArray.VBCheckBoxArray
End Class
