Option Strict On
Option Infer On
Imports Npgsql

Public Module UtilsSeguridad
    Public Function TienePgcrypto() As Boolean
        Try
            Dim dt = Conexiones.Consulta(
                "SELECT 1 FROM pg_extension WHERE extname='pgcrypto' LIMIT 1;")
            Return dt.Rows.Count > 0
        Catch
            Return False
        End Try
    End Function
End Module
