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
New-Item .\artifacts\OkraAppFrameworkVsix2015 -type directory -Force | Out-Null
Remove-Item .\artifacts\OkraAppFrameworkVsix2015\* -Recurse -Force

# Copy the base VSIX files

Copy-Item .\templates-vs2015\base\* .\artifacts\OkraAppFrameworkVsix2015\ -Recurse

# Create the individual project templates

Write-Zip @(".\templates-vs2015\ProjectTemplates\CSharp\Windows8\OkraBlankApp\*") ".\artifacts\OkraAppFrameworkVsix2015\ProjectTemplates\CSharp\OkraAppFramework\Windows8\OkraBlankApp.zip"
Write-Zip @(".\templates-vs2015\ProjectTemplates\CSharp\Windows8\OkraGridApp\*", ".\templates-vs2015\ProjectTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ProjectTemplates\CSharp\OkraAppFramework\Windows8\OkraGridApp.zip"
Write-Zip @(".\templates-vs2015\ProjectTemplates\CSharp\Windows8\OkraHubApp\*", ".\templates-vs2015\ProjectTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ProjectTemplates\CSharp\OkraAppFramework\Windows8\OkraHubApp.zip"
Write-Zip @(".\templates-vs2015\ProjectTemplates\CSharp\Windows8\OkraSplitApp\*", ".\templates-vs2015\ProjectTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ProjectTemplates\CSharp\OkraAppFramework\Windows8\OkraSplitApp.zip"

Write-Zip @(".\templates-vs2015\ProjectTemplates\CSharp\Universal\OkraBlankApplication\*") ".\artifacts\OkraAppFrameworkVsix2015\ProjectTemplates\CSharp\OkraAppFramework\Universal\OkraBlankApplication.zip"

# Create the individual item templates

Write-Zip @(".\templates-vs2015\ItemTemplates\CSharp\Windows8\OkraBasicPage\*", ".\templates-vs2015\ItemTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ItemTemplates\CSharp\OkraAppFramework\OkraBasicPage.zip"
# Write-Zip @(".\templates-vs2015\ItemTemplates\CSharp\Windows8\OkraFileOpenPickerContract\*", ".\templates-vs2015\ItemTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ItemTemplates\CSharp\OkraAppFramework\OkraFileOpenPickerContract.zip"
Write-Zip @(".\templates-vs2015\ItemTemplates\CSharp\Windows8\OkraGroupDetailPage\*", ".\templates-vs2015\ItemTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ItemTemplates\CSharp\OkraAppFramework\OkraGroupDetailPage.zip"
Write-Zip @(".\templates-vs2015\ItemTemplates\CSharp\Windows8\OkraGroupedItemsPage\*", ".\templates-vs2015\ItemTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ItemTemplates\CSharp\OkraAppFramework\OkraGroupedItemsPage.zip"
Write-Zip @(".\templates-vs2015\ItemTemplates\CSharp\Windows8\OkraHubPage\*", ".\templates-vs2015\ItemTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ItemTemplates\CSharp\OkraAppFramework\OkraHubPage.zip"
Write-Zip @(".\templates-vs2015\ItemTemplates\CSharp\Windows8\OkraItemDetailPage\*", ".\templates-vs2015\ItemTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ItemTemplates\CSharp\OkraAppFramework\OkraItemDetailPage.zip"
Write-Zip @(".\templates-vs2015\ItemTemplates\CSharp\Windows8\OkraItemsPage\*", ".\templates-vs2015\ItemTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ItemTemplates\CSharp\OkraAppFramework\OkraItemsPage.zip"
Write-Zip @(".\templates-vs2015\ItemTemplates\CSharp\Windows8\OkraSearchContract\*", ".\templates-vs2015\ItemTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ItemTemplates\CSharp\OkraAppFramework\OkraSearchContract.zip"
Write-Zip @(".\templates-vs2015\ItemTemplates\CSharp\Windows8\OkraSettingsFlyout\*", ".\templates-vs2015\ItemTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ItemTemplates\CSharp\OkraAppFramework\OkraSettingsFlyout.zip"
Write-Zip @(".\templates-vs2015\ItemTemplates\CSharp\Windows8\OkraShareTargetContract\*", ".\templates-vs2015\ItemTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ItemTemplates\CSharp\OkraAppFramework\OkraShareTargetContract.zip"
Write-Zip @(".\templates-vs2015\ItemTemplates\CSharp\Windows8\OkraSplitPage\*", ".\templates-vs2015\ItemTemplates\CSharp\Windows8\shared\*") ".\artifacts\OkraAppFrameworkVsix2015\ItemTemplates\CSharp\OkraAppFramework\OkraSplitPage.zip"

# Create the VSIX file

Write-Zip .\artifacts\OkraAppFrameworkVsix2015\* .\artifacts\OkraAppFramework-vs2015.vsix

# Delete the temporary folder

Remove-Item .\artifacts\OkraAppFrameworkVsix2015 -Recurse -Force