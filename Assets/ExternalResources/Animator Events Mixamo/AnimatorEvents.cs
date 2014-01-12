/*
	MIXAMO Mecanim Animator Event System      
	Copyright Mixamo www.mixamo.com 2013 Created by Oliver Barraza
	
	The product is delivered AS-IS. Mixamo will not provide support for this product
	If you would like to give feedback we welcome it at Support@Mixamo.com but we cannot support
	all questions about features and opperations of this script.
	
	We do plan to periodically update the script. If you've added something cool you want to share with
	other users of the script you can also send your updates to Support@Mixamo.com and we'll be happy
	to look them over and add them into a future update!
	
	USE:
	1. Attach this script to any GameObject with an Animator component.
	2. Tag the STATES in the Animator Controller you wish to set events on.
	3. Set an Event and Enjoy.
*/

using System;
using UnityEngine;
using System.Collections;

public enum AE_EventType {Instantiate, FireMethod}
public enum Rotation{ NONE, MatchTarget, Random_X, Random_Y, Random_Z, Random_XY, Random_XZ, Random_YZ, Random_XYZ }

[System.Serializable]
public class AnimatorEvents : MonoBehaviour
{
	
	#region Fields
	public AnimEvent[] Events;
	
	private AnimatorStateInfo stateInfo;
	private Animator animator;
	#endregion

	#region Unity Events
	void Awake ()
	{
		foreach (AnimEvent item in Events) item.Init ();
		animator = GetComponent<Animator> ();
	}

	void Update ()
	{	
		foreach (AnimEvent animEvent in Events) {
			stateInfo = animator.GetCurrentAnimatorStateInfo (animEvent.layer);

			if (stateInfo.IsTag (animEvent.animationTag)) {
				if (stateInfo.normalizedTime >= animEvent.fireTime) {
					if (animEvent.eventType == AE_EventType.Instantiate)
						CreateInstance (animEvent);
					else if (animEvent.eventType == AE_EventType.FireMethod)
						FireMethod (animEvent);
				}
			} else animEvent.Reset ();
		}
	}
	#endregion
	
	#region Methods
	/// <summary>
	/// Creates an instance.
	/// </summary>
	/// <param name='animEvent'>
	/// Animation event.
	/// </param>
	private void CreateInstance (AnimEvent animEvent)
	{
		Quaternion rot = GenerateRandomRotation (animEvent);
		animEvent.fireTime++;
		
		if (animEvent.castToSurface == false)
			Instantiate (animEvent.effect, animEvent.target.position, rot);
		else {
			RaycastHit hit;
			Physics.Raycast (animEvent.target.position, -Vector3.up, out hit, 5);
			Instantiate (animEvent.effect, hit.point, rot);
		}
	}
	
	/// <summary>
	/// Fires a method based on the string given when setting up the event array.
	/// </summary>
	/// <param name='animEvent'>
	/// Animation event.
	/// </param>
	private void FireMethod (AnimEvent animEvent)
	{
		Debug.Log ("FireMethod: " + animEvent.ToString() );
		animEvent.fireTime++;
		animEvent.target.SendMessage (animEvent.MethodCall);
	}
	
	/// <summary>
	/// Generates a random rotation.
	/// </summary>
	/// <returns>
	/// A random quaternion rotation.
	/// </returns>
	/// <param name='animEvent'>
	/// Animation event.
	/// </param>
	private Quaternion GenerateRandomRotation (AnimEvent animEvent)
	{
		Quaternion quaternion = Quaternion.identity;
		
		switch (animEvent.rotation) {
		case Rotation.MatchTarget:
			quaternion = animEvent.target.rotation;
			break;
		case Rotation.Random_X:
			quaternion = Quaternion.Euler (new Vector3 (UnityEngine.Random.Range (0f, 361f), 0, 0));
			break;
		case Rotation.Random_Y:
			quaternion = Quaternion.Euler (new Vector3 (0, UnityEngine.Random.Range (0f, 361f), 0));
			break;
		case Rotation.Random_Z:
			quaternion = Quaternion.Euler (new Vector3 (0, 0, UnityEngine.Random.Range (0f, 361f)));
			break;
		case Rotation.Random_XY:
			quaternion = Quaternion.Euler (new Vector3 (UnityEngine.Random.Range (0f, 361f), UnityEngine.Random.Range (0f, 361f), 0));
			break;
		case Rotation.Random_XZ:
			quaternion = Quaternion.Euler (new Vector3 (UnityEngine.Random.Range (0f, 361f), 0, UnityEngine.Random.Range (0f, 361f)));
			break;
		case Rotation.Random_YZ:
			quaternion = Quaternion.Euler (new Vector3 (0, UnityEngine.Random.Range (0f, 361f), UnityEngine.Random.Range (0f, 361f)));
			break;
		case Rotation.Random_XYZ:
			quaternion = Quaternion.Euler (new Vector3 (UnityEngine.Random.Range (0f, 361f), UnityEngine.Random.Range (0f, 361f), UnityEngine.Random.Range (0f, 361f)));
			break;
		}
		return quaternion;
	}
}
	#endregion