# VeytrixVPN

WPF (.NET 8) Windows desktop VPN client for WireGuard.

## Platform

- **Windows 10/11** — supported (WPF + .NET 8 Desktop runtime)
- **macOS** — not supported by this project (WPF is Windows-only)
- **Linux** — not supported

The Base44 sandbox is Linux-based, so this app **cannot run or preview here**. Build and run on a Windows machine.

## Build

```powershell
dotnet restore
dotnet build -c Release
```

## Publish (self-contained single-file)

```powershell
dotnet publish -c Release -r win-x64 --self-contained true
```

Output: `bin\Release\net8.0-windows\win-x64\publish\VeytrixVPN.exe`

## Project Structure

```
VeytrixVPN/
├── VeytrixVPN.csproj
├── App.xaml / App.xaml.cs
├── MainWindow.xaml / MainWindow.xaml.cs
├── Models/
│   ├── VpnServer.cs
│   └── VpnSettings.cs
├── Services/
│   ├── SettingsService.cs
│   ├── ServerService.cs
│   ├── WireGuardService.cs
│   ├── PingService.cs
│   ├── BlockListService.cs
│   ├── IntegrityService.cs
│   └── LicenseService.cs
├── Views/
│   └── AboutWindow.xaml / .cs
├── Data/
│   ├── servers.json
│   ├── adblock.txt
│   └── trackers.txt
├── VPN/
│   └── README.txt
└── Assets/
    └── VeytrixVPN.ico
```

## Key Details

- **servers.json** — server list loaded at startup. Replace placeholder values (`YOUR_SERVER_IPV4`, `YOUR_SERVER_PUBLIC_KEY`, etc.) with real WireGuard server data.
- **VPN/ folder** — place WireGuard `.conf` files here, named `<ServerId>.conf` (e.g. `se-stockholm-001.conf`). The app looks for these when connecting.
- **WireGuard** must be installed at `C:\Program Files\WireGuard\wireguard.exe` for tunnel install/uninstall to work.
- **Settings** are stored in `%LOCALAPPDATA%\VeytrixVPN\settings.json`.
- **Blocklists** (`adblock.txt`, `trackers.txt`) use uBlock-style `||domain^` format; rule counts are shown in the connection panel.
- **PingService** tests latency for the first 25 servers only to avoid network flooding.
- **IntegrityService** provides SHA-256 file verification helpers.
- **LicenseService** holds copyright/ownership metadata for Veytrix Games.

## Verifying

Since the app can't run in the sandbox, verify on Windows:
1. `dotnet build` compiles without errors
2. App launches, loads `servers.json`, shows server count in status bar
3. Search filters the server list by country/city/hostname
4. Auto Select picks the lowest-load server
5. Ping Servers tests latency for first 25 servers
6. Connect button checks for WireGuard and config file, shows appropriate status
7. About window opens from the header button
