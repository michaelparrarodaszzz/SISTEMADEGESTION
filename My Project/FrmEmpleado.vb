Option Strict On
Option Infer On
Imports Npgsql
Imports System.Text.RegularExpressions

Public Class FrmEmpleado
    Private _cargando As Boolean = False

    Private Sub FrmEmpleado_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PrepararGrid()
        CargarCiudades()
        CargarEstados()
        CargarListado()
        DeshabilitarInputs()
        lblStatus.Text = "Listo."

        AddHandler txtNombre.TextChanged, Sub() ValNombre()
        AddHandler txtApellido.TextChanged, Sub() ValApellido()
        AddHandler cboCiudad.SelectedIndexChanged, Sub() ValCiudad()
        AddHandler txtTelefono.Leave, Sub() ValTelefono()
    End Sub

    ' ======= Grid =======
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

    ' ======= Data =======
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
            _cargando = True
            Dim dt = Conexiones.Consulta("
                SELECT e.emp_id,
                       e.emp_nombre,
                       e.emp_apellido,
                       e.ciu_id,
                       c.ciu_nombre,
                       e.emp_ci,
                       e.emp_direccion,
                       e.emp_telefono,
                       e.emp_fecha_nacimiento,
                       e.emp_estado
                FROM empleado e
                LEFT JOIN ciudad c ON c.ciu_id = e.ciu_id
                ORDER BY e.emp_id;")
            DataGridView1.DataSource = dt

            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = Nothing
        Catch ex As Exception
            MostrarError("Error al consultar empleados", ex)
        Finally
            _cargando = False
        End Try
    End Sub

    ' ======= Estados UI =======
    Private Sub DeshabilitarInputs()
        For Each tb In New TextBox() {txtId, txtNombre, txtApellido, txtCi, txtTelefono}
            tb.ReadOnly = True
        Next
        txtDireccion.ReadOnly = True
        cboCiudad.Enabled = False
        dtpFechaNac.Enabled = False
        chkSinFecha.Enabled = False
        cboEstado.Enabled = False

        BtnGuardar.Enabled = False : BtnCancelar.Enabled = False
        BtnEditar.Enabled = False : BtnEliminar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        For Each tb In New TextBox() {txtNombre, txtApellido, txtCi, txtTelefono}
            tb.ReadOnly = False
        Next
        txtDireccion.ReadOnly = False
        cboCiudad.Enabled = True
        dtpFechaNac.Enabled = True
        chkSinFecha.Enabled = True
        cboEstado.Enabled = True

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
        txtId.Clear() : txtNombre.Clear() : txtApellido.Clear()
        txtCi.Clear() : txtTelefono.Clear() : txtDireccion.Clear()
        If cboCiudad.Items.Count > 0 Then cboCiudad.SelectedIndex = -1
        cboEstado.SelectedIndex = 0
        dtpFechaNac.Value = Date.Today
        chkSinFecha.Checked = False
        ep.Clear()
    End Sub

    ' ======= Grid -> Form =======
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellClick, DataGridView1.CellContentClick
        If _cargando OrElse e.RowIndex < 0 Then Return
        CargarFila(e.RowIndex)
    End Sub

    Private Sub CargarFila(idx As Integer)
        If idx < 0 OrElse idx >= DataGridView1.Rows.Count Then Return
        Dim row = DataGridView1.Rows(idx)

        txtId.Text = Convert.ToString(TryGetCell(row, {"colId", "emp_id"}))
        txtNombre.Text = Convert.ToString(TryGetCell(row, {"colNombre", "emp_nombre"}))
        txtApellido.Text = Convert.ToString(TryGetCell(row, {"colApellido", "emp_apellido"}))
        txtCi.Text = Convert.ToString(TryGetCell(row, {"colCi", "emp_ci"}))
        txtTelefono.Text = Convert.ToString(TryGetCell(row, {"colTelefono", "emp_telefono"}))
        txtDireccion.Text = Convert.ToString(TryGetCell(row, {"colDireccion", "emp_direccion"}))

        Dim vCiudadId = TryGetCell(row, {"colCiudadId", "ciu_id"})
        Dim vCiudadNom = TryGetCell(row, {"colCiudad", "ciu_nombre"})
        If vCiudadId IsNot Nothing AndAlso vCiudadId IsNot DBNull.Value Then
            cboCiudad.SelectedValue = CInt(vCiudadId)
        Else
            SeleccionarCiudadPorNombre(Convert.ToString(vCiudadNom))
        End If

        Dim vFec = TryGetCell(row, {"colFechaNac", "emp_fecha_nacimiento"})
        If vFec IsNot Nothing AndAlso vFec IsNot DBNull.Value Then
            dtpFechaNac.Value = CDate(vFec)
            chkSinFecha.Checked = False
        Else
            chkSinFecha.Checked = True
        End If

        Dim vEstado = TryGetCell(row, {"colEstado", "emp_estado"})
        cboEstado.SelectedItem = If(vEstado Is Nothing OrElse vEstado Is DBNull.Value,
                                    "ACTIVO",
                                    Convert.ToString(vEstado).ToUpperInvariant())

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

    ' ======= Botones =======
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
            If Not (ValNombre() AndAlso ValApellido() AndAlso ValCiudad() AndAlso ValTelefono()) Then
                lblStatus.Text = "Corrija los campos marcados." : Return
            End If

            Dim pars As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@nom", txtNombre.Text.Trim()),
                New NpgsqlParameter("@ape", txtApellido.Text.Trim()),
                New NpgsqlParameter("@ci", If(String.IsNullOrWhiteSpace(txtCi.Text), CType(DBNull.Value, Object), txtCi.Text.Trim())),
                New NpgsqlParameter("@tel", If(String.IsNullOrWhiteSpace(txtTelefono.Text), CType(DBNull.Value, Object), txtTelefono.Text.Trim())),
                New NpgsqlParameter("@dir", If(String.IsNullOrWhiteSpace(txtDireccion.Text), CType(DBNull.Value, Object), txtDireccion.Text.Trim())),
                New NpgsqlParameter("@ciu", CInt(cboCiudad.SelectedValue)),
                New NpgsqlParameter("@estado", cboEstado.SelectedItem.ToString()),
                New NpgsqlParameter("@fnac", If(chkSinFecha.Checked, CType(DBNull.Value, Object), CType(dtpFechaNac.Value.Date, Object)))
            }

            If txtId.TextLength = 0 Then
                Dim sql = "
                    INSERT INTO empleado (emp_nombre, emp_apellido, emp_ci, emp_direccion,
                                          emp_telefono, ciu_id, emp_fecha_nacimiento, emp_estado)
                    VALUES (@nom, @ape, @ci, @dir, @tel, @ciu, @fnac, @estado)
                    RETURNING emp_id;"
                Dim newId = Conexiones.EjecutarsqlScalar(sql, pars)
                txtId.Text = Convert.ToString(newId)
                lblStatus.Text = $"Insertado ID {txtId.Text}."
            Else
                pars.Add(New NpgsqlParameter("@id", Integer.Parse(txtId.Text)))
                Dim sql = "
                    UPDATE empleado
                    SET emp_nombre=@nom, emp_apellido=@ape, emp_ci=@ci, emp_direccion=@dir,
                        emp_telefono=@tel, ciu_id=@ciu, emp_fecha_nacimiento=@fnac, emp_estado=@estado
                    WHERE emp_id=@id;"
                Dim ok = Conexiones.Ejecutarsql(sql, pars)
                lblStatus.Text = If(ok, "Actualizado.", "Sin cambios.")
            End If
            LimpiarInputs()
            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23505" ' unique_violation (emp_ci UNIQUE)
            ep.SetError(txtCi, "CI duplicado.")
            lblStatus.Text = "CI duplicado."
        Catch ex As PostgresException When ex.SqlState = "23503" ' FK
            MostrarError("Ciudad inválida (FK)", ex)
        Catch ex As Exception
            MostrarError("Error al guardar empleado", ex)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        If MessageBox.Show("¿Eliminar el empleado?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("DELETE FROM empleado WHERE emp_id=@id;",
                    New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", Integer.Parse(txtId.Text))})
                If ok Then
                    CargarListado() : DeshabilitarInputs() : lblStatus.Text = "Eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MessageBox.Show("No se puede eliminar: empleado referenciado.",
                                "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Catch ex As Exception
                MostrarError("Error al eliminar empleado", ex)
            End Try
        End If
    End Sub

    ' ======= Validaciones =======
    Private Function ValNombre() As Boolean
        Dim v = txtNombre.Text.Trim()
        If v.Length = 0 Then ep.SetError(txtNombre, "Obligatorio.") : Return False
        If v.Length > 20 Then ep.SetError(txtNombre, "Máx. 20 caracteres.") : Return False
        ep.SetError(txtNombre, "") : Return True
    End Function

    Private Function ValApellido() As Boolean
        Dim v = txtApellido.Text.Trim()
        If v.Length = 0 Then ep.SetError(txtApellido, "Obligatorio.") : Return False
        If v.Length > 20 Then ep.SetError(txtApellido, "Máx. 20 caracteres.") : Return False
        ep.SetError(txtApellido, "") : Return True
    End Function

    Private Function ValCiudad() As Boolean
        If cboCiudad.SelectedIndex < 0 Then ep.SetError(cboCiudad, "Seleccione ciudad.") : Return False
        ep.SetError(cboCiudad, "") : Return True
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
