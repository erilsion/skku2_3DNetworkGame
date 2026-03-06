using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private void Update()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
