using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Check if there's already an instance in the scene
                _instance = FindFirstObjectByType<T>();

                // If not, create a new GameObject and attach the singleton component
                if (_instance == null)
                {
                    GameObject singletonObject = new(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }

                // Make sure the instance won't be destroyed when loading a new scene
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        // If an instance already exists and it's not this one, destroy this one
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
