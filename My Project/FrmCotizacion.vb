Option Strict On
Option Infer On
Imports Npgsql
Imports System.Globalization
Imports NpgsqlTypes

Public Class FrmCotizacion
    Private _cargando As Boolean = False

    Private Sub FrmCotizacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Formateos sugeridos (hacer también en diseñador)
        dtpFecha.Format = DateTimePickerFormat.Custom
        dtpFecha.CustomFormat = "dd/MM/yyyy"
        dtpHora.Format = DateTimePickerFormat.Custom
        dtpHora.CustomFormat = "HH:mm"
        dtpHora.ShowUpDown = True

        PrepararGrid()
        CargarMonedas()
        CargarListado()
        DeshabilitarInputs()
        lblStatus.Text = "Listo."

        AddHandler txtPrecio.TextChanged, Sub() ValPrecio()
        AddHandler CboMoneda.SelectedIndexChanged, Sub() ValMoneda()
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

    ' ===== Data =====
    Private Sub CargarMonedas()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT mon_id, mon_descripcion
                FROM moneda
                ORDER BY mon_descripcion;")
            CboMoneda.DataSource = dt
            CboMoneda.ValueMember = "mon_id"
            CboMoneda.DisplayMember = "mon_descripcion"
            CboMoneda.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Error al cargar monedas", ex)
        End Try
    End Sub

    Private Sub CargarListado()
        Try
            _cargando = True
            Dim dt = Conexiones.Consulta("
                SELECT c.cot_id,
                       c.mon_id,
                       m.mon_descripcion,
                       c.cot_precio,
                       c.cot_fecha,
                       c.cot_hora
                FROM cotizacion c
                JOIN moneda m ON m.mon_id = c.mon_id
                ORDER BY c.cot_fecha DESC, c.cot_hora DESC;")
            DataGridView1.DataSource = dt

            If DataGridView1.Columns.Contains("mon_id") Then DataGridView1.Columns("mon_id").Visible = False
            If DataGridView1.Columns.Contains("cot_precio") Then DataGridView1.Columns("cot_precio").DefaultCellStyle.Format = "N2"
            If DataGridView1.Columns.Contains("cot_fecha") Then DataGridView1.Columns("cot_fecha").DefaultCellStyle.Format = "dd/MM/yyyy"
            If DataGridView1.Columns.Contains("cot_hora") Then DataGridView1.Columns("cot_hora").DefaultCellStyle.Format = "HH:mm"

            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = Nothing
        Catch ex As Exception
            MostrarError("Error al consultar cotizaciones", ex)
        Finally
            _cargando = False
        End Try
    End Sub

    ' ===== Estados UI =====
    Private Sub DeshabilitarInputs()
        txtId.ReadOnly = True
        txtPrecio.ReadOnly = True
        CboMoneda.Enabled = False
        dtpFecha.Enabled = False
        dtpHora.Enabled = False

        BtnGuardar.Enabled = False : BtnCancelar.Enabled = False
        BtnEditar.Enabled = False : BtnEliminar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        txtPrecio.ReadOnly = False
        CboMoneda.Enabled = True
        dtpFecha.Enabled = True
        dtpHora.Enabled = True

        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False

        If Not paraEdicion Then
            LimpiarInputs()
            If CboMoneda.Items.Count > 0 Then CboMoneda.SelectedIndex = -1
            dtpFecha.Value = DateTime.Today
            dtpHora.Value = DateTime.Now
            txtPrecio.Focus()
        End If
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear()
        txtPrecio.Clear()
        ep.Clear()
    End Sub

    ' ===== Grid -> Form =====
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellClick, DataGridView1.CellContentClick
        If _cargando OrElse e.RowIndex < 0 Then Return
        Dim row = DataGridView1.Rows(e.RowIndex)

        txtId.Text = Convert.ToString(TryGetCell(row, {"colId", "cot_id"}))
        txtPrecio.Text = FormateaDecimal(TryGetCell(row, {"colPrecio", "cot_precio"}), "0.00")

        Dim vMonId = TryGetCell(row, {"colMonedaId", "mon_id"})
        If vMonId IsNot Nothing AndAlso vMonId IsNot DBNull.Value Then
            CboMoneda.SelectedValue = CInt(vMonId)
        Else
            ' por si sólo tenés mon_descripcion en la grilla
            SeleccionarMonedaPorNombre(Convert.ToString(TryGetCell(row, {"colMoneda", "mon_descripcion"})))
        End If

        Dim vFecha = TryGetCell(row, {"colFecha", "cot_fecha"})
        Dim vHora = TryGetCell(row, {"colHora", "cot_hora"})
        If vFecha IsNot Nothing AndAlso vFecha IsNot DBNull.Value Then dtpFecha.Value = CDate(vFecha)
        If vHora IsNot Nothing AndAlso vHora IsNot DBNull.Value Then
            ' Npgsql devuelve Time como TimeSpan o DateTime según mapeo
            If TypeOf vHora Is TimeSpan Then
                dtpHora.Value = Date.Today.Add(CType(vHora, TimeSpan))
            Else
                dtpHora.Value = CDate(vHora)
            End If
        End If

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

    Private Sub SeleccionarMonedaPorNombre(nombre As String)
        If String.IsNullOrWhiteSpace(nombre) Then CboMoneda.SelectedIndex = -1 : Return
        For i = 0 To CboMoneda.Items.Count - 1
            Dim drv = TryCast(CboMoneda.Items(i), DataRowView)
            If drv IsNot Nothing AndAlso
               String.Equals(Convert.ToString(drv("mon_descripcion")), nombre, StringComparison.OrdinalIgnoreCase) Then
                CboMoneda.SelectedIndex = i : Exit For
            End If
        Next
    End Sub

    ' ===== Botones =====
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False)
        lblStatus.Text = "Modo: Nuevo"
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione una cotización." : Return
        HabilitarInputs(True)
        lblStatus.Text = "Modo: Editar"
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValMoneda() AndAlso ValPrecio()) Then
                lblStatus.Text = "Corrija los campos marcados." : Return
            End If

            Dim precio As Decimal
            Decimal.TryParse(NormalizaNumero(txtPrecio.Text), NumberStyles.Number, CultureInfo.InvariantCulture, precio)

            Dim pMon As New NpgsqlParameter("@mon", NpgsqlDbType.Integer) With {.Value = CInt(CboMoneda.SelectedValue)}
            Dim pPrecio As New NpgsqlParameter("@pre", NpgsqlDbType.Numeric) With {.Value = precio}
            Dim pFecha As New NpgsqlParameter("@fec", NpgsqlDbType.Date) With {.Value = dtpFecha.Value.Date}
            Dim pHora As New NpgsqlParameter("@hor", NpgsqlDbType.Time) With {.Value = dtpHora.Value.TimeOfDay}

            If txtId.TextLength = 0 Then
                Dim newId = Conexiones.EjecutarsqlScalar("
                    INSERT INTO cotizacion (mon_id, cot_precio, cot_fecha, cot_hora)
                    VALUES (@mon, @pre, @fec, @hor)
                    RETURNING cot_id;",
                    New List(Of NpgsqlParameter) From {pMon, pPrecio, pFecha, pHora})
                txtId.Text = Convert.ToString(newId)
                lblStatus.Text = $"Insertado ID {txtId.Text}."
            Else
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE cotizacion
                    SET mon_id=@mon, cot_precio=@pre, cot_fecha=@fec, cot_hora=@hor
                    WHERE cot_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        pMon, pPrecio, pFecha, pHora,
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtId.Text)}
                    })
                lblStatus.Text = If(ok, "Actualizado.", "Sin cambios.")
            End If
            LimpiarInputs()
            CargarListado()
            DeshabilitarInputs()

        Catch ex As Exception
            MostrarError("Error al guardar cotización", ex)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then lblStatus.Text = "Seleccione una cotización." : Return
        If MessageBox.Show("¿Eliminar la cotización seleccionada?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM cotizacion WHERE cot_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = Integer.Parse(txtId.Text)}
                    })
                If ok Then
                    CargarListado() : DeshabilitarInputs() : lblStatus.Text = "Eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MostrarError("No se puede eliminar: cotización referenciada.", ex)
            Catch ex As Exception
                MostrarError("Error al eliminar cotización", ex)
            End Try
        End If
    End Sub

    ' ===== Validaciones =====
    Private Function ValMoneda() As Boolean
        If CboMoneda.SelectedIndex < 0 Then ep.SetError(CboMoneda, "Seleccione una moneda.") : Return False
        ep.SetError(CboMoneda, "") : Return True
    End Function

    Private Function ValPrecio() As Boolean
        Dim s = NormalizaNumero(txtPrecio.Text)
        Dim n As Decimal
        If Not Decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, n) OrElse n < 0D Then
            ep.SetError(txtPrecio, "Número decimal ≥ 0 (ej.: 7300,50).") : Return False
        End If
        txtPrecio.Text = n.ToString("0.00")
        ep.SetError(txtPrecio, "") : Return True
    End Function

    Private Function NormalizaNumero(input As String) As String
        Dim s = input.Trim()
        If s.Contains(",") AndAlso Not s.Contains(".") Then s = s.Replace(",", ".")
        Return s
    End Function

    Private Function FormateaDecimal(v As Object, fmt As String) As String
        If v Is Nothing OrElse v Is DBNull.Value Then Return ""
        Return Convert.ToDecimal(v).ToString(fmt)
    End Function

    ' ===== Errores amigables =====
    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub
End Class
