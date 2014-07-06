function Update-AssemblyInfo
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$True, Position = 1)][string]$FileName,
        [Parameter(Mandatory=$True, Position = 2)][Version]$VersionNumber
    )

    $assemblyVersionRegex = 'AssemblyVersion\("[^"]+"\)'
    $assemblyVersion = 'AssemblyVersion("' + $VersionNumber + '")'

    $assemblyFileVersionRegex = 'AssemblyFileVersion\("[^"]+"\)'
    $assemblyFileVersion = 'AssemblyFileVersion("' + $VersionNumber + '")'

    $assemblyInformationalVersionRegex = 'AssemblyInformationalVersion\("[^"]+"\)'
    $assemblyInformationalVersion = 'AssemblyInformationalVersion("' + $VersionNumber.ToString(3) + '")'

    (Get-Content $FileName) |
        ForEach-Object {$_ -replace $assemblyVersionRegex, $assemblyVersion} |
        ForEach-Object {$_ -replace $assemblyFileVersionRegex, $assemblyFileVersion} |
        ForEach-Object {$_ -replace $assemblyInformationalVersionRegex, $assemblyInformationalVersion} |
        Set-Content $FileName
}

function Update-Nuspec
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$True, Position = 1)][string]$FileName,
        [Parameter(Mandatory=$True, Position = 2)][Version]$VersionNumber
    )

    $versionRegex = '<version>[^\<]+</version>'
    $version = '<version>' + $VersionNumber + '</version>'

    $okraCoreRegex = '<dependency id="Okra.Core" version="[^"]+"/>'
    $okraCore = '<dependency id="Okra.Core" version="[' + $versionNumber + ']"/>'

    $okraMefRegex = '<dependency id="Okra.MEF" version="[^"]+"/>'
    $okraMef = '<dependency id="Okra.MEF" version="[' + $versionNumber + ']"/>'

    $okraUniCoreRegex = '<dependency id="OkraUniversalPreview.Core" version="[^"]+"/>'
    $okraUniCore = '<dependency id="OkraUniversalPreview.Core" version="[' + $versionNumber + ']"/>'

    $okraUniMefRegex = '<dependency id="OkraUniversalPreview.MEF" version="[^"]+"/>'
    $okraUniMef = '<dependency id="OkraUniversalPreview.MEF" version="[' + $versionNumber + ']"/>'

    (Get-Content $FileName) |
        ForEach-Object {$_ -replace $versionRegex, $version} |
        ForEach-Object {$_ -replace $okraCoreRegex, $okraCore} |
        ForEach-Object {$_ -replace $okraMefRegex, $okraMef} |
        ForEach-Object {$_ -replace $okraUniCoreRegex, $okraUniCore} |
        ForEach-Object {$_ -replace $okraUniMefRegex, $okraUniMef} |
        Set-Content $FileName
}

function Update-VsixManifest
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$True, Position = 1)][string]$FileName,
        [Parameter(Mandatory=$True, Position = 2)][Version]$VersionNumber
    )

    $versionRegex = '<Identity Id="OkraAppFramework" Version="[^"]+"'
    $version = '<Identity Id="OkraAppFramework" Version="' + $VersionNumber + '"'

    (Get-Content $FileName) |
        ForEach-Object {$_ -replace $versionRegex, $version} |
        Set-Content $FileName
}

function Update-PackagesConfig
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$True, Position = 1)][string]$FileName,
        [Parameter(Mandatory=$True, Position = 2)][Version]$VersionNumber
    )

    $okraCoreRegex = '<package id="Okra.Core" version="[^"]+"'
    $okraCore = '<package id="Okra.Core" version="' + $versionNumber + '"'

    $okraMefRegex = '<package id="Okra.MEF" version="[^"]+"'
    $okraMef = '<package id="Okra.MEF" version="' + $versionNumber + '"'

    (Get-Content $FileName) |
        ForEach-Object {$_ -replace $okraCoreRegex, $okraCore} |
        ForEach-Object {$_ -replace $okraMefRegex, $okraMef} |
        Set-Content $FileName
}

function Update-CsprojReferences
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$True, Position = 1)][string]$FileName,
        [Parameter(Mandatory=$True, Position = 2)][Version]$VersionNumber
    )

    $okraCoreRegex = '<Reference Include="Okra.Core, Version=[^,]+,'
    $okraCore = '<Reference Include="Okra.Core, Version=' + $versionNumber + ','

    $okraCoreHintRegex = '<HintPath>..\\packages\\Okra.Core.[^\\]+\\'
    $okraCoreHint = '<HintPath>..\packages\Okra.Core.' + $versionNumber + '\'

    $okraMefRegex = '<Reference Include="Okra.MEF, Version=[^,]+,'
    $okraMef = '<Reference Include="Okra.MEF, Version=' + $versionNumber + ','

    $okraMefHintRegex = '<HintPath>..\\packages\\Okra.MEF.[^\\]+\\'
    $okraMefHint = '<HintPath>..\packages\Okra.MEF.' + $versionNumber + '\'

    (Get-Content $FileName) |
        ForEach-Object {$_ -replace $okraCoreRegex, $okraCore} |
        ForEach-Object {$_ -replace $okraCoreHintRegex, $okraCoreHint} |
        ForEach-Object {$_ -replace $okraMefRegex, $okraMef} |
        ForEach-Object {$_ -replace $okraMefHintRegex, $okraMefHint} |
        Set-Content $FileName
}

Export-ModuleMember -Function Update-AssemblyInfo
Export-ModuleMember -Function Update-Nuspec
Export-ModuleMember -Function Update-VsixManifest
Export-ModuleMember -Function Update-PackagesConfig
Export-ModuleMember -Function Update-CsprojReferences
