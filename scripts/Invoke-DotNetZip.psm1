function Install-DotNetZip
{
    Install-NuGet
    Install-NuGetPackage DotNetZip 1.9.3 .\packages
}

function Write-Zip
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$True, Position = 1)][string[]]$Sourcefiles,
        [Parameter(Mandatory=$True, Position = 2)][string]$ZipPath
    )

    $item = New-Item .\artifacts\ziptemp -type directory
    Copy-item $Sourcefiles .\artifacts\ziptemp -Recurse

    $zipFullPath = Join-Path (pwd) $ZipPath

    Add-Type -Path ".\packages\DotNetZip.1.9.3\lib\net20\Ionic.zip.dll"

    $zip = new-object Ionic.Zip.ZipFile
    $zip.AddDirectory($item.FullName)
    $zip.Save($zipFullPath)
    $zip.Dispose()

    Remove-Item .\artifacts\ziptemp -Recurse -Force
    return $ZipPath
}