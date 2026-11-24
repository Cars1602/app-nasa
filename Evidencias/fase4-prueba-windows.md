# Prueba real de colección SQLite en Windows

Entorno: SpaceExplorer Debug Windows x64. Se utilizó la ventana real de la app, con la NASA accesible. No se cambió la red ni se usó un dispositivo Android en esta prueba.

| Paso | Resultado observado |
| --- | --- |
| Buscar Luna y abrir Nearside of the Moon | NASA ID PIA12235, centro JPL, fecha 24/09/2009 |
| Escribir categoría y nota | Prueba SQLite; Prueba de guardado local de SpaceExplorer. |
| Pulsar Guardar en mi colección | Mensaje Descubrimiento e imagen guardados para consultar sin conexión; botón cambió a Actualizar notas y categoría |
| Abrir Colección | Un registro, con título, imagen, categoría, nota e indicación de imagen local |
| Usar Ver / editar | Se recuperaron notas y categoría guardadas |
| Cambiar y actualizar nota | Nota editada: persistencia de SpaceExplorer comprobada.; mensaje Notas y categoría actualizadas. |
| Cerrar y volver a abrir SpaceExplorer | Se abrió una nueva ventana con Inicio |
| Abrir Colección sin realizar una nueva búsqueda | Persistieron PIA12235, la categoría Prueba SQLite, la nota editada y la imagen local |

Se dejó el registro de ejemplo en la colección del usuario. No se borró desde la interfaz. Los datos sintéticos del verificador no se insertaron en la base de la app.

La eliminación del servicio SQLite sí se comprobó en una base de prueba separada, junto con el borrado del archivo de imagen. La confirmación/cancelación visual de eliminación, una desconexión física y el CRUD de Android siguen pendientes.

Los registros fase4-sqlite-preparar.txt y fase4-sqlite-reabrir.txt contienen 17 comprobaciones correctas en dos procesos, utilizando SQLite real y HTTP simulado. La consulta y recuperación local se verificaron con cero llamadas HTTP en el segundo proceso del verificador. Esto no sustituye una prueba manual de red desactivada.

Las capturas de inspección se mostraron durante la sesión. Este archivo registra lo observado, pero no se afirma que existan imágenes PNG locales. Para el informe, tomar capturas de la colección, la nota editada y la prueba manual sin red.
