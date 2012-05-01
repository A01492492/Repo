using UnityEngine;
using System.Collections;

public class Vibrate : MonoBehaviour 
{
		
	// Use this for initialization
	void Start () 
	{	
		m_originalPosition = rigidbody.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		m_counter %= 3;
		
		switch(m_counter)
		{
			case 0:
				m_offsetPosition.x = m_originalPosition.x + m_offset;
				m_offsetPosition.y = m_originalPosition.y;
				m_offsetPosition.z = m_originalPosition.z;
				break;
			case 1:
				m_offsetPosition.x = m_originalPosition.x ;
				m_offsetPosition.y = m_originalPosition.y + m_offset;
				m_offsetPosition.z = m_originalPosition.z;
				break;
			case 2:
				m_offsetPosition.x = m_originalPosition.x ;
				m_offsetPosition.y = m_originalPosition.y;
				m_offsetPosition.z = m_originalPosition.z + m_offset;
				break;
		}
		
		m_counter ++;
	}
	
	void FixedUpdate()
	{
		rigidbody.MovePosition(m_offsetPosition);
	}
	
	public float m_offset = 0.01f;
	
	private Vector3 m_originalPosition;
	private Vector3 m_offsetPosition = new Vector3(0, 0, 0);
	
	private int m_counter = 0;
			
}
