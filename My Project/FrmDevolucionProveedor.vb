Option Strict On
Option Infer On
Imports Npgsql
Imports NpgsqlTypes

Public Class FrmDevolucionProveedor

    Private ReadOnly ESTADOS As String() = {"PENDIENTE", "APROBADA", "ANULADA"}

    Private Sub FrmDevolucionProveedor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarEstados()
        CargarProveedores()
        CargarUsuarios()
        CargarListado()
        Dev_Disable()

        ' Validación en tiempo real
        AddHandler txtObs.TextChanged, Sub() Dev_ValObs()

        lblStatus.Text = "Listo."
    End Sub

    ' ========== Utilitarios ==========
    Private Function TryGetCell(dgv As DataGridView, row As DataGridViewRow, names() As String) As Object
        For Each n In names
            If n IsNot Nothing AndAlso dgv.Columns.Contains(n) Then Return row.Cells(n).Value
        Next
        Return Nothing
    End Function

    Private Sub ShowErr(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    ' ========== Data ==========
    Private Sub CargarEstados()
        cboEstado.Items.Clear()
        cboEstado.Items.AddRange(ESTADOS.Cast(Of Object).ToArray())
        cboEstado.SelectedIndex = -1
    End Sub

    Private Sub CargarProveedores()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT prov_id, prov_nombre
                  FROM proveedor
              ORDER BY prov_nombre;")
            cboProv.DataSource = dt
            cboProv.ValueMember = "prov_id"
            cboProv.DisplayMember = "prov_nombre"
            cboProv.SelectedIndex = -1
        Catch ex As Exception
            ShowErr("Cargar proveedores", ex)
        End Try
    End Sub

    Private Sub CargarUsuarios()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT usua_id, usua_nombre
                  FROM usuario
              ORDER BY usua_nombre;")
            cboUsuario.DataSource = dt
            cboUsuario.ValueMember = "usua_id"
            cboUsuario.DisplayMember = "usua_nombre"
            cboUsuario.SelectedIndex = -1
        Catch ex As Exception
            ShowErr("Cargar usuarios", ex)
        End Try
    End Sub

    Private Sub CargarListado()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT d.devprov_id,
                       d.prov_id, p.prov_nombre,
                       d.usua_id, u.usua_nombre,
                       d.devprov_fecha, d.devprov_hora,
                       d.devprov_obs, d.devprov_estado
                  FROM devolucion_a_proveedor d
                  JOIN proveedor p ON p.prov_id = d.prov_id
                  LEFT JOIN usuario u ON u.usua_id = d.usua_id
              ORDER BY d.devprov_fecha DESC, d.devprov_id DESC;")
            dgvDevProv.DataSource = dt
            dgvDevProv.ClearSelection()
            dgvDevProv.CurrentCell = Nothing
        Catch ex As Exception
            ShowErr("Cargar devoluciones", ex)
        End Try
    End Sub

    ' ========== Estados de edición ==========
    Private Sub Dev_Disable()
        txtDevId.ReadOnly = True
        cboProv.Enabled = False
        cboUsuario.Enabled = False
        dtpFecha.Enabled = False
        dtpHora.Enabled = False
        cboEstado.Enabled = False
        txtObs.ReadOnly = True

        btnDevGuardar.Enabled = False
        btnDevEliminar.Enabled = False
        btnDevEditar.Enabled = False
        btnDevEliminar.Enabled = False
        btnDevNuevo.Enabled = True

        ep.Clear()
    End Sub

    Private Sub Dev_Enable(editing As Boolean)
        cboProv.Enabled = True
        cboUsuario.Enabled = True
        dtpFecha.Enabled = True
        dtpHora.Enabled = True
        cboEstado.Enabled = True
        txtObs.ReadOnly = False

        btnDevGuardar.Enabled = True
        btnDevEliminar.Enabled = True
        btnDevNuevo.Enabled = False
        btnDevEliminar.Enabled = editing
        btnDevEditar.Enabled = False

        If Not editing Then
            txtDevId.Clear()
            If cboProv.Items.Count > 0 Then cboProv.SelectedIndex = -1
            If cboUsuario.Items.Count > 0 Then cboUsuario.SelectedIndex = -1
            cboEstado.SelectedIndex = -1
            txtObs.Clear()
            dtpFecha.Value = Date.Today
            dtpHora.Value = Date.Today.AddHours(Date.Now.Hour).AddMinutes(Date.Now.Minute)
        End If

        cboProv.Focus()
    End Sub

    ' ========== Selección en grilla ==========
    Private Sub dgvDevProv_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDevProv.CellClick
        If e.RowIndex < 0 Then Return
        Dim r = dgvDevProv.Rows(e.RowIndex)

        txtDevId.Text = Convert.ToString(TryGetCell(dgvDevProv, r, {"colId", "devprov_id"}))
        txtObs.Text = Convert.ToString(TryGetCell(dgvDevProv, r, {"colObs", "devprov_obs"}))

        Dim vProvId = TryGetCell(dgvDevProv, r, {"colProvId", "prov_id"})
        If vProvId IsNot Nothing AndAlso vProvId IsNot DBNull.Value Then cboProv.SelectedValue = CInt(vProvId)

        Dim vUsuId = TryGetCell(dgvDevProv, r, {"colUsuaId", "usua_id"})
        If vUsuId IsNot Nothing AndAlso vUsuId IsNot DBNull.Value Then cboUsuario.SelectedValue = CInt(vUsuId) Else cboUsuario.SelectedIndex = -1

        Dim vFec = TryGetCell(dgvDevProv, r, {"colFecha", "devprov_fecha"})
        If vFec IsNot Nothing AndAlso vFec IsNot DBNull.Value Then dtpFecha.Value = CDate(vFec)

        Dim vHora = TryGetCell(dgvDevProv, r, {"colHora", "devprov_hora"})
        If vHora IsNot Nothing AndAlso vHora IsNot DBNull.Value Then dtpHora.Value = CDate("2000-01-01 " & vHora.ToString())

        Dim vEst = Convert.ToString(TryGetCell(dgvDevProv, r, {"colEstado", "devprov_estado"}))
        If Not String.IsNullOrWhiteSpace(vEst) Then cboEstado.SelectedItem = vEst Else cboEstado.SelectedIndex = -1

        Dev_Disable()
        btnDevEditar.Enabled = True
        btnDevEliminar.Enabled = True
    End Sub

    ' ========== Botones ==========
    Private Sub btnDevNuevo_Click(sender As Object, e As EventArgs) Handles btnDevNuevo.Click
        Dev_Enable(False)
        lblStatus.Text = "Devolución - Nuevo"
    End Sub

    Private Sub btnDevEditar_Click(sender As Object, e As EventArgs) Handles btnDevEditar.Click
        If txtDevId.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        Dev_Enable(True)
        lblStatus.Text = "Devolución - Editar"
    End Sub

    Private Sub btnDevGuardar_Click(sender As Object, e As EventArgs) Handles btnDevGuardar.Click
        Try
            If Not (Dev_ValProv() AndAlso Dev_ValUsuario() AndAlso Dev_ValEstado() AndAlso Dev_ValObs()) Then
                lblStatus.Text = "Corrija los campos marcados."
                Return
            End If

            Dim pProv As New NpgsqlParameter("@prov", NpgsqlDbType.Integer) With {.Value = CInt(cboProv.SelectedValue)}
            Dim pUsu As New NpgsqlParameter("@usu", NpgsqlDbType.Integer) With {.Value = CInt(cboUsuario.SelectedValue)}
            Dim pFec As New NpgsqlParameter("@fec", NpgsqlDbType.Date) With {.Value = dtpFecha.Value.Date}
            Dim pHor As New NpgsqlParameter("@hor", NpgsqlDbType.Time) With {.Value = dtpHora.Value.TimeOfDay}
            Dim pEst As New NpgsqlParameter("@est", NpgsqlDbType.Varchar) With {.Value = CStr(cboEstado.SelectedItem)}
            Dim pObs As New NpgsqlParameter("@obs", NpgsqlDbType.Varchar)
            pObs.Value = If(String.IsNullOrWhiteSpace(txtObs.Text), CType(DBNull.Value, Object), txtObs.Text.Trim())

            If txtDevId.TextLength = 0 Then
                Dim id = Conexiones.EjecutarsqlScalar("
                    INSERT INTO devolucion_a_proveedor(prov_id, usua_id, devprov_fecha, devprov_hora, devprov_obs, devprov_estado)
                    VALUES(@prov, @usu, @fec, @hor, @obs, @est)
                    RETURNING devprov_id;",
                    New List(Of NpgsqlParameter) From {pProv, pUsu, pFec, pHor, pObs, pEst})
                txtDevId.Text = CStr(id)
                lblStatus.Text = $"Devolución insertada (ID {txtDevId.Text})."
            Else
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE devolucion_a_proveedor
                       SET prov_id=@prov,
                           usua_id=@usu,
                           devprov_fecha=@fec,
                           devprov_hora=@hor,
                           devprov_obs=@obs,
                           devprov_estado=@est
                     WHERE devprov_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        pProv, pUsu, pFec, pHor, pObs, pEst,
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtDevId.Text)}
                    })
                lblStatus.Text = If(ok, "Devolución actualizada.", "Sin cambios.")
            End If

            CargarListado()
            Dev_Enable(False)

        Catch ex As PostgresException When ex.SqlState = "23505"
            ShowErr("Registro duplicado", ex)
        Catch ex As Exception
            ShowErr("Guardar devolución", ex)
        End Try
    End Sub

    Private Sub btnDevEliminar_Click(sender As Object, e As EventArgs) Handles btnDevEliminar.Click
        If txtDevId.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        If MessageBox.Show("¿Eliminar la devolución seleccionada?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM devolucion_a_proveedor WHERE devprov_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtDevId.Text)}
                    })
                If ok Then
                    CargarListado()
                    Dev_Disable()
                    lblStatus.Text = "Devolución eliminada."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                ShowErr("No se puede eliminar: registro referenciado.", ex)
            Catch ex As Exception
                ShowErr("Eliminar devolución", ex)
            End Try
        End If
    End Sub

    ' ========== Validaciones ==========
    Private Function Dev_ValProv() As Boolean
        If cboProv.SelectedIndex < 0 Then ep.SetError(cboProv, "Seleccione un proveedor.") : Return False
        ep.SetError(cboProv, "") : Return True
    End Function

    Private Function Dev_ValUsuario() As Boolean
        If cboUsuario.SelectedIndex < 0 Then ep.SetError(cboUsuario, "Seleccione un usuario.") : Return False
        ep.SetError(cboUsuario, "") : Return True
    End Function

    Private Function Dev_ValEstado() As Boolean
        If cboEstado.SelectedIndex < 0 Then ep.SetError(cboEstado, "Seleccione un estado.") : Return False
        ep.SetError(cboEstado, "") : Return True
    End Function

    Private Function Dev_ValObs() As Boolean
        Dim v = txtObs.Text.Trim()
        If v.Length > 120 Then ep.SetError(txtObs, "Máx. 120 caracteres.") : Return False
        ep.SetError(txtObs, "") : Return True
    End Function

    Private Sub BtnPagCerrar_Click(sender As Object, e As EventArgs) Handles BtnDevCerrar.Click
        Me.Close()
    End Sub

    Private Sub BtnDevCancelar_Click_1(sender As Object, e As EventArgs) Handles BtnDevCancelar.Click
        Dev_Enable(False)
        lblStatus.Text = "Cancelado."
    End Sub
End Class
