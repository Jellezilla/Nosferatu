using UnityEngine;

//Singleton Implementation

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool appQuiting = false;
    private static object container = new object();

    public static T Instance
    {
        get
        {
            if (appQuiting)
            {
                return null;
            }

            lock (container)
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        return instance;
                        //we have more than one instance, WHICH IS BAD!;
                    }

                    if (instance == null)
                    {
                        GameObject singleton = new GameObject();
                        instance = singleton.AddComponent<T>();
                        singleton.name = "[Singleton] " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("Autocreating an instance of " + typeof(T));
                    }
                }

                return instance;
            }
        }
    }

    

    public void OnDestroy()
    {
        appQuiting = true;
    }
}
