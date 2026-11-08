namespace MiTiendita;

partial class FormPrincipal
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Codigo generado por el Disenador de Windows Forms

    private void InitializeComponent()
    {
        grpDatos = new GroupBox();
        lblCodigo = new Label();
        txtCodigo = new TextBox();
        lblNombre = new Label();
        txtNombre = new TextBox();
        lblPrecio = new Label();
        txtPrecio = new TextBox();
        lblCantidad = new Label();
        numCantidad = new NumericUpDown();
        lblTipo = new Label();
        cmbTipo = new ComboBox();
        lblVence = new Label();
        dtpVencimiento = new DateTimePicker();
        btnAgregar = new Button();
        btnEliminar = new Button();
        btnGuardar = new Button();
        btnTrm = new Button();
        lblTrm = new Label();
        lblTotal = new Label();
        lblBuscar = new Label();
        txtBuscar = new TextBox();
        btnBuscar = new Button();
        dgvProductos = new DataGridView();
        grpDatos.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numCantidad).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvProductos).BeginInit();
        SuspendLayout();
        //
        // grpDatos
        //
        grpDatos.Controls.Add(lblCodigo);
        grpDatos.Controls.Add(txtCodigo);
        grpDatos.Controls.Add(lblNombre);
        grpDatos.Controls.Add(txtNombre);
        grpDatos.Controls.Add(lblPrecio);
        grpDatos.Controls.Add(txtPrecio);
        grpDatos.Controls.Add(lblCantidad);
        grpDatos.Controls.Add(numCantidad);
        grpDatos.Controls.Add(lblTipo);
        grpDatos.Controls.Add(cmbTipo);
        grpDatos.Controls.Add(lblVence);
        grpDatos.Controls.Add(dtpVencimiento);
        grpDatos.Controls.Add(btnAgregar);
        grpDatos.Controls.Add(btnEliminar);
        grpDatos.Location = new Point(12, 12);
        grpDatos.Name = "grpDatos";
        grpDatos.Size = new Size(300, 340);
        grpDatos.TabStop = false;
        grpDatos.Text = "Datos del producto";
        //
        // lblCodigo / txtCodigo
        //
        lblCodigo.AutoSize = true;
        lblCodigo.Location = new Point(15, 33);
        lblCodigo.Name = "lblCodigo";
        lblCodigo.Text = "Codigo:";
        txtCodigo.Location = new Point(100, 30);
        txtCodigo.Name = "txtCodigo";
        txtCodigo.Size = new Size(180, 23);
        txtCodigo.TabIndex = 0;
        //
        // lblNombre / txtNombre
        //
        lblNombre.AutoSize = true;
        lblNombre.Location = new Point(15, 72);
        lblNombre.Name = "lblNombre";
        lblNombre.Text = "Nombre:";
        txtNombre.Location = new Point(100, 69);
        txtNombre.Name = "txtNombre";
        txtNombre.Size = new Size(180, 23);
        txtNombre.TabIndex = 1;
        //
        // lblPrecio / txtPrecio
        //
        lblPrecio.AutoSize = true;
        lblPrecio.Location = new Point(15, 111);
        lblPrecio.Name = "lblPrecio";
        lblPrecio.Text = "Precio ($):";
        txtPrecio.Location = new Point(100, 108);
        txtPrecio.Name = "txtPrecio";
        txtPrecio.Size = new Size(180, 23);
        txtPrecio.TabIndex = 2;
        //
        // lblCantidad / numCantidad
        //
        lblCantidad.AutoSize = true;
        lblCantidad.Location = new Point(15, 150);
        lblCantidad.Name = "lblCantidad";
        lblCantidad.Text = "Cantidad:";
        numCantidad.Location = new Point(100, 147);
        numCantidad.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        numCantidad.Name = "numCantidad";
        numCantidad.Size = new Size(180, 23);
        numCantidad.TabIndex = 3;
        numCantidad.Value = new decimal(new int[] { 1, 0, 0, 0 });
        //
        // lblTipo / cmbTipo
        //
        lblTipo.AutoSize = true;
        lblTipo.Location = new Point(15, 189);
        lblTipo.Name = "lblTipo";
        lblTipo.Text = "Tipo:";
        cmbTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTipo.Items.AddRange(new object[] { "Normal", "Perecedero" });
        cmbTipo.Location = new Point(100, 186);
        cmbTipo.Name = "cmbTipo";
        cmbTipo.Size = new Size(180, 23);
        cmbTipo.TabIndex = 4;
        cmbTipo.SelectedIndexChanged += cmbTipo_SelectedIndexChanged;
        //
        // lblVence / dtpVencimiento
        //
        lblVence.AutoSize = true;
        lblVence.Location = new Point(15, 228);
        lblVence.Name = "lblVence";
        lblVence.Text = "Vence:";
        dtpVencimiento.Format = DateTimePickerFormat.Short;
        dtpVencimiento.Location = new Point(100, 225);
        dtpVencimiento.Name = "dtpVencimiento";
        dtpVencimiento.Size = new Size(180, 23);
        dtpVencimiento.TabIndex = 5;
        //
        // btnAgregar
        //
        btnAgregar.Location = new Point(15, 265);
        btnAgregar.Name = "btnAgregar";
        btnAgregar.Size = new Size(265, 30);
        btnAgregar.TabIndex = 6;
        btnAgregar.Text = "Agregar producto";
        btnAgregar.UseVisualStyleBackColor = true;
        btnAgregar.Click += btnAgregar_Click;
        //
        // btnEliminar
        //
        btnEliminar.Location = new Point(15, 300);
        btnEliminar.Name = "btnEliminar";
        btnEliminar.Size = new Size(265, 30);
        btnEliminar.TabIndex = 7;
        btnEliminar.Text = "Eliminar seleccionado";
        btnEliminar.UseVisualStyleBackColor = true;
        btnEliminar.Click += btnEliminar_Click;
        //
        // btnGuardar
        //
        btnGuardar.Location = new Point(12, 365);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(300, 30);
        btnGuardar.TabIndex = 8;
        btnGuardar.Text = "Guardar en archivo (JSON)";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += btnGuardar_Click;
        //
        // btnTrm
        //
        btnTrm.Location = new Point(12, 401);
        btnTrm.Name = "btnTrm";
        btnTrm.Size = new Size(300, 30);
        btnTrm.TabIndex = 9;
        btnTrm.Text = "Consultar TRM del dia";
        btnTrm.UseVisualStyleBackColor = true;
        btnTrm.Click += btnTrm_Click;
        //
        // lblTrm
        //
        lblTrm.AutoSize = true;
        lblTrm.Location = new Point(12, 442);
        lblTrm.Name = "lblTrm";
        lblTrm.Text = "TRM: sin consultar";
        //
        // lblBuscar / txtBuscar / btnBuscar
        //
        lblBuscar.AutoSize = true;
        lblBuscar.Location = new Point(325, 17);
        lblBuscar.Name = "lblBuscar";
        lblBuscar.Text = "Buscar codigo:";
        txtBuscar.Location = new Point(420, 14);
        txtBuscar.Name = "txtBuscar";
        txtBuscar.Size = new Size(150, 23);
        txtBuscar.TabIndex = 11;
        btnBuscar.Location = new Point(580, 13);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 25);
        btnBuscar.TabIndex = 12;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = true;
        btnBuscar.Click += btnBuscar_Click;
        //
        // dgvProductos
        //
        dgvProductos.AllowUserToAddRows = false;
        dgvProductos.AllowUserToDeleteRows = false;
        dgvProductos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvProductos.BackgroundColor = SystemColors.Window;
        dgvProductos.Location = new Point(325, 45);
        dgvProductos.MultiSelect = false;
        dgvProductos.Name = "dgvProductos";
        dgvProductos.ReadOnly = true;
        dgvProductos.RowHeadersVisible = false;
        dgvProductos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvProductos.Size = new Size(600, 386);
        dgvProductos.TabIndex = 10;
        //
        // lblTotal
        //
        lblTotal.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblTotal.AutoSize = true;
        lblTotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblTotal.Location = new Point(325, 440);
        lblTotal.Name = "lblTotal";
        lblTotal.Text = "Valor del inventario: $0";
        //
        // FormPrincipal
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(940, 475);
        Controls.Add(grpDatos);
        Controls.Add(btnGuardar);
        Controls.Add(btnTrm);
        Controls.Add(lblTrm);
        Controls.Add(lblBuscar);
        Controls.Add(txtBuscar);
        Controls.Add(btnBuscar);
        Controls.Add(dgvProductos);
        Controls.Add(lblTotal);
        MinimumSize = new Size(900, 500);
        Name = "FormPrincipal";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Mi Tiendita - Gestor de productos";
        FormClosing += FormPrincipal_FormClosing;
        Load += FormPrincipal_Load;
        grpDatos.ResumeLayout(false);
        grpDatos.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numCantidad).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvProductos).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox grpDatos;
    private Label lblCodigo;
    private TextBox txtCodigo;
    private Label lblNombre;
    private TextBox txtNombre;
    private Label lblPrecio;
    private TextBox txtPrecio;
    private Label lblCantidad;
    private NumericUpDown numCantidad;
    private Label lblTipo;
    private ComboBox cmbTipo;
    private Label lblVence;
    private DateTimePicker dtpVencimiento;
    private Button btnAgregar;
    private Button btnEliminar;
    private Button btnGuardar;
    private Button btnTrm;
    private Label lblTrm;
    private Label lblTotal;
    private Label lblBuscar;
    private TextBox txtBuscar;
    private Button btnBuscar;
    private DataGridView dgvProductos;
}
