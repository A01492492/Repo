using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    void Awake() 
    {
        DontDestroyOnLoad(gameObject);
    }
}
