# Registro de comandos

## Diagnóstico
```powershell
dotnet --info
dotnet workload list
dotnet new list maui
git --version
& 'C:\Program Files\Android\openjdk\jdk-21.0.8\bin\javac.exe' -version
```
Las consultas de cargas de trabajo/plantillas inicialmente encontraron restricciones de acceso de la sesión. Se usó un directorio temporal para el estado del CLI y las plantillas, sin modificar la instalación de Visual Studio.

## Creación realizada desde la carpeta Actividad 06
```powershell
$env:DOTNET_CLI_HOME = Join-Path $env:TEMP 'SpaceExplorer-dotnet'
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
dotnet new install 'C:\Program Files\dotnet\template-packs\Microsoft.Maui.Templates.net10.10.0.110.nupkg' --debug:custom-hive (Join-Path $env:TEMP 'SpaceExplorer-templates')
dotnet new maui -n SpaceExplorer -o SpaceExplorer --no-restore --debug:custom-hive (Join-Path $env:TEMP 'SpaceExplorer-templates')
```
Se adaptó después el proyecto para Android y se sustituyó MainPage por Vistas/InicioPage.

## Compilación de desarrollo habitual (desde SpaceExplorer)
```powershell
dotnet build .\SpaceExplorer.csproj -f net10.0-android -c Debug
```

## Compilación intentada en esta sesión (desde Actividad 06)
Primer intento: `dotnet build` con rutas explícitas de Android/JDK, `--ignore-failed-sources` y `-p:NuGetAudit=false`; no pudo acceder al índice remoto de NuGet. Un intento previo usando DOTNET_CLI_HOME en C:\workspace falló por acceso a esa carpeta, antes de compilar.

Segundo intento, usando los paquetes ya instalados:
```powershell
$env:DOTNET_CLI_HOME = Join-Path $env:TEMP 'SpaceExplorer-dotnet'
$env:NUGET_PACKAGES = 'C:\Users\danie\.nuget\packages'
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
dotnet build '.\SpaceExplorer\SpaceExplorer.csproj' -f net10.0-android -c Debug -p:JavaSdkDirectory='C:\Program Files\Android\openjdk\jdk-21.0.8' -p:AndroidSdkDirectory='C:\Program Files (x86)\Android\android-sdk' -p:NuGetAudit=false --source 'C:\Users\danie\.nuget\packages' -v:minimal
```
La fuente local y NuGetAudit=false se usaron para una verificación sin acceso a NuGet; no son una configuración de publicación. Consultar el resultado real en Evidencias/fase1-compilacion-local.txt.

## Compilación ARM64 comprobada
El intento con varios destinos terminó con fallo sin errores concretos después de compilar C#/XAML. Se conservó un diagnóstico local de MSBuild. Luego se ejecutó:
```powershell
dotnet build '.\SpaceExplorer\SpaceExplorer.csproj' -f net10.0-android -r android-arm64 -c Debug -p:JavaSdkDirectory='C:\Program Files\Android\openjdk\jdk-21.0.8' -p:AndroidSdkDirectory='C:\Program Files (x86)\Android\android-sdk' -p:NuGetAudit=false -p:BuildInParallel=false -m:1 -nr:false --source 'C:\Users\danie\.nuget\packages' -v:normal
```
Se usaron las variables de entorno del segundo intento. Resultado: compilación correcta, 0 advertencias, 0 errores, 46,57 segundos. Registro: Evidencias/fase1-compilacion-arm64.txt.

## Firma y publicación
Las herramientas generaron automáticamente una clave Debug temporal y firmaron el paquete de desarrollo con ella. No se creó una clave propia para distribuir ni se publicó un Release. Las fases 6 y 7 siguen pendientes. Se registrarán los comandos de distribución sin contraseñas ni claves privadas.

## Verificación de fase 2
Se repitió el comando de compilación ARM64 de fase 1, sustituyendo `-v:normal` por `-v:minimal`. Se conservaron las mismas variables de entorno y rutas. Resultado: 0 errores, 0 advertencias; 49,37 segundos. Registro: Evidencias/fase2-compilacion-arm64.txt.

Se ejecutó el verificador local, desde Actividad 06:
```powershell
dotnet run --project '.\tmp\PruebasApi\PruebasApi.csproj' -p:NuGetAudit=false --source 'C:\Users\danie\.nuget\packages'
```
Resultado: 11 comprobaciones correctas. Código del verificador en tmp/PruebasApi (incluido en la copia de fase 2). Utiliza HTTP simulado para verificar la lógica; una prueba utiliza el recurso real conservado como entrada. No demuestra conectividad Android.

La consulta real de la muestra se abrió en el navegador:
https://images-api.nasa.gov/search?q=mars&media_type=image&page_size=10

El intento previo con `Invoke-WebRequest -Uri $url -TimeoutSec 20` falló con host desconocido en esta sesión; no generó un archivo JSON completo. La muestra conservada se extrajo de la respuesta observada en el navegador.

## Destino Windows Machine
Se añadió el destino Windows condicionalmente en SpaceExplorer.csproj. El perfil Windows Machine ya existía. Desde la carpeta SpaceExplorer se puede compilar habitualmente con:
```powershell
dotnet build .\SpaceExplorer.csproj -f net10.0-windows10.0.19041.0 -c Debug
```
En esta sesión, desde Actividad 06 y usando las variables del intento ARM64, se verificó con paquetes locales:
```powershell
dotnet build '.\SpaceExplorer\SpaceExplorer.csproj' -f net10.0-windows10.0.19041.0 -p:TargetFrameworks=net10.0-windows10.0.19041.0 -c Debug -p:NuGetAudit=false -p:BuildInParallel=false -m:1 -nr:false --source 'C:\Users\danie\.nuget\packages' -v:minimal
```
El parámetro TargetFrameworks limita esta verificación a Windows sin quitar Android del proyecto. Evidencia: Evidencias/fase2-compilacion-windows.txt.

## Fase 3: compilación de interfaces
Se repitió exactamente el comando Windows Machine anterior, con las mismas variables de entorno. La salida se guardó en Evidencias/fase3-compilacion-windows.txt.
Resultado: compilación Debug Windows x64 correcta, 0 advertencias, 0 errores, 10,28 segundos. No se ejecutó una nueva prueba de dispositivo Android en esta fase.

## Fase 4: SQLite
Se añadió PackageReference sqlite-net-pcl 1.11.285. La restauración utilizó los paquetes locales, sin descarga de NuGet. Se repitió el comando Windows anterior: 0 errores, 0 advertencias, 30,46 segundos; salida en Evidencias/fase4-compilacion-windows.txt.

Se compiló Android, sin abrir el emulador:
```powershell
dotnet build '.\SpaceExplorer\SpaceExplorer.csproj' -f net10.0-android -p:TargetFrameworks=net10.0-android -r android-arm64 -c Debug -p:JavaSdkDirectory='C:\Program Files\Android\openjdk\jdk-21.0.8' -p:AndroidSdkDirectory='C:\Program Files (x86)\Android\android-sdk' -p:NuGetAudit=false -p:BuildInParallel=false -m:1 -nr:false --source 'C:\Users\danie\.nuget\packages' -v:minimal
```
Resultado: 0 errores, 0 advertencias, 37,47 segundos; Evidencias/fase4-compilacion-android.txt.

El verificador está en tmp/PruebasSqlite; usa una base de prueba separada. Después de restaurar ese proyecto, se ejecutaron DOS procesos:
```powershell
dotnet run --project '.\tmp\PruebasSqlite\PruebasSqlite.csproj' --no-restore -- '.\tmp\PruebasSqlite\datos-fase4\Pruebas.db3' preparar
dotnet run --project '.\tmp\PruebasSqlite\PruebasSqlite.csproj' --no-build --no-restore -- '.\tmp\PruebasSqlite\datos-fase4\Pruebas.db3' comprobar
```
Pasaron 11 y 6 comprobaciones, respectivamente. Se usan SQLite y archivos reales; HTTP es simulado. En el primer intento se pasó --source a dotnet run y se interpretó como argumento del programa, por lo que el verificador ejecutó una fase equivocada y falló. Se corrigió la invocación y se conservó el registro como fase4-verificador-intento-inicial.txt; no se contabiliza ese intento como fallo funcional de la app. Para restaurar desde cero se puede usar dotnet restore con --source antes de dotnet run.

La prueba interactiva se ejecutó con el .exe Windows compilado. Se guardó PIA12235, se editó una nota, se cerró y se reabrió la ventana para comprobar persistencia. No se generó un nuevo APK Release ni se publicó nada en GitHub.
