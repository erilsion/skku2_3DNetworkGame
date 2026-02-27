using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

public class PlayerRegistry : MonoBehaviour
{
    public static PlayerRegistry Instance { get; private set; }

    private readonly Dictionary<int, Transform> _players = new Dictionary<int, Transform>();

    public IReadOnlyDictionary<int, Transform> Players => _players;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        if (PhotonRoomManager.Instance != null)
        {
            PhotonRoomManager.Instance.OnRoomJoined += RegisterExistingPlayers;
            PhotonRoomManager.Instance.OnPlayerEnter += HandlePlayerEnter;
            PhotonRoomManager.Instance.OnPlayerLeft += HandlePlayerLeft;
            PhotonRoomManager.Instance.OnMasterClientChanged += RebuildRegistry;
        }
    }

    private void OnDisable()
    {
        if (PhotonRoomManager.Instance != null)
        {
            PhotonRoomManager.Instance.OnRoomJoined -= RegisterExistingPlayers;
            PhotonRoomManager.Instance.OnPlayerEnter -= HandlePlayerEnter;
            PhotonRoomManager.Instance.OnPlayerLeft -= HandlePlayerLeft;
            PhotonRoomManager.Instance.OnMasterClientChanged -= RebuildRegistry;
        }
    }

    private void RegisterExistingPlayers()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            TryRegisterPlayer(player);
        }
    }

    private void HandlePlayerEnter(Player player)
    {
        TryRegisterPlayer(player);
    }

    private void HandlePlayerLeft(Player player)
    {
        if (_players.ContainsKey(player.ActorNumber))
        {
            _players.Remove(player.ActorNumber);
        }
    }

    private void TryRegisterPlayer(Player player)
    {
        PlayerNetworkIdentity[] identities = FindObjectsByType<PlayerNetworkIdentity>(FindObjectsSortMode.None);

        foreach (var identity in identities)
        {
            if (identity.ActorNumber == player.ActorNumber)
            {
                _players[player.ActorNumber] = identity.transform;
                break;
            }
        }
    }

    private void RebuildRegistry()
    {
        _players.Clear();

        var identities = FindObjectsByType<PlayerNetworkIdentity>(FindObjectsSortMode.None);

        foreach (var identity in identities)
        {
            _players[identity.ActorNumber] = identity.transform;
        }
    }

    public Transform GetClosestPlayer(Vector3 position)
    {
        if (!PhotonNetwork.IsMasterClient) return null;
        
        float minDistance = float.MaxValue;
        Transform closest = null;

        foreach (var player in _players)
        {
            if (player.Value == null) continue;

            float distance = Vector3.Distance(position, player.Value.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = player.Value;
            }
        }

        return closest;
    }

    public void Register(PlayerNetworkIdentity identity)
    {
        _players[identity.ActorNumber] = identity.transform;
    }

    public void Unregister(PlayerNetworkIdentity identity)
    {
        if (_players.ContainsKey(identity.ActorNumber))
        {
            _players.Remove(identity.ActorNumber);
        }
    }
}
