Imports VBCompatible
Imports System.Numerics
Imports System.Globalization

Module Program

    <STAThread>
    Sub Main()
        Dim A = "𩸽AB"
        Dim B = "B𩸽B"
        Dim C = "BB𩸽"
        'Debug.Print(VB6.MidU(A, 1))
        'Debug.Print(VB6.MidU(A, 2))
        'Debug.Print(VB6.MidU(A, 3))

        'Debug.Print(VB6.MidU(A, 2, 1))
        'Debug.Print(VB6.MidU(A, 2, 2))
        'Debug.Print(VB6.MidU(A, 2, 3))

        Debug.Print(VB6.InStrU(A, "𩸽"))
        Debug.Print(VB6.InStrU(B, "𩸽"))
        Debug.Print(VB6.InStrU(C, "𩸽"))

        Debug.Print(VB6.InStrU(A, "𩸽b", CompareMethod.Text))
        Debug.Print(VB6.InStrU(B, "𩸽b", CompareMethod.Text))
        Debug.Print(VB6.InStrU(C, "𩸽b", CompareMethod.Text))


        If App.PrevInstance Then
            MsgBox("App.PrevInstance")
            Return
        End If
        VBApp.EnableVisualStyles()
        VBApp.SetCompatibleTextRenderingDefault(False)
        VBApp.Run(New Form2())
    End Sub

End Module

