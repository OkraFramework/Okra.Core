# Okra.Core Release Checklist

* Rebuild all artifacts using the 'build.ps1' script (passing the new version number as the first parameter)
* Commit and push the new version numbers to GitHub
* Tag the associated commit in git and create a new release in GitHub
* Upload the NuGet packages to NuGet.org using the 'scripts\Push.ps1' script
* Upload the Visual Studio extension to the Visual Studio Gallery
* Update AppVeyor to the next version for prerelease package publishing