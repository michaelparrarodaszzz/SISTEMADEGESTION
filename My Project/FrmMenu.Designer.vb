<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmMenu
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.CatalogosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MonedaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CobrotipoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AjustestipoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CajaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CotizacionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ArqueoDeCajaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PersonasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClienteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProveedorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EmpleadoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UsuarioToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProductosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MarcaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProductoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UbicacionesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DepartamentoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CiudadToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ComprasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PedidoAProveedorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OrdenDeCompraToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CompraToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CuentaACobrarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CuentaAPagarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OrdenDePagoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PagoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DevoluciónAProveedorToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VentasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PresupuestoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PedidoClienteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VentaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CobroToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SistemaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CerrarSesionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SalirToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AuditoriaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CatalogosToolStripMenuItem, Me.PersonasToolStripMenuItem, Me.ProductosToolStripMenuItem, Me.UbicacionesToolStripMenuItem, Me.ComprasToolStripMenuItem, Me.VentasToolStripMenuItem, Me.SistemaToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1370, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'CatalogosToolStripMenuItem
        '
        Me.CatalogosToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MonedaToolStripMenuItem, Me.CobrotipoToolStripMenuItem, Me.AjustestipoToolStripMenuItem, Me.CajaToolStripMenuItem, Me.CotizacionToolStripMenuItem, Me.ArqueoDeCajaToolStripMenuItem})
        Me.CatalogosToolStripMenuItem.Name = "CatalogosToolStripMenuItem"
        Me.CatalogosToolStripMenuItem.Size = New System.Drawing.Size(72, 20)
        Me.CatalogosToolStripMenuItem.Text = "Catalogos"
        '
        'MonedaToolStripMenuItem
        '
        Me.MonedaToolStripMenuItem.Name = "MonedaToolStripMenuItem"
        Me.MonedaToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.MonedaToolStripMenuItem.Text = "Moneda"
        '
        'CobrotipoToolStripMenuItem
        '
        Me.CobrotipoToolStripMenuItem.Name = "CobrotipoToolStripMenuItem"
        Me.CobrotipoToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.CobrotipoToolStripMenuItem.Text = "Cobro_tipo"
        '
        'AjustestipoToolStripMenuItem
        '
        Me.AjustestipoToolStripMenuItem.Name = "AjustestipoToolStripMenuItem"
        Me.AjustestipoToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.AjustestipoToolStripMenuItem.Text = "Ajustes_tipo"
        '
        'CajaToolStripMenuItem
        '
        Me.CajaToolStripMenuItem.Name = "CajaToolStripMenuItem"
        Me.CajaToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.CajaToolStripMenuItem.Text = "Caja"
        '
        'CotizacionToolStripMenuItem
        '
        Me.CotizacionToolStripMenuItem.Name = "CotizacionToolStripMenuItem"
        Me.CotizacionToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.CotizacionToolStripMenuItem.Text = "Cotizacion"
        '
        'ArqueoDeCajaToolStripMenuItem
        '
        Me.ArqueoDeCajaToolStripMenuItem.Name = "ArqueoDeCajaToolStripMenuItem"
        Me.ArqueoDeCajaToolStripMenuItem.Size = New System.Drawing.Size(153, 22)
        Me.ArqueoDeCajaToolStripMenuItem.Text = "Arqueo de caja"
        '
        'PersonasToolStripMenuItem
        '
        Me.PersonasToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClienteToolStripMenuItem, Me.ProveedorToolStripMenuItem, Me.EmpleadoToolStripMenuItem, Me.UsuarioToolStripMenuItem})
        Me.PersonasToolStripMenuItem.Name = "PersonasToolStripMenuItem"
        Me.PersonasToolStripMenuItem.Size = New System.Drawing.Size(66, 20)
        Me.PersonasToolStripMenuItem.Text = "Personas"
        '
        'ClienteToolStripMenuItem
        '
        Me.ClienteToolStripMenuItem.Name = "ClienteToolStripMenuItem"
        Me.ClienteToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ClienteToolStripMenuItem.Text = "Cliente"
        '
        'ProveedorToolStripMenuItem
        '
        Me.ProveedorToolStripMenuItem.Name = "ProveedorToolStripMenuItem"
        Me.ProveedorToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.ProveedorToolStripMenuItem.Text = "Proveedor"
        '
        'EmpleadoToolStripMenuItem
        '
        Me.EmpleadoToolStripMenuItem.Name = "EmpleadoToolStripMenuItem"
        Me.EmpleadoToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.EmpleadoToolStripMenuItem.Text = "Empleado"
        '
        'UsuarioToolStripMenuItem
        '
        Me.UsuarioToolStripMenuItem.Name = "UsuarioToolStripMenuItem"
        Me.UsuarioToolStripMenuItem.Size = New System.Drawing.Size(128, 22)
        Me.UsuarioToolStripMenuItem.Text = "Usuario"
        '
        'ProductosToolStripMenuItem
        '
        Me.ProductosToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MarcaToolStripMenuItem, Me.ProductoToolStripMenuItem})
        Me.ProductosToolStripMenuItem.Name = "ProductosToolStripMenuItem"
        Me.ProductosToolStripMenuItem.Size = New System.Drawing.Size(73, 20)
        Me.ProductosToolStripMenuItem.Text = "Productos"
        '
        'MarcaToolStripMenuItem
        '
        Me.MarcaToolStripMenuItem.Name = "MarcaToolStripMenuItem"
        Me.MarcaToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.MarcaToolStripMenuItem.Text = "Marca"
        '
        'ProductoToolStripMenuItem
        '
        Me.ProductoToolStripMenuItem.Name = "ProductoToolStripMenuItem"
        Me.ProductoToolStripMenuItem.Size = New System.Drawing.Size(123, 22)
        Me.ProductoToolStripMenuItem.Text = "Producto"
        '
        'UbicacionesToolStripMenuItem
        '
        Me.UbicacionesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DepartamentoToolStripMenuItem, Me.CiudadToolStripMenuItem})
        Me.UbicacionesToolStripMenuItem.Name = "UbicacionesToolStripMenuItem"
        Me.UbicacionesToolStripMenuItem.Size = New System.Drawing.Size(83, 20)
        Me.UbicacionesToolStripMenuItem.Text = "Ubicaciones"
        '
        'DepartamentoToolStripMenuItem
        '
        Me.DepartamentoToolStripMenuItem.Name = "DepartamentoToolStripMenuItem"
        Me.DepartamentoToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.DepartamentoToolStripMenuItem.Text = "Departamento"
        '
        'CiudadToolStripMenuItem
        '
        Me.CiudadToolStripMenuItem.Name = "CiudadToolStripMenuItem"
        Me.CiudadToolStripMenuItem.Size = New System.Drawing.Size(150, 22)
        Me.CiudadToolStripMenuItem.Text = "Ciudad"
        '
        'ComprasToolStripMenuItem
        '
        Me.ComprasToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PedidoAProveedorToolStripMenuItem, Me.OrdenDeCompraToolStripMenuItem, Me.CompraToolStripMenuItem, Me.CuentaACobrarToolStripMenuItem, Me.CuentaAPagarToolStripMenuItem, Me.OrdenDePagoToolStripMenuItem, Me.PagoToolStripMenuItem, Me.DevoluciónAProveedorToolStripMenuItem})
        Me.ComprasToolStripMenuItem.Name = "ComprasToolStripMenuItem"
        Me.ComprasToolStripMenuItem.Size = New System.Drawing.Size(67, 20)
        Me.ComprasToolStripMenuItem.Text = "Compras"
        '
        'PedidoAProveedorToolStripMenuItem
        '
        Me.PedidoAProveedorToolStripMenuItem.Name = "PedidoAProveedorToolStripMenuItem"
        Me.PedidoAProveedorToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.PedidoAProveedorToolStripMenuItem.Text = "Pedido a Proveedor"
        '
        'OrdenDeCompraToolStripMenuItem
        '
        Me.OrdenDeCompraToolStripMenuItem.Name = "OrdenDeCompraToolStripMenuItem"
        Me.OrdenDeCompraToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.OrdenDeCompraToolStripMenuItem.Text = "Orden de Compra"
        '
        'CompraToolStripMenuItem
        '
        Me.CompraToolStripMenuItem.Name = "CompraToolStripMenuItem"
        Me.CompraToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.CompraToolStripMenuItem.Text = "Compra"
        '
        'CuentaACobrarToolStripMenuItem
        '
        Me.CuentaACobrarToolStripMenuItem.Name = "CuentaACobrarToolStripMenuItem"
        Me.CuentaACobrarToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.CuentaACobrarToolStripMenuItem.Text = "Cuenta a cobrar"
        '
        'CuentaAPagarToolStripMenuItem
        '
        Me.CuentaAPagarToolStripMenuItem.Name = "CuentaAPagarToolStripMenuItem"
        Me.CuentaAPagarToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.CuentaAPagarToolStripMenuItem.Text = "Cuenta a Pagar"
        '
        'OrdenDePagoToolStripMenuItem
        '
        Me.OrdenDePagoToolStripMenuItem.Name = "OrdenDePagoToolStripMenuItem"
        Me.OrdenDePagoToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.OrdenDePagoToolStripMenuItem.Text = "Orden de Pago"
        '
        'PagoToolStripMenuItem
        '
        Me.PagoToolStripMenuItem.Name = "PagoToolStripMenuItem"
        Me.PagoToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.PagoToolStripMenuItem.Text = "Pago"
        '
        'DevoluciónAProveedorToolStripMenuItem
        '
        Me.DevoluciónAProveedorToolStripMenuItem.Name = "DevoluciónAProveedorToolStripMenuItem"
        Me.DevoluciónAProveedorToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.DevoluciónAProveedorToolStripMenuItem.Text = "Devolución a Proveedor"
        '
        'VentasToolStripMenuItem
        '
        Me.VentasToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PresupuestoToolStripMenuItem, Me.PedidoClienteToolStripMenuItem, Me.VentaToolStripMenuItem, Me.CobroToolStripMenuItem})
        Me.VentasToolStripMenuItem.Name = "VentasToolStripMenuItem"
        Me.VentasToolStripMenuItem.Size = New System.Drawing.Size(53, 20)
        Me.VentasToolStripMenuItem.Text = "Ventas"
        '
        'PresupuestoToolStripMenuItem
        '
        Me.PresupuestoToolStripMenuItem.Name = "PresupuestoToolStripMenuItem"
        Me.PresupuestoToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.PresupuestoToolStripMenuItem.Text = "Presupuesto"
        '
        'PedidoClienteToolStripMenuItem
        '
        Me.PedidoClienteToolStripMenuItem.Name = "PedidoClienteToolStripMenuItem"
        Me.PedidoClienteToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.PedidoClienteToolStripMenuItem.Text = "Pedido Cliente"
        '
        'VentaToolStripMenuItem
        '
        Me.VentaToolStripMenuItem.Name = "VentaToolStripMenuItem"
        Me.VentaToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.VentaToolStripMenuItem.Text = "Venta"
        '
        'CobroToolStripMenuItem
        '
        Me.CobroToolStripMenuItem.Name = "CobroToolStripMenuItem"
        Me.CobroToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.CobroToolStripMenuItem.Text = "Cobro"
        '
        'SistemaToolStripMenuItem
        '
        Me.SistemaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CerrarSesionToolStripMenuItem, Me.SalirToolStripMenuItem, Me.AuditoriaToolStripMenuItem})
        Me.SistemaToolStripMenuItem.Name = "SistemaToolStripMenuItem"
        Me.SistemaToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
        Me.SistemaToolStripMenuItem.Text = "Sistema"
        '
        'CerrarSesionToolStripMenuItem
        '
        Me.CerrarSesionToolStripMenuItem.Name = "CerrarSesionToolStripMenuItem"
        Me.CerrarSesionToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.CerrarSesionToolStripMenuItem.Text = "Cerrar sesion"
        '
        'SalirToolStripMenuItem
        '
        Me.SalirToolStripMenuItem.Name = "SalirToolStripMenuItem"
        Me.SalirToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.SalirToolStripMenuItem.Text = "Salir"
        '
        'AuditoriaToolStripMenuItem
        '
        Me.AuditoriaToolStripMenuItem.Name = "AuditoriaToolStripMenuItem"
        Me.AuditoriaToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.AuditoriaToolStripMenuItem.Text = "Auditoria"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(12, 1019)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(39, 13)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "Label1"
        '
        'FrmMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1370, 749)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FrmMenu"
        Me.Text = "FrmMenu"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents CatalogosToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MonedaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CobrotipoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AjustestipoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CajaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UbicacionesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DepartamentoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CiudadToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PersonasToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ClienteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ProveedorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EmpleadoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ComprasToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PedidoAProveedorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OrdenDeCompraToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CompraToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CuentaAPagarToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OrdenDePagoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PagoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DevoluciónAProveedorToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UsuarioToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ProductosToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MarcaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ProductoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VentasToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PresupuestoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PedidoClienteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VentaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CobroToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SistemaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CerrarSesionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SalirToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents lblStatus As Label
    Friend WithEvents AuditoriaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CotizacionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ArqueoDeCajaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CuentaACobrarToolStripMenuItem As ToolStripMenuItem
End Class
