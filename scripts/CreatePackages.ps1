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

# Create the artifacts directory if it doesn't exist

New-Item .\artifacts -type directory -Force | Out-Null

# Check NuGet is installed and updated

Install-NuGet

# Create packages

Add-NuGetPackage .\src\Okra.Core\Okra.Core.nuspec .\artifacts
Add-NuGetPackage .\src\Okra.MEF\Okra.MEF.nuspec .\artifacts
Add-NuGetPackage .\src\Okra.Core\OkraUniversalPreview.Core.nuspec .\artifacts
Add-NuGetPackage .\src\Okra.MEF\OkraUniversalPreview.MEF.nuspec .\artifacts