#Git Submodules

##Info
This Visual Studio plugin is for users that need a easier way to handle with Git submodules inside Visual Studio.

##How to
1. Grab from [Visual Studio extension site](https://visualstudiogallery.msdn.microsoft.com/0e71baf2-2d0b-44f9-8172-d27df583ad20) or over Visual Studio extension manager ([direct from this repository](https://github.com/Dark-Water/GitSubModulesVsPlugin/tree/master/VSIX%20for%20Testers)
2. Install VSIX 
3. Restart Visual Studio
4. Menu -> View -> Other Windows -> Git Submodules

##Supported Git functions for each and all submodules
* Status
* Init
* Deinit (Force)
* Update (Force)
* Pull origin master

##Shown information
* Git version
* Path to the current open solution (clickable -> Open in File-Explorer)
* Submodules of this repository
 * Status of the last action
 * Name of the Submodule (clickable -> Open in File-Explorer)
 * Id (SHA-1) of the submodule
 * Last commit Id (SHA-1 or Tag) of the submodule
 * Status of the submodule

##Right-click options for a submodule
* Open in File-Explorer
* Init
* Deinit (Force)
* Update (Force)
* Pull origin master
 
##Others
* Supported any used Visual Studio theme
* Debug and error messages will be written on a separate output window
* The output window is automatical actiavte on the first use and when a error occures

##Pictures
On blue theme
![picture](picture2.png)

On dark theme
![picture](picture3.png)

##System requirements
* Visual Studio 2013, 2015
* [.NET Framework 4.5](https://www.microsoft.com/de-de/download/details.aspx?id=30653)
* [Git for Windows](https://git-for-windows.github.io/)

##Tested with
* Visual Studio 2013 Professional, Ultimate (Update 5)
* Visual Studio 2015 Community, Professional (Update 3)

##Copyrights and Copylefts
* Indicator Icons
 * Found: Via IconFinder 
 * Licence: Free for commercial use
 * Autor: Andy Gogena
 * Website: http://www.graphicrating.com/
 * Changes: none
* Git Logo 
 * Found: On Git website -> Orange logomark for light backgrounds
 * Licence: [Creative Commons Attribution 3.0 Unported License](https://creativecommons.org/licenses/by/3.0/)
 * Auto: Jason Long
 * Website: https://git-scm.com/downloads/logos
 * Changes: none

##Whats next?
* [See milestones inside bugtracker](https://github.com/Dark-Water/GitSubModulesVsPlugin/milestones)

