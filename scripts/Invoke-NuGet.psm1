function Install-NuGet
{
    If (!(Test-Path .\.nuget\nuget.exe))
    {
        New-Item .\.nuget -type directory -Force | Out-Null
        Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '.\.nuget\nuget.exe'
    }

    .\.nuget\NuGet.exe update -self
}

function Add-NuGetPackage
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$True, Position = 1)][string]$NuspecFileName,
        [Parameter(Mandatory=$True, Position = 2)][string]$OutputDirectory
    )

    .\.nuget\NuGet.exe pack $NuspecFileName -Prop Configuration=Release -Output $OutputDirectory -Symbols
}