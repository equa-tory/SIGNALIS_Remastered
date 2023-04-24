using UnityEngine;
using TMPro;

public class VersionText : MonoBehaviour
{
    private void Start()
    {
        GetComponent<TMP_Text>().text = "v." + Application.version;
    }
}
