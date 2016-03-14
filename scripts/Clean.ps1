# Move to the project root folder (parent from current script folder)

function Get-ScriptDirectory
{ 
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value 
    Split-Path $Invocation.MyCommand.Path 
}

$rootFolder = (Get-Item (Get-ScriptDirectory)).Parent.FullName
Set-Location $rootFolder

# Functions

function Remove-IfExists
{
    Param([string]$filePath)

    If (Test-Path $filePath)
    {
        Remove-Item $filePath -Recurse
    }
}

# Remove the artifacts folder

Remove-IfExists .\artifacts

# Remove any build artifacts

Remove-IfExists .\src\Okra.Core\Okra.PortableCore\bin
Remove-IfExists .\src\Okra.Core\Okra.PortableCore\obj
Remove-IfExists .\src\Okra.Core\Okra.Core.Universal\bin
Remove-IfExists .\src\Okra.Core\Okra.Core.Universal\obj

Remove-IfExists .\src\Okra.MEF\Okra.MEF.PortableCore\bin
Remove-IfExists .\src\Okra.MEF\Okra.MEF.PortableCore\obj

Remove-IfExists .\test\Okra.Core.Tests\Okra.PortableCore.Tests\bin
Remove-IfExists .\test\Okra.Core.Tests\Okra.PortableCore.Tests\obj
Remove-IfExists .\test\Okra.Core.Tests\Okra.Core.Universal.Tests\bin
Remove-IfExists .\test\Okra.Core.Tests\Okra.Core.Universal.Tests\obj

Remove-IfExists .\test\Okra.MEF.Tests\Okra.MEF.Tests.PortableCore\bin
Remove-IfExists .\test\Okra.MEF.Tests\Okra.MEF.Tests.PortableCore\obj