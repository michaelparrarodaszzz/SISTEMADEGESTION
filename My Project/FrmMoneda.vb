Option Strict On
Option Infer On
Imports Npgsql

Public Class FrmMoneda
    Private _cargando As Boolean = False

    Private Sub FrmMoneda_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PrepararGrid()
        CargarListado()
        DeshabilitarInputs()
        lblStatus.Text = "Listo."
        AddHandler txtDescripcion.TextChanged, Sub() ValDescripcion()
        AddHandler txtSigla.TextChanged, Sub()
                                             txtSigla.Text = txtSigla.Text.ToUpperInvariant()
                                             txtSigla.SelectionStart = txtSigla.TextLength
                                             ValSigla()
                                         End Sub
    End Sub

    ' === Grid ===
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

    ' === Data ===
    Private Sub CargarListado()
        Try
            _cargando = True
            Dim dt = Conexiones.Consulta("
                SELECT mon_id, mon_descripcion, mon_sigla
                FROM moneda
                ORDER BY mon_descripcion;")
            DataGridView1.DataSource = dt
            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = Nothing
        Catch ex As Exception
            MostrarError("Error al consultar monedas", ex)
        Finally
            _cargando = False
        End Try
    End Sub

    ' === Estados UI ===
    Private Sub DeshabilitarInputs()
        txtid.ReadOnly = True
        txtDescripcion.ReadOnly = True
        txtSigla.ReadOnly = True
        BtnGuardar.Enabled = False : BtnCancelar.Enabled = False
        BtnEditar.Enabled = False : BtnEliminar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        txtDescripcion.ReadOnly = False
        txtSigla.ReadOnly = False
        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False
        If Not paraEdicion Then
            LimpiarInputs()
            txtDescripcion.Focus()
        End If
    End Sub

    Private Sub LimpiarInputs()
        txtid.Clear()
        txtDescripcion.Clear()
        txtSigla.Clear()
        ep.Clear()
    End Sub

    ' === Grid -> Form ===
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellClick, DataGridView1.CellContentClick
        If _cargando OrElse e.RowIndex < 0 Then Return
        Dim row = DataGridView1.Rows(e.RowIndex)
        txtid.Text = Convert.ToString(TryGetCell(row, {"colId", "mon_id"}))
        txtDescripcion.Text = Convert.ToString(TryGetCell(row, {"colDescripcion", "mon_descripcion"}))
        txtSigla.Text = Convert.ToString(TryGetCell(row, {"colSigla", "mon_sigla"})).ToUpperInvariant()
        DeshabilitarInputs()
        BtnEditar.Enabled = True : BtnEliminar.Enabled = True
        lblStatus.Text = "Registro seleccionado."
    End Sub

    Private Function TryGetCell(row As DataGridViewRow, names() As String) As Object
        For Each n In names
            If n Is Nothing Then Continue For
            If DataGridView1.Columns.Contains(n) Then
                Return row.Cells(n).Value
            End If
        Next
        Return Nothing
    End Function

    ' === Botones ===
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False)
        lblStatus.Text = "Modo: Nuevo"
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtid.TextLength = 0 Then lblStatus.Text = "Seleccione una moneda." : Return
        HabilitarInputs(True)
        lblStatus.Text = "Modo: Editar"
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValDescripcion() AndAlso ValSigla()) Then
                lblStatus.Text = "Corrija los campos marcados." : Return
            End If

            Dim pars As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@des", txtDescripcion.Text.Trim()),
                New NpgsqlParameter("@sig", txtSigla.Text.Trim().ToUpperInvariant())
            }

            If txtid.TextLength = 0 Then
                Dim newId = Conexiones.EjecutarsqlScalar("
                    INSERT INTO moneda (mon_descripcion, mon_sigla)
                    VALUES (@des, @sig)
                    RETURNING mon_id;", pars)
                txtid.Text = Convert.ToString(newId)
                lblStatus.Text = $"Insertado ID {txtid.Text}."
            Else
                pars.Add(New NpgsqlParameter("@id", Integer.Parse(txtid.Text)))
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE moneda
                    SET mon_descripcion=@des, mon_sigla=@sig
                    WHERE mon_id=@id;", pars)
                lblStatus.Text = If(ok, "Actualizado.", "Sin cambios.")
            End If
            LimpiarInputs()
            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23503" ' referenciada (cotizacion/cuenta_a_pagar)
            MostrarError("No se puede modificar: moneda referenciada.", ex)
        Catch ex As Exception
            MostrarError("Error al guardar moneda", ex)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtid.TextLength = 0 Then lblStatus.Text = "Seleccione una moneda." : Return
        If MessageBox.Show("¿Eliminar la moneda seleccionada?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM moneda WHERE mon_id=@id;",
                    New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", Integer.Parse(txtid.Text))})
                If ok Then
                    CargarListado() : DeshabilitarInputs() : lblStatus.Text = "Eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MostrarError("No se puede eliminar: moneda referenciada.", ex)
            Catch ex As Exception
                MostrarError("Error al eliminar moneda", ex)
            End Try
        End If
    End Sub

    ' === Validaciones ===
    Private Function ValDescripcion() As Boolean
        Dim v = txtDescripcion.Text.Trim()
        If v.Length = 0 Then ep.SetError(txtDescripcion, "Obligatorio.") : Return False
        If v.Length > 20 Then ep.SetError(txtDescripcion, "Máx. 20 caracteres.") : Return False
        ep.SetError(txtDescripcion, "") : Return True
    End Function

    Private Function ValSigla() As Boolean
        Dim v = txtSigla.Text.Trim().ToUpperInvariant()
        If v.Length = 0 Then ep.SetError(txtSigla, "Obligatorio.") : Return False
        If v.Length > 5 Then ep.SetError(txtSigla, "Máx. 5 caracteres.") : Return False
        ep.SetError(txtSigla, "") : Return True
    End Function

    ' === Errores amigables ===
    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub
End Class
