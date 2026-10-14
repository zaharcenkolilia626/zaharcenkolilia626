#!/usr/bin/env bash
# Offline Roslyn type-check of Assets/Scripts/Generated, with and without B_LOGS.
# A canary pass proves the compiler really binds our method bodies: csc reports nothing
# inside a body when the declaration phase failed, so a silent "clean" can be a lie.
set -u
cd "$(dirname "$0")/.." || exit 2
U="${UNITY_EDITOR:-C:/Program Files/Unity/Hub/Editor/6000.5.10f1}"
DOTNET="$U/Editor/Data/DotNetSdk/dotnet.exe"
CSC="$U/Editor/Data/DotNetSdk/sdk/8.0.318/Roslyn/bincore/csc.dll"
[ -f "$CSC" ] || { echo "no csc at $CSC"; exit 2; }

run() {
    node .gen/make-rsp.js "$U" "$@" >/dev/null || return 9
    "$DOTNET" "$CSC" "@.gen/typecheck/check.rsp" 2>&1 | grep -E "error CS"
}

fail=0
for mode in "" "--logs"; do
    label="${mode:-nologs}"
    out="$(run $mode)"
    n="$(printf '%s' "$out" | grep -c "error CS")"
    if [ "$n" != 0 ]; then
        echo "[typecheck] $label: $n error(s)"
        printf '%s\n' "$out" | head -25
        fail=1
    else
        echo "[typecheck] $label: clean"
    fi
done

# Canary: a deliberate type error must surface, or "clean" means nothing.
CANARY=Assets/Scripts/Generated/EaseKit.cs
cp "$CANARY" .gen/typecheck/canary.bak
printf '\npublic static class CanaryProbe { public static int Broken() { int x = "not an int"; return x; } }\n' >> "$CANARY"
seen="$(run | grep -c 'CS0029')"
cp .gen/typecheck/canary.bak "$CANARY"
rm -f .gen/typecheck/canary.bak
if [ "$seen" = 0 ]; then
    echo "[typecheck] CANARY DID NOT SURFACE - the check is blind, do not trust it"
    fail=1
else
    echo "[typecheck] canary surfaced (bodies really are bound)"
fi

node .gen/make-rsp.js "$U" >/dev/null
exit $fail
