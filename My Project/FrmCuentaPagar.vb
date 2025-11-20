Option Strict On
Option Infer On
Imports Npgsql
Imports NpgsqlTypes
Imports System.Globalization

Public Class FrmCuentaPagar

    ' Cultura PY: miles con punto, decimales con coma
    Private ReadOnly culturaPY As New CultureInfo("es-PY")
    Private ReadOnly decSep As Char = culturaPY.NumberFormat.NumberDecimalSeparator(0)

    Private Sub FrmCuentaPagar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            CargarProveedores()
            CargarMonedas()
            CargarOrdenesCompra()
            CargarEstados()
            CargarListado()
            DeshabilitarInputs()

            ' Validaciones y formato
            AddHandler cboProveedor.SelectedIndexChanged, Sub() ValProveedor()
            AddHandler cboMoneda.SelectedIndexChanged, Sub() ValMoneda()
            AddHandler cboEstado.SelectedIndexChanged, Sub() ValEstado()
            AddHandler txtMonto.KeyPress, AddressOf Monto_KeyPress
            AddHandler txtMonto.Leave, AddressOf Monto_Leave

            lblStatus.Text = "Listo."
        Catch ex As Exception
            MostrarError("Error al iniciar", ex)
        End Try
    End Sub

    ' ================== DATA ==================
    Private Sub CargarListado()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT cp.capag_id,
                       cp.prov_id,
                       p.prov_nombre,
                       cp.moneda_mon_id,
                       m.mon_descripcion,
                       cp.orden_de_compra_ordcom_id,
                       oc.ordcom_id,
                       cp.capag_documento,
                       cp.capag_monto,
                       cp.capag_vencimiento,
                       cp.capag_estado
                  FROM cuenta_a_pagar cp
                  JOIN proveedor p ON p.prov_id = cp.prov_id
                  LEFT JOIN moneda m ON m.mon_id = cp.moneda_mon_id
                  LEFT JOIN orden_de_compra oc ON oc.ordcom_id = cp.orden_de_compra_ordcom_id
              ORDER BY cp.capag_vencimiento NULLS LAST, cp.capag_id;")

            DataGridView1.DataSource = dt

            ' Formato de monto
            Dim colMonto As DataGridViewColumn = Nothing
            If DataGridView1.Columns.Contains("colMonto") Then
                colMonto = DataGridView1.Columns("colMonto")
            ElseIf DataGridView1.Columns.Contains("capag_monto") Then
                colMonto = DataGridView1.Columns("capag_monto")
            End If
            If colMonto IsNot Nothing Then
                With colMonto.DefaultCellStyle
                    .Format = "N2"
                    .FormatProvider = culturaPY
                    .Alignment = DataGridViewContentAlignment.MiddleRight
                    .NullValue = ""
                End With
            End If

            ' Ocultar FKs técnicas si están
            For Each fk In New String() {"prov_id", "moneda_mon_id", "orden_de_compra_ordcom_id"}
                If DataGridView1.Columns.Contains(fk) Then
                    DataGridView1.Columns(fk).Visible = False
                End If
            Next

            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = Nothing

        Catch ex As Exception
            MostrarError("Error al consultar cuentas a pagar", ex)
        End Try
    End Sub

    Private Sub CargarProveedores()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT prov_id,
                       COALESCE(NULLIF(TRIM(prov_nombre),''),'SIN NOMBRE') AS nombre
                  FROM proveedor
              ORDER BY nombre;")
            cboProveedor.DataSource = dt
            cboProveedor.ValueMember = "prov_id"
            cboProveedor.DisplayMember = "nombre"
            cboProveedor.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Error al cargar proveedores", ex)
        End Try
    End Sub

    Private Sub CargarMonedas()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT mon_id, mon_descripcion
                  FROM moneda
              ORDER BY mon_descripcion;")
            cboMoneda.DataSource = dt
            cboMoneda.ValueMember = "mon_id"
            cboMoneda.DisplayMember = "mon_descripcion"
            cboMoneda.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Error al cargar monedas", ex)
        End Try
    End Sub

    Private Sub CargarOrdenesCompra()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT ordcom_id
                  FROM orden_de_compra
              ORDER BY ordcom_id;")
            cboOrdenCompra.DataSource = dt
            cboOrdenCompra.ValueMember = "ordcom_id"
            cboOrdenCompra.DisplayMember = "ordcom_id"
            cboOrdenCompra.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Error al cargar órdenes de compra", ex)
        End Try
    End Sub

    Private Sub CargarEstados()
        If cboEstado.Items.Count = 0 Then
            cboEstado.Items.AddRange(New Object() {"PENDIENTE", "PAGADO", "VENCIDO"})
        End If
        If cboEstado.SelectedIndex < 0 Then
            cboEstado.SelectedItem = "PENDIENTE"
        End If
    End Sub

    ' ================== ESTADOS UI ==================
    Private Sub DeshabilitarInputs()
        txtId.ReadOnly = True
        cboProveedor.Enabled = False
        cboMoneda.Enabled = False
        cboOrdenCompra.Enabled = False
        txtDocumento.ReadOnly = True
        txtMonto.ReadOnly = True
        dtpVencimiento.Enabled = False
        cboEstado.Enabled = False

        BtnGuardar.Enabled = False
        BtnCancelar.Enabled = False
        BtnEditar.Enabled = False
        BtnEliminar.Enabled = False
        BtnNuevo.Enabled = True

        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        cboProveedor.Enabled = True
        cboMoneda.Enabled = True
        cboOrdenCompra.Enabled = True
        txtDocumento.ReadOnly = False
        txtMonto.ReadOnly = False
        dtpVencimiento.Enabled = True
        cboEstado.Enabled = True

        BtnGuardar.Enabled = True
        BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEditar.Enabled = False
        BtnEliminar.Enabled = paraEdicion

        If Not paraEdicion Then
            LimpiarInputs()
            dtpVencimiento.Value = Date.Today
            cboEstado.SelectedItem = "PENDIENTE"
        End If

        cboProveedor.Focus()
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear()
        txtDocumento.Clear()
        txtMonto.Clear()

        If cboProveedor.Items.Count > 0 Then cboProveedor.SelectedIndex = -1
        If cboMoneda.Items.Count > 0 Then cboMoneda.SelectedIndex = -1
        If cboOrdenCompra.Items.Count > 0 Then cboOrdenCompra.SelectedIndex = -1

        ep.Clear()
    End Sub

    ' ================== GRID SELECCIÓN ==================
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellClick

        If e.RowIndex < 0 Then Return
        Dim row = DataGridView1.Rows(e.RowIndex)

        txtId.Text = Convert.ToString(TryGetCell(row, {"colId", "capag_id"}))

        Dim vProv = TryGetCell(row, {"prov_id"})
        If vProv IsNot Nothing AndAlso vProv IsNot DBNull.Value Then
            cboProveedor.SelectedValue = CInt(vProv)
        End If

        Dim vMon = TryGetCell(row, {"moneda_mon_id"})
        If vMon IsNot Nothing AndAlso vMon IsNot DBNull.Value Then
            cboMoneda.SelectedValue = CInt(vMon)
        Else
            cboMoneda.SelectedIndex = -1
        End If

        Dim vOc = TryGetCell(row, {"orden_de_compra_ordcom_id"})
        If vOc IsNot Nothing AndAlso vOc IsNot DBNull.Value Then
            cboOrdenCompra.SelectedValue = CInt(vOc)
        Else
            cboOrdenCompra.SelectedIndex = -1
        End If

        txtDocumento.Text = Convert.ToString(TryGetCell(row, {"colDocumento", "capag_documento"}))

        Dim vMonto = TryGetCell(row, {"colMonto", "capag_monto"})
        If vMonto IsNot Nothing AndAlso vMonto IsNot DBNull.Value Then
            Dim m = Convert.ToDecimal(vMonto)
            txtMonto.Text = m.ToString("N2", culturaPY)
        Else
            txtMonto.Text = ""
        End If

        Dim vVenc = TryGetCell(row, {"colVencimiento", "capag_vencimiento"})
        If vVenc IsNot Nothing AndAlso vVenc IsNot DBNull.Value Then
            dtpVencimiento.Value = Convert.ToDateTime(vVenc)
        End If

        Dim vEst = TryGetCell(row, {"colEstado", "capag_estado"})
        If vEst IsNot Nothing AndAlso vEst IsNot DBNull.Value Then
            cboEstado.SelectedItem = vEst.ToString()
        End If

        DeshabilitarInputs()
        BtnEditar.Enabled = True
        BtnEliminar.Enabled = True
        BtnNuevo.Enabled = True
    End Sub

    Private Function TryGetCell(row As DataGridViewRow, names() As String) As Object
        For Each n In names
            If n IsNot Nothing AndAlso DataGridView1.Columns.Contains(n) Then
                Return row.Cells(n).Value
            End If
        Next
        Return Nothing
    End Function

    ' ================== BOTONES ==================
    Private Sub BtnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False)
        lblStatus.Text = "Cuenta a pagar - Nuevo"
    End Sub

    Private Sub BtnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtId.TextLength = 0 Then
            lblStatus.Text = "Seleccione una cuenta a pagar."
            Return
        End If
        HabilitarInputs(True)
        lblStatus.Text = "Cuenta a pagar - Editar"
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValProveedor() AndAlso ValMoneda() AndAlso ValMonto() AndAlso ValEstado()) Then
                lblStatus.Text = "Corrija los campos marcados."
                Return
            End If

            Dim monto As Decimal
            Decimal.TryParse(txtMonto.Text, NumberStyles.Number, culturaPY, monto)

            Dim pProv As New NpgsqlParameter("@prov", NpgsqlDbType.Integer) With {.Value = CInt(cboProveedor.SelectedValue)}
            Dim pMon As New NpgsqlParameter("@mon", NpgsqlDbType.Integer)
            If cboMoneda.SelectedIndex >= 0 Then
                pMon.Value = CInt(cboMoneda.SelectedValue)
            Else
                pMon.Value = DBNull.Value
            End If

            Dim pOc As New NpgsqlParameter("@oc", NpgsqlDbType.Integer)
            If cboOrdenCompra.SelectedIndex >= 0 Then
                pOc.Value = CInt(cboOrdenCompra.SelectedValue)
            Else
                pOc.Value = DBNull.Value
            End If

            Dim pDoc As New NpgsqlParameter("@doc", NpgsqlDbType.Varchar)
            pDoc.Value = If(String.IsNullOrWhiteSpace(txtDocumento.Text),
                            CType(DBNull.Value, Object),
                            txtDocumento.Text.Trim())

            Dim pMonto As New NpgsqlParameter("@monto", NpgsqlDbType.Numeric) With {.Value = monto}
            Dim pVenc As New NpgsqlParameter("@venc", NpgsqlDbType.Date) With {.Value = dtpVencimiento.Value.Date}
            Dim pEst As New NpgsqlParameter("@est", NpgsqlDbType.Varchar) With {.Value = cboEstado.SelectedItem.ToString()}

            Dim pars = New List(Of NpgsqlParameter) From {pProv, pMon, pOc, pDoc, pMonto, pVenc, pEst}

            If txtId.TextLength = 0 Then
                ' INSERT
                Dim id = Conexiones.EjecutarsqlScalar("
                    INSERT INTO cuenta_a_pagar
                        (prov_id, moneda_mon_id, orden_de_compra_ordcom_id,
                         capag_documento, capag_monto, capag_vencimiento, capag_estado)
                    VALUES
                        (@prov, @mon, @oc, @doc, @monto, @venc, @est)
                    RETURNING capag_id;", pars)

                txtId.Text = CStr(id)
                lblStatus.Text = $"Cuenta a pagar insertada (ID {txtId.Text})."
            Else
                ' UPDATE
                pars.Add(New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = CInt(txtId.Text)})
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE cuenta_a_pagar
                       SET prov_id=@prov,
                           moneda_mon_id=@mon,
                           orden_de_compra_ordcom_id=@oc,
                           capag_documento=@doc,
                           capag_monto=@monto,
                           capag_vencimiento=@venc,
                           capag_estado=@est
                     WHERE capag_id=@id;", pars)
                lblStatus.Text = If(ok, "Cuenta a pagar actualizada.", "Sin cambios.")
            End If

            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23503"
            MostrarError("Clave foránea inválida (proveedor/moneda/OC).", ex)
        Catch ex As Exception
            MostrarError("Error al guardar cuenta a pagar", ex)
        End Try
    End Sub

    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then
            lblStatus.Text = "Seleccione una cuenta a pagar."
            Return
        End If

        If MessageBox.Show("¿Eliminar la cuenta a pagar seleccionada?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM cuenta_a_pagar WHERE capag_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = CInt(txtId.Text)}
                    })
                If ok Then
                    CargarListado()
                    LimpiarInputs()
                    DeshabilitarInputs()
                    lblStatus.Text = "Cuenta a pagar eliminada."
                Else
                    lblStatus.Text = "No se eliminó el registro."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MostrarError("No se puede eliminar: cuenta a pagar referenciada.", ex)
            Catch ex As Exception
                MostrarError("Error al eliminar cuenta a pagar", ex)
            End Try
        End If
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub

    ' ================== VALIDACIONES ==================
    Private Function ValProveedor() As Boolean
        If cboProveedor.SelectedIndex < 0 Then
            ep.SetError(cboProveedor, "Seleccione un proveedor.")
            Return False
        End If
        ep.SetError(cboProveedor, "")
        Return True
    End Function

    Private Function ValMoneda() As Boolean
        ' Si querés que sea obligatorio: descomentar estas líneas:
        'If cboMoneda.SelectedIndex < 0 Then
        '    ep.SetError(cboMoneda, "Seleccione una moneda.")
        '    Return False
        'End If
        ep.SetError(cboMoneda, "")
        Return True
    End Function

    Private Function ValEstado() As Boolean
        If cboEstado.SelectedIndex < 0 Then
            ep.SetError(cboEstado, "Seleccione un estado.")
            Return False
        End If
        ep.SetError(cboEstado, "")
        Return True
    End Function

    Private Function ValMonto() As Boolean
        Dim n As Decimal
        If Not Decimal.TryParse(txtMonto.Text, NumberStyles.Number, culturaPY, n) OrElse n < 0D Then
            ep.SetError(txtMonto, "Monto ≥ 0, ej.: 123.456,78")
            Return False
        End If
        ep.SetError(txtMonto, "")
        Return True
    End Function

    ' ================== Monto helpers ==================
    Private Sub Monto_KeyPress(sender As Object, e As KeyPressEventArgs)
        Dim txt = CType(sender, TextBox)

        If Char.IsControl(e.KeyChar) OrElse Char.IsDigit(e.KeyChar) Then
            Return
        End If

        ' Solo permitir un separador decimal
        If e.KeyChar = decSep AndAlso Not txt.Text.Contains(decSep) Then
            Return
        End If

        e.Handled = True
    End Sub

    Private Sub Monto_Leave(sender As Object, e As EventArgs)
        Dim txt = CType(sender, TextBox)
        Dim m As Decimal
        If Decimal.TryParse(txt.Text, NumberStyles.Number, culturaPY, m) Then
            txt.Text = m.ToString("N2", culturaPY)
        End If
    End Sub

    ' ================== ERRORES ==================
    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

End Class
