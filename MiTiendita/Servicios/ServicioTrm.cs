using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MiTiendita.Servicios;

/// <summary>
/// G7 - Consumo de APIs: consulta la TRM (dolar oficial en Colombia) en la API publica
/// de datos abiertos del gobierno (datos.gov.co). No necesita clave ni registro.
/// </summary>
public class ServicioTrm
{
    /// <summary>Pide el registro mas reciente de la TRM.</summary>
    private const string Url =
        "https://www.datos.gov.co/resource/32sa-8pi3.json?$order=vigenciadesde%20DESC&$limit=1";

    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(10) };

    /// <summary>
    /// Devuelve el valor de la TRM en pesos, o null si no se pudo consultar (sin internet, error, etc.).
    /// </summary>
    /// <remarks>
    /// Ejemplo de respuesta de la API (es una lista con un elemento):
    /// [{"valor":"3157.94","unidad":"COP","vigenciadesde":"2026-10-27T00:00:00.000","vigenciahasta":"2026-10-27T00:00:00.000"}]
    /// Abre la URL en el navegador para verla tu mismo.
    /// </remarks>
    public async Task<decimal?> ObtenerTrmAsync()
    {
        try
        {
            string json = await Http.GetStringAsync(Url);
            var datos = JsonSerializer.Deserialize<List<RespuestaTrm>>(json);

            if (datos == null || datos.Count == 0)
                return null;

            return decimal.Parse(datos[0].Valor, CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            return null;
        }
    }
}

/// <summary>
/// Representa un elemento de la respuesta de la API. [JsonPropertyName] une
/// el nombre del campo en el JSON con la propiedad de C#.
/// </summary>
public class RespuestaTrm
{
    [JsonPropertyName("valor")]
    public string Valor { get; set; } = "";

    [JsonPropertyName("unidad")]
    public string Unidad { get; set; } = "";

    [JsonPropertyName("vigenciadesde")]
    public string VigenciaDesde { get; set; } = "";
}
