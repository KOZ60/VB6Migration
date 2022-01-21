OracleInProcServer は oo4o 互換ライブラリです。
ODP.NET を使用し Oracle データベースに接続します。
OraClient クラスの ProviderType プロパティで、管理対象ドライバを使用するか、非管理対象ドライバを使用するかを選択します。
DbProviderFactory を使用して ODP.NET を使いますので、GAC に登録されている必要があります。

例)

Imports Managed.OracleInProcServer
Imports Managed.OracleInProcServer.dynOption
Imports System.Text

Module Module1

    Sub Main()
        OraClient.ProviderType = OracleProviderTypes.Unmanaged
        Using con As New OraDatabase()
            con.ConnectionString = "Data Source=KOZ;User Id=KOZ;Password=KOZ"
            con.Open()
            Dim sql As String = "SELECT * FROM DBA_TABLES"
            Using rec As OraDynaset = con.CreateDynaset(sql, ORADYN_READONLY)
                Do Until rec.EOF
                    Dim Buffer As New StringBuilder
                    For Each item As OraField In rec.Fields
                        Buffer.Append(",")
                        Buffer.Append(item.Value)
                    Next
                    Console.WriteLine(Buffer.ToString.Substring(1))
                    rec.MoveNext()
                Loop
            End Using
        End Using
        Console.WriteLine("何かキーを押すと終了します。")
        Console.ReadKey()
    End Sub

End Module

