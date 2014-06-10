New-Item ..\artifacts -type directory -Force

&"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "..\Okra.Core\Okra.Core.csproj" "/verbosity:minimal" "/property:Configuration=Release"
&"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" "..\Okra.MEF\Okra.MEF.csproj" "/verbosity:minimal" "/property:Configuration=Release"

..\.nuget\NuGet.exe update -self
..\.nuget\NuGet.exe pack ..\Okra.Core\Okra.Core.csproj -Prop Configuration=Release -Output ..\artifacts -Symbols
..\.nuget\NuGet.exe pack ..\Okra.MEF\Okra.MEF.csproj -Prop Configuration=Release -Output ..\artifacts -Symbols

$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")