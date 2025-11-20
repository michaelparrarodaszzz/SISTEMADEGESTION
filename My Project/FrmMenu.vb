
Option Strict On
Option Infer On
Public Class FrmMenu
    Private Sub MonedaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MonedaToolStripMenuItem.Click
        FrmMoneda.Show()
    End Sub

    Private Sub CobrotipoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CobrotipoToolStripMenuItem.Click
        FrmCobroTipo.Show()
    End Sub

    Private Sub AjustestipoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AjustestipoToolStripMenuItem.Click
        FrmAjustesTipo.Show()
    End Sub

    Private Sub CajaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CajaToolStripMenuItem.Click
        FrmCaja.Show()
    End Sub

    Private Sub DepartamentoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DepartamentoToolStripMenuItem.Click
        FrmDepartamento.Show()
    End Sub

    Private Sub CiudadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CiudadToolStripMenuItem.Click
        FrmCiudad.Show()
    End Sub

    Private Sub ClienteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClienteToolStripMenuItem.Click
        FrmCliente.Show()
    End Sub

    Private Sub ProveedorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProveedorToolStripMenuItem.Click
        FrmProveedor.Show()
    End Sub

    Private Sub EmpleadoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EmpleadoToolStripMenuItem.Click
        FrmEmpleado.Show()
    End Sub

    Private Sub PagoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PagoToolStripMenuItem.Click
        FrmPago.Show()
    End Sub

    Private Sub DevoluciónAProveedorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DevoluciónAProveedorToolStripMenuItem.Click
        FrmDevolucionProveedor.Show()

    End Sub

    Private Sub UsuarioToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UsuarioToolStripMenuItem.Click
        FrmUsuario.Show()
    End Sub

    Private Sub SalirToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles SalirToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub MarcaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MarcaToolStripMenuItem.Click
        FrmMarca.Show()
    End Sub

    Private Sub ProductoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ProductoToolStripMenuItem.Click
        FrmProducto.Show()
    End Sub

    Private Sub PedidoAProveedorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PedidoAProveedorToolStripMenuItem.Click
        FrmPedidoProveedor.Show()
    End Sub

    Private Sub OrdenDeCompraToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OrdenDeCompraToolStripMenuItem.Click
        FrmOrdenCompra.Show()
    End Sub

    Private Sub CompraToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CompraToolStripMenuItem.Click
        FrmCompra.Show()
    End Sub

    Private Sub CuentaAPagarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CuentaAPagarToolStripMenuItem.Click
        FrmCuentaPagar.Show()
    End Sub

    Private Sub OrdenDePagoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OrdenDePagoToolStripMenuItem.Click
        FrmOrdenPago.Show()
    End Sub
    Private Sub FrmMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.IsMdiContainer = True
        lblStatus.Text = "Listo."
        MostrarLogin()   ' pide login al iniciar
    End Sub

    ' ---- login modal y preparación de sesión ----
    Private Sub MostrarLogin()
        CerrarHijos()

        Do
            Using f As New FrmLogin()
                Dim r = f.ShowDialog(Me)   ' modal
                If r <> DialogResult.OK Then
                    Me.Close()             ' canceló login -> cerrar app
                    Return
                End If
            End Using

            If AppSession.IsLoggedIn Then
                lblStatus.Text = $"Bienvenido {AppSession.CurrentUser.Nombre} ({AppSession.CurrentUser.Rol})"
                AplicarPermisos(AppSession.CurrentUser.Rol)
                Exit Do
            End If
        Loop
    End Sub

    Private Sub CerrarHijos()
        For Each f As Form In Me.MdiChildren
            Try : f.Close() : Catch : End Try
        Next
    End Sub

    Private Sub AplicarPermisos(rol As String)
        Dim isAdmin = (rol = "ADMIN")
        ' habilita/oculta menús según tu necesidad:
        UsuarioToolStripMenuItem.Visible = isAdmin
        AuditoriaToolStripMenuItem.Visible = isAdmin
        ' etc.
    End Sub

    ' ---- handler del menú “Cerrar sesión” ----
    Private Sub mnuCerrarSesion_Click(sender As Object, e As EventArgs) Handles CerrarSesionToolStripMenuItem.Click
        If MessageBox.Show("¿Cerrar la sesión actual?", "Confirmación",
                           MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

            CerrarHijos()
            AppSession.Logout()

            ' reset visual
            lblStatus.Text = "Sesión cerrada."
            AplicarPermisos("")

            ' pedir login otra vez
            MostrarLogin()
        End If
    End Sub

    Private Sub PresupuestoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PresupuestoToolStripMenuItem.Click
        FrmPresupuesto.Show()
    End Sub

    Private Sub PedidoClienteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PedidoClienteToolStripMenuItem.Click
        FrmPedidoCliente.Show()
    End Sub

    Private Sub VentaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VentaToolStripMenuItem.Click
        FrmVenta.Show()
    End Sub

    Private Sub CobroToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CobroToolStripMenuItem.Click
        FrmCobro.Show()
    End Sub

    Private Sub CotizacionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CotizacionToolStripMenuItem.Click
        FrmCotizacion.Show()
    End Sub

    Private Sub ArqueoDeCajaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ArqueoDeCajaToolStripMenuItem.Click
        FrmArqueo.Show()
    End Sub

    Private Sub CuentaACobrarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CuentaACobrarToolStripMenuItem.Click
        FrmCuentaCobrar.Show()
    End Sub


End Class