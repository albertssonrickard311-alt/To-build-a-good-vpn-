sudo apt update
sudo apt install wireguard iptables -y

wg genkey | tee server_private.key | wg pubkey > server_public.key
wg genkey | tee client_private.key | wg pubkey > client_public.key


sudo nano /etc/sysctl.conf

net.ipv4.ip_forward=1
net.ipv6.conf.all.forwarding=1

sudo sysctl -p

sudo nano /etc/wireguard/wg0.conf


[Interface]
Address = 10.8.0.1/24
ListenPort = 51820
PrivateKey = SERVER_PRIVATE_KEY

PostUp = iptables -A FORWARD -i wg0 -j ACCEPT
PostUp = iptables -A FORWARD -o wg0 -j ACCEPT
PostUp = iptables -t nat -A POSTROUTING -o eth0 -j MASQUERADE

PostDown = iptables -D FORWARD -i wg0 -j ACCEPT
PostDown = iptables -D FORWARD -o wg0 -j ACCEPT
PostDown = iptables -t nat -D POSTROUTING -o eth0 -j MASQUERADE

[Peer]
PublicKey = CLIENT_PUBLIC_KEY
AllowedIPs = 10.8.0.2/32


sudo systemctl enable wg-quick@wg0
sudo systemctl start wg-quick@wg0



sudo wg



[Interface]
PrivateKey = CLIENT_PRIVATE_KEY
Address = 10.8.0.2/24
DNS = 1.1.1.1

[Peer]
PublicKey = SERVER_PUBLIC_KEY
Endpoint = DIN_SERVER_IP:51820
AllowedIPs = 0.0.0.0/0, ::/0
PersistentKeepalive = 25



AllowedIPs = 0.0.0.0/0, ::/0





┌─────────────────────────────────────┐
│           🔐 MY VPN                 │
│                                     │
│            🟢 SKYDDAD               │
│                                     │
│          🇸🇪 Sweden #1               │
│          12 ms                      │
│                                     │
│       ┌─────────────────┐           │
│       │   KOPPLA FRÅN   │           │
│       └─────────────────┘           │
│                                     │
│  🌍 VPN IP                          │
│  185.xxx.xxx.xxx                    │
│                                     │
│  🛡️ Kill Switch          ON         │
│  🔒 DNS Protection       ON         │
│                                     │
│              ⚙️ Inställningar       │
└─────────────────────────────────────┘

embeddable-dll-service

