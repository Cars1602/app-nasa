# SpaceExplorer - Plan de desarrollo

## Objetivo
Evaluar con pruebas reales la distribución directa de una aplicación .NET MAUI para Android mediante un APK firmado y alojado en GitHub Releases. La aplicación educativa consultará la NASA Image and Video Library y guardará una colección local en SQLite.

## Requisitos del docente verificados
Fuentes revisadas: Actividad 08.pdf, Modelo IMRD.pdf y Articulo Emprendedorismo como pilar de la educación.pdf.
- Aplicación con base de datos y API externa.
- Paquete instalable firmado, publicado en alojamiento gratuito y probado en un dispositivo real.
- Documentar requisitos de firma, diferencias de paquetes firmados/no firmados, advertencias del sistema, ventajas, limitaciones y riesgos.
- Informe IMRD de 12 a 15 páginas; mínimo 10 referencias; entrega digital e impresa.
- Presentación y demostración práctica.
- Rúbrica: introducción/resumen/pregunta 10%; metodología 20%; resultados 20%; discusión 20%; documento y referencias 15%; profundidad y práctica 15%.
- El modelo exige una metodología reproducible; los resultados describen observaciones y la discusión las interpreta. El artículo es una referencia de estructura, no evidencia sobre Android.

## Fases y criterios de cierre
| Fase | Entregable y comprobación | Estado |
| --- | --- | --- |
| 1. Entorno | Proyecto inicial Android, estructura sencilla, compilación y pantalla inicial comprobadas | Comprobada en emulador |
| 2. API | JSON real conservado y analizado antes de crear NasaResponse; servicio HTTP, búsqueda y errores comprobados | Usuario confirma funcionamiento en Windows; Android pendiente |
| 3. Interfaces | Inicio, exploración, detalle, CollectionView y navegación probados | Compilación y prueba básica real comprobadas en Windows |
| 4. SQLite | Guardar sin duplicados, consultar, editar notas/categorías y eliminar; persistencia al reiniciar | Implementada; pruebas SQLite y recorrido Windows comprobados |
| 5. Diseño y pruebas | Casos con/sin Internet, validaciones e imágenes locales; pruebas Android | Mejoras aplicadas; pruebas Windows realizadas, dispositivo Android pendiente |
| 6. APK firmado | Keystore protegido, Release generado, firma verificada y SHA-256 registrado | Pendiente |
| 7. Distribución | Código público sin secretos, v1.0.0, APK adjunto y descarga sin autenticación verificada | Pendiente |
| 8. Dispositivo real | Descarga, advertencias, instalación, ejecución, API y SQLite; evidencias reales | Pendiente |
| 9. IMRD | 12-15 páginas, APA 7, al menos 10 referencias verificadas, resultados reales | Pendiente |
| 10. Exposición | 10 diapositivas, demostración y preguntas para defensa | Pendiente |

No se avanza de fase hasta comprobar la actual. Una compilación correcta no demuestra instalación ni ejecución en un celular.

## Entorno observado
- Visual Studio Community 2026 18.11.3.
- .NET SDK 10.0.403; paquetes MAUI 10.0.110 y 9.0.120.
- Android SDK: plataformas 35 y 36; Build Tools 36.0.0.
- JDK: C:\Program Files\Android\openjdk\jdk-21.0.8; javac confirmó 21.0.8.
- Git 2.53.0. GitHub CLI no localizado en PATH.
- `dotnet workload list` no pudo consultar el Service Control Manager por restricciones de esta sesión. No equivale a ausencia de MAUI.
- Se creó SpaceExplorer usando la plantilla MAUI local, sin descargar plantillas. La carpeta AppNasa se detectó después y no se modificó.

## Avances de fase 1
- [x] Revisar los tres PDF.
- [x] Localizar .NET, Visual Studio, Android SDK, JDK y Git.
- [x] Crear SpaceExplorer y orientar la compilación a net10.0-android.
- [x] Crear Modelo, Data y Vistas; conservar Resources y Platforms.
- [x] Crear InicioPage con botón de comprobación y aviso de proyecto independiente.
- [x] Preparar .gitignore para compilados, configuración de usuario y claves.
- [x] Compilar Debug para Android ARM64: 0 advertencias, 0 errores; 46,57 segundos (ver Evidencias/fase1-compilacion-arm64.txt).
- [x] Ejecutar en Android y pulsar el botón de comprobación: captura enviada por el usuario muestra el mensaje correcto en pixel_7_-_api_36, puerto 5554.
- [x] Recibir captura de la pantalla funcionando. La captura está en la conversación; no se afirma que exista una copia local. La compilación tiene registro de texto.

## Decisiones para las siguientes fases
- API: https://images-api.nasa.gov; documentación: https://images.nasa.gov/docs/images.nasa.gov_api_docs.pdf.
- No se han creado modelos ni un cliente API en fase 1. En fase 2 se consultará JSON real y se comprobarán campos ausentes/nulos, arreglos vacíos y enlaces de vista previa.
- Categorías en español podrán mapearse a consultas en inglés. La interfaz será española; se conservará el idioma original de las descripciones sin inventar traducciones.
- SQLite guardará datos y notas; para imágenes sin conexión se descargará un archivo local y se registrará su ruta. Guardar solo una URL no permite ver la imagen sin Internet.
- No usar el logotipo NASA como identidad de la app. Revisar los créditos y condiciones del recurso; conservar procedencia y enlace.
- Registrar dispositivo, versión Android, versión de la app, pasos, resultado esperado/observado y evidencia en cada prueba.
- Comparar un APK realmente sin firma con uno firmado. Debug también suele estar firmado con una clave de desarrollo; no confundir Debug con ausencia de firma.
- No desactivar protecciones generales de Android. Documentar únicamente los permisos de instalación por origen que el dispositivo solicite.
- No publicar contraseñas, keystores ni claves privadas. No guardar secretos en los registros de comandos.

## Evidencias
Evidencias/fase1-compilacion.txt conserva el intento inicial con NuGet remoto.
Evidencias/fase1-compilacion-local.txt conserva el intento utilizando los paquetes locales.
Evidencias/fase1-compilacion-arm64.txt conserva la compilación correcta. Se especificó android-arm64 y compilación secuencial después de un fallo en la preparación para varios destinos que no mostró errores concretos. No se atribuye una causa que aún no se ha demostrado.
Se produjo bin/Debug/net10.0-android/android-arm64/com.upds.spaceexplorer-Signed.apk, firmado automáticamente con la clave Debug generada por las herramientas. No es el APK Release de distribución y puede depender del despliegue de desarrollo de Visual Studio.
La generación de claves propias de distribución, la comprobación de firma Release y la publicación siguen pendientes. El diagnóstico detallado de MSBuild permanece local y excluido de Git y de la copia compartible porque incluye el entorno de compilación.
Se recibió evidencia de la pantalla inicial en emulador Android API 36. Las pruebas de fase 2 y las pruebas en un celular físico siguen pendientes; no se han simulado.

## Fase 2: API y búsqueda
- [x] Consultar una respuesta real sin API Key desde el navegador, antes de crear los modelos.
- [x] Conservar el primer recurso de la respuesta real: Evidencias/fase2-muestra-nasa.json. Es un extracto, no la respuesta completa.
- [x] Crear NasaResponse y Descubrimiento; admitir campos opcionales, listas nulas y fechas inválidas.
- [x] Crear ApiServicio con HttpClient reutilizable, consulta codificada, HTTPS, límite de 10 resultados y tiempo máximo de 25 segundos.
- [x] Preferir enlaces preview compatibles; evitar descargar el original TIFF.
- [x] Añadir pantalla mínima ExplorarPage para comprobar búsquedas dinámicas, carga, resultados vacíos y errores. El diseño completo y los detalles corresponden a fase 3.
- [x] Ejecutar 11 comprobaciones locales del servicio con HTTP simulado, usando también el recurso real conservado. Ver Evidencias/fase2-pruebas-servicio.txt.
- [x] Compilar fase 2 Debug ARM64: 0 errores, 0 advertencias, 49,37 segundos. Registro: Evidencias/fase2-compilacion-arm64.txt.
- [ ] Probar en el emulador con moon, saturn, mars surface y black hole.
- [ ] Probar consulta vacía, término sin coincidencias y fallo de conexión desde Android.
- [ ] Capturar resultados y errores, sin atribuir al emulador las pruebas simuladas del servicio.

La conexión HTTP desde PowerShell no resolvió images-api.nasa.gov en esta sesión. La consulta real pudo realizarse desde el navegador. Esto no demuestra ni descarta que el emulador tenga conexión.

## Desarrollo en Windows Machine
Por la lentitud del emulador indicada por el usuario, se habilitó net10.0-windows10.0.19041.0 además de Android. Se conserva WindowsPackageType=None y el perfil Windows Machine existente. Las pruebas de buscador y lógica pueden realizarse en Windows; no se contabilizarán como pruebas Android. La distribución final continúa siendo APK para Android.
Registro de la compilación: Evidencias/fase2-compilacion-windows.txt. La ejecución interactiva debe comprobarse en Windows Machine desde Visual Studio.
Resultado comprobado: compilación Debug Windows x64 correcta, 0 errores, 0 advertencias, 31,40 segundos. No se ha comprobado todavía la ejecución de la ventana ni el buscador en Windows.
Actualización: el usuario confirmó posteriormente que funciona en Windows Machine y solicitó avanzar. No se atribuyen a esa confirmación pruebas específicas de errores o de Android.

## Fase 3: interfaces y navegación
- [x] Inicio con identidad gráfica propia, presentación y seis categorías de exploración.
- [x] Shell con pestañas Inicio, Explorar y Colección; colores oscuros y navegación compartida Windows/Android.
- [x] Categorías envían un tema en inglés al buscador mediante ShellNavigationQueryParameters e IQueryAttributable, sin crear servicios nuevos.
- [x] CollectionView muestra tarjetas seleccionables; búsqueda manual, indicador de carga y errores de fase 2 se conservan.
- [x] DetallePage recibe el mismo Descubrimiento de la lista y muestra imagen, título, descripción, fecha, centro y NASA ID. No repite la llamada HTTP.
- [x] Al volver se conserva la lista; selección se limpia para poder abrir nuevamente la misma tarjeta.
- [x] ColeccionPage tiene una presentación inicial y acceso a Explorar. Guardar/editar/eliminar se implementarán con SQLite en fase 4; no hay un botón que simule guardado.
- [x] Compilación Debug Windows x64: 0 advertencias, 0 errores, 10,28 segundos. Evidencias/fase3-compilacion-windows.txt.
- [x] Ejecutar el programa Windows y observar inicio, identidad, categorías y pestañas.
- [x] Pulsar Luna: navegación a Explorar, término moon, carga y 10 imágenes reales.
- [x] Abrir Nearside of the Moon (PIA12235), observar imagen, centro JPL y fecha 24/09/2009; regresar con Atrás y reabrir la misma tarjeta.
- [ ] Probar visualmente en Windows: seis categorías, búsqueda manual, apertura y regreso de detalles, reapertura de la misma tarjeta y cambio entre pestañas.
- [ ] Comprobar una ventana estrecha y otra amplia; revisar textos largos, desplazamiento e imágenes.
- [ ] Guardar capturas de inicio, resultados y detalle para el informe.
- [ ] Verificar estas pantallas en Android antes de la distribución final.

Los íconos vectoriales propios son recursos SVG; MAUI genera sus imágenes PNG durante la compilación. No se utiliza el logotipo de la NASA como identidad. No se han instalado dependencias nuevas ni modificado ApiServicio.
La prueba interactiva anterior se realizó desde el ejecutable Windows, sin el emulador. Su registro es Evidencias/fase3-prueba-windows.md. Las vistas observadas no equivalen a pruebas Android ni a la comprobación de todas las categorías o de todos los tamaños de ventana.

## Fase 4: colección SQLite
- [x] Incorporar sqlite-net-pcl 1.11.285 y sus dependencias disponibles en la caché local.
- [x] DescubrimientoGuardado representa la tabla, con Id autoincremental y nasa_id único. Descubrimiento y el servicio API conservan sus responsabilidades.
- [x] BaseDatos crea SpaceExplorer.db3 en FileSystem.AppDataDirectory; App comparte una instancia.
- [x] Guardar y consultar descubrimientos; evitar duplicados incluso en operaciones concurrentes.
- [x] Editar exclusivamente notas y categoría con consultas parametrizadas. Validar categoría (1-50 caracteres) y notas (hasta 2000).
- [x] Eliminar un registro y su copia de imagen. La pantalla solicita confirmación antes de eliminar.
- [x] Descargar opcionalmente la vista previa HTTPS, con límite de 5 MB y 15 segundos. Si falla, conservar texto y notas.
- [x] Guardar solo el nombre de la imagen y reconstruir su ruta desde la ubicación actual. Abrir un guardado utiliza imagen local o marcador propio, sin cargar su URL remota.
- [x] Ejecutar 17 comprobaciones con SQLite real en dos procesos: 11 de preparación y 6 de reapertura. HTTP controlado; no se presenta como desconexión física.
- [x] Compilar Windows x64: 0 errores, 0 advertencias, 30,46 segundos.
- [x] Compilar Android ARM64: 0 errores, 0 advertencias, 37,47 segundos. No acredita ejecución Android.
- [x] Prueba interactiva Windows con PIA12235: guardar, consultar, editar nota, cerrar y reabrir; conservar nota, categoría e imagen local.
- [ ] Probar la eliminación y su cancelación desde la interfaz (el borrado del servicio sí se comprobó con registros de prueba).
- [ ] Probar manualmente una desconexión real, capturar la colección y abrir el detalle sin red.
- [ ] Comprobar el CRUD y la persistencia desde Android en fase de pruebas.

Evidencias: fase4-compilacion-windows.txt, fase4-compilacion-android.txt, fase4-sqlite-preparar.txt, fase4-sqlite-reabrir.txt y fase4-prueba-windows.md. El verificador reproducible se incluye en tmp/PruebasSqlite.
Se conserva un registro de ejemplo en la colección Windows, categoría Prueba SQLite; no se insertaron los datos sintéticos del verificador en la base de la aplicación. Los archivos .db3 y las copias de imágenes personales se excluyen del código compartido.
