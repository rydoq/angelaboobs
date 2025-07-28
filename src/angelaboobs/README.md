# Angela Boob Mod

Adds breast sliders, a couple body sliders, and 3D nipples to Angela, changeable in EasySettings. You may change her textures, and remove her clothing too.

Requires EasySettings

Included both as an example of a texture mod and because they are great are Goth Angela textures by FunfettiPhantom. I edited them some to work with the extra geometry in this mod. https://ko-fi.com/funfettiphantom

Please install with your mod manager. I will not be responsible for issues with a manual install. If you do install manually, the "config" folder is where texture mods go.

### New textures guide for modders:
If you want to edit or add textures, they are in /BepInEx/Config/NPCNSFWModFiles/Angela. Every texture shown in the Default folder can be changed (except faces currently), but if you only want to change some, the mod loads what's in the Default folder for the rest so don't feel like you need to change all of those. Make a new folder titled what you want the texture group to be. All the vanilla naming is expected **except** angelaTex_01.png needs renamed to angelaTex_01_base.png, and you want to include nude, bottomless, and topless variants of that with _nude, _bottomless, and _topless at the end, as shown in the files.

Feel free to make your own texture mods requiring this! That's part of the point of this mod, making swapping textures easy and able to work through Thunderstore.

### Known issues:
* Her face texture does not change. As far as I can tell it's controlled by her animator, but that's just my best guess. I have no idea how to change it during runtime. If someone who's good at unity wants to reach out on the github for this I'd love the help.
* If you change her dialog textures in a texture pack (e.g. facepic_angela01.png, etc.) they do not take effect until a game load. They're Sprite resources so I can't change them so easily. Help on this would be nice too.

### Next steps:
* If I can, make breast bounce from the dynamic bones reduce if the breasts are smaller
* If I can get her face texture able to be swapped then I'll say she's finished
* Most of this code should be simple to reuse on other npcs, so as long as I can make the right meshes and textures those should be much, much faster. Once I have another NPC done I will deprecate this mod in favor of a more generally named npc appearance controller mod.