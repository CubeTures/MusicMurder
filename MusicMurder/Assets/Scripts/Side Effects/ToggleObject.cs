using UnityEngine;

public class ToggleObject : MonoBehaviour
{
    [SerializeField] GameObject obj;

    public void Toggle()
    {
        obj.SetActive(!obj.activeInHierarchy);
    }
}
