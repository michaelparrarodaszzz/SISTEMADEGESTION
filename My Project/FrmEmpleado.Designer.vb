<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmEmpleado
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
        Me.lblfechanac = New System.Windows.Forms.Label()
        Me.cboCiudad = New System.Windows.Forms.ComboBox()
        Me.BtnCerrar = New System.Windows.Forms.Button()
        Me.BtnCancelar = New System.Windows.Forms.Button()
        Me.BtnEliminar = New System.Windows.Forms.Button()
        Me.BtnGuardar = New System.Windows.Forms.Button()
        Me.BtnNuevo = New System.Windows.Forms.Button()
        Me.ep = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.BtnEditar = New System.Windows.Forms.Button()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.txtCi = New System.Windows.Forms.TextBox()
        Me.lblci = New System.Windows.Forms.Label()
        Me.txtDireccion = New System.Windows.Forms.TextBox()
        Me.txtTelefono = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.colid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colnombre = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colapellido = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.coltelefono = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colciudad = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.coldireccion = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colfechanac = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colestado = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colciuid = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colci = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lbltelefono = New System.Windows.Forms.Label()
        Me.lblemail = New System.Windows.Forms.Label()
        Me.lblciudad = New System.Windows.Forms.Label()
        Me.txtApellido = New System.Windows.Forms.TextBox()
        Me.lblapellido = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNombre = New System.Windows.Forms.TextBox()
        Me.txtId = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.chkSinFecha = New System.Windows.Forms.CheckBox()
        Me.lblestado = New System.Windows.Forms.Label()
        Me.dtpFechaNac = New System.Windows.Forms.DateTimePicker()
        Me.cboEstado = New System.Windows.Forms.ComboBox()
        CType(Me.ep, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Panel2.Location = New System.Drawing.Point(9, 373)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(767, 57)
        Me.Panel2.TabIndex = 91
        '
        'lblfechanac
        '
        Me.lblfechanac.AutoSize = True
        Me.lblfechanac.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblfechanac.Location = New System.Drawing.Point(27, 212)
        Me.lblfechanac.Name = "lblfechanac"
        Me.lblfechanac.Size = New System.Drawing.Size(70, 13)
        Me.lblfechanac.TabIndex = 27
        Me.lblfechanac.Text = "FECHA  NAC"
        '
        'cboCiudad
        '
        Me.cboCiudad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCiudad.FormattingEnabled = True
        Me.cboCiudad.Location = New System.Drawing.Point(101, 156)
        Me.cboCiudad.Name = "cboCiudad"
        Me.cboCiudad.Size = New System.Drawing.Size(188, 21)
        Me.cboCiudad.TabIndex = 26
        '
        'BtnCerrar
        '
        Me.BtnCerrar.FlatAppearance.BorderSize = 0
        Me.BtnCerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCerrar.Location = New System.Drawing.Point(486, 382)
        Me.BtnCerrar.Name = "BtnCerrar"
        Me.BtnCerrar.Size = New System.Drawing.Size(90, 32)
        Me.BtnCerrar.TabIndex = 90
        Me.BtnCerrar.Text = "Cerrar"
        Me.BtnCerrar.UseVisualStyleBackColor = True
        '
        'BtnCancelar
        '
        Me.BtnCancelar.FlatAppearance.BorderSize = 0
        Me.BtnCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnCancelar.Location = New System.Drawing.Point(392, 382)
        Me.BtnCancelar.Name = "BtnCancelar"
        Me.BtnCancelar.Size = New System.Drawing.Size(90, 32)
        Me.BtnCancelar.TabIndex = 89
        Me.BtnCancelar.Text = "Cancelar"
        Me.BtnCancelar.UseVisualStyleBackColor = True
        '
        'BtnEliminar
        '
        Me.BtnEliminar.FlatAppearance.BorderSize = 0
        Me.BtnEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnEliminar.Location = New System.Drawing.Point(298, 382)
        Me.BtnEliminar.Name = "BtnEliminar"
        Me.BtnEliminar.Size = New System.Drawing.Size(90, 32)
        Me.BtnEliminar.TabIndex = 88
        Me.BtnEliminar.Text = "Eliminar"
        Me.BtnEliminar.UseVisualStyleBackColor = True
        '
        'BtnGuardar
        '
        Me.BtnGuardar.FlatAppearance.BorderSize = 0
        Me.BtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnGuardar.Location = New System.Drawing.Point(110, 382)
        Me.BtnGuardar.Name = "BtnGuardar"
        Me.BtnGuardar.Size = New System.Drawing.Size(90, 32)
        Me.BtnGuardar.TabIndex = 86
        Me.BtnGuardar.Text = "Guardar"
        Me.BtnGuardar.UseVisualStyleBackColor = True
        '
        'BtnNuevo
        '
        Me.BtnNuevo.FlatAppearance.BorderSize = 0
        Me.BtnNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnNuevo.Location = New System.Drawing.Point(16, 382)
        Me.BtnNuevo.Name = "BtnNuevo"
        Me.BtnNuevo.Size = New System.Drawing.Size(90, 32)
        Me.BtnNuevo.TabIndex = 85
        Me.BtnNuevo.Text = "Nuevo"
        Me.BtnNuevo.UseVisualStyleBackColor = True
        '
        'ep
        '
        Me.ep.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.ep.ContainerControl = Me
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
        'BtnEditar
        '
        Me.BtnEditar.FlatAppearance.BorderSize = 0
        Me.BtnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnEditar.Location = New System.Drawing.Point(204, 382)
        Me.BtnEditar.Name = "BtnEditar"
        Me.BtnEditar.Size = New System.Drawing.Size(90, 32)
        Me.BtnEditar.TabIndex = 87
        Me.BtnEditar.Text = "Editar"
        Me.BtnEditar.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.Panel3.Controls.Add(Me.lblStatus)
        Me.Panel3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Panel3.Location = New System.Drawing.Point(-4, 0)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1913, 42)
        Me.Panel3.TabIndex = 84
        '
        'txtCi
        '
        Me.txtCi.Location = New System.Drawing.Point(101, 105)
        Me.txtCi.Name = "txtCi"
        Me.txtCi.Size = New System.Drawing.Size(190, 20)
        Me.txtCi.TabIndex = 25
        '
        'lblci
        '
        Me.lblci.AutoSize = True
        Me.lblci.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblci.Location = New System.Drawing.Point(80, 108)
        Me.lblci.Name = "lblci"
        Me.lblci.Size = New System.Drawing.Size(17, 13)
        Me.lblci.TabIndex = 24
        Me.lblci.Text = "CI"
        '
        'txtDireccion
        '
        Me.txtDireccion.Location = New System.Drawing.Point(103, 183)
        Me.txtDireccion.Name = "txtDireccion"
        Me.txtDireccion.Size = New System.Drawing.Size(290, 20)
        Me.txtDireccion.TabIndex = 22
        '
        'txtTelefono
        '
        Me.txtTelefono.Location = New System.Drawing.Point(101, 131)
        Me.txtTelefono.Name = "txtTelefono"
        Me.txtTelefono.Size = New System.Drawing.Size(188, 20)
        Me.txtTelefono.TabIndex = 23
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(31, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(153, 29)
        Me.Label1.TabIndex = 81
        Me.Label1.Text = "EMPLEADO"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colid, Me.colnombre, Me.colapellido, Me.coltelefono, Me.colciudad, Me.coldireccion, Me.colfechanac, Me.colestado, Me.colciuid, Me.colci})
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.DataGridView1.Location = New System.Drawing.Point(0, 436)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(1370, 313)
        Me.DataGridView1.TabIndex = 82
        '
        'colid
        '
        Me.colid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colid.DataPropertyName = "emp_id"
        Me.colid.HeaderText = "ID"
        Me.colid.Name = "colid"
        Me.colid.ReadOnly = True
        '
        'colnombre
        '
        Me.colnombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colnombre.DataPropertyName = "emp_nombre"
        Me.colnombre.HeaderText = "NOMBRE"
        Me.colnombre.Name = "colnombre"
        Me.colnombre.ReadOnly = True
        '
        'colapellido
        '
        Me.colapellido.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colapellido.DataPropertyName = "emp_apellido"
        Me.colapellido.HeaderText = "APELLIDO"
        Me.colapellido.Name = "colapellido"
        Me.colapellido.ReadOnly = True
        '
        'coltelefono
        '
        Me.coltelefono.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.coltelefono.DataPropertyName = "emp_telefono"
        Me.coltelefono.HeaderText = "TELEFONO"
        Me.coltelefono.Name = "coltelefono"
        Me.coltelefono.ReadOnly = True
        '
        'colciudad
        '
        Me.colciudad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colciudad.DataPropertyName = "ciu_nombre"
        Me.colciudad.HeaderText = "CIUDAD"
        Me.colciudad.Name = "colciudad"
        Me.colciudad.ReadOnly = True
        '
        'coldireccion
        '
        Me.coldireccion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.coldireccion.DataPropertyName = "emp_direccion"
        Me.coldireccion.HeaderText = "DIRECCION"
        Me.coldireccion.Name = "coldireccion"
        Me.coldireccion.ReadOnly = True
        '
        'colfechanac
        '
        Me.colfechanac.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colfechanac.DataPropertyName = "emp_fecha_nacimiento"
        Me.colfechanac.HeaderText = "FECHA NAC."
        Me.colfechanac.Name = "colfechanac"
        Me.colfechanac.ReadOnly = True
        '
        'colestado
        '
        Me.colestado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colestado.DataPropertyName = "emp_estado"
        Me.colestado.HeaderText = "ESTADO"
        Me.colestado.Name = "colestado"
        Me.colestado.ReadOnly = True
        '
        'colciuid
        '
        Me.colciuid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colciuid.DataPropertyName = "ciu_id"
        Me.colciuid.HeaderText = "SI"
        Me.colciuid.Name = "colciuid"
        Me.colciuid.ReadOnly = True
        Me.colciuid.Visible = False
        '
        'colci
        '
        Me.colci.HeaderText = "CI"
        Me.colci.Name = "colci"
        Me.colci.ReadOnly = True
        '
        'lbltelefono
        '
        Me.lbltelefono.AutoSize = True
        Me.lbltelefono.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lbltelefono.Location = New System.Drawing.Point(31, 134)
        Me.lbltelefono.Name = "lbltelefono"
        Me.lbltelefono.Size = New System.Drawing.Size(64, 13)
        Me.lbltelefono.TabIndex = 20
        Me.lbltelefono.Text = "TELEFONO"
        '
        'lblemail
        '
        Me.lblemail.AutoSize = True
        Me.lblemail.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblemail.Location = New System.Drawing.Point(29, 186)
        Me.lblemail.Name = "lblemail"
        Me.lblemail.Size = New System.Drawing.Size(66, 13)
        Me.lblemail.TabIndex = 21
        Me.lblemail.Text = "DIRECCION"
        '
        'lblciudad
        '
        Me.lblciudad.AutoSize = True
        Me.lblciudad.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblciudad.Location = New System.Drawing.Point(47, 159)
        Me.lblciudad.Name = "lblciudad"
        Me.lblciudad.Size = New System.Drawing.Size(48, 13)
        Me.lblciudad.TabIndex = 19
        Me.lblciudad.Text = "CIUDAD"
        '
        'txtApellido
        '
        Me.txtApellido.Location = New System.Drawing.Point(103, 79)
        Me.txtApellido.Name = "txtApellido"
        Me.txtApellido.Size = New System.Drawing.Size(217, 20)
        Me.txtApellido.TabIndex = 12
        '
        'lblapellido
        '
        Me.lblapellido.AutoSize = True
        Me.lblapellido.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblapellido.Location = New System.Drawing.Point(38, 82)
        Me.lblapellido.Name = "lblapellido"
        Me.lblapellido.Size = New System.Drawing.Size(59, 13)
        Me.lblapellido.TabIndex = 10
        Me.lblapellido.Text = "APELLIDO"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label2.Location = New System.Drawing.Point(79, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(18, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "ID"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label3.Location = New System.Drawing.Point(43, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(54, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "NOMBRE"
        '
        'txtNombre
        '
        Me.txtNombre.Location = New System.Drawing.Point(103, 53)
        Me.txtNombre.Name = "txtNombre"
        Me.txtNombre.Size = New System.Drawing.Size(217, 20)
        Me.txtNombre.TabIndex = 1
        '
        'txtId
        '
        Me.txtId.Location = New System.Drawing.Point(103, 27)
        Me.txtId.Name = "txtId"
        Me.txtId.Size = New System.Drawing.Size(100, 20)
        Me.txtId.TabIndex = 3
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Panel1.Controls.Add(Me.chkSinFecha)
        Me.Panel1.Controls.Add(Me.lblestado)
        Me.Panel1.Controls.Add(Me.dtpFechaNac)
        Me.Panel1.Controls.Add(Me.cboEstado)
        Me.Panel1.Controls.Add(Me.lblfechanac)
        Me.Panel1.Controls.Add(Me.cboCiudad)
        Me.Panel1.Controls.Add(Me.txtCi)
        Me.Panel1.Controls.Add(Me.lblci)
        Me.Panel1.Controls.Add(Me.txtDireccion)
        Me.Panel1.Controls.Add(Me.txtTelefono)
        Me.Panel1.Controls.Add(Me.lbltelefono)
        Me.Panel1.Controls.Add(Me.lblemail)
        Me.Panel1.Controls.Add(Me.lblciudad)
        Me.Panel1.Controls.Add(Me.txtApellido)
        Me.Panel1.Controls.Add(Me.lblapellido)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.txtNombre)
        Me.Panel1.Controls.Add(Me.txtId)
        Me.Panel1.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel1.Location = New System.Drawing.Point(9, 82)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1328, 285)
        Me.Panel1.TabIndex = 83
        '
        'chkSinFecha
        '
        Me.chkSinFecha.AutoSize = True
        Me.chkSinFecha.BackColor = System.Drawing.SystemColors.ControlLight
        Me.chkSinFecha.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.chkSinFecha.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.chkSinFecha.Location = New System.Drawing.Point(208, 211)
        Me.chkSinFecha.Name = "chkSinFecha"
        Me.chkSinFecha.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.chkSinFecha.Size = New System.Drawing.Size(79, 17)
        Me.chkSinFecha.TabIndex = 31
        Me.chkSinFecha.Text = "SIN FECHA"
        Me.chkSinFecha.UseVisualStyleBackColor = False
        '
        'lblestado
        '
        Me.lblestado.AutoSize = True
        Me.lblestado.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.lblestado.Location = New System.Drawing.Point(44, 238)
        Me.lblestado.Name = "lblestado"
        Me.lblestado.Size = New System.Drawing.Size(51, 13)
        Me.lblestado.TabIndex = 30
        Me.lblestado.Text = "ESTADO"
        '
        'dtpFechaNac
        '
        Me.dtpFechaNac.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpFechaNac.Location = New System.Drawing.Point(103, 209)
        Me.dtpFechaNac.Name = "dtpFechaNac"
        Me.dtpFechaNac.Size = New System.Drawing.Size(99, 20)
        Me.dtpFechaNac.TabIndex = 29
        '
        'cboEstado
        '
        Me.cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboEstado.FormattingEnabled = True
        Me.cboEstado.Items.AddRange(New Object() {"ACTIVO", "INACTIVO"})
        Me.cboEstado.Location = New System.Drawing.Point(103, 235)
        Me.cboEstado.Name = "cboEstado"
        Me.cboEstado.Size = New System.Drawing.Size(188, 21)
        Me.cboEstado.TabIndex = 28
        '
        'FrmEmpleado
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1370, 749)
        Me.Controls.Add(Me.BtnCerrar)
        Me.Controls.Add(Me.BtnCancelar)
        Me.Controls.Add(Me.BtnEliminar)
        Me.Controls.Add(Me.BtnGuardar)
        Me.Controls.Add(Me.BtnNuevo)
        Me.Controls.Add(Me.BtnEditar)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Name = "FrmEmpleado"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "FrmEmpleado"
        CType(Me.ep, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel2 As Panel
    Friend WithEvents lblfechanac As Label
    Friend WithEvents cboCiudad As ComboBox
    Private WithEvents BtnCerrar As Button
    Private WithEvents BtnCancelar As Button
    Friend WithEvents BtnEliminar As Button
    Friend WithEvents BtnGuardar As Button
    Friend WithEvents BtnNuevo As Button
    Friend WithEvents ep As ErrorProvider
    Friend WithEvents BtnEditar As Button
    Friend WithEvents Panel3 As Panel
    Friend WithEvents lblStatus As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents txtCi As TextBox
    Friend WithEvents lblci As Label
    Friend WithEvents txtDireccion As TextBox
    Friend WithEvents txtTelefono As TextBox
    Friend WithEvents lbltelefono As Label
    Friend WithEvents lblemail As Label
    Friend WithEvents lblciudad As Label
    Friend WithEvents txtApellido As TextBox
    Friend WithEvents lblapellido As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtNombre As TextBox
    Friend WithEvents txtId As TextBox
    Friend WithEvents dtpFechaNac As DateTimePicker
    Friend WithEvents chkSinFecha As CheckBox
    Friend WithEvents colid As DataGridViewTextBoxColumn
    Friend WithEvents colnombre As DataGridViewTextBoxColumn
    Friend WithEvents colapellido As DataGridViewTextBoxColumn
    Friend WithEvents coltelefono As DataGridViewTextBoxColumn
    Friend WithEvents colciudad As DataGridViewTextBoxColumn
    Friend WithEvents coldireccion As DataGridViewTextBoxColumn
    Friend WithEvents colfechanac As DataGridViewTextBoxColumn
    Friend WithEvents colestado As DataGridViewTextBoxColumn
    Friend WithEvents colciuid As DataGridViewTextBoxColumn
    Friend WithEvents colci As DataGridViewTextBoxColumn
    Friend WithEvents lblestado As Label
    Friend WithEvents cboEstado As ComboBox
End Class
