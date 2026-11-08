using MiTiendita.Modelos;

namespace MiTiendita.Patrones;

/// <summary>
/// G4 - Patron Factory (fabrica): el formulario no hace "new" de cada tipo de producto,
/// le pide a la fabrica que lo cree segun el tipo elegido en pantalla.
/// </summary>
public static class ProductoFactory
{
    /// <summary>
    /// Crea el producto correcto segun el tipo ("Normal" o "Perecedero").
    /// </summary>
    public static ProductoBase Crear(string tipo, string codigo, string nombre,
        decimal precio, int cantidad, DateTime fechaVencimiento)
    {
        switch (tipo)
        {
            case "Normal":
                return new ProductoNormal(codigo, nombre, precio, cantidad);
            case "Perecedero":
                return new ProductoPerecedero(codigo, nombre, precio, cantidad, fechaVencimiento);
            default:
                throw new ArgumentException("Tipo de producto no valido");
        }
    }
}
