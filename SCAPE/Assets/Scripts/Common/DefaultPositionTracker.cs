using UnityEngine;
using System.Collections;

/// <summary>
/// Keeps track of the default location of game object
/// </summary>
public class DefaultPositionTracker : MonoBehaviour 
{
    internal Vector3 Position 
    { 
        get 
        {
            return mDefaultPosition; 
        }

        set 
        { 
            mDefaultPosition = value; 
        }
    }

    internal void SetDefault() 
    {
        gameObject.transform.position = mDefaultPosition;        
    }

	// Use this for initialization
	void Start () 
    {
        mDefaultPosition = gameObject.transform.position;	
	}
	
    private Vector3 mDefaultPosition;
}
