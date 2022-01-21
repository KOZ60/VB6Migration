Option Strict On

Imports Managed.OracleInProcServer
Imports Managed.OracleInProcServer.dynOption
Imports System.Text

Module Module1

    <STAThread>
    Sub Main()

        Console.ForegroundColor = ConsoleColor.Gray
        Console.BackgroundColor = ConsoleColor.Black
        Console.Write("Database に接続中です")

        Dim DB As New OraDatabase()
        DB.ConnectionString = "Data Source=ORCL;User Id=KOZ;Password=KOZ"
        DB.Open()

        StopConsole("何かキーを押すと開始します。")

        WriteLineColor("* * * S T A R T * * *", ConsoleColor.Green)

        Dim list As List(Of TestBase) = CreateTest()

        For Each t As TestBase In list
            t.DoTest(DB)
        Next

        WriteLineColor("* * * E N D * * *", ConsoleColor.Green)
        Console.WriteLine("何かキーを押すと終了します。")
        Console.ReadKey()

    End Sub

    Public Function BytesCompare(ByVal a As Byte(), ByVal b As Byte()) As Boolean

        If a.Length <> b.Length Then
            Return False
        End If

        For i As Integer = 0 To a.Length - 1
            If a(i) <> b(i) Then
                Return False
            End If
        Next

        Return True

    End Function

    Public Sub StopConsole(ByVal message As String)
        Console.Write(vbCr)
        Console.WriteLine(message)
        Console.ReadKey()
    End Sub

    Public Function Check(ByVal result As Boolean, ByVal message As String) As Boolean
        If result Then
            WriteColor("OK:", ConsoleColor.Cyan)
            Console.WriteLine(message)
        Else
            Console.Beep()
            WriteColor("NG:", ConsoleColor.Red)
            WriteLineColor(message, ConsoleColor.White)
            StopConsole("何かキーを押すと続行します。")
        End If
        Return result
    End Function

    Public Function EditItem(value As String) As String
        Const Q As String = "'"
        Const QQ As String = "''"
        Return Q & value.Replace(Q, QQ) & Q
    End Function

    Public Function CreateTest() As List(Of TestBase)

        Dim list As New List(Of TestBase)
        list.Add(New TestBFile())
        list.Add(New TestDecimal())
        list.Add(New TestDataClear())
        list.Add(New TestFillZero(False))
        list.Add(New TestDataClear())
        list.Add(New UnicodeTest1())
        list.Add(New UnicodeTest2())
        list.Add(New TestDuplicateField())
        list.Add(New TestParamArray())
        list.Add(New TestOutputParameter())
        list.Add(New TestSQLInsert())
        list.Add(New TestParameterInsert())
        list.Add(New TestParamArrayInsert())
        list.Add(New TestParameterLONG())
        list.Add(New TestParameterBLOB())
        list.Add(New TestNoChacheDynaset())
        list.Add(New TestParameterNULL())
        list.Add(New TestDuplicateField2())
        list.Add(New TestSpaceParameter())
        Return list
    End Function

    Public Sub WriteColor(ByVal value As String, ByVal foreColor As ConsoleColor)
        Console.ForegroundColor = foreColor
        Console.Write(value)
        Console.ForegroundColor = ConsoleColor.Gray
    End Sub

    Public Sub WriteLineColor(ByVal value As String, ByVal foreColor As ConsoleColor)
        Console.ForegroundColor = foreColor
        Console.WriteLine(value)
        Console.ForegroundColor = ConsoleColor.Gray
    End Sub

End Module
