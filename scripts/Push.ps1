[CmdletBinding()]
Param([Parameter(Mandatory=$True)][Version]$versionNumber)

# Move to the project root folder (parent from current script folder)

function Get-ScriptDirectory
{ 
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value 
    Split-Path $Invocation.MyCommand.Path 
}

$rootFolder = (Get-Item (Get-ScriptDirectory)).Parent.FullName
Set-Location $rootFolder

# Import modules

Import-Module -Name ".\scripts\Invoke-MsBuild.psm1"
Import-Module -Name ".\scripts\Invoke-NuGet.psm1"

# Check NuGet is installed and updated

Install-NuGet

# Push packages to NuGet.org

Push-NuGetPackage .\artifacts\Okra.Core.$versionNumber.nupkg
Push-NuGetPackage .\artifacts\Okra.MEF.$versionNumber.nupkg