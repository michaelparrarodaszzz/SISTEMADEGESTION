
Option Strict On
Option Infer On

Imports Npgsql

Public Class FrmCiudad

    Private Sub FrmCiudad_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            CargarDepartamentos()
            CargarListado()
            DeshabilitarInputs()
            lblStatus.Text = "Listo."


            AddHandler txtNombre.TextChanged, Sub() ValNombre()
            AddHandler cbodep.SelectedIndexChanged, Sub() ValDep()

        Catch ex As Exception
            MostrarError("Error al iniciar", ex)
        End Try
    End Sub

    ' ========= Listado =========
    Private Sub CargarListado()

        Dim dt = Conexiones.Consulta("
                SELECT c.ciu_id,
                       c.ciu_nombre,
                       c.departamento_dep_id,
                       d.dep_nombre
                FROM ciudad c
                JOIN departamento d ON d.dep_id = c.departamento_dep_id
                ORDER BY c.ciu_id;")
        DataGridView1.DataSource = dt
    End Sub

    Private Sub CargarDepartamentos()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT dep_id, dep_nombre
                FROM departamento
                ORDER BY dep_nombre;")
            cboDep.DataSource = dt
            cboDep.ValueMember = "dep_id"
            cboDep.DisplayMember = "dep_nombre"
            cboDep.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Error al cargar departamentos", ex)
        End Try
    End Sub

    ' ========= Estados / Habilitación =========
    Private Sub DeshabilitarInputs()
        txtid.ReadOnly = True : txtid.TabStop = False
        txtNombre.ReadOnly = True : txtNombre.BackColor = Drawing.Color.FromArgb(248, 248, 248)
        cboDep.Enabled = False
        BtnGuardar.Enabled = False : BtnEditar.Enabled = False
        BtnEliminar.Enabled = False : BtnCancelar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub HabilitarInputs(paraEdicion As Boolean)
        txtNombre.ReadOnly = False : txtNombre.BackColor = Drawing.Color.White
        cboDep.Enabled = True
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
        If cboDep.Items.Count > 0 Then cboDep.SelectedIndex = -1
        ep.Clear()
    End Sub

    ' ========= Grid: seleccionar fila =========
    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex < 0 Then Return
        Dim row = DataGridView1.Rows(e.RowIndex)

        Dim vId = TryGetCell(row, {"colId", "ciu_id"})
        Dim vNom = TryGetCell(row, {"colNombre", "ciu_nombre"})
        Dim vDepNombre = TryGetCell(row, {"colDep", "dep_nombre"})
        Dim vDepId = TryGetCell(row, {"departamento_dep_id"})

        txtid.Text = Convert.ToString(vId)
        txtNombre.Text = Convert.ToString(vNom)

        ' Intentar seleccionar por id (más confiable). Si no viene, intentamos por nombre.
        If vDepId IsNot Nothing AndAlso vDepId IsNot DBNull.Value Then
            cboDep.SelectedValue = CInt(vDepId)
        Else
            SeleccionarDepartamentoPorNombre(Convert.ToString(vDepNombre))
        End If

        ' Modo visualización al seleccionar
        txtNombre.ReadOnly = True
        cboDep.Enabled = False
        BtnEditar.Enabled = True : BtnEliminar.Enabled = True
        BtnGuardar.Enabled = False : BtnCancelar.Enabled = False
        BtnNuevo.Enabled = True
        ep.Clear()
    End Sub

    Private Sub SeleccionarDepartamentoPorNombre(depNombre As String)
        If String.IsNullOrWhiteSpace(depNombre) Then
            cboDep.SelectedIndex = -1 : Return
        End If
        For i = 0 To cboDep.Items.Count - 1
            Dim drv = TryCast(cboDep.Items(i), DataRowView)
            If drv IsNot Nothing AndAlso
               String.Equals(Convert.ToString(drv("dep_nombre")), depNombre, StringComparison.OrdinalIgnoreCase) Then
                cboDep.SelectedIndex = i : Exit For
            End If
        Next
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
            If Not (ValNombre() AndAlso ValDep()) Then
                lblStatus.Text = "Corrija los campos marcados."
                Return
            End If

            Dim pars = New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@nombre", txtNombre.Text.Trim()),
                New NpgsqlParameter("@dep", CInt(cboDep.SelectedValue))
            }

            If String.IsNullOrWhiteSpace(txtid.Text) Then
                ' INSERT y devolver nuevo ID
                Dim sql = "
                    INSERT INTO ciudad(ciu_nombre, departamento_dep_id)
                    VALUES (@nombre, @dep)
                    RETURNING ciu_id;"
                Dim newId = Conexiones.EjecutarsqlScalar(sql, pars)
                txtid.Text = Convert.ToString(newId)
                lblStatus.Text = $"Insertado ID {txtid.Text}."
            Else
                ' UPDATE
                Dim sql = "
                    UPDATE ciudad
                    SET ciu_nombre=@nombre, departamento_dep_id=@dep
                    WHERE ciu_id=@id;"
                pars.Add(New NpgsqlParameter("@id", Integer.Parse(txtid.Text)))
                Dim ok = Conexiones.Ejecutarsql(sql, pars)
                lblStatus.Text = If(ok, "Registro actualizado.", "No se actualizó ningún registro.")
            End If

            CargarListado()
            DeshabilitarInputs()

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
                Dim sql = "DELETE FROM ciudad WHERE ciu_id=@id;"
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
            Catch ex As Exception
                MostrarError("Error al eliminar", ex)
            End Try
        End If
    End Sub

    ' ========= Validación UI (ErrorProvider) =========
    Private Function ValNombre() As Boolean
        Dim v = txtNombre.Text.Trim()
        If v.Length = 0 Then ep.SetError(txtNombre, "Obligatorio.") : Return False
        If v.Length > 40 Then ep.SetError(txtNombre, "Máx. 40 caracteres.") : Return False
        ep.SetError(txtNombre, "") : Return True
    End Function

    Private Function ValDep() As Boolean
        If cboDep.SelectedIndex < 0 Then ep.SetError(cboDep, "Seleccione un departamento.") : Return False
        ep.SetError(cboDep, "") : Return True
    End Function

    ' ========= Errores amigables =========
    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbodep.SelectedIndexChanged

    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()

    End Sub
End Class
