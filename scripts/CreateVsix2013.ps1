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

# Create the folder structure to construct the VSIX contents

New-Item .\artifacts -type directory -Force | Out-Null
New-Item .\artifacts\OkraAppFrameworkVsix2013 -type directory -Force | Out-Null
Remove-Item .\artifacts\OkraAppFrameworkVsix2013\* -Recurse -Force

# Copy the base VSIX files

Copy-Item .\templates-vs2013\base\* .\artifacts\OkraAppFrameworkVsix2013\ -Recurse

# Create the individual project templates

Write-Zip @(".\templates-vs2013\ProjectTemplates\CSharp\WindowsApps\OkraBlankApp\*") ".\artifacts\OkraAppFrameworkVsix2013\ProjectTemplates\CSharp\OkraAppFramework\WindowsApps\OkraBlankApp.zip"
Write-Zip @(".\templates-vs2013\ProjectTemplates\CSharp\WindowsApps\OkraGridApp\*", ".\templates-vs2013\ProjectTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ProjectTemplates\CSharp\OkraAppFramework\WindowsApps\OkraGridApp.zip"
Write-Zip @(".\templates-vs2013\ProjectTemplates\CSharp\WindowsApps\OkraHubApp\*", ".\templates-vs2013\ProjectTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ProjectTemplates\CSharp\OkraAppFramework\WindowsApps\OkraHubApp.zip"
Write-Zip @(".\templates-vs2013\ProjectTemplates\CSharp\WindowsApps\OkraSplitApp\*", ".\templates-vs2013\ProjectTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ProjectTemplates\CSharp\OkraAppFramework\WindowsApps\OkraSplitApp.zip"

# Create the individual item templates

Write-Zip @(".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\OkraBasicPage\*", ".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ItemTemplates\CSharp\OkraAppFramework\OkraBasicPage.zip"
# Write-Zip @(".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\OkraFileOpenPickerContract\*", ".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ItemTemplates\CSharp\OkraAppFramework\OkraFileOpenPickerContract.zip"
Write-Zip @(".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\OkraGroupDetailPage\*", ".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ItemTemplates\CSharp\OkraAppFramework\OkraGroupDetailPage.zip"
Write-Zip @(".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\OkraGroupedItemsPage\*", ".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ItemTemplates\CSharp\OkraAppFramework\OkraGroupedItemsPage.zip"
Write-Zip @(".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\OkraHubPage\*", ".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ItemTemplates\CSharp\OkraAppFramework\OkraHubPage.zip"
Write-Zip @(".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\OkraItemDetailPage\*", ".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ItemTemplates\CSharp\OkraAppFramework\OkraItemDetailPage.zip"
Write-Zip @(".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\OkraItemsPage\*", ".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ItemTemplates\CSharp\OkraAppFramework\OkraItemsPage.zip"
Write-Zip @(".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\OkraSearchContract\*", ".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ItemTemplates\CSharp\OkraAppFramework\OkraSearchContract.zip"
Write-Zip @(".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\OkraSettingsFlyout\*", ".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ItemTemplates\CSharp\OkraAppFramework\OkraSettingsFlyout.zip"
Write-Zip @(".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\OkraShareTargetContract\*", ".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ItemTemplates\CSharp\OkraAppFramework\OkraShareTargetContract.zip"
Write-Zip @(".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\OkraSplitPage\*", ".\templates-vs2013\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix2013\ItemTemplates\CSharp\OkraAppFramework\OkraSplitPage.zip"

# Create the VSIX file

Write-Zip .\artifacts\OkraAppFrameworkVsix2013\* .\artifacts\OkraAppFramework-vs2013.vsix

# Delete the temporary folder

Remove-Item .\artifacts\OkraAppFrameworkVsix2013 -Recurse -Force