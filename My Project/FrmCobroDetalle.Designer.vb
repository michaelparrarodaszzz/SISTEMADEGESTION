<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmCobroDetalle
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then components.Dispose()
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PanelTop = New System.Windows.Forms.Panel()
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.PanelInfo = New System.Windows.Forms.Panel()
        Me.txtDisponible = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtMontoCobro = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtFechaCobro = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtCliente = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cboCobro = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PanelButtons = New System.Windows.Forms.Panel()
        Me.BtnCerrar = New System.Windows.Forms.Button()
        Me.BtnCancelar = New System.Windows.Forms.Button()
        Me.BtnGuardar = New System.Windows.Forms.Button()
        Me.BtnNuevo = New System.Windows.Forms.Button()
        Me.dgvPend = New System.Windows.Forms.DataGridView()
        Me.colVenId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDoc = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFecha = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTotal = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAplicado = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSaldo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colAplicar = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.ep = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.PanelTop.SuspendLayout()
        Me.PanelInfo.SuspendLayout()
        Me.PanelButtons.SuspendLayout()
        CType(Me.dgvPend, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ep, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelTop
        '
        Me.PanelTop.Controls.Add(Me.lblTitulo)
        Me.PanelTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelTop.Location = New System.Drawing.Point(0, 0)
        Me.PanelTop.Name = "PanelTop"
        Me.PanelTop.Size = New System.Drawing.Size(1100, 44)
        Me.PanelTop.TabIndex = 0
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Font = New System.Drawing.Font("Segoe UI", 16.0!, System.Drawing.FontStyle.Bold)
        Me.lblTitulo.Location = New System.Drawing.Point(12, 9)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(209, 30)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "APLICAR COBRO"
        '
        'PanelInfo
        '
        Me.PanelInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelInfo.Controls.Add(Me.txtDisponible)
        Me.PanelInfo.Controls.Add(Me.Label6)
        Me.PanelInfo.Controls.Add(Me.txtMontoCobro)
        Me.PanelInfo.Controls.Add(Me.Label5)
        Me.PanelInfo.Controls.Add(Me.txtFechaCobro)
        Me.PanelInfo.Controls.Add(Me.Label4)
        Me.PanelInfo.Controls.Add(Me.txtCliente)
        Me.PanelInfo.Controls.Add(Me.Label3)
        Me.PanelInfo.Controls.Add(Me.cboCobro)
        Me.PanelInfo.Controls.Add(Me.Label2)
        Me.PanelInfo.Location = New System.Drawing.Point(18, 56)
        Me.PanelInfo.Name = "PanelInfo"
        Me.PanelInfo.Size = New System.Drawing.Size(1064, 96)
        Me.PanelInfo.TabIndex = 1
        '
        'txtDisponible
        '
        Me.txtDisponible.Location = New System.Drawing.Point(838, 54)
        Me.txtDisponible.Name = "txtDisponible"
        Me.txtDisponible.ReadOnly = True
        Me.txtDisponible.Size = New System.Drawing.Size(200, 23)
        Me.txtDisponible.TabIndex = 9
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(763, 58)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(69, 15)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Disponible"
        '
        'txtMontoCobro
        '
        Me.txtMontoCobro.Location = New System.Drawing.Point(538, 54)
        Me.txtMontoCobro.Name = "txtMontoCobro"
        Me.txtMontoCobro.ReadOnly = True
        Me.txtMontoCobro.Size = New System.Drawing.Size(200, 23)
        Me.txtMontoCobro.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(455, 58)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(77, 15)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Monto cobro"
        '
        'txtFechaCobro
        '
        Me.txtFechaCobro.Location = New System.Drawing.Point(96, 54)
        Me.txtFechaCobro.Name = "txtFechaCobro"
        Me.txtFechaCobro.ReadOnly = True
        Me.txtFechaCobro.Size = New System.Drawing.Size(140, 23)
        Me.txtFechaCobro.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 58)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(75, 15)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Fecha cobro"
        '
        'txtCliente
        '
        Me.txtCliente.Location = New System.Drawing.Point(538, 15)
        Me.txtCliente.Name = "txtCliente"
        Me.txtCliente.ReadOnly = True
        Me.txtCliente.Size = New System.Drawing.Size(500, 23)
        Me.txtCliente.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(489, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Cliente"
        '
        'cboCobro
        '
        Me.cboCobro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCobro.FormattingEnabled = True
        Me.cboCobro.Location = New System.Drawing.Point(96, 15)
        Me.cboCobro.Name = "cboCobro"
        Me.cboCobro.Size = New System.Drawing.Size(372, 23)
        Me.cboCobro.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Cobro"
        '
        'PanelButtons
        '
        Me.PanelButtons.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PanelButtons.Controls.Add(Me.BtnCerrar)
        Me.PanelButtons.Controls.Add(Me.BtnCancelar)
        Me.PanelButtons.Controls.Add(Me.BtnGuardar)
        Me.PanelButtons.Controls.Add(Me.BtnNuevo)
        Me.PanelButtons.Location = New System.Drawing.Point(18, 558)
        Me.PanelButtons.Name = "PanelButtons"
        Me.PanelButtons.Size = New System.Drawing.Size(1064, 46)
        Me.PanelButtons.TabIndex = 3
        '
        'BtnCerrar
        '
        Me.BtnCerrar.Location = New System.Drawing.Point(294, 8)
        Me.BtnCerrar.Name = "BtnCerrar"
        Me.BtnCerrar.Size = New System.Drawing.Size(85, 30)
        Me.BtnCerrar.TabIndex = 3
        Me.BtnCerrar.Text = "Cerrar"
        Me.BtnCerrar.UseVisualStyleBackColor = True
        '
        'BtnCancelar
        '
        Me.BtnCancelar.Location = New System.Drawing.Point(203, 8)
        Me.BtnCancelar.Name = "BtnCancelar"
        Me.BtnCancelar.Size = New System.Drawing.Size(85, 30)
        Me.BtnCancelar.TabIndex = 2
        Me.BtnCancelar.Text = "Cancelar"
        Me.BtnCancelar.UseVisualStyleBackColor = True
        '
        'BtnGuardar
        '
        Me.BtnGuardar.Location = New System.Drawing.Point(112, 8)
        Me.BtnGuardar.Name = "BtnGuardar"
        Me.BtnGuardar.Size = New System.Drawing.Size(85, 30)
        Me.BtnGuardar.TabIndex = 1
        Me.BtnGuardar.Text = "Guardar"
        Me.BtnGuardar.UseVisualStyleBackColor = True
        '
        'BtnNuevo
        '
        Me.BtnNuevo.Location = New System.Drawing.Point(21, 8)
        Me.BtnNuevo.Name = "BtnNuevo"
        Me.BtnNuevo.Size = New System.Drawing.Size(85, 30)
        Me.BtnNuevo.TabIndex = 0
        Me.BtnNuevo.Text = "Nuevo"
        Me.BtnNuevo.UseVisualStyleBackColor = True
        '
        'dgvPend
        '
        Me.dgvPend.AllowUserToAddRows = False
        Me.dgvPend.AllowUserToDeleteRows = False
        Me.dgvPend.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvPend.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPend.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colVenId, Me.colDoc, Me.colFecha, Me.colTotal, Me.colAplicado, Me.colSaldo, Me.colAplicar})
        Me.dgvPend.Location = New System.Drawing.Point(18, 170)
        Me.dgvPend.Name = "dgvPend"
        Me.dgvPend.Size = New System.Drawing.Size(1064, 372)
        Me.dgvPend.TabIndex = 2
        '
        'colVenId
        '
        Me.colVenId.DataPropertyName = "ven_id"
        Me.colVenId.HeaderText = "ven_id"
        Me.colVenId.Name = "colVenId"
        Me.colVenId.Visible = False
        '
        'colDoc
        '
        Me.colDoc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colDoc.DataPropertyName = "doc"
        Me.colDoc.HeaderText = "Documento"
        Me.colDoc.Name = "colDoc"
        '
        'colFecha
        '
        Me.colFecha.DataPropertyName = "fecha"
        Me.colFecha.HeaderText = "Fecha"
        Me.colFecha.Name = "colFecha"
        '
        'colTotal
        '
        Me.colTotal.DataPropertyName = "total"
        Me.colTotal.HeaderText = "Total"
        Me.colTotal.Name = "colTotal"
        '
        'colAplicado
        '
        Me.colAplicado.DataPropertyName = "aplicado"
        Me.colAplicado.HeaderText = "Aplicado"
        Me.colAplicado.Name = "colAplicado"
        '
        'colSaldo
        '
        Me.colSaldo.DataPropertyName = "saldo"
        Me.colSaldo.HeaderText = "Saldo"
        Me.colSaldo.Name = "colSaldo"
        '
        'colAplicar
        '
        Me.colAplicar.HeaderText = "A aplicar"
        Me.colAplicar.Name = "colAplicar"
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.Location = New System.Drawing.Point(18, 611)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(1064, 20)
        Me.lblStatus.TabIndex = 4
        Me.lblStatus.Text = "Listo."
        '
        'ep
        '
        Me.ep.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ep.ContainerControl = Me
        '
        'FrmCobroDetalle
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1100, 640)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.dgvPend)
        Me.Controls.Add(Me.PanelButtons)
        Me.Controls.Add(Me.PanelInfo)
        Me.Controls.Add(Me.PanelTop)
        Me.Name = "FrmCobroDetalle"
        Me.Text = "Aplicación de Cobro"
        Me.PanelTop.ResumeLayout(False)
        Me.PanelTop.PerformLayout()
        Me.PanelInfo.ResumeLayout(False)
        Me.PanelInfo.PerformLayout()
        Me.PanelButtons.ResumeLayout(False)
        CType(Me.dgvPend, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ep, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PanelTop As Panel
    Friend WithEvents lblTitulo As Label
    Friend WithEvents PanelInfo As Panel
    Friend WithEvents cboCobro As ComboBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtCliente As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtFechaCobro As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtMontoCobro As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtDisponible As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents PanelButtons As Panel
    Friend WithEvents BtnCerrar As Button
    Friend WithEvents BtnCancelar As Button
    Friend WithEvents BtnGuardar As Button
    Friend WithEvents BtnNuevo As Button
    Friend WithEvents dgvPend As DataGridView
    Friend WithEvents lblStatus As Label
    Friend WithEvents ep As ErrorProvider
    Friend WithEvents colVenId As DataGridViewTextBoxColumn
    Friend WithEvents colDoc As DataGridViewTextBoxColumn
    Friend WithEvents colFecha As DataGridViewTextBoxColumn
    Friend WithEvents colTotal As DataGridViewTextBoxColumn
    Friend WithEvents colAplicado As DataGridViewTextBoxColumn
    Friend WithEvents colSaldo As DataGridViewTextBoxColumn
    Friend WithEvents colAplicar As DataGridViewTextBoxColumn
End Class
