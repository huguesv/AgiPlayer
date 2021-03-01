# Agi Player

Agi Player is an open-source implementation of Sierra's popular AGI adventure game engine used by the first entries in the King's Quest, Space Quest, Leisure Suit Larry series and more.

It is implemented in C#, and uses SDL for graphics, input and sound.

## Credits

Agi Player is based on Nick Sonneveld's [NAGI](https://github.com/sonneveld/nagi), which is written in C.

Nick converted the original Sierra AGI implementation from assembly code to C and ported all graphics, input and sound to SDL.

I converted his implementation from C to C# and added some new features.

Why? Because it's fun and it was an opportunity to learn more about the internals of AGI.

## Requirements

- .NET 5 or later
- Windows 7 or later, 32-bit or 64-bit

## Features

- Supports PC, Atari ST and Amiga AGI v2 & v3 games.
- Themes to emulate the color palettes and fonts used by the various AGI interpreters. This includes themes for CGA, EGA, Hercules, Atari ST, Amiga and AppleIIgs. You can cycle through the themes while the game is running using CTRL+R.
- 1-channel and 4-channel sound support.
- Optionally skip the question at the startup of some Sierra games.
- Support for games that are compressed using the zip file format.
- Automatic game detection of nearly all known versions of the PC, Atari ST and Amiga Sierra games.
- 3 input methods: Classic prompt (no-pause), Input box (pause), and Classic w/ Word List box.

## Getting Started

Agi Player looks in the current working directory for an AGI game. This makes it easy to run a game using Agi Player by simply copying it to the game folder.

If no game is found in the directory, then it searches its subdirectories recursively, as well as any .zip files. The games that are found are then listed in a game selection screen.

You can specify an alternate directory by passing it as a command-line argument, in order to launch or discover games in a directory separate from where Agi Player is located.

## Configuration

Agi Player can be configured using the settings.xml file, included with the release.

See the contents of settings.xml for a description of each setting, used to control display, sound, input and logic execution.

## Supported Games

Sierra developed several games during the years 1985-1989 using the AGI system. There were 4 major revisions of the system for the PC platform.

- Version 0: Unsupported. Used by the original booter version of King's Quest I.
- Version 1: Unsupported. Used by the original booter version of King's Quest II and The Black Cauldron.
- Version 2: Supported. Donald Duck's Playground, Christmas Card, King's Quest I, II, III, Leisure Suit Larry I, Police Quest I, Space Quest I and II, Mixed-Up Mother Goose.
- Version 3: Supported. The Black Cauldron, King's Quest IV, Gold Rush!, Manhunter I and II.

In addition to the PC versions of AGI v2 and v3 games, most Atari ST and Amiga versions of the games also work.

Official Sierra demos also work. Fan games probably work too, but are untested.

![AGI Demo 1](images//AGI%20Demo%201.png?raw=true "AGI Demo 1")
![AGI Demo 2](images//AGI%20Demo%202.png?raw=true "AGI Demo 2")
![AGI Demo 3](images//AGI%20Demo%203.png?raw=true "AGI Demo 3")
![The Black Cauldron](images//The%20Black%20Cauldron%202.00.png?raw=true "The Black Cauldron")
![Christmas Card](images//Christmas%20Card%20v1.png?raw=true "Christmas Card")
![Donald Duck's Playground](images//Donald%20Duck's%20Playground%201.0A.png?raw=true "Donald Duck's Playground")
![King's Quest I](images//King's%20Quest%20I%202.0F.png?raw=true "King's Quest I")
![King's Quest II](images//King's%20Quest%20II%202.1.png?raw=true "King's Quest II")
![King's Quest III](images//King's%20Quest%20III%201.01.png?raw=true "King's Quest III")
![King's Quest IV](images//King's%20Quest%20IV%202.0.png?raw=true "King's Quest IV")
![Leisure Suit Larry in the Land of the Lounge Lizards](images//Leisure%20Suit%20Larry%20I%201.05.png?raw=true "Leisure Suit Larry in the Land of the Lounge Lizards")
![Manhunter: New York](images//Manhunter%20New%20York%201.22.png?raw=true "Manhunter: New York")
![Manhunter: San Francisco](images//Manhunter%20San%20Francisco%203.02.png?raw=true "Manhunter: San Francisco")
![Mixed-Up Mother Goose](images//Mixed-Up%20Mother%20Goose.png?raw=true "Mixed-Up Mother Goose")
![Police Quest: In Pursuit of the Death Angel](images//Police%20Quest%20I%202.0G.png?raw=true "Police Quest: In Pursuit of the Death Angel")
![Space Quest I: The Sarien Encounter](images//Space%20Quest%20I%202.2.png?raw=true "Space Quest I: The Sarien Encounter")
![Space Quest II: Vohaul's Revenge](images//Space%20Quest%20II%202.0F.png?raw=true "Space Quest II: Vohaul's Revenge")

## License

COPYRIGHT AND PERMISSION NOTICE

Copyright (c) 2006-2021 Hugues Valois

Copyright (c) 2000-2002 Nick Sonneveld

All rights reserved.

Permission is hereby granted, free of charge, to any person obtaining a
copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, and/or sell copies of the Software, and to permit persons
to whom the Software is furnished to do so, provided that the above
copyright notice(s) and this permission notice appear in all copies of
the Software and that both the above copyright notice(s) and this
permission notice appear in supporting documentation.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT
OF THIRD PARTY RIGHTS. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
HOLDERS INCLUDED IN THIS NOTICE BE LIABLE FOR ANY CLAIM, OR ANY SPECIAL
INDIRECT OR CONSEQUENTIAL DAMAGES, OR ANY DAMAGES WHATSOEVER RESULTING
FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT,
NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF OR IN CONNECTION
WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.

Except as contained in this notice, the name of a copyright holder
shall not be used in advertising or otherwise to promote the sale, use
or other dealings in this Software without prior written authorization
of the copyright holder.
