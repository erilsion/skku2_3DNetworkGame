using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private List<ScoreItemUI> _items;

    private void Start()
    {
        _items = GetComponentsInChildren<ScoreItemUI>().ToList();
    }

    private void Refresh()
    {

    }
}
