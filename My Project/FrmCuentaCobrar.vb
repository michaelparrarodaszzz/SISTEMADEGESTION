Option Strict On
Option Infer On
Imports Npgsql
Imports NpgsqlTypes
Imports System.Globalization

Public Class FrmCuentaCobrar

    ' Cultura Paraguay: miles con punto, decimales con coma
    Private ReadOnly culturaPY As New CultureInfo("es-PY")
    Private ReadOnly decSep As Char = culturaPY.NumberFormat.NumberDecimalSeparator(0)

    Private Sub FrmCuentaCobrar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            CargarVentas()
            CargarClientes()
            CargarEstados()
            CargarListado()
            DeshabilitarInputs()

            ' Validaciones / formateo
            AddHandler cboVenta.SelectedIndexChanged, Sub() ValVenta()
            AddHandler cboCliente.SelectedIndexChanged, Sub() ValCliente()
            AddHandler cboEstado.SelectedIndexChanged, Sub() ValEstado()

            AddHandler txtMonto.KeyPress, AddressOf MontoSaldo_KeyPress
            AddHandler txtMonto.Leave, AddressOf MontoSaldo_Leave
            AddHandler txtSaldo.KeyPress, AddressOf MontoSaldo_KeyPress
            AddHandler txtSaldo.Leave, AddressOf MontoSaldo_Leave

            lblStatus.Text = "Listo."
        Catch ex As Exception
            MostrarError("Error al iniciar", ex)
        End Try
    End Sub

    ' ==================== DATA ====================
    Private Sub CargarListado()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT cxc.cxc_id,
                       cxc.ven_id,
                       cxc.cli_id,
                       cl.cli_nombre,
                       cxc.cxc_fecha,
                       cxc.cxc_hora,
                       cxc.cxc_monto,
                       cxc.cxc_saldo,
                       cxc.cxc_estado,
                       cxc.cxc_observacion
                  FROM cuenta_cobrar cxc
                  JOIN cliente cl ON cl.cli_id = cxc.cli_id
              ORDER BY cxc.cxc_id DESC;")

            DataGridView1.DataSource = dt

            ' Formato numérico
            FormatearColumnaNumerica("colMonto", "cxc_monto")
            FormatearColumnaNumerica("colSaldo", "cxc_saldo")

            ' Ocultar FK técnicas si vienen en el SELECT
            For Each fk In New String() {"cli_id"}
                If DataGridView1.Columns.Contains(fk) Then DataGridView1.Columns(fk).Visible = False
            Next

            DataGridView1.ClearSelection() : DataGridView1.CurrentCell = Nothing
        Catch ex As Exception
            MostrarError("Error al consultar cuentas a cobrar", ex)
        End Try
    End Sub

    Private Sub FormatearColumnaNumerica(nombreColManual As String, nombreColQuery As String)
        Dim col As DataGridViewColumn = Nothing
        If DataGridView1.Columns.Contains(nombreColManual) Then
            col = DataGridView1.Columns(nombreColManual)
        ElseIf DataGridView1.Columns.Contains(nombreColQuery) Then
            col = DataGridView1.Columns(nombreColQuery)
        End If
        If col IsNot Nothing Then
            With col.DefaultCellStyle
                .Format = "N2"
                .FormatProvider = culturaPY
                .Alignment = DataGridViewContentAlignment.MiddleRight
                .NullValue = ""
            End With
        End If
    End Sub

    Private Sub CargarVentas()
        Try
            ' Muestra el ID de la venta (puedes enriquecer el display si quieres)
            Dim dt = Conexiones.Consulta("
                SELECT ven_id
                  FROM venta
              ORDER BY ven_id DESC;")
            cboVenta.DataSource = dt
            cboVenta.ValueMember = "ven_id"
            cboVenta.DisplayMember = "ven_id"
            cboVenta.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Ventas", ex)
        End Try
    End Sub

    Private Sub CargarClientes()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT cli_id,
                       COALESCE(NULLIF(TRIM(cli_nombre||' '||COALESCE(cli_apellido,'')),''),'SIN NOMBRE') AS nombre
                  FROM cliente
              ORDER BY nombre;")
            cboCliente.DataSource = dt
            cboCliente.ValueMember = "cli_id"
            cboCliente.DisplayMember = "nombre"
            cboCliente.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Clientes", ex)
        End Try
    End Sub

    Private Sub CargarEstados()
        If cboEstado.Items.Count = 0 Then
            ' Estados típicos para CxC
            cboEstado.Items.AddRange(New Object() {"PENDIENTE", "PARCIAL", "CANCELADA"})
        End If
        If cboEstado.SelectedIndex < 0 Then cboEstado.SelectedItem = "PENDIENTE"
    End Sub

    ' ==================== ESTADOS UI ====================
    Private Sub DeshabilitarInputs()
        txtId.ReadOnly = True
        cboVenta.Enabled = False
        cboCliente.Enabled = False
        dtpFecha.Enabled = False
        dtpHora.Enabled = False
        txtMonto.ReadOnly = True
        txtSaldo.ReadOnly = True
        cboEstado.Enabled = False
        txtObs.ReadOnly = True

        BtnGuardar.Enabled = False : BtnEditar.Enabled = False
        BtnEliminar.Enabled = False : BtnCancelar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        cboVenta.Enabled = True
        cboCliente.Enabled = True
        dtpFecha.Enabled = True
        dtpHora.Enabled = True
        txtMonto.ReadOnly = False
        txtSaldo.ReadOnly = False
        cboEstado.Enabled = True
        txtObs.ReadOnly = False

        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False

        If Not paraEdicion Then
            LimpiarInputs()
            dtpFecha.Value = Date.Today
            dtpHora.Value = Date.Now
            cboEstado.SelectedItem = "PENDIENTE"
        End If
        cboVenta.Focus()
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear()
        If cboVenta.Items.Count > 0 Then cboVenta.SelectedIndex = -1
        If cboCliente.Items.Count > 0 Then cboCliente.SelectedIndex = -1
        txtMonto.Clear()
        txtSaldo.Clear()
        txtObs.Clear()
        ep.Clear()
    End Sub

    ' ==================== GRID SELECCIÓN ====================
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Return
        Dim r = DataGridView1.Rows(e.RowIndex)

        txtId.Text = CStr(TryGetCell(r, {"colId", "cxc_id"}))

        Dim vVen = TryGetCell(r, {"ven_id"})
        If vVen IsNot Nothing AndAlso vVen IsNot DBNull.Value Then cboVenta.SelectedValue = CInt(vVen)

        Dim vCli = TryGetCell(r, {"cli_id"})
        If vCli IsNot Nothing AndAlso vCli IsNot DBNull.Value Then cboCliente.SelectedValue = CInt(vCli)

        Dim vF = TryGetCell(r, {"colFecha", "cxc_fecha"})
        If vF IsNot Nothing AndAlso vF IsNot DBNull.Value Then dtpFecha.Value = Convert.ToDateTime(vF)

        Dim vH = TryGetCell(r, {"colHora", "cxc_hora"})
        If vH IsNot Nothing AndAlso vH IsNot DBNull.Value Then
            Dim ts As TimeSpan = If(TypeOf vH Is TimeSpan, CType(vH, TimeSpan), TimeSpan.Parse(vH.ToString()))
            dtpHora.Value = Date.Today.Add(ts)
        End If

        Dim vMonto = TryGetCell(r, {"colMonto", "cxc_monto"})
        txtMonto.Text = If(vMonto IsNot Nothing AndAlso vMonto IsNot DBNull.Value,
                           Convert.ToDecimal(vMonto).ToString("N2", culturaPY), "")

        Dim vSaldo = TryGetCell(r, {"colSaldo", "cxc_saldo"})
        txtSaldo.Text = If(vSaldo IsNot Nothing AndAlso vSaldo IsNot DBNull.Value,
                           Convert.ToDecimal(vSaldo).ToString("N2", culturaPY), "")

        Dim vEst = TryGetCell(r, {"colEstado", "cxc_estado"})
        If vEst IsNot Nothing AndAlso vEst IsNot DBNull.Value Then cboEstado.SelectedItem = vEst.ToString()

        txtObs.Text = CStr(TryGetCell(r, {"colObs", "cxc_observacion"}))

        DeshabilitarInputs()
        BtnEditar.Enabled = True : BtnEliminar.Enabled = True : BtnNuevo.Enabled = True
    End Sub

    Private Function TryGetCell(row As DataGridViewRow, names() As String) As Object
        For Each n In names
            If n IsNot Nothing AndAlso DataGridView1.Columns.Contains(n) Then
                Return row.Cells(n).Value
            End If
        Next
        Return Nothing
    End Function

    ' ==================== BOTONES ====================
    Private Sub BtnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False) : lblStatus.Text = "Cuenta a Cobrar - Nuevo"
    End Sub

    Private Sub BtnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        HabilitarInputs(True) : lblStatus.Text = "Cuenta a Cobrar - Editar"
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs() : lblStatus.Text = "Cancelado."
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValVenta() AndAlso ValCliente() AndAlso ValEstado() AndAlso ValMontos()) Then
                lblStatus.Text = "Corrija los campos marcados." : Return
            End If

            Dim mto As Decimal, sal As Decimal
            Decimal.TryParse(txtMonto.Text, NumberStyles.Number, culturaPY, mto)
            Decimal.TryParse(txtSaldo.Text, NumberStyles.Number, culturaPY, sal)

            Dim pars = New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@ven", NpgsqlDbType.Integer) With {.Value = CInt(cboVenta.SelectedValue)},
                New NpgsqlParameter("@cli", NpgsqlDbType.Integer) With {.Value = CInt(cboCliente.SelectedValue)},
                New NpgsqlParameter("@fec", NpgsqlDbType.Date) With {.Value = dtpFecha.Value.Date},
                New NpgsqlParameter("@hor", NpgsqlDbType.Time) With {.Value = dtpHora.Value.TimeOfDay},
                New NpgsqlParameter("@mto", NpgsqlDbType.Numeric) With {.Value = mto},
                New NpgsqlParameter("@sal", NpgsqlDbType.Numeric) With {.Value = sal},
                New NpgsqlParameter("@est", NpgsqlDbType.Varchar) With {.Value = cboEstado.SelectedItem.ToString()},
                New NpgsqlParameter("@obs", NpgsqlDbType.Varchar) With {.Value = If(String.IsNullOrWhiteSpace(txtObs.Text), CType(DBNull.Value, Object), txtObs.Text.Trim())}
            }

            If txtId.TextLength = 0 Then
                Dim id = Conexiones.EjecutarsqlScalar("
                    INSERT INTO cuenta_cobrar (ven_id, cli_id, cxc_fecha, cxc_hora, cxc_monto, cxc_saldo, cxc_estado, cxc_observacion)
                    VALUES (@ven, @cli, @fec, @hor, @mto, @sal, @est, @obs)
                    RETURNING cxc_id;", pars)
                txtId.Text = CStr(id)
                lblStatus.Text = $"Insertado (ID {txtId.Text})."
            Else
                pars.Add(New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = CInt(txtId.Text)})
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE cuenta_cobrar
                       SET ven_id=@ven, cli_id=@cli, cxc_fecha=@fec, cxc_hora=@hor,
                           cxc_monto=@mto, cxc_saldo=@sal, cxc_estado=@est, cxc_observacion=@obs
                     WHERE cxc_id=@id;", pars)
                lblStatus.Text = If(ok, "Actualizado.", "Sin cambios.")
            End If

            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23503"
            MostrarError("Clave foránea inválida (venta/cliente).", ex)
        Catch ex As Exception
            MostrarError("Guardar cuenta a cobrar", ex)
        End Try
    End Sub

    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        If MessageBox.Show("¿Eliminar la cuenta seleccionada?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM cuenta_cobrar WHERE cxc_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = CInt(txtId.Text)}
                    })
                If ok Then
                    CargarListado() : LimpiarInputs() : DeshabilitarInputs()
                    lblStatus.Text = "Registro eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MostrarError("No se puede eliminar: registro referenciado.", ex)
            Catch ex As Exception
                MostrarError("Eliminar cuenta a cobrar", ex)
            End Try
        End If
    End Sub

    ' ==================== VALIDACIONES ====================
    Private Function ValVenta() As Boolean
        If cboVenta.SelectedIndex < 0 Then ep.SetError(cboVenta, "Seleccione una venta.") : Return False
        ep.SetError(cboVenta, "") : Return True
    End Function

    Private Function ValCliente() As Boolean
        If cboCliente.SelectedIndex < 0 Then ep.SetError(cboCliente, "Seleccione un cliente.") : Return False
        ep.SetError(cboCliente, "") : Return True
    End Function

    Private Function ValEstado() As Boolean
        If cboEstado.SelectedIndex < 0 Then ep.SetError(cboEstado, "Seleccione un estado.") : Return False
        ep.SetError(cboEstado, "") : Return True
    End Function

    Private Function ValMontos() As Boolean
        Dim m As Decimal, s As Decimal
        If Not Decimal.TryParse(txtMonto.Text, NumberStyles.Number, culturaPY, m) OrElse m < 0D Then
            ep.SetError(txtMonto, "Monto ≥ 0 (ej.: 123.456,78).") : Return False
        Else
            ep.SetError(txtMonto, "")
        End If
        If Not Decimal.TryParse(txtSaldo.Text, NumberStyles.Number, culturaPY, s) OrElse s < 0D Then
            ep.SetError(txtSaldo, "Saldo ≥ 0 (ej.: 123.456,78).") : Return False
        ElseIf s > m Then
            ep.SetError(txtSaldo, "Saldo no puede superar al Monto.") : Return False
        Else
            ep.SetError(txtSaldo, "")
        End If
        Return True
    End Function

    ' ==================== FORMATEO MONTOS ====================
    Private Sub MontoSaldo_KeyPress(sender As Object, e As KeyPressEventArgs)
        ' Acepta dígitos, controles y un único separador decimal (coma es-PY)
        If Char.IsControl(e.KeyChar) OrElse Char.IsDigit(e.KeyChar) Then Return
        Dim txt = CType(sender, TextBox)
        If e.KeyChar = decSep AndAlso Not txt.Text.Contains(decSep) Then Return
        e.Handled = True
    End Sub

    Private Sub MontoSaldo_Leave(sender As Object, e As EventArgs)
        Dim txt = CType(sender, TextBox)
        Dim n As Decimal
        If Decimal.TryParse(txt.Text, NumberStyles.Number, culturaPY, n) Then
            txt.Text = n.ToString("N2", culturaPY)
        End If
    End Sub

    ' ==================== ERRORES / CIERRE ====================
    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.[Error])
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub

End Class
