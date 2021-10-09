# WalkBoxes Reader/Converter

This is an application designed for reading and converting **.wbox files (WalkBoxes)** and also modifying them.

**If there are any issues PLEASE report them to [HERE](https://github.com/Telltale-Modding-Group/WalkBoxes-Converter/issues)**

## DISCLAIMER (Please Read)
Supports the following *(haven't tested all of them)*
- Jurassic Park: The Game *(ERTM)*
- Law & Order: Legacies *(ERTM)*
- The Walking Dead *(ERTM)*
- Poker Night 2 *(ERTM)*
- The Wolf Among Us *(5VSM)*
- The Walking Dead: Season Two *(5VSM)*
- Tales from the Borderlands *(5VSM)*
- Game of Thrones *(5VSM)*
- Minecraft: Story Mode *(5VSM)*
- The Walking Dead: Michonne *(5VSM)*
- Batman: The Telltale Series *(6VSM)*
- The Walking Dead: A New Frontier *(6VSM)*
- Guardians of the Galaxy: The Telltale Series *(6VSM)*
- Minecraft: Story Mode – Season Two *(6VSM)*
- Batman: The Enemy Within *(6VSM)*
- The Walking Dead Collection? (not sure) *(6VSM)*
- The Walking Dead: The Final Season *(6VSM)*
- The Walking Dead: The Telltale Definitive Series *(6VSM)*

This is a console app, and it spits out a .json file with all of the serialized data in the telltale .wbox file. 

For the time being given with the complexity and amount of data in a wbox file a custom editor will need to be created in order to edit or interpert these files and modifing the data within it (but I am working on that).

However, with that said, the json file can be fully read and convertered or interperted in any way you see fit. It is reading the full file so its possible to generate one from scratch and have it work in the given game version you want.

### Developers

**If you want to learn about the file format** I suggest you look in the **[Program.cs](https://github.com/Telltale-Modding-Group/WalkBoxes-Converter/blob/main/app-console/wbox-console/wbox-console/WalkBoxes_File.cs)** script, it's is well documented and describes the .wbox format well.
