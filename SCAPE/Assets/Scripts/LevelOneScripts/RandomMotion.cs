using UnityEngine;
using System.Collections;

public class RandomMotion : MonoBehaviour
{
	public float m_frameRate = 5.0f;
	public int m_force = 50;
	
	// Use this for initialization
	void Start() 
	{
		//m_originalPosition = rigidbody.position;
	}
	
	void Awake()
	{
		Debug.LogError("Starting InvokeRepeating");
		InvokeRepeating("ApplyForce", 2, m_frameRate);
	}
	
	// Update is called once per frame
	void ApplyForce() 
	{
		m_counter %= 6;
		
		Debug.LogError("Applying Force" + m_counter);
		
		switch(m_counter)
		{
			case 0:
				m_forceDirection = transform.up;
				break;
			case 1:
				m_forceDirection = transform.right;
				break;
			case 2:
				m_forceDirection = transform.forward;
				break;
			case 3:
				m_forceDirection = -transform.up;
				break;
			case 4:
				m_forceDirection = -transform.right;
				break;
			case 5:
				m_forceDirection = -transform.forward;
				break;
		}
		
		rigidbody.AddForce(m_forceDirection * m_force * rigidbody.mass, ForceMode.Impulse);
		
		m_counter++;
	}
		
	//private Vector3 m_originalPosition;
	private Vector3 m_forceDirection;
	private int m_counter = 0;

}
