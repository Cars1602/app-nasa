# SpaceExplorer v1.0.0 para Android

## APK generado el 24 de noviembre de 2026

- Archivo `bin/Release/net10.0-android/SpaceExplorer-v1.0.0.apk`.
- Paquete `com.upds.spaceexplorer`, versión `1.0.0`, código de versión `1`.
- Android API mínima 21, API objetivo 36. Arquitecturas `arm64-v8a` y `x86_64`.
- Tamaño de 66 737 919 bytes. SHA-256 del APK `9f475ca8a71594650b7181331ba4313a512ec2b91299708129e537d04a7036c4`.
- Compilación Release con cero errores y cero advertencias. Se desactivaron el recorte de ensamblados y AOT para evitar un fallo de conexión con el proceso de tareas de MSBuild durante el recorte (`MSB4216`).
- `zipalign` verificó la alineación del APK firmado a 16 KiB. `apksigner verify --verbose --print-certs` verificó las firmas v1, v2 y v3. Emitió avisos sobre metadatos bajo `META-INF` sin protección individual en el esquema v1.
- Certificado `CN=SpaceExplorer Release`, RSA de 3072 bits. Huella SHA-256 del certificado `de83deb10255532f38362b954d6c5ee0636144252a3640eca2357f3592f53fbe`.

La identidad de firma está en `firmas/spaceexplorer-release.p12`, con el alias `spaceexplorer`. La contraseña se guarda cifrada con DPAPI de Windows en `firmas/spaceexplorer-release.dpapi`. La carpeta `firmas/` está excluida por `.gitignore`. Conserva la identidad y el acceso a la contraseña para firmar futuras actualizaciones con el mismo certificado. El archivo DPAPI depende de la cuenta de Windows que lo creó.

## Pruebas locales

El verificador ejecutó el código de `Data/` y `Modelo/` en Windows con SQLite real y respuestas HTTP controladas. Pasaron ocho comprobaciones.

| Caso | Resultado observado |
| --- | --- |
| Buscar `lunar surface` | Interpretó el NASA ID y el enlace de vista previa de la respuesta de prueba. |
| Búsqueda vacía | Rechazó la consulta con la validación prevista. |
| Colección inicial | La base de pruebas comenzó vacía. |
| Guardar | Creó un registro y un archivo de vista previa local. |
| Guardar dos veces | Conservó un solo registro para el NASA ID. |
| Editar | La nota y categoría nuevas se conservaron al cerrar y reabrir SQLite. |
| Leer sin red | Recuperó el detalle y la ruta del archivo local sin solicitudes HTTP. |
| Eliminar | Quitó el registro y la copia local. |

La consulta directa a la API de NASA desde esta sesión falló al resolver el dominio. Quedan pendientes la prueba con la API en el celular, las capturas en Android y la publicación y descarga desde GitHub Releases.

## Comprobación pendiente en el celular

1. Cuando se publique `v1.0.0`, abrir la página pública de Releases desde el navegador del celular y descargar `SpaceExplorer-v1.0.0.apk` desde **Assets**. Registrar la URL y comprobar que la descarga funciona sin iniciar sesión.
2. Abrir el APK descargado. Si Android pide autorización para instalar desde esa fuente, registrar la pantalla y el texto de la advertencia. Play Protect puede analizar el paquete o mostrar su propio mensaje. Si el equipo tiene una versión Debug con otra firma, Android puede rechazar la actualización. Antes de desinstalarla, comprobar si contiene una colección que se deba conservar. [Ayuda de Android sobre descargas](https://support.google.com/android/answer/9457058) · [Firma de aplicaciones Android](https://developer.android.com/studio/publish/app-signing)
3. Abrir la aplicación con conexión, buscar `moon` y capturar la lista y el detalle de un resultado. Guardar ese resultado con una nota y una categoría de prueba. Capturar la confirmación y la fila en Colección.
4. Editar la nota y categoría. Cerrar la app, activar el modo avión y comprobar que el Wi-Fi está apagado. Reabrir Colección y capturar la nota editada y la imagen local. Si el guardado informó que no pudo descargar la vista previa, comprobar el texto y registrar el marcador mostrado.
5. Abrir el aviso de eliminación del registro de prueba. Cancelar y comprobar que se conserva. Volver a eliminar, confirmar y capturar la colección sin el registro. Restaurar la conexión.
