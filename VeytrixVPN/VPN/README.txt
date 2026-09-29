Veytrix VPN - WireGuard Configuration
=====================================

Place your WireGuard .conf files in this directory.

Each file must be named <ServerId>.conf
(for example: se-stockholm-001.conf)

Example configuration:

[Interface]
PrivateKey = YOUR_CLIENT_PRIVATE_KEY
Address = 10.20.0.2/32
DNS = 10.20.0.1

[Peer]
PublicKey = YOUR_SERVER_PUBLIC_KEY
AllowedIPs = 0.0.0.0/0, ::/0
Endpoint = YOUR_SERVER_HOSTNAME:51820
PersistentKeepalive = 25
