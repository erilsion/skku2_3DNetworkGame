using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 플레이어의 대표로서 외부와의 소통 또는 어빌리티들을 관리하는 역할이다.
public class PlayerController : MonoBehaviour, IPunObservable
{
    public PlayerStat Stat;
    public PhotonView PhotonView;

    private void Awake()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    // 데이터 동기화를 위한 데이터 읽기(전송), 쓰기(수신) 메서드이다.
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 스트림: '시냇물'처럼 데이터가 멈추지 않고 연속적으로 흐르는 데이터의 흐름이다.
        // 서버에서 주고 받을 데이터가 담겨있는 변수이다.

        // 읽기 / 쓰기 모드가 있다.
        if (stream.IsWriting)
        {
            // 이 PhotonView의 데이터를 보내줘야 하는 상황일 때.
            // Debug.Log("전송중");
            stream.SendNext(Stat.Health);   // 현재 체력
            stream.SendNext(Stat.Stamina);  // 현재 스태미너
        }
        else if (stream.IsReading)
        {
            // 이 PhotonView의 데이터를 받아야 하는 상황일 때.
            // Debug.Log("수신중");
            Stat.Health = (float)stream.ReceiveNext();  // 전송한 순서대로 받아와진다.
            Stat.Stamina = (float)stream.ReceiveNext();
        }
    }

    private Dictionary<Type, PlayerAbility> _abilitiesCache = new();

    public T GetAbility<T>() where T : PlayerAbility
    {
        var type = typeof(T);

        if (_abilitiesCache.TryGetValue(type, out PlayerAbility ability))
        {
            return ability as T;
        }

        // 게으른 초기화/로딩 -> 처음에 곧바로 초기화/로딩을 하는게 아니라 필요할때만 하도록 뒤로 미루는 기법
        ability = GetComponent<T>();

        if (ability != null)
        {
            _abilitiesCache[ability.GetType()] = ability;

            return ability as T;
        }

        throw new Exception($"어빌리티 {type.Name}을 {gameObject.name}에서 찾을 수 없습니다.");
    }
}
