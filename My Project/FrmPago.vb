Option Strict On
Option Infer On
Imports Npgsql
Imports NpgsqlTypes
Imports System.Globalization

Public Class FrmPago

    Private Sub FrmPago_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Configuración de columnas: si ya existen en el diseñador, solo mapeo DataPropertyName

        CargarProveedores()
        CargarOrdenesPago()
        CargarUsuarios()
        CargarListado()

        Pago_Disable()

        ' Validación en tiempo real
        AddHandler txtMonto.TextChanged, Sub() Pago_ValMonto()

        lblStatus.Text = "Listo."
    End Sub

    ' ========== Utilitarios ==========
    Private Function TryGetCell(dgv As DataGridView, row As DataGridViewRow, names() As String) As Object
        For Each n In names
            If n IsNot Nothing AndAlso dgv.Columns.Contains(n) Then Return row.Cells(n).Value
        Next
        Return Nothing
    End Function

    Private Function NormalizaNumero(input As String) As String
        Dim s = input.Trim()
        If s.Contains(",") AndAlso Not s.Contains(".") Then s = s.Replace(",", ".")
        Return s
    End Function

    Private Sub ShowErr(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    ' ========== Data ==========
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

    Private Sub CargarOrdenesPago()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT ordpag_id
                  FROM orden_de_pago
              ORDER BY ordpag_id DESC;")
            cboOrdPag.DataSource = dt
            cboOrdPag.ValueMember = "ordpag_id"
            cboOrdPag.DisplayMember = "ordpag_id"
            cboOrdPag.SelectedIndex = -1 ' es opcional
        Catch ex As Exception
            ShowErr("Cargar órdenes de pago", ex)
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
                SELECT p.pag_id,
                       p.prov_id, pr.prov_nombre,
                       p.orden_de_pago_ordpag_id AS ordpag_id,
                       p.usua_id, u.usua_nombre,
                       p.pag_fecha, p.pag_monto, p.pag_obs
                  FROM pago p
                  LEFT JOIN proveedor pr ON pr.prov_id = p.prov_id
                  LEFT JOIN usuario   u  ON u.usua_id = p.usua_id
              ORDER BY p.pag_fecha DESC, p.pag_id DESC;")
            dgvPago.DataSource = dt
            dgvPago.ClearSelection()
            dgvPago.CurrentCell = Nothing
        Catch ex As Exception
            ShowErr("Cargar pagos", ex)
        End Try
    End Sub

    ' ========== Estados ==========
    Private Sub Pago_Disable()
        txtPagId.ReadOnly = True
        cboProv.Enabled = False
        cboOrdPag.Enabled = False
        cboUsuario.Enabled = False
        dtpFecha.Enabled = False
        txtMonto.ReadOnly = True
        txtObs.ReadOnly = True

        BtnPagGuardar.Enabled = False
        BtnPagCancelar.Enabled = False
        BtnPagEditar.Enabled = False
        BtnPagEliminar.Enabled = False
        BtnPagNuevo.Enabled = True

        ep.Clear()
    End Sub

    Private Sub Pago_Enable(editing As Boolean)
        cboProv.Enabled = True
        cboOrdPag.Enabled = True   ' opcional
        cboUsuario.Enabled = True
        dtpFecha.Enabled = True
        txtMonto.ReadOnly = False
        txtObs.ReadOnly = False

        BtnPagGuardar.Enabled = True
        BtnPagCancelar.Enabled = True
        BtnPagNuevo.Enabled = False
        BtnPagEliminar.Enabled = editing
        BtnPagEditar.Enabled = False

        If Not editing Then
            txtPagId.Clear()
            txtMonto.Clear()
            txtObs.Clear()
            If cboProv.Items.Count > 0 Then cboProv.SelectedIndex = -1
            If cboOrdPag.Items.Count > 0 Then cboOrdPag.SelectedIndex = -1
            If cboUsuario.Items.Count > 0 Then cboUsuario.SelectedIndex = -1
            dtpFecha.Value = Date.Today
        End If

        cboProv.Focus()
    End Sub

    ' ========== Grilla selección ==========
    Private Sub dgvPago_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPago.CellClick
        If e.RowIndex < 0 Then Return
        Dim r = dgvPago.Rows(e.RowIndex)

        txtPagId.Text = Convert.ToString(TryGetCell(dgvPago, r, {"colId", "pag_id"}))
        txtMonto.Text = Convert.ToDecimal(TryGetCell(dgvPago, r, {"colMonto", "pag_monto"})).ToString("0.00")
        txtObs.Text = Convert.ToString(TryGetCell(dgvPago, r, {"colObs", "pag_obs"}))

        Dim vProvId = TryGetCell(dgvPago, r, {"colProvId", "prov_id"})
        If vProvId IsNot Nothing AndAlso vProvId IsNot DBNull.Value Then cboProv.SelectedValue = CInt(vProvId)

        Dim vOrd = TryGetCell(dgvPago, r, {"colOrdPagId", "ordpag_id"})
        If vOrd IsNot Nothing AndAlso vOrd IsNot DBNull.Value Then
            ' puede ser Null en DB; si no existe en combo, lo dejamos en -1
            Dim id As Integer = CInt(vOrd)
            Dim found As Boolean = False
            For i = 0 To cboOrdPag.Items.Count - 1
                Dim drv = TryCast(cboOrdPag.Items(i), DataRowView)
                If drv IsNot Nothing AndAlso CInt(drv("ordpag_id")) = id Then
                    cboOrdPag.SelectedIndex = i : found = True : Exit For
                End If
            Next
            If Not found Then cboOrdPag.SelectedIndex = -1
        Else
            cboOrdPag.SelectedIndex = -1
        End If

        Dim vUsuId = TryGetCell(dgvPago, r, {"colUsuaId", "usua_id"})
        If vUsuId IsNot Nothing AndAlso vUsuId IsNot DBNull.Value Then cboUsuario.SelectedValue = CInt(vUsuId)

        Dim vFec = TryGetCell(dgvPago, r, {"colFecha", "pag_fecha"})
        If vFec IsNot Nothing AndAlso vFec IsNot DBNull.Value Then dtpFecha.Value = CDate(vFec)

        Pago_Disable()
        BtnPagEditar.Enabled = True
        BtnPagEliminar.Enabled = True
    End Sub

    ' ========== Botones ==========
    Private Sub btnPagNuevo_Click(sender As Object, e As EventArgs) Handles BtnPagNuevo.Click
        Pago_Enable(False)
        lblStatus.Text = "Pago - Nuevo"
    End Sub

    Private Sub btnPagEditar_Click(sender As Object, e As EventArgs) Handles BtnPagEditar.Click
        If txtPagId.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        Pago_Enable(True)
        lblStatus.Text = "Pago - Editar"
    End Sub

    Private Sub btnPagCancelar_Click(sender As Object, e As EventArgs) Handles BtnPagCancelar.Click
        Pago_Disable()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub btnPagGuardar_Click(sender As Object, e As EventArgs) Handles BtnPagGuardar.Click
        Try
            If Not (Pago_ValProv() AndAlso Pago_ValUsuario() AndAlso Pago_ValMonto()) Then
                lblStatus.Text = "Corrija los campos marcados."
                Return
            End If

            Dim monto As Decimal
            Decimal.TryParse(NormalizaNumero(txtMonto.Text), NumberStyles.Number, CultureInfo.InvariantCulture, monto)

            Dim pProv As New NpgsqlParameter("@prov", NpgsqlDbType.Integer) With {.Value = CInt(cboProv.SelectedValue)}
            Dim pUsu As New NpgsqlParameter("@usu", NpgsqlDbType.Integer) With {.Value = CInt(cboUsuario.SelectedValue)}
            Dim pFec As New NpgsqlParameter("@fec", NpgsqlDbType.Date) With {.Value = dtpFecha.Value.Date}
            Dim pMon As New NpgsqlParameter("@mon", NpgsqlDbType.Numeric) With {.Value = monto}
            Dim pObs As New NpgsqlParameter("@obs", NpgsqlDbType.Varchar)
            pObs.Value = If(String.IsNullOrWhiteSpace(txtObs.Text), CType(DBNull.Value, Object), txtObs.Text.Trim())

            Dim pOrd As New NpgsqlParameter("@ord", NpgsqlDbType.Integer)
            If cboOrdPag.SelectedIndex >= 0 Then
                pOrd.Value = CInt(cboOrdPag.SelectedValue)
            Else
                pOrd.Value = DBNull.Value
            End If

            If txtPagId.TextLength = 0 Then
                Dim id = Conexiones.EjecutarsqlScalar("
                    INSERT INTO pago(prov_id, orden_de_pago_ordpag_id, usua_id, pag_fecha, pag_monto, pag_obs)
                    VALUES(@prov, @ord, @usu, @fec, @mon, @obs)
                    RETURNING pag_id;",
                    New List(Of NpgsqlParameter) From {pProv, pOrd, pUsu, pFec, pMon, pObs})
                txtPagId.Text = CStr(id)
                lblStatus.Text = $"Pago insertado (ID {txtPagId.Text})."
            Else
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE pago
                       SET prov_id=@prov,
                           orden_de_pago_ordpag_id=@ord,
                           usua_id=@usu,
                           pag_fecha=@fec,
                           pag_monto=@mon,
                           pag_obs=@obs
                     WHERE pag_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        pProv, pOrd, pUsu, pFec, pMon, pObs,
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtPagId.Text)}
                    })
                lblStatus.Text = If(ok, "Pago actualizado.", "Sin cambios.")
            End If

            CargarListado()
            Pago_Disable()

        Catch ex As PostgresException When ex.SqlState = "23505"
            ShowErr("Registro duplicado", ex)
        Catch ex As Exception
            ShowErr("Guardar pago", ex)
        End Try
    End Sub

    Private Sub btnPagEliminar_Click(sender As Object, e As EventArgs) Handles BtnPagEliminar.Click
        If txtPagId.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        If MessageBox.Show("¿Eliminar el pago seleccionado?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM pago WHERE pag_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtPagId.Text)}
                    })
                If ok Then
                    CargarListado()
                    Pago_Disable()
                    lblStatus.Text = "Pago eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                ShowErr("No se puede eliminar: pago referenciado.", ex)
            Catch ex As Exception
                ShowErr("Eliminar pago", ex)
            End Try
        End If
    End Sub

    ' ========== Validaciones ==========
    Private Function Pago_ValProv() As Boolean
        If cboProv.SelectedIndex < 0 Then ep.SetError(cboProv, "Seleccione un proveedor.") : Return False
        ep.SetError(cboProv, "") : Return True
    End Function

    Private Function Pago_ValUsuario() As Boolean
        If cboUsuario.SelectedIndex < 0 Then ep.SetError(cboUsuario, "Seleccione un usuario.") : Return False
        ep.SetError(cboUsuario, "") : Return True
    End Function

    Private Function Pago_ValMonto() As Boolean
        Dim s = NormalizaNumero(txtMonto.Text)
        Dim n As Decimal
        If Not Decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, n) OrElse n < 0D Then
            ep.SetError(txtMonto, "Monto ≥ 0 (ej.: 1234,56).") : Return False
        End If
        txtMonto.Text = n.ToString("0.00")
        ep.SetError(txtMonto, "") : Return True
    End Function

    Private Sub dvgPago_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPago.CellContentClick

    End Sub

    Private Sub BtnPagCerrar_Click(sender As Object, e As EventArgs) Handles BtnPagCerrar.Click
        Me.Close()
    End Sub
End Class
