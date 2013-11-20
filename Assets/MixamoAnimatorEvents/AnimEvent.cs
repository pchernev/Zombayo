using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnimEvent : System.Object
{
	public string animationTag;
	public int layer = 0;
	public float fireTime;
	public AE_EventType eventType;
	//public bool repeat = true; NOT Implemented
	public GameObject effect;
	public string MethodCall = "NULL";
	public Transform target;
	public bool castToSurface;
	public Rotation rotation;

	public float DefaultFireTime { get { return defaultFireTime; } }
	
	private float defaultFireTime;
	
	public void Init ()
	{
		defaultFireTime = fireTime;
	}
	
	public void Reset ()
	{
		fireTime = defaultFireTime;
	}
}
