Option Strict On
Option Infer On
Imports Npgsql
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports NpgsqlTypes


Public Class FrmUsuario
    Private _cargando As Boolean = False

    Private Sub FrmUsuario_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PrepararGrid()
        CargarRoles()
        CargarListado()
        DeshabilitarInputs()
        lblStatus.Text = "Listo."

        AddHandler txtNombre.TextChanged, Sub() ValNombre()
        AddHandler txtUsuario.TextChanged, Sub() ValUsuario()
        AddHandler CboRol.SelectedIndexChanged, Sub() ValRol()
    End Sub

    ' ===== Grid =====
    Private Sub PrepararGrid()
        With DataGridView1
            .AutoGenerateColumns = False
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .MultiSelect = False
            .AllowUserToAddRows = False
            .RowHeadersVisible = False
            .BorderStyle = BorderStyle.None
        End With
    End Sub

    ' ===== Data =====
    Private Sub CargarRoles()
        CboRol.Items.Clear()
        CboRol.Items.AddRange(New Object() {"ADMIN", "OPERADOR"})
        CboRol.SelectedIndex = 1 ' OPERADOR por defecto
    End Sub

    Private Sub CargarListado()
        Try
            _cargando = True
            Dim dt = Conexiones.Consulta("
                SELECT usua_id,
                       usua_nombre,
                       usua_usuario,
                       usua_rol,
                       usua_estado
                FROM usuario
                ORDER BY usua_id;")
            DataGridView1.DataSource = dt

            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = Nothing
        Catch ex As Exception
            MostrarError("Error al consultar usuarios", ex)
        Finally
            _cargando = False
        End Try
    End Sub

    ' ===== Estados UI =====
    Private Sub DeshabilitarInputs()
        txtId.ReadOnly = True
        txtNombre.ReadOnly = True
        txtUsuario.ReadOnly = True
        txtPassword.ReadOnly = True
        CboRol.Enabled = False
        chkEstado.Enabled = False

        BtnGuardar.Enabled = False : BtnCancelar.Enabled = False
        BtnEditar.Enabled = False : BtnEliminar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
        txtPassword.Text = ""
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        txtNombre.ReadOnly = False
        txtUsuario.ReadOnly = False
        CboRol.Enabled = True
        chkEstado.Enabled = True
        txtPassword.ReadOnly = False

        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False

        If Not paraEdicion Then
            LimpiarInputs()
            txtNombre.Focus()
        End If
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear()
        txtNombre.Clear()
        txtUsuario.Clear()
        txtPassword.Clear()
        CboRol.SelectedIndex = 1
        chkEstado.Checked = True
        ep.Clear()
    End Sub

    ' ===== Grid -> Form =====
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellClick, DataGridView1.CellContentClick
        If _cargando OrElse e.RowIndex < 0 Then Return
        CargarFila(e.RowIndex)
    End Sub

    Private Sub CargarFila(idx As Integer)
        If idx < 0 OrElse idx >= DataGridView1.Rows.Count Then Return
        Dim row = DataGridView1.Rows(idx)
        txtId.Text = Convert.ToString(TryGetCell(row, {"colId", "usua_id"}))
        txtNombre.Text = Convert.ToString(TryGetCell(row, {"colNombre", "usua_nombre"}))
        txtUsuario.Text = Convert.ToString(TryGetCell(row, {"colUsuario", "usua_usuario"}))
        CboRol.SelectedItem = Convert.ToString(TryGetCell(row, {"colRol", "usua_rol"}))
        Dim vActivo = TryGetCell(row, {"colEstado", "usua_estado"})
        chkEstado.Checked = If(vActivo IsNot Nothing AndAlso vActivo IsNot DBNull.Value AndAlso CBool(vActivo), True, False)

        DeshabilitarInputs()
        BtnEditar.Enabled = True : BtnEliminar.Enabled = True
        lblStatus.Text = "Registro seleccionado."
    End Sub

    Private Function TryGetCell(row As DataGridViewRow, names() As String) As Object
        For Each n In names
            If n Is Nothing Then Continue For
            If DataGridView1.Columns.Contains(n) Then
                Return row.Cells(n).Value
            End If
        Next
        Return Nothing
    End Function

    ' ===== Botones =====
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False)
        lblStatus.Text = "Modo: Nuevo"
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un usuario." : Return
        HabilitarInputs(True)
        lblStatus.Text = "Modo: Editar"
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        If txtNombre.Text.Trim() = "" Then ep.SetError(txtNombre, "Obligatorio.") : Exit Sub
        If txtUsuario.Text.Trim() = "" Then ep.SetError(txtUsuario, "Obligatorio.") : Exit Sub
        If CboRol.SelectedIndex < 0 Then ep.SetError(CboRol, "Seleccione un rol.") : Exit Sub
        ep.Clear()

        Dim parsBase = New List(Of NpgsqlParameter) From {
            New NpgsqlParameter("@nom", NpgsqlDbType.Varchar) With {.Value = txtNombre.Text.Trim()},
            New NpgsqlParameter("@usr", NpgsqlDbType.Varchar) With {.Value = txtUsuario.Text.Trim()},
            New NpgsqlParameter("@rol", NpgsqlDbType.Varchar) With {.Value = CStr(CboRol.SelectedItem)},
            New NpgsqlParameter("@est", NpgsqlDbType.Boolean) With {.Value = chkEstado.Checked}
        }

        Dim usaCrypt = UtilsSeguridad.TienePgcrypto()

        Try
            If txtId.TextLength = 0 Then
                ' ---------- INSERT ----------
                If txtPassword.TextLength = 0 Then
                    MessageBox.Show("Ingrese una clave para el nuevo usuario.", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
                Dim pars = New List(Of NpgsqlParameter)(parsBase) From {
                    New NpgsqlParameter("@pwd", NpgsqlDbType.Varchar) With {.Value = txtPassword.Text}
                }

                Dim sql As String
                If usaCrypt Then
                    sql = "
                    INSERT INTO usuario (usua_nombre, usua_usuario, usua_contrasena, usua_rol, usua_estado)
                    VALUES (@nom, @usr, crypt(@pwd, gen_salt('bf')), @rol, @est)
                    RETURNING usua_id;"
                Else
                    sql = "
                    INSERT INTO usuario (usua_nombre, usua_usuario, usua_contrasena, usua_rol, usua_estado)
                    VALUES (@nom, @usr, @pwd, @rol, @est)
                    RETURNING usua_id;"
                End If

                Dim newId = Conexiones.EjecutarsqlScalar(sql, pars)
                txtId.Text = CStr(newId)
                MessageBox.Show("Usuario creado.", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Else
                ' ---------- UPDATE ----------
                Dim sql As String
                Dim pars = New List(Of NpgsqlParameter)(parsBase) From {
                    New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtId.Text)}
                }

                If txtPassword.TextLength > 0 Then
                    ' Cambia también la clave
                    pars.Add(New NpgsqlParameter("@pwd", NpgsqlDbType.Varchar) With {.Value = txtPassword.Text})
                    If usaCrypt Then
                        sql = "
                        UPDATE usuario
                           SET usua_nombre=@nom,
                               usua_usuario=@usr,
                               usua_contrasena=crypt(@pwd, gen_salt('bf')),
                               usua_rol=@rol,
                               usua_estado=@est
                         WHERE usua_id=@id;"
                    Else
                        sql = "
                        UPDATE usuario
                           SET usua_nombre=@nom,
                               usua_usuario=@usr,
                               usua_contrasena=@pwd,
                               usua_rol=@rol,
                               usua_estado=@est
                         WHERE usua_id=@id;"
                    End If
                Else
                    ' No cambia la clave
                    sql = "
                    UPDATE usuario
                       SET usua_nombre=@nom,
                           usua_usuario=@usr,
                           usua_rol=@rol,
                           usua_estado=@est
                     WHERE usua_id=@id;"
                End If

                Dim ok = Conexiones.Ejecutarsql(sql, pars)
                MessageBox.Show(If(ok, "Usuario actualizado.", "Sin cambios."), "Usuario",
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            ' refresca tu grilla si corresponde…

        Catch ex As PostgresException When ex.SqlState = "23505" ' unique_violation
            MessageBox.Show("El usuario ya existe (login duplicado).", "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Catch ex As Exception
            MessageBox.Show("Error al guardar: " & ex.Message, "Usuario", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un usuario." : Return
        If MessageBox.Show("¿Eliminar el usuario seleccionado?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("DELETE FROM usuario WHERE usua_id=@id;",
                    New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", Integer.Parse(txtId.Text))})
                If ok Then
                    CargarListado() : DeshabilitarInputs() : LimpiarInputs() : lblStatus.Text = "Eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As Exception
                MostrarError("Error al eliminar usuario", ex)
            End Try
        End If
    End Sub

    ' ===== Validaciones =====
    Private Function ValNombre() As Boolean
        Dim v = txtNombre.Text.Trim()
        If v.Length = 0 Then ep.SetError(txtNombre, "Obligatorio.") : Return False
        If v.Length > 30 Then ep.SetError(txtNombre, "Máx. 30 caracteres.") : Return False
        ep.SetError(txtNombre, "") : Return True
    End Function

    Private Function ValUsuario() As Boolean
        Dim v = txtUsuario.Text.Trim()
        If v.Length = 0 Then ep.SetError(txtUsuario, "Obligatorio.") : Return False
        If v.Length > 20 Then ep.SetError(txtUsuario, "Máx. 20 caracteres.") : Return False
        If Not Regex.IsMatch(v, "^[a-zA-Z0-9_.-]+$") Then
            ep.SetError(txtUsuario, "Solo letras, números, _, . o -") : Return False
        End If
        ep.SetError(txtUsuario, "") : Return True
    End Function

    Private Function ValPasswordNuevo() As Boolean
        Dim v = txtPassword.Text.Trim()
        If txtId.TextLength > 0 Then Return True ' en edición es opcional
        If v.Length < 6 Then ep.SetError(txtPassword, "Mínimo 6 caracteres.") : Return False
        ep.SetError(txtPassword, "") : Return True
    End Function

    Private Function ValRol() As Boolean
        If CboRol.SelectedIndex < 0 Then ep.SetError(CboRol, "Seleccione un rol.") : Return False
        ep.SetError(CboRol, "") : Return True
    End Function

    ' ===== Utilidades =====
    Private Function Sha256Hex(input As String) As String
        Using sha As SHA256 = SHA256.Create()
            Dim bytes = Encoding.UTF8.GetBytes(input)
            Dim hash = sha.ComputeHash(bytes)
            Dim sb As New StringBuilder(hash.Length * 2)
            For Each b In hash
                sb.Append(b.ToString("x2"))
            Next
            Return sb.ToString()
        End Using
    End Function

    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub

    Private Sub txtPassword_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged
        txtPassword.UseSystemPasswordChar = True
        txtPassword.MaxLength = 6
        txtPassword.ShortcutsEnabled = False
    End Sub
End Class
