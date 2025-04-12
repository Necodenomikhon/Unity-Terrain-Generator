# Unity Terrain Generator

This is a set of scripts to generate terrain in Unity3D using a custom wave function collapse algorithm. These scripts were last tested in January 2023 so they may not work for the latest versions of Unity.

## Instructions: 
1. Create a Unity3D project
2. Drag and drop the scripts into the game editor hierarchy.
3. Create a game object that will act as the terrain generator.
4. Attach the WaveCollapseGrid.cs to it.
5. Create a game object that will act as the cell.
6. Attach the WaveCollapseCell.cs to it.
7. Hit Run in the editor.
8. A grid of unity gizmos should appear with different colors representing different terrains.
9. Alternatively, one could test the script found in /Old/ using similar Instructions.
   - It was last tested in March 2021, so it may require an even older version of Unity.

## Known Bugs:
- Some cells do not collapse.
