using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager Instance;

    private int _score;
    private Dictionary<int, ScoreData> _scores = new();

    private int _halfDevider = 2;

    public int Score => _score;

    // 외부에서 수정하지 못하도록 ReadOnlyDictionary로 반환한다.
    public ReadOnlyDictionary<int, ScoreData> Scores => new ReadOnlyDictionary<int, ScoreData>(_scores);

    private List<int> _numbers = new List<int>();
    public ReadOnlyCollection<int> Numbers => _numbers.AsReadOnly();

    public static event Action OnDataChanged;
    public static event Action<int> OnScoreChanged;

    [SerializeField] private TotalScoreUI _totalScoreUI;

    private void Awake()
    {
        Instance = this;
    }

    public override void OnJoinedRoom()
    {
        // 방에 들어가면 내 점수를 0점으로 초기화한다.
        Refresh();
    }

    // [데이터 공유]
    // 1. OnSerializeView (+TransformView, AnimatorView, ...)
    //    ㄴ C# 기본 타입, Vector
    //    ㄴ PhotonNetwork... Rate...에 따라.

    // 2. RPC -> 매개변수를 활용해서 데이터 동기화
    //    ㄴ 주로 변화가 빈번하지 않은 데이터를 함수 호출을 이용해서 동기화한다.

    // 3. 커스텀 프로퍼티(Custom Property)
    //    ㄴ 주로 변화가 빈번하지 않은 데이터들을 해시 테이블로 동기화한다.
    //    ㄴ 플레이어 준비 상태, 점수, 룸의 모드, 맵 선택 등...

    public void AddScore(int score)
    {
        if (score <= 0) return;
        _score += score;
        OnScoreChanged?.Invoke(_score);
        Refresh();
    }

    public void HalfScore()
    {
        if (_score <= 0) return;
        _score /= _halfDevider;
        OnScoreChanged?.Invoke(_score);
        Refresh();
    }

    private void Refresh()
    {
        // 해시테이블은 딕셔너리와 같은 키-값 형태로 저장한다. 다만 키-값에 있어서 자료형이 object이다.
        Hashtable hashtable = new Hashtable();
        hashtable.Add("score", _score);

        // 프로퍼티를 등록한다.
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }

    // 플레이어의 커스텀 프로퍼티가 변경되면 자동으로 호출되는 함수이다.
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("score")) return;

        ScoreData scoreData = new ScoreData
        {
            Nickname = targetPlayer.NickName,
            Score = (int)changedProps["score"]
        };

        _scores[targetPlayer.ActorNumber] = scoreData;

        OnDataChanged?.Invoke();
    }
}
