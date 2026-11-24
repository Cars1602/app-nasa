namespace SpaceExplorer.Modelo;

// Información sencilla para la interfaz. SQLite se añadirá en su propia fase.
public class Descubrimiento
{
    public string NasaId { get; set; } = "";
    public string Titulo { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public DateTimeOffset? FechaCreacion { get; set; }
    public string Centro { get; set; } = "";
    public string TipoContenido { get; set; } = "";
    public string UrlImagen { get; set; } = "";
}
