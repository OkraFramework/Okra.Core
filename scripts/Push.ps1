param([Parameter(Mandatory=$true)][string]$Version) 

cd ..\.nuget
.\NuGet.exe push ..\artifacts\Okra.Core.$Version.nupkg
.\NuGet.exe push ..\artifacts\Okra.MEF.$Version.nupkg

$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")