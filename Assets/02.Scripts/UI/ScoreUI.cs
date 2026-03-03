using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private List<ScoreItemUI> _items;

    private void Start()
    {
        _items = GetComponentsInChildren<ScoreItemUI>().ToList();

        ScoreManager.OnDataChanged += Refresh;
        Refresh();
    }

    private void Refresh()
    {
        var scores = ScoreManager.Instance.Scores;

        // ReadOnly가 아니면 원본을 수정하게 되므로 무결성에 문제가 생긴다.
        List<ScoreData> scoreDatas = scores.Values.ToList();

        // 1. todo: 1등부터 3등까지 정렬한다. 3명이 있는 지 적절하게 반복문을 진행한다.
        //          - 정렬은 이미 매니저에서 해서 넘긴다 vs 정렬은 UI에서 한다. (도메인 규칙에 따라 다르다.)
        //          - 정리 과제: Linq를 사용한다. (무엇인지, 언제 쓰이는 지, 장단점은 무엇인지)
        // 2. todo: 점수 1,000점마다 플레이어 무기의 scale이 0.1씩 증가한다. (동기화 되어야 한다.)
        // 3. todo: 죽으면 점수의 반을 잃는다.

        // 점수가 높은 사람이 1등이라 Descending(내림차순)을 사용했다. (Ascending(오름차순))
        // UI 개수만큼 Take하기 위해 _items.Count를 사용했다.
        // Linq는 지연 실행이기 때문에, UI에 안정적으로 바인딩하기 위해 ToList()를 붙여 즉시 실행해준다.
        var ranking = scoreDatas.OrderByDescending(data => data.Score).Take(_items.Count).ToList();

        for (int i = 0; i < ranking.Count; i++)
        {
            _items[i].Set(i+1, ranking[i].Nickname, ranking[i].Score);
            _items[i].gameObject.SetActive(true);
        }
    }
}
