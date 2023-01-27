using Unity.VisualScripting;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    public const string gameObjectName = "Manager";

    private static T instance;
    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                GameObject gameObject = GameObject.Find(gameObjectName);
                if (null == gameObject)
                {
                    gameObject = new GameObject();
                    gameObject.name = gameObjectName;
                    gameObject.AddComponent<T>();
                }
                instance = gameObject.GetOrAddComponent<T>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
