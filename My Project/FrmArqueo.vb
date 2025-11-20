Option Strict On
Option Infer On
Imports Npgsql
Imports NpgsqlTypes
Imports System.Globalization

Public Class FrmArqueo

    ' Cultura Paraguay: 1.234,56
    Private ReadOnly culturaPY As New CultureInfo("es-PY")
    Private ReadOnly decSep As Char = culturaPY.NumberFormat.NumberDecimalSeparator(0)

    Private Sub FrmArqueo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            CargarCajas()
            CargarUsuarios()
            CargarListado()
            DeshabilitarInputs()

            ' Formato / validación simple de montos
            AddHandler txtMontoIni.KeyPress, AddressOf Monto_KeyPress
            AddHandler txtMontoFin.KeyPress, AddressOf Monto_KeyPress
            AddHandler txtMontoIni.Leave, AddressOf Monto_Leave
            AddHandler txtMontoFin.Leave, AddressOf Monto_Leave

            lblStatus.Text = "Listo."
        Catch ex As Exception
            MostrarError("Error al iniciar", ex)
        End Try
    End Sub

    ' ===================== DATA =====================
    Private Sub CargarCajas()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT caj_id, caj_descripcion
                  FROM caja
              ORDER BY caj_descripcion;")
            cboCaja.DataSource = dt
            cboCaja.ValueMember = "caj_id"
            cboCaja.DisplayMember = "caj_descripcion"
            cboCaja.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Error al cargar cajas", ex)
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

    Private Sub CargarListado()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT a.arq_id,
                       a.caj_id,
                       c.caj_descripcion,
                       a.usua_id,
                       u.usua_nombre,
                       a.arq_fecha,
                       a.arq_hora_inicio,
                       a.arq_hora_cierre,
                       a.arq_monto_ini,
                       a.arq_monto_fin
                  FROM arqueo a
             LEFT JOIN caja    c ON c.caj_id  = a.caj_id
             LEFT JOIN usuario u ON u.usua_id = a.usua_id
              ORDER BY a.arq_fecha DESC, a.arq_hora_inicio DESC;")

            DataGridView1.DataSource = dt

            ' Formato numérico para montos
            Dim colIni As DataGridViewColumn = Nothing
            Dim colFin As DataGridViewColumn = Nothing

            If DataGridView1.Columns.Contains("colMontoIni") Then
                colIni = DataGridView1.Columns("colMontoIni")
            ElseIf DataGridView1.Columns.Contains("arq_monto_ini") Then
                colIni = DataGridView1.Columns("arq_monto_ini")
            End If

            If DataGridView1.Columns.Contains("colMontoFin") Then
                colFin = DataGridView1.Columns("colMontoFin")
            ElseIf DataGridView1.Columns.Contains("arq_monto_fin") Then
                colFin = DataGridView1.Columns("arq_monto_fin")
            End If

            If colIni IsNot Nothing Then
                With colIni.DefaultCellStyle
                    .Format = "N2"
                    .FormatProvider = culturaPY
                    .Alignment = DataGridViewContentAlignment.MiddleRight
                    .NullValue = ""
                End With
            End If

            If colFin IsNot Nothing Then
                With colFin.DefaultCellStyle
                    .Format = "N2"
                    .FormatProvider = culturaPY
                    .Alignment = DataGridViewContentAlignment.MiddleRight
                    .NullValue = ""
                End With
            End If

            ' Ocultar FKs técnicas si tenés columnas en la grilla
            For Each fk In New String() {"caj_id", "usua_id"}
                If DataGridView1.Columns.Contains(fk) Then
                    DataGridView1.Columns(fk).Visible = False
                End If
            Next

            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = Nothing

        Catch ex As Exception
            MostrarError("Error al consultar arqueos", ex)
        End Try
    End Sub

    ' ===================== ESTADOS =====================
    Private Sub DeshabilitarInputs()
        txtId.ReadOnly = True
        cboCaja.Enabled = False
        cboUsuario.Enabled = False
        dtpFecha.Enabled = False
        dtpHoraIni.Enabled = False
        dtpHoraFin.Enabled = False
        txtMontoIni.ReadOnly = True
        txtMontoFin.ReadOnly = True

        BtnGuardar.Enabled = False
        BtnCancelar.Enabled = False
        BtnEditar.Enabled = False
        BtnEliminar.Enabled = False
        BtnNuevo.Enabled = True

        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        cboCaja.Enabled = True
        cboUsuario.Enabled = True
        dtpFecha.Enabled = True
        dtpHoraIni.Enabled = True
        dtpHoraFin.Enabled = True
        txtMontoIni.ReadOnly = False
        txtMontoFin.ReadOnly = False

        BtnGuardar.Enabled = True
        BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False

        If Not paraEdicion Then
            LimpiarInputs()
            dtpFecha.Value = Date.Today
            dtpHoraIni.Value = Date.Now
            dtpHoraFin.Value = Date.Now
        End If

        cboCaja.Focus()
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear()
        If cboCaja.Items.Count > 0 Then cboCaja.SelectedIndex = -1
        If cboUsuario.Items.Count > 0 Then cboUsuario.SelectedIndex = -1
        txtMontoIni.Clear()
        txtMontoFin.Clear()
        ep.Clear()
    End Sub

    ' ===================== GRID SELECCIÓN =====================
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellClick

        If e.RowIndex < 0 Then Return
        Dim r = DataGridView1.Rows(e.RowIndex)

        txtId.Text = CStr(TryGetCell(r, {"colId", "arq_id"}))

        Dim vCaj = TryGetCell(r, {"caj_id"})
        If vCaj IsNot Nothing AndAlso vCaj IsNot DBNull.Value Then
            cboCaja.SelectedValue = CInt(vCaj)
        End If

        Dim vUsu = TryGetCell(r, {"usua_id"})
        If vUsu IsNot Nothing AndAlso vUsu IsNot DBNull.Value Then
            cboUsuario.SelectedValue = CInt(vUsu)
        End If

        Dim vF = TryGetCell(r, {"colFecha", "arq_fecha"})
        If vF IsNot Nothing AndAlso vF IsNot DBNull.Value Then
            dtpFecha.Value = Convert.ToDateTime(vF)
        End If

        Dim vHi = TryGetCell(r, {"colHoraIni", "arq_hora_inicio"})
        If vHi IsNot Nothing AndAlso vHi IsNot DBNull.Value Then
            Dim ts As TimeSpan = If(TypeOf vHi Is TimeSpan,
                                    CType(vHi, TimeSpan),
                                    TimeSpan.Parse(vHi.ToString()))
            dtpHoraIni.Value = Date.Today.Add(ts)
        End If

        Dim vHf = TryGetCell(r, {"colHoraFin", "arq_hora_cierre"})
        If vHf IsNot Nothing AndAlso vHf IsNot DBNull.Value Then
            Dim ts As TimeSpan = If(TypeOf vHf Is TimeSpan,
                                    CType(vHf, TimeSpan),
                                    TimeSpan.Parse(vHf.ToString()))
            dtpHoraFin.Value = Date.Today.Add(ts)
        End If

        Dim vMi = TryGetCell(r, {"colMontoIni", "arq_monto_ini"})
        If vMi IsNot Nothing AndAlso vMi IsNot DBNull.Value Then
            txtMontoIni.Text = Convert.ToDecimal(vMi).ToString("N2", culturaPY)
        Else
            txtMontoIni.Text = ""
        End If

        Dim vMf = TryGetCell(r, {"colMontoFin", "arq_monto_fin"})
        If vMf IsNot Nothing AndAlso vMf IsNot DBNull.Value Then
            txtMontoFin.Text = Convert.ToDecimal(vMf).ToString("N2", culturaPY)
        Else
            txtMontoFin.Text = ""
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

    ' ===================== BOTONES =====================
    Private Sub BtnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False)
        lblStatus.Text = "Arqueo - Nuevo"
    End Sub

    Private Sub BtnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtId.TextLength = 0 Then
            lblStatus.Text = "Seleccione un arqueo."
            Return
        End If
        HabilitarInputs(True)
        lblStatus.Text = "Arqueo - Editar"
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValCaja() AndAlso ValUsuario() AndAlso ValMontoIni() AndAlso ValMontoFin()) Then
                lblStatus.Text = "Corrija los campos marcados."
                Return
            End If

            Dim mIni As Decimal
            Dim mFin As Decimal
            Decimal.TryParse(txtMontoIni.Text, NumberStyles.Number, culturaPY, mIni)
            Decimal.TryParse(txtMontoFin.Text, NumberStyles.Number, culturaPY, mFin)

            Dim pars = New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@caj", NpgsqlDbType.Integer) With {.Value = CInt(cboCaja.SelectedValue)},
                New NpgsqlParameter("@usu", NpgsqlDbType.Integer) With {.Value = CInt(cboUsuario.SelectedValue)},
                New NpgsqlParameter("@fec", NpgsqlDbType.Date) With {.Value = dtpFecha.Value.Date},
                New NpgsqlParameter("@hi", NpgsqlDbType.Time) With {.Value = dtpHoraIni.Value.TimeOfDay},
                New NpgsqlParameter("@hf", NpgsqlDbType.Time) With {.Value = dtpHoraFin.Value.TimeOfDay},
                New NpgsqlParameter("@mi", NpgsqlDbType.Numeric) With {.Value = mIni},
                New NpgsqlParameter("@mf", NpgsqlDbType.Numeric) With {.Value = mFin}
            }

            If txtId.TextLength = 0 Then
                ' INSERT
                Dim id = Conexiones.EjecutarsqlScalar("
                    INSERT INTO arqueo
                        (caj_id, usua_id, arq_fecha, arq_hora_inicio, arq_hora_cierre, arq_monto_ini, arq_monto_fin)
                    VALUES (@caj, @usu, @fec, @hi, @hf, @mi, @mf)
                    RETURNING arq_id;", pars)
                txtId.Text = CStr(id)
                lblStatus.Text = $"Arqueo insertado (ID {txtId.Text})."
            Else
                ' UPDATE
                pars.Add(New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = CInt(txtId.Text)})
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE arqueo
                       SET caj_id=@caj,
                           usua_id=@usu,
                           arq_fecha=@fec,
                           arq_hora_inicio=@hi,
                           arq_hora_cierre=@hf,
                           arq_monto_ini=@mi,
                           arq_monto_fin=@mf
                     WHERE arq_id=@id;", pars)
                lblStatus.Text = If(ok, "Arqueo actualizado.", "Sin cambios.")
            End If

            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23503"
            MostrarError("Caja o usuario inválido (FK).", ex)
        Catch ex As Exception
            MostrarError("Error al guardar arqueo", ex)
        End Try
    End Sub

    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtId.TextLength = 0 Then
            lblStatus.Text = "Seleccione un arqueo."
            Return
        End If

        If MessageBox.Show("¿Eliminar el arqueo seleccionado?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM arqueo
                     WHERE arq_id=@id;",
                    New List(Of NpgsqlParameter) From {
                        New NpgsqlParameter("@id", NpgsqlDbType.Integer) With {.Value = CInt(txtId.Text)}
                    })

                If ok Then
                    CargarListado()
                    LimpiarInputs()
                    DeshabilitarInputs()
                    lblStatus.Text = "Arqueo eliminado."
                Else
                    lblStatus.Text = "No se eliminó el registro."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MostrarError("No se puede eliminar: arqueo referenciado.", ex)
            Catch ex As Exception
                MostrarError("Error al eliminar arqueo", ex)
            End Try
        End If
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub

    ' ===================== VALIDACIONES =====================
    Private Function ValCaja() As Boolean
        If cboCaja.SelectedIndex < 0 Then
            ep.SetError(cboCaja, "Seleccione una caja.")
            Return False
        End If
        ep.SetError(cboCaja, "")
        Return True
    End Function

    Private Function ValUsuario() As Boolean
        If cboUsuario.SelectedIndex < 0 Then
            ep.SetError(cboUsuario, "Seleccione un usuario.")
            Return False
        End If
        ep.SetError(cboUsuario, "")
        Return True
    End Function

    Private Function ValMontoIni() As Boolean
        Dim n As Decimal
        If Not Decimal.TryParse(txtMontoIni.Text, NumberStyles.Number, culturaPY, n) OrElse n < 0D Then
            ep.SetError(txtMontoIni, "Monto inicial ≥ 0 (ej.: 100.000,00).")
            Return False
        End If
        ep.SetError(txtMontoIni, "")
        Return True
    End Function

    Private Function ValMontoFin() As Boolean
        Dim n As Decimal
        If Not Decimal.TryParse(txtMontoFin.Text, NumberStyles.Number, culturaPY, n) OrElse n < 0D Then
            ep.SetError(txtMontoFin, "Monto final ≥ 0 (ej.: 150.000,00).")
            Return False
        End If
        ep.SetError(txtMontoFin, "")
        Return True
    End Function

    ' ===================== HELPERS MONTOS =====================
    Private Sub Monto_KeyPress(sender As Object, e As KeyPressEventArgs)
        Dim txt = CType(sender, TextBox)

        If Char.IsControl(e.KeyChar) OrElse Char.IsDigit(e.KeyChar) Then
            Return
        End If

        ' Permitir un solo separador decimal (coma)
        If e.KeyChar = decSep AndAlso Not txt.Text.Contains(decSep) Then
            Return
        End If

        e.Handled = True
    End Sub

    Private Sub Monto_Leave(sender As Object, e As EventArgs)
        Dim txt = CType(sender, TextBox)
        Dim n As Decimal
        If Decimal.TryParse(txt.Text, NumberStyles.Number, culturaPY, n) Then
            txt.Text = n.ToString("N2", culturaPY)
        End If
    End Sub

    ' ===================== ERRORES =====================
    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

End Class
