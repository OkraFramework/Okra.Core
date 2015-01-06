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

Write-Zip @(".\templates\ItemTemplates\CSharp\WindowsApps\OkraBasicPage\*", ".\templates\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraBasicPage.zip"
# Write-Zip @(".\templates\ItemTemplates\CSharp\WindowsApps\OkraFileOpenPickerContract\*", ".\templates\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraFileOpenPickerContract.zip"
Write-Zip @(".\templates\ItemTemplates\CSharp\WindowsApps\OkraGroupDetailPage\*", ".\templates\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraGroupDetailPage.zip"
Write-Zip @(".\templates\ItemTemplates\CSharp\WindowsApps\OkraGroupedItemsPage\*", ".\templates\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraGroupedItemsPage.zip"
Write-Zip @(".\templates\ItemTemplates\CSharp\WindowsApps\OkraHubPage\*", ".\templates\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraHubPage.zip"
Write-Zip @(".\templates\ItemTemplates\CSharp\WindowsApps\OkraItemDetailPage\*", ".\templates\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraItemDetailPage.zip"
Write-Zip @(".\templates\ItemTemplates\CSharp\WindowsApps\OkraItemsPage\*", ".\templates\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraItemsPage.zip"
Write-Zip @(".\templates\ItemTemplates\CSharp\WindowsApps\OkraSearchContract\*", ".\templates\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraSearchContract.zip"
Write-Zip @(".\templates\ItemTemplates\CSharp\WindowsApps\OkraSettingsFlyout\*", ".\templates\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraSettingsFlyout.zip"
Write-Zip @(".\templates\ItemTemplates\CSharp\WindowsApps\OkraShareTargetContract\*", ".\templates\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraShareTargetContract.zip"
Write-Zip @(".\templates\ItemTemplates\CSharp\WindowsApps\OkraSplitPage\*", ".\templates\ItemTemplates\CSharp\WindowsApps\shared\*") ".\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraSplitPage.zip"

# Create the VSIX file

Write-Zip .\artifacts\OkraAppFrameworkVsix\* .\artifacts\OkraAppFramework.vsix

# Delete the temporary folder

Remove-Item .\artifacts\OkraAppFrameworkVsix -Recurse -Force