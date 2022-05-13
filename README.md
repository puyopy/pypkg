<html>
<center><h1><b>pypkg</b></h1></center>
<center><p><b>A messy wally client written in C#</b></p></center>
</html>

<br>
<br>

## Available commands:
    pypkg install scope/name - Creates a "Packages" folder if it doesn't already exist and installs the package onto it.

## Will not do:
    wally publish / pypkg publish
    wally login / pypkg login
    wally logout / pypkg logout
    wally package

    for obvious reasons. I would rather not mess with their mess of an API, nor are those endpoints documented

<html>
<center><h5><i><l>will probably not do:</l></i></h5></center>
</html>
    Manifest parsing - dear lord i hate the toml format
    Lockfiles

