# NPC Appearance Mod

Adds the ability to change parts of NPC appearances, including body sliders and textures. Currently featuring Angela and Sally.

Requires EasySettings

Included both as an example of a texture mod and because they are great are Goth Angela textures by FunfettiPhantom. I edited them some to work with the extra geometry in this mod. https://ko-fi.com/funfettiphantom

Please install with your mod manager. I will not be responsible for issues with a manual install. If you do install manually, the "config" folder is where texture mods go.

### Usage:
Press Esc, go to the Mods tab, and change various settings. Each NPC has their own category. Settings are saved between sessions, so you can find what you like and keep them there.

### New textures guide for modders:
If you want to edit or add textures, they are in /BepInEx/Config/NPCNSFWModFiles. The textures are organized by NPC, so Angela has her own folder, Sally has hers, etc. Every texture shown in the Default folder can be changed (except faces currently), but if you only want to change some, the mod loads what's in the Default folder for the rest so don't feel like you need to change all of those. Make a new folder titled what you want the texture group to be. All the vanilla naming is expected **except** angelaTex_01.png needs renamed to angelaTex_01_base.png, and you want to include nude, bottomless, and topless variants of that with _nude, _bottomless, and _topless at the end, as shown in the files.

Feel free to make your own texture mods requiring this! That's part of the point of this mod, making swapping textures easy and able to work through Thunderstore.

### Known issues:
* Face textures do not change. As far as I can tell it's controlled by the character's animator, but that's just my best guess. I have no idea how to change it during runtime. If someone who's good at unity wants to reach out on the github for this I'd love the help.
* If you change dialog textures in a texture pack (e.g. facepic_angela01.png, etc.) they do not take effect until a game load. They're Sprite resources so I can't change them so easily. Help on this would be nice too.

### Next steps:
* Make Angela textures asymmetrical
* Probably fix UV mapping of Sally long hair, it sucks
* Put in Enok to be done with the main 3
* Face texture fix if possible