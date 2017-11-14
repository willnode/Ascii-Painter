# ASCII-Painter Usage Instruction

## UI

ASCII-Painter have simple menu bar. Divided into three sections:

+ Menu bar. The place for obvious commands.
+ Palette box. The character to be drawn.
+ Tool box. Range of tools to be used.

## Primary Hotkeys

Obvious hotkeys:

| Hotkey | Action |
|---|---|
| Delete | Clear selected cell contents |
| Ctrl+A | Select All |
| Ctrl+C | Copy |
| Ctrl+F | Select Font |
| Ctrl+G | Toggle Gridlines |
| Ctrl+N | New Art |
| Ctrl+S | Export Art as plain text |
| Ctrl+V | Paste |
| Ctrl+Shift+V | Trim and Paste |
| Ctrl+Z | Undo |
| Alt+F4 | Save and Close |

Optionally these hotkeys for tools as well.

| Hotkey | Activate Tool |
|---|---|
| F1 | Select |
| F2 | Freetype |
| F3 | Dragdrop |
| F4 | Brush |
| F5 | Line |
| F6 | Rectangle |
| F7 | Circle |

## Select Tool

> Manage selection and perform basic tasks. Selected cells are highlighted blue.

| Input | Action |
|---|:--|
| Drag | Select new Area |
| Ctrl+Drag | Begin Dragdrop (Release the mouse to finish) |
| [Any key] | Begin Freetyping (Enter to finish) |

## Freetype Tool

> Type characters directly in the selection. Yellow block will appear as the cursor.

> If selection size is 1x1, the cursor able to roam the whole art.

| Input | Action |
|---|:--|
| [Any key] | Type new character and advance right |
| [Arrow key] | Navigate cursor inside selection |
| Backspace | Move left and clear the content |
| Shift+Delete | Move right and clear the content |
| Delete | Move characters right side backward |
| Shift+Backspace | Move characters left side forward |
| Insert | Move characters right side forward |
| Shift+Insert | Move characters left side backward |

## Dragdrop Tool

> Move the selected cell.

| Input | Action |
|---|:--|
| Drag | Move the selected cell |

## Drawing Tools

The hotkeys are the same for drawing tools below:

| Input | Action |
|---|:--|
| Drag | Fill the character to cells along the path |
| Space | Set palette box to blank space |
| Backspace | Clear palette box |
| [Any key] | Set palette box to that character |

### Brush Tool

> Hold down to fill cells with a character from palette box.

> When palette box is empty or set to black space, the brush will clear contents that painted along its path.

### Line Tool

> Hold down to fill cells with a character from palette box using straight pattern.

> When palette box is set to blank space, the brush will clear contents that painted along its straight path.

> When palette box is set to empty, the contents that painted along its straight path will adapt.

### Rectangle Tool

> Hold down to fill cells with a character from palette box using rectangular pattern.

> When palette box is set to blank space, the brush will clear contents that painted along its rectangular path.

> When palette box is set to empty, the contents that painted along its rectangular path will adapt.

### Circle Tool

> Hold down to fill cells with a character from palette box using circular pattern.

> When palette box is set to blank space, the brush will clear contents that painted along its circular path.

> When palette box is set to empty, the contents that painted along its circular path will be set to '+'.

***

## Have a new idea?

Use issues panel for new ideas and feedback. Enjoy!