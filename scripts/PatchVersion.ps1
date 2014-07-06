[CmdletBinding()]
Param([Parameter(Mandatory=$True)][Version]$versionNumber)

# Validate parameters

If ($versionNumber.Revision -eq -1)
{
    throw "Paramter error: Version numbers should be four digit"
}

# Move to the project root folder (parent from current script folder)

function Get-ScriptDirectory
{ 
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value 
    Split-Path $Invocation.MyCommand.Path 
}

$rootFolder = (Get-Item (Get-ScriptDirectory)).Parent.FullName
Set-Location $rootFolder

# Import modules

Import-Module -Name ".\scripts\Update-Version.psm1"

# Patch files

write-host "Patching AssemblyInfo.cs files to" $versionNumber.ToString()

Update-AssemblyInfo ".\src\Okra.Core\Okra.Core.Windows\Properties\AssemblyInfo.cs" $versionNumber
Update-AssemblyInfo ".\src\Okra.Core\Okra.Core.WindowsPhone\Properties\AssemblyInfo.cs" $versionNumber
Update-AssemblyInfo ".\src\Okra.MEF\Okra.MEF.Windows\Properties\AssemblyInfo.cs" $versionNumber
Update-AssemblyInfo ".\src\Okra.MEF\Okra.MEF.WindowsPhone\Properties\AssemblyInfo.cs" $versionNumber

Update-AssemblyInfo ".\test\Okra.Core.Tests\Okra.Core.Tests.Windows\Properties\AssemblyInfo.cs" $versionNumber
Update-AssemblyInfo ".\test\Okra.Core.Tests\Okra.Core.Tests.WindowsPhone\Properties\AssemblyInfo.cs" $versionNumber
Update-AssemblyInfo ".\test\Okra.MEF.Tests\Okra.MEF.Tests.Windows\Properties\AssemblyInfo.cs" $versionNumber
Update-AssemblyInfo ".\test\Okra.MEF.Tests\Okra.MEF.Tests.WindowsPhone\Properties\AssemblyInfo.cs" $versionNumber

write-host "Patching *.nuspec files to" $versionNumber

Update-Nuspec ".\src\Okra.Core\Okra.Core.nuspec" $versionNumber.ToString()
Update-Nuspec ".\src\Okra.MEF\Okra.MEF.nuspec" $versionNumber.ToString()
Update-Nuspec ".\src\Okra.Core\OkraUniversalPreview.Core.nuspec" $versionNumber.ToString()
Update-Nuspec ".\src\Okra.MEF\OkraUniversalPreview.MEF.nuspec" $versionNumber.ToString()

write-host "Patching *.vsixmanifest files to" $versionNumber

Update-VsixManifest ".\templates\base\extension.vsixmanifest" $versionNumber.ToString()

write-host "Patching *.csproj files to" $versionNumber

Update-CsprojReferences ".\templates\ProjectTemplates\CSharp\WindowsApps\OkraBlankApp\Application.csproj" $versionNumber.ToString()
Update-CsprojReferences ".\templates\ProjectTemplates\CSharp\WindowsApps\OkraGridApp\Application.csproj" $versionNumber.ToString()
Update-CsprojReferences ".\templates\ProjectTemplates\CSharp\WindowsApps\OkraHubApp\Application.csproj" $versionNumber.ToString()
Update-CsprojReferences ".\templates\ProjectTemplates\CSharp\WindowsApps\OkraSplitApp\Application.csproj" $versionNumber.ToString()

write-host "Patching *.packageconfig files to" $versionNumber

Update-PackagesConfig ".\templates\ProjectTemplates\CSharp\WindowsApps\shared\packages.config" $versionNumber.ToString()
Update-PackagesConfig ".\templates\ProjectTemplates\CSharp\WindowsApps\OkraBlankApp\packages.config" $versionNumber.ToString()
