Option Strict On
Option Infer On
Imports Npgsql

Public Class FrmAjustesTipo
    Private _cargando As Boolean = False

    Private Sub FrmAjustesTipo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        PrepararGrid()
        CargarListado()
        DeshabilitarInputs()
        lblStatus.Text = "Listo."
        AddHandler txtMotivo.TextChanged, Sub() ValMotivo()
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
    Private Sub CargarListado()
        Try
            _cargando = True
            Dim dt = Conexiones.Consulta("
                SELECT ajustp_id, ajustp_motivo
                FROM ajustes_tipo
                ORDER BY ajustp_motivo;")
            DataGridView1.DataSource = dt
            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = Nothing
        Catch ex As Exception
            MostrarError("Error al consultar tipos de ajuste", ex)
        Finally
            _cargando = False
        End Try
    End Sub

    ' ===== Estados UI =====
    Private Sub DeshabilitarInputs()
        txtid.ReadOnly = True
        txtMotivo.ReadOnly = True
        BtnGuardar.Enabled = False : BtnCancelar.Enabled = False
        BtnEditar.Enabled = False : BtnEliminar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        txtMotivo.ReadOnly = False
        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False
        If Not paraEdicion Then
            LimpiarInputs()
            txtMotivo.Focus()
        End If
    End Sub

    Private Sub LimpiarInputs()
        txtid.Clear()
        txtMotivo.Clear()
        ep.Clear()
    End Sub

    ' ===== Grid -> Form =====
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) _
        Handles DataGridView1.CellClick, DataGridView1.CellContentClick
        If _cargando OrElse e.RowIndex < 0 Then Return
        Dim row = DataGridView1.Rows(e.RowIndex)
        txtid.Text = Convert.ToString(TryGetCell(row, {"colId", "ajustp_id"}))
        txtMotivo.Text = Convert.ToString(TryGetCell(row, {"colmotivo", "ajustp_motivo"}))
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

    ' ===== Botones =====
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(False)
        lblStatus.Text = "Modo: Nuevo"
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtid.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        HabilitarInputs(True)
        lblStatus.Text = "Modo: Editar"
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        DeshabilitarInputs()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not ValMotivo() Then
                lblStatus.Text = "Corrija los campos marcados." : Return
            End If

            Dim pars As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@mot", txtMotivo.Text.Trim())
            }

            If txtid.TextLength = 0 Then
                Dim newId = Conexiones.EjecutarsqlScalar("
                    INSERT INTO ajustes_tipo (ajustp_motivo)
                    VALUES (@mot)
                    RETURNING ajustp_id;", pars)
                txtid.Text = Convert.ToString(newId)
                lblStatus.Text = $"Insertado ID {txtid.Text}."
            Else
                pars.Add(New NpgsqlParameter("@id", Integer.Parse(txtid.Text)))
                Dim ok = Conexiones.Ejecutarsql("
                    UPDATE ajustes_tipo
                    SET ajustp_motivo=@mot
                    WHERE ajustp_id=@id;", pars)
                lblStatus.Text = If(ok, "Actualizado.", "Sin cambios.")
            End If
            LimpiarInputs()
            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23503" ' si existiera FK dependiente
            MostrarError("No se puede modificar/eliminar: tipo de ajuste referenciado.", ex)
        Catch ex As Exception
            MostrarError("Error al guardar tipo de ajuste", ex)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtid.TextLength = 0 Then lblStatus.Text = "Seleccione un registro." : Return
        If MessageBox.Show("¿Eliminar el tipo de ajuste seleccionado?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim ok = Conexiones.Ejecutarsql("
                    DELETE FROM ajustes_tipo WHERE ajustp_id=@id;",
                    New List(Of NpgsqlParameter) From {New NpgsqlParameter("@id", Integer.Parse(txtid.Text))})
                If ok Then
                    CargarListado() : DeshabilitarInputs() : lblStatus.Text = "Eliminado."
                Else
                    lblStatus.Text = "No se eliminó."
                End If
            Catch ex As PostgresException When ex.SqlState = "23503"
                MostrarError("No se puede eliminar: en uso por otra tabla (FK).", ex)
            Catch ex As Exception
                MostrarError("Error al eliminar tipo de ajuste", ex)
            End Try
        End If
    End Sub

    ' ===== Validación =====
    Private Function ValMotivo() As Boolean
        Dim v = txtMotivo.Text.Trim()
        If v.Length = 0 Then ep.SetError(txtMotivo, "Obligatorio.") : Return False
        If v.Length > 45 Then ep.SetError(txtMotivo, "Máx. 45 caracteres.") : Return False
        ep.SetError(txtMotivo, "") : Return True
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
