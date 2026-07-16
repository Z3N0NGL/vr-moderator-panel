# VR Moderator Panel (Unity)

An open-source in-game moderator panel for Unity VR projects. Opens on a
joystick click, floats in front of the player's face and tracks their
head until closed, with 5 tabs: Movement, Local, Players, Session, and
Settings.

No icons, no images. Plain text UI, plain positional head tracking.

## Requirements

- Unity 2021 LTS or newer
- A working VR project (XR Plugin Management + OpenXR or another XR
  provider), with a player rig and tracked controllers already set up
- Your own multiplayer networking already in place (Mirror, Fusion,
  Netcode for GameObjects, a custom server, etc.) — required for the
  Players tab to do anything real. See below.

## Before you do anything else

Read **[MUST_DO_CHECKLIST.md](MUST_DO_CHECKLIST.md)**. It lists exactly
what is required for this to function versus what's optional. In short:

- Panel opening, tabs, sliders, toggles, and head tracking work out of
  the box once wired up in the Unity Editor (no coding required).
- Kick / Bring Here / Teleport To / Mute All / Respawn All / Lock Lobby
  require you to fill in `Scripts/ExampleNetworkAdapter.cs` with real
  calls into your own networking system. This cannot be made generic —
  every multiplayer game routes these differently, so there is no way
  to ship this working for every backend out of the box.
- Anyone can open the panel until you add `Scripts/
  ModeratorPermissionGate.cs` and set real moderator PlayFab IDs, or
  wire your own permission system into `ModPanelActivator.SetAuthorized()`.

## Full setup

Full click-by-click Unity setup instructions, written for people who
have never wired up Unity UI before, are in
**[SETUP_GUIDE.md](SETUP_GUIDE.md)**.

## Scripts

| File | What it does |
|---|---|
| `ModPanelActivator.cs` | Joystick click opens/closes the panel; panel follows the player's head |
| `ModPanelTabController.cs` | Switches between the 5 tabs |
| `MovementTabController.cs` | Fly toggle + speed, speed boost toggle + multiplier, scale toggle + multiplier |
| `LocalTabController.cs` | Priority speaker toggle + volume multiplier |
| `PlayerTabController.cs` | Player list, selection, kick/bring here/teleport to |
| `PlayerListEntry.cs` | One row in the player list |
| `SessionTabController.cs` | Mute all, respawn all, lock lobby, live player count |
| `PanelSettingsTabController.cs` | Panel distance, follow speed, lock-in-place, close button |
| `IModNetworkAdapter.cs` | Interface separating the UI from your networking code |
| `ExampleNetworkAdapter.cs` | Fake example data — copy and fill in with your real networking calls |
| `ModeratorPermissionGate.cs` | Restricts who can open the panel to a list of PlayFab IDs |

## Ranges (enforced in code, not just the Inspector)

- Fly speed: 5–25
- Speed boost multiplier: 1–3x
- Scale multiplier: 0.2–5x
- Priority speaker volume: 1–5x

## Contributing

Issues and pull requests welcome. If you write a network adapter for a
specific backend (Mirror, Fusion, Netcode, etc.) and want to share it
for others to reference, a PR adding it under `Scripts/Adapters/` would
be genuinely useful to other people using this.

## License

MIT — see [LICENSE](LICENSE).
