Imports System.Text
Imports ADODB.DataTypeEnum
Imports ADODB.FieldAttributeEnum

Module Module1

    Sub Main()
        TestSort()

        Dim con As New ADODB.Connection()
        con.Provider = "Oracle.DataAccess.Client"
        con.ConnectionString = "Data Source=NCNUV3;User Id=KOZ;Password=KOZ"
        con.Open()

        TBL_MASTER.Create(con, 1000)

    End Sub

    Sub TestSort()

        Dim objRec As ADODB.Recordset
        Dim lngValue As Integer
        Dim i As Long

        '   Recordset オブジェクトを作成
        objRec = New ADODB.Recordset

        '   フィールドを追加
        With objRec.Fields
            Call .Append("myfield0", adInteger, , adFldIsNullable)
            Call .Append("myfield1", adVarChar, 10, adFldIsNullable)
        End With

        With objRec
            Call .Open()
            For i = 1 To 100
                lngValue = Rnd(1) * 32767
                .AddNew()
                .Fields(0).Value = lngValue
                .Fields(1).Value = CStr(lngValue)
                .Update()
            Next

            .Sort = "myfield0"

            .MoveFirst()
            Do Until .EOF
                Debug.Print(.Fields(1))
                .MoveNext()
            Loop
            .Close()
        End With

        objRec = Nothing
    End Sub

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
