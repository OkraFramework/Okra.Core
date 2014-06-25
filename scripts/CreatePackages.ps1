# Move to the project root folder (parent from current script folder)

function Get-ScriptDirectory
{ 
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value 
    Split-Path $Invocation.MyCommand.Path 
}

$rootFolder = (Get-Item (Get-ScriptDirectory)).Parent.FullName
Set-Location $rootFolder

# Create the artifacts directory if it doesn't exist

New-Item .\artifacts -type directory -Force | Out-Null

# Check NuGet is installed and updated

If (!(Test-Path .\.nuget\nuget.exe))
{
    New-Item .\.nuget -type directory -Force | Out-Null
    Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '.\.nuget\nuget.exe'
}

.\.nuget\NuGet.exe update -self

# Create packages

.\.nuget\NuGet.exe pack .\src\Okra.Core\Okra.Core.nuspec -Prop Configuration=Release -Output .\artifacts -Symbols
.\.nuget\NuGet.exe pack .\src\Okra.MEF\Okra.MEF.nuspec -Prop Configuration=Release -Output .\artifacts -Symbols
.\.nuget\NuGet.exe pack .\src\Okra.Core\OkraUniversalPreview.Core.nuspec -Prop Configuration=Release -Output .\artifacts -Symbols
.\.nuget\NuGet.exe pack .\src\Okra.MEF\OkraUniversalPreview.MEF.nuspec -Prop Configuration=Release -Output .\artifacts -Symbols