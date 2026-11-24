# Análisis previo del JSON real

Consulta observada: https://images-api.nasa.gov/search?q=mars&media_type=image&page_size=10

El navegador devolvió 10 recursos y metadata.total_hits = 26903 en esta consulta. Son observaciones de esa respuesta, no cantidades garantizadas para consultas futuras. `fase2-muestra-nasa.json` conserva únicamente el primer recurso y los metadatos; no representa la página completa.

## Estructura
| Ruta | Contenido observado | Uso |
| --- | --- | --- |
| collection.items | Arreglo de recursos | Recorrer resultados |
| collection.items[].data | Arreglo de metadatos | Seleccionar información de imagen |
| data[].nasa_id | Identificador | Identificar descubrimiento |
| data[].title | Título original | Mostrar sin inventar traducción |
| data[].description | Descripción original y créditos | Conservar para detalle posterior |
| data[].date_created | Fecha ISO 8601 | DateTimeOffset nullable |
| data[].center | HQ en la muestra | Mostrar procedencia |
| data[].media_type | image | Filtrar imágenes |
| collection.items[].links | Versiones de la imagen | Elegir preview HTTPS |
| collection.metadata.total_hits | Total de coincidencias | No confundir con tamaño de página |
| collection.links | Enlaces de navegación | No confundir con imágenes de cada recurso |

En los 10 resultados inspeccionados no se observaron valores nulos en los campos principales. Los enlaces sí variaron: un resultado tenía cuatro versiones y los demás cinco. Las clases admiten campos ausentes o nulos como medida de robustez; su presencia en pruebas sintéticas no se presenta como hallazgo de la consulta real.

El primer recurso es NHQ201905310045, título Mars Celebration. Sus enlaces incluyen medium, small, thumb, large y un original TIFF de 27000000 bytes. La vista previa tiene 33000 bytes. No tomar links[0] como miniatura y no descargar automáticamente el original.

Una búsqueda general de mars también encontró eventos en Mars, Pennsylvania. Para buscar superficies planetarias conviene probar mars surface. Los resultados dependen de la búsqueda de la biblioteca, sin una garantía de clasificación científica automática.

## Manejo de datos incompletos
- Colección raíz ausente o JSON inválido: error de formato, no un supuesto resultado vacío.
- Recursos/listas ausentes o nulas: no fallar al recorrerlos.
- Recurso sin identificador o sin metadatos de imagen: omitir.
- Título, descripción o centro ausentes: texto de respaldo.
- Fecha inválida: null; no sustituir por una fecha inventada.
- Sin enlace HTTPS JPG/PNG/WebP: conservar los datos con URL vacía.

Documentación oficial consultada: https://images.nasa.gov/docs/images.nasa.gov_api_docs.pdf

La consulta real se observó desde el navegador. PowerShell falló por resolución del host. Las 11 pruebas locales posteriores usan HTTP simulado y no prueban conexión desde Android.
