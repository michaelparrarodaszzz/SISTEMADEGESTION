Option Strict On
Option Infer On
Imports Npgsql
Imports System.Text.RegularExpressions

Public Class FrmCliente

    Private Sub FrmCliente_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PrepararGrid()
        CargarCiudades()
        CargarEstados()
        CargarListado()
        DeshabilitarInputs()
        lblStatus.Text = "Listo."

        AddHandler txtNombre.TextChanged, Sub() ValNombre()
        AddHandler cboCiudad.SelectedIndexChanged, Sub() ValCiudad()
        AddHandler txtEmail.Leave, Sub() ValEmail()
        AddHandler txtTelefono.Leave, Sub() ValTelefono()
    End Sub

    ' ================== Grid ==================
    Private Sub PrepararGrid()
        With DataGridView1
            .AutoGenerateColumns = False
            .ReadOnly = True
            .MultiSelect = False
            .AllowUserToAddRows = False
            .RowHeadersVisible = False
            .BorderStyle = BorderStyle.None
        End With
    End Sub

    ' ================== Data ==================
    Private Sub CargarCiudades()
        Dim dt = Conexiones.Consulta("
            SELECT c.ciu_id, c.ciu_nombre
            FROM ciudad c
            ORDER BY c.ciu_nombre;")
        cboCiudad.DataSource = dt
        cboCiudad.ValueMember = "ciu_id"
        cboCiudad.DisplayMember = "ciu_nombre"
        cboCiudad.SelectedIndex = -1
    End Sub

    Private Sub CargarEstados()
        cboEstado.Items.Clear()
        cboEstado.Items.AddRange(New Object() {"ACTIVO", "INACTIVO"})
        cboEstado.SelectedIndex = 0
    End Sub

    Private Sub CargarListado()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT cl.cli_id,
                       cl.cli_nombre,
                       cl.cli_apellido,
                       cl.ciu_id,
                       ciu.ciu_nombre,
                       cl.cli_ruc,
                       cl.cli_ci,
                       cl.cli_direccion,
                       cl.cli_telefono,
                       cl.cli_email,
                       cl.cli_estado
                FROM cliente cl
                JOIN ciudad ciu ON ciu.ciu_id = cl.ciu_id
                ORDER BY cl.cli_id;")

            DataGridView1.DataSource = dt

            If DataGridView1.Rows.Count > 0 Then
                Dim idx As Integer = If(DataGridView1.CurrentRow IsNot Nothing, DataGridView1.CurrentRow.Index, 0)
                If idx < 0 OrElse idx >= DataGridView1.Rows.Count Then idx = 0
                CargarFila(idx)
            Else
                LimpiarInputs() : DeshabilitarInputs() : BtnNuevo.Enabled = True
            End If
        Catch ex As Exception
            MostrarError("Error al consultar clientes", ex)
        End Try
    End Sub

    ' ================== Estados UI ==================
    Private Sub DeshabilitarInputs()
        For Each tb In New TextBox() {txtId, txtNombre, txtApellido, txtRuc, txtCi, txtTelefono, txtEmail}
            tb.ReadOnly = True
        Next
        txtDireccion.ReadOnly = True
        cboCiudad.Enabled = False
        cboEstado.Enabled = False

        BtnGuardar.Enabled = False : BtnCancelar.Enabled = False
        BtnEditar.Enabled = False : BtnEliminar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        For Each tb In New TextBox() {txtNombre, txtApellido, txtRuc, txtCi, txtTelefono, txtEmail}
            tb.ReadOnly = False
        Next
        txtDireccion.ReadOnly = False
        cboCiudad.Enabled = True
        cboEstado.Enabled = True

        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False

        If Not paraEdicion Then
            txtId.Clear() : txtNombre.Clear() : txtApellido.Clear() : txtRuc.Clear()
            txtCi.Clear() : txtTelefono.Clear() : txtEmail.Clear() : txtDireccion.Clear()
            If cboCiudad.Items.Count > 0 Then cboCiudad.SelectedIndex = -1
            cboEstado.SelectedIndex = 0
            ep.Clear()
            txtNombre.Focus()
        End If
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear() : txtNombre.Clear() : txtApellido.Clear() : txtRuc.Clear()
        txtCi.Clear() : txtTelefono.Clear() : txtEmail.Clear() : txtDireccion.Clear()
        If cboCiudad.Items.Count > 0 Then cboCiudad.SelectedIndex = -1
        cboEstado.SelectedIndex = 0
        ep.Clear()
    End Sub

    ' ================== Grid -> Form ==================
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellClick, DataGridView1.CellContentClick
        If e.RowIndex >= 0 Then CargarFila(e.RowIndex)
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs) Handles DataGridView1.SelectionChanged
        If DataGridView1.CurrentRow IsNot Nothing AndAlso BtnGuardar.Enabled = False Then
            CargarFila(DataGridView1.CurrentRow.Index)
        End If
    End Sub

    Private Sub CargarFila(idx As Integer)
        If idx < 0 OrElse idx >= DataGridView1.Rows.Count Then Return
        Dim row = DataGridView1.Rows(idx)

        txtId.Text = Convert.ToString(TryGetCell(row, {"colId", "cli_id"}))
        txtNombre.Text = Convert.ToString(TryGetCell(row, {"colNombre", "cli_nombre"}))
        txtApellido.Text = Convert.ToString(TryGetCell(row, {"colApellido", "cli_apellido"}))
        txtRuc.Text = Convert.ToString(TryGetCell(row, {"colRuc", "cli_ruc"}))
        txtCi.Text = Convert.ToString(TryGetCell(row, {"colCi", "cli_ci"}))
        txtTelefono.Text = Convert.ToString(TryGetCell(row, {"colTelefono", "cli_telefono"}))
        txtEmail.Text = Convert.ToString(TryGetCell(row, {"colEmail", "cli_email"}))
        txtDireccion.Text = Convert.ToString(TryGetCell(row, {"colDireccion", "cli_direccion"}))
        Dim vCiudadId = TryGetCell(row, {"colCiudadId", "ciu_id"})
        Dim vCiudadNom = TryGetCell(row, {"colCiudad", "ciu_nombre"})
        Dim vEstado = TryGetCell(row, {"colEstado", "cli_estado"})

        If vCiudadId IsNot Nothing AndAlso vCiudadId IsNot DBNull.Value Then
            cboCiudad.SelectedValue = CInt(vCiudadId)
        Else
            SeleccionarCiudadPorNombre(Convert.ToString(vCiudadNom))
        End If

        If vEstado IsNot Nothing AndAlso vEstado IsNot DBNull.Value Then
            Dim s = Convert.ToString(vEstado).ToUpperInvariant()
            cboEstado.SelectedItem = If(s = "INACTIVO", "INACTIVO", "ACTIVO")
        Else
            cboEstado.SelectedIndex = 0
        End If

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

    Private Sub SeleccionarCiudadPorNombre(nombre As String)
        If String.IsNullOrWhiteSpace(nombre) Then cboCiudad.SelectedIndex = -1 : Return
        For i = 0 To cboCiudad.Items.Count - 1
            Dim drv = TryCast(cboCiudad.Items(i), DataRowView)
            If drv IsNot Nothing AndAlso
               String.Equals(Convert.ToString(drv("ciu_nombre")), nombre, StringComparison.OrdinalIgnoreCase) Then
                cboCiudad.SelectedIndex = i : Exit For
            End If
        Next
    End Sub

    ' ================== Botones ==================
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False)
        lblStatus.Text = "Modo: Nuevo"
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        HabilitarInputs(True)
        lblStatus.Text = "Modo: Editar"
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValNombre() AndAlso ValCiudad() AndAlso ValEmail() AndAlso ValTelefono()) Then
                lblStatus.Text = "Corrija los campos marcados." : Return
            End If

            Dim pars As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@nom", txtNombre.Text.Trim()),
                New NpgsqlParameter("@ape", If(String.IsNullOrWhiteSpace(txtApellido.Text), CType(DBNull.Value, Object), txtApellido.Text.Trim())),
                New NpgsqlParameter("@ruc", If(String.IsNullOrWhiteSpace(txtRuc.Text), CType(DBNull.Value, Object), txtRuc.Text.Trim())),
                New NpgsqlParameter("@ci", If(String.IsNullOrWhiteSpace(txtCi.Text), CType(DBNull.Value, Object), txtCi.Text.Trim())),
                New NpgsqlParameter("@tel", If(String.IsNullOrWhiteSpace(txtTelefono.Text), CType(DBNull.Value, Object), txtTelefono.Text.Trim())),
                New NpgsqlParameter("@mail", If(String.IsNullOrWhiteSpace(txtEmail.Text), CType(DBNull.Value, Object), txtEmail.Text.Trim())),
                New NpgsqlParameter("@dir", If(String.IsNullOrWhiteSpace(txtDireccion.Text), CType(DBNull.Value, Object), txtDireccion.Text.Trim())),
                New NpgsqlParameter("@estado", cboEstado.SelectedItem.ToString()),
                New NpgsqlParameter("@ciu", CInt(cboCiudad.SelectedValue))
            }

            If txtId.TextLength = 0 Then
                Dim sql = "
                    INSERT INTO cliente (ciu_id, cli_nombre, cli_apellido, cli_ruc, cli_ci,
                                         cli_direccion, cli_telefono, cli_email, cli_estado)
                    VALUES (@ciu, @nom, @ape, @ruc, @ci, @dir, @tel, @mail, @estado)
                    RETURNING cli_id;"
                Dim newId = Conexiones.EjecutarsqlScalar(sql, pars)
                txtId.Text = Convert.ToString(newId)
                lblStatus.Text = $"Insertado ID {txtId.Text}."
            Else
                pars.Add(New NpgsqlParameter("@id", Integer.Parse(txtId.Text)))
                Dim sql = "
                    UPDATE cliente
                    SET ciu_id=@ciu, cli_nombre=@nom, cli_apellido=@ape, cli_ruc=@ruc, cli_ci=@ci,
                        cli_direccion=@dir, cli_telefono=@tel, cli_email=@mail, cli_estado=@estado
                    WHERE cli_id=@id;"
                Dim ok = Conexiones.Ejecutarsql(sql, pars)
                lblStatus.Text = If(ok, "Actualizado.", "Sin cambios.")
            End If

            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23503"
            MostrarError("Ciudad inválida (FK)", ex)
        Catch ex As PostgresException When ex.SqlState = "23505"
            MostrarError("Posible duplicado (revisa RUC/CI si hay índice único)", ex)
        Catch ex As Exception
            MostrarError("Error al guardar cliente", ex)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        If MessageBox.Show("¿Eliminar el cliente?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("DELETE FROM cliente WHERE cli_id=@id;",
                    New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", Integer.Parse(txtId.Text))})
                If ok Then
                    CargarListado() : DeshabilitarInputs() : lblStatus.Text = "Eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MessageBox.Show("No se puede eliminar: el cliente está referenciado.",
                                "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Catch ex As Exception
                MostrarError("Error al eliminar", ex)
            End Try
        End If
    End Sub

    ' ================== Validaciones ==================
    Private Function ValNombre() As Boolean
        Dim v = txtNombre.Text.Trim()
        If v.Length = 0 Then ep.SetError(txtNombre, "Obligatorio.") : Return False
        If v.Length > 45 Then ep.SetError(txtNombre, "Máx. 45 caracteres.") : Return False
        ep.SetError(txtNombre, "") : Return True
    End Function

    Private Function ValCiudad() As Boolean
        If cboCiudad.SelectedIndex < 0 Then ep.SetError(cboCiudad, "Seleccione ciudad.") : Return False
        ep.SetError(cboCiudad, "") : Return True
    End Function

    Private Function ValEmail() As Boolean
        Dim v = txtEmail.Text.Trim()
        If v = "" Then ep.SetError(txtEmail, "") : Return True
        Dim ok = Regex.IsMatch(v, "^[^@\s]+@[^@\s]+\.[^@\s]+$")
        If Not ok Then ep.SetError(txtEmail, "Formato inválido.") : Return False
        ep.SetError(txtEmail, "") : Return True
    End Function

    Private Function ValTelefono() As Boolean
        Dim v = txtTelefono.Text.Trim()
        If v = "" Then ep.SetError(txtTelefono, "") : Return True
        Dim ok = Regex.IsMatch(v, "^[0-9+\-\s]{6,20}$")
        If Not ok Then ep.SetError(txtTelefono, "Use números, + o - (6-20).") : Return False
        ep.SetError(txtTelefono, "") : Return True
    End Function

    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub
End Class
