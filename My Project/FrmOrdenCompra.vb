' FrmOrdenCompra.vb (solo lógica; los controles están en el Diseñador)
Option Strict On
Option Infer On
Imports Npgsql
Imports NpgsqlTypes

Public Class FrmOrdenCompra
    Private _cargando As Boolean = False

    Private Sub FrmOrdenCompra_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtpFecha.Format = DateTimePickerFormat.Custom : dtpFecha.CustomFormat = "dd/MM/yyyy"
        dtpHora.Format = DateTimePickerFormat.Custom : dtpHora.CustomFormat = "HH:mm" : dtpHora.ShowUpDown = True

        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList
        cboEstado.Items.Clear()
        cboEstado.Items.AddRange(New Object() {"PENDIENTE", "APROBADO", "ANULADO"})
        cboEstado.SelectedIndex = -1

        PrepararGrid()
        CargarProveedores()
        CargarUsuarios()
        CargarListado()
        DeshabilitarInputs()
        lblStatus.Text = "Listo."

        AddHandler cboProveedor.SelectedIndexChanged, Sub() ValProveedor()
        AddHandler cboEstado.SelectedIndexChanged, Sub() ValEstado()
    End Sub

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

    ' ==== Combos ====
    Private Sub CargarProveedores()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT prov_id, prov_nombre
                FROM proveedor
                WHERE COALESCE(prov_estado,'ACTIVO') <> 'INACTIVO'
                ORDER BY prov_nombre;")
            cboProveedor.DataSource = dt
            cboProveedor.ValueMember = "prov_id"
            cboProveedor.DisplayMember = "prov_nombre"
            cboProveedor.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Error al cargar proveedores", ex)
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

    ' ==== Listado ====
    Private Sub CargarListado()
        Try
            _cargando = True
            Dim dt = Conexiones.Consulta("
                SELECT o.ordcom_id,
                       o.prov_id,
                       p.prov_nombre,
                       o.usua_id,
                       u.usua_nombre,
                       o.ordcom_fecha,
                       o.ordcom_hora,
                       o.ordcom_estado
                FROM orden_de_compra o
                JOIN proveedor p ON p.prov_id = o.prov_id
                LEFT JOIN usuario u ON u.usua_id = o.usua_id
                ORDER BY o.ordcom_fecha DESC, o.ordcom_hora DESC, o.ordcom_id DESC;")
            DataGridView1.DataSource = dt

            If DataGridView1.Columns.Contains("ordcom_fecha") Then DataGridView1.Columns("ordcom_fecha").DefaultCellStyle.Format = "dd/MM/yyyy"
            If DataGridView1.Columns.Contains("ordcom_hora") Then DataGridView1.Columns("ordcom_hora").DefaultCellStyle.Format = "HH:mm"
            If DataGridView1.Columns.Contains("prov_id") Then DataGridView1.Columns("prov_id").Visible = False
            If DataGridView1.Columns.Contains("usua_id") Then DataGridView1.Columns("usua_id").Visible = False

            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = Nothing
        Catch ex As Exception
            MostrarError("Error al consultar órdenes de compra", ex)
        Finally
            _cargando = False
        End Try
    End Sub

    ' ==== Estados UI ====
    Private Sub DeshabilitarInputs()
        txtId.ReadOnly = True
        cboProveedor.Enabled = False
        cboUsuario.Enabled = False
        dtpFecha.Enabled = False
        dtpHora.Enabled = False
        cboEstado.Enabled = False

        BtnGuardar.Enabled = False : BtnCancelar.Enabled = False
        BtnEditar.Enabled = False : BtnEliminar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        cboProveedor.Enabled = True
        cboUsuario.Enabled = True
        dtpFecha.Enabled = True
        dtpHora.Enabled = True
        cboEstado.Enabled = True

        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False

        If Not paraEdicion Then
            LimpiarInputs()
            If cboProveedor.Items.Count > 0 Then cboProveedor.SelectedIndex = -1
            If cboUsuario.Items.Count > 0 Then cboUsuario.SelectedIndex = -1
            cboEstado.SelectedIndex = 0   ' PENDIENTE
            dtpFecha.Value = DateTime.Today
            dtpHora.Value = DateTime.Now
            cboProveedor.Focus()
        End If
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear()
        ep.Clear()
    End Sub

    ' ==== Grid -> Form ====
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellClick, DataGridView1.CellContentClick
        If _cargando OrElse e.RowIndex < 0 Then Return
        Dim row = DataGridView1.Rows(e.RowIndex)

        txtId.Text = Convert.ToString(TryGetCell(row, {"colid", "ordcom_id"}))

        Dim vProvId = TryGetCell(row, {"colProvId", "prov_id"})
        If vProvId IsNot Nothing AndAlso vProvId IsNot DBNull.Value Then
            cboProveedor.SelectedValue = CInt(vProvId)
        Else
            SeleccionarProveedorPorNombre(Convert.ToString(TryGetCell(row, {"colProveedor", "prov_nombre"})))
        End If

        Dim vUsuId = TryGetCell(row, {"colUsuId", "usua_id"})
        If vUsuId IsNot Nothing AndAlso vUsuId IsNot DBNull.Value Then
            cboUsuario.SelectedValue = CInt(vUsuId)
        Else
            SeleccionarUsuarioPorNombre(Convert.ToString(TryGetCell(row, {"colUsuario", "usua_nombre"})))
        End If

        Dim vFecha = TryGetCell(row, {"colFecha", "ordcom_fecha"})
        Dim vHora = TryGetCell(row, {"colHora", "ordcom_hora"})
        If vFecha IsNot Nothing AndAlso vFecha IsNot DBNull.Value Then dtpFecha.Value = CDate(vFecha)
        If vHora IsNot Nothing AndAlso vHora IsNot DBNull.Value Then
            If TypeOf vHora Is TimeSpan Then
                dtpHora.Value = Date.Today.Add(CType(vHora, TimeSpan))
            Else
                dtpHora.Value = CDate(vHora)
            End If
        End If

        cboEstado.Text = Convert.ToString(TryGetCell(row, {"colEstado", "ordcom_estado"}))

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

    Private Sub SeleccionarProveedorPorNombre(nombre As String)
        If String.IsNullOrWhiteSpace(nombre) Then cboProveedor.SelectedIndex = -1 : Return
        For i = 0 To cboProveedor.Items.Count - 1
            Dim drv = TryCast(cboProveedor.Items(i), DataRowView)
            If drv IsNot Nothing AndAlso
               String.Equals(CStr(drv("prov_nombre")), nombre, StringComparison.OrdinalIgnoreCase) Then
                cboProveedor.SelectedIndex = i : Exit For
            End If
        Next
    End Sub

    Private Sub SeleccionarUsuarioPorNombre(nombre As String)
        If String.IsNullOrWhiteSpace(nombre) Then cboUsuario.SelectedIndex = -1 : Return
        For i = 0 To cboUsuario.Items.Count - 1
            Dim drv = TryCast(cboUsuario.Items(i), DataRowView)
            If drv IsNot Nothing AndAlso
               String.Equals(CStr(drv("usua_nombre")), nombre, StringComparison.OrdinalIgnoreCase) Then
                cboUsuario.SelectedIndex = i : Exit For
            End If
        Next
    End Sub

    ' ==== Botones ====
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False)
        lblStatus.Text = "Modo: Nuevo"
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione una orden." : Return
        HabilitarInputs(True)
        lblStatus.Text = "Modo: Editar"
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValProveedor() AndAlso ValEstado()) Then
                lblStatus.Text = "Corrija los campos marcados." : Return
            End If

            Dim pProv As New NpgsqlParameter("@prov", NpgsqlDbType.Integer) With {.Value = CInt(cboProveedor.SelectedValue)}
            Dim pUsu As New NpgsqlParameter("@usu", NpgsqlDbType.Integer)
            pUsu.Value = If(cboUsuario.SelectedIndex >= 0, CInt(cboUsuario.SelectedValue), CType(DBNull.Value, Object))
            Dim pFec As New NpgsqlParameter("@fec", NpgsqlDbType.Date) With {.Value = dtpFecha.Value.Date}
            Dim pHor As New NpgsqlParameter("@hor", NpgsqlDbType.Time) With {.Value = dtpHora.Value.TimeOfDay}
            Dim pEst As New NpgsqlParameter("@est", NpgsqlDbType.Varchar) With {.Value = cboEstado.Text}

            If txtId.TextLength = 0 Then
                Dim newId = Conexiones.EjecutarsqlScalar("
                    INSERT INTO orden_de_compra (prov_id, usua_id, ordcom_fecha, ordcom_hora, ordcom_estado)
                    VALUES (@prov, @usu, @fec, @hor, @est)
                    RETURNING ordcom_id;",
                    New List(Of NpgsqlParameter) From {pProv, pUsu, pFec, pHor, pEst})
                txtId.Text = CStr(newId)
                lblStatus.Text = $"Insertado ID {txtId.Text}."
            Else
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE orden_de_compra
                       SET prov_id=@prov,
                           usua_id=@usu,
                           ordcom_fecha=@fec,
                           ordcom_hora=@hor,
                           ordcom_estado=@est
                     WHERE ordcom_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        pProv, pUsu, pFec, pHor, pEst,
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtId.Text)}
                    })
                lblStatus.Text = If(ok, "Actualizado.", "Sin cambios.")
            End If
            LimpiarInputs()
            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23503"
            MostrarError("Proveedor/Usuario no válido o registro referenciado (FK).", ex)
        Catch ex As Exception
            MostrarError("Error al guardar orden de compra", ex)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione una orden." : Return
        If MessageBox.Show("¿Eliminar la orden de compra seleccionada?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM orden_de_compra WHERE ordcom_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtId.Text)}
                    })
                If ok Then
                    CargarListado() : DeshabilitarInputs() : LimpiarInputs() : lblStatus.Text = "Eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MostrarError("No se puede eliminar: orden referenciada.", ex)
            Catch ex As Exception
                MostrarError("Error al eliminar orden de compra", ex)
            End Try
        End If
    End Sub

    ' ==== Validaciones ====
    Private Function ValProveedor() As Boolean
        If cboProveedor.SelectedIndex < 0 Then ep.SetError(cboProveedor, "Seleccione un proveedor.") : Return False
        ep.SetError(cboProveedor, "") : Return True
    End Function

    Private Function ValEstado() As Boolean
        If String.IsNullOrWhiteSpace(cboEstado.Text) Then ep.SetError(cboEstado, "Seleccione un estado.") : Return False
        ep.SetError(cboEstado, "") : Return True
    End Function

    ' ==== Util ====
    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub
End Class
