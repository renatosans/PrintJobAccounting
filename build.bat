
@REM REQUISITOS:
@REM � necess�rio adicionar o caminho do ANT na vari�vel de ambiente PATH
@REM o ANT por sua vez procura pela vari�vel de ambiente JAVA_HOME que deve apontar para o JDK

@REM CARACTER�STICAS:
@REM Os caminhos utilizados no build s�o relativos, definidos em rela��o a CURRENT_DIR
@REM Nos projetos abaixo o build � feito com a task <exec/> do ANT chamando o compilador csc.exe do .NET Framework
@REM O caminho do .NET Framework 4.0 foi adicionado na vari�vel de ambiente TARGET_FRAMEWORK que � onde o csc.exe se encontra
SET CURRENT_DIR=%CD%
SET TARGET_FRAMEWORK=C:\Windows\Microsoft.NET\Framework\v4.0.30319

CD ClassLibraries\DocMageFramework
CALL ANT
CD /d %CURRENT_DIR%

CD ClassLibraries\iTextSharp
CALL ANT
CD /d %CURRENT_DIR%

CD ClassLibraries\MyXls
CALL ANT
CD /d %CURRENT_DIR%

CD ClassLibraries\SharpZipLib
CALL ANT
CD /d %CURRENT_DIR%

CD ClassLibraries\SharpSnmpLib
CALL ANT
CD /d %CURRENT_DIR%

CD ClassLibraries\AccountingLib
CALL ANT
CD /d %CURRENT_DIR%

CD Services\CopyLogImporter
CALL ANT
CD /d %CURRENT_DIR%

CD Services\PrintLogImporter
CALL ANT
CD /d %CURRENT_DIR%

CD Services\PrintInspector
CALL ANT
CD /d %CURRENT_DIR%

CD Services\PrintLogRouter
CALL ANT
CD /d %CURRENT_DIR%

CD Services\ReportMailer
CALL ANT
CD /d %CURRENT_DIR%

CD DesktopApplications\ServiceKicker
CALL ANT
CD /d %CURRENT_DIR%

CD DesktopApplications\BackupUtility
CALL ANT
CD /d %CURRENT_DIR%

CD DesktopApplications\AccountingClientInstaller
CALL ANT
CD /d %CURRENT_DIR%

CD WebApplications\WebAccounting
CALL ANT
CD /d %CURRENT_DIR%

CD WebApplications\WebAdministrator
CALL ANT
CD /d %CURRENT_DIR%

CD DesktopApplications\AccountingInstaller
CALL ANT
CD /d %CURRENT_DIR%
