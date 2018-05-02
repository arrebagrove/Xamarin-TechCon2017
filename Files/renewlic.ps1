param(
    [switch]$GetDeveloperLicense = $false
)
 
$ScriptPath = $null
try
{
    $ScriptPath = (Get-Variable MyInvocation).Value.MyCommand.Path
    $ScriptDir = Split-Path -Parent $ScriptPath
}
catch {}
 
function CheckIfNeedDeveloperLicense
{
    $Result = $true
    try
    {
        $Result = (Get-WindowsDeveloperLicense | Where-Object { $_.IsValid }).Count -eq 0
    }
    catch {}
 
    return $Result
}
 
if ($GetDeveloperLicense)
{
        try
        {
            Show-WindowsDeveloperLicenseRegistration
        }
        catch
        {
            $Error[0] # Dump details about the last error
            Write-Host $UiStrings.ErrorGetDeveloperLicenseFailed
        }
        Start-Sleep -Seconds 5
}
else{
    Get-WindowsDeveloperLicense
    if (CheckIfNeedDeveloperLicense)
    {
        try
        {
            $RelaunchArgs = '-ExecutionPolicy Unrestricted -file "' + $ScriptPath + '"' + ' -GetDeveloperLicense'
            $AdminProcess = Start-Process "$PsHome\PowerShell.exe" -windowstyle hidden -Verb RunAs -ArgumentList $RelaunchArgs -PassThru
        }
        catch
        {
            $Error[0] # Dump details about the last error
            Write-Host $UiStrings.ErrorLaunchAdminFailed
        }
    }
    else
    {
        Write-Host "License Valid"
        Start-Sleep -Seconds 3
	}
}
  