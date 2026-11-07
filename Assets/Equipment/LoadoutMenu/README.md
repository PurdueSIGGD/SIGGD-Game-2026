# Loadout Menu

This menu changes the gun, tool, and passive a player has equipped. Set it up in the Unity Inspector. You do not need to open the scripts to use it.

The buttons call `PrimaryGun.EquipWeapon`, `MiscGun.EquipMisc`, and `PassiveHolder.setPassive`. The tool slot still uses the Misc Gun component on the Weapon Holder. The buttons do not use `EquipmentManager` or `PlayerEquipmentSystem`. Those scripts do not drive the guns.

## Setup

1. Put an **Input Manager** in the scene. The guns subscribe to it when Play starts. Without it, Play errors before a button can run.
2. On the player, add **Passive Holder** if it is missing.
3. On the menu panel, add **Loadout Target**. Drag in the player's **Primary Gun**, **Tool**, and **Passive Holder**. The Tool field takes the Misc Gun on the Weapon Holder. Both sit on that same object.
4. Put each choice on its own button, as a child of that panel:
   - **Equip Gun Button**: drag one Primary Weapon asset into **Weapon**.
   - **Equip Tool Button**: drag one tool asset into **Tool**.
   - **Equip Passive Button**: drag one Passive asset into **Passive**.
5. Leave **Target** empty. Leave the button's On Click list empty. The button equips the item on its own.
6. Duplicate a button and swap the asset to add another choice.
7. For a second player, duplicate the whole menu and point that **Loadout Target** at player 2.

`Test.unity` in this folder is one working setup. Press Play and click the buttons. While the game is running, the Weapon Holder and Passive Holder fields in the Inspector change to match the click.

## Optional pieces

**Equipped Marker** turns on while that button's item is the one equipped. Make a child of the button, such as a checkmark, and drag that child into the field. Do not drag the button itself there. If the marker is the button, the script turns the button off.

**Loadout Readout** shows the equipped names. Drag a Text or TextMeshPro component into each label field.

## What a click does

A gun click refills ammo and replaces the held gun visual. A tool click resets its cooldown and replaces the tool visual. A passive click only stores the asset on Passive Holder. Health, speed, and cooldowns do not change yet. Another script still has to read the passive and apply it.

## If you open the scripts

| Script | What it does |
| --- | --- |
| `LoadoutTarget` | Holds the player's gun, tool, and passive holder. Equip methods live here. |
| `EquipGunButton` | One button, one gun asset. Calls `LoadoutTarget.EquipGun`. |
| `EquipToolButton` | One button, one tool asset. Calls `LoadoutTarget.EquipTool`. |
| `EquipPassiveButton` | One button, one passive asset. Calls `LoadoutTarget.EquipPassive`. |
| `LoadoutReadout` | Writes the current names into text labels. |
| `Editor/LoadoutMenuEditors` | Draws the blue help boxes in the Inspector. Editor only. |

`PassiveHolder` spells its methods `setPassive` and `getPassive`, with a capital P. The menu uses that spelling. `setpassive` and `getpassive` do not compile.

Each button finds the Loadout Target on a parent when **Target** is empty. After an equip, `LoadoutTarget.Changed` runs so markers and labels update.
