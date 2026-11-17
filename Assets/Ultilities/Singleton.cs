using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindFirstObjectByType(typeof(T));
                if (_instance == null)
                    SetUpInstance();
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        RemoveDuplicates();
    }

    private void RemoveDuplicates()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private static void SetUpInstance()
    {
        GameObject newGo = new GameObject();
        newGo.name = typeof(T).Name;
        _instance = newGo.AddComponent<T>();
        DontDestroyOnLoad(newGo);
    }
}
