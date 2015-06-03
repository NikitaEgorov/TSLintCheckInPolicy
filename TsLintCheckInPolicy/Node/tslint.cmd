echo %~dp0
@IF EXIST "%~dp0\node.exe" (
  "%~dp0\node.exe"  "%~dp0\.\node_modules\tslint\bin\tslint-cli.js" %*
) ELSE (
  @SETLOCAL
  @SET PATHEXT=%PATHEXT:;.JS;=;%
  node  "%~dp0\.\node_modules\tslint\bin\tslint-cli.js" %*
)