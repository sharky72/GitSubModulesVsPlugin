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
- Git version
- Path to the current open solution
- Submodules of this repository
 - Name of the Submodule
 - Id (SHA-1) of the submodule
 - Last commit Id (SHA-1 or Tag) of the submodule
 - Status of the submodule

##Right-click options for a submodule
- Open in File-Explorer
- Init
- Deinit (Force)
- Update (Force)
- Pull origin master
 
##Other features
- Debug and error messages will be written on a separate output window
- The output window is automatical actiavte on the first use and when a error occures

##Pictures
![picture](picture2.png)

##System requirements
* Visual Studio 2013, 2015
* [.NET Framework 4.5](https://www.microsoft.com/de-de/download/details.aspx?id=30653)
* [Git for Windows](https://git-for-windows.github.io/)

##Tested with
- Visual Studio 2013 Professional, Ultimate (Update 5)
- Visual Studio 2015 Community, Professional (Update 3)

##Whats next?
* [See milestones inside bugtracker](https://github.com/Dark-Water/GitSubModulesVsPlugin/milestones)

