Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Reflection
Imports System.IO
Imports System.Text
Imports System.Threading
Imports OracleInProcServer
Imports OracleInProcServer.dynOption
Imports OracleInProcServer.paramMode
Imports OracleInProcServer.serverType

MustInherit Class TestBase

    Private _title As String = String.Empty

    Protected Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
            WriteLineColor("*** " & Me.GetType.Name & ":" & _title & " 開始 ***", ConsoleColor.Yellow)
        End Set
    End Property

    Public Sub DoTest(ByVal DB As OraDatabase)
        Dim dtStart As DateTime = Now
        Try
            DB.ResetTrans()
            DB.Parameters.Clear()
            OnTest(DB)
            If Not String.IsNullOrEmpty(_title) Then
                Console.Write("*** " & Me.GetType.Name & ":" & _title & " 終了 *** ")
                WriteLineColor(String.Format("{0} ms", (Now - dtStart).TotalMilliseconds), ConsoleColor.Blue)
            End If
        Catch ex As Exception
            Console.Beep()
            WriteLineColor(ex.Message, ConsoleColor.Red)
            Console.ReadKey()
        End Try

    End Sub

    Protected MustOverride Sub OnTest(ByVal DB As OraDatabase)

End Class

Class TestDecimal
    Inherits TestBase

    Protected Overrides Sub OnTest(DB As OraDatabase)
        Me.Title = "OracleDecimal のテスト"
        Using rec As OraDynaset = DB.CreateDynaset("SELECT 1/3 AS Value FROM DUAL", ORADYN_READONLY)
            Dim obj As Object = rec.Fields("Value")
            Dim s1 As Object = "" & rec.Fields("Value")
            Dim s2 As Object = 0 + CInt(rec.Fields("Value"))
            Dim dbl1 As Double = CDbl(rec.Fields("Value"))
            Dim dbl2 As Double = CDbl(rec.Fields("Value").Value)
            Dim dec As Decimal = CDec(rec.Fields("Value").Value)
        End Using
    End Sub
End Class

Class TestFillZero
    Inherits TestBase
    Dim m_FillZero As Boolean
    Dim m_FillZeroBak As Boolean

    Public Sub New(fillZero As Boolean)
        m_FillZero = fillZero
    End Sub

    Protected Overrides Sub OnTest(DB As OraDatabase)

        DB.ExecuteSQL("DELETE FROM TBL_MASTER")

        Dim nCount As Integer = 10
        With DB.Parameters
            .Clear()
            .AddTable("SEQ", ORAPARM_INPUT, ORATYPE_NUMBER, nCount)
            .AddTable("CODE", ORAPARM_INPUT, ORATYPE_VARCHAR2, nCount)
            .AddTable("NAME", ORAPARM_INPUT, ORATYPE_VARCHAR2, nCount)
            .AddTable("KINGAKU", ORAPARM_INPUT, ORATYPE_NUMBER, nCount)
            .AddTable("hoge", ORAPARM_INPUT, ORATYPE_NUMBER, nCount)
            .AddTable("HIDUKE", ORAPARM_INPUT, ORATYPE_DATE, nCount)

            For i As Integer = 0 To nCount - 1
                .Item("SEQ").put_Value(i + 1, i)
                .Item("CODE").put_Value(String.Format("CODE{0:0000}", i), i)
                .Item("NAME").put_Value(String.Format("NAME{0:0000}", i), i)
                .Item("KINGAKU").put_Value(DBNull.Value, i)
                .Item("HIDUKE").put_Value(Now.ToString(), i)
            Next
        End With

        Dim Ret As Integer = DB.ExecuteSQL("INSERT INTO TBL_MASTER(SEQ, CODE, NAME, KINGAKU, HIDUKE) VALUES (SEQ_SEQ.NEXTVAL, :CODE, :NAME, :KINGAKU, :HIDUKE)")

        Using dyn As OraDynaset = DB.CreateDynaset("SELECT * FROM TBL_MASTER", ORADYN_DEFAULT)
            If m_FillZero Then
                If dyn.Fields("KINGAKU").Value.IsNull Then
                    Check(False, "NULL にはならない")
                Else
                    Check(True, "NULL ではない")
                End If
            Else
                If dyn.Fields("KINGAKU").Value.IsNull Then
                    Check(True, "NULL になる")
                Else
                    Check(False, "NULL ではない")
                End If
            End If
        End Using
    End Sub
End Class

Class TBL_MASTER
    Inherits TestBase
    Shared m_Count As Integer

    Private Sub New()

    End Sub

    Public Shared Sub Create(ByVal DB As OraDatabase, ByVal Count As Integer)
        m_Count = Count
        Dim t As New TBL_MASTER
        t.DoTest(DB)
    End Sub

    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
        Title = String.Format("テストデータ作成({0}件)", m_Count)

        Dim nRet As Integer

        With DB
            .ResetTrans()
            .BeginTrans()

            With .Parameters
                .Clear()
                .AddTable("SEQ", ORAPARM_INPUT, ORATYPE_NUMBER, m_Count)
                .AddTable("CODE", ORAPARM_INPUT, ORATYPE_VARCHAR2, m_Count)
                .AddTable("NAME", ORAPARM_INPUT, ORATYPE_VARCHAR2, m_Count)
                .AddTable("KINGAKU", ORAPARM_INPUT, ORATYPE_NUMBER, m_Count)
                .AddTable("hoge", ORAPARM_INPUT, ORATYPE_NUMBER, m_Count)
                .AddTable("HIDUKE", ORAPARM_INPUT, ORATYPE_DATE, m_Count)

                For i As Integer = 0 To m_Count - 1
                    .Item("SEQ").put_Value(i + 1, i)
                    .Item("CODE").put_Value(String.Format("CODE{0:0000}", i), i)
                    .Item("NAME").put_Value(String.Format("NAME{0:0000}", i), i)
                    .Item("KINGAKU").put_Value(i * 1000, i)
                    .Item("HIDUKE").put_Value(Now.ToString(), i)
                Next
            End With

            nRet = DB.ExecuteSQL("DELETE FROM TBL_MASTER")
            nRet = .ExecuteSQL("INSERT INTO TBL_MASTER(SEQ, CODE, NAME, KINGAKU, HIDUKE) VALUES (:SEQ, :CODE, :NAME, :KINGAKU, :HIDUKE)")

            .Parameters.Clear()
            .CommitTrans()

        End With
    End Sub


End Class

Class TBL_ALLTYPES
    Inherits TestBase

    Protected Overrides Sub OnTest(DB As OraDatabase)

        DB.BeginTrans()
        Using dyn As OraDynaset = DB.CreateDynaset("SELECT ITEM_BFILE FROM TBL_ALLTYPES FOR UPDATE", dynOption.ORADYN_READONLY)
            If Not dyn.EOF Then
                Dim bfile As OraBFile = dyn(0).Value

                bfile.Open()
                bfile.Value = New Byte() {65, 66, 67}
                bfile.Close()
            End If
        End Using


        DB.CommitTrans()

        Using dyn As OraDynaset = DB.CreateDynaset("SELECT ITEM_BFILE FROM TBL_ALLTYPES", dynOption.ORADYN_READONLY)
            If Not dyn.EOF Then
                Dim bfile As OraBFile = dyn(0).Value

                bfile.Open()
                bfile.Value = New Byte() {65, 66, 67}
                bfile.Close()

            End If



            With DB.Parameters
                Const count = 10
                .Clear()

                Dim items As New List(Of String)
                Dim values As New List(Of String)

                For Each f In dyn.Fields
                    .AddTable(f.Name, paramMode.ORAPARM_INPUT, f.serverType, count)
                    If f.Name <> "SEQ" Then
                        items.Add(f.Name)
                        values.Add(":" & f.Name)
                    End If
                Next
                For i As Integer = 0 To count - 1
                    For Each p In DB.ParamArrays
                        p.put_Value(DBNull.Value, i)
                    Next
                Next

                Dim sql As New StringBuilder()
                sql.AppendLine("INSERT INTO TBL_ALLTYPES(")
                sql.AppendLine(String.Join(",", items))
                sql.AppendLine(",SEQ")
                sql.AppendLine(") VALUES (")
                sql.AppendLine(String.Join(",", values))
                sql.AppendLine(",SEQ_SEQ.NEXTVAL")
                sql.AppendLine(")")

                DB.ExecuteSQL(sql.ToString())

            End With
        End Using
    End Sub

End Class


'Class TestThreadInsert
'    Inherits TestBase

'    Protected Overrides Sub OnTest(DB As OraDatabase)
'        Dim t As New TestSQLInsert

'        Dim waitHandle As New ManualResetEvent(False)
'        Dim th1 As New Thread(New System.Threading.ParameterizedThreadStart(AddressOf SubThread))
'        th1.SetApartmentState(ApartmentState.STA)
'        th1.Start(waitHandle)

'        Dim th2 As New Thread(New System.Threading.ParameterizedThreadStart(AddressOf SubThread))
'        th2.SetApartmentState(ApartmentState.STA)
'        th2.Start(waitHandle)
'        waitHandle.Set()

'        th1.Join()
'        th2.Join()
'    End Sub

'    Sub SubThread(ByVal parameter As Object)
'        Dim Con As New OraSessionClass
'        Dim DB As OraDatabase = Con.OpenDatabase(HostName, ConnectionString, ORADB_DEFAULT)

'        Dim waitHandle As ManualResetEvent = DirectCast(parameter, ManualResetEvent)
'        Dim t As New TestSQLInsert
'        waitHandle.WaitOne()
'        t.DoTest(DB)
'    End Sub

'End Class

'Class StringOverFlow
'    Inherits TestBase

'    Protected Overrides Sub OnTest(DB As OraDatabase)
'        Dim nRet As Integer
'        With DB
'            .ResetTrans()
'            .BeginTrans()

'            With .Parameters
'                .Clear()
'                .AddTable("SEQ", ORAPARM_INPUT, ORATYPE_NUMBER, 1)
'                .AddTable("CODE", ORAPARM_INPUT, ORATYPE_VARCHAR2, 1, 10)
'                .AddTable("NAME", ORAPARM_INPUT, ORATYPE_VARCHAR2, 1, 10)
'                .AddTable("FLG1", ORAPARM_INPUT, ORATYPE_VARCHAR2, 1, 1)
'                .Item("SEQ").put_Value(1, 0)
'                .Item("CODE").put_Value(String.Format("CODE{0:0000}", 1), 0)
'                .Item("NAME").put_Value(String.Format("NAME{0:0000}", 1), 0)

'                Try
'                    .Item("FLG1").put_Value("-1", 0)
'                    Check(False, "エラーが発生するはず")

'                Catch ex As Exception
'                    Check(True, ex.Message)
'                End Try
'                .Item("FLG1").put_Value("1", 0)
'            End With

'            nRet = DB.ExecuteSQL("DELETE FROM TBL_MASTER")
'            nRet = .ExecuteSQL("INSERT INTO TBL_MASTER(SEQ, CODE, NAME, FLG1) VALUES (:SEQ, :CODE, :NAME, :FLG1)")

'            .CommitTrans()
'            .Parameters.Clear()

'        End With
'    End Sub
'End Class

Class UnicodeTest1
    Inherits TestBase

    Protected Overrides Sub OnTest(DB As OraDatabase)
        Dim nRet As Integer
        Dim unicodeValue As String = "ԐԐԐԐԐԐԐԐԐԐ"

        With DB
            .ResetTrans()
            .BeginTrans()

            With .Parameters
                .Clear()
                .AddTable("SEQ", ORAPARM_INPUT, ORATYPE_NUMBER, 1)
                .AddTable("CODE", ORAPARM_INPUT, ORATYPE_VARCHAR2, 1, 10)
                .AddTable("NAME", ORAPARM_INPUT, ORATYPE_VARCHAR2, 1, 40)
                .Item("SEQ").put_Value(1, 0)
                .Item("CODE").put_Value(String.Format("CODE{0:0000}", 1), 0)
                .Item("NAME").put_Value(unicodeValue, 0)
            End With

            nRet = DB.ExecuteSQL("DELETE FROM TBL_MASTER")
            nRet = .ExecuteSQL("INSERT INTO TBL_MASTER(SEQ, CODE, NAME) VALUES (:SEQ, :CODE, :NAME)")

            .CommitTrans()
            .Parameters.Clear()


            Using rec As OraDynaset = DB.CreateDynaset("SELECT * FROM TBL_MASTER", ORADYN_DEFAULT)

                Check(rec.Fields("NAME").Value = unicodeValue, "UNICODE 取得")

            End Using

        End With
    End Sub
End Class

Class UnicodeTest2
    Inherits TestBase

    Protected Overrides Sub OnTest(DB As OraDatabase)
        Dim unicodeValue As String = "ԐԐԐԐԐԐԐԐԐԐ"
        Dim sql As New StringBuilder

        With DB.Parameters
            .Clear()
            .Add("INPUT_VALUE", unicodeValue, ORAPARM_INPUT, ORATYPE_NVARCHAR2)
            .Add("OUTPUT_VALUE", unicodeValue, ORAPARM_OUTPUT, ORATYPE_NVARCHAR2)
            .Add("RETURN_VALUE", unicodeValue, ORAPARM_OUTPUT, ORATYPE_NVARCHAR2)
        End With

        sql.AppendLine("BEGIN")
        sql.AppendLine("    :RETURN_VALUE   ")
        sql.AppendLine("    := FUNC_NVARCHAR2(:INPUT_VALUE, :OUTPUT_VALUE)")
        sql.AppendLine("    ;")
        sql.AppendLine("END;")

        DB.ExecuteSQL(sql.ToString())

        Dim outputValue As String = DB.Parameters("OUTPUT_VALUE").Value.ToString()
        Dim returnValue As String = DB.Parameters("RETURN_VALUE").Value.ToString()

        Check(outputValue = unicodeValue, String.Format("OUTPUT_VALUE = {0}", outputValue))
        Check(returnValue = unicodeValue, String.Format("RETURN_VALUE = {0}", returnValue))

        Dim selectSQL As String = "SELECT ROWID AS InternalRow, T.* FROM TMP T FOR UPDATE"
        Using rec As OraDynaset = DB.CreateDynaset(selectSQL, ORADYN_DEFAULT)
            Do Until rec.EOF
                Dim updateSQL As String = "UPDATE TMP SET ITEM = 'test' WHERE ROWID = '" & rec.Fields("InternalRow").Value & "'"
                DB.ExecuteSQL(updateSQL)
                rec.MoveNext()
            Loop
        End Using
    End Sub
End Class

'Class TestSubstitution1
'    Inherits TestBase

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "OraVariant 代入テスト(.NET)"

'        Dim ov As OraVariant
'        ov = CStr("1")
'        ov = CChar("1")
'        ov = DateTime.Now
'        ov = CByte("1")
'        ov = CSByte("1")
'        ov = CShort("1")
'        ov = CUShort("1")
'        ov = CInt("1")
'        ov = CUInt("1")
'        ov = CLng("1")
'        ov = CULng("1")

'        ov = CSng("1")
'        ov = CDbl("1")

'        ov = CDec("1")

'        ov = CDbl(Decimal.MaxValue) + 1
'    End Sub

'End Class

'Class TestSubstitution2
'    Inherits TestBase

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "OraVariant 代入テスト(ODP.NET)"
'        Dim v As OraVariant
'        Dim S As String
'        Dim B As Byte()
'        Dim D As DateTime
'        Dim T As TimeSpan

'        v = New OracleBinary(New Byte() {1, 2, 3})
'        Dim OracleBinary As OracleBinary = v
'        S = v
'        B = v

'        v = New OracleDate(1, 1, 1, 1, 1, 1)
'        Dim OracleDate As OracleDate = v
'        S = v
'        B = v
'        D = v
'        T = v

'        v = New OracleDecimal("123456")
'        Dim OracleDecimal As OracleDecimal = v
'        S = v
'        B = v
'        D = v
'        T = v

'        v = New OracleIntervalDS(1, 0, 0, 0, 0)
'        Dim OracleIntervalDS As OracleIntervalDS = v
'        S = v
'        B = v
'        D = v
'        T = v


'        v = New OracleIntervalYM(1, 0)
'        Dim OracleIntervalYM As OracleIntervalYM = v
'        S = v
'        B = v
'        D = v
'        T = v

'        v = New OracleString("123456")
'        Dim OraDataOracleString As OracleString = v
'        S = v
'        B = v
'        D = v
'        T = v

'        v = New OracleTimeStamp(1, 1, 1, 1, 1, 1, 1)
'        Dim OracleTimeStamp As OracleTimeStamp = v
'        S = v
'        B = v
'        D = v
'        T = v

'        v = New OracleTimeStampLTZ(1, 1, 1, 1, 1, 1, 1)
'        Dim OracleTimeStampLTZ As OracleTimeStampLTZ = v

'        v = New OracleTimeStampTZ(1999, 1, 1)
'        Dim OracleTimeStampTZ As OracleTimeStampTZ = v
'        S = v
'        B = v
'        D = v
'        T = v


'        Dim OracleBFile As OracleBFile = DB.CreateBFile("MYDIR", "TEST2.txt")
'        OracleBFile.OpenFile()
'        OracleBFile.CloseFile()

'        v = OracleBFile
'        OracleBFile = v
'        S = v
'        B = v

'    End Sub
'End Class

Class TestDataClear
    Inherits TestBase

    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
        Title = "テストデータ削除"

        DB.BeginTrans()
        DB.ExecuteSQL("DELETE FROM TBL_ALLTYPES")
        DB.ExecuteSQL("DELETE FROM TBL_LOB")
        DB.ExecuteSQL("DELETE FROM TBL_BFILE")
        DB.ExecuteSQL("DELETE FROM TBL_LONG")
        DB.ExecuteSQL("DELETE FROM TBL_MASTER")
        DB.CommitTrans()

        Dim builder As New StringBuilder
        builder.AppendLine("SELECT")
        builder.AppendLine("    COUNT(*) CNT")
        builder.AppendLine("FROM")
        builder.AppendLine("    TBL_ALLTYPES")
        builder.AppendLine(",   TBL_LOB")
        builder.AppendLine(",   TBL_BFILE")
        builder.AppendLine(",   TBL_LONG")
        builder.AppendLine(",   TBL_MASTER")

        Dim rec As OraDynaset = DB.CreateDynaset(builder.ToString(), ORADYN_NOCACHE)

        Check(CInt(rec.Fields("CNT").Value) = 0, "テストデータ削除")

    End Sub

End Class

'Class TestChangePassword
'    Inherits TestBase

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "パスワードの変更"

'        Dim userName As String = "HOGE"
'        Dim passWord As String = "PASS"
'        Dim newPassWord As String = "NEWPASS"
'        Dim index As Integer = 0

'        Using rec As OraDynaset = DB.CreateDynaset("SELECT USERNAME FROM ALL_USERS", ORADYN_READONLY)
'            Do
'                rec.FindFirst("USERNAME = '" & userName & "'")
'                If rec.NoMatch Then Exit Do
'                index += 1
'                userName = "HOGE" & index.ToString()
'            Loop
'        End Using

'        Check(True, String.Format("ユーザの作成 {0}/{1}", userName, passWord))

'        DB.ExecuteSQL(String.Format("CREATE USER {0} IDENTIFIED BY {1} PASSWORD EXPIRE", userName, passWord))
'        DB.ExecuteSQL(String.Format("GRANT CONNECT TO {0}", userName))

'        Try

'            Dim session As New OraSessionClass
'            Dim database As OraDatabase = Nothing
'            Try
'                Console.WriteLine("旧パスワードでログイン")
'                database = session.OpenDatabase(HostName, userName & "/" & passWord, ORADB_DEFAULT)

'            Catch ex As Exception
'                If session.LastServerErr = 28001 Then
'                    Console.WriteLine("パスワード期限切れ。パスワード変更")
'                    session.ChangePassword(HostName, userName, passWord, newPassWord)
'                    Console.WriteLine("新パスワードでログイン")
'                    database = session.OpenDatabase(HostName, userName & "/" & newPassWord, ORADB_DEFAULT)
'                    Console.WriteLine("成功")
'                Else
'                    Console.WriteLine(session.LastServerErrText)
'                End If
'            End Try

'            database.Close()

'        Catch ex As Exception
'            Console.WriteLine(ex.Message)

'        Finally
'            Check(True, String.Format("ユーザの削除 {0}", userName))
'            DB.ExecuteSQL(String.Format("DROP USER {0} CASCADE", userName))
'        End Try

'    End Sub
'End Class

'Class TestUpdateDynaset
'    Inherits TestBase

'    Sub SubTest(ByVal DB As OraDatabase, ByVal options As dynOption)
'        Console.WriteLine(options.ToString())

'        ' 選択列にプライマリキーが存在していれば行を更新できます。

'        Dim rec As OraDynaset
'        Dim seq As String

'        'DB.BeginTrans()

'        rec = DB.CreateDynaset("SELECT SEQ_SEQ.NEXTVAL SEQ FROM DUAL", options)
'        seq = rec.Fields("seq").Value          ' 大文字小文字を区別しないはず

'        ' インデクサのテスト
'        seq = rec("seq").Value
'        seq = rec.Item("seq").Value


'        rec = DB.CreateDynaset("SELECT T.* FROM TBL_ALLTYPES T FOR UPDATE", options)
'        Dim nFieldCount As Integer = 0

'        Dim lst As New List(Of String)

'        For Each f As OraField In rec.Fields
'            lst.Add(f.Name)
'            nFieldCount = nFieldCount + 1
'        Next

'        Check(rec.Fields.Count = nFieldCount, String.Format("Enumurator のテスト(数) {0}, {1}", rec.Fields.Count, nFieldCount))

'        For i As Integer = 0 To nFieldCount - 1
'            Check(lst(i) = rec.Fields(i).Name, lst(i) & "," & rec.Fields(i).Name)
'        Next

'        rec.AddNew()
'        rec.Fields("SEQ").Value = seq
'        rec.Fields("ITEM_VARCHAR2").Value = "VARCHAR2"
'        rec.Fields("ITEM_CHAR").Value = "CHAR"
'        rec.Fields("ITEM_DATE").Value = Now
'        rec.Fields("ITEM_INT16").Value = 1234
'        rec.Fields("ITEM_INT32").Value = 123456789
'        rec.Fields("ITEM_INT64").Value = 123456789012345678@
'        rec.Fields("ITEM_DECIMAL").Value = 1234567890123456789@
'        rec.Fields("ITEM_RAW").Value = New Byte() {0, 1, 2, 3}
'        rec.Fields("ITEM_TIMESTAMP").Value = New OracleTimeStamp(Date.Now.Year, Date.Now.Month, Date.Now.Day)
'        rec.Fields("ITEM_TIMESTAMPLTZ").Value = New OracleTimeStampLTZ(Date.Now.Year, Date.Now.Month, Date.Now.Day)
'        rec.Fields("ITEM_TIMESTAMPTZ").Value = New OracleTimeStampTZ(Date.Now.Year, Date.Now.Month, Date.Now.Day)
'        rec.Fields("ITEM_INTERVALDS").Value = New OracleIntervalDS(1, 0, 0, 0, 0)
'        rec.Fields("ITEM_INTERVALYM").Value = New OracleIntervalYM(1, 1)
'        rec.Fields("ITEM_SINGLE").Value = 123456.7@
'        rec.Fields("ITEM_DOUBLE").Value = 12345678901234.5@
'        rec.Fields("ITEM_DECIMAL2").Value = 123456789012345.6@

'        Dim itemColletion As New Dictionary(Of String, OraVariant)
'        For Each f As OraField In rec.Fields
'            itemColletion.Add(f.Name, f.Value)
'        Next

'        rec.Update()

'        rec = DB.CreateDynaset("SELECT T.* FROM TBL_ALLTYPES T WHERE T.SEQ = " & seq, options)

'        For Each f As OraField In rec.Fields
'            Dim a As OraVariant = itemColletion.Item(f.Name)
'            Dim b As OraVariant = f.Value

'            Check(a = b, f.Name & "に書き込んだ値")
'        Next

'        Console.WriteLine("Refresh 後に比較")
'        rec.Refresh()
'        rec.MoveFirst()
'        For Each f As OraField In rec.Fields
'            Check(itemColletion.Item(f.Name) = f.Value, f.Name & "に書き込んだ値")
'        Next

'        Console.WriteLine("全列 NULL で更新")
'        rec = DB.CreateDynaset("SELECT * FROM TBL_ALLTYPES WHERE SEQ = " & seq, options)
'        For Each f As OraField In rec.Fields
'            If String.Compare(f.Name, "SEQ", True) <> 0 Then
'                f.Value = Nothing
'            End If
'        Next
'        rec.Update()
'        rec = DB.CreateDynaset("SELECT * FROM TBL_ALLTYPES WHERE SEQ = " & seq, options)
'        For Each f As OraField In rec.Fields
'            Console.WriteLine(f.Name & "=" & f.Value)
'        Next

'        Console.WriteLine("全列 DBNull.Value で更新")
'        rec = DB.CreateDynaset("SELECT * FROM TBL_ALLTYPES WHERE SEQ = " & seq, options)
'        For Each f As OraField In rec.Fields
'            If String.Compare(f.Name, "SEQ", True) <> 0 Then
'                f.Value = DBNull.Value
'            End If
'        Next
'        rec.Update()

'        rec = DB.CreateDynaset("SELECT * FROM TBL_ALLTYPES WHERE SEQ = " & seq, options)
'        For Each f As OraField In rec.Fields
'            Console.WriteLine(f.Name & "=" & f.Value)
'        Next
'        rec.Update()

'        rec = DB.CreateDynaset("SELECT * FROM TBL_ALLTYPES WHERE SEQ = " & seq, options)
'        rec.Delete()

'        rec = DB.CreateDynaset("SELECT * FROM TBL_ALLTYPES WHERE SEQ = " & seq, options)
'        Check(rec.EOF, "Dynaset.Delete()")

'    End Sub

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "全型の行を更新"
'        SubTest(DB, ORADYN_NOCACHE)
'        SubTest(DB, ORADYN_DEFAULT)
'        SubTest(DB, ORADYN_ASYNC_READ)
'    End Sub

'End Class

'Class TestUpdateDynaset2
'    Inherits TestBase

'    Sub SubTest(ByVal DB As OraDatabase, ByVal options As dynOption)
'        Console.WriteLine(options.ToString())

'        ' プライマリキーが無いテーブルは ROWID 列を含めると更新できます。

'        DB.ExecuteSQL("DELETE FROM TBL_NOPRIMARY")
'        DB.ExecuteSQL("INSERT INTO TBL_NOPRIMARY(ITEM1, ITEM2, ITEM3) VALUES ('AAA', 10.1, SYSDATE)")

'        Using r As OraDynaset = DB.CreateDynaset("SELECT T.*,  T.ROWID FROM TBL_NOPRIMARY T", ORADYN_DEFAULT)

'            ' 追加した行に位置づける
'            r.FindFirst("ITEM1 = 'AAAA'")
'            Console.WriteLine(String.Format("ROWID = {0}", r("ROWID").Value))

'            r.Edit()
'            r.Fields("ITEM1").Value = "BBBB"
'            r.Update()
'            Console.WriteLine(String.Format("ROWID = {0}", r("ROWID").Value))

'            r.Edit()
'            r.Fields("ITEM1").Value = "CCCC"
'            r.Update()
'            Console.WriteLine(String.Format("ROWID = {0}", r("ROWID").Value))

'            r.Edit()
'            r.Fields("ITEM1").Value = "DDDD"
'            r.Update()
'            Console.WriteLine(String.Format("ROWID = {0}", r("ROWID").Value))
'        End Using

'    End Sub

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "全型の行を更新(プライマリキー無し)"

'        SubTest(DB, ORADYN_NOCACHE)
'        SubTest(DB, ORADYN_DEFAULT)
'        SubTest(DB, ORADYN_ASYNC_READ)

'    End Sub
'End Class

Class TestDuplicateField
    Inherits TestBase

    Sub SubTest(ByVal DB As OraDatabase, ByVal options As dynOption)
        Console.WriteLine(options.ToString())
        Dim sql As String = "SELECT * FROM TBL_MASTER A, TBL_MASTER B"
        Using dyn As OraDynaset = DB.CreateDynaset(sql, options)
            For Each f As OraField In dyn.Fields
                Console.WriteLine(String.Format("Name = {0}, Value = {1}", f.Name, f.Value))
            Next
        End Using

    End Sub

    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
        Title = "重複した項目名のテスト"

        SubTest(DB, ORADYN_NOCACHE)
        SubTest(DB, ORADYN_DEFAULT)

    End Sub

End Class

Class TestBFile
    Inherits TestBase

    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
        Title = "BFile"

        Dim sql As New StringBuilder

        ' MYDIR にテキストファイルを作成しておく
        sql.AppendLine("DECLARE")
        sql.AppendLine("    vHandle UTL_FILE.FILE_TYPE;")
        sql.AppendLine("BEGIN")
        sql.AppendLine("    vHandle := UTL_FILE.FOPEN('MYDIR','TEST1.txt','w',32767);")
        sql.AppendLine("    UTL_FILE.PUT_LINE(vHandle, 'BFILEテスト1');")
        sql.AppendLine("    UTL_FILE.FCLOSE(vHandle);")
        sql.AppendLine("    vHandle := UTL_FILE.FOPEN('MYDIR','TEST2.txt','w',32767);")
        sql.AppendLine("    UTL_FILE.PUT_LINE(vHandle, 'BFILEテスト2');")
        sql.AppendLine("    UTL_FILE.FCLOSE(vHandle);")
        sql.AppendLine("    vHandle := UTL_FILE.FOPEN('MYDIR','TEST3.txt','w',32767);")
        sql.AppendLine("    UTL_FILE.PUT_LINE(vHandle, 'BFILEテスト3');")
        sql.AppendLine("    UTL_FILE.FCLOSE(vHandle);")
        sql.AppendLine("EXCEPTION WHEN OTHERS THEN")
        sql.AppendLine("	UTL_FILE.FCLOSE_ALL;")
        sql.AppendLine("	RAISE;")
        sql.AppendLine("END;")
        sql.Replace(vbCrLf, vbLf)

        DB.ExecuteSQL(sql.ToString)

        ' 更新開始
        DB.BeginTrans()

        Dim rec As OraDynaset
        Dim rec2 As OraDynaset
        Dim seq As String
        Using seqRec = DB.CreateDynaset("SELECT SEQ_SEQ.NEXTVAL SEQ FROM DUAL", ORADYN_READONLY)
            seq = seqRec.Fields("SEQ").Value
        End Using

        ' BFILE はちょっと特殊
        Dim bf As New OraBFile(DB)
        bf.DirectoryName = "MYDIR"
        bf.FileName = "TEST1.txt"

        DB.Parameters.Clear()
        DB.Parameters.Add("SEQ", seq, paramMode.ORAPARM_INPUT, serverType.ORATYPE_NUMBER)
        DB.Parameters.Add("ITEM_BFILE", bf, paramMode.ORAPARM_INPUT, serverType.ORATYPE_BFILE)

        DB.ExecuteSQL("INSERT INTO TBL_BFILE(SEQ, ITEM_BFILE) VALUES (:SEQ, :ITEM_BFILE)")
    End Sub

End Class

Class TestParamArray
    Inherits TestBase

    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)

        Title = "ParamArrays"

        Dim ps As OraParameters = DB.Parameters

        ' 引数の逆順に追加(名称でバインドされるかテスト)
        With ps
            .Clear()
            .AddTable("addValue3", ORAPARM_INPUT, ORATYPE_NUMBER, 2)
            .AddTable("addValue2", ORAPARM_INPUT, ORATYPE_VARCHAR2, 2)
            .AddTable("addValue1", ORAPARM_INPUT, ORATYPE_VARCHAR2, 2)
        End With

        Dim addValue1 As OraParameter = ps("addValue1")
        Dim addValue2 As OraParameter = ps("addValue2")
        Dim addValue3 As OraParameter = ps("addValue3")

        ' 空白は除去される
        addValue1.put_Value("001 ", 0)
        addValue1.put_Value("002 ", 1)

        addValue2.put_Value("相手先名称001", 0)
        addValue2.put_Value("相手先名称002", 1)

        addValue3.put_Value(1.5, 0)
        addValue3.put_Value(8.8, 1)

        DB.BeginTrans()

        Console.WriteLine("ExecuteSQL 1 回目")
        DB.ExecuteSQL("INSERT INTO TBL_MASTER(SEQ,CODE,NAME,KINGAKU) VALUES (SEQ_SEQ.NEXTVAL, :addValue1,:addValue2,:addValue3)")
        Console.WriteLine("ExecuteSQL 2 回目")
        DB.ExecuteSQL("INSERT INTO TBL_MASTER(SEQ,CODE,NAME,KINGAKU) VALUES (SEQ_SEQ.NEXTVAL, :addValue1,:addValue2,:addValue3)")

        DB.CommitTrans()

        Console.WriteLine("OraParamArrays 列挙")
        For Each p As OraParamArray In DB.Parameters
            Console.WriteLine(p.Name)
        Next

        ' オートバインドしない
        addValue1.AutoBindDisable()
        addValue2.AutoBindDisable()
        addValue3.AutoBindDisable()

        Dim Rec As OraDynaset

        ps.Add("CODE", "", ORAPARM_INPUT, ORATYPE_VARCHAR2)
        Dim selectValue As OraParameter = ps("CODE")

        ps("CODE").Value = "001"
        Rec = DB.CreateDynaset("SELECT * FROM TBL_MASTER WHERE CODE =:CODE", ORADYN_NOCACHE)
        Check(ps("CODE").Value.ToString() = Rec("CODE").Value.ToString(), "更新レコード取得")

        'ps("CODE").Value = "002"
        'Rec.Refresh()
        'Check(ps("CODE").Value.ToString = Rec("CODE").Value, "更新レコード取得")

        ps.Clear()

    End Sub
End Class

'Class TestLOB
'    Inherits TestBase

'    Sub SubTest(ByVal DB As OraDatabase, ByVal options As dynOption)
'        Console.WriteLine(options.ToString())

'        DB.BeginTrans()

'        Dim bytes(65536) As Byte
'        Using Rec As OraDynaset = DB.CreateDynaset("SELECT A.* FROM TBL_LOB A FOR UPDATE", options)
'            Using seqRec As OraDynaset = DB.CreateDynaset("SELECT SEQ_SEQ.NEXTVAL SEQ FROM DUAL", options)
'                Rec.AddNew()
'                Rec.Fields("SEQ").Value = seqRec.Fields("SEQ").Value
'                Rec.Update()

'                Using Rec2 As OraDynaset = DB.CreateDynaset("SELECT A.* FROM TBL_LOB A WHERE SEQ = " & seqRec.Fields("seq").Value, options)
'                    ' 1フィールドが 4000バイトを超える場合は複数フィールドを一度で更新できないので複数回に分けて更新する
'                    Rec2.MoveFirst()
'                    If Not Rec2.EOF Then
'                        Rec2.Fields("ITEM_BLOB").Value = bytes
'                        Rec2.Update()
'                    End If

'                    Rec2.MoveFirst()
'                    If Not Rec2.EOF Then
'                        Rec2.Fields("ITEM_CLOB").Value = New String("C".ToCharArray()(0), 65536)
'                        Rec2.Update()
'                    End If
'                End Using
'            End Using
'        End Using

'        DB.CommitTrans()
'    End Sub

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "BLOB,CLOB"
'        SubTest(DB, ORADYN_NOCACHE)
'        SubTest(DB, ORADYN_DEFAULT)
'        SubTest(DB, ORADYN_ASYNC_READ)
'    End Sub
'End Class

'Class TestUpdateDynasetOption
'    Inherits TestBase

'    Dim WithEvents EventDynaset As OraDynaset


'    Sub SubTest(ByVal DB As OraDatabase, ByVal options As dynOption)
'        Console.WriteLine(String.Format("## {0} Dyneset による行更新テスト開始 ##", options.ToString))

'        Const nCount As Integer = 100

'        TBL_MASTER.Create(DB, nCount)

'        DB.ResetTrans()


'        Dim dtStart As DateTime = Now

'        Dim sql As String = "SELECT * FROM TBL_MASTER A ORDER BY SEQ"
'        Dim rec As OraDynaset = DB.CreateDynaset(sql, options)
'        'Dim dt As DataTable = rec.GetDataTable


'        Dim addCount As Integer = 0
'        Dim readCount As Integer = 0
'        Dim delCount As Integer = 0

'        rec.MoveFirst()

'        Dim nRecCount As Integer = 0

'        Do Until rec.EOF
'            readCount += 1
'            nRecCount += 1

'            If nRecCount Mod 10 = 0 Then
'                rec.Fields("CODE").Value = "CODE" & nRecCount.ToString
'                rec.Update()
'                rec.AddNew()
'                rec.Fields("SEQ").Value = nCount + addCount + 1
'                rec.Update()
'                addCount += 1
'            End If

'            If rec.Fields("SEQ").Value <> nRecCount Then
'                Check(False, String.Format("更新がおかしいよ！{0} 件 SEQ={1}", nRecCount, rec.Fields("SEQ").Value))
'                Exit Sub
'            End If
'            rec.Delete()
'            rec.Update()

'            delCount += 1
'            rec.MoveNext()

'            Console.Write(vbCr & String.Format("追加件数 {0} 読込件数 {1} 削除件数 {2}", addCount, readCount, delCount))
'            If nRecCount Mod 100 = 0 Then
'            End If
'        Loop
'        Console.WriteLine("")


'        Console.WriteLine(String.Format("## {0} Dyneset による行更新テスト終了 ## {1} ms", options.ToString, (Now - dtStart).TotalMilliseconds))

'        Check(True, "行追加エリアへのアクセス")
'        rec = DB.CreateDynaset(sql, options)

'        Check(rec.EOF, "行削除OK")

'    End Sub

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "Dynaset による行更新テスト"
'        SubTest(DB, ORADYN_DEFAULT)
'        SubTest(DB, ORADYN_ASYNC_READ)
'        SubTest(DB, ORADYN_NOCACHE)
'    End Sub

'End Class

Class TestOutputParameter
    Inherits TestBase

    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
        Title = "Parameters OUTPUT"
        Dim sql As New StringBuilder
        Dim ps As OraParameters
        Dim funcValue As String
        Dim rec As OraDynaset

        ps = DB.Parameters

        DB.Parameters.Clear()
        DB.Parameters.Add("RESULT", Nothing, ORAPARM_OUTPUT, ORATYPE_VARCHAR2)
        Dim result As OraParameter = DB.Parameters("RESULT")

        sql.AppendLine("DECLARE")
        sql.AppendLine("    RESULT VARCHAR2(20);")
        sql.AppendLine("BEGIN")
        sql.AppendLine("    :RESULT := FUNC_HOGE();")
        sql.AppendLine("END;")

        DB.ExecuteSQL(sql.ToString)

        funcValue = result.Value.ToString()

        Console.WriteLine("FUNC_HOGE() RETURN = " & funcValue)
        Do Until DB.Parameters.Count = 0
            DB.Parameters.Remove(0)
        Loop

        rec = DB.CreateDynaset("SELECT FUNC_HOGE() FROM DUAL", ORADYN_READONLY)

        Console.WriteLine(rec.Fields(0).Value)

        If funcValue = rec.Fields(0).Value Then
            Console.WriteLine("Parameters OUTPUT OK!!!")
        Else
            Console.WriteLine("******** Parameters OUTPUT NG *********")
        End If
        Dim a As String = rec.Fields(0).Value & rec.Fields(0).Value
    End Sub
End Class

'Class TestCopyToClipboard
'    Inherits TestBase

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "CopyToClipboard"

'        Const nCount As Integer = 100

'        DB.ExecuteSQL("DELETE FROM TBL_MASTER")
'        Using Rec As OraDynaset = DB.CreateDynaset("SELECT * FROM TBL_MASTER ORDER BY SEQ", 0)
'            Rec.CopyToClipboard()
'        End Using

'        Call TBL_MASTER.Create(DB, nCount)

'        Using Rec As OraDynaset = DB.CreateDynaset("SELECT * FROM TBL_MASTER ORDER BY SEQ", 0)
'            Rec.CopyToClipboard()
'        End Using

'    End Sub
'End Class

'Class TestDynasetMethods
'    Inherits TestBase

'    Sub SubTest(ByVal DB As OraDatabase, ByVal options As dynOption)
'        Console.WriteLine(options.ToString())

'        Title = "Find系メソッド"

'        Dim Rec As OraDynaset
'        Dim Filter As String = "OWNER=" & EditItem(UID)
'        Dim nCount As Integer = 0
'        Dim nCount1 As Integer
'        Dim nCount2 As Integer

'        Rec = DB.CreateDynaset("SELECT COUNT(*) FROM DBA_TABLES", ORADYN_NOCACHE)
'        nCount1 = CInt(Rec.Fields(0).Value)
'        nCount2 = CInt(Rec.Fields(0))
'        Console.WriteLine(Rec.SQL & " : " & nCount1.ToString & " 件")

'        Rec = DB.CreateDynaset("SELECT COUNT(*) FROM DBA_TABLES WHERE OWNER = " & EditItem(UID), ORADYN_NOCACHE)
'        nCount2 = CInt(Rec.Fields(0).Value)
'        Console.WriteLine(Rec.SQL & " : " & nCount2.ToString & " 件")

'        Rec = DB.CreateDynaset("SELECT * FROM DBA_TABLES", options)
'        Check(Rec.RecordCount = nCount1, String.Format("RecordCount 数={0}", Rec.RecordCount))

'        nCount = 0
'        Rec.FindFirst(Filter)
'        Do Until Rec.NoMatch
'            nCount = nCount + 1
'            Rec.FindNext(Filter)
'        Loop
'        Check(nCount2 = nCount, String.Format("RecordCount 数={0}, Find 数={1}", nCount2, nCount))

'        Rec.FindLast(Filter)
'        Do Until Rec.NoMatch
'            nCount = nCount - 1
'            Rec.FindPrevious(Filter)
'        Loop
'        Check(nCount = 0, "FIND メソッドの件数")

'        nCount = 0
'        Rec.MoveFirst()
'        Do Until Rec.EOF
'            nCount = nCount + 1
'            Rec.MoveNext()
'        Loop

'        Rec.MoveLast()
'        Do Until Rec.BOF
'            nCount = nCount - 1
'            Rec.MovePrevious()
'        Loop
'        Check(nCount = 0, "MOVE メソッドの件数")
'    End Sub


'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "Find系, Move系メソッドのテスト"

'        SubTest(DB, ORADYN_DEFAULT)
'    End Sub
'End Class

Class TestSQLInsert
    Inherits TestBase

    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
        Title = "SQL による 1000件 INSERT"
        Const nCount As Integer = 1000

        DB.BeginTrans()
        For i As Integer = 1 To nCount
            DB.ExecuteSQL("INSERT INTO TBL_MASTER(SEQ,CODE,NAME,KINGAKU) VALUES (SEQ_SEQ.NEXTVAL, 'CODE0', 'NAME', 1.5)")
            'Console.Write(vbCr & String.Format("追加件数 {0}/{1}", i, nCount))
        Next
        DB.CommitTrans()

        'Console.Write(vbCr)
        Check(True, String.Format("処理件数 = {0}", nCount))
    End Sub
End Class

Class TestParameterInsert
    Inherits TestBase

    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
        Title = "Parameter による 1000件 INSERT"

        Const nCount As Integer = 1000

        DB.Parameters.Clear()
        DB.Parameters.Add("CODE", "CODE1", ORAPARM_INPUT, ORATYPE_VARCHAR2)
        DB.Parameters.Add("NAME", "NAME", ORAPARM_INPUT, ORATYPE_VARCHAR2)
        DB.Parameters.Add("KINGAKU", 1.5, ORAPARM_INPUT, ORATYPE_NUMBER)
        DB.Parameters.Add("HIDUKE", Now, ORAPARM_INPUT, ORATYPE_DATE)
        DB.Parameters.Add("HIDUKE2", Now.ToString, ORAPARM_INPUT, ORATYPE_DATE)

        Console.WriteLine("OraParameter 列挙")
        For Each op As OraParameter In DB.Parameters
            Console.WriteLine(op.Name)
        Next

        Dim P(1) As OraParameter
        P(0) = DB.Parameters(0)
        P(1) = DB.Parameters(1)
        P(0).MinimumSize = 5
        P(1).MinimumSize = 10

        DB.BeginTrans()
        For i As Integer = 1 To nCount
            DB.ExecuteSQL("INSERT INTO TBL_MASTER(SEQ,CODE,NAME,KINGAKU,HIDUKE,HIDUKE2) VALUES (SEQ_SEQ.NEXTVAL,:CODE,:NAME,:KINGAKU,:HIDUKE,:HIDUKE2)")
            'Console.Write(vbCr & String.Format("追加件数 {0}/{1}", i, nCount))
        Next
        DB.CommitTrans()

        Do Until DB.Parameters.Count = 0
            DB.Parameters.Remove(0)
        Loop

        'Console.Write(vbCr)
        Check(True, String.Format("処理件数 = {0}", nCount))

    End Sub
End Class

Class TestParamArrayInsert
    Inherits TestBase

    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
        Title = "ParamArray による 1000件 INSERT"

        Const nCount As Integer = 1000
        DB.BeginTrans()

        Dim pas As OraParameters = DB.Parameters

        pas.AddTable("CODE", ORAPARM_INPUT, ORATYPE_VARCHAR2, nCount)
        pas.AddTable("NAME", ORAPARM_INPUT, ORATYPE_VARCHAR2, nCount)
        pas.AddTable("KINGAKU", ORAPARM_INPUT, ORATYPE_NUMBER, nCount)
        pas.AddTable("HIDUKE", ORAPARM_INPUT, ORATYPE_DATE, nCount)
        pas.AddTable("HIDUKE2", ORAPARM_INPUT, ORATYPE_DATE, nCount)


        Dim p(5) As OraParameter
        p(0) = pas(0)
        p(1) = pas(1)
        p(2) = pas(2)
        p(3) = pas(3)
        p(4) = pas(4)

        For i As Integer = 0 To nCount - 1
            p(0).put_Value("CODE2", i)
            p(1).put_Value("NAME", i)
            p(2).put_Value(1.5, i)
            p(3).put_Value(Now.ToString(), i)
            p(4).put_Value(Now, i)
        Next
        Dim nRet As Integer
        nRet = DB.ExecuteSQL("INSERT INTO TBL_MASTER(SEQ,CODE,NAME,KINGAKU,HIDUKE,HIDUKE2) VALUES (SEQ_SEQ.NEXTVAL, :CODE, :NAME, :KINGAKU, :HIDUKE, :HIDUKE2)")

        DB.CommitTrans()

        Do Until DB.Parameters.Count = 0
            DB.Parameters.Remove(0)
        Loop

        Check(nRet = nCount, String.Format("処理件数 = {0}", nRet))


        Dim sql As String = "SELECT COUNT(*) CNT FROM TBL_MASTER"
        Dim nRecordCount As Integer

        Dim rec As OraDynaset = DB.CreateDynaset(sql, ORADYN_DEFAULT)
        nRecordCount = CInt(rec.Fields("CNT").Value)

    End Sub
End Class

'Class TestCreateSQLSync
'    Inherits TestBase

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "CreateSQL による 100件 INSERT x 2 (同期)"
'        Dim sql As String
'        Const nCount As Integer = 100

'        DB.ExecuteSQL("DELETE FROM TBL_MASTER")

'        DB.BeginTrans()

'        Dim pas As OraParamArrays = DB.ParamArrays

'        pas.Clear()
'        pas.AddTable("SEQ", ORAPARM_INPUT, ORATYPE_NUMBER, nCount)
'        pas.AddTable("CODE", ORAPARM_INPUT, ORATYPE_VARCHAR2, nCount, 10)
'        pas.AddTable("NAME", ORAPARM_INPUT, ORATYPE_VARCHAR2, nCount, 10)
'        pas.AddTable("KINGAKU", ORAPARM_INPUT, ORATYPE_NUMBER, nCount)

'        For i As Integer = 0 To nCount - 1
'            pas("SEQ").put_Value(i, i)
'            pas("CODE").put_Value("CODE", i)
'            pas("NAME").put_Value("NAME", i)
'            pas("KINGAKU").put_Value(1.5, i)
'        Next

'        sql = "INSERT INTO TBL_MASTER(SEQ,CODE,NAME,KINGAKU) VALUES (:SEQ, :CODE, :NAME, :KINGAKU)"
'        Dim stmt As OraSqlStmt = DB.CreateSQL(sql, sqlstmtObjOption.ORASQL_FAILEXEC, False)
'        Dim nRet As Integer = stmt.RecordCount

'        Check(nRet = nCount, String.Format("処理件数 = {0}", nRet))

'        'パラメータを修正して Refresh 時に反映されるか？
'        For i As Integer = 0 To nCount - 1
'            pas("SEQ").put_Value(i + nCount, i)
'            pas("CODE").put_Value("CODE", i)
'            pas("NAME").put_Value("NAME", i)
'            pas("KINGAKU").put_Value(1.5, i)
'        Next

'        stmt.Refresh()
'        Check(nRet = nCount, String.Format("処理件数 = {0}", nRet))

'        DB.CommitTrans()
'    End Sub
'End Class

'Class TestCreateSQLAsync
'    Inherits TestBase

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "CreateSQL による非同期実行"

'        Dim stmt As OraSqlStmt

'        ' 新しい接続を開いてレコードを排他する
'        Using db2 As OraDatabase = DB.Session.OpenDatabase(HostName, ConnectionString, ORADB_DEFAULT)

'            TBL_MASTER.Create(DB, 2)

'            DB.BeginTrans()

'            db2.BeginTrans()
'            Dim recLock As OraDynaset = db2.CreateDynaset("SELECT * FROM TBL_MASTER FOR UPDATE", ORADYN_DEFAULT)

'            ' CreateSQL 非同期実行
'            Dim sql As New StringBuilder
'            sql.AppendLine("BEGIN")
'            sql.AppendLine("    DELETE FROM TBL_MASTER WHERE SEQ = (SELECT MIN(SEQ) FROM TBL_MASTER);")
'            sql.AppendLine("    RAISE_APPLICATION_ERROR(-20001,'エラーを起こします。');")
'            sql.AppendLine("END;")

'            DB.LastServerErrReset()
'            stmt = DB.CreateSQL(sql.ToString(), sqlstmtObjOption.ORASQL_NONBLK)

'            'キャンセル
'            stmt.Cancel()
'            db2.CommitTrans()       '排他待ちの場合は Wait している筈

'            Do
'                If stmt.NonBlockingState = ORASQL_SUCCESS Then
'                    Exit Do
'                End If
'                System.Threading.Thread.Sleep(100)
'            Loop

'            '取消したエラーが発生する筈
'            Check(DB.LastServerErr <> 0, String.Format("Database.LastServerErr = {0} {1}", DB.LastServerErr, DB.LastServerErrText))

'            DB.Rollback()
'            DB.BeginTrans()


'            ' 再度実行

'            db2.BeginTrans()
'            recLock.Refresh()

'            DB.LastServerErrReset()
'            stmt.Refresh()

'            db2.CommitTrans()
'            Do
'                If stmt.NonBlockingState = ORASQL_SUCCESS Then
'                    Exit Do
'                End If
'                System.Threading.Thread.Sleep(100)
'            Loop

'            'RAISE_APPLICATION エラーが起きている筈
'            Check(DB.LastServerErr = 20001, String.Format("Database.LastServerErr = {0} {1}", DB.LastServerErr, DB.LastServerErrText))

'            DB.ResetTrans()
'        End Using


'        '正常テスト(エラーはリセットしてね！0）

'        DB.BeginTrans()
'        DB.LastServerErrReset()
'        stmt = DB.CreateSQL("DELETE FROM TBL_MASTER WHERE SEQ = (SELECT MIN(SEQ) FROM TBL_MASTER)", sqlstmtObjOption.ORASQL_NONBLK)
'        stmt.WaitOne()
'        DB.CommitTrans()

'        '正常のはず

'        Check(stmt.NonBlockingState = ORASQL_SUCCESS, stmt.NonBlockingState.ToString)
'        Check(DB.LastServerErr = 0, String.Format("Database.LastServerErr = {0} {1}", DB.LastServerErr, DB.LastServerErrText))

'    End Sub

'End Class

Class TestParameterLONG
    Inherits TestBase

    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
        Title = "Parameter による LONG 更新"

        Dim p As OraParameter

        DB.Parameters.Clear()
        DB.Parameters.Add("ITEM_LONG", Nothing, ORAPARM_INPUT, ORATYPE_LONG)

        p = DB.Parameters("ITEM_LONG")
        p.Value = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

        DB.ExecuteSQL("INSERT INTO TBL_LONG(SEQ, ITEM_LONG) VALUES (SEQ_SEQ.NEXTVAL, :ITEM_LONG)")
        Do Until DB.Parameters.Count = 0
            DB.Parameters.Remove(0)
        Loop
    End Sub
End Class

Class TestParameterBLOB
    Inherits TestBase

    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
        Title = "Parameter による BLOB 更新"
        Dim p As OraParameter
        Dim bytes(65536) As Byte

        DB.Parameters.Clear()
        DB.Parameters.Add("ITEM_BLOB", Nothing, ORAPARM_INPUT, ORATYPE_BLOB)
        p = DB.Parameters("ITEM_BLOB")

        p.Value = bytes
        DB.ExecuteSQL("INSERT INTO TBL_LOB(SEQ, ITEM_BLOB) VALUES (SEQ_SEQ.NEXTVAL, :ITEM_BLOB)")
        Do Until DB.Parameters.Count = 0
            DB.Parameters.Remove(0)
        Loop
    End Sub
End Class

'''' <summary>
'''' PL/SQL 連想配列を使用する
'''' </summary>
'Class TestUsePLSQLAssociativeArray1
'    Inherits TestBase

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "PL/SQL 連想配列(1)"

'        Dim nArraySize As Integer = 5
'        Dim pas As OraParamArrays = DB.ParamArrays

'        DB.BeginTrans()

'        pas.Clear()
'        pas.AddTable("CODE", ORAPARM_INPUT, ORATYPE_VARCHAR2, nArraySize, 20)
'        pas.AddTable("NAME", ORAPARM_INPUT, ORATYPE_VARCHAR2, nArraySize, 40)
'        pas.AddTable("KINGAKU", ORAPARM_INPUT, ORATYPE_NUMBER, nArraySize)

'        For i As Integer = 0 To nArraySize - 1
'            pas("CODE").put_Value("CODE" & i.ToString(), i)
'            pas("NAME").put_Value("NAME" & i.ToString(), i)
'            pas("KINGAKU").put_Value(i * 1000, i)
'        Next

'        DB.ExecuteSQL("BEGIN PKG_MASTER.BULK_INSERT(:CODE, :NAME, :KINGAKU); END;", True)

'        pas.Clear()
'        DB.CommitTrans()

'    End Sub
'End Class

'''' <summary>
'''' PL/SQL 連想配列を使用する
'''' </summary>
'Class TestUsePLSQLAssociativeArray2
'    Inherits TestBase

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "PL/SQL 連想配列(2)"

'        Dim nArraySize As Integer = 5

'        With DB
'            .BeginTrans()
'            With .ParamArrays
'                .Clear()
'                .AddTable("CODE", ORAPARM_INPUT, ORATYPE_VARCHAR2, nArraySize, 20)
'                .AddTable("NAME", ORAPARM_INPUT, ORATYPE_VARCHAR2, nArraySize, 40)
'                .AddTable("KINGAKU", ORAPARM_INPUT, ORATYPE_NUMBER, nArraySize)
'                .AddTable("SEQ", ORAPARM_OUTPUT, ORATYPE_NUMBER, nArraySize)
'                For i As Integer = 0 To nArraySize - 1
'                    .Item("CODE").put_Value("CODE" & i.ToString(), i)
'                    .Item("NAME").put_Value("NAME" & i.ToString(), i)
'                    .Item("KINGAKU").put_Value(i * 1000, i)
'                Next
'            End With
'            .UsePLSQLAssociativeArray = True
'            .ExecuteSQL("BEGIN PKG_MASTER.ARRAY_INSERT(:CODE, :NAME, :KINGAKU, :SEQ); END;")
'            .UsePLSQLAssociativeArray = False

'            With .ParamArrays
'                For i As Integer = 0 To nArraySize - 1
'                    Console.WriteLine("SEQ=" & .Item("SEQ").get_Value(i).ToString())
'                Next
'                .Clear()
'            End With
'            .CommitTrans()
'        End With
'    End Sub
'End Class

'Class TestExclusion
'    Inherits TestBase

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Title = "排他エラー"

'        DB.BeginTrans()
'        Dim rec As OraDynaset = DB.CreateDynaset("SELECT * FROM TBL_MASTER FOR UPDATE", ORADYN_READONLY)

'        Using db2 As OraDatabase = DB.Session.OpenDatabase(HostName, ConnectionString, ORADB_DEFAULT)
'            db2.BeginTrans()
'            Try
'                Console.WriteLine("FOR UPDATE NOWAIT 実行")
'                Dim rec2 As OraDynaset = db2.CreateDynaset("SELECT * FROM TBL_MASTER FOR UPDATE NOWAIT", ORADYN_READONLY)
'                Check(False, "排他エラー")

'            Catch ex As Exception
'                Check(True, "排他エラー")
'                Console.WriteLine("ex.Message = " & ex.Message)
'                Console.WriteLine("Database.LastServerErr = " & db2.LastServerErr)
'                Console.WriteLine("Database.LastServerErrText = " & db2.LastServerErrText)
'                Console.WriteLine("Database.LastServerErrPos = " & db2.LastServerErrPos)
'            End Try
'            Try
'                Console.WriteLine("FOR UPDATE WAIT 実行")
'                Dim rec2 As OraDynaset = db2.CreateDynaset("SELECT * FROM TBL_MASTER FOR UPDATE WAIT 1", ORADYN_READONLY)
'                Check(False, "排他エラー")

'            Catch ex As Exception
'                Check(True, "排他エラー")
'                Console.WriteLine("ex.Message = " & ex.Message)
'                Console.WriteLine("Database.LastServerErr = " & db2.LastServerErr)
'                Console.WriteLine("Database.LastServerErrText = " & db2.LastServerErrText)
'                Console.WriteLine("Database.LastServerErrPos = " & db2.LastServerErrPos)
'            End Try
'            db2.Rollback()
'        End Using

'        DB.Rollback()
'    End Sub
'End Class

'Class TestRefCursor
'    Inherits TestBase

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)

'        Me.Title = "ORATYPE_CURSOR"
'        Dim rec As OraDynaset

'        With DB.Parameters
'            .Clear()
'            .Add("refcur_ret", Nothing, ORAPARM_OUTPUT, ORATYPE_CURSOR)
'            .Add("refcur_out", Nothing, ORAPARM_OUTPUT, ORATYPE_CURSOR)
'        End With

'        Dim sql As New StringBuilder

'        sql.AppendLine("BEGIN")
'        sql.AppendLine(" :refcur_ret := MyFunc(:refcur_out);")
'        sql.AppendLine("END;")

'        Console.WriteLine(sql.ToString())

'        DB.ExecuteSQL(sql.ToString)

'        rec = DirectCast(DB.Parameters("refcur_ret").Value, OraDynaset)
'        Do Until rec.EOF
'            rec.MoveNext()
'        Loop

'        rec = DirectCast(DB.Parameters("refcur_out").Value, OraDynaset)
'        Do Until rec.EOF
'            rec.MoveNext()
'        Loop

'    End Sub

'End Class

Class TestNoChacheDynaset
    Inherits TestBase

    Sub SubTest(ByVal db As OraDatabase, ByVal options As dynOption)
        Console.WriteLine(options.ToString)
        Using dyn As OraDynaset = db.CreateDynaset("SELECT * FROM TBL_MASTER ORDER BY SEQ", options)
            Dim seq As Integer = CInt(dyn.Fields("SEQ").Value)

            Do Until dyn.EOF
                Console.Write(vbCr & "SEQ = " & dyn.Fields("SEQ").Value)
                If seq <> CInt(dyn.Fields("SEQ").Value) Then
                    Check(False, String.Format("{0}, {1}", seq, dyn.Fields("SEQ").Value))
                End If
                seq = seq + 1
                dyn.MoveNext()
            Loop
        End Using
        Console.WriteLine("")
    End Sub

    Protected Overrides Sub OnTest(ByVal DB As OracleInProcServer.OraDatabase)
        Title = "NOCHACHE 動作確認"
        TBL_MASTER.Create(DB, 1000)
        SubTest(DB, ORADYN_NOCACHE)
    End Sub

End Class

Class TestParameterNULL
    Inherits TestBase

    Private m_SEQ As Integer = 0

    Private ReadOnly Property nSEQ As Integer
        Get
            m_SEQ = m_SEQ + 1
            Return m_SEQ
        End Get
    End Property


    Protected Overrides Sub OnTest(ByVal DB As OracleInProcServer.OraDatabase)
        Title = "Parameter 動作確認"

        Dim nCount As Integer = 2
        Dim nRet As Integer
        With DB.Parameters

            ' 数値型に何もセットしない

            DB.ExecuteSQL("DELETE FROM TBL_MASTER")

            .Clear()
            .AddTable("SEQ", ORAPARM_INPUT, ORATYPE_NUMBER, nCount)
            .AddTable("CODE", ORAPARM_INPUT, ORATYPE_VARCHAR2, nCount)
            .AddTable("NAME", ORAPARM_INPUT, ORATYPE_VARCHAR2, nCount)
            .AddTable("KINGAKU", ORAPARM_INPUT, ORATYPE_NUMBER, nCount)
            .AddTable("HIDUKE", ORAPARM_INPUT, ORATYPE_DATE, nCount)

            For i As Integer = 0 To nCount - 1
                .Item("SEQ").put_Value(nSEQ, i)
                .Item("CODE").put_Value(String.Format("CODE{0:0000}", i), i)
                .Item("NAME").put_Value(String.Format("NAME{0:0000}", i), i)
                .Item("HIDUKE").put_Value(Now.ToString(), i)
            Next
            nRet = DB.ExecuteSQL("INSERT INTO TBL_MASTER(SEQ, CODE, NAME, KINGAKU, HIDUKE) VALUES (:SEQ, :CODE, :NAME, :KINGAKU, :HIDUKE)")

            .Clear()
            .Add("SEQ", Nothing, ORAPARM_INPUT, ORATYPE_NUMBER)
            .Add("CODE", Nothing, ORAPARM_INPUT, ORATYPE_VARCHAR2)
            .Add("NAME", Nothing, ORAPARM_INPUT, ORATYPE_VARCHAR2)
            .Add("KINGAKU", Nothing, ORAPARM_INPUT, ORATYPE_NUMBER)
            .Add("HIDUKE", Nothing, ORAPARM_INPUT, ORATYPE_DATE)

            .Item("SEQ").Value = nSEQ
            .Item("CODE").Value = "CODE"
            .Item("NAME").Value = "NAME"
            .Item("HIDUKE").Value = Nothing
            nRet = DB.ExecuteSQL("INSERT INTO TBL_MASTER(SEQ, CODE, NAME, KINGAKU, HIDUKE) VALUES (:SEQ, :CODE, :NAME, :KINGAKU, :HIDUKE)")


            ' 数値型に Nothing をセット

            .Clear()
            .AddTable("SEQ", ORAPARM_INPUT, ORATYPE_NUMBER, nCount)
            .AddTable("CODE", ORAPARM_INPUT, ORATYPE_VARCHAR2, nCount)
            .AddTable("NAME", ORAPARM_INPUT, ORATYPE_VARCHAR2, nCount)
            .AddTable("KINGAKU", ORAPARM_INPUT, ORATYPE_NUMBER, nCount)
            .AddTable("HIDUKE", ORAPARM_INPUT, ORATYPE_DATE, nCount)

            For i As Integer = 0 To nCount - 1
                .Item("SEQ").put_Value(nSEQ, i)
                .Item("CODE").put_Value(String.Format("CODE{0:0000}", i), i)
                .Item("NAME").put_Value(String.Format("NAME{0:0000}", i), i)
                .Item("KINGAKU").put_Value(Nothing, i)
                .Item("HIDUKE").put_Value(Now.ToString(), i)
            Next
            nRet = DB.ExecuteSQL("INSERT INTO TBL_MASTER(SEQ, CODE, NAME, KINGAKU, HIDUKE) VALUES (:SEQ, :CODE, :NAME, :KINGAKU, :HIDUKE)")

            .Clear()
            .Add("SEQ", Nothing, ORAPARM_INPUT, ORATYPE_NUMBER)
            .Add("CODE", Nothing, ORAPARM_INPUT, ORATYPE_VARCHAR2)
            .Add("NAME", Nothing, ORAPARM_INPUT, ORATYPE_VARCHAR2)
            .Add("KINGAKU", Nothing, ORAPARM_INPUT, ORATYPE_NUMBER)
            .Add("HIDUKE", Nothing, ORAPARM_INPUT, ORATYPE_DATE)

            .Item("SEQ").Value = nSEQ
            .Item("CODE").Value = "CODE"
            .Item("NAME").Value = "NAME"
            .Item("KINGAKU").Value = Nothing
            .Item("HIDUKE").Value = Nothing
            nRet = DB.ExecuteSQL("INSERT INTO TBL_MASTER(SEQ, CODE, NAME, KINGAKU, HIDUKE) VALUES (:SEQ, :CODE, :NAME, :KINGAKU, :HIDUKE)")

            Using rec As OraDynaset = DB.CreateDynaset("SELECT KINGAKU FROM TBL_MASTER", ORADYN_READONLY)
                Do Until rec.EOF
                    Check(rec.Fields("KINGAKU").Value.IsNull, "数値型に NULL が落ちていない")
                    rec.MoveNext()
                Loop
            End Using
        End With

    End Sub

End Class

Class TestDuplicateField2
    Inherits TestBase
    Protected Overrides Sub OnTest(DB As OraDatabase)
        Title = "重複フィールドのテスト"
        Dim sql As New StringBuilder
        sql.AppendLine("SELECT")
        sql.AppendLine("    CODE")
        sql.AppendLine(",   NAME  CODE")
        sql.AppendLine("FROM")
        sql.AppendLine("    TBL_MASTER")

        Dim rec As OraDynaset = DB.CreateDynaset(sql.ToString, ORADYN_DEFAULT)
        Check(rec("CODE").Value.ToString() = rec(1).Value.ToString(), "VALUE=" & rec("CODE").Value)

    End Sub
End Class

Class TestSpaceParameter
    Inherits TestBase
    Protected Overrides Sub OnTest(DB As OraDatabase)
        Title = "スペースの登録"

        DB.Parameters.Clear()
        DB.Parameters.Add("CODE", " ", ORAPARM_INPUT, ORATYPE_VARCHAR2)
        DB.Parameters.Add("NAME", "　", ORAPARM_INPUT, ORATYPE_VARCHAR2)
        DB.Parameters.Add("KINGAKU", 1.5, ORAPARM_INPUT, ORATYPE_NUMBER)
        DB.Parameters.Add("HIDUKE", Now, ORAPARM_INPUT, ORATYPE_DATE)
        DB.Parameters.Add("HIDUKE2", Now.ToString, ORAPARM_INPUT, ORATYPE_DATE)

        DB.ExecuteSQL("DELETE FROM TBL_MASTER")
        DB.ExecuteSQL("INSERT INTO TBL_MASTER(SEQ,CODE,NAME,KINGAKU,HIDUKE,HIDUKE2) VALUES (SEQ_SEQ.NEXTVAL,:CODE,:NAME,:KINGAKU,:HIDUKE,:HIDUKE2)")

        Using rec As OraDynaset = DB.CreateDynaset("SELECT * FROM TBL_MASTER", ORADYN_NO_BLANKSTRIP)
            Check(rec("CODE").Value = " ", "OraParameter CODE=半角スペース")
            Check(rec("NAME").Value = "　", "OraParameter NAME=全角スペース")
        End Using

        With DB.ParamArrays
            .Clear()
            .AddTable("SEQ", ORAPARM_INPUT, ORATYPE_NUMBER, 1)
            .AddTable("CODE", ORAPARM_INPUT, ORATYPE_VARCHAR2, 1, 10)
            .AddTable("NAME", ORAPARM_INPUT, ORATYPE_VARCHAR2, 1, 10)
            .AddTable("KINGAKU", ORAPARM_INPUT, ORATYPE_NUMBER, 1)
            .AddTable("hoge", ORAPARM_INPUT, ORATYPE_NUMBER, 1)
            .AddTable("HIDUKE", ORAPARM_INPUT, ORATYPE_DATE, 1)

            For i As Integer = 0 To 0
                .Item("SEQ").put_Value(i + 1, i)
                .Item("CODE").put_Value(" ", i)
                .Item("NAME").put_Value("　", i)
                .Item("KINGAKU").put_Value(i * 1000, i)
                .Item("HIDUKE").put_Value(Now.ToString(), i)
            Next
        End With

        DB.ExecuteSQL("DELETE FROM TBL_MASTER")
        DB.ExecuteSQL("INSERT INTO TBL_MASTER(SEQ, CODE, NAME, KINGAKU, HIDUKE) VALUES (:SEQ, :CODE, :NAME, :KINGAKU, :HIDUKE)")

        Using rec As OraDynaset = DB.CreateDynaset("SELECT * FROM TBL_MASTER", ORADYN_NO_BLANKSTRIP)
            Check(rec("CODE").Value = " ", "OraParamArray CODE=半角スペース")
            Check(rec("NAME").Value = "　", "OraParamArray NAME=全角スペース")
        End Using


    End Sub
End Class

'Class TestDisconnect
'    Inherits TestBase

'    Sub ShowCollection(ByVal DB As OraDatabase)

'        Console.WriteLine("残っている Dynaset")
'        For i As Integer = 0 To DB.Dynasets.Count - 1
'            Console.WriteLine(DB.Dynasets.GetWeakReference(i).Name)
'        Next
'        Console.WriteLine("残っている SqlStmts")
'        For i As Integer = 0 To DB.SqlStmts.Count - 1
'            Console.WriteLine(DB.SqlStmts.GetWeakReference(i).Name)
'        Next
'    End Sub

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)

'        Dim Con As OraSession = DB.Session

'        ShowCollection(DB)

'        Console.WriteLine("OraDatabase.ConnectionOK = {0}", DB.ConnectionOK)
'        Console.WriteLine("Connection Count = {0}", Con.Connections.Count)
'        Console.WriteLine("何かキーを押すと、OraDatabase を Dispose() します。")
'        GC.Collect()
'        Console.ReadKey()

'        ShowCollection(DB)

'        DB.Parameters.Clear()
'        DB.Parameters.Add("HOGE", "HOGE", ORAPARM_INPUT, ORATYPE_VARCHAR2)
'        Using rec As OraDynaset = DB.CreateDynaset("SELECT 1 CNT FROM TBL_MASTER", ORADYN_NO_AUTOBIND)
'            GC.Collect()
'            DB.Close()

'            Console.WriteLine("OraDatabase.ConnectionOK = {0}", DB.ConnectionOK)
'            Console.WriteLine("Connection Count = {0}", Con.Connections.Count)

'            ' ダイナセットを開いていても接続が閉じているのを確認
'            WriteLineColor("接続が閉じているのを確認してください。", ConsoleColor.DarkYellow)
'            Console.WriteLine("何かキーを押すと、OraSession を Dispose() します。")
'            Console.ReadKey()
'            Con.Dispose()

'        End Using

'        Dim Client As IDisposable = DirectCast(Con.Client, IDisposable)
'        Console.WriteLine("何かキーを押すと、OraClient.Dispose() を実行します。")
'        Console.ReadKey()
'        Client.Dispose()

'        Console.WriteLine("何かキーを押すと、GC.Collect を実行します。")
'        Console.ReadKey()
'        GC.Collect()
'    End Sub
'End Class

'Class TestOraVariant
'    Inherits TestBase

'    Protected Overrides Sub OnTest(ByVal DB As OraDatabase)
'        Test_Compare(Nothing, Nothing, 0)
'        Test_Compare("A", "A", 0)
'        Test_Compare("A", Nothing, 1)
'        Test_Compare(Nothing, "B", -1)
'        Test_Compare("A", "B", -1)
'        Test_Compare("B", "A", 1)

'        Test_Compare(1, 1, 0)
'        Test_Compare(1, Nothing, 1)
'        Test_Compare(Nothing, 1, -1)
'        Test_Compare(0, 1, -1)
'        Test_Compare(1, 0, 1)

'        Dim small As DateTime = DateTime.Now
'        Dim big As DateTime = small + New TimeSpan(1, 0, 0, 0)

'        Test_Compare(small, small, 0)
'        Test_Compare(small, Nothing, 1)
'        Test_Compare(Nothing, small, -1)
'        Test_Compare(small, big, -1)
'        Test_Compare(big, small, 1)
'    End Sub

'    Sub Test_Compare(ByVal A As OraVariant, ByVal B As OraVariant, ByVal Compare As Integer)

'        Console.WriteLine("A={0}, B={1}", A, B)

'        Select Case Compare
'            Case 0  ' A = B のとき
'                Check(A = B, String.Format("比較 {0}={1}", A, B))
'                Check(Not (A <> B), String.Format("比較 {0}<>{1}", A, B))
'                Check(Not (A < B), String.Format("比較 {0}<{1}", A, B))
'                Check(A <= B, String.Format("比較 {0}<={1}", A, B))
'                Check(Not (A > B), String.Format("比較 {0}>{1}", A, B))
'                Check(A >= B, String.Format("比較 {0}>={1}", A, B))
'            Case 1  ' A > B のとき
'                Check(Not (A = B), String.Format("比較 {0}={1}", A, B))
'                Check(A <> B, String.Format("比較 {0}<>{1}", A, B))
'                Check(Not (A < B), String.Format("比較 {0}<{1}", A, B))
'                Check(Not (A <= B), String.Format("比較 {0}<={1}", A, B))
'                Check(A > B, String.Format("比較 {0}>{1}", A, B))
'                Check(A >= B, String.Format("比較 {0}>={1}", A, B))
'            Case -1  ' A < B のとき
'                Check(Not (A = B), String.Format("比較 {0}={1}", A, B))
'                Check(A <> B, String.Format("比較 {0}<>{1}", A, B))
'                Check((A < B), String.Format("比較 {0}<{1}", A, B))
'                Check((A <= B), String.Format("比較 {0}<={1}", A, B))
'                Check(Not (A > B), String.Format("比較 {0}>{1}", A, B))
'                Check(Not (A >= B), String.Format("比較 {0}>={1}", A, B))
'        End Select

'    End Sub

'End Class
