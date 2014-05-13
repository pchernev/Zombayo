using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent( typeof(FingerDownDetector))]
[RequireComponent( typeof(FingerMotionDetector))]
[RequireComponent( typeof(FingerUpDetector))]

public class KickSwipe : MonoBehaviour
{
	public TrailRenderer trail;
	public float minAngle;
	public float maxAngle;
	private float startTime;
	private Vector2 startPos;
	private Vector3 minDirection;
	private Vector3 maxDirection;

	void Start ()
	{
		minDirection = Quaternion.AngleAxis (minAngle, Vector3.forward) * Vector3.right;
		maxDirection = Quaternion.AngleAxis (maxAngle, Vector3.forward) * Vector3.right;
	}

	void OnFingerDown (FingerDownEvent e)
	{
		trail.time = 100;
		startTime = Time.time;

		startPos = e.Finger.Position;
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (e.Finger.Position.x, e.Finger.Position.y, 1));
	}

	void OnFingerMove (FingerMotionEvent e)
	{
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (e.Finger.Position.x, e.Finger.Position.y, 1));
	}

	void OnFingerUp (FingerUpEvent e)
	{
		transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (e.Finger.Position.x, e.Finger.Position.y, 1));
		trail.time = Time.time - startTime;

		Vector3 direction = (e.Finger.Position - startPos);
		if (direction.sqrMagnitude < 1)
			return;

		direction.Normalize ();
		float angle = Vector2.Angle (Vector2.right, direction);
		if (direction.y <= 0 || angle < minAngle)
			direction = minDirection;
		else if (direction.x <= 0 || maxAngle < angle)
			direction = maxDirection;

		GameLogic.Instance.initiateKickInCustomDirection (direction);
		angle = Vector2.Angle (Vector2.right, direction);
	}
}
