
Option Strict On
Option Infer On
Imports Npgsql
Imports System.Configuration
Public Module Conexiones
    Private Function GetOpenConnection() As NpgsqlConnection
        Dim cs = ConfigurationManager.ConnectionStrings("PgConnection").ConnectionString
        Dim cn As New NpgsqlConnection(cs)
        cn.Open()
        Return cn
    End Function

    ' SELECT con o sin parámetros
    Public Function Consulta(sql As String, Optional pars As IEnumerable(Of NpgsqlParameter) = Nothing) As DataTable
        Using cn = GetOpenConnection()
            Using cmd As New NpgsqlCommand(sql, cn)
                If pars IsNot Nothing Then cmd.Parameters.AddRange(pars.ToArray())
                Using da As New NpgsqlDataAdapter(cmd)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    Return dt
                End Using
            End Using
        End Using
    End Function


    Public Function Ejecutarsql(sql As String, Optional pars As IEnumerable(Of NpgsqlParameter) = Nothing) As Boolean
        Using cn = GetOpenConnection()
            Using cmd As New NpgsqlCommand(sql, cn)
                If pars IsNot Nothing Then cmd.Parameters.AddRange(pars.ToArray())
                Dim aff = cmd.ExecuteNonQuery()
                Return aff > 0
            End Using
        End Using
    End Function


    Public Function EjecutarsqlScalar(sql As String, Optional pars As IEnumerable(Of NpgsqlParameter) = Nothing) As Object
        Using cn = GetOpenConnection()
            Using cmd As New NpgsqlCommand(sql, cn)
                If pars IsNot Nothing Then cmd.Parameters.AddRange(pars.ToArray())
                Return cmd.ExecuteScalar()
            End Using
        End Using
    End Function
End Module
