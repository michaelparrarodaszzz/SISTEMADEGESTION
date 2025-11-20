Option Strict On
Option Infer On
Imports Npgsql
Imports System.Globalization
Imports System.Text

Public Class FrmProducto


    Private isFormatting As Boolean = False
    Private ReadOnly culturaPY As New CultureInfo("es-PY")
    Private ReadOnly grpSep As Char = culturaPY.NumberFormat.NumberGroupSeparator(0)

    Private Sub FrmProducto_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            CargarMarcas()
            CargarListado()
            DeshabilitarInputs()
            lblStatus.Text = "Listo."

            AddHandler txtCodigo.TextChanged, Sub() ValCodigo()
            AddHandler txtNombre.TextChanged, Sub() ValNombre()
            AddHandler txtIva.TextChanged, Sub() ValIVA()
            AddHandler cboMarca.SelectedIndexChanged, Sub() ValMarca()

        Catch ex As Exception
            MostrarError("Error al iniciar", ex)
        End Try
    End Sub

    ' ====== Data ======
    Private Sub CargarListado()
        Try
            Dim dt = Conexiones.Consulta("
            SELECT p.prod_id,
                   p.prod_nombre,
                   p.mar_id,
                   m.mar_nombre,
                   p.prod_precio,
                   p.prod_iva,
                   p.prod_descripcion
            FROM producto p
            JOIN marca m ON m.mar_id = p.mar_id
            ORDER BY p.prod_nombre;")

            DataGridView1.DataSource = dt

            If DataGridView1.Columns.Contains("colPrecio") Then
                With DataGridView1.Columns("colPrecio").DefaultCellStyle
                    .Format = "N0"
                    .FormatProvider = culturaPY
                    .Alignment = DataGridViewContentAlignment.MiddleRight
                    .NullValue = ""
                End With
            ElseIf DataGridView1.Columns.Contains("prod_precio") Then
                With DataGridView1.Columns("prod_precio").DefaultCellStyle
                    .Format = "N0"
                    .FormatProvider = culturaPY
                    .Alignment = DataGridViewContentAlignment.MiddleRight
                    .NullValue = ""
                End With
            End If

            ' Formato de IVA
            If DataGridView1.Columns.Contains("colIVA") Then
                With DataGridView1.Columns("colIVA").DefaultCellStyle
                    .Format = "N0"
                    .FormatProvider = culturaPY
                    .Alignment = DataGridViewContentAlignment.MiddleRight
                    .NullValue = ""
                End With
            ElseIf DataGridView1.Columns.Contains("prod_iva") Then
                With DataGridView1.Columns("prod_iva").DefaultCellStyle
                    .Format = "N0"
                    .FormatProvider = culturaPY
                    .Alignment = DataGridViewContentAlignment.MiddleRight
                    .NullValue = ""
                End With
            End If

            ' Oculta la FK técnica si está
            If DataGridView1.Columns.Contains("colMarId") Then
                DataGridView1.Columns("colMarId").Visible = False
            ElseIf DataGridView1.Columns.Contains("mar_id") Then
                DataGridView1.Columns("mar_id").Visible = False
            End If

        Catch ex As Exception
            MostrarError("Error al consultar productos", ex)
        End Try
    End Sub

    Private Sub CargarMarcas()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT mar_id, mar_nombre
                FROM marca
                ORDER BY mar_nombre;")
            cboMarca.DataSource = dt
            cboMarca.ValueMember = "mar_id"
            cboMarca.DisplayMember = "mar_nombre"
            cboMarca.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Error al cargar marcas", ex)
        End Try
    End Sub

    ' ====== Estados / Habilitación ======
    Private Sub DeshabilitarInputs()
        txtCodigo.ReadOnly = True
        txtNombre.ReadOnly = True
        txtDescripcion.ReadOnly = True
        txtPrecio.ReadOnly = True
        txtIva.ReadOnly = True
        cboMarca.Enabled = False

        BtnGuardar.Enabled = False : BtnEditar.Enabled = False
        BtnEliminar.Enabled = False : BtnCancelar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        txtNombre.ReadOnly = False
        txtDescripcion.ReadOnly = False
        txtPrecio.ReadOnly = False
        txtIva.ReadOnly = False
        cboMarca.Enabled = True

        ' PK es manual (código). Solo editable en "Nuevo"
        txtCodigo.ReadOnly = paraEdicion
        txtCodigo.BackColor = If(paraEdicion, Drawing.Color.FromArgb(248, 248, 248), Drawing.Color.White)

        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False

        If Not paraEdicion Then
            LimpiarInputs()
        End If
        txtCodigo.Focus()
    End Sub

    Private Sub LimpiarInputs()
        txtCodigo.Clear()
        txtNombre.Clear()
        txtDescripcion.Clear()
        txtPrecio.Clear()
        txtIva.Clear()
        If cboMarca.Items.Count > 0 Then cboMarca.SelectedIndex = -1
        ep.Clear()
    End Sub

    ' ====== Grid selección ======
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Return
        Dim row = DataGridView1.Rows(e.RowIndex)

        txtCodigo.Text = Convert.ToString(TryGetCell(row, {"colCodigo", "prod_id"}))
        txtNombre.Text = Convert.ToString(TryGetCell(row, {"colNombre", "prod_nombre"}))
        txtDescripcion.Text = Convert.ToString(TryGetCell(row, {"colDesc", "prod_descripcion"}))

        Dim vPrecio = TryGetCell(row, {"colPrecio", "prod_precio"})
        If vPrecio IsNot Nothing AndAlso vPrecio IsNot DBNull.Value Then
            Dim n = Convert.ToDecimal(vPrecio)
            txtPrecio.Text = n.ToString("N0", culturaPY) ' muestra 1.000.000
        Else
            txtPrecio.Text = ""
        End If

        Dim vIVA = TryGetCell(row, {"colIVA", "prod_iva"})
        If vIVA IsNot Nothing AndAlso vIVA IsNot DBNull.Value Then
            txtIva.Text = Convert.ToDecimal(vIVA).ToString("0")
        Else
            txtIva.Text = ""
        End If

        Dim vMarcaId = TryGetCell(row, {"mar_id"})
        If vMarcaId IsNot Nothing AndAlso vMarcaId IsNot DBNull.Value Then
            cboMarca.SelectedValue = CInt(vMarcaId)
        Else
            SeleccionarMarcaPorNombre(Convert.ToString(TryGetCell(row, {"colMarca", "mar_nombre"})))
        End If

        ' Modo visualización
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

    Private Sub SeleccionarMarcaPorNombre(nombre As String)
        If String.IsNullOrWhiteSpace(nombre) Then
            cboMarca.SelectedIndex = -1 : Return
        End If
        For i = 0 To cboMarca.Items.Count - 1
            Dim drv = TryCast(cboMarca.Items(i), DataRowView)
            If drv IsNot Nothing AndAlso
               String.Equals(Convert.ToString(drv("mar_nombre")), nombre, StringComparison.OrdinalIgnoreCase) Then
                cboMarca.SelectedIndex = i : Exit For
            End If
        Next
    End Sub

    ' ====== Botones ======
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(paraEdicion:=False)
        lblStatus.Text = "Modo: Nuevo"
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtCodigo.TextLength = 0 Then
            lblStatus.Text = "Seleccione un registro."
            Return
        End If
        HabilitarInputs(paraEdicion:=True)
        lblStatus.Text = "Modo: Editar"
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValCodigo() AndAlso ValNombre() AndAlso ValMarca() AndAlso ValPrecio() AndAlso ValIVA()) Then
                lblStatus.Text = "Corrija los campos marcados."
                Return
            End If

            ' Precio NUMERIC(20,0): tomar sólo dígitos del textbox (quita puntos)
            Dim rawDigits = New String(txtPrecio.Text.Where(AddressOf Char.IsDigit).ToArray())
            Dim precio As Decimal
            If Not Decimal.TryParse(rawDigits, NumberStyles.None, CultureInfo.InvariantCulture, precio) Then
                ep.SetError(txtPrecio, "Precio inválido.")
                lblStatus.Text = "Precio inválido."
                Return
            End If

            Dim iva As Decimal
            Decimal.TryParse(NormalizaNumero(txtIva.Text), NumberStyles.Number, CultureInfo.InvariantCulture, iva)

            Dim pars = New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@id", txtCodigo.Text.Trim()),
                New NpgsqlParameter("@nombre", txtNombre.Text.Trim()),
                New NpgsqlParameter("@mar", CInt(cboMarca.SelectedValue)),
                New NpgsqlParameter("@precio", precio),
                New NpgsqlParameter("@iva", iva),
                New NpgsqlParameter("@desc", If(String.IsNullOrWhiteSpace(txtDescripcion.Text), CType(DBNull.Value, Object), txtDescripcion.Text.Trim()))
            }

            If BtnEliminar.Enabled = False AndAlso txtCodigo.ReadOnly = False Then
                ' NUEVO
                Dim sql = "
                    INSERT INTO producto (prod_id, prod_nombre, mar_id, prod_descripcion, prod_precio, prod_iva)
                    VALUES (@id, @nombre, @mar, @desc, @precio, @iva);"
                Dim ok = Conexiones.Ejecutarsql(sql, pars)
                lblStatus.Text = If(ok, "Insertado correctamente.", "No se insertó.")
            Else
                ' EDITAR
                Dim sql = "
                    UPDATE producto
                    SET prod_nombre=@nombre,
                        mar_id=@mar,
                        prod_descripcion=@desc,
                        prod_precio=@precio,
                        prod_iva=@iva
                    WHERE prod_id=@id;"
                Dim ok = Conexiones.Ejecutarsql(sql, pars)
                lblStatus.Text = If(ok, "Actualizado correctamente.", "No se actualizó.")
            End If
            LimpiarInputs()
            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23505"
            ep.SetError(txtCodigo, "Ese código ya existe.")
            lblStatus.Text = "Código duplicado."
        Catch ex As Exception
            MostrarError("Error al guardar", ex)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtCodigo.TextLength = 0 Then
            lblStatus.Text = "Seleccione un registro."
            Return
        End If
        If MessageBox.Show("¿Confirma eliminar el producto seleccionado?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim sql = "DELETE FROM producto WHERE prod_id=@id;"
                Dim ok = Conexiones.Ejecutarsql(sql, New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@id", txtCodigo.Text.Trim())
                })
                If ok Then
                    CargarListado()
                    LimpiarInputs()
                    DeshabilitarInputs()
                    lblStatus.Text = "Registro eliminado."
                Else
                    lblStatus.Text = "No se eliminó el registro."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MostrarError("No se puede eliminar: el producto está referenciado.", ex)
            Catch ex As Exception
                MostrarError("Error al eliminar", ex)
            End Try
        End If
    End Sub

    ' ====== Validación UI ======
    Private Function ValCodigo() As Boolean
        Dim v = txtCodigo.Text.Trim()
        If v.Length = 0 Then ep.SetError(txtCodigo, "Obligatorio.") : Return False
        If v.Length > 13 Then ep.SetError(txtCodigo, "Máx. 13 caracteres.") : Return False
        ep.SetError(txtCodigo, "") : Return True
    End Function

    Private Function ValNombre() As Boolean
        Dim v = txtNombre.Text.Trim()
        If v.Length = 0 Then ep.SetError(txtNombre, "Obligatorio.") : Return False
        If v.Length > 60 Then ep.SetError(txtNombre, "Máx. 60 caracteres.") : Return False
        ep.SetError(txtNombre, "") : Return True
    End Function

    Private Function ValMarca() As Boolean
        If cboMarca.SelectedIndex < 0 Then ep.SetError(cboMarca, "Seleccione una marca.") : Return False
        ep.SetError(cboMarca, "") : Return True
    End Function

    Private Function ValPrecio() As Boolean
        ' Solo enteros (NUMERIC(20,0)): quitar puntos y validar 1..20 dígitos
        Dim raw = New String(txtPrecio.Text.Where(AddressOf Char.IsDigit).ToArray())
        If raw.Length = 0 Then ep.SetError(txtPrecio, "Ingrese un precio.") : Return False
        If raw.Length > 20 Then ep.SetError(txtPrecio, "Máx. 20 dígitos.") : Return False

        Dim dummy As Decimal
        If Not Decimal.TryParse(raw, NumberStyles.None, CultureInfo.InvariantCulture, dummy) Then
            ep.SetError(txtPrecio, "Número inválido.") : Return False
        End If
        ep.SetError(txtPrecio, "")
        Return True
    End Function

    Private Function ValIVA() As Boolean
        Dim s = NormalizaNumero(txtIva.Text)
        Dim n As Decimal
        If Not Decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, n) Then
            ep.SetError(txtIva, "Ingrese 0, 5 o 10.") : Return False
        End If
        If n <> 0D AndAlso n <> 5D AndAlso n <> 10D Then
            ep.SetError(txtIva, "Solo 0, 5 o 10.") : Return False
        End If
        ep.SetError(txtIva, "") : Return True
    End Function

    Private Function NormalizaNumero(input As String) As String
        Dim s = input.Trim()
        If s.Contains(",") AndAlso Not s.Contains(".") Then s = s.Replace(",", ".")
        Return s
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

    ' ==== Formateo en vivo de txtPrecio (miles, sin decimales) ====
    Private Sub txtPrecio_TextChanged(sender As Object, e As EventArgs) Handles txtPrecio.TextChanged
        FormatThousandsLive(CType(sender, TextBox))
    End Sub

    Private Sub txtPrecio_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrecio.KeyPress
        If Char.IsControl(e.KeyChar) OrElse Char.IsDigit(e.KeyChar) Then Return
        e.Handled = True ' no permitas coma/punto; NUMERIC(20,0)
    End Sub

    Private Sub FormatThousandsLive(txt As TextBox)
        If isFormatting Then Exit Sub
        isFormatting = True

        Dim original As String = txt.Text
        Dim caret As Integer = txt.SelectionStart


        Dim digitsLeft As Integer = 0
        For i = 0 To Math.Min(caret, original.Length) - 1
            If Char.IsDigit(original(i)) Then digitsLeft += 1
        Next


        Dim sb As New StringBuilder()
        For Each ch In original
            If Char.IsDigit(ch) Then sb.Append(ch)
        Next


        Dim sInt As String = sb.ToString()
        Dim formatted As String = GroupThousands(sInt, grpSep)


        If formatted = original Then
            txt.SelectionStart = caret
            txt.SelectionLength = 0
            isFormatting = False
            Exit Sub
        End If

        Dim pos As Integer = 0, seen As Integer = 0
        While pos < formatted.Length AndAlso seen < digitsLeft
            If Char.IsDigit(formatted(pos)) Then seen += 1
            pos += 1
        End While

        txt.Text = formatted
        txt.SelectionStart = Math.Min(pos, txt.TextLength)
        txt.SelectionLength = 0

        isFormatting = False
    End Sub

    Private Function GroupThousands(s As String, sep As Char) As String
        If String.IsNullOrEmpty(s) OrElse s.Length <= 3 Then Return s
        Dim sb As New StringBuilder()
        Dim count As Integer = 0
        For i = s.Length - 1 To 0 Step -1
            sb.Insert(0, s(i))
            count += 1
            If count = 3 AndAlso i > 0 Then
                sb.Insert(0, sep)
                count = 0
            End If
        Next
        Return sb.ToString()
    End Function

End Class
