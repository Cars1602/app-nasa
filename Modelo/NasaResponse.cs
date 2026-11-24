using System.Text.Json.Serialization;

namespace SpaceExplorer.Modelo;

// Estas clases siguen los niveles reales del JSON, no la apariencia de la pantalla.
public class NasaResponse
{
    [JsonPropertyName("collection")]
    public NasaColeccion? Coleccion { get; set; }
}

public class NasaColeccion
{
    [JsonPropertyName("items")]
    public List<NasaRecurso?>? Recursos { get; set; }
}

public class NasaRecurso
{
    [JsonPropertyName("data")]
    public List<NasaDatos?>? Datos { get; set; }

    [JsonPropertyName("links")]
    public List<NasaEnlace?>? Enlaces { get; set; }
}

public class NasaDatos
{
    [JsonPropertyName("nasa_id")]
    public string? NasaId { get; set; }

    [JsonPropertyName("title")]
    public string? Titulo { get; set; }

    [JsonPropertyName("description")]
    public string? Descripcion { get; set; }

    [JsonPropertyName("date_created")]
    public string? FechaCreacion { get; set; }

    [JsonPropertyName("center")]
    public string? Centro { get; set; }

    [JsonPropertyName("media_type")]
    public string? TipoContenido { get; set; }
}

public class NasaEnlace
{
    [JsonPropertyName("href")]
    public string? Url { get; set; }

    [JsonPropertyName("rel")]
    public string? Relacion { get; set; }

    [JsonPropertyName("render")]
    public string? Tipo { get; set; }
}

// Genera el lector JSON al compilar; evita depender de reflexión en Android Release.
[JsonSerializable(typeof(NasaResponse))]
internal partial class NasaJsonContext : JsonSerializerContext
{
}
