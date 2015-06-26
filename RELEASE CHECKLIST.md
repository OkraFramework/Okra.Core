# Okra.Core Release Checklist

* Pre-release checks
  * Run https://github.com/dotnet/codeformatter to fix any deviations from dotNet coding guidelines
  * Run the Coverity Scan static analysis tool to check for defects (https://scan.coverity.com)
  * Check that the Okra.Core.Portable API matches the platform specific ones
* Rebuild all artifacts using the 'build.ps1' script
  * NB: Pass the new version number as the first parameter
* Release on GitHub
  * Commit and push the new version numbers to GitHub
  * Tag the associated commit in git with the release number
  * Associate the tag with a new release in GitHub
* Upload artifacts
  * Upload the NuGet packages to NuGet.org using the 'scripts\Push.ps1' script
  * Upload the Visual Studio extension to the Visual Studio Gallery
* Update AppVeyor to the next version for prerelease package publishing
* Update Okra-Samples to use the latest release of the framework
* Update Okra-Todo to user the latest release of the framework
* Write blog post announcing the release
