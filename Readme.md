# Git Submodules

[![Build status](https://ci.appveyor.com/api/projects/status/a61sbrynb0krd831?svg=true)](https://ci.appveyor.com/project/Dark-Water/gitsubmodulesvsplugin)
[![Coverage Status](https://coveralls.io/repos/github/Dark-Water/GitSubModulesVsPlugin/badge.svg)](https://coveralls.io/github/Dark-Water/GitSubModulesVsPlugin)

## Info
This Visual Studio extension is for users that need a easier way to handle with Git submodules inside Visual Studio.

## System requirements
* Visual Studio 2010, 2012, 2013, 2015, 2017
* [.NET Framework 4.5](https://www.microsoft.com/de-de/download/details.aspx?id=30653)
* [Git for Windows](https://git-for-windows.github.io/)

## How to
1. Download from [Visual Studio extension site](https://visualstudiogallery.msdn.microsoft.com/0e71baf2-2d0b-44f9-8172-d27df583ad20) or over Visual Studio extension manager ([or direct](https://github.com/Dark-Water/GitSubModulesVsPlugin/tree/master/VSIX%20for%20Testers))
2. Install VSIX 
3. Restart Visual Studio
4. Menu -> View -> Other Windows -> Git Submodules

## For Visual Studio 2010 users
* The tool window is automatic close on Visual Studio 2010 shutdown.
* It's currently not possible to restore the window on startup without a partial crash inside Visual Studio 2010.
* When you know about this problem and you have a solution, please write me.
* Support will be discontinued in 2020, when Microsoft stop support for Visual Studio 2010.

## Whats next?
* [See milestones inside bugtracker](https://github.com/Dark-Water/GitSubModulesVsPlugin/milestones)

## Supported Git functions for each and all submodules
* Fetch
* Update (Force)
* Pull origin master
* Init
* Deinit (Force)

## Shown information
* Git version
* Path to the current open solution
  * Left-click for open in File-Explorer
* Current brach and count of all local branches of the repository
  * Tool-Tip with list of all local branches of the repository
* Submodules of this repository
  * Status of the submodule (HEAD, Current, Error, Unkown)
  * Name of the Submodule
    * Left-click for open in File-Explorer
  * Id (SHA-1) of the submodule
    * Full version on expaned info, otherwise short version 
    * Mark and use CTRL+C to copy to clipboard
  * Current branch and count of branches of this submodule
    * Tool-Tip with list of all local branches of the repository
  * Last Tag of the submodule
    * Mark and use CTRL+C to copy to clipboard
  * Status of the submodule

## Right-click options for a submodule
* Open in File-Explorer
* -
* Update (Force)
* Pull origin master
* -
* Init
* Deinit (Force)
* -
* Copy complete id to clipboard
* Copy short id to clipboard
* Copy complete tag to clipboard
* -
* Copy branch name to clipboard
* Copy branch list to clipboard
 
## Others
* Show all informations and all functions only for the current entered submodule
  * All other submodules show only name, short id and buttons for update and pull orgin master
* Supported any used Visual Studio theme
* Automatic fetch submodule status from server on first open of a soultion
* Debug and error messages will be written on a separate output window
* The output window is automatical actiavte on the first use and when a error occures

## Pictures
On dark theme
![picture](picture1.png)

On blue theme
![picture](picture2.png)

On light theme
![picture](picture3.png)

## Submodule status colours
| Submodule status               | Colour           |
| ------------------------------ | ---------------- |
| Unknown Status (Please Report) | LightGray        |
| Submodule is not initialized   | LightCoral (Red) |
| Submodule is initialized       | Yellow           |
| Submodule has merge conflicts  | DarkOrange       |
| Submodule is current           | YellowGreen      |
| Submodule is not current       | LightSkyBlue     |

## Test matrix
| Visual Studio | Community / Express        | Professional   | Premium        | Ultimate / Enterprise | Support dropped |
|-------------- | -------------------------- | -------------- | -------------- | --------------------- | --------------- |
| 2010          | -                          | Should be work | Should be work | **tested**            | 2020            |
| 2012          | -                          | **tested**     | Should be work | Should be work        | 2022            |
| 2013          | -                          | **tested**     | Should be work | **tested**            | 2023            |
| 2015          | **tested**                 | **tested**     | -              | Should be work        | 2025            |
| 2017          | *testing*                  | Should be work | -              | Should be work        | -               |

## Copyrights and Copylefts
* Indicator Icons
 * Found: Via IconFinder 
 * Licence: Free for commercial use
 * Autor: Andy Gogena
 * Website: http://www.graphicrating.com/
 * Changes: Reduced image dimensions
* Git Logo 
 * Found: On Git website -> Orange logomark for light backgrounds
 * Licence: [Creative Commons Attribution 3.0 Unported License](https://creativecommons.org/licenses/by/3.0/)
 * Auto: Jason Long
 * Website: https://git-scm.com/downloads/logos
 * Changes: Reduced image dimensions
