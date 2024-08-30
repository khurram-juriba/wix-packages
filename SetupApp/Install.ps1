$logFile = "script.log"
function WriteToLog ([string]$msg) {
   [string]::Concat((get-date), " - ", $msg  ) | Out-File -filepath $logFile -Append
   [string]::Concat((get-date), " - ", $msg  ) | Write-Output
}

$msi = "`"$PSScriptRoot\SimpleApp.msi`""
$arg = " /i $msi /l*v+ install.log /qb!-"

WriteToLog "Installation started"
WriteToLog "Commandline : msiexec.exe $arg"

$cmd = Start-Process -FilePath "msiexec.exe" -ArgumentList $arg -Wait -PassThru
$cmd.WaitForExit()
$exitCode = $cmd.ExitCode

If (!($exitCode -eq 0 -or $exitCode -eq 3010)) {
   WriteToLog "Installation failed. Return Code $($exitCode)"
   [Environment]::Exit($exitCode)
}
Else {
   WriteToLog "Installation completed. Return Code $($exitCode)"
   [Environment]::Exit($exitCode)
}
