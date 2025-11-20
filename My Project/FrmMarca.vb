' FrmMarca.vb (solo lógica; los controles se crean en el Diseñador)
Option Strict On
Option Infer On

Imports Npgsql

Public Class FrmMarca

    Private Sub FrmMarca_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            PrepararGridOpcional()   ' deja vacío si AutoGenerateColumns=True
            CargarListado()
            DeshabilitarInputs()
            lblStatus.Text = "Listo."
            ' Validación en tiempo real
            AddHandler txtNombre.TextChanged, Sub() ValNombre()
        Catch ex As Exception
            MostrarError("Error al iniciar", ex)
        End Try
    End Sub

    ' ====== Listado ======
    Private Sub CargarListado()
        Dim dt = Conexiones.Consulta("
                SELECT mar_id, mar_nombre
                FROM marca
                ORDER BY mar_id;")
        DataGridView1.DataSource = dt


    End Sub

    Private Sub PrepararGridOpcional()
        DataGridView1.AutoGenerateColumns = False
    End Sub

    ' ====== Habilitación / Estados ======
    Private Sub DeshabilitarInputs()
        txtid.ReadOnly = True : txtid.TabStop = False
        txtNombre.ReadOnly = True : txtNombre.BackColor = Drawing.Color.FromArgb(248, 248, 248)
        BtnGuardar.Enabled = False : BtnEditar.Enabled = False
        BtnEliminar.Enabled = False : BtnCancelar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        txtNombre.ReadOnly = False : txtNombre.BackColor = Drawing.Color.White
        BtnGuardar.Enabled = True : BtnCancelar.Enabled = True
        BtnNuevo.Enabled = False
        BtnEliminar.Enabled = paraEdicion
        BtnEditar.Enabled = False
        If Not paraEdicion Then
            LimpiarInputs()
        End If
        txtNombre.Focus()
    End Sub

    Private Sub LimpiarInputs()
        txtid.Clear()
        txtNombre.Clear()
        ep.Clear()
    End Sub

    ' ====== Grid: seleccionar fila ======
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Return
        Dim row = DataGridView1.Rows(e.RowIndex)

        Dim vId = TryGetCell(row, {"colId", "mar_id"})
        Dim vNom = TryGetCell(row, {"colNombre", "mar_nombre"})

        txtid.Text = Convert.ToString(vId)
        txtNombre.Text = Convert.ToString(vNom)

        ' Modo visualización
        txtNombre.ReadOnly = True
        BtnEditar.Enabled = True : BtnEliminar.Enabled = True
        BtnGuardar.Enabled = False : BtnCancelar.Enabled = False
        BtnNuevo.Enabled = True
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

    ' ====== Botones ======
    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles BtnNuevo.Click
        HabilitarInputs(paraEdicion:=False)
        lblStatus.Text = "Modo: Nuevo"
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles BtnEditar.Click
        If txtid.TextLength = 0 Then
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
            If Not ValNombre() Then
                lblStatus.Text = "Corrija los campos marcados."
                Return
            End If

            Dim pars = New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@nombre", txtNombre.Text.Trim())
            }

            If String.IsNullOrWhiteSpace(txtid.Text) Then
                ' INSERT con RETURNING
                Dim sql = "
                    INSERT INTO marca(mar_nombre)
                    VALUES (@nombre)
                    RETURNING mar_id;"
                Dim newId = Conexiones.EjecutarsqlScalar(sql, pars)
                txtid.Text = Convert.ToString(newId)
                lblStatus.Text = $"Insertado ID {txtid.Text}."
            Else
                ' UPDATE
                Dim sql = "
                    UPDATE marca
                    SET mar_nombre=@nombre
                    WHERE mar_id=@id;"
                pars.Add(New NpgsqlParameter("@id", Integer.Parse(txtid.Text)))
                Dim ok = Conexiones.Ejecutarsql(sql, pars)
                lblStatus.Text = If(ok, "Registro actualizado.", "No se actualizó ningún registro.")
            End If

            CargarListado()
            DeshabilitarInputs()

        Catch ex As PostgresException When ex.SqlState = "23505" ' unique_violation
            ep.SetError(txtNombre, "Ya existe una marca con ese nombre.")
            lblStatus.Text = "Violación de unicidad."
        Catch ex As Exception
            MostrarError("Error al guardar", ex)
        End Try
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        If txtid.TextLength = 0 Then
            lblStatus.Text = "Seleccione un registro."
            Return
        End If
        If MessageBox.Show("¿Confirma eliminar el registro seleccionado?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
            Try
                Dim sql = "DELETE FROM marca WHERE mar_id=@id;"
                Dim ok = Conexiones.Ejecutarsql(sql, New List(Of NpgsqlParameter) From {
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
            Catch ex As PostgresException When ex.SqlState = "23503" ' foreign_key_violation
                MostrarError("No se puede eliminar: está referenciada por productos.", ex)
            Catch ex As Exception
                MostrarError("Error al eliminar", ex)
            End Try
        End If
    End Sub

    ' ====== Validación UI ======
    Private Function ValNombre() As Boolean
        Dim v = txtNombre.Text.Trim()
        If v.Length = 0 Then ep.SetError(txtNombre, "Obligatorio.") : Return False
        If v.Length > 45 Then ep.SetError(txtNombre, "Máx. 45 caracteres.") : Return False
        ep.SetError(txtNombre, "") : Return True
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
End Class
