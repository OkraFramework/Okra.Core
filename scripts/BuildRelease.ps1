[CmdletBinding()]
Param
(
    [switch]$noTests
)

# Move to the project root folder (parent from current script folder)

function Get-ScriptDirectory
{ 
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value 
    Split-Path $Invocation.MyCommand.Path 
}

$rootFolder = (Get-Item (Get-ScriptDirectory)).Parent.FullName
Set-Location $rootFolder

# Import modules

Import-Module -Name ".\scripts\Invoke-MsBuild.psm1"
Import-Module -Name ".\scripts\Invoke-NuGet.psm1"

# Functions

function Invoke-MsBuildAndThrow
{
    Param([string]$Path)

    write-host "MSBuild" $Path

    $buildResult = Invoke-MsBuild -Path $Path -Params "/verbosity:minimal /property:Configuration=Release;VisualStudioVersion=14.0" -AutoOutput

    if (!$buildResult)
    {
        throw("Building " + $Path + " failed")
    }
}

# Check NuGet is installed and restore packages

Install-NuGet
Restore-NuGetPackages ".\Okra.Core.sln"
Restore-NuGetPackages ".\Okra.MEF.sln"

# Perform builds

Invoke-MsBuildAndThrow ".\src\Okra.Core\Okra.Core.Windows\Okra.Core.Windows.csproj"
Invoke-MsBuildAndThrow ".\src\Okra.Core\Okra.Core.WindowsPhone\Okra.Core.WindowsPhone.csproj"
# Invoke-MsBuildAndThrow ".\src\Okra.Core\Okra.Core.Universal\Okra.Core.Universal.csproj"
Invoke-MsBuildAndThrow ".\src\Okra.Core\Okra.Core.Portable\Okra.Core.Portable.csproj"
Invoke-MsBuildAndThrow ".\src\Okra.MEF\Okra.MEF.Windows\Okra.MEF.Windows.csproj"
Invoke-MsBuildAndThrow ".\src\Okra.MEF\Okra.MEF.WindowsPhone\Okra.MEF.WindowsPhone.csproj"
# Invoke-MsBuildAndThrow ".\src\Okra.MEF\Okra.MEF.Universal\Okra.MEF.Universal.csproj"
Invoke-MsBuildAndThrow ".\src\Okra.MEF\Okra.MEF.Portable\Okra.MEF.Portable.csproj"

if (!$noTests)
{
	Invoke-MsBuildAndThrow ".\test\Okra.Core.Tests\Okra.Core.Tests.Windows\Okra.Core.Tests.Windows.csproj"
	Invoke-MsBuildAndThrow ".\test\Okra.Core.Tests\Okra.Core.Tests.WindowsPhone\Okra.Core.Tests.WindowsPhone.csproj"
	# Invoke-MsBuildAndThrow ".\test\Okra.Core.Tests\Okra.Core.Tests.Universal\Okra.Core.Tests.Universal.csproj"
	Invoke-MsBuildAndThrow ".\test\Okra.Core.Tests\Okra.Core.Tests.WindowsPhone\Okra.Core.Tests.WindowsPhone.csproj"
	Invoke-MsBuildAndThrow ".\test\Okra.MEF.Tests\Okra.MEF.Tests.Windows\Okra.MEF.Tests.Windows.csproj"
	Invoke-MsBuildAndThrow ".\test\Okra.MEF.Tests\Okra.MEF.Tests.WindowsPhone\Okra.MEF.Tests.WindowsPhone.csproj"
	# Invoke-MsBuildAndThrow ".\test\Okra.MEF.Tests\Okra.MEF.Tests.Universal\Okra.MEF.Tests.Universal.csproj"
}