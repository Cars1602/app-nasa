# Fase 5: mejoras y comprobaciones

Cambios: contador singular de colección; campos de notas/categoría bloqueados durante el guardado; regreso a colección con texto correcto; encabezado compacto para evitar partir SpaceExplorer en ventanas pequeñas.

Windows compiló con 0 errores y 0 advertencias después del ajuste final (31,04 s). El primer intento falló por mantener abierto el ejecutable; se conservó su registro y se recompiló después de cerrarlo.

Pasaron 16 pruebas adicionales con SQLite real y HTTP controlado: fallos/timeout de imagen, contenido vacío/HTML, tamaño excesivo, archivos parciales, protocolos/extensiones inválidos, categoría larga, edición inexistente, imagen local ausente, HTTP 404/503 y timeout del buscador.

Prueba interactiva de acceso restringido: el lanzador separado tmp/PruebasSinRed asignó un proxy inaccesible solo al proceso. La NASA no estaba disponible para el buscador, que mostró su error; Colección y el detalle PIA12235 conservaron imagen, notas y categoría. No se desconectó el Wi-Fi ni se modificó la red del equipo. Esta prueba no equivale a una desconexión física ni a Android.

También se observaron las validaciones de búsqueda vacía y categoría vacía, y se abrió y canceló el diálogo de eliminación: el registro se conservó. El borrado del servicio se había comprobado en SQLite de prueba; no se confirmó un borrado irreversible desde la interfaz.

Después se cerró la ejecución restringida y se abrió SpaceExplorer normalmente. Las seis categorías devolvieron 10 resultados: moon, solar system planets, stars, galaxy, black hole y space mission. zzzinexistente987654321 mostró el mensaje de ausencia de resultados.

Se revisaron ventanas amplia y estrecha. En el ancho final cercano a 349 px se observó SpaceExplorer completo en una línea después del ajuste. Las pestañas adicionales pasan al menú Más en Windows estrecho. No se afirma compatibilidad visual con anchos arbitrariamente pequeños.

El usuario detuvo posteriormente Computer Use con Escape; no se continuó controlando la ventana. Pendientes: ejecución en Android real, desconexión física y confirmación de borrado desde interfaz. No se guardaron capturas PNG locales; las vistas se mostraron durante la sesión.
