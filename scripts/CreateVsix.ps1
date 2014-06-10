# --- Functions ---

function Write-Zip( $sourcefiles, $zipfilename )
{
    $item = New-Item ..\artifacts\ziptemp -type directory
    Copy-item $sourcefiles ..\artifacts\ziptemp -Recurse

    $assembly = [Reflection.Assembly]::LoadWithPartialName( "System.IO.Compression.FileSystem" )
    $compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
    [System.IO.Compression.ZipFile]::CreateFromDirectory( "..\artifacts\ziptemp", $zipfilename, $compressionLevel, $false)

    Remove-Item ..\artifacts\ziptemp -Recurse -Force
    return $zipfilename
}

# --- Script ---

# Create the folder structure to construct the VSIX contents

New-Item ..\artifacts -type directory -Force
New-Item ..\artifacts\OkraAppFrameworkVsix -type directory -Force
Remove-Item ..\artifacts\OkraAppFrameworkVsix\* -Recurse -Force

# Copy the base VSIX files

Copy-Item ..\templates\base\* ..\artifacts\OkraAppFrameworkVsix\ -Recurse

# Create the individual project templates

Write-Zip ..\templates\ProjectTemplates\CSharp\OkraBasicApp\* "..\artifacts\OkraAppFrameworkVsix\ProjectTemplates\CSharp\OkraAppFramework\OkraBasicApp.zip"
Write-Zip ..\templates\ProjectTemplates\CSharp\OkraGridApp\* "..\artifacts\OkraAppFrameworkVsix\ProjectTemplates\CSharp\OkraAppFramework\OkraGridApp.zip"
Write-Zip ..\templates\ProjectTemplates\CSharp\OkraSplitApp\* "..\artifacts\OkraAppFrameworkVsix\ProjectTemplates\CSharp\OkraAppFramework\OkraSplitApp.zip"

# Create the individual item templates

Write-Zip @("..\templates\ItemTemplates\CSharp\OkraBasicPage\*", "..\templates\ItemTemplates\CSharp\common\*") "..\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraBasicPage.zip"
Write-Zip @("..\templates\ItemTemplates\CSharp\OkraSplitPage\*", "..\templates\ItemTemplates\CSharp\common\*") "..\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraSplitPage.zip"
Write-Zip @("..\templates\ItemTemplates\CSharp\OkraItemsPage\*", "..\templates\ItemTemplates\CSharp\common\*") "..\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraItemsPage.zip"
Write-Zip @("..\templates\ItemTemplates\CSharp\OkraItemDetailPage\*", "..\templates\ItemTemplates\CSharp\common\*") "..\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraItemDetailPage.zip"
Write-Zip @("..\templates\ItemTemplates\CSharp\OkraGroupedItemsPage\*", "..\templates\ItemTemplates\CSharp\common\*") "..\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraGroupedItemsPage.zip"
Write-Zip @("..\templates\ItemTemplates\CSharp\OkraGroupDetailPage\*", "..\templates\ItemTemplates\CSharp\common\*") "..\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraGroupDetailPage.zip"
Write-Zip @("..\templates\ItemTemplates\CSharp\OkraShareTargetContract\*", "..\templates\ItemTemplates\CSharp\common\*") "..\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraShareTargetContract.zip"
Write-Zip @("..\templates\ItemTemplates\CSharp\OkraSettingsPane\*", "..\templates\ItemTemplates\CSharp\common\*") "..\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraSettingsPane.zip"
Write-Zip @("..\templates\ItemTemplates\CSharp\OkraSearchContract\*", "..\templates\ItemTemplates\CSharp\common\*") "..\artifacts\OkraAppFrameworkVsix\ItemTemplates\CSharp\OkraAppFramework\OkraSearchContract.zip"

# Create the VSIX file
# NB: For some reason the VSIX package reader doesn't like zip files created like this - use the Windows "Send to compressed folder" option instead
# Write-Zip ..\artifacts\OkraAppFrameworkVsix\* ..\artifacts\OkraAppFramework.vsix

Echo ""
Echo "Package Folder Created - Further steps required"
Echo "  1. Compress the folder into a Zip file"
Echo "  2. Rename the file with the .vsix extension"

$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")