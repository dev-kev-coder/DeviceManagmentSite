# AgentInstaller.Setup — Quick Build & Tooling

## One‑time repo boot‑strap (already done)

```bash
# Run once, then commit .config/dotnet-tools.json
dotnet new tool-manifest     # creates the local tool manifest
dotnet tool install wix      # adds WiX v4 CLI to the manifest
```

The generated **.config/dotnet-tools.json** is now under source control, so
no one needs to install WiX globally.

---

## Day‑to‑day developer / CI workflow

```bash
git clone <repo>
cd <repo>
dotnet tool restore                # installs wix into a local cache

dotnet build AgentInstaller.Setup/AgentInstaller.Setup.wixproj -c Release
```

*Output →* `AgentInstaller.Setup/bin/Release/net8.0/AgentInstaller.Setup.msi`

### Silent install on a target machine

```bash
msiexec /i AgentInstaller.Setup.msi /qn
```

### Clean build artifacts

```bash
dotnet clean AgentInstaller.Setup/AgentInstaller.Setup.wixproj
```

---

## Project skeleton recap

* `AgentInstaller.Setup.wixproj` — `<Project Sdk="WixToolset.Sdk/4.0.0" />`
* `Product.wxs` — package metadata & service registration (stub for now)
* `build-setup.ps1` or `build-setup.sh` — convenience script:

  ```
  dotnet tool restore
  dotnet build AgentInstaller.Setup/AgentInstaller.Setup.wixproj -c Release
  ```

*Flip `-c Release` to `Debug` for faster local iterations.*
