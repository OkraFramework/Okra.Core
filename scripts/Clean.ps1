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

Remove-IfExists .\src\Okra.Core\Okra.Core.Windows\bin
Remove-IfExists .\src\Okra.Core\Okra.Core.Windows\obj
Remove-IfExists .\src\Okra.Core\Okra.Core.WindowsPhone\bin
Remove-IfExists .\src\Okra.Core\Okra.Core.WindowsPhone\obj

Remove-IfExists .\src\Okra.MEF\Okra.MEF.Windows\bin
Remove-IfExists .\src\Okra.MEF\Okra.MEF.Windows\obj
Remove-IfExists .\src\Okra.MEF\Okra.MEF.WindowsPhone\bin
Remove-IfExists .\src\Okra.MEF\Okra.MEF.WindowsPhone\obj

Remove-IfExists .\test\Okra.Core.Tests\Okra.Core.Tests.Windows\bin
Remove-IfExists .\test\Okra.Core.Tests\Okra.Core.Tests.Windows\obj
Remove-IfExists .\test\Okra.Core.Tests\Okra.Core.Tests.WindowsPhone\bin
Remove-IfExists .\test\Okra.Core.Tests\Okra.Core.Tests.WindowsPhone\obj

Remove-IfExists .\test\Okra.MEF.Tests\Okra.MEF.Tests.Windows\bin
Remove-IfExists .\test\Okra.MEF.Tests\Okra.MEF.Tests.Windows\obj
Remove-IfExists .\test\Okra.MEF.Tests\Okra.MEF.Tests.WindowsPhone\bin
Remove-IfExists .\test\Okra.MEF.Tests\Okra.MEF.Tests.WindowsPhone\obj