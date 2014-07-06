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

# Check NuGet is installed and updated

If (!(Test-Path .\.nuget\nuget.exe))
{
    New-Item .\.nuget -type directory -Force | Out-Null
    Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '.\.nuget\nuget.exe'
}

.\.nuget\NuGet.exe update -self

# Push packages to NuGet.org

.\.nuget\NuGet.exe push .\artifacts\Okra.Core.$versionNumber.nupkg
.\.nuget\NuGet.exe push .\artifacts\Okra.MEF.$versionNumber.nupkg