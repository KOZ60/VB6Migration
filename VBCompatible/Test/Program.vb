Imports VBCompatible

Module Program

    <STAThread>
    Sub Main()
        App.Title = "VB6 移行サンプル"
        If App.PrevInstance Then
            MsgBox("App.PrevInstance")
            Return
        End If
        VBApp.EnableVisualStyles()
        VBApp.SetCompatibleTextRenderingDefault(False)
        VBApp.Run(New Form1())
    End Sub

End Module

