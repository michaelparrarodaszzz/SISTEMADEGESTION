Option Strict On
Option Infer On

Imports Npgsql
Public Class FrmDepartamento

    Private Sub FrmDepartamento_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            DeshabilitarInputs()
            CargarListado()
            lblStatus.Text = "Listo."

            AddHandler txtNombre.TextChanged, Sub() ValNombre()
            AddHandler txtCapital.TextChanged, Sub() ValCapital()
        Catch ex As Exception
            MostrarError("Error al iniciar", ex)
        End Try
    End Sub

    Private Sub CargarListado()
        Try
            Dim dt = Conexiones.consulta("
                SELECT dep_id, dep_nombre, dep_capital
                FROM departamento
                ORDER BY dep_id;")
            DataGridView1.DataSource = dt
        Catch ex As Exception
            MostrarError("Error al consultar departamentos", ex)
        End Try
    End Sub
    ' ========= Habilitación / Limpieza =========
    Private Sub DeshabilitarInputs()
        txtId.ReadOnly = True : txtId.TabStop = False
        txtNombre.ReadOnly = True : txtNombre.TabStop = False
        txtCapital.ReadOnly = True : txtCapital.TabStop = False
        BtnGuardar.Enabled = False
        BtnEliminar.Enabled = False
        BtnNuevo.Enabled = True
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        txtNombre.ReadOnly = False : txtNombre.BackColor = Drawing.Color.White
        txtCapital.ReadOnly = False : txtCapital.BackColor = Drawing.Color.White
        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False
        If Not paraEdicion Then
            txtid.Clear()
            txtNombre.Clear()
            txtCapital.Clear()
            ep.Clear()
        End If
        txtNombre.Focus()
    End Sub

    ' ========= Grid: seleccionar fila =========
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Return
        Dim row = DataGridView1.Rows(e.RowIndex)
        Dim vId = TryGetCell(row, {"colId", "dep_id"})
        Dim vNom = TryGetCell(row, {"colNombre", "dep_nombre"})
        Dim vCap = TryGetCell(row, {"colCapital", "dep_capital"})

        txtid.Text = Convert.ToString(vId)
        txtNombre.Text = Convert.ToString(vNom)
        txtCapital.Text = Convert.ToString(vCap)

        ' Al seleccionar, quedamos en modo visualización; permito Editar/Eliminar
        txtNombre.ReadOnly = True : txtCapital.ReadOnly = True
        BtnEditar.Enabled = True : BtnEliminar.Enabled = True
        BtnNuevo.Enabled = True
        BtnGuardar.Enabled = False : BtnCancelar.Enabled = False
        ep.Clear()
    End Sub

    Private Function TryGetCell(row As DataGridViewRow, names() As String) As Object
        For Each n In names
            If DataGridView1.Columns.Contains(n) Then
                Return row.Cells(n).Value
            End If
        Next
        Return Nothing
    End Function

    ' ========= Botones =========
    Private Sub BtnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(paraEdicion:=False)
        lblStatus.Text = "Modo: Nuevo"
    End Sub

    Private Sub BtnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtid.TextLength = 0 Then
            lblStatus.Text = "Seleccione un registro."
            Return
        End If
        HabilitarInputs(paraEdicion:=True)
        lblStatus.Text = "Modo: Editar"
    End Sub

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles BtnCancelar.Click
        txtid.Clear()
        txtNombre.Clear()
        txtCapital.Clear()
        DeshabilitarInputs()
        ep.Clear()
        lblStatus.Text = "Cancelado."
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Try
            If Not (ValNombre() AndAlso ValCapital()) Then
                lblStatus.Text = "Corrija los campos marcados."
                Return
            End If

            Dim pars = New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@nombre", txtNombre.Text.Trim()),
                New NpgsqlParameter("@capital", If(String.IsNullOrWhiteSpace(txtCapital.Text), CType(DBNull.Value, Object), txtCapital.Text.Trim()))
            }

            If String.IsNullOrWhiteSpace(txtid.Text) Then
                ' INSERT (dep_id es SERIAL). Traemos el nuevo id con RETURNING.
                Dim sql = "
                    INSERT INTO departamento(dep_nombre, dep_capital)
                    VALUES (@nombre, @capital)
                    RETURNING dep_id;"
                Dim newIdObj = Conexiones.ejecutarsqlScalar(sql, pars)
                If newIdObj IsNot Nothing AndAlso newIdObj IsNot DBNull.Value Then
                    txtid.Text = newIdObj.ToString()
                    lblStatus.Text = $"Insertado ID {txtid.Text}."
                Else
                    lblStatus.Text = "Insert realizado, pero no se obtuvo el ID."
                End If
            Else
                ' UPDATE
                Dim sql = "
                    UPDATE departamento
                    SET dep_nombre=@nombre, dep_capital=@capital
                    WHERE dep_id=@id;"
                pars.Add(New NpgsqlParameter("@id", Integer.Parse(txtid.Text)))
                Dim ok = Conexiones.ejecutarsql(sql, pars)
                lblStatus.Text = If(ok, "Registro actualizado.", "No se actualizó ningún registro.")
            End If
            txtid.Clear()
            txtNombre.Clear()
            txtCapital.Clear()
            ep.Clear()
            CargarListado()
            DeshabilitarInputs()

        Catch ex As Exception
            MostrarError("Error al guardar", ex)
        End Try
    End Sub

    Private Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtid.TextLength = 0 Then
            lblStatus.Text = "Seleccione un registro."
            Return
        End If
        If MessageBox.Show("¿Confirma eliminar el registro seleccionado?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim sql = "DELETE FROM departamento WHERE dep_id=@id;"
                Dim ok = Conexiones.ejecutarsql(sql, New List(Of NpgsqlParameter) From {
                    New NpgsqlParameter("@id", Integer.Parse(txtid.Text))
                })
                If ok Then
                    CargarListado()
                    LimpiarInputs()
                    DeshabilitarInputs()
                    lblStatus.Text = "Registro eliminado."
                Else
                    lblStatus.Text = "No se eliminó el registro."
                End If
            Catch ex As Exception
                MostrarError("Error al eliminar", ex)
            End Try
        End If
    End Sub

    Private Sub LimpiarInputs()
        txtId.Clear()
        txtNombre.Clear()
        txtCapital.Clear()
        ep.Clear()
    End Sub

    ' ========= Validación UI (ErrorProvider) =========
    Private Function ValNombre() As Boolean
        Dim v = txtNombre.Text.Trim()
        If v.Length = 0 Then ep.SetError(txtNombre, "Obligatorio.") : Return False
        If v.Length > 30 Then ep.SetError(txtNombre, "Máx. 30 caracteres.") : Return False
        ep.SetError(txtNombre, "") : Return True
    End Function

    Private Function ValCapital() As Boolean
        Dim v = txtCapital.Text.Trim()
        If v.Length > 30 Then ep.SetError(txtCapital, "Máx. 30 caracteres.") : Return False
        ep.SetError(txtCapital, "") : Return True
    End Function

    ' ========= Errores amigables =========
    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub
End Class