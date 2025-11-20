' FrmPresupuesto.vb (solo lógica; los controles están en el Diseñador)
Option Strict On
Option Infer On
Imports Npgsql
Imports NpgsqlTypes

Public Class FrmPresupuesto
    Private _cargando As Boolean = False

    Private Sub FrmPresupuesto_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Estados fijos
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList
        cboEstado.Items.Clear()
        cboEstado.Items.AddRange(New Object() {"PENDIENTE", "APROBADO", "ANULADO"})
        cboEstado.SelectedIndex = -1

        PrepararGrid()
        CargarClientes()
        CargarUsuarios()
        CargarListado()
        DeshabilitarInputs()
        lblStatus.Text = "Listo."

        ' Validaciones en tiempo real
        AddHandler CboCliente.SelectedIndexChanged, Sub() ValCliente()
        AddHandler cboEstado.SelectedIndexChanged, Sub() ValEstado()
        AddHandler txtObs.TextChanged, Sub() ValObs()
    End Sub

    ' ====== Grid ======
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

    ' ====== Data (combos) ======
    Private Sub CargarClientes()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT cli_id, cli_nombre
                FROM cliente
                WHERE COALESCE(cli_estado,'ACTIVO') <> 'INACTIVO'
                ORDER BY cli_nombre;")
            cboCliente.DataSource = dt
            cboCliente.ValueMember = "cli_id"
            cboCliente.DisplayMember = "cli_nombre"
            cboCliente.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Error al cargar clientes", ex)
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
            MostrarError("Error al cargar usuarios", ex)
        End Try
    End Sub

    ' ====== Listado ======
    Private Sub CargarListado()
        Try
            _cargando = True
            Dim dt = Conexiones.Consulta("
                SELECT p.pres_id,
                       p.cli_id,
                       c.cli_nombre,
                       p.usua_id,
                       u.usua_nombre,
                       p.pres_fecha,
                       p.pres_hora,
                       p.pres_estado,
                       p.pres_obs
                FROM presupuesto p
                JOIN cliente  c ON c.cli_id  = p.cli_id
                LEFT JOIN usuario u ON u.usua_id = p.usua_id
                ORDER BY p.pres_fecha DESC, p.pres_hora DESC, p.pres_id DESC;")
            DataGridView1.DataSource = dt

            ' Formatos y ocultos
            If DataGridView1.Columns.Contains("pres_fecha") Then DataGridView1.Columns("pres_fecha").DefaultCellStyle.Format = "dd/MM/yyyy"
            If DataGridView1.Columns.Contains("pres_hora") Then DataGridView1.Columns("pres_hora").DefaultCellStyle.Format = "HH:mm"
            If DataGridView1.Columns.Contains("cli_id") Then DataGridView1.Columns("cli_id").Visible = False
            If DataGridView1.Columns.Contains("usua_id") Then DataGridView1.Columns("usua_id").Visible = False

            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = Nothing
        Catch ex As Exception
            MostrarError("Error al consultar presupuestos", ex)
        Finally
            _cargando = False
        End Try
    End Sub

    ' ====== Estados UI ======
    Private Sub DeshabilitarInputs()
        txtId.ReadOnly = True
        cboCliente.Enabled = False
        cboUsuario.Enabled = False
        dtpFecha.Enabled = False
        dtpHora.Enabled = False
        cboEstado.Enabled = False
        txtObs.ReadOnly = True

        BtnGuardar.Enabled = False : BtnCancelar.Enabled = False
        BtnEditar.Enabled = False : BtnEliminar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        cboCliente.Enabled = True
        cboUsuario.Enabled = True
        dtpFecha.Enabled = True
        dtpHora.Enabled = True
        cboEstado.Enabled = True
        txtObs.ReadOnly = False

        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False

        If Not paraEdicion Then
            LimpiarInputs()
            If cboCliente.Items.Count > 0 Then cboCliente.SelectedIndex = -1
            If cboUsuario.Items.Count > 0 Then cboUsuario.SelectedIndex = -1
            cboEstado.SelectedIndex = 0 ' PENDIENTE por defecto
            dtpFecha.Value = DateTime.Today
            dtpHora.Value = DateTime.Now
            cboCliente.Focus()
        End If
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear()
        txtObs.Clear()
        ep.Clear()
    End Sub

    ' ====== Grid -> Form ======
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellClick, DataGridView1.CellContentClick
        If _cargando OrElse e.RowIndex < 0 Then Return
        Dim row = DataGridView1.Rows(e.RowIndex)

        txtId.Text = Convert.ToString(TryGetCell(row, {"colId", "pres_id"}))

        Dim vCliId = TryGetCell(row, {"colCliId", "cli_id"})
        If vCliId IsNot Nothing AndAlso vCliId IsNot DBNull.Value Then
            cboCliente.SelectedValue = CInt(vCliId)
        Else
            SeleccionarClientePorNombre(Convert.ToString(TryGetCell(row, {"colCliente", "cli_nombre"})))
        End If

        Dim vUsuId = TryGetCell(row, {"colUsuId", "usua_id"})
        If vUsuId IsNot Nothing AndAlso vUsuId IsNot DBNull.Value Then
            cboUsuario.SelectedValue = CInt(vUsuId)
        Else
            SeleccionarUsuarioPorNombre(Convert.ToString(TryGetCell(row, {"colUsuario", "usua_nombre"})))
        End If

        Dim vFecha = TryGetCell(row, {"colFecha", "pres_fecha"})
        Dim vHora = TryGetCell(row, {"colHora", "pres_hora"})
        If vFecha IsNot Nothing AndAlso vFecha IsNot DBNull.Value Then dtpFecha.Value = CDate(vFecha)
        If vHora IsNot Nothing AndAlso vHora IsNot DBNull.Value Then
            If TypeOf vHora Is TimeSpan Then
                dtpHora.Value = Date.Today.Add(CType(vHora, TimeSpan))
            Else
                dtpHora.Value = CDate(vHora)
            End If
        End If

        cboEstado.Text = Convert.ToString(TryGetCell(row, {"colEstado", "pres_estado"}))
        txtObs.Text = Convert.ToString(TryGetCell(row, {"colObs", "pres_obs"}))

        DeshabilitarInputs()
        BtnEditar.Enabled = True : BtnEliminar.Enabled = True
        lblStatus.Text = "Registro seleccionado."
    End Sub

    Private Function TryGetCell(row As DataGridViewRow, names() As String) As Object
        For Each n In names
            If n Is Nothing Then Continue For
            If DataGridView1.Columns.Contains(n) Then Return row.Cells(n).Value
        Next
        Return Nothing
    End Function

    Private Sub SeleccionarClientePorNombre(nombre As String)
        If String.IsNullOrWhiteSpace(nombre) Then cboCliente.SelectedIndex = -1 : Return
        For i = 0 To cboCliente.Items.Count - 1
            Dim drv = TryCast(cboCliente.Items(i), DataRowView)
            If drv IsNot Nothing AndAlso
               String.Equals(Convert.ToString(drv("cli_nombre")), nombre, StringComparison.OrdinalIgnoreCase) Then
                cboCliente.SelectedIndex = i : Exit For
            End If
        Next
    End Sub

    Private Sub SeleccionarUsuarioPorNombre(nombre As String)
        If String.IsNullOrWhiteSpace(nombre) Then cboUsuario.SelectedIndex = -1 : Return
        For i = 0 To cboUsuario.Items.Count - 1
            Dim drv = TryCast(cboUsuario.Items(i), DataRowView)
            If drv IsNot Nothing AndAlso
               String.Equals(Convert.ToString(drv("usua_nombre")), nombre, StringComparison.OrdinalIgnoreCase) Then
                cboUsuario.SelectedIndex = i : Exit For
            End If
        Next
    End Sub

    ' ====== Botones ======
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False)
        lblStatus.Text = "Modo: Nuevo"
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un presupuesto." : Return
        HabilitarInputs(True)
        lblStatus.Text = "Modo: Editar"
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValCliente() AndAlso ValEstado() AndAlso ValObs()) Then
                lblStatus.Text = "Corrija los campos marcados." : Return
            End If

            ' Parámetros
            Dim pCli As New NpgsqlParameter("@cli", NpgsqlDbType.Integer) With {.Value = CInt(cboCliente.SelectedValue)}
            Dim pUsu As New NpgsqlParameter("@usu", NpgsqlDbType.Integer)
            pUsu.Value = If(cboUsuario.SelectedIndex >= 0, CInt(cboUsuario.SelectedValue), CType(DBNull.Value, Object))
            Dim pFec As New NpgsqlParameter("@fec", NpgsqlDbType.Date) With {.Value = dtpFecha.Value.Date}
            Dim pHor As New NpgsqlParameter("@hor", NpgsqlDbType.Time) With {.Value = dtpHora.Value.TimeOfDay}
            Dim pEst As New NpgsqlParameter("@est", NpgsqlDbType.Varchar) With {.Value = cboEstado.Text}
            Dim pObs As New NpgsqlParameter("@obs", NpgsqlDbType.Varchar)
            pObs.Value = If(String.IsNullOrWhiteSpace(txtObs.Text), CType(DBNull.Value, Object), txtObs.Text.Trim())

            If txtId.TextLength = 0 Then
                ' INSERT
                Dim newId = Conexiones.EjecutarsqlScalar("
                    INSERT INTO presupuesto (cli_id, usua_id, pres_fecha, pres_hora, pres_estado, pres_obs)
                    VALUES (@cli, @usu, @fec, @hor, @est, @obs)
                    RETURNING pres_id;",
                    New List(Of NpgsqlParameter) From {pCli, pUsu, pFec, pHor, pEst, pObs})
                txtId.Text = Convert.ToString(newId)
                lblStatus.Text = $"Insertado ID {txtId.Text}."
            Else
                ' UPDATE
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE presupuesto
                       SET cli_id=@cli,
                           usua_id=@usu,
                           pres_fecha=@fec,
                           pres_hora=@hor,
                           pres_estado=@est,
                           pres_obs=@obs
                     WHERE pres_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        pCli, pUsu, pFec, pHor, pEst, pObs,
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtId.Text)}
                    })
                lblStatus.Text = If(ok, "Actualizado.", "Sin cambios.")
            End If
            LimpiarInputs()
            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23503"
            ' FK de cliente/usuario inválida
            MostrarError("Cliente/Usuario no válido (FK).", ex)
        Catch ex As Exception
            MostrarError("Error al guardar presupuesto", ex)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un presupuesto." : Return
        If MessageBox.Show("¿Eliminar el presupuesto seleccionado?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM presupuesto WHERE pres_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtId.Text)}
                    })
                If ok Then
                    CargarListado() : DeshabilitarInputs() : LimpiarInputs() : lblStatus.Text = "Eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                ' Si existieran detalles/relaciones que referencian la cabecera
                MostrarError("No se puede eliminar: presupuesto referenciado.", ex)
            Catch ex As Exception
                MostrarError("Error al eliminar presupuesto", ex)
            End Try
        End If
    End Sub

    ' ====== Validaciones UI ======
    Private Function ValCliente() As Boolean
        If cboCliente.SelectedIndex < 0 Then ep.SetError(cboCliente, "Seleccione un cliente.") : Return False
        ep.SetError(cboCliente, "") : Return True
    End Function

    Private Function ValEstado() As Boolean
        If String.IsNullOrWhiteSpace(cboEstado.Text) Then ep.SetError(cboEstado, "Seleccione un estado.") : Return False
        ep.SetError(cboEstado, "") : Return True
    End Function

    Private Function ValObs() As Boolean
        Dim v = txtObs.Text.Trim()
        If v.Length > 200 Then ep.SetError(txtObs, "Máx. 200 caracteres.") : Return False
        ep.SetError(txtObs, "") : Return True
    End Function

    ' ====== Errores amigables ======
    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub

End Class
