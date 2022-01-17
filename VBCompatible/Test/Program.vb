Imports VBCompatible

Module Program

    <STAThread>
    Sub Main()
        VBApp.EnableVisualStyles()
        VBApp.SetCompatibleTextRenderingDefault(False)
        VBApp.Run(New Form1())
    End Sub

End Module
