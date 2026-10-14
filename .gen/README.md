# .gen — the generation and verification kit for DragonRushBalance

There is no Unity Editor on this host, so the scenes, the prefab and every `.meta` were
authored as text. These are the tools that wrote them and the checks that proved them.

## Build (in order; the scene builders refuse to run twice)

    node .gen/alloc-guids.js .gen/names.json .gen/guids.json   # guids provably unused project-wide
    node .gen/write-metas.js                                   # .cs.meta / .png.meta / folder metas
    node .gen/build-prefabs.js                                 # EmberStone.prefab
    node .gen/build-loader.js                                  # Loader.unity   (splash + temple map)
    node .gen/build-game.js                                    # 2_GameScene    (balance puzzle)
    node .gen/theme-font.js                                    # the shared TMP material
    node .gen/write-manifests.js                               # assets.json + screenshot-scenario.json

The two scene builders APPEND into the template scenes. Rebuilding means restoring the
pristine scene from the ticket's template first; the guard at the top of each refuses
otherwise, because a second pass would duplicate every object.

## Verify

    bash .gen/typecheck.sh                 # real Roslyn, with and without B_LOGS, canary-proven
    node .gen/audit.js <scene|prefab>...   # every serialized field resolves, and to the right type
    node .gen/layout-check.js <scene>... --canary   # rects at four device shapes
    node .gen/final-scan.js                # the rules no compiler and no gate enforces
    bash /c/AndroidWorkspace/scripts/lib/unity-gates.sh <projectDir>

`harness/` compiles the REAL `EmberLevelGenerator.cs` and `ChamberConfig.cs` against tiny
`UnityEngine` stubs and fuzzes them over 2000 seeds per chamber, printing distributions
rather than a pass counter. It is what caught the first chamber table: with equal stone
counts on the two pans every high target is a mirror split, the variety guard rejects it,
and the target band silently collapsed from seven values to five.

    dotnet run --project harness/harness.csproj -c Release

## Things measured on this ticket, not assumed

* `Loader.unity` md5 `f075a8eb02fcdb732d9ee08ce09a46bb` — a fourth distinct copy of this
  template family. Every hardcoded fileID still held; `assertTemplate()` checks each one's
  class and class identifier before anything is written through it.
* `ControllerLoader` sits on its own `loader` root under `PanelLoader` (mono `1836528986`),
  not on `SceneFlow`, and shipped `slider_wait_time: 30`.
* `SceneFlow` carries two components whose scripts are in no `.meta` (`BootFlow`,
  `PageHost`). They are template debt on an object that must not be touched.
* `ControllerMenu` DOES hold `button_ui_main_play`, but `InitUI()` only ever wires the
  privacy button, so the play button's listener is ours.
* `ca9f5fa9…` → `2bcd2660…` is provable rather than remembered: the scenes' eleven action
  fileIDs are exactly the sub-asset ids inside `Assets/Settings/InputSystem_Actions.inputactions`.
  `ProjectSettings` named a third, equally dead guid (`cb681eff…`) and was repointed too.
