# Move to the project root folder (current script folder)
 
function Get-ScriptDirectory 
{  
    $Invocation = (Get-Variable MyInvocation -Scope 1).Value  
    Split-Path $Invocation.MyCommand.Path  
}

$rootFolder = (Get-Item (Get-ScriptDirectory)).FullName 
Set-Location $rootFolder

# Clone the Okra Build system (or use local copy if in parent folder)

If (Test-Path "../Okra-Build")
{
    $okraBuildTools = "../Okra-Build"
}
Else
{
    If (Test-Path "./.build")
    {
        Remove-Item "./.build" -Recurse -Force
    }

    git clone https://github.com/OkraFramework/Okra-Build.git .build
    $okraBuildTools = "./.build"
}

# Run the build script

$buildScript = Join-Path $okraBuildTools "build.ps1"
Invoke-Expression "& `"$buildScript`" $Args"
exit $LASTEXITCODE