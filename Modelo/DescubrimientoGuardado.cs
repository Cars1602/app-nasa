using System.Globalization;
using SQLite;

namespace SpaceExplorer.Modelo;

[Table("Descubrimientos")]
public class DescubrimientoGuardado
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    [NotNull, Unique]
    public string NasaId { get; set; } = "";
    public string Titulo { get; set; } = "";
    public string Descripcion { get; set; } = "";
    // ISO 8601 conserva la zona y evita depender del formato SQLite de DateTimeOffset.
    public string FechaCreacionIso { get; set; } = "";
    public string Centro { get; set; } = "";
    public string TipoContenido { get; set; } = "image";
    public string UrlImagen { get; set; } = "";
    public string Notas { get; set; } = "";
    public string Categoria { get; set; } = "General";
    public string ArchivoImagen { get; set; } = "";

    // La ruta se reconstruye al consultar; solo se persiste el nombre del archivo.
    [Ignore]
    public string RutaImagenLocal { get; set; } = "";
    [Ignore]
    public string ImagenMostrar => File.Exists(RutaImagenLocal) ? RutaImagenLocal : "orbita.png";
    [Ignore]
    public string EstadoImagen => File.Exists(RutaImagenLocal) ? "Imagen disponible sin conexión" : "Sin copia local de la imagen";

    public Descubrimiento ComoDescubrimiento() => new()
    {
        NasaId = NasaId, Titulo = Titulo, Descripcion = Descripcion,
        FechaCreacion = DateTimeOffset.TryParse(FechaCreacionIso, CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var fecha) ? fecha : null,
        Centro = Centro, TipoContenido = TipoContenido, UrlImagen = UrlImagen
    };
}
