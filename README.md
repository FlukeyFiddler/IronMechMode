# S.O.L. - SavetyOffLine

Catch yourself F5-ing and F9-ing alot, just out of habit? Miss that in BT? 
Well, look no further! No seriously, you still here? You don't want this mod!

This is crazy-ass "Hey let's use a mod that messes with save files in a bug-ridden game and a month-old
modding community, Yeah! Whoop Whoop!"

S.O.L. aims to give BT a rogue-like experience. It will auto-save during the campaign and in combat and disallow 
loading. No save-scumming, no "ah let's try to get those reinforcements too", and maybe even "run awaaaay!" 
from time to time. 

## Changelog 
- V0.6.0
	- Disable S.O.L. if vanilla Ironman campaign is played, enable if any other campaign
	- Differentiate S.O.L. from Ironman on the savegames screen and disable the load button for S.O.L.
- V0.5.0 (Pre-Release)
	- Autosaves at the start of each round in combat, deletes saves over 4
	- Autosaves in simgame left unchanged, deletes saves over 4
	- Saving forced on quit to menu or quit game
	- No loading or saving, only continue, existing saves are not shown in-game

## Big Fat Disclaimer
I wasn't kidding about BT still having a lot of bugs, and the modding community being fresh and still learning / deciding how 
to handle dependencies and conflicts between mods, or decide on best practices for modding. 
So maybe it's **not a good idea to disable saving and loading, your game can and probably will break at some point and your
saves will be gone!** Oh and did I forget to mention **this is my first mod, ever, written in C#, a new language to me.**

## Requirements / Installation
- Install [BattleTechModLoader](https://github.com/Mpstark/BattleTechModLoader/releases) using the [instructions here](https://github.com/Mpstark/BattleTechModLoader)
- Install [ModTek](https://github.com/Mpstark/ModTek/releases) using the [instructions here](https://github.com/Mpstark/ModTek)
- Download the latest release (the .zip) from [github](https://github.com/FlukeyFiddler/SavetyOffLine/releases)
- Unzip into your Mods folder
- **Start a new campaign**, many of your saves *will* be deleted on continuing and **you *will* lose saves when you disable S.O.L.**

## Issues
Please report any issues [here](https://github.com/FlukeyFiddler/SavetyOffLine/issues)
Also feel free to drop by on the [BattleTechGame Discord](https://discord.gg/dqT4yWz) if you have any questions.

## Special thanks
As mentioned, this is my first step into modding, and I want to thank some people for showing me the ropes and supporting me during
the creation of this mod.
- Morphyum, first pro I ran into and very helpful to get me started, thanks for letting me steal many lines of code ;p
- LadyAlekto, seeing the amount of work you (and Morph) put in to [RogueTech](https://www.nexusmods.com/battletech/mods/79) was inspiring, and
the encouragement really helped getting into it!
- JF, ah JF, had many a chat about everything and absolutely nothing, good for winding down and rubber-ducking, love ya man!
- And everyone on the [BattleTechGame Discord](https://discord.gg/dqT4yWz), it's great to be part of the community and I'm excited to see how
far we'll take this!