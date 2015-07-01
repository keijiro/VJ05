using UnityEngine;

public class GlobalConfig : MonoBehaviour
{
    void Start()
    {
        if (!Application.isEditor) Cursor.visible = false;
    }
}
