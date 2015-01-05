# Move to the project root folder (parent from current script folder)

function Get-ScriptDirectory
{ 
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value 
    Split-Path $Invocation.MyCommand.Path 
}

$rootFolder = (Get-Item (Get-ScriptDirectory)).Parent.FullName
Set-Location $rootFolder

# --- Initialization ---

# Check NuGet is installed and updated

If (!(Test-Path .\.nuget\nuget.exe))
{
    New-Item .\.nuget -type directory -Force | Out-Null
    Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '.\.nuget\nuget.exe'
}

.\.nuget\NuGet.exe update -self

# Install required NuGet packages

.\.nuget\NuGet.exe install DotNetZip -OutputDirectory .\packages -Version 1.9.3

# --- Functions ---

function Write-Zip( $sourcefiles, $zipPath )
{
    $item = New-Item .\artifacts\ziptemp -type directory
    Copy-item $sourcefiles .\artifacts\ziptemp -Recurse

    $zipFullPath = Join-Path (pwd) $zipPath

    Add-Type -Path ".\packages\DotNetZip.1.9.3\lib\net20\Ionic.zip.dll"

    $zip = new-object Ionic.Zip.ZipFile
    $zip.AddDirectory($item.FullName)
    $zip.Save($zipFullPath)
    $zip.Dispose()

    Remove-Item .\artifacts\ziptemp -Recurse -Force
    return $zipPath
}

# --- Script ---

# Clean the solution

./scripts/Clean.ps1

# Run the Coverity Build tool

& "C:\Program Files\cov-analysis-win64-7.5.0\bin\cov-build" --dir artifacts\cov-int powershell ./scripts/BuildRelease.ps1 -noTests

# Zip the resulting files

Write-Zip .\artifacts\cov-int .\artifacts\Okra.Core-cov-int.zip