using MiTiendita.Datos;
using MiTiendita.Modelos;
using MiTiendita.Patrones;
using MiTiendita.Servicios;

namespace MiTiendita;

/// <summary>
/// G5 - Interfaz grafica: ventana principal de la tienda.
/// Los controles (cajas de texto, botones, tabla) estan en FormPrincipal.Designer.cs.
/// </summary>
public partial class FormPrincipal : Form
{
    private readonly Inventario _inventario = new();
    private readonly RepositorioJson _repositorio = new();
    private readonly ServicioTrm _servicioTrm = new();

    /// <summary>Ultima TRM consultada. Es null mientras no se haya consultado.</summary>
    private decimal? _trm;

    public FormPrincipal()
    {
        InitializeComponent();
    }

    private void FormPrincipal_Load(object sender, EventArgs e)
    {
        cmbTipo.SelectedIndex = 0;

        // Al abrir la ventana se cargan los productos guardados (G6).
        _inventario.Cargar(_repositorio.Cargar());
        RefrescarTabla();
    }

    /// <summary>EJEMPLO RESUELTO: lee los datos de la pantalla, crea el producto y lo agrega.</summary>
    private void btnAgregar_Click(object sender, EventArgs e)
    {
        try
        {
            if (!decimal.TryParse(txtPrecio.Text, out decimal precio))
                throw new ArgumentException("El precio debe ser un numero.");

            var producto = ProductoFactory.Crear(
                cmbTipo.Text,
                txtCodigo.Text,
                txtNombre.Text,
                precio,
                (int)numCantidad.Value,
                dtpVencimiento.Value);

            _inventario.Agregar(producto);
            RefrescarTabla();
            LimpiarCampos();
        }
        catch (Exception ex)
        {
            // Las validaciones de las clases (G1) llegan aqui como excepciones.
            MessageBox.Show(ex.Message, "No se pudo agregar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void btnEliminar_Click(object sender, EventArgs e)
    {
        if (dgvProductos.CurrentRow == null)
        {
            MessageBox.Show("Seleccione el producto que desea eliminar.");
            return;
        }

        string? codigo = dgvProductos.CurrentRow.Cells["Codigo"].Value?.ToString();
        if (string.IsNullOrEmpty(codigo))
            return;

        var respuesta = MessageBox.Show("¿Seguro que desea eliminar el producto " + codigo + "?",
            "Eliminar producto", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (respuesta == DialogResult.Yes)
        {
            _inventario.Eliminar(codigo);
            RefrescarTabla();
        }
    }

    private void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            _repositorio.Guardar(_inventario.Productos);
            MessageBox.Show("Productos guardados correctamente.", "Guardar");
        }
        catch (Exception ex)
        {
            MessageBox.Show("No se pudo guardar el archivo: " + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnTrm_Click(object sender, EventArgs e)
    {
        lblTrm.Text = "Consultando...";
        btnTrm.Enabled = false;

        _trm = await _servicioTrm.ObtenerTrmAsync();

        if (_trm == null)
        {
            lblTrm.Text = "No se pudo consultar la TRM";
            MessageBox.Show("No se pudo consultar la TRM. Revise la conexion a internet.", "TRM",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        else
        {
            lblTrm.Text = $"TRM hoy: ${_trm:N2}";
        }

        RefrescarTabla();
        btnTrm.Enabled = true;
    }

    private void btnBuscar_Click(object sender, EventArgs e)
    {
        var producto = _inventario.BuscarPorCodigo(txtBuscar.Text);
        if (producto == null)
        {
            MessageBox.Show("No existe un producto con ese codigo.", "Buscar");
            return;
        }

        foreach (DataGridViewRow fila in dgvProductos.Rows)
        {
            if (fila.Cells["Codigo"].Value?.ToString() == producto.Codigo)
            {
                dgvProductos.CurrentCell = fila.Cells["Codigo"];
                break;
            }
        }
    }

    private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
    {
        try
        {
            _repositorio.Guardar(_inventario.Productos);
        }
        catch (Exception)
        {
            MessageBox.Show("No se pudieron guardar los productos al cerrar.", "Guardar");
        }
    }

    private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        // La fecha de vencimiento solo aplica a productos perecederos.
        dtpVencimiento.Enabled = cmbTipo.Text == "Perecedero";
    }

    /// <summary>Vuelve a dibujar la tabla con los productos del inventario.</summary>
    private void RefrescarTabla()
    {
        dgvProductos.DataSource = _inventario.Productos
            .Select(p => new
            {
                p.Codigo,
                p.Nombre,
                p.Tipo,
                Vence = p is ProductoPerecedero per ? per.FechaVencimiento.ToShortDateString() : "",
                Precio = p.Precio.ToString("N0"),
                PrecioFinal = p.CalcularPrecioFinal().ToString("N0"),
                PrecioUsd = _trm is null ? "-" : (p.CalcularPrecioFinal() / _trm.Value).ToString("N2"),
                p.Cantidad
            })
            .ToList();

        lblTotal.Text = $"Valor del inventario: ${_inventario.ValorTotal():N0}";
    }

    private void LimpiarCampos()
    {
        txtCodigo.Clear();
        txtNombre.Clear();
        txtPrecio.Clear();
        numCantidad.Value = 1;
        txtCodigo.Focus();
    }
}
