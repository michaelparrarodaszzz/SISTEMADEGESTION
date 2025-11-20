Option Strict On
Option Infer On
Imports Npgsql
Imports NpgsqlTypes
Imports System.Globalization

Public Class FrmCobro

    ' Cultura Paraguay: miles con punto, decimales con coma
    Private ReadOnly culturaPY As New CultureInfo("es-PY")
    Private ReadOnly decSep As Char = culturaPY.NumberFormat.NumberDecimalSeparator(0)

    Private Sub FrmCobro_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            CargarClientes()
            CargarTiposCobro()
            CargarUsuarios()
            CargarListado()
            DeshabilitarInputs()

            ' Validaciones simples / formateo
            AddHandler cboCliente.SelectedIndexChanged, Sub() ValCliente()
            AddHandler cboCobroTipo.SelectedIndexChanged, Sub() ValTipo()
            AddHandler cboUsuario.SelectedIndexChanged, Sub() ValUsuario()
            AddHandler txtMonto.KeyPress, AddressOf Monto_KeyPress
            AddHandler txtMonto.Leave, AddressOf Monto_Leave

            If dtpFecha IsNot Nothing Then dtpFecha.Value = Date.Today
            If dtpHora IsNot Nothing Then dtpHora.Value = Date.Now

            lblStatus.Text = "Listo."
        Catch ex As Exception
            MostrarError("Error al iniciar", ex)
        End Try
    End Sub

    ' ================= DATA =================
    Private Sub CargarListado()
        Try
            ' *** Importante: sin mon_id / caj_id porque NO existen en tu tabla ***
            Dim dt = Conexiones.Consulta("
                SELECT cb.cob_id,
                       cb.ven_id,
                       cb.cobro_tipo_id,
                       tp.cobtp_nombre,
                       cb.cliente_cli_id,
                       cl.cli_nombre,
                       cb.usua_id,
                       u.usua_usuario,
                       cb.cob_fecha,
                       cb.cob_hora,
                       cb.cob_monto
                  FROM cobro cb
                  JOIN cobro_tipo tp   ON tp.cobtp_id    = cb.cobro_tipo_id
                  JOIN cliente cl      ON cl.cli_id      = cb.cliente_cli_id
                  JOIN usuario u       ON u.usua_id      = cb.usua_id
              ORDER BY cb.cob_id DESC;")

            DataGridView1.DataSource = dt

            ' Formato del monto (funciona con columna manual o con nombre de la query)
            Dim colMonto As DataGridViewColumn = Nothing
            If DataGridView1.Columns.Contains("colMonto") Then
                colMonto = DataGridView1.Columns("colMonto")
            ElseIf DataGridView1.Columns.Contains("cob_monto") Then
                colMonto = DataGridView1.Columns("cob_monto")
            End If
            If colMonto IsNot Nothing Then
                With colMonto.DefaultCellStyle
                    .Format = "N2"
                    .FormatProvider = culturaPY
                    .Alignment = DataGridViewContentAlignment.MiddleRight
                    .NullValue = ""
                End With
            End If

            ' Ocultar FKs técnicas si están en el SELECT
            For Each fk In New String() {"cobro_tipo_id", "cliente_cli_id", "usua_id"}
                If DataGridView1.Columns.Contains(fk) Then DataGridView1.Columns(fk).Visible = False
            Next
        Catch ex As Exception
            MostrarError("Error al consultar cobros", ex)
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

    Private Sub CargarTiposCobro()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT cobtp_id, cobtp_nombre
                  FROM cobro_tipo
              ORDER BY cobtp_nombre;")
            cboCobroTipo.DataSource = dt
            cboCobroTipo.ValueMember = "cobtp_id"
            cboCobroTipo.DisplayMember = "cobtp_nombre"
            cboCobroTipo.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Tipos de cobro", ex)
        End Try
    End Sub

    Private Sub CargarUsuarios()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT usua_id, COALESCE(usua_usuario,'(sin usuario)') AS usuario
                  FROM usuario
              ORDER BY usuario;")
            cboUsuario.DataSource = dt
            cboUsuario.ValueMember = "usua_id"
            cboUsuario.DisplayMember = "usuario"
            cboUsuario.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Usuarios", ex)
        End Try
    End Sub

    ' ============== HABILITAR / DESHABILITAR ==============
    Private Sub DeshabilitarInputs()
        txtId.ReadOnly = True
        cboCliente.Enabled = False
        cboCobroTipo.Enabled = False
        cboUsuario.Enabled = False
        dtpFecha.Enabled = False
        dtpHora.Enabled = False
        txtMonto.ReadOnly = True

        BtnGuardar.Enabled = False : BtnEditar.Enabled = False
        BtnEliminar.Enabled = False : BtnCancelar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        cboCliente.Enabled = True
        cboCobroTipo.Enabled = True
        cboUsuario.Enabled = True
        dtpFecha.Enabled = True
        dtpHora.Enabled = True
        txtMonto.ReadOnly = False

        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False

        If Not paraEdicion Then
            LimpiarInputs()
            dtpFecha.Value = Date.Today
            dtpHora.Value = Date.Now
        End If
        cboCliente.Focus()
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear()
        If cboCliente.Items.Count > 0 Then cboCliente.SelectedIndex = -1
        If cboCobroTipo.Items.Count > 0 Then cboCobroTipo.SelectedIndex = -1
        If cboUsuario.Items.Count > 0 Then cboUsuario.SelectedIndex = -1
        txtMonto.Clear()
        ep.Clear()
    End Sub

    ' ================= GRID SELECCIÓN =================
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellClick

        If e.RowIndex < 0 Then Return
        Dim r = DataGridView1.Rows(e.RowIndex)

        txtId.Text = CStr(TryGetCell(r, {"colId", "cob_id"}))

        Dim vCli = TryGetCell(r, {"cliente_cli_id"})
        If vCli IsNot Nothing AndAlso vCli IsNot DBNull.Value Then cboCliente.SelectedValue = CInt(vCli)

        Dim vTp = TryGetCell(r, {"cobro_tipo_id"})
        If vTp IsNot Nothing AndAlso vTp IsNot DBNull.Value Then cboCobroTipo.SelectedValue = CInt(vTp)

        Dim vUsr = TryGetCell(r, {"usua_id"})
        If vUsr IsNot Nothing AndAlso vUsr IsNot DBNull.Value Then cboUsuario.SelectedValue = CInt(vUsr)

        Dim vF = TryGetCell(r, {"colFecha", "cob_fecha"})
        If vF IsNot Nothing AndAlso vF IsNot DBNull.Value Then dtpFecha.Value = Convert.ToDateTime(vF)

        Dim vH = TryGetCell(r, {"colHora", "cob_hora"})
        If vH IsNot Nothing AndAlso vH IsNot DBNull.Value Then
            Dim ts As TimeSpan = If(TypeOf vH Is TimeSpan, CType(vH, TimeSpan), TimeSpan.Parse(vH.ToString()))
            dtpHora.Value = Date.Today.Add(ts)
        End If

        Dim vMonto = TryGetCell(r, {"colMonto", "cob_monto"})
        txtMonto.Text = If(vMonto IsNot Nothing AndAlso vMonto IsNot DBNull.Value,
                           Convert.ToDecimal(vMonto).ToString("N2", culturaPY), "")

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

    ' ================= BOTONES =================
    Private Sub BtnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False) : lblStatus.Text = "Cobro - Nuevo"
    End Sub

    Private Sub BtnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un cobro." : Return
        HabilitarInputs(True) : lblStatus.Text = "Cobro - Editar"
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs() : lblStatus.Text = "Cancelado."
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValCliente() AndAlso ValTipo() AndAlso ValUsuario() AndAlso ValMonto()) Then
                lblStatus.Text = "Corrija los campos marcados." : Return
            End If

            Dim monto As Decimal
            Decimal.TryParse(txtMonto.Text, NumberStyles.Number, culturaPY, monto)

            ' ven_id es opcional en tu tabla; si no tenés un selector, guardamos NULL
            Dim pVen As New NpgsqlParameter("@ven", NpgsqlDbType.Integer) With {.Value = DBNull.Value}

            Dim pars = New List(Of NpgsqlParameter) From {
                pVen,
                New NpgsqlParameter("@tp", NpgsqlDbType.Integer) With {.Value = CInt(cboCobroTipo.SelectedValue)},
                New NpgsqlParameter("@cli", NpgsqlDbType.Integer) With {.Value = CInt(cboCliente.SelectedValue)},
                New NpgsqlParameter("@usr", NpgsqlDbType.Integer) With {.Value = CInt(cboUsuario.SelectedValue)},
                New NpgsqlParameter("@fec", NpgsqlDbType.Date) With {.Value = dtpFecha.Value.Date},
                New NpgsqlParameter("@hor", NpgsqlDbType.Time) With {.Value = dtpHora.Value.TimeOfDay},
                New NpgsqlParameter("@mto", NpgsqlDbType.Numeric) With {.Value = monto}
            }

            If txtId.TextLength = 0 Then
                Dim id = Conexiones.EjecutarsqlScalar("
                    INSERT INTO cobro (ven_id, cobro_tipo_id, cliente_cli_id, usua_id, cob_fecha, cob_hora, cob_monto)
                    VALUES (@ven, @tp, @cli, @usr, @fec, @hor, @mto)
                    RETURNING cob_id;", pars)
                txtId.Text = CStr(id)
                lblStatus.Text = $"Cobro insertado (ID {txtId.Text})."
            Else
                pars.Add(New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = CInt(txtId.Text)})
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE cobro
                       SET ven_id=@ven,
                           cobro_tipo_id=@tp,
                           cliente_cli_id=@cli,
                           usua_id=@usr,
                           cob_fecha=@fec,
                           cob_hora=@hor,
                           cob_monto=@mto
                     WHERE cob_id=@id;", pars)
                lblStatus.Text = If(ok, "Cobro actualizado.", "Sin cambios.")
            End If

            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23503"
            MostrarError("Clave foránea inválida (cliente/tipo/usuario/venta).", ex)
        Catch ex As Exception
            MostrarError("Guardar cobro", ex)
        End Try
    End Sub

    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione un cobro." : Return
        If MessageBox.Show("¿Eliminar el cobro seleccionado?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM cobro WHERE cob_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = CInt(txtId.Text)}
                    })
                If ok Then
                    CargarListado() : LimpiarInputs() : DeshabilitarInputs()
                    lblStatus.Text = "Cobro eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MostrarError("No se puede eliminar: cobro referenciado.", ex)
            Catch ex As Exception
                MostrarError("Eliminar cobro", ex)
            End Try
        End If
    End Sub

    ' ================= VALIDACIONES =================
    Private Function ValCliente() As Boolean
        If cboCliente.SelectedIndex < 0 Then ep.SetError(cboCliente, "Seleccione un cliente.") : Return False
        ep.SetError(cboCliente, "") : Return True
    End Function

    Private Function ValTipo() As Boolean
        If cboCobroTipo.SelectedIndex < 0 Then ep.SetError(cboCobroTipo, "Seleccione un tipo de cobro.") : Return False
        ep.SetError(cboCobroTipo, "") : Return True
    End Function

    Private Function ValUsuario() As Boolean
        If cboUsuario.SelectedIndex < 0 Then ep.SetError(cboUsuario, "Seleccione un usuario.") : Return False
        ep.SetError(cboUsuario, "") : Return True
    End Function

    Private Function ValMonto() As Boolean
        Dim n As Decimal
        If Not Decimal.TryParse(txtMonto.Text, NumberStyles.Number, culturaPY, n) OrElse n < 0D Then
            ep.SetError(txtMonto, "Monto ≥ 0, ej.: 123.456,78") : Return False
        End If
        ep.SetError(txtMonto, "") : Return True
    End Function

    ' ================= HELPERS MONTOS =================
    Private Sub Monto_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Char.IsControl(e.KeyChar) OrElse Char.IsDigit(e.KeyChar) Then Return
        If e.KeyChar = decSep AndAlso Not CType(sender, TextBox).Text.Contains(decSep) Then Return
        e.Handled = True
    End Sub

    Private Sub Monto_Leave(sender As Object, e As EventArgs)
        Dim txt = CType(sender, TextBox)
        Dim m As Decimal
        If Decimal.TryParse(txt.Text, NumberStyles.Number, culturaPY, m) Then
            txt.Text = m.ToString("N2", culturaPY)
        End If
    End Sub

    ' ================= ERRORES / CIERRE =================
    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub

End Class
