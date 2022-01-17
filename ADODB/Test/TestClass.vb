Option Strict On
Option Explicit On

Imports System.Text
Imports System.Threading
Imports ADODB

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

    Public Sub DoTest(ByVal con As Connection)
        Dim dtStart As DateTime = Now
        Try
            con.ResetTrans()
            OnTest(con)
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

    Protected MustOverride Sub OnTest(ByVal con As Connection)

End Class

Class TBL_MASTER
    Inherits TestBase
    Shared m_Count As Integer

    Private Sub New()

    End Sub

    Public Shared Sub Create(ByVal con As Connection, ByVal Count As Integer)
        m_Count = Count
        Dim t As New TBL_MASTER
        t.DoTest(con)
    End Sub

    Protected Overrides Sub OnTest(ByVal con As Connection)
        Title = String.Format("テストデータ作成({0}件)", m_Count)

        With con
            .ResetTrans()
            .BeginTrans()

            Using cmd As New ADODB.Command()
                cmd.ActiveConnection = con
                cmd.CommandText = "DELETE FROM TBL_MASTER"
                cmd.Execute()
            End Using

            Using cmd As New ADODB.Command
                cmd.ActiveConnection = con
                cmd.CommandText = "INSERT INTO TBL_MASTER(SEQ, CODE, NAME, KINGAKU, HIDUKE) VALUES (:SEQ, :CODE, :NAME, :KINGAKU, :HIDUKE)"

                cmd.Parameters.Append(cmd.CreateParameter("SEQ", DataTypeEnum.adDecimal, ParameterDirectionEnum.adParamInput))
                cmd.Parameters.Append(cmd.CreateParameter("CODE", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput))
                cmd.Parameters.Append(cmd.CreateParameter("NAME", DataTypeEnum.adVarChar, ParameterDirectionEnum.adParamInput))
                cmd.Parameters.Append(cmd.CreateParameter("KINGAKU", DataTypeEnum.adDecimal, ParameterDirectionEnum.adParamInput))
                cmd.Parameters.Append(cmd.CreateParameter("HIDUKE", DataTypeEnum.adDBDate, ParameterDirectionEnum.adParamInput))

                For i As Integer = 0 To m_Count - 1
                    cmd.Parameters("SEQ").Value = i
                    cmd.Parameters("CODE").Value = String.Format("CODE{0:0000}", i)
                    cmd.Parameters("NAME").Value = String.Format("NAME{0:0000}", i)
                    cmd.Parameters("KINGAKU").Value = i * 1000
                    cmd.Parameters("HIDUKE").Value = Now.ToString("yyyy/MM/dd")
                Next

                cmd.Execute()
            End Using
            .CommitTrans()

        End With
    End Sub


End Class
