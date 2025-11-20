Option Strict On
Option Infer On
Imports Npgsql
Imports System.Text.RegularExpressions

Public Class FrmProveedor

    Private Sub FrmProveedor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PrepararGrid()
        CargarCiudades()
        CargarEstados()
        CargarListado()
        DeshabilitarInputs()
        LimpiarInputs()
        lblStatus.Text = "Listo."

        AddHandler txtNombre.TextChanged, Sub() ValNombre()
        AddHandler cboCiudad.SelectedIndexChanged, Sub() ValCiudad()
        AddHandler txtEmail.Leave, Sub() ValEmail()
        AddHandler txtTelefono.Leave, Sub() ValTelefono()
    End Sub

    ' =========== Grid ===========
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

    ' =========== Data ===========
    Private Sub CargarCiudades()
        Dim dt = Conexiones.Consulta("
            SELECT ciu_id, ciu_nombre
            FROM ciudad
            ORDER BY ciu_nombre;")
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
                SELECT p.prov_id,
                       p.prov_nombre,
                       p.ciu_id,
                       c.ciu_nombre,
                       p.prov_ruc,
                       p.prov_direccion,
                       p.prov_telefono,
                       p.prov_email,
                       p.prov_estado
                FROM proveedor p
                JOIN ciudad c ON c.ciu_id = p.ciu_id
                ORDER BY p.prov_id;")
            DataGridView1.DataSource = dt

            ' Opcional: ocultar la FK si la tenés como columna
            If DataGridView1.Columns.Contains("ciu_id") Then DataGridView1.Columns("ciu_id").Visible = False
            If DataGridView1.Columns.Contains("colCiudadId") Then DataGridView1.Columns("colCiudadId").Visible = False

            If DataGridView1.Rows.Count > 0 Then
                Dim idx = If(DataGridView1.CurrentRow IsNot Nothing, DataGridView1.CurrentRow.Index, 0)
                If idx < 0 OrElse idx >= DataGridView1.Rows.Count Then idx = 0
                CargarFila(idx)
            Else
                LimpiarInputs() : DeshabilitarInputs() : BtnNuevo.Enabled = True
            End If
        Catch ex As Exception
            MostrarError("Error al consultar proveedores", ex)
        End Try
    End Sub

    ' =========== Estados UI ===========
    Private Sub DeshabilitarInputs()
        For Each tb In New TextBox() {txtId, txtNombre, txtRuc, txtTelefono, txtEmail}
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
        For Each tb In New TextBox() {txtNombre, txtRuc, txtTelefono, txtEmail}
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
            txtId.Clear() : txtNombre.Clear() : txtRuc.Clear()
            txtTelefono.Clear() : txtEmail.Clear() : txtDireccion.Clear()
            If cboCiudad.Items.Count > 0 Then cboCiudad.SelectedIndex = -1
            cboEstado.SelectedIndex = 0
            ep.Clear()
            txtNombre.Focus()
        End If
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear() : txtNombre.Clear() : txtRuc.Clear()
        txtTelefono.Clear() : txtEmail.Clear() : txtDireccion.Clear()
        If cboCiudad.Items.Count > 0 Then cboCiudad.SelectedIndex = -1
        cboEstado.SelectedIndex = 0
        ep.Clear()
    End Sub

    ' =========== Grid -> Form ===========
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

        txtId.Text = Convert.ToString(TryGetCell(row, {"colId", "prov_id"}))
        txtNombre.Text = Convert.ToString(TryGetCell(row, {"colNombre", "prov_nombre"}))
        txtRuc.Text = Convert.ToString(TryGetCell(row, {"colRuc", "prov_ruc"}))
        txtTelefono.Text = Convert.ToString(TryGetCell(row, {"colTelefono", "prov_telefono"}))
        txtEmail.Text = Convert.ToString(TryGetCell(row, {"colEmail", "prov_email"}))
        txtDireccion.Text = Convert.ToString(TryGetCell(row, {"colDireccion", "prov_direccion"}))

        Dim vCiudadId = TryGetCell(row, {"colCiudadId", "ciu_id"})
        Dim vCiudadNom = TryGetCell(row, {"colCiudad", "ciu_nombre"})
        Dim vEstado = TryGetCell(row, {"colEstado", "prov_estado"})

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

    ' =========== Botones ===========
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False)
        LimpiarInputs()
        lblStatus.Text = "Modo: Nuevo"
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        HabilitarInputs(True)
        LimpiarInputs()
        lblStatus.Text = "Modo: Editar"
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        LimpiarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValNombre() AndAlso ValCiudad() AndAlso ValEmail() AndAlso ValTelefono()) Then
                lblStatus.Text = "Corrija los campos marcados." : Return
            End If

            Dim pars As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@nom", txtNombre.Text.Trim()),
                New NpgsqlParameter("@ruc", If(String.IsNullOrWhiteSpace(txtRuc.Text), CType(DBNull.Value, Object), txtRuc.Text.Trim())),
                New NpgsqlParameter("@dir", If(String.IsNullOrWhiteSpace(txtDireccion.Text), CType(DBNull.Value, Object), txtDireccion.Text.Trim())),
                New NpgsqlParameter("@tel", If(String.IsNullOrWhiteSpace(txtTelefono.Text), CType(DBNull.Value, Object), txtTelefono.Text.Trim())),
                New NpgsqlParameter("@mail", If(String.IsNullOrWhiteSpace(txtEmail.Text), CType(DBNull.Value, Object), txtEmail.Text.Trim())),
                New NpgsqlParameter("@estado", cboEstado.SelectedItem.ToString()),
                New NpgsqlParameter("@ciu", CInt(cboCiudad.SelectedValue))
            }

            If txtId.TextLength = 0 Then
                Dim sql = "
                    INSERT INTO proveedor (ciu_id, prov_nombre, prov_ruc, prov_direccion, prov_telefono, prov_email, prov_estado)
                    VALUES (@ciu, @nom, @ruc, @dir, @tel, @mail, @estado)
                    RETURNING prov_id;"
                Dim newId = Conexiones.EjecutarsqlScalar(sql, pars)
                txtId.Text = Convert.ToString(newId)
                lblStatus.Text = $"Insertado ID {txtId.Text}."
            Else
                pars.Add(New NpgsqlParameter("@id", Integer.Parse(txtId.Text)))
                Dim sql = "
                    UPDATE proveedor
                    SET ciu_id=@ciu, prov_nombre=@nom, prov_ruc=@ruc, prov_direccion=@dir,
                        prov_telefono=@tel, prov_email=@mail, prov_estado=@estado
                    WHERE prov_id=@id;"
                Dim ok = Conexiones.Ejecutarsql(sql, pars)
                lblStatus.Text = If(ok, "Actualizado.", "Sin cambios.")
            End If
            LimpiarInputs()
            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23503"
            MostrarError("Ciudad inválida (FK)", ex)
        Catch ex As PostgresException When ex.SqlState = "23505"
            MostrarError("Posible duplicado (revisa restricciones únicas)", ex)
        Catch ex As Exception
            MostrarError("Error al guardar proveedor", ex)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        If MessageBox.Show("¿Eliminar el proveedor?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("DELETE FROM proveedor WHERE prov_id=@id;",
                    New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", Integer.Parse(txtId.Text))})
                If ok Then
                    CargarListado() : DeshabilitarInputs() : lblStatus.Text = "Eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MessageBox.Show("No se puede eliminar: proveedor referenciado.",
                                "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Catch ex As Exception
                MostrarError("Error al eliminar", ex)
            End Try
        End If
    End Sub

    ' =========== Validaciones ===========
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
        Me.close()
    End Sub
End Class
