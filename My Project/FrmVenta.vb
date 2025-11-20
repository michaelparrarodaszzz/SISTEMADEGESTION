Option Strict On
Option Infer On
Imports Npgsql
Imports NpgsqlTypes
Imports System.Globalization

Public Class FrmVenta

    ' Cultura PY: miles con punto y decimales con coma
    Private ReadOnly culturaPY As New CultureInfo("es-PY")
    Private ReadOnly grpSep As Char = culturaPY.NumberFormat.NumberGroupSeparator(0)
    Private ReadOnly decSep As Char = culturaPY.NumberFormat.NumberDecimalSeparator(0)

    Private Sub FrmVenta_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            CargarClientes()
            CargarEstados()
            CargarListado()
            DeshabilitarInputs()

            ' Validaciones en tiempo real mínimas
            AddHandler cboCliente.SelectedIndexChanged, Sub() ValCliente()
            AddHandler txtTotal.Leave, AddressOf Monto_Leave
            AddHandler txtDescuento.Leave, AddressOf Monto_Leave
            AddHandler txtTotal.KeyPress, AddressOf Monto_KeyPress
            AddHandler txtDescuento.KeyPress, AddressOf Monto_KeyPress

            lblStatus.Text = "Listo."
        Catch ex As Exception
            MostrarError("Error al iniciar", ex)
        End Try
    End Sub

    ' =================== Data ===================
    Private Sub CargarListado()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT v.ven_id,
                       v.cli_id,
                       c.cli_nombre,
                       v.ven_fecha,
                       v.ven_hora,
                       v.ven_total,
                       v.ven_descuento,
                       v.ven_estado
                  FROM venta v
                  JOIN cliente c ON c.cli_id = v.cli_id
              ORDER BY v.ven_id DESC;")

            DataGridView1.DataSource = dt

            ' Formatos de columnas (funciona con columnas manuales o automáticas)
            FormatearColumnaMoneda("colTotal", "ven_total")
            FormatearColumnaMoneda("colDesc", "ven_descuento")

        Catch ex As Exception
            MostrarError("Error al consultar ventas", ex)
        End Try
    End Sub

    Private Sub FormatearColumnaMoneda(nombreManual As String, nombreQuery As String)
        Dim col As DataGridViewColumn = Nothing
        If DataGridView1.Columns.Contains(nombreManual) Then
            col = DataGridView1.Columns(nombreManual)
        ElseIf DataGridView1.Columns.Contains(nombreQuery) Then
            col = DataGridView1.Columns(nombreQuery)
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

    Private Sub CargarClientes()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT cli_id, COALESCE(NULLIF(TRIM(cli_nombre||' '||COALESCE(cli_apellido,'')),''), 'SIN NOMBRE') AS nombre
                  FROM cliente
              ORDER BY nombre;")
            cboCliente.DataSource = dt
            cboCliente.ValueMember = "cli_id"
            cboCliente.DisplayMember = "nombre"
            cboCliente.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Error al cargar clientes", ex)
        End Try
    End Sub

    Private Sub CargarEstados()
        If cboEstado.Items.Count = 0 Then
            cboEstado.Items.AddRange(New Object() {"PENDIENTE", "PAGADA", "ANULADA"})
        End If
        If cboEstado.SelectedIndex < 0 Then cboEstado.SelectedItem = "PENDIENTE"
    End Sub

    ' =================== Estados / Habilitación ===================
    Private Sub DeshabilitarInputs()
        txtId.ReadOnly = True
        cboCliente.Enabled = False
        dtpFecha.Enabled = False
        dtpHora.Enabled = False
        txtTotal.ReadOnly = True
        txtDescuento.ReadOnly = True
        cboEstado.Enabled = False

        BtnGuardar.Enabled = False : BtnEditar.Enabled = False
        BtnEliminar.Enabled = False : BtnCancelar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        cboCliente.Enabled = True
        dtpFecha.Enabled = True
        dtpHora.Enabled = True
        txtTotal.ReadOnly = False
        txtDescuento.ReadOnly = False
        cboEstado.Enabled = True

        txtId.ReadOnly = True ' PK es SERIAL

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
        cboCliente.Focus()
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear()
        If cboCliente.Items.Count > 0 Then cboCliente.SelectedIndex = -1
        txtTotal.Clear()
        txtDescuento.Clear()
        ep.Clear()
    End Sub

    ' =================== Grid selección ===================
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Return
        Dim row = DataGridView1.Rows(e.RowIndex)

        txtId.Text = Convert.ToString(TryGetCell(row, {"colId", "ven_id"}))

        ' Cliente por id (mejor) o por nombre si sólo está el texto
        Dim vCliId = TryGetCell(row, {"cli_id"})
        If vCliId IsNot Nothing AndAlso vCliId IsNot DBNull.Value Then
            cboCliente.SelectedValue = CInt(vCliId)
        Else
            SeleccionarClientePorNombre(Convert.ToString(TryGetCell(row, {"colCliente", "cli_nombre"})))
        End If

        Dim vFec = TryGetCell(row, {"colFecha", "ven_fecha"})
        If vFec IsNot Nothing AndAlso vFec IsNot DBNull.Value Then
            dtpFecha.Value = Convert.ToDateTime(vFec)
        End If

        Dim vHora = TryGetCell(row, {"colHora", "ven_hora"})
        If vHora IsNot Nothing AndAlso vHora IsNot DBNull.Value Then
            ' TIME en PG mapea a TimeSpan; armamos DateTime para el picker
            Dim ts As TimeSpan = If(TypeOf vHora Is TimeSpan, CType(vHora, TimeSpan), TimeSpan.Parse(vHora.ToString()))
            dtpHora.Value = Date.Today.Add(ts)
        End If

        Dim vTot = TryGetCell(row, {"colTotal", "ven_total"})
        txtTotal.Text = If(vTot IsNot Nothing AndAlso vTot IsNot DBNull.Value,
                           Convert.ToDecimal(vTot).ToString("N2", culturaPY), "")

        Dim vDesc = TryGetCell(row, {"colDesc", "ven_descuento"})
        txtDescuento.Text = If(vDesc IsNot Nothing AndAlso vDesc IsNot DBNull.Value,
                               Convert.ToDecimal(vDesc).ToString("N2", culturaPY), "")

        Dim vEst = TryGetCell(row, {"colEstado", "ven_estado"})
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

    Private Sub SeleccionarClientePorNombre(nombre As String)
        If String.IsNullOrWhiteSpace(nombre) Then
            cboCliente.SelectedIndex = -1 : Return
        End If
        For i = 0 To cboCliente.Items.Count - 1
            Dim drv = TryCast(cboCliente.Items(i), DataRowView)
            If drv IsNot Nothing AndAlso
               String.Equals(Convert.ToString(drv("nombre")), nombre, StringComparison.OrdinalIgnoreCase) Then
                cboCliente.SelectedIndex = i : Exit For
            End If
        Next
    End Sub

    ' =================== Botones ===================
    Private Sub BtnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(paraEdicion:=False)
        lblStatus.Text = "Venta - Nuevo"
    End Sub

    Private Sub BtnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtId.TextLength = 0 Then
            lblStatus.Text = "Seleccione una venta."
            Return
        End If
        HabilitarInputs(paraEdicion:=True)
        lblStatus.Text = "Venta - Editar"
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        LimpiarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValCliente() AndAlso ValTotales() AndAlso ValEstado()) Then
                lblStatus.Text = "Corrija los campos marcados."
                Return
            End If

            Dim total As Decimal, desc As Decimal
            Decimal.TryParse(txtTotal.Text, NumberStyles.Number, culturaPY, total)
            Decimal.TryParse(txtDescuento.Text, NumberStyles.Number, culturaPY, desc)

            Dim pars = New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@cli", CInt(cboCliente.SelectedValue)),
                New NpgsqlParameter("@usu", DBNull.Value), ' usa tu user actual si lo tienes
                New NpgsqlParameter("@fec", dtpFecha.Value.Date),
                New NpgsqlParameter("@hor", dtpHora.Value.TimeOfDay),
                New NpgsqlParameter("@tot", total),
                New NpgsqlParameter("@des", desc),
                New NpgsqlParameter("@est", cboEstado.SelectedItem.ToString())
            }

            If txtId.TextLength = 0 Then
                ' NUEVO
                Dim id = Conexiones.EjecutarsqlScalar("
                    INSERT INTO venta (cli_id, usua_id, ven_fecha, ven_hora, ven_total, ven_descuento, ven_estado)
                    VALUES (@cli, @usu, @fec, @hor, @tot, @des, @est)
                    RETURNING ven_id;", pars)
                txtId.Text = CStr(id)
                lblStatus.Text = $"Venta insertada (ID {txtId.Text})."
            Else
                ' EDITAR
                pars.Add(New NpgsqlParameter("@id", CInt(txtId.Text)))
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE venta
                       SET cli_id=@cli,
                           usua_id=@usu,
                           ven_fecha=@fec,
                           ven_hora=@hor,
                           ven_total=@tot,
                           ven_descuento=@des,
                           ven_estado=@est
                     WHERE ven_id=@id;", pars)
                lblStatus.Text = If(ok, "Venta actualizada.", "Sin cambios.")
            End If
            LimpiarInputs()
            CargarListado()
            DeshabilitarInputs()

        Catch ex As Exception
            MostrarError("Error al guardar venta", ex)
        End Try
    End Sub

    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then
            lblStatus.Text = "Seleccione una venta."
            Return
        End If
        If MessageBox.Show("¿Eliminar la venta seleccionada?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM venta WHERE ven_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@id", CInt(txtId.Text))
                    })
                If ok Then
                    CargarListado()
                    LimpiarInputs()
                    DeshabilitarInputs()
                    lblStatus.Text = "Venta eliminada."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MostrarError("No se puede eliminar: venta referenciada.", ex)
            Catch ex As Exception
                MostrarError("Error al eliminar venta", ex)
            End Try
        End If
    End Sub

    ' =================== Validaciones ===================
    Private Function ValCliente() As Boolean
        If cboCliente.SelectedIndex < 0 Then
            ep.SetError(cboCliente, "Seleccione un cliente.") : Return False
        End If
        ep.SetError(cboCliente, "") : Return True
    End Function

    Private Function ValTotales() As Boolean
        Dim ok1 = Decimal.TryParse(txtTotal.Text, NumberStyles.Number, culturaPY, Nothing)
        Dim ok2 = Decimal.TryParse(txtDescuento.Text, NumberStyles.Number, culturaPY, Nothing)
        If Not ok1 Then ep.SetError(txtTotal, "Monto inválido.") Else ep.SetError(txtTotal, "")
        If Not ok2 Then ep.SetError(txtDescuento, "Monto inválido.") Else ep.SetError(txtDescuento, "")
        Return ok1 AndAlso ok2
    End Function

    Private Function ValEstado() As Boolean
        If cboEstado.SelectedIndex < 0 Then
            ep.SetError(cboEstado, "Seleccione estado.") : Return False
        End If
        ep.SetError(cboEstado, "") : Return True
    End Function

    ' =================== Helpers de monto ===================
    Private Sub Monto_KeyPress(sender As Object, e As KeyPressEventArgs)
        ' Permite dígitos, Backspace y un único separador decimal local
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

    ' =================== Errores amigables ===================
    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub

End Class
