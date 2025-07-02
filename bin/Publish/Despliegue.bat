@echo off
setlocal

REM === Ruta del sitio publicado (ajústala a la tuya)
set SITIO_PATH=C:\Users\Deivy\source\repos\Papeleria\bin\Publish

REM === Crear carpeta de logs
mkdir "%SITIO_PATH%\logs"

REM === Dar permisos a IIS_IUSRS
icacls "%SITIO_PATH%" /grant "IIS_IUSRS:(OI)(CI)RX" /T
icacls "%SITIO_PATH%\logs" /grant "IIS_IUSRS:(OI)(CI)M" /T

REM === Reiniciar IIS
iisreset

echo ----------------------------------------
echo IIS reiniciado y permisos configurados.
echo Verifica tu aplicación en el navegador.
pause
