[CmdletBinding()] 
Param 
(
    [Parameter(Mandatory=$False, Position=1)]$Target,
    [Parameter(Mandatory=$False)][string]$VersionSuffix
)

# Move to the project root folder (current script folder)
 
function Get-ScriptDirectory 
{  
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value  
    Split-Path $Invocation.MyCommand.Path  
}

$rootFolder = (Get-Item (Get-ScriptDirectory)).FullName 
Set-Location $rootFolder

# Clone the Okra Build system (or use local copy if in parent folder)

If (Test-Path "./.build")
{
    Remove-Item "./.build" -Recurse -Force
}

If (Test-Path "../Okra-Build")
{
    Copy-Item "../Okra-Build" "./.build" -Recurse
}
Else
{
    git clone https://github.com/OkraFramework/Okra-Build.git .build
}

# Set environment variables

$env:OKRA_BUILD_VERSIONSUFFIX = $VersionSuffix

# Run the build script

.\.build\build.ps1 $Target