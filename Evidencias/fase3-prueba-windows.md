# Prueba real de interfaces en Windows

Fecha local de la sesión: 23 de noviembre de 2026 (America/La_Paz).
Plataforma: ejecutable Debug Windows x64, compilado para net10.0-windows10.0.19041.0. No se utilizó Android en esta prueba.

| Acción realizada | Resultado observado |
| --- | --- |
| Abrir SpaceExplorer.exe compilado | Ventana SpaceExplorer con inicio, órbita propia, presentación, seis categorías y pestañas |
| Pulsar Luna en Inicio | Se abrió Explorar NASA; el buscador mostró moon y el indicador de carga |
| Esperar respuesta | Mensaje de 10 imágenes; imágenes visibles, títulos, centros y NASA ID |
| Seleccionar la primera tarjeta | Se abrió Descubrimiento con Nearside of the Moon, NASA ID PIA12235, centro JPL y fecha 24/09/2009; imagen ampliada visible |
| Pulsar Atrás en la barra de la ventana | Regresó la lista de 10 resultados con moon, sin volver al estado de carga |
| Seleccionar otra vez la misma tarjeta | Se abrió nuevamente el detalle con el mismo título e identificador |

Se inspeccionaron visualmente las pantallas y sus textos de accesibilidad. En Windows, Shell presenta estas pestañas en la parte superior. No se afirma que se haya comprobado aún su ubicación en Android.

En esta observación la descripción del primer recurso era el mismo título, Nearside of the Moon; no se añadió una explicación científica inventada.

Limitaciones: no se probaron todavía todas las categorías, el botón inferior Volver a los resultados (se usó Atrás), la pestaña Colección mediante clic, una ventana estrecha ni los errores de red de esta versión. Se observaron imágenes de la biblioteca real; no se inyectaron respuestas simuladas en esta ejecución.

Las imágenes de la inspección se mostraron durante la sesión; este archivo es el registro textual. No se afirma que existan capturas PNG locales. Para el informe, conservar capturas de Inicio, resultados y detalle mediante la herramienta de capturas de Windows.
