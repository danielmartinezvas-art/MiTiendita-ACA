namespace MiTiendita.Modelos;

/// <summary>
/// Producto que se vence (leche, pan, queso...). Si le quedan menos de 3 dias
/// para vencer, se vende con 10% de descuento (G2 - polimorfismo).
/// </summary>
public class ProductoPerecedero : ProductoBase
{
    /// <summary>Fecha en la que vence el producto.</summary>
    public DateTime FechaVencimiento { get; set; }

    public override string Tipo => "Perecedero";

    public ProductoPerecedero() { }

    public ProductoPerecedero(string codigo, string nombre, decimal precio, int cantidad, DateTime fechaVencimiento)
        : base(codigo, nombre, precio, cantidad)
    {
        FechaVencimiento = fechaVencimiento;
    }

    public override decimal CalcularPrecioFinal()
    {
        double dias = (FechaVencimiento.Date - DateTime.Today).TotalDays;

        if (dias < 3)
            return Precio * 0.90m;

        return Precio;
    }
}
