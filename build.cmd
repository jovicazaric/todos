@echo off
cls

.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

packages\FAKE\tools\FAKE.exe build.fsx %*

if errorlevel 1 (
  exit /b %errorlevel%
)

.\Todos\bin\Debug\net461\Todos.exe