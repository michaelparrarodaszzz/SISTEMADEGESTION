' FrmLogin.vb
Option Strict On
Option Infer On
Imports Npgsql
Imports NpgsqlTypes
Public Class FrmLogin
    Private Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles BtnEntrar.Click
        lblError.Text = ""
        Dim u = txtUsuario.Text.Trim()
        Dim p = txtClave.Text

        If u = "" OrElse p = "" Then
            lblError.Text = "Complete usuario y clave."
            Exit Sub
        End If

        Try
            Dim usaCrypt = UtilsSeguridad.TienePgcrypto()

            Dim sql As String =
            "SELECT usua_id, usua_usuario, usua_nombre, usua_rol " &
            "FROM usuario " &
            "WHERE lower(usua_usuario)=lower(@u) AND usua_estado = TRUE " &
            If(usaCrypt,
               "AND usua_contrasena = crypt(@p, usua_contrasena) ",
               "AND usua_contrasena = @p ") &
            "LIMIT 1;"

            Dim dt = Conexiones.Consulta(sql,
            New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@u", u),
                New NpgsqlParameter("@p", p)
            })

            If dt.Rows.Count = 1 Then
                AppSession.CurrentUser = New AppSession.UserInfo With {
                .Id = CInt(dt.Rows(0)("usua_id")),
                .Username = CStr(dt.Rows(0)("usua_usuario")),
                .Nombre = CStr(dt.Rows(0)("usua_nombre")),
                .Rol = CStr(dt.Rows(0)("usua_rol"))
            }
                Me.DialogResult = DialogResult.OK
                Exit Sub
            End If

            ' Diagnóstico si falla
            Dim chk = Conexiones.Consulta(
            "SELECT usua_estado FROM usuario WHERE lower(usua_usuario)=lower(@u) LIMIT 1;",
            New List(Of NpgsqlParameter) From {New NpgsqlParameter("@u", u)})

            If chk.Rows.Count = 0 Then
                lblError.Text = "El usuario no existe (o la app apunta a otra base)."
            ElseIf Not CBool(chk.Rows(0)("usua_estado")) Then
                lblError.Text = "El usuario está deshabilitado."
            Else
                lblError.Text = "Clave incorrecta."
            End If

        Catch ex As Exception
            MessageBox.Show("Error al autenticar: " & ex.Message, "Login",
                        MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        Me.DialogResult = DialogResult.Cancel
    End Sub

    Private Sub txtClave_TextChanged(sender As Object, e As EventArgs) Handles txtClave.TextChanged
        txtClave.UseSystemPasswordChar = True
        txtClave.MaxLength = 6
        txtClave.ShortcutsEnabled = False
    End Sub
End Class
