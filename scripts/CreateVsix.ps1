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

# Create the folder structure to construct the VSIX contents

New-Item .\artifacts -type directory -Force | Out-Null
New-Item .\artifacts\OkraAppFrameworkVsix -type directory -Force | Out-Null
Remove-Item .\artifacts\OkraAppFrameworkVsix\* -Recurse -Force

# Copy the base VSIX files

Copy-Item .\templates\base\* .\artifacts\OkraAppFrameworkVsix\ -Recurse

# Create the individual project templates

Write-Zip @(".\templates\ProjectTemplates\CSharp\WindowsApps\OkraBlankApp\*") ".\artifacts\OkraAppFrameworkVsix\ProjectTemplates\CSharp\OkraAppFramework\WindowsApps\OkraBlankApp.zip"
Write-Zip @(".\templates\ProjectTemplates\CSharp\WindowsApps\OkraGridApp\*", ".\templates\ProjectTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ProjectTemplates\CSharp\OkraAppFramework\WindowsApps\OkraGridApp.zip"
Write-Zip @(".\templates\ProjectTemplates\CSharp\WindowsApps\OkraHubApp\*", ".\templates\ProjectTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ProjectTemplates\CSharp\OkraAppFramework\WindowsApps\OkraHubApp.zip"
Write-Zip @(".\templates\ProjectTemplates\CSharp\WindowsApps\OkraSplitApp\*", ".\templates\ProjectTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ProjectTemplates\CSharp\OkraAppFramework\WindowsApps\OkraSplitApp.zip"

# Create the individual item templates

#Write-Zip @(".\templates\ItemTemplates\CSharp\OkraBasicPage\*", ".\templates\ItemTemplates\CSharp\common\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraBasicPage.zip"
#Write-Zip @(".\templates\ItemTemplates\CSharp\OkraSplitPage\*", ".\templates\ItemTemplates\CSharp\common\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraSplitPage.zip"
#Write-Zip @(".\templates\ItemTemplates\CSharp\OkraItemsPage\*", ".\templates\ItemTemplates\CSharp\common\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraItemsPage.zip"
#Write-Zip @(".\templates\ItemTemplates\CSharp\OkraItemDetailPage\*", ".\templates\ItemTemplates\CSharp\common\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraItemDetailPage.zip"
#Write-Zip @(".\templates\ItemTemplates\CSharp\OkraGroupedItemsPage\*", ".\templates\ItemTemplates\CSharp\common\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraGroupedItemsPage.zip"
#Write-Zip @(".\templates\ItemTemplates\CSharp\OkraGroupDetailPage\*", ".\templates\ItemTemplates\CSharp\common\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraGroupDetailPage.zip"
#Write-Zip @(".\templates\ItemTemplates\CSharp\OkraShareTargetContract\*", ".\templates\ItemTemplates\CSharp\common\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraShareTargetContract.zip"
#Write-Zip @(".\templates\ItemTemplates\CSharp\OkraSettingsPane\*", ".\templates\ItemTemplates\CSharp\common\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraSettingsPane.zip"
#Write-Zip @(".\templates\ItemTemplates\CSharp\OkraSearchContract\*", ".\templates\ItemTemplates\CSharp\common\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraSearchContract.zip"

# Create the VSIX file

Write-Zip .\artifacts\OkraAppFrameworkVsix\* .\artifacts\OkraAppFramework.vsix

# Delete the temporary folder

Remove-Item .\artifacts\OkraAppFrameworkVsix -Recurse -Force