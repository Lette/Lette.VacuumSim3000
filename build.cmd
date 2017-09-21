@echo off
.paket\paket.bootstrapper.exe
.paket\paket.exe install
"%ProgramFiles(x86)%\Microsoft SDKs\F#\4.1\Framework\v4.0\fsi.exe" build.fsx
