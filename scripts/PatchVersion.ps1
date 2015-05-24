[CmdletBinding()]
Param
(
    [Parameter(Mandatory=$True)][Version]$versionNumber,
    [Parameter(Mandatory=$False)][string]$prereleaseType
)

# Validate parameters

If ($versionNumber.Revision -eq -1)
{
    throw "Parameter error: Version numbers should be four digit"
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

Update-AssemblyInfo ".\src\Okra.Core\Okra.Core.Windows\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\src\Okra.Core\Okra.Core.WindowsPhone\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\src\Okra.Core\Okra.Core.Universal\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\src\Okra.Core\Okra.Core.Xamarin.Forms\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\src\Okra.Core\Okra.Core.Portable\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\src\Okra.MEF\Okra.MEF.Windows\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\src\Okra.MEF\Okra.MEF.WindowsPhone\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\src\Okra.MEF\Okra.MEF.Universal\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\src\Okra.MEF\Okra.Core.Xamarin.Forms\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType	
Update-AssemblyInfo ".\src\Okra.MEF\Okra.MEF.Portable\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType

Update-AssemblyInfo ".\test\Okra.Core.Tests\Okra.Core.Tests.Windows\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\test\Okra.Core.Tests\Okra.Core.Tests.WindowsPhone\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\test\Okra.Core.Tests\Okra.Core.Tests.Universal\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\test\Okra.MEF.Tests\Okra.MEF.Tests.Windows\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\test\Okra.MEF.Tests\Okra.MEF.Tests.WindowsPhone\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType
Update-AssemblyInfo ".\test\Okra.MEF.Tests\Okra.MEF.Tests.Universal\Properties\AssemblyInfo.cs" $versionNumber $prereleaseType

write-host "Patching *.nuspec files to" $versionNumber

Update-Nuspec ".\src\Okra.Core\Okra.Core.nuspec" $versionNumber $prereleaseType
Update-Nuspec ".\src\Okra.MEF\Okra.MEF.nuspec" $versionNumber $prereleaseType

write-host "Patching *.vsixmanifest files to" $versionNumber

Update-VsixManifest ".\templates\base\extension.vsixmanifest" $versionNumber.ToString()

write-host "Patching *.csproj files to" $versionNumber

Update-CsprojReferences ".\templates\ProjectTemplates\CSharp\WindowsApps\OkraBlankApp\Application.csproj" $versionNumber $prereleaseType
Update-CsprojReferences ".\templates\ProjectTemplates\CSharp\WindowsApps\OkraGridApp\Application.csproj" $versionNumber $prereleaseType
Update-CsprojReferences ".\templates\ProjectTemplates\CSharp\WindowsApps\OkraHubApp\Application.csproj" $versionNumber $prereleaseType
Update-CsprojReferences ".\templates\ProjectTemplates\CSharp\WindowsApps\OkraSplitApp\Application.csproj" $versionNumber $prereleaseType

write-host "Patching *.packageconfig files to" $versionNumber

Update-PackagesConfig ".\templates\ProjectTemplates\CSharp\WindowsApps\shared\packages.config" $versionNumber $prereleaseType
Update-PackagesConfig ".\templates\ProjectTemplates\CSharp\WindowsApps\OkraBlankApp\packages.config" $versionNumber $prereleaseType
