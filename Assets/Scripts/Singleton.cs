using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool isApplicationQuitting = false;
    
    public static T Instance
    {
        get
        {
            if (!instance && !isApplicationQuitting)
            {
                instance = FindObjectOfType(typeof( T)) as T;
                if (!instance)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }
    
    private void OnApplicationQuit()
    {
        instance = null;
        Destroy(gameObject);
        isApplicationQuitting = true;
    }
}