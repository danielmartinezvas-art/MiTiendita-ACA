using MiTiendita.Modelos;

namespace MiTiendita.Datos;

/// <summary>
/// G3 - Estructuras de datos: guarda los productos en memoria.
/// La lista conserva el orden; el diccionario permite buscar por codigo rapidamente.
/// </summary>
public class Inventario
{
    private readonly List<ProductoBase> _productos = new();
    private readonly Dictionary<string, ProductoBase> _porCodigo = new();

    /// <summary>Productos en el orden en que se agregaron (solo lectura).</summary>
    public IReadOnlyList<ProductoBase> Productos => _productos;

    /// <summary>
    /// EJEMPLO RESUELTO: agrega un producto a la lista y al diccionario.
    /// No permite codigos repetidos.
    /// </summary>
    public void Agregar(ProductoBase producto)
    {
        if (_porCodigo.ContainsKey(producto.Codigo))
            throw new InvalidOperationException($"Ya existe un producto con el codigo {producto.Codigo}.");

        _productos.Add(producto);
        _porCodigo[producto.Codigo] = producto;
    }

    /// <summary>Busca un producto por su codigo. Devuelve null si no existe.</summary>
    public ProductoBase? BuscarPorCodigo(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return null;

        if (_porCodigo.TryGetValue(codigo.Trim().ToUpper(), out ProductoBase? producto))
            return producto;

        return null;
    }

    /// <summary>Elimina un producto por codigo. Devuelve true si lo encontro y lo elimino.</summary>
    public bool Eliminar(string codigo)
    {
        ProductoBase? producto = BuscarPorCodigo(codigo);
        if (producto == null)
            return false;

        _productos.Remove(producto);
        _porCodigo.Remove(producto.Codigo);
        return true;
    }

    /// <summary>Valor total del inventario: suma de (precio final x cantidad) de cada producto.</summary>
    public decimal ValorTotal()
    {
        decimal total = 0;
        foreach (ProductoBase producto in _productos)
        {
            total += producto.CalcularPrecioFinal() * producto.Cantidad;
        }

        return total;
    }

    /// <summary>Reemplaza todo el inventario (se usa al cargar el archivo JSON).</summary>
    public void Cargar(IEnumerable<ProductoBase> productos)
    {
        _productos.Clear();
        _porCodigo.Clear();
        foreach (var producto in productos)
            Agregar(producto);
    }
}
