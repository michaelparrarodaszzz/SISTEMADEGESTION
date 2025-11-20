Option Strict On
Option Infer On
Imports Npgsql
Imports NpgsqlTypes
Imports System.Globalization

Public Class FrmAuditoria

    Private Sub FrmAuditoria_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            DataGridView1.AutoGenerateColumns = False

            ' Fechas por defecto: últimos 7 días
            dtpHasta.Value = Date.Today
            dtpDesde.Value = Date.Today.AddDays(-7)

            CargarUsuarios()
            CargarTablas()
            CargarAcciones()
            CargarListado()
            lblStatus.Text = "Listo."
        Catch ex As Exception
            MostrarError("Error al iniciar", ex)
        End Try
    End Sub

    '=========== CARGA DE COMBOS ===========

    Private Sub CargarUsuarios()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT usua_id,
                       usua_usuario
                  FROM usuario
              ORDER BY usua_usuario;")

            cboUsuario.DataSource = dt
            cboUsuario.DisplayMember = "usua_usuario"
            cboUsuario.ValueMember = "usua_id"
            cboUsuario.SelectedIndex = -1
        Catch ex As Exception
            MostrarError("Error al cargar usuarios", ex)
        End Try
    End Sub

    Private Sub CargarTablas()
        Try
            Dim dt = Conexiones.Consulta("
                SELECT DISTINCT aud_tabla
                  FROM auditoria
              ORDER BY aud_tabla;")

            cboTabla.DataSource = dt
            cboTabla.DisplayMember = "aud_tabla"
            cboTabla.ValueMember = "aud_tabla"
            cboTabla.SelectedIndex = -1
        Catch ex As Exception
            ' Si todavía no hay registros de auditoría, no pasa nada
            cboTabla.DataSource = Nothing
        End Try
    End Sub

    Private Sub CargarAcciones()
        cboAccion.Items.Clear()
        cboAccion.Items.AddRange(New Object() {"INSERT", "UPDATE", "DELETE"})
        cboAccion.SelectedIndex = -1
    End Sub

    '=========== LISTADO ===========

    Private Sub CargarListado()
        Try
            ' Validar rango de fechas
            If dtpDesde.Value.Date > dtpHasta.Value.Date Then
                ep.SetError(dtpHasta, "La fecha 'Hasta' debe ser mayor o igual que 'Desde'.")
                lblStatus.Text = "Rango de fechas inválido."
                Return
            Else
                ep.SetError(dtpHasta, "")
            End If

            Dim sql As String = "
                SELECT a.aud_id,
                       a.aud_tabla,
                       a.aud_accion,
                       a.aud_fecha,
                       a.aud_hora,
                       u.usua_usuario
                  FROM auditoria a
             LEFT JOIN usuario u
                    ON u.usua_id = a.usuario_usua_id
                 WHERE a.aud_fecha BETWEEN @desde AND @hasta
                   AND (@tabla IS NULL OR a.aud_tabla = @tabla)
                   AND (@accion IS NULL OR a.aud_accion = @accion)
                   AND (@userId IS NULL OR a.usuario_usua_id = @userId)
              ORDER BY a.aud_fecha DESC,
                       a.aud_hora DESC,
                       a.aud_id DESC;"

            Dim pars As New List(Of NpgsqlParameter) From {
                New NpgsqlParameter("@desde", NpgsqlDbType.Date) With {.Value = dtpDesde.Value.Date},
                New NpgsqlParameter("@hasta", NpgsqlDbType.Date) With {.Value = dtpHasta.Value.Date}
            }

            ' Tabla
            Dim tablaVal As Object = If(cboTabla.SelectedIndex >= 0,
                                        CType(cboTabla.SelectedValue, Object),
                                        CType(DBNull.Value, Object))
            pars.Add(New NpgsqlParameter("@tabla", NpgsqlDbType.Varchar) With {.Value = tablaVal})

            ' Acción
            Dim accVal As Object = If(cboAccion.SelectedIndex >= 0,
                                      CType(cboAccion.SelectedItem.ToString(), Object),
                                      CType(DBNull.Value, Object))
            pars.Add(New NpgsqlParameter("@accion", NpgsqlDbType.Varchar) With {.Value = accVal})

            ' Usuario
            Dim usrVal As Object
            If cboUsuario.SelectedIndex >= 0 Then
                usrVal = CInt(cboUsuario.SelectedValue)
            Else
                usrVal = CType(DBNull.Value, Object)
            End If
            pars.Add(New NpgsqlParameter("@userId", NpgsqlDbType.Integer) With {.Value = usrVal})

            Dim dt = Conexiones.Consulta(sql, pars)
            DataGridView1.DataSource = dt
            DataGridView1.ClearSelection()
            DataGridView1.CurrentCell = Nothing

            lblStatus.Text = $"Registros: {dt.Rows.Count}"

        Catch ex As Exception
            MostrarError("Error al consultar auditoría", ex)
        End Try
    End Sub

    '=========== BOTONES ===========

    Private Sub BtnBuscar_Click(sender As Object, e As EventArgs) Handles BtnBuscar.Click
        CargarListado()
    End Sub

    Private Sub BtnLimpiar_Click(sender As Object, e As EventArgs) Handles BtnLimpiar.Click
        dtpHasta.Value = Date.Today
        dtpDesde.Value = Date.Today.AddDays(-7)
        If cboTabla.Items.Count > 0 Then cboTabla.SelectedIndex = -1
        If cboUsuario.Items.Count > 0 Then cboUsuario.SelectedIndex = -1
        cboAccion.SelectedIndex = -1
        ep.Clear()
        CargarListado()
        lblStatus.Text = "Filtros limpios."
    End Sub

    Private Sub BtnCerrar_Click(sender As Object, e As EventArgs) Handles BtnCerrar.Click
        Me.Close()
    End Sub

    '=========== ERRORES ===========

    Private Sub MostrarError(prefix As String, ex As Exception)
        lblStatus.Text = $"Error: {prefix}."
        MessageBox.Show($"{prefix}:{Environment.NewLine}{ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
