OracleInProcServer �� oo4o �݊����C�u�����ł��B
ODP.NET ���g�p�� Oracle �f�[�^�x�[�X�ɐڑ����܂��B
OraClient �N���X�� ProviderType �v���p�e�B�ŁA�Ǘ��Ώۃh���C�o���g�p���邩�A��Ǘ��Ώۃh���C�o���g�p���邩��I�����܂��B
DbProviderFactory ���g�p���� ODP.NET ���g���܂��̂ŁAGAC �ɓo�^����Ă���K�v������܂��B

��)

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
        Console.WriteLine("�����L�[�������ƏI�����܂��B")
        Console.ReadKey()
    End Sub

End Module

