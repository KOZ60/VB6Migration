Imports VBCompatible
Imports System.Numerics

Module Program

    <STAThread>
    Sub Main()
        If App.PrevInstance Then
            MsgBox("App.PrevInstance")
            Return
        End If
        VBApp.EnableVisualStyles()
        VBApp.SetCompatibleTextRenderingDefault(False)
        VBApp.Run(New Form1())
    End Sub

End Module

Public Structure VBDouble

    Private Value As String



End Structure