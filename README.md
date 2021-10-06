# WalkBoxes Reader/Converter

This is an application designed for reading and converting **.wbox files (WalkBoxes)** and also modifying them.

**If there are any issues PLEASE report them to [HERE](https://github.com/Telltale-Modding-Group/WalkBoxes-Converter/issues)**

## DISCLAIMER (Please Read)
- Supports 6VSM Version Files (TWD DE, TFS, other recent versions)
- Supports 5VSM Version Files (Walking Dead S2, Walking Dead S3, similar versions)
- Supports ERTM Version Files (Walking Dead S1, and other similar versions)

This is a console app, and it spits out a .json file with all of the serialized data in the telltale .wbox file. 

For the time being given with the complexity and amount of data in a wbox file a custom editor will need to be created in order to edit or interpert these files and modifing the data within it (but I am working on that).

However, with that said, the json file can be fully read and convertered or interperted in any way you see fit. It is reading the full file so its possible to generate one from scratch and have it work in the given game version you want.

### Developers

**If you want to learn about the file format** I suggest you look in the **[Program.cs](https://github.com/Telltale-Modding-Group/WalkBoxes-Converter/blob/main/app-console/wbox-console/wbox-console/WalkBoxes_File.cs)** script, it's is well documented and describes the .wbox format well.
