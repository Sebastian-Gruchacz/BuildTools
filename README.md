# BuildTools
Building tools, scripts and cmdlets

Rough description: I just need a very specific flow for my Console project. I want to write it simply, with project references only, but I need to build a cascade of Nuget packages from this.
Therefore flow is as following:

0. Increase build version?
1. Download recent code from Github into separate folder. Specify branch and folder. -> Will use existing tools with PS or create cmd.
2. Locate / download nuget.exe -> use PS script directly.
3. Locate msbuild -> create PS cmdlet for it
4. Load solution, list projects -> PS cmdlet; need to sort with proper build order, to build prerequisities first
5. Convert each project to reference nuegts instead of projects -> CMD
6. Build and publish project after project -> PS script
  a. Upgrade each project with newest nuget versions -> PS script
  b. Build project (2 ways, depending of project type)
  c. Build package (2+ ways, depending of project type and how nuget is configured)
  d. Use marked commits from Github as release descriptors.
  e. Publish package
