using UnityEngine;
using System.Collections;

public class LevelPreLoader : MonoBehaviour {

    [SerializeField]
    private string m_levelName;

    void Start()
    {
        Application.LoadLevel(m_levelName);
    }
	
}
