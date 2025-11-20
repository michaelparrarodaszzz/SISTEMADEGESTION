<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDevolucionProveedor
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
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.BtnDevCancelar = New System.Windows.Forms.Button()
        Me.btnDevEliminar = New System.Windows.Forms.Button()
        Me.BtnDevCerrar = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtObs = New System.Windows.Forms.TextBox()
        Me.btnDevNuevo = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cboEstado = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.dtpHora = New System.Windows.Forms.DateTimePicker()
        Me.lblDescripcion = New System.Windows.Forms.Label()
        Me.cboProv = New System.Windows.Forms.ComboBox()
        Me.dtpFecha = New System.Windows.Forms.DateTimePicker()
        Me.lblIva = New System.Windows.Forms.Label()
        Me.cboUsuario = New System.Windows.Forms.ComboBox()
        Me.lblestado = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDevId = New System.Windows.Forms.TextBox()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.btnDevEditar = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.ep = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.btnDevGuardar = New System.Windows.Forms.Button()
        Me.dgvDevProv = New System.Windows.Forms.DataGridView()
        Me.colId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colProv = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUsuario = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFecha = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colHora = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEstado = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colObs = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.proid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pagid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUsuaId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.ep, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvDevProv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Panel2.Controls.Add(Me.BtnDevCancelar)
        Me.Panel2.Controls.Add(Me.btnDevEliminar)
        Me.Panel2.Controls.Add(Me.BtnDevCerrar)
        Me.Panel2.Location = New System.Drawing.Point(9, 346)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(767, 57)
        Me.Panel2.TabIndex = 124
        '
        'BtnDevCancelar
        '
        Me.BtnDevCancelar.BackColor = System.Drawing.SystemColors.Control
        Me.BtnDevCancelar.FlatAppearance.BorderSize = 0
        Me.BtnDevCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnDevCancelar.Location = New System.Drawing.Point(387, 9)
        Me.BtnDevCancelar.Name = "BtnDevCancelar"
        Me.BtnDevCancelar.Size = New System.Drawing.Size(90, 32)
        Me.BtnDevCancelar.TabIndex = 126
        Me.BtnDevCancelar.Text = "Cancelar"
        Me.BtnDevCancelar.UseVisualStyleBackColor = False
        '
        'btnDevEliminar
        '
        Me.btnDevEliminar.BackColor = System.Drawing.SystemColors.Control
        Me.btnDevEliminar.FlatAppearance.BorderSize = 0
        Me.btnDevEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDevEliminar.Location = New System.Drawing.Point(291, 9)
        Me.btnDevEliminar.Name = "btnDevEliminar"
        Me.btnDevEliminar.Size = New System.Drawing.Size(90, 32)
        Me.btnDevEliminar.TabIndex = 125
        Me.btnDevEliminar.Text = "Eliminar"
        Me.btnDevEliminar.UseVisualStyleBackColor = False
        '
        'BtnDevCerrar
        '
        Me.BtnDevCerrar.BackColor = System.Drawing.SystemColors.Control
        Me.BtnDevCerrar.FlatAppearance.BorderSize = 0
        Me.BtnDevCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnDevCerrar.Location = New System.Drawing.Point(494, 9)
        Me.BtnDevCerrar.Name = "BtnDevCerrar"
        Me.BtnDevCerrar.Size = New System.Drawing.Size(90, 32)
        Me.BtnDevCerrar.TabIndex = 123
        Me.BtnDevCerrar.Text = "Cerrar"
        Me.BtnDevCerrar.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label4.Location = New System.Drawing.Point(87, 187)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(29, 13)
        Me.Label4.TabIndex = 40
        Me.Label4.Text = "OBS"
        '
        'txtObs
        '
        Me.txtObs.Location = New System.Drawing.Point(122, 182)
        Me.txtObs.Name = "txtObs"
        Me.txtObs.Size = New System.Drawing.Size(188, 20)
        Me.txtObs.TabIndex = 39
        '
        'btnDevNuevo
        '
        Me.btnDevNuevo.FlatAppearance.BorderSize = 0
        Me.btnDevNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDevNuevo.Location = New System.Drawing.Point(16, 355)
        Me.btnDevNuevo.Name = "btnDevNuevo"
        Me.btnDevNuevo.Size = New System.Drawing.Size(90, 32)
        Me.btnDevNuevo.TabIndex = 118
        Me.btnDevNuevo.Text = "Nuevo"
        Me.btnDevNuevo.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Panel1.Controls.Add(Me.cboEstado)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.dtpHora)
        Me.Panel1.Controls.Add(Me.lblDescripcion)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.txtObs)
        Me.Panel1.Controls.Add(Me.cboProv)
        Me.Panel1.Controls.Add(Me.dtpFecha)
        Me.Panel1.Controls.Add(Me.lblIva)
        Me.Panel1.Controls.Add(Me.cboUsuario)
        Me.Panel1.Controls.Add(Me.lblestado)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.txtDevId)
        Me.Panel1.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel1.Location = New System.Drawing.Point(9, 82)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1328, 258)
        Me.Panel1.TabIndex = 116
        '
        'cboEstado
        '
        Me.cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEstado.FormattingEnabled = True
        Me.cboEstado.Items.AddRange(New Object() {"PENDIENTE", "APROVADO", "ANULADO"})
        Me.cboEstado.Location = New System.Drawing.Point(122, 155)
        Me.cboEstado.Name = "cboEstado"
        Me.cboEstado.Size = New System.Drawing.Size(188, 21)
        Me.cboEstado.TabIndex = 44
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label6.Location = New System.Drawing.Point(65, 160)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(51, 13)
        Me.Label6.TabIndex = 43
        Me.Label6.Text = "ESTADO"
        '
        'dtpHora
        '
        Me.dtpHora.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpHora.Location = New System.Drawing.Point(122, 129)
        Me.dtpHora.Name = "dtpHora"
        Me.dtpHora.ShowUpDown = True
        Me.dtpHora.Size = New System.Drawing.Size(188, 20)
        Me.dtpHora.TabIndex = 42
        '
        'lblDescripcion
        '
        Me.lblDescripcion.AutoSize = True
        Me.lblDescripcion.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblDescripcion.Location = New System.Drawing.Point(78, 133)
        Me.lblDescripcion.Name = "lblDescripcion"
        Me.lblDescripcion.Size = New System.Drawing.Size(38, 13)
        Me.lblDescripcion.TabIndex = 41
        Me.lblDescripcion.Text = "HORA"
        '
        'cboProv
        '
        Me.cboProv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboProv.FormattingEnabled = True
        Me.cboProv.Location = New System.Drawing.Point(122, 49)
        Me.cboProv.Name = "cboProv"
        Me.cboProv.Size = New System.Drawing.Size(188, 21)
        Me.cboProv.TabIndex = 33
        '
        'dtpFecha
        '
        Me.dtpFecha.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFecha.Location = New System.Drawing.Point(122, 103)
        Me.dtpFecha.Name = "dtpFecha"
        Me.dtpFecha.Size = New System.Drawing.Size(188, 20)
        Me.dtpFecha.TabIndex = 31
        '
        'lblIva
        '
        Me.lblIva.AutoSize = True
        Me.lblIva.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblIva.Location = New System.Drawing.Point(74, 106)
        Me.lblIva.Name = "lblIva"
        Me.lblIva.Size = New System.Drawing.Size(42, 13)
        Me.lblIva.TabIndex = 30
        Me.lblIva.Text = "FECHA"
        '
        'cboUsuario
        '
        Me.cboUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboUsuario.FormattingEnabled = True
        Me.cboUsuario.Items.AddRange(New Object() {"PENDIENTE", "APROVADO", "ANULADO"})
        Me.cboUsuario.Location = New System.Drawing.Point(122, 76)
        Me.cboUsuario.Name = "cboUsuario"
        Me.cboUsuario.Size = New System.Drawing.Size(188, 21)
        Me.cboUsuario.TabIndex = 28
        '
        'lblestado
        '
        Me.lblestado.AutoSize = True
        Me.lblestado.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblestado.Location = New System.Drawing.Point(60, 79)
        Me.lblestado.Name = "lblestado"
        Me.lblestado.Size = New System.Drawing.Size(56, 13)
        Me.lblestado.TabIndex = 27
        Me.lblestado.Text = "USUARIO"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label2.Location = New System.Drawing.Point(98, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "ID"
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
        'txtDevId
        '
        Me.txtDevId.Location = New System.Drawing.Point(122, 23)
        Me.txtDevId.Name = "txtDevId"
        Me.txtDevId.Size = New System.Drawing.Size(100, 20)
        Me.txtDevId.TabIndex = 3
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
        'btnDevEditar
        '
        Me.btnDevEditar.FlatAppearance.BorderSize = 0
        Me.btnDevEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDevEditar.Location = New System.Drawing.Point(204, 355)
        Me.btnDevEditar.Name = "btnDevEditar"
        Me.btnDevEditar.Size = New System.Drawing.Size(90, 32)
        Me.btnDevEditar.TabIndex = 120
        Me.btnDevEditar.Text = "Editar"
        Me.btnDevEditar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(31, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(371, 29)
        Me.Label1.TabIndex = 114
        Me.Label1.Text = "DEVOLUCION A PROVEEDOR"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Panel3.Controls.Add(Me.lblStatus)
        Me.Panel3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Panel3.Location = New System.Drawing.Point(-4, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1913, 42)
        Me.Panel3.TabIndex = 117
        '
        'ep
        '
        Me.ep.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ep.ContainerControl = Me
        '
        'btnDevGuardar
        '
        Me.btnDevGuardar.FlatAppearance.BorderSize = 0
        Me.btnDevGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDevGuardar.Location = New System.Drawing.Point(110, 355)
        Me.btnDevGuardar.Name = "btnDevGuardar"
        Me.btnDevGuardar.Size = New System.Drawing.Size(90, 32)
        Me.btnDevGuardar.TabIndex = 119
        Me.btnDevGuardar.Text = "Guardar"
        Me.btnDevGuardar.UseVisualStyleBackColor = True
        '
        'dgvDevProv
        '
        Me.dgvDevProv.AllowUserToAddRows = False
        Me.dgvDevProv.AllowUserToDeleteRows = False
        Me.dgvDevProv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDevProv.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colId, Me.colProv, Me.colUsuario, Me.colFecha, Me.colHora, Me.colEstado, Me.colObs, Me.proid, Me.pagid, Me.colUsuaId})
        Me.dgvDevProv.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.dgvDevProv.Location = New System.Drawing.Point(0, 409)
        Me.dgvDevProv.Name = "dgvDevProv"
        Me.dgvDevProv.ReadOnly = True
        Me.dgvDevProv.Size = New System.Drawing.Size(1370, 340)
        Me.dgvDevProv.TabIndex = 115
        '
        'colId
        '
        Me.colId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colId.DataPropertyName = "devprov_id"
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
        Me.colFecha.DataPropertyName = "devprov_fecha"
        Me.colFecha.HeaderText = "FECHA"
        Me.colFecha.Name = "colFecha"
        Me.colFecha.ReadOnly = True
        '
        'colHora
        '
        Me.colHora.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colHora.DataPropertyName = "devprov_hora"
        Me.colHora.HeaderText = "HORA"
        Me.colHora.Name = "colHora"
        Me.colHora.ReadOnly = True
        '
        'colEstado
        '
        Me.colEstado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colEstado.DataPropertyName = "devprov_estado"
        Me.colEstado.HeaderText = "ESTADO"
        Me.colEstado.Name = "colEstado"
        Me.colEstado.ReadOnly = True
        '
        'colObs
        '
        Me.colObs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colObs.DataPropertyName = "devprov_obs"
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
        'FrmDevolucionProveedor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1370, 749)
        Me.Controls.Add(Me.btnDevNuevo)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.btnDevEditar)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.btnDevGuardar)
        Me.Controls.Add(Me.dgvDevProv)
        Me.Controls.Add(Me.Panel2)
        Me.Name = "FrmDevolucionProveedor"
        Me.Text = "FrmDevolucionProveedor"
        Me.Panel2.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.ep, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvDevProv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label4 As Label
    Friend WithEvents txtObs As TextBox
    Friend WithEvents btnDevNuevo As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents cboProv As ComboBox
    Friend WithEvents dtpFecha As DateTimePicker
    Friend WithEvents lblIva As Label
    Friend WithEvents cboUsuario As ComboBox
    Friend WithEvents lblestado As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtDevId As TextBox
    Friend WithEvents lblStatus As Label
    Friend WithEvents btnDevEditar As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents ep As ErrorProvider
    Friend WithEvents btnDevGuardar As Button
    Private WithEvents BtnDevCerrar As Button
    Friend WithEvents dgvDevProv As DataGridView
    Friend WithEvents dtpHora As DateTimePicker
    Friend WithEvents lblDescripcion As Label
    Friend WithEvents cboEstado As ComboBox
    Friend WithEvents Label6 As Label
    Friend WithEvents BtnDevCancelar As Button
    Friend WithEvents btnDevEliminar As Button
    Friend WithEvents colId As DataGridViewTextBoxColumn
    Friend WithEvents colProv As DataGridViewTextBoxColumn
    Friend WithEvents colUsuario As DataGridViewTextBoxColumn
    Friend WithEvents colFecha As DataGridViewTextBoxColumn
    Friend WithEvents colHora As DataGridViewTextBoxColumn
    Friend WithEvents colEstado As DataGridViewTextBoxColumn
    Friend WithEvents colObs As DataGridViewTextBoxColumn
    Friend WithEvents proid As DataGridViewTextBoxColumn
    Friend WithEvents pagid As DataGridViewTextBoxColumn
    Friend WithEvents colUsuaId As DataGridViewTextBoxColumn
End Class
