using System.Windows;
using System.Windows.Controls;
using VeytrixVPN.Models;
using VeytrixVPN.Services;
using VeytrixVPN.Views;

namespace VeytrixVPN;

public partial class MainWindow : Window
{
    private readonly ServerService serverService = new();
    private readonly PingService pingService = new();
    private readonly SettingsService settingsService = new();
    private readonly BlockListService blockListService = new();
    private readonly WireGuardService wireGuardService = new();

    private List<VpnServer> allServers = new();

    private VpnServer? selectedServer;

    private VpnSettings settings = new();

    private bool connected;

    public MainWindow()
    {
        InitializeComponent();

        settings = settingsService.Load();

        Loaded += MainWindow_Loaded;
    }

    private async void MainWindow_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            allServers =
                await serverService.LoadServersAsync();

            ServerList.ItemsSource =
                allServers;

            AdBlockCheckBox.IsChecked =
                settings.AdBlock;

            TrackerCheckBox.IsChecked =
                settings.TrackerBlock;

            AutoReconnectCheckBox.IsChecked =
                settings.AutoReconnect;

            KillSwitchCheckBox.IsChecked =
                settings.KillSwitch;

            IPv6CheckBox.IsChecked =
                settings.IPv6;

            int ads =
                blockListService.CountRules(
                    "adblock.txt");

            int trackers =
                blockListService.CountRules(
                    "trackers.txt");

            RuleCountText.Text =
                $"Blocklists: {ads:N0} ads / {trackers:N0} trackers";

            StatusText.Text =
                $"{allServers.Count:N0} servers loaded.";

            if (!string.IsNullOrWhiteSpace(
                settings.LastServerId))
            {
                selectedServer =
                    allServers.FirstOrDefault(
                        x => x.Id ==
                        settings.LastServerId);

                if (selectedServer != null)
                    ServerList.SelectedItem =
                        selectedServer;
            }
        }
        catch (Exception ex)
        {
            StatusText.Text =
                $"Startup error: {ex.Message}";
        }
    }

    private void SearchBox_TextChanged(
        object sender,
        TextChangedEventArgs e)
    {
        string search =
            SearchBox.Text.Trim();

        if (string.IsNullOrWhiteSpace(search))
        {
            ServerList.ItemsSource =
                allServers;

            return;
        }

        ServerList.ItemsSource =
            allServers
                .Where(server =>
                    $"{server.Country} " +
                    $"{server.City} " +
                    $"{server.Hostname}"
                    .Contains(
                        search,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
    }

    private void ServerList_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        selectedServer =
            ServerList.SelectedItem
            as VpnServer;

        if (selectedServer == null)
            return;

        SelectedServerText.Text =
            $"{selectedServer.Country} — " +
            $"{selectedServer.City}\n" +
            $"{selectedServer.Hostname}\n\n" +
            $"Ping: {selectedServer.PingText}\n" +
            $"Load: {selectedServer.Load}%";
    }

    private void AutoSelect_Click(
        object sender,
        RoutedEventArgs e)
    {
        selectedServer =
            allServers
                .OrderBy(x =>
                    x.Load)
                .ThenBy(x =>
                    x.Ping < 0
                        ? int.MaxValue
                        : x.Ping)
                .FirstOrDefault();

        if (selectedServer == null)
        {
            StatusText.Text =
                "No servers available.";

            return;
        }

        ServerList.SelectedItem =
            selectedServer;

        StatusText.Text =
            $"Selected {selectedServer.DisplayName}";
    }

    private async void PingServers_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (allServers.Count == 0)
            return;

        StatusText.Text =
            "Testing server latency...";

        foreach (VpnServer server
                 in allServers.Take(25))
        {
            server.Ping =
                await pingService
                    .GetPingAsync(server);
        }

        ServerList.Items.Refresh();

        StatusText.Text =
            "Latency test completed.";
    }

    private async void ConnectButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        if (selectedServer == null)
        {
            ConnectionStatus.Text =
                "Select a server first.";

            return;
        }

        if (connected)
        {
            connected = false;

            ConnectionStatus.Text =
                "Disconnected";

            ConnectButton.Content =
                "CONNECT";

            StatusText.Text =
                "VPN disconnected.";

            return;
        }

        settings.AdBlock =
            AdBlockCheckBox.IsChecked == true;

        settings.TrackerBlock =
            TrackerCheckBox.IsChecked == true;

        settings.AutoReconnect =
            AutoReconnectCheckBox.IsChecked == true;

        settings.KillSwitch =
            KillSwitchCheckBox.IsChecked == true;

        settings.IPv6 =
            IPv6CheckBox.IsChecked == true;

        settings.LastServerId =
            selectedServer.Id;

        settingsService.Save(settings);

        if (!wireGuardService.IsInstalled())
        {
            ConnectionStatus.Text =
                "WireGuard not installed.";

            StatusText.Text =
                "Install WireGuard for Windows first.";

            return;
        }

        string config =
            Path.Combine(
                AppContext.BaseDirectory,
                "VPN",
                selectedServer.Id + ".conf");

        if (!File.Exists(config))
        {
            ConnectionStatus.Text =
                "Configuration missing.";

            StatusText.Text =
                $"Missing: {config}";

            return;
        }

        try
        {
            ConnectionStatus.Text =
                "Connecting...";

            bool success =
                await wireGuardService
                    .InstallTunnelAsync(config);

            if (success)
            {
                connected = true;

                ConnectionStatus.Text =
                    "Connected";

                ConnectButton.Content =
                    "DISCONNECT";

                StatusText.Text =
                    $"Connected to {selectedServer.DisplayName}";
            }
            else
            {
                ConnectionStatus.Text =
                    "Connection failed";

                StatusText.Text =
                    "WireGuard tunnel installation failed.";
            }
        }
        catch (Exception ex)
        {
            ConnectionStatus.Text =
                "Connection failed";

            StatusText.Text =
                ex.Message;
        }
    }

    private void About_Click(
        object sender,
        RoutedEventArgs e)
    {
        AboutWindow about = new()
        {
            Owner = this
        };

        about.ShowDialog();
    }
}
