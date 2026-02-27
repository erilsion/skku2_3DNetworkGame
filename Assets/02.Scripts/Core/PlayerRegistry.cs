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
        }
    }

    private void OnDisable()
    {
        if (PhotonRoomManager.Instance != null)
        {
            PhotonRoomManager.Instance.OnRoomJoined -= RegisterExistingPlayers;
            PhotonRoomManager.Instance.OnPlayerEnter -= HandlePlayerEnter;
            PhotonRoomManager.Instance.OnPlayerLeft -= HandlePlayerLeft;
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

    public Transform GetPlayerTransform(int actorNumber)
    {
        _players.TryGetValue(actorNumber, out Transform result);
        return result;
    }

    public Transform GetClosestPlayer(Vector3 position)
    {
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
}
