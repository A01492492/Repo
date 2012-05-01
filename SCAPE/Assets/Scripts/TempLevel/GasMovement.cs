using UnityEngine;
using System.Collections;

/// <summary>
/// This scripts controls the movement of the molecule. It handles the angles and speed of
/// molecules.
/// @reference - http://en.wikipedia.org/wiki/Specular_reflection
/// </summary>
public class GasMovement : MonoBehaviour
{
    /// <summary>
    /// Represents the methods to set the speed of the molecule
    /// </summary>
    /// <param name="speed"> The speed of the molecule</param>
    internal void SetSpeed(float speed)
    {
        m_speed = speed;
    }

    /// <summary>
    /// Initilizes a random velocity for the molecule when it is instantiated. 
    /// 
    /// Note - The magnitude of all the random velocities generated will be (2^2 + 2^2 + 2^2)^ 1/2
    ///        but the direction will be different(governed by the vector rules)
    /// </summary>
    void Awake()
    {
        int x = -1;
        int y = -1;
        int z = -1;

        // Change the sign of x depending upon the sign of random number 
        if (Random.Range(-2, 2) > 0)
            x = 1;

        if (Random.Range(-2, 2) > 0)
            y = 1;

        if (Random.Range(-2, 2) > 0)
            z = 1;

        m_velocity = new Vector3(x * 2, y * 2, z * 2);
    }

    
    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculate the new velocity
        m_end = m_velocity * Time.deltaTime * m_speed;

        // Set the new velocity
        transform.position += m_end;        
    }

    
    void OnCollisionEnter(Collision collision)
    {        
        //m_velocity = rigidbody.velocity;

        /*foreach (ContactPoint contact in collision.contacts) 
        {
            m_velocity = (Vector3.Dot(m_velocity, Vector3.Normalize(contact.normal))) * Vector3.Normalize(contact.normal) - m_velocity;
            m_velocity *= -1;
        }*/

        m_velocity *= -1;
    }
    
    /*void OnCollisionStay(Collision collision) 
    {
        Debug.LogError("OnCollisionStay" + m_speed);
        if (m_speed == 2)
        {            
            Vector3 tempForce;

            foreach (ContactPoint contact in collision.contacts)
            {
                tempForce = .01f * contact.normal;
                rigidbody.AddForce(tempForce);
            }
        }
        
    }*/

    //default speed
	private float m_speed = .1f;

    //private RaycastHit m_hit = new RaycastHit();   
    
    //private bool m_isColliding = false;

    private float m_radius;
	
    private Vector3 m_velocity;
  
    private Vector3 m_end;
}