# Moderator Panel — Full Beginner Setup Guide

This guide assumes you have never wired up a Unity UI before. Follow it top
to bottom, in order. Don't skip steps — each one depends on the last.

---

## Why so many files?

Each script does exactly one job. That's on purpose, not clutter:

- If everything were in one giant script, one typo could break the whole
  panel, and you'd have to scroll through hundreds of lines to find the
  one toggle you want to tweak.
- Each tab (Movement, Local, Players, Session, Settings) is genuinely a
  separate feature with its own settings. Splitting them means you can
  open `MovementTabController.cs` and see *only* movement code — nothing
  about players or audio gets in the way.
- `IModNetworkAdapter.cs` exists so that the "kick/teleport/bring here"
  logic — which depends entirely on YOUR networking setup — is isolated
  from everything else. You only ever have to edit one file when you hook
  up your real network code. Nothing else needs to change.
- Small files are easier to reuse, easier to debug (the error message
  tells you exactly which file and job broke), and easier for someone
  else on your team to understand without reading everything.

Think of it like a toolbox: one file = one tool. You reach for the one you
need instead of digging through a pile.

---

## Part 0 — What you need before starting

- Unity 2021 LTS or newer, with an XR project already set up (XR Plugin
  Management + OpenXR, or Meta/Oculus XR plugin — either is fine).
- Your player rig already in the scene, with a working XR Camera (the
  Camera that moves with the headset).
- The 11 script files from the previous message, downloaded.

If you don't already have a VR rig in your scene, set that up first
(Unity's XR Interaction Toolkit sample rig is the easiest starting point)
before doing anything below.

---

## Part 1 — Import the scripts

1. In the **Project** window, right-click your `Assets` folder → **Create
   → Folder**, name it `ModPanel`.
2. Drag all 11 `.cs` files (and the README) into that folder.
3. Wait for Unity to finish compiling (bottom-right corner spinner). If
   you see red errors in the **Console** window, stop and check you copied
   every file — some scripts reference each other.

---

## Part 2 — Build the panel's UI skeleton

This is the biggest part. We're building a floating screen with 5 tabs.

### 2.1 Create the World Space Canvas

1. Hierarchy → right-click → **UI → Canvas**. Rename it `ModPanelCanvas`.
2. Select it. In the **Canvas** component, set **Render Mode** to
   **World Space**.
3. In the **Rect Transform**, set **Width** = `600`, **Height** = `400`.
4. Set the **Scale** (X, Y, Z) to `0.001` on all three axes. This shrinks
   the 600x400 pixel canvas down to roughly 0.6m x 0.4m in world space —
   readable size for a VR menu.
5. Add a background: right-click `ModPanelCanvas` → **UI → Image**,
   rename it `Background`, stretch it to fill the whole canvas (use the
   anchor presets in the top-left of the Rect Transform, hold Alt+Shift
   and click the "stretch both" option). Set its color to something dark,
   e.g. near-black with slight transparency.
6. `ModPanelCanvas` also needs a **Canvas Scaler** and **Graphic Raycaster**
   component — Unity adds a Graphic Raycaster automatically. Add a
   **Tracked Device Graphic Raycaster** instead if you're using XR
   Interaction Toolkit for pointer input (Add Component → search for it).

### 2.2 Create the tab buttons row

1. Right-click `ModPanelCanvas` → **UI → Panel**, rename it `TabBar`.
   Anchor it to the top of the canvas, height ~50px.
2. Under `TabBar`, add a **Horizontal Layout Group** component so the 5
   buttons space themselves out evenly.
3. Create 5 buttons under `TabBar`: right-click `TabBar` → **UI → Button
   - TextMeshPro** (if prompted to import TMP Essentials, click **Import**).
   Do this 5 times, or create one and duplicate it (Ctrl+D) 4 times.
4. Rename them `Btn_Movement`, `Btn_Local`, `Btn_Players`, `Btn_Session`,
   `Btn_Settings`.
5. For each button, expand it, select the child **Text (TMP)** object, and
   set the text to `MOVEMENT`, `LOCAL`, `PLAYERS`, `SESSION`, `SETTINGS`
   respectively (plain text, no icons — matches the design).

### 2.3 Create the 5 tab content panels

1. Right-click `ModPanelCanvas` → **Create Empty**, rename it
   `Tab_Movement`. Add a **Rect Transform** stretch (fills the canvas
   below the tab bar).
2. Duplicate it 4 times, renaming the copies `Tab_Local`, `Tab_Players`,
   `Tab_Session`, `Tab_Settings`.
3. You'll build the contents of each one in Parts 3–7 below.

Your Hierarchy under `ModPanelCanvas` should now look like:

```
ModPanelCanvas
├── Background
├── TabBar
│   ├── Btn_Movement
│   ├── Btn_Local
│   ├── Btn_Players
│   ├── Btn_Session
│   └── Btn_Settings
├── Tab_Movement
├── Tab_Local
├── Tab_Players
├── Tab_Session
└── Tab_Settings
```

---

## Part 3 — Wire up tab switching

1. Select `ModPanelCanvas`. **Add Component → Mod Panel Tab Controller**.
2. In the Inspector you'll see a **Tabs** array. Set **Size** to `5`.
3. Fill in each element:
   - Element 0: Tab Name = `Movement`, Tab Root = `Tab_Movement`, Tab
     Button = `Btn_Movement`
   - Element 1: `Local` / `Tab_Local` / `Btn_Local`
   - Element 2: `Players` / `Tab_Players` / `Btn_Players`
   - Element 3: `Session` / `Tab_Session` / `Btn_Session`
   - Element 4: `Settings` / `Tab_Settings` / `Btn_Settings`
4. Press Play once everything below is wired up — clicking a tab button
   should show only that tab's content.

---

## Part 4 — Movement tab contents

Inside `Tab_Movement`, build this layout (each row = one setting):

1. **Fly Toggle**: right-click `Tab_Movement` → **UI → Toggle**. Rename
   `Toggle_Fly`. Set its label text to `Fly`.
2. **Fly Speed Slider**: right-click → **UI → Slider**, rename
   `Slider_FlySpeed`. Add a **Text (TMP)** next to it named
   `Label_FlySpeedValue` to show the number.
3. **Speed Boost Toggle**: duplicate the fly toggle, rename
   `Toggle_SpeedBoost`, label `Speed Boost`.
4. **Speed Boost Slider**: duplicate the fly speed slider, rename
   `Slider_SpeedBoost`, value label `Label_SpeedBoostValue`.
5. **Scale Toggle**: duplicate again, rename `Toggle_Scale`, label
   `Scale`.
6. **Scale Slider**: duplicate again, rename `Slider_Scale`, value label
   `Label_ScaleValue`.
7. Arrange them vertically — easiest way: add a **Vertical Layout Group**
   component to `Tab_Movement` so they stack automatically with spacing.

Now wire the script:

1. Select `Tab_Movement`. **Add Component → Movement Tab Controller**.
2. Drag your player rig's Transform (the object that actually moves in
   the world — not the camera, the root rig object) into **Player Rig**.
3. Drag `Toggle_Fly` into **Fly Toggle**.
4. Drag `Slider_FlySpeed` into **Fly Speed Slider**, and
   `Label_FlySpeedValue` into **Fly Speed Value Label**.
5. Repeat for **Speed Boost Toggle/Slider/Label** and **Scale
   Toggle/Slider/Label** using the matching objects.

You do NOT need to manually set the slider min/max — the script sets
them automatically on Play (5–25 for fly speed, 1–3 for speed boost,
0.2–5 for scale).

---

## Part 5 — Local tab contents

Inside `Tab_Local`:

1. Add one **Toggle** (`Toggle_PrioritySpeaker`, label `Priority
   Speaker`).
2. Add one **Slider** (`Slider_PriorityVolume`) with a value label
   (`Label_PriorityVolumeValue`).
3. Select `Tab_Local`. **Add Component → Local Tab Controller**.
4. Drag the toggle into **Priority Speaker Toggle**.
5. Drag the slider into **Priority Volume Slider**, label into **Priority
   Volume Value Label**.

---

## Part 6 — Players tab contents (the most involved one)

### 6.1 Build the player row prefab first

1. In your `ModPanel` Project folder, create a new empty GameObject in
   the scene (Hierarchy → right-click → Create Empty), rename it
   `PlayerRowPrefab`.
2. Add a **Horizontal Layout Group** to it (optional but keeps things
   tidy).
3. Add a child **Text (TMP)** object for the name, rename `NameLabel`.
4. Add a child **Button - TextMeshPro**, rename `SelectButton`, label its
   text `Select` — or make the whole row clickable and skip a separate
   button if you prefer (either works, just make sure something has a
   Button component).
5. Select `PlayerRowPrefab`. **Add Component → Player List Entry**.
   - Drag `NameLabel` into **Name Label**.
   - Drag `SelectButton` into **Select Button**.
6. Drag `PlayerRowPrefab` from the Hierarchy into your `ModPanel` Project
   folder to turn it into a prefab. Once it's a prefab asset, delete the
   copy that's sitting in the Hierarchy.

### 6.2 Build the scrolling list

1. Inside `Tab_Players`, right-click → **UI → Scroll View**, rename
   `PlayerScrollView`. Resize/anchor it to take up roughly the left half
   of the tab.
2. Inside it, find `PlayerScrollView → Viewport → Content`. Add a
   **Vertical Layout Group** and a **Content Size Fitter** (set Vertical
   Fit to **Preferred Size**) to `Content` so rows stack downward and the
   scroll area grows correctly.

### 6.3 Build the selected-player detail panel

1. Right-click `Tab_Players` → **Create Empty**, rename
   `SelectedPlayerPanel`. Position it on the right half of the tab.
2. Add a **Text (TMP)** child, rename `NameLabel`.
3. Add another **Text (TMP)** child below it, rename `IdLabel` (this
   shows the PlayFab ID).
4. Add three **Button - TextMeshPro** children: `KickButton` (label
   `Kick`), `BringHereButton` (label `Bring Here`), `TeleportToButton`
   (label `Teleport To`).
5. `SelectedPlayerPanel` will be hidden until a player is clicked — leave
   it active for now, the script disables it automatically on Start.

### 6.4 Wire the script

1. Select `Tab_Players`. **Add Component → Player Tab Controller**.
2. Drag `Content` (from inside the Scroll View) into **Player List
   Content**.
3. Drag the `PlayerRowPrefab` asset from your Project folder into
   **Player List Entry Prefab**.
4. Drag `SelectedPlayerPanel` into **Selected Player Panel**.
5. Drag `NameLabel` (the one inside SelectedPlayerPanel) into **Selected
   Player Name Label**.
6. Drag `IdLabel` into **Selected Player Id Label**.
7. Drag `KickButton`, `BringHereButton`, `TeleportToButton` into their
   matching fields.

This tab will show nothing useful yet because it has no network adapter
— that's Part 9.

---

## Part 7 — Session tab contents

Inside `Tab_Session`:

1. Add two **Buttons**: `Btn_MuteAll` (label `Mute All`), `Btn_
   RespawnAll` (label `Respawn All`).
2. Add one **Toggle**: `Toggle_LockLobby`, label `Lock Lobby`.
3. Add two **Text (TMP)** objects: `Label_PlayerCount`, `Label_
   SessionName`.
4. Select `Tab_Session`. **Add Component → Session Tab Controller**.
5. Drag `Btn_MuteAll` into **Mute All Button**, `Btn_RespawnAll` into
   **Respawn All Button**, `Toggle_LockLobby` into **Lock Lobby Toggle**.
6. Drag `Label_PlayerCount` into **Player Count Label**, `Label_
   SessionName` into **Session Name Label**.
7. Optionally set the **Session Name** field in the Inspector to your
   world/lobby's actual name.

---

## Part 8 — Settings tab contents

Inside `Tab_Settings`:

1. Add a **Slider**: `Slider_Distance`, value label `Label_
   DistanceValue`.
2. Add a **Slider**: `Slider_FollowSpeed`, value label `Label_
   FollowSpeedValue`.
3. Add a **Toggle**: `Toggle_LockPanel`, label `Lock Panel In Place`.
4. Add a **Button**: `Btn_Close`, label `Close`.
5. Select `Tab_Settings`. **Add Component → Panel Settings Tab
   Controller**.
6. Drag your `ModPanelCanvas`'s parent object (whichever object has
   `ModPanelActivator` on it — see Part 9) into **Panel Activator**.
7. Wire the sliders, labels, toggle, and close button into their
   matching fields the same way as previous tabs.

---

## Part 9 — The activator (joystick open/close + head tracking)

This is the object that makes the whole panel appear when a moderator
clicks their joystick.

1. In the Hierarchy, create an empty GameObject at the root of your
   scene (not inside the rig necessarily, just somewhere persistent),
   rename it `ModPanelManager`.
2. **Add Component → Mod Panel Activator**.
3. Drag `ModPanelCanvas` into **Panel Root**.
4. Drag your rig's XR Camera (the object that moves with the headset)
   into **Head Transform**.
5. Leave **Distance From Face**, **Height Offset**, and the follow speeds
   at their defaults for now — you can tune them later from the Settings
   tab in Play mode, or directly in the Inspector.
6. Go back to Part 8, step 6, and make sure you dragged `ModPanelManager`
   into the Settings tab's **Panel Activator** field.
7. Set `ModPanelCanvas` to **inactive** in the Hierarchy (uncheck the box
   next to its name) — the script handles turning it on/off, so it should
   start hidden.

---

## Part 10 — The network adapter (kick / bring here / teleport to)

This is the one piece that depends on YOUR game's networking, and it's
the reason Part 6's buttons won't do anything yet.

1. Duplicate `ExampleNetworkAdapter.cs`, rename the copy to something
   like `MyGameNetworkAdapter.cs`.
2. Open it. Every method has a `// TODO` comment showing exactly what it
   needs to do (get the real player list, send a kick command, etc).
   Replace each TODO with a call into your actual networking system —
   this part requires knowing your networking framework (Mirror, Fusion,
   Netcode for GameObjects, a custom server, etc), so if you tell me
   which one you're using, I'll write this file for real instead of a
   stub.
3. Add this script to `ModPanelManager` (same object as `Mod Panel
   Activator` — this is important, the other scripts search the scene for
   any script that implements `IModNetworkAdapter`, and having it there
   keeps everything on one object).
4. Once it's filled in and returns a real player list, the Players tab
   will populate automatically and Kick/Bring Here/Teleport To will work.

---

## Part 11 — Give the panel to a moderator

The scripts don't check who's allowed to see the panel — that's a
decision for you (e.g. based on PlayFab role data, an admin list, etc).
The simplest approach:

1. Keep `ModPanelManager` disabled by default (`ModPanelManager`
   unchecked in the Hierarchy, or the `ModPanelActivator` component
   disabled).
2. When your game determines a player is a moderator (however you check
   that — a role field from PlayFab, a hardcoded ID list, etc), enable
   `ModPanelManager` (or just the `ModPanelActivator` component) for that
   client only.

I can help write this check once you tell me how you're storing who's a
moderator.

---

## Part 12 — Test it

1. Enter Play mode with a VR headset connected (or your XR simulator).
2. Click either controller's joystick straight down (the click, not just
   tilting it). The panel should appear in front of your face.
3. Try each tab. Sliders should update their number labels live. Toggles
   should flip visually.
4. Move your head — the panel should smoothly follow and stay facing you.
5. Click the joystick again — the panel should close.
6. In the Players tab, once your network adapter is wired up, you should
   see real players listed, and clicking one should reveal their PlayFab
   ID and the three action buttons.

---

## Troubleshooting

- **Panel doesn't appear at all**: check `ModPanelCanvas` is assigned to
  **Panel Root** on `ModPanelActivator`, and that `Head Transform` is
  assigned to your actual XR Camera (not the rig root).
- **Joystick click does nothing**: your controller may report clicks
  differently. Try lowering **Joystick Click Threshold** on `Mod Panel
  Activator` (e.g. to `0.7`), which makes a hard push in any direction
  count as a click even without a real button press.
- **Buttons don't respond to VR pointer**: make sure you're using a
  **Tracked Device Graphic Raycaster** (not the default Graphic
  Raycaster) on the Canvas, and that your controllers have XR
  Interaction Toolkit's ray interactor set up.
- **Player list stays empty**: your network adapter isn't returning real
  data yet — check Part 10.
- **Sliders show 0–1 instead of the real range**: this only happens if
  the tab's controller script isn't attached, or the slider wasn't
  dragged into the right Inspector field — double check the wiring for
  that tab.

---

If any single part of this trips you up, tell me exactly which step and
what you're seeing (screenshot of the Inspector or Console error helps
a lot) and I'll walk through it with you.
