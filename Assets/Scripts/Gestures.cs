using UnityEngine;
using System.Collections;

public class Gestures : MonoBehaviour
{
	private GameObject player;
	private GameObject doctor;

	void Start()
	{
		player = GameObject.FindWithTag( "Player" );
		doctor = GameObject.Find( "Dr.Fishhead" );
	}
	
	void OnSwipe( SwipeGesture gesture )
	{
		if( doctor != null )
		{
			doctor.SendMessage( "SetStartState" );
		}
	}
}
