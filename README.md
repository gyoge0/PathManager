# Path Manager

This is a simple UWP app I built with WinUI 3 to manage the Path variable.

# Installation

You'll need to build with Visual Studio in order to use this app. 

Once the solution is opened in Visual Studio, right click the PathManager.UI project > package and publish > create app packages.

You can create a self signed key to sign the msix files and then install the app that way.

Make sure to use Release mode and not Debug mode when building the app.

## Bundling

If you want to create an msixbundle, I've provided bundle.ps1 as a (not very secure or 100% functional) way of doing this.

Run the script and pass the password for the cert as the first argument. 
If you don't pass a second argument, it will look for the default temp key created by Visual Studio.
If you have another key that you used to sign it, pass the path to the pfx file as the second argument.
