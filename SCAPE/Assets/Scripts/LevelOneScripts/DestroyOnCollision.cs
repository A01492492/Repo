using UnityEngine;
using System.Collections;

public class DestroyOnCollision : MonoBehaviour 
{
    

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
