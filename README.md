<html>
<center><h1><b>pypkg</b></h1></center>
<center><p><b>A messy wally client written in C#</b></p></center>
</html>

<br>
<br>

# Installation
There is currently no easy way to install pypkg. -- you have to add it manually to your PATH environment variable, which I am not gonna bother with yet.
## Available commands:
    pypkg install scope/name - Creates a "Packages" folder if it doesn't already exist and installs the package onto it.
    
    Example:
    pypkg install roblox/roact
	
	pypkg uninstall scope/name or pypkg uninstall name - Attempts to uninstall the specified package.
	
	Examples:
	pypkg uninstall roblox/roact
	pypkg uninstall fusion

## Will not do:
    wally publish / pypkg publish
    wally login / pypkg login
    wally logout / pypkg logout
    wally package

    for obvious reasons. I would rather not mess with their mess of an API, nor are 

<html>
<center><h5><i><l>will probably not do:</l></i></h5></center>

<center><code>Manifest parsing - dear lord i hate the toml format</code></center>
<center><code>Lockfiles</code></center>
</html>
