using UnityEngine;
using System.Collections;

public class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour {

    private static T sm_instance;

    public static T Instance
    {
        get { return sm_instance; }
    }

    protected virtual void Awake()
    {
        if(sm_instance != null)
        {
            Debug.LogError("instance of " + name + " spawned more than once");
        }
        sm_instance = this.GetComponent<T>();
        DontDestroyOnLoad(sm_instance);
    }

}
