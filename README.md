# ACA Mi Tiendita: gestor de productos

Programación Orientada a Objetos II (INGS0806), CUN. C#, .NET 8, Windows Forms y xUnit.

Docente: Nemis Eduardo Padilla Oliveros

## Integrantes

1. Daniel David Martínez Vásquez
2. [Nombre completo]
3. [Nombre completo]
4. [Nombre completo]

## Contexto

Don Jorge tiene una tienda de barrio y lleva sus productos en un cuaderno. Esta aplicación de escritorio le permite registrar productos, guardarlos en un archivo y ver cuánto valen en pesos y en dólares con la TRM del día.

## Cómo ejecutarlo

Requisitos: Windows con Visual Studio 2022 (carga de trabajo Desarrollo de escritorio de .NET) o con el SDK de .NET 8.

```bash
dotnet run --project MiTiendita
dotnet test
```

En Visual Studio se abre `MiTiendita.sln` y se presiona F5. Las pruebas están en Prueba > Explorador de pruebas.

## Estructura

| Carpeta | Contenido | Guía |
|---|---|---|
| `MiTiendita/Modelos` | ProductoBase (abstracta), ProductoNormal, ProductoPerecedero | G1, G2 |
| `MiTiendita/Datos` | Inventario (List y Dictionary), RepositorioJson | G3, G6 |
| `MiTiendita/Patrones` | ProductoFactory | G4 |
| `MiTiendita/Servicios` | ServicioTrm (API de la TRM) | G7 |
| `MiTiendita/FormPrincipal` | Ventana de la aplicación | G5 |
| `MiTiendita.Tests` | Pruebas unitarias con xUnit | G8 |

## Qué se completó

| Guía | Archivo | Qué se implementó |
|---|---|---|
| G1 | `Modelos/ProductoBase.cs` | El precio debe ser mayor que 0 y la cantidad no puede ser negativa. Si no se cumple, se lanza `ArgumentException`. |
| G2 | `Modelos/ProductoPerecedero.cs` | `CalcularPrecioFinal()` aplica 10 % de descuento cuando faltan menos de 3 días para vencer. |
| G3 | `Datos/Inventario.cs` | `BuscarPorCodigo`, `Eliminar` y `ValorTotal`. |
| G4 | `Patrones/ProductoFactory.cs` | Crea `ProductoNormal` o `ProductoPerecedero` según el tipo; un tipo desconocido lanza `ArgumentException`. |
| G5 | `FormPrincipal.cs` | Botón Eliminar seleccionado con confirmación. |
| G6 | `Datos/RepositorioJson.cs` | Guardar y cargar `productos.json` conservando el tipo de cada producto. |
| G7 | `Servicios/ServicioTrm.cs` | Consulta de la TRM en datos.gov.co y columna PrecioUsd. Sin internet muestra un mensaje y la aplicación sigue abierta. |
| G8 | `MiTiendita.Tests/ProductoTests.cs` | Las 3 pruebas pendientes y 9 pruebas adicionales, 14 en total. |

Bonificación: buscar por código desde la ventana, guardado automático al cerrar, columna Vence en la tabla y pruebas adicionales.

## API usada

[TRM de datos.gov.co](https://www.datos.gov.co/resource/32sa-8pi3.json?$order=vigenciadesde%20DESC&$limit=1). Es pública, gratuita y no necesita clave.
