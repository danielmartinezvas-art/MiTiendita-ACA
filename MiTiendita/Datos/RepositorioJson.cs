using System.Text.Json;
using MiTiendita.Modelos;

namespace MiTiendita.Datos;

/// <summary>
/// G6 - Persistencia: guarda y lee los productos en un archivo JSON
/// para que no se pierdan al cerrar la aplicacion.
/// </summary>
public class RepositorioJson
{
    private static readonly JsonSerializerOptions Opciones = new() { WriteIndented = true };

    private readonly string _ruta;

    /// <param name="ruta">Archivo donde se guardan los datos. Por defecto "productos.json".</param>
    public RepositorioJson(string ruta = "productos.json")
    {
        _ruta = ruta;
    }

    /// <summary>Escribe todos los productos en el archivo (lo sobrescribe).</summary>
    public void Guardar(IEnumerable<ProductoBase> productos)
    {
        var lista = productos.ToList();
        string json = JsonSerializer.Serialize(lista, Opciones);
        File.WriteAllText(_ruta, json);
    }

    /// <summary>Lee los productos del archivo. Si el archivo no existe, devuelve una lista vacia.</summary>
    public List<ProductoBase> Cargar()
    {
        if (!File.Exists(_ruta))
            return new List<ProductoBase>();

        string json = File.ReadAllText(_ruta);
        var productos = JsonSerializer.Deserialize<List<ProductoBase>>(json, Opciones);

        if (productos == null)
            return new List<ProductoBase>();

        return productos;
    }
}
