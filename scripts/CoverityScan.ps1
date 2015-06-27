# Move to the project root folder (parent from current script folder)

function Get-ScriptDirectory
{ 
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value 
    Split-Path $Invocation.MyCommand.Path 
}

$rootFolder = (Get-Item (Get-ScriptDirectory)).Parent.FullName
Set-Location $rootFolder

# Import modules

Import-Module -Name ".\scripts\Invoke-NuGet.psm1"
Import-Module -Name ".\scripts\Invoke-DotNetZip.psm1"

# Install DotNetZip package via NuGet

Install-DotNetZip

# Clean the solution

./scripts/Clean.ps1

# Run the Coverity Build tool

& "C:\Program Files\cov-analysis-win64-7.6.0\bin\cov-build" --dir artifacts\cov-int powershell ./scripts/BuildRelease.ps1 -noTests

# Zip the resulting files

Write-Zip .\artifacts\cov-int .\artifacts\Okra.Core-cov-int.zip