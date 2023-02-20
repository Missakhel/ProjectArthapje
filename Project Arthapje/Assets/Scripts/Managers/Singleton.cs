using UnityEngine;
using System.Collections;

//Class that will be used as Singleton.
//Be careful with this class, must be used only for managers.
//      public class MyClass : MonoBehaviourSingleton<MyClass>{}
//      MyClass.Instance.foo();
public class MonoBehaviourSingleton<T> : MonoBehaviour
    where T : Component //Can only be Compon
{
    //The only instance that must exist
    private static T _instance = null;
    //Instance Getter. Must be used to use T class only instance.
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                //Look if T exist and if it exist more than 1 time it'll log an error. 
                T[] objs = FindObjectsOfType<T>();

                if (objs.Length > 0)
                {
                    _instance = objs[0];
                }

                //Must be very careful treating with this error if happens
                if (objs.Length > 1)
                {
                    Debug.LogError(string.Format("There is more than one {0} in the scene.", typeof(T).Name));
                }

                if (_instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    _instance = obj.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning(string.Format("There is more than one {0} deleting this {1}", typeof(T).Name, gameObject.name));
            Destroy(gameObject);
        }
    }
}