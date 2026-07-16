# MUST-DO CHECKLIST

Read this before anything else. These are the non-negotiable steps.
If you skip any item marked REQUIRED, the panel will not work — not
"work poorly," it simply will not function, and no amount of trial and
error inside Unity will fix it without doing this specific step.

---

## REQUIRED — panel won't open at all without these

1. **Unity project must already have VR working.** A player rig with a
   working XR Camera and controllers tracked. If you don't have this
   yet, none of these scripts will do anything — get a basic VR scene
   running first (Unity's XR Interaction Toolkit sample rig is the
   fastest way).

2. **All 13 script files must be in the project**, with no compile
   errors in the Console. If even one file is missing, everything else
   fails, because the scripts reference each other directly
   (`ModPanelActivator`, `IModNetworkAdapter`, etc are not optional
   add-ons, they're load-bearing).

3. **`ModPanelActivator` must have both fields assigned**:
   - **Panel Root** → your Canvas GameObject
   - **Head Transform** → your XR Camera (the one that physically moves
     with the headset, not the rig root)
   
   Missing either one = the panel cannot appear. The script will now
   print a warning in the Console telling you exactly which one is
   missing — check there first if nothing happens.

4. **Every Toggle/Slider/Button/Text field on every tab controller must
   be dragged in**, exactly as listed in SETUP_GUIDE.md Parts 4–8. An
   unassigned field means that one control silently does nothing — it
   won't crash, it just won't respond.

5. **You must build your own network adapter.** `ExampleNetworkAdapter.cs`
   is fake data — it will make the Players tab show two players named
   "ExamplePlayerOne/Two" that don't actually exist and can't actually
   be kicked. For Kick / Bring Here / Teleport To / Mute All / Respawn
   All / Lock Lobby to do anything real, you must copy
   `ExampleNetworkAdapter.cs`, rename it, and fill in every `TODO` with
   real calls into whatever networking system your game already uses
   (Mirror, Fusion, Netcode, a custom server, etc). **No script anyone
   writes can do this step for you without knowing your networking
   setup** — this is true for every multiplayer moderator tool in
   existence, not a gap specific to this one.

---

## REQUIRED before you give this to real moderators (not just testing)

6. **Add `ModeratorPermissionGate`** to the same object as
   `ModPanelActivator`, and fill in the authorized PlayFab ID list (or
   wire your own permission system into `SetAuthorized()`). Without
   this, **every player** can open the panel and use it, not just
   moderators. It defaults to open-for-everyone specifically so you can
   test it easily — that default is not safe to ship.

---

## Optional / cosmetic — safe to skip or tune later

- Panel distance, follow speed, joystick click threshold — all have
  working defaults, tune later from the Settings tab or Inspector.
- Session tab's session name label.
- Exact colors/fonts/spacing of the UI — purely visual, doesn't affect
  function.

---

## How to know if it's actually working

Test in this order, don't skip ahead:

1. Enter Play mode. Click a joystick. Panel should appear in front of
   your face. **If not** → check Console for a warning, fix whatever it
   names, retry.
2. Switch tabs. Each one should show different content.
3. Toggle/drag every control on Movement, Local, Settings — numbers
   should update live. **If a specific control does nothing** → that
   control's field wasn't dragged into the Inspector, go re-check that
   one tab's wiring against the guide.
4. Players tab — will show nothing (or fake example data if you left
   `ExampleNetworkAdapter` attached) until step 5 above is done. This
   is expected, not a bug.
5. Once your real network adapter is attached and returns real player
   data, Kick/Bring Here/Teleport To should work as long as your TODOs
   were filled in correctly. If a button does nothing at this point,
   the problem is in your adapter's code, not these scripts — check
   your Debug.Log output / your networking system's own logs.

---

If you do all of the REQUIRED steps above and it still doesn't work,
tell me exactly which numbered step you're on and what you're seeing
(Console warning text is the most useful thing you can paste) and I'll
debug it with you directly.
