$logFile = "script.log"
function WriteToLog ([string]$msg) {
   [string]::Concat((get-date), " - ", $msg  ) | Out-File -filepath $logFile -Append
   [string]::Concat((get-date), " - ", $msg  ) | Write-Output
}

$msi = "`"$PSScriptRoot\SimpleApp.msi`""
$arg = " /i $msi /qb!-"

WriteToLog "Installation started"
WriteToLog "Commandline : msiexec.exe $arg"

$cmd = Start-Process -FilePath "msiexec.exe" -ArgumentList $arg -Wait -PassThru
$cmd.WaitForExit()
$exitCode = $cmd.ExitCode
WriteToLog "Process completed. Return Code $($exitCode)"

$number = 10
$zero = 0
$result = $number / $zero

Write-Output $result
[Environment]::Exit($exitCode)