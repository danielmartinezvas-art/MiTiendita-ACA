using System.Text.Json.Serialization;

namespace MiTiendita.Modelos;

/// <summary>
/// Clase base de todos los productos de la tienda (G1 y G2).
/// Es abstracta: no se puede crear un "ProductoBase" directamente, solo sus clases hijas.
/// </summary>
/// <remarks>
/// Los atributos [JsonPolymorphic] y [JsonDerivedType] le dicen a System.Text.Json
/// que guarde el tipo de cada producto en el archivo (G6). No los borres.
/// </remarks>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "tipo")]
[JsonDerivedType(typeof(ProductoNormal), "normal")]
[JsonDerivedType(typeof(ProductoPerecedero), "perecedero")]
public abstract class ProductoBase
{
    // G1 - Encapsulamiento: los datos son privados y solo se acceden por propiedades.
    private string _codigo = "";
    private string _nombre = "";
    private decimal _precio;
    private int _cantidad;

    /// <summary>Codigo unico del producto (ej: "ARR01"). Se guarda en mayusculas.</summary>
    public string Codigo
    {
        get => _codigo;
        set
        {
            // EJEMPLO RESUELTO: asi se valida un dato antes de guardarlo.
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El codigo no puede estar vacio.");
            _codigo = value.Trim().ToUpper();
        }
    }

    /// <summary>Nombre del producto (ej: "Arroz Diana 500 g").</summary>
    public string Nombre
    {
        get => _nombre;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El nombre no puede estar vacio.");
            _nombre = value.Trim();
        }
    }

    /// <summary>Precio de venta en pesos colombianos.</summary>
    public decimal Precio
    {
        get => _precio;
        set
        {
            if (value <= 0)
                throw new ArgumentException("El precio debe ser mayor que 0.");
            _precio = value;
        }
    }

    /// <summary>Unidades disponibles en la tienda.</summary>
    public int Cantidad
    {
        get => _cantidad;
        set
        {
            if (value < 0)
                throw new ArgumentException("La cantidad no puede ser negativa.");
            _cantidad = value;
        }
    }

    /// <summary>Nombre del tipo para mostrar en la tabla ("Normal", "Perecedero").</summary>
    [JsonIgnore]
    public abstract string Tipo { get; }

    /// <summary>Constructor vacio: lo necesita System.Text.Json para leer el archivo.</summary>
    protected ProductoBase() { }

    protected ProductoBase(string codigo, string nombre, decimal precio, int cantidad)
    {
        Codigo = codigo;
        Nombre = nombre;
        Precio = precio;
        Cantidad = cantidad;
    }

    /// <summary>
    /// G2 - Metodo abstracto: cada tipo de producto decide como calcula su precio final.
    /// </summary>
    public abstract decimal CalcularPrecioFinal();

    public override string ToString() => $"{Codigo} - {Nombre} ({Tipo}) ${CalcularPrecioFinal():N0}";
}
