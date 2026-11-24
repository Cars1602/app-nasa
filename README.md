# SpaceExplorer - Explorador del universo

Proyecto educativo independiente de Aplicaciones Móviles I (UPDS). Se desarrollará una app Android con .NET MAUI, NASA Image and Video Library y SQLite para estudiar la distribución directa de APK firmados mediante GitHub Releases. No es una aplicación oficial de la NASA.

## Estado actual: fase 5
La colección ahora utiliza SQLite: permite guardar, consultar, editar notas/categoría y eliminar con confirmación. Se evita guardar dos veces el mismo nasa_id. Cuando es posible, la vista previa se descarga para consultar el guardado sin red. El buscador y los metadatos originales de NASA se conservan.

Mejoras de fase 5: encabezado compacto, contador singular, campos bloqueados durante el guardado y botón de regreso contextual. Las pruebas y sus límites están en Evidencias/fase5-resultados.md. Las comprobaciones de Android real siguen pendientes.

Se comprobaron 17 casos con SQLite real en dos procesos. También se guardó y editó un descubrimiento desde la app Windows; al cerrar y volver a abrir se conservaron sus datos y su imagen. Compilaciones Windows y Android ARM64: 0 errores y 0 advertencias. La desconexión manual y el CRUD en Android todavía requieren pruebas.

La compilación de fase 1 y la prueba del botón inicial están comprobadas. La conexión NASA desde el emulador debe verificarse con los pasos siguientes. El APK Release para distribución corresponde a la fase 6.

## Abrir y comprobar en Visual Studio
1. Abrir `SpaceExplorer.csproj` en Visual Studio.
2. Esperar a que termine la restauración de paquetes.
3. Seleccionar Debug y Windows Machine para trabajar con mayor fluidez.
4. Ejecutar con F5.
5. Pulsar **Comenzar a explorar**, buscar un tema y seleccionar una tarjeta.
6. Tomar capturas de inicio, resultados y detalle. Registrar Windows como plataforma de estas pruebas.

Si se utiliza un dispositivo físico, aceptar la depuración USB solo en el equipo propio. Esta ejecución de desarrollo todavía no es la prueba de descarga e instalación desde GitHub.

## Cómo se comunican los archivos
- `MauiProgram.cs` configura MAUI y las fuentes.
- `App.xaml` reúne estilos; `App.xaml.cs` crea la ventana con AppShell.
- `AppShell.xaml` selecciona `Vistas/InicioPage.xaml` como pantalla inicial.
- `InicioPage.xaml` describe la presentación y las categorías. Su archivo `.xaml.cs` cambia de pestaña y pasa el tema al buscador.
- `InitializeComponent()` conecta el XAML con la clase C#. `Clicked` y `SelectionChanged` ejecutan los métodos de los botones y de selección.
- `Modelo/NasaResponse.cs` representa los niveles del JSON con JsonPropertyName y un lector generado al compilar.
- `Modelo/Descubrimiento.cs` contiene los campos sencillos usados por la interfaz.
- `Data/ApiServicio.cs` consulta HTTP, interpreta JSON y devuelve descubrimientos. Todavía no guarda datos.
- `Vistas/ExplorarPage.xaml.cs` toma la búsqueda, espera al servicio y asigna los resultados a la lista; no bloquea la interfaz mientras espera la red.
- `Vistas/DetallePage.xaml.cs` recibe el descubrimiento seleccionado y lo utiliza como BindingContext. Limpia etiquetas de la descripción para mostrar texto y permite volver a la lista.
- `Modelo/DescubrimientoGuardado.cs` define la tabla local, sus claves, notas, categoría y nombre de imagen. Las propiedades Ignore solo sirven para mostrar datos.
- `Data/BaseDatos.cs` crea y consulta SQLite, guarda registros sin duplicarlos, actualiza notas/categoría y elimina filas e imágenes.
- `App.BaseDatos` comparte la conexión usando una ruta de la aplicación; no se necesita un servidor.
- `Vistas/ColeccionPage` consulta SQLite al abrirse; Ver / editar abre el detalle con los datos e imagen locales.
- `Resources/` contiene recursos gráficos y estilos; `Platforms/Android/`, configuración de Android.

Ver `PLAN_DESARROLLO.md`, `COMANDOS.md` y los registros de `Evidencias/`.

## Comprobar la fase 2
Para trabajar con mayor fluidez se habilitó también Windows: en Visual Studio seleccionar **Windows Machine**, mantener **Debug** y ejecutar con F5. Si el proyecto estaba abierto, aceptar su recarga o cerrarlo y volver a abrirlo. El mismo código de C#, XAML y API se utiliza en ambos destinos. Las pruebas de Windows se registrarán como Windows; las del APK y del dispositivo real seguirán realizándose en Android.

1. Detener la aplicación y volver a ejecutar con F5 para compilar los archivos nuevos.
2. Pulsar **Comenzar a explorar** o la pestaña **Explorar**.
3. Buscar `moon`, `saturn`, `mars surface` y `black hole`, una consulta a la vez.
4. Comprobar indicador de carga, títulos, imágenes, centro y NASA ID. El límite es 10 resultados; no se afirma que cada consulta deba devolver exactamente 10.
5. Buscar sin escribir nada: debe pedir un tema.
6. Probar un término muy improbable, como `zzzinexistente987654321`, y registrar la respuesta observada.
7. Desconectar la red del emulador y hacer una nueva búsqueda: debe mostrar un error de conexión o tiempo de espera, sin cerrarse. Una imagen previamente descargada puede seguir en caché; eso no equivale todavía a la colección offline.
8. Restaurar la conexión y repetir una búsqueda. Tomar capturas de un resultado y de un error; anotar lo observado.

Las pruebas locales de lógica están registradas en `Evidencias/fase2-pruebas-servicio.txt`. No reemplazan estas pruebas de Android. Antes de pasar a fase 3 se comprobará el buscador en el emulador.

Actualización: por la lentitud del emulador, el usuario comprobó el funcionamiento en Windows y autorizó fase 3. Las pruebas específicas de Android siguen pendientes.

## Comprobar fase 3
1. Detener y volver a ejecutar con F5 en Windows Machine; no depender solo de Hot Reload porque cambió la estructura de Shell.
2. En Inicio, pulsar Luna. Debe abrir Explorar, escribir moon y consultar sus imágenes. Repetir con otra categoría.
3. Seleccionar una tarjeta. Verificar título, imagen, descripción, fecha, centro y NASA ID.
4. Pulsar Volver a los resultados. Comprobar que la lista sigue disponible y volver a abrir la misma tarjeta.
5. Cambiar entre Inicio, Explorar y Colección. Desde Colección, volver a Explorar.
6. Probar búsqueda manual y validación vacía; estrechar y ampliar la ventana para revisar la adaptación.
7. Conservar capturas de inicio, lista y detalle. El guardado se incorporó posteriormente en fase 4.

Compilación fase 3 Windows comprobada: 0 errores y 0 advertencias. Se probó la categoría Luna, se recibieron 10 imágenes y se abrió Nearside of the Moon; se regresó y se reabrió la misma tarjeta. Ver Evidencias/fase3-prueba-windows.md. Las demás categorías, la ventana estrecha y Android requieren comprobaciones adicionales.

## Comprobar fase 4
1. Detener y ejecutar con F5 en Windows Machine.
2. Explorar una imagen y abrir sus detalles. Desplazarse hasta Mi descubrimiento.
3. Escribir categoría y notas; pulsar Guardar en mi colección. Observar el mensaje, incluida la disponibilidad de imagen local.
4. Abrir Colección y comprobar el registro. Usar Ver / editar, modificar notas o categoría y pulsar Actualizar notas y categoría.
5. Volver a la colección, cerrar y reabrir la aplicación. Los cambios deben conservarse.
6. Abrir otra vez desde el buscador el mismo NASA ID. Debe reconocer el guardado y permitir editar, sin añadir una fila duplicada.
7. Para comprobar lectura offline, desconectar temporalmente la conexión normal, volver a abrir la app y consultar Colección y su detalle. Restaurar la conexión después. No es necesario desactivar protecciones de seguridad.
8. Probar Eliminar con un registro que se pueda descartar: primero Cancelar y luego confirmar. La fila y su copia local deben retirarse.
9. Tomar capturas de confirmación de guardado, colección, nota editada y lectura sin red. Registrar resultados reales y cualquier fallo.

La colección de Windows es independiente de la de Android. No hay sincronización entre equipos. Las imágenes no descargadas muestran la órbita de la app; el texto y las notas siguen disponibles sin Internet. Eliminar un registro de esta versión no tiene papelera.

## Archivos locales
La base es SpaceExplorer.db3 dentro de FileSystem.AppDataDirectory. Las imágenes quedan en ImagenesColeccion, junto a la base. Se utiliza la ubicación asignada por MAUI, no una ruta fija de Windows. Al desinstalar o borrar los datos de la aplicación se puede perder la colección. La copia del código no incluye tus registros personales.
#   a p p - n a s a  
 