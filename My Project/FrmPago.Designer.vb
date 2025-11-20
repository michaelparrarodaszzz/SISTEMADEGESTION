<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPago
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.BtnPagCerrar = New System.Windows.Forms.Button()
        Me.dgvPago = New System.Windows.Forms.DataGridView()
        Me.colId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colProv = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colOrdPag = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUsuario = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFecha = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colMonto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colObs = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.proid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pagid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUsuaId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.txtMonto = New System.Windows.Forms.TextBox()
        Me.BtnPagCancelar = New System.Windows.Forms.Button()
        Me.BtnPagEliminar = New System.Windows.Forms.Button()
        Me.BtnPagGuardar = New System.Windows.Forms.Button()
        Me.ep = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.cboProv = New System.Windows.Forms.ComboBox()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.lblIva = New System.Windows.Forms.Label()
        Me.lblruc = New System.Windows.Forms.Label()
        Me.txtPagId = New System.Windows.Forms.TextBox()
        Me.BtnPagEditar = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.BtnPagNuevo = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtObs = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cboUsuario = New System.Windows.Forms.ComboBox()
        Me.lblestado = New System.Windows.Forms.Label()
        Me.cboOrdPag = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        CType(Me.dgvPago, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ep, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnPagCerrar
        '
        Me.BtnPagCerrar.FlatAppearance.BorderSize = 0
        Me.BtnPagCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPagCerrar.Location = New System.Drawing.Point(486, 355)
        Me.BtnPagCerrar.Name = "BtnPagCerrar"
        Me.BtnPagCerrar.Size = New System.Drawing.Size(90, 32)
        Me.BtnPagCerrar.TabIndex = 112
        Me.BtnPagCerrar.Text = "Cerrar"
        Me.BtnPagCerrar.UseVisualStyleBackColor = True
        '
        'dgvPago
        '
        Me.dgvPago.AllowUserToAddRows = False
        Me.dgvPago.AllowUserToDeleteRows = False
        Me.dgvPago.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPago.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colId, Me.colProv, Me.colOrdPag, Me.colUsuario, Me.colFecha, Me.colMonto, Me.colObs, Me.proid, Me.pagid, Me.colUsuaId})
        Me.dgvPago.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvPago.Location = New System.Drawing.Point(0, 409)
        Me.dgvPago.Name = "dgvPago"
        Me.dgvPago.ReadOnly = True
        Me.dgvPago.Size = New System.Drawing.Size(1370, 340)
        Me.dgvPago.TabIndex = 104
        '
        'colId
        '
        Me.colId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colId.DataPropertyName = "pag_id"
        Me.colId.HeaderText = "ID"
        Me.colId.Name = "colId"
        Me.colId.ReadOnly = True
        '
        'colProv
        '
        Me.colProv.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colProv.DataPropertyName = "prov_nombre"
        Me.colProv.HeaderText = "PROVEEDOR"
        Me.colProv.Name = "colProv"
        Me.colProv.ReadOnly = True
        '
        'colOrdPag
        '
        Me.colOrdPag.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colOrdPag.DataPropertyName = "ordpag_id"
        Me.colOrdPag.HeaderText = "ORDEN DE PAGO"
        Me.colOrdPag.Name = "colOrdPag"
        Me.colOrdPag.ReadOnly = True
        '
        'colUsuario
        '
        Me.colUsuario.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colUsuario.DataPropertyName = "usua_nombre"
        Me.colUsuario.HeaderText = "USUARIO"
        Me.colUsuario.Name = "colUsuario"
        Me.colUsuario.ReadOnly = True
        '
        'colFecha
        '
        Me.colFecha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colFecha.HeaderText = "FECHA"
        Me.colFecha.Name = "colFecha"
        Me.colFecha.ReadOnly = True
        '
        'colMonto
        '
        Me.colMonto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colMonto.DataPropertyName = "pag_nombre"
        Me.colMonto.HeaderText = "MONTO"
        Me.colMonto.Name = "colMonto"
        Me.colMonto.ReadOnly = True
        '
        'colObs
        '
        Me.colObs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colObs.DataPropertyName = "pag_obs"
        Me.colObs.HeaderText = "OBS"
        Me.colObs.Name = "colObs"
        Me.colObs.ReadOnly = True
        '
        'proid
        '
        Me.proid.DataPropertyName = "prov_id"
        Me.proid.HeaderText = "si"
        Me.proid.Name = "proid"
        Me.proid.ReadOnly = True
        Me.proid.Visible = False
        '
        'pagid
        '
        Me.pagid.DataPropertyName = "orden_de_pago_ordpag_id"
        Me.pagid.HeaderText = "si"
        Me.pagid.Name = "pagid"
        Me.pagid.ReadOnly = True
        Me.pagid.Visible = False
        '
        'colUsuaId
        '
        Me.colUsuaId.DataPropertyName = "usua_id"
        Me.colUsuaId.HeaderText = "si"
        Me.colUsuaId.Name = "colUsuaId"
        Me.colUsuaId.ReadOnly = True
        Me.colUsuaId.Visible = False
        '
        'txtMonto
        '
        Me.txtMonto.Location = New System.Drawing.Point(121, 153)
        Me.txtMonto.Name = "txtMonto"
        Me.txtMonto.Size = New System.Drawing.Size(188, 20)
        Me.txtMonto.TabIndex = 37
        '
        'BtnPagCancelar
        '
        Me.BtnPagCancelar.FlatAppearance.BorderSize = 0
        Me.BtnPagCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPagCancelar.Location = New System.Drawing.Point(392, 355)
        Me.BtnPagCancelar.Name = "BtnPagCancelar"
        Me.BtnPagCancelar.Size = New System.Drawing.Size(90, 32)
        Me.BtnPagCancelar.TabIndex = 111
        Me.BtnPagCancelar.Text = "Cancelar"
        Me.BtnPagCancelar.UseVisualStyleBackColor = True
        '
        'BtnPagEliminar
        '
        Me.BtnPagEliminar.FlatAppearance.BorderSize = 0
        Me.BtnPagEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPagEliminar.Location = New System.Drawing.Point(298, 355)
        Me.BtnPagEliminar.Name = "BtnPagEliminar"
        Me.BtnPagEliminar.Size = New System.Drawing.Size(90, 32)
        Me.BtnPagEliminar.TabIndex = 110
        Me.BtnPagEliminar.Text = "Eliminar"
        Me.BtnPagEliminar.UseVisualStyleBackColor = True
        '
        'BtnPagGuardar
        '
        Me.BtnPagGuardar.FlatAppearance.BorderSize = 0
        Me.BtnPagGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPagGuardar.Location = New System.Drawing.Point(110, 355)
        Me.BtnPagGuardar.Name = "BtnPagGuardar"
        Me.BtnPagGuardar.Size = New System.Drawing.Size(90, 32)
        Me.BtnPagGuardar.TabIndex = 108
        Me.BtnPagGuardar.Text = "Guardar"
        Me.BtnPagGuardar.UseVisualStyleBackColor = True
        '
        'ep
        '
        Me.ep.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ep.ContainerControl = Me
        '
        'cboProv
        '
        Me.cboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProv.FormattingEnabled = True
        Me.cboProv.Location = New System.Drawing.Point(121, 49)
        Me.cboProv.Name = "cboProv"
        Me.cboProv.Size = New System.Drawing.Size(188, 21)
        Me.cboProv.TabIndex = 33
        '
        'dtpFecha
        '
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Location = New System.Drawing.Point(121, 127)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(188, 20)
        Me.dtpFecha.TabIndex = 31
        '
        'lblIva
        '
        Me.lblIva.AutoSize = True
        Me.lblIva.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblIva.Location = New System.Drawing.Point(73, 132)
        Me.lblIva.Name = "lblIva"
        Me.lblIva.Size = New System.Drawing.Size(42, 13)
        Me.lblIva.TabIndex = 30
        Me.lblIva.Text = "FECHA"
        '
        'lblruc
        '
        Me.lblruc.AutoSize = True
        Me.lblruc.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblruc.Location = New System.Drawing.Point(18, 76)
        Me.lblruc.Name = "lblruc"
        Me.lblruc.Size = New System.Drawing.Size(97, 13)
        Me.lblruc.TabIndex = 10
        Me.lblruc.Text = "ORDEN DE PAGO"
        '
        'txtPagId
        '
        Me.txtPagId.Location = New System.Drawing.Point(121, 23)
        Me.txtPagId.Name = "txtPagId"
        Me.txtPagId.Size = New System.Drawing.Size(100, 20)
        Me.txtPagId.TabIndex = 3
        '
        'BtnPagEditar
        '
        Me.BtnPagEditar.FlatAppearance.BorderSize = 0
        Me.BtnPagEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPagEditar.Location = New System.Drawing.Point(204, 355)
        Me.BtnPagEditar.Name = "BtnPagEditar"
        Me.BtnPagEditar.Size = New System.Drawing.Size(90, 32)
        Me.BtnPagEditar.TabIndex = 109
        Me.BtnPagEditar.Text = "Editar"
        Me.BtnPagEditar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(31, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 29)
        Me.Label1.TabIndex = 103
        Me.Label1.Text = "PAGO"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label3.Location = New System.Drawing.Point(41, 52)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(75, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "PROVEEDOR"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Panel3.Controls.Add(Me.lblStatus)
        Me.Panel3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Panel3.Location = New System.Drawing.Point(-4, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1913, 42)
        Me.Panel3.TabIndex = 106
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.lblStatus.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblStatus.Location = New System.Drawing.Point(12, 14)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(68, 13)
        Me.lblStatus.TabIndex = 18
        Me.lblStatus.Text = "CARGANDO"
        '
        'BtnPagNuevo
        '
        Me.BtnPagNuevo.FlatAppearance.BorderSize = 0
        Me.BtnPagNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnPagNuevo.Location = New System.Drawing.Point(16, 355)
        Me.BtnPagNuevo.Name = "BtnPagNuevo"
        Me.BtnPagNuevo.Size = New System.Drawing.Size(90, 32)
        Me.BtnPagNuevo.TabIndex = 107
        Me.BtnPagNuevo.Text = "Nuevo"
        Me.BtnPagNuevo.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.txtObs)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.txtMonto)
        Me.Panel1.Controls.Add(Me.cboProv)
        Me.Panel1.Controls.Add(Me.dtpFecha)
        Me.Panel1.Controls.Add(Me.lblIva)
        Me.Panel1.Controls.Add(Me.cboUsuario)
        Me.Panel1.Controls.Add(Me.lblestado)
        Me.Panel1.Controls.Add(Me.cboOrdPag)
        Me.Panel1.Controls.Add(Me.lblruc)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.txtPagId)
        Me.Panel1.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel1.Location = New System.Drawing.Point(9, 82)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1328, 258)
        Me.Panel1.TabIndex = 105
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label4.Location = New System.Drawing.Point(73, 186)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(29, 13)
        Me.Label4.TabIndex = 40
        Me.Label4.Text = "OBS"
        '
        'txtObs
        '
        Me.txtObs.Location = New System.Drawing.Point(121, 183)
        Me.txtObs.Name = "txtObs"
        Me.txtObs.Size = New System.Drawing.Size(188, 20)
        Me.txtObs.TabIndex = 39
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label5.Location = New System.Drawing.Point(73, 156)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(47, 13)
        Me.Label5.TabIndex = 38
        Me.Label5.Text = "MONTO"
        '
        'cboUsuario
        '
        Me.cboUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUsuario.FormattingEnabled = True
        Me.cboUsuario.Items.AddRange(New Object() {"PENDIENTE", "APROVADO", "ANULADO"})
        Me.cboUsuario.Location = New System.Drawing.Point(121, 100)
        Me.cboUsuario.Name = "cboUsuario"
        Me.cboUsuario.Size = New System.Drawing.Size(188, 21)
        Me.cboUsuario.TabIndex = 28
        '
        'lblestado
        '
        Me.lblestado.AutoSize = True
        Me.lblestado.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblestado.Location = New System.Drawing.Point(64, 103)
        Me.lblestado.Name = "lblestado"
        Me.lblestado.Size = New System.Drawing.Size(56, 13)
        Me.lblestado.TabIndex = 27
        Me.lblestado.Text = "USUARIO"
        '
        'cboOrdPag
        '
        Me.cboOrdPag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOrdPag.FormattingEnabled = True
        Me.cboOrdPag.Location = New System.Drawing.Point(121, 73)
        Me.cboOrdPag.Name = "cboOrdPag"
        Me.cboOrdPag.Size = New System.Drawing.Size(188, 21)
        Me.cboOrdPag.TabIndex = 26
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label2.Location = New System.Drawing.Point(97, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "ID"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Panel2.Location = New System.Drawing.Point(9, 346)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(767, 57)
        Me.Panel2.TabIndex = 113
        '
        'FrmPago
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1370, 749)
        Me.Controls.Add(Me.BtnPagCerrar)
        Me.Controls.Add(Me.dgvPago)
        Me.Controls.Add(Me.BtnPagCancelar)
        Me.Controls.Add(Me.BtnPagEliminar)
        Me.Controls.Add(Me.BtnPagGuardar)
        Me.Controls.Add(Me.BtnPagEditar)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.BtnPagNuevo)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Name = "FrmPago"
        Me.Text = "FrmPago"
        CType(Me.dgvPago, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ep, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents BtnPagCerrar As Button
    Friend WithEvents dgvPago As DataGridView
    Friend WithEvents txtMonto As TextBox
    Private WithEvents BtnPagCancelar As Button
    Friend WithEvents BtnPagEliminar As Button
    Friend WithEvents BtnPagGuardar As Button
    Friend WithEvents ep As ErrorProvider
    Friend WithEvents BtnPagEditar As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents lblStatus As Label
    Friend WithEvents BtnPagNuevo As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents cboProv As ComboBox
    Friend WithEvents dtpFecha As DateTimePicker
    Friend WithEvents lblIva As Label
    Friend WithEvents cboUsuario As ComboBox
    Friend WithEvents lblestado As Label
    Friend WithEvents cboOrdPag As ComboBox
    Friend WithEvents lblruc As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtPagId As TextBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents txtObs As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents colId As DataGridViewTextBoxColumn
    Friend WithEvents colProv As DataGridViewTextBoxColumn
    Friend WithEvents colOrdPag As DataGridViewTextBoxColumn
    Friend WithEvents colUsuario As DataGridViewTextBoxColumn
    Friend WithEvents colFecha As DataGridViewTextBoxColumn
    Friend WithEvents colMonto As DataGridViewTextBoxColumn
    Friend WithEvents colObs As DataGridViewTextBoxColumn
    Friend WithEvents proid As DataGridViewTextBoxColumn
    Friend WithEvents pagid As DataGridViewTextBoxColumn
    Friend WithEvents colUsuaId As DataGridViewTextBoxColumn
End Class
