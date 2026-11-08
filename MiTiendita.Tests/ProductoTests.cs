using MiTiendita.Datos;
using MiTiendita.Modelos;
using MiTiendita.Patrones;

namespace MiTiendita.Tests;

/// <summary>
/// G8 - Pruebas unitarias. Cada prueba sigue el patron AAA:
/// Arrange (preparar), Act (ejecutar), Assert (verificar).
/// Ejecutar todas: dotnet test
/// </summary>
public class ProductoTests
{
    // ---------- EJEMPLOS RESUELTOS ----------

    [Fact]
    public void ProductoNormal_PrecioFinal_EsIgualAlPrecio()
    {
        // Arrange
        var producto = new ProductoNormal("ARR01", "Arroz Diana 500 g", 3500m, 10);

        // Act
        decimal precioFinal = producto.CalcularPrecioFinal();

        // Assert
        Assert.Equal(3500m, precioFinal);
    }

    [Fact]
    public void Inventario_AgregarCodigoRepetido_LanzaExcepcion()
    {
        var inventario = new Inventario();
        inventario.Agregar(new ProductoNormal("ARR01", "Arroz", 3500m, 10));

        Assert.Throws<InvalidOperationException>(
            () => inventario.Agregar(new ProductoNormal("arr01", "Otro arroz", 4000m, 5)));
    }

    [Fact]
    public void ProductoPerecedero_VenceEnMenosDe3Dias_Tiene10PorCientoDeDescuento()
    {
        // Arrange
        var leche = new ProductoPerecedero("LEC01", "Leche Alqueria 1 L", 5000m, 6, DateTime.Today.AddDays(1));

        // Act
        decimal precioFinal = leche.CalcularPrecioFinal();

        // Assert
        Assert.Equal(4500m, precioFinal);
    }

    [Fact]
    public void Producto_PrecioNegativo_LanzaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new ProductoNormal("X1", "Prueba", -100m, 1));
    }

    [Fact]
    public void Factory_TipoPerecedero_CreaProductoPerecedero()
    {
        // Act
        var producto = ProductoFactory.Crear("Perecedero", "PAN01", "Pan tajado", 6500m, 4, DateTime.Today.AddDays(5));

        // Assert
        Assert.IsType<ProductoPerecedero>(producto);
    }

    [Fact]
    public void ProductoPerecedero_VenceEn3DiasOMas_NoTieneDescuento()
    {
        var queso = new ProductoPerecedero("QUE01", "Queso campesino", 12000m, 3, DateTime.Today.AddDays(3));

        Assert.Equal(12000m, queso.CalcularPrecioFinal());
    }

    [Fact]
    public void Producto_PrecioCero_LanzaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new ProductoNormal("X2", "Prueba", 0m, 1));
    }

    [Fact]
    public void Producto_CantidadNegativa_LanzaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new ProductoNormal("X3", "Prueba", 1000m, -1));
    }

    [Fact]
    public void Factory_TipoNoValido_LanzaArgumentException()
    {
        Assert.Throws<ArgumentException>(
            () => ProductoFactory.Crear("Electronico", "TV01", "Televisor", 900000m, 1, DateTime.Today));
    }

    [Fact]
    public void Inventario_BuscarPorCodigo_EncuentraSinImportarMayusculas()
    {
        var inventario = new Inventario();
        inventario.Agregar(new ProductoNormal("ARR01", "Arroz", 3500m, 10));

        ProductoBase? encontrado = inventario.BuscarPorCodigo(" arr01 ");

        Assert.NotNull(encontrado);
        Assert.Equal("Arroz", encontrado!.Nombre);
        Assert.Null(inventario.BuscarPorCodigo("NOEXISTE"));
    }

    [Fact]
    public void Inventario_Eliminar_QuitaElProducto()
    {
        var inventario = new Inventario();
        inventario.Agregar(new ProductoNormal("ARR01", "Arroz", 3500m, 10));

        bool eliminado = inventario.Eliminar("ARR01");

        Assert.True(eliminado);
        Assert.Empty(inventario.Productos);
        Assert.Null(inventario.BuscarPorCodigo("ARR01"));
        Assert.False(inventario.Eliminar("ARR01"));
    }

    [Fact]
    public void Inventario_ValorTotal_SumaPrecioFinalPorCantidad()
    {
        var inventario = new Inventario();
        inventario.Agregar(new ProductoNormal("ARR01", "Arroz", 3500m, 10));
        inventario.Agregar(new ProductoPerecedero("LEC01", "Leche", 5000m, 2, DateTime.Today.AddDays(1)));

        Assert.Equal(44000m, inventario.ValorTotal());
    }

    [Fact]
    public void RepositorioJson_GuardarYCargar_ConservaElTipoDeProducto()
    {
        string ruta = Path.Combine(Path.GetTempPath(), $"productos_prueba_{Guid.NewGuid():N}.json");
        try
        {
            var repositorio = new RepositorioJson(ruta);
            var vence = DateTime.Today.AddDays(2);
            repositorio.Guardar(new List<ProductoBase>
            {
                new ProductoNormal("ARR01", "Arroz", 3500m, 10),
                new ProductoPerecedero("LEC01", "Leche", 5000m, 2, vence)
            });

            List<ProductoBase> cargados = repositorio.Cargar();

            Assert.Equal(2, cargados.Count);
            Assert.IsType<ProductoNormal>(cargados[0]);
            var leche = Assert.IsType<ProductoPerecedero>(cargados[1]);
            Assert.Equal(vence, leche.FechaVencimiento);
            Assert.Equal(4500m, leche.CalcularPrecioFinal());
        }
        finally
        {
            if (File.Exists(ruta))
                File.Delete(ruta);
        }
    }

    [Fact]
    public void RepositorioJson_ArchivoInexistente_DevuelveListaVacia()
    {
        var repositorio = new RepositorioJson(Path.Combine(Path.GetTempPath(), $"no_existe_{Guid.NewGuid():N}.json"));

        Assert.Empty(repositorio.Cargar());
    }
}
