' FrmCompra.vb (solo lógica; los controles están en el Diseñador)
Option Strict On
Option Infer On
Imports Npgsql
Imports NpgsqlTypes
Imports System.Globalization

Public Class FrmCompra
    Private _cargando As Boolean = False

    Private Sub FrmCompra_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Fecha/Hora
        dtpFecha.Format = DateTimePickerFormat.Custom : dtpFecha.CustomFormat = "dd/MM/yyyy"
        dtpHora.Format = DateTimePickerFormat.Custom : dtpHora.CustomFormat = "HH:mm" : dtpHora.ShowUpDown = True

        ' Estado
        cboEstado.DropDownStyle = ComboBoxStyle.DropDownList
        cboEstado.Items.Clear()
        cboEstado.Items.AddRange(New Object() {"PENDIENTE", "APROBADO", "ANULADO"})
        cboEstado.SelectedIndex = -1

        PrepararGrid()
        CargarProveedores()
        CargarUsuarios()
        CargarOrdenes()
        CargarListado()
        DeshabilitarInputs()
        lblStatus.Text = "Listo."

        ' Validación en tiempo real
        AddHandler cboProveedor.SelectedIndexChanged, Sub() ValProveedor()
        AddHandler cboEstado.SelectedIndexChanged, Sub() ValEstado()
        AddHandler txtTotal.TextChanged, Sub() ValTotal()
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

    ' ===== Combos =====
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

    Private Sub CargarOrdenes()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT ordcom_id
                FROM orden_de_compra
                ORDER BY ordcom_id DESC;")
            cboOrden.DataSource = dt
            cboOrden.ValueMember = "ordcom_id"
            cboOrden.DisplayMember = "ordcom_id"
            cboOrden.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Error al cargar órdenes de compra", ex)
        End Try
    End Sub

    ' ===== Listado =====
    Private Sub CargarListado()
        Try
            _cargando = True
            Dim dt = Conexiones.Consulta("
                SELECT c.comp_id,
                       c.prov_id,
                       p.prov_nombre,
                       c.usua_id,
                       u.usua_nombre,
                       c.orden_de_compra_ordcom_id AS ordcom_id,
                       c.comp_numerofactura,
                       c.comp_fecha,
                       c.comp_hora,
                       c.comp_total,
                       c.comp_estado
                FROM compra c
                JOIN proveedor p ON p.prov_id = c.prov_id
                LEFT JOIN usuario u ON u.usua_id = c.usua_id
                ORDER BY c.comp_fecha DESC, c.comp_hora DESC, c.comp_id DESC;")
            DataGridView1.DataSource = dt

            If DataGridView1.Columns.Contains("comp_fecha") Then DataGridView1.Columns("comp_fecha").DefaultCellStyle.Format = "dd/MM/yyyy"
            If DataGridView1.Columns.Contains("comp_hora") Then DataGridView1.Columns("comp_hora").DefaultCellStyle.Format = "HH:mm"
            If DataGridView1.Columns.Contains("comp_total") Then DataGridView1.Columns("comp_total").DefaultCellStyle.Format = "N2"
            If DataGridView1.Columns.Contains("prov_id") Then DataGridView1.Columns("prov_id").Visible = False
            If DataGridView1.Columns.Contains("usua_id") Then DataGridView1.Columns("usua_id").Visible = False
            If DataGridView1.Columns.Contains("orden_de_compra_ordcom_id") Then DataGridView1.Columns("orden_de_compra_ordcom_id").Visible = False

            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = Nothing
        Catch ex As Exception
            MostrarError("Error al consultar compras", ex)
        Finally
            _cargando = False
        End Try
    End Sub

    ' ===== Estados UI =====
    Private Sub DeshabilitarInputs()
        txtId.ReadOnly = True
        cboProveedor.Enabled = False
        cboUsuario.Enabled = False
        cboOrden.Enabled = False
        txtNumFactura.ReadOnly = True
        dtpFecha.Enabled = False
        dtpHora.Enabled = False
        cboEstado.Enabled = False
        txtTotal.ReadOnly = True

        BtnGuardar.Enabled = False : BtnCancelar.Enabled = False
        BtnEditar.Enabled = False : BtnEliminar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        cboProveedor.Enabled = True
        cboUsuario.Enabled = True
        cboOrden.Enabled = True
        txtNumFactura.ReadOnly = False
        dtpFecha.Enabled = True
        dtpHora.Enabled = True
        cboEstado.Enabled = True
        txtTotal.ReadOnly = False     ' Si lo vas a calcular por detalle, ponelo True

        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False

        If Not paraEdicion Then
            LimpiarInputs()
            If cboProveedor.Items.Count > 0 Then cboProveedor.SelectedIndex = -1
            If cboUsuario.Items.Count > 0 Then cboUsuario.SelectedIndex = -1
            If cboOrden.Items.Count > 0 Then cboOrden.SelectedIndex = -1
            cboEstado.SelectedIndex = 0 ' PENDIENTE
            dtpFecha.Value = DateTime.Today
            dtpHora.Value = DateTime.Now
            txtTotal.Text = "0,00"
            cboProveedor.Focus()
        End If
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear()
        txtNumFactura.Clear()
        txtTotal.Clear()
        ep.Clear()
    End Sub

    ' ===== Grid -> Form =====
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellClick, DataGridView1.CellContentClick
        If _cargando OrElse e.RowIndex < 0 Then Return
        Dim row = DataGridView1.Rows(e.RowIndex)

        txtId.Text = CStr(TryGetCell(row, {"colId", "comp_id"}))

        Dim vProvId = TryGetCell(row, {"colProvId", "prov_id"})
        If vProvId IsNot Nothing AndAlso vProvId IsNot DBNull.Value Then
            cboProveedor.SelectedValue = CInt(vProvId)
        Else
            SeleccionarProveedorPorNombre(CStr(TryGetCell(row, {"colProveedor", "prov_nombre"})))
        End If

        Dim vUsuId = TryGetCell(row, {"colUsuId", "usua_id"})
        If vUsuId IsNot Nothing AndAlso vUsuId IsNot DBNull.Value Then
            cboUsuario.SelectedValue = CInt(vUsuId)
        Else
            SeleccionarUsuarioPorNombre(CStr(TryGetCell(row, {"colUsuario", "usua_nombre"})))
        End If

        Dim vOrd = TryGetCell(row, {"colOrdCom", "ordcom_id"})
        If vOrd IsNot Nothing AndAlso vOrd IsNot DBNull.Value Then
            cboOrden.SelectedValue = CInt(vOrd)
        Else
            cboOrden.SelectedIndex = -1
        End If

        txtNumFactura.Text = CStr(TryGetCell(row, {"colFactura", "comp_numerofactura"}))

        Dim vFecha = TryGetCell(row, {"colFecha", "comp_fecha"})
        Dim vHora = TryGetCell(row, {"colHora", "comp_hora"})
        If vFecha IsNot Nothing AndAlso vFecha IsNot DBNull.Value Then dtpFecha.Value = CDate(vFecha)
        If vHora IsNot Nothing AndAlso vHora IsNot DBNull.Value Then
            If TypeOf vHora Is TimeSpan Then
                dtpHora.Value = Date.Today.Add(CType(vHora, TimeSpan))
            Else
                dtpHora.Value = CDate(vHora)
            End If
        End If

        Dim vTotal = TryGetCell(row, {"colTotal", "comp_total"})
        If vTotal IsNot Nothing AndAlso vTotal IsNot DBNull.Value Then
            txtTotal.Text = Convert.ToDecimal(vTotal).ToString("N2")
        Else
            txtTotal.Text = ""
        End If

        cboEstado.Text = CStr(TryGetCell(row, {"colEstado", "comp_estado"}))

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

    ' ===== Botones =====
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False)
        lblStatus.Text = "Modo: Nuevo"
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione una compra." : Return
        HabilitarInputs(True)
        lblStatus.Text = "Modo: Editar"
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValProveedor() AndAlso ValEstado() AndAlso ValTotal()) Then
                lblStatus.Text = "Corrija los campos marcados." : Return
            End If

            Dim pProv As New NpgsqlParameter("@prov", NpgsqlDbType.Integer) With {.Value = CInt(cboProveedor.SelectedValue)}
            Dim pUsu As New NpgsqlParameter("@usu", NpgsqlDbType.Integer)
            pUsu.Value = If(cboUsuario.SelectedIndex >= 0, CInt(cboUsuario.SelectedValue), CType(DBNull.Value, Object))
            Dim pOrd As New NpgsqlParameter("@ord", NpgsqlDbType.Integer)
            pOrd.Value = If(cboOrden.SelectedIndex >= 0, CInt(cboOrden.SelectedValue), CType(DBNull.Value, Object))
            Dim pFac As New NpgsqlParameter("@fac", NpgsqlDbType.Varchar)
            pFac.Value = If(String.IsNullOrWhiteSpace(txtNumFactura.Text), CType(DBNull.Value, Object), txtNumFactura.Text.Trim())
            Dim pFec As New NpgsqlParameter("@fec", NpgsqlDbType.Date) With {.Value = dtpFecha.Value.Date}
            Dim pHor As New NpgsqlParameter("@hor", NpgsqlDbType.Time) With {.Value = dtpHora.Value.TimeOfDay}

            Dim totalDec As Decimal
            Decimal.TryParse(NormalizaNumero(txtTotal.Text), NumberStyles.Number, CultureInfo.InvariantCulture, totalDec)
            Dim pTot As New NpgsqlParameter("@tot", NpgsqlDbType.Numeric) With {.Value = totalDec}
            Dim pEst As New NpgsqlParameter("@est", NpgsqlDbType.Varchar) With {.Value = cboEstado.Text}

            If txtId.TextLength = 0 Then
                ' INSERT
                Dim newId = Conexiones.EjecutarsqlScalar("
                    INSERT INTO compra (prov_id, usua_id, orden_de_compra_ordcom_id,
                                        comp_numerofactura, comp_fecha, comp_hora, comp_total, comp_estado)
                    VALUES (@prov, @usu, @ord, @fac, @fec, @hor, @tot, @est)
                    RETURNING comp_id;",
                    New List(Of NpgsqlParameter) From {pProv, pUsu, pOrd, pFac, pFec, pHor, pTot, pEst})
                txtId.Text = CStr(newId)
                lblStatus.Text = $"Insertado ID {txtId.Text}."
            Else
                ' UPDATE
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE compra
                       SET prov_id=@prov,
                           usua_id=@usu,
                           orden_de_compra_ordcom_id=@ord,
                           comp_numerofactura=@fac,
                           comp_fecha=@fec,
                           comp_hora=@hor,
                           comp_total=@tot,
                           comp_estado=@est
                     WHERE comp_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        pProv, pUsu, pOrd, pFac, pFec, pHor, pTot, pEst,
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtId.Text)}
                    })
                lblStatus.Text = If(ok, "Actualizado.", "Sin cambios.")
            End If

            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23503"
            MostrarError("Proveedor/Usuario/Orden inválida o registro referenciado (FK).", ex)
        Catch ex As Exception
            MostrarError("Error al guardar compra", ex)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione una compra." : Return
        If MessageBox.Show("¿Eliminar la compra seleccionada?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM compra WHERE comp_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtId.Text)}
                    })
                If ok Then
                    CargarListado() : DeshabilitarInputs() : lblStatus.Text = "Eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MostrarError("No se puede eliminar: compra referenciada.", ex)
            Catch ex As Exception
                MostrarError("Error al eliminar compra", ex)
            End Try
        End If
    End Sub

    ' ===== Validaciones =====
    Private Function ValProveedor() As Boolean
        If cboProveedor.SelectedIndex < 0 Then ep.SetError(cboProveedor, "Seleccione un proveedor.") : Return False
        ep.SetError(cboProveedor, "") : Return True
    End Function

    Private Function ValEstado() As Boolean
        If String.IsNullOrWhiteSpace(cboEstado.Text) Then ep.SetError(cboEstado, "Seleccione un estado.") : Return False
        ep.SetError(cboEstado, "") : Return True
    End Function

    Private Function ValTotal() As Boolean
        Dim s = NormalizaNumero(txtTotal.Text)
        Dim n As Decimal
        If Not Decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, n) OrElse n < 0D Then
            ep.SetError(txtTotal, "Total numérico ≥ 0 (ej.: 1234,56).") : Return False
        End If
        txtTotal.Text = n.ToString("N2")
        ep.SetError(txtTotal, "") : Return True
    End Function

    Private Function NormalizaNumero(input As String) As String
        Dim s = input.Trim()
        If s.Contains(",") AndAlso Not s.Contains(".") Then s = s.Replace(",", ".")
        Return s
    End Function

    ' ===== Utils =====
    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub
End Class
