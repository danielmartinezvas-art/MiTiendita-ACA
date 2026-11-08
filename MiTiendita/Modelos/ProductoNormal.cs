namespace MiTiendita.Modelos;

/// <summary>
/// EJEMPLO RESUELTO (G2): producto sin descuentos. Usalo como guia para ProductoPerecedero.
/// </summary>
public class ProductoNormal : ProductoBase
{
    public override string Tipo => "Normal";

    public ProductoNormal() { }

    public ProductoNormal(string codigo, string nombre, decimal precio, int cantidad)
        : base(codigo, nombre, precio, cantidad) { }

    /// <summary>El producto normal se vende al precio de lista.</summary>
    public override decimal CalcularPrecioFinal() => Precio;
}
