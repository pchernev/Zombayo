using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
	public enum CameraFollowState {
		Initial, 
		KickInProgress,
		InGame
	};

	public GameObject playerTarget;

	// initial camera
	public Vector3 initialOffset;
	public Vector3 initialRotation;

	// kick in progress camera last position
	public Vector3 kickInProgressOffset;
	public Vector3 kickInProgressRotation;

	// in game camera position
	public Vector3 inGameOffset;
	public Vector3 inGameRotation;


	private Vector3 cameraOffset;
	private Vector3 cameraRotaion;
	private Vector3 nextOffset;
	private Vector3 nextRotaion;
	public float transitionTime;

	private bool transitionInProgress;
	private float transitionStartTime;
	private Vector3 offsetChangeRate;
	private Vector3 rotationChangeRate;

	private float startTime;

//	[HideInInspector]
	public CameraFollowState State {
		get {
			if( Application.isPlaying )
			{
				if( transitionInProgress ) 
					_state = CameraFollowState.KickInProgress;
			}

			return _state;
		}
		set {
			_state = value;

			Debug.Log( "Camera Follow State: " + _state.ToString()  );

			if( Application.isPlaying )
			{
				if( _state == CameraFollowState.InGame )
					startTransition();
			}
		}
	}
	private CameraFollowState _state = CameraFollowState.Initial;

	void Update ()
	{
//		if (doTransition) {
//			startTransition ();
//			doTransition = false;
//		}
//
//		if (transitionInProgress)
//			updateTransition ();
//
//		follow ();

		Debug.Log ("camera udate");
		var state = this.State;
		switch( state ) {
			case CameraFollowState.Initial:
				updateInitialState();
				break;

			case CameraFollowState.KickInProgress:
				updateKickInProgressState();
				break;

			case CameraFollowState.InGame:
				updateInGameState();
				break;

			default:
				throw new UnityException( "Not supported camera follow state: " + (int)state );
				break;
		}
	}



	public void reset()
	{
		cameraOffset = initialOffset;
		cameraRotaion = initialRotation;

//		Update();
	}


	private void startTransition ()
	{
		nextOffset = inGameOffset;
		nextRotaion = inGameRotation;

		offsetChangeRate = (nextOffset - cameraOffset) / transitionTime;
		rotationChangeRate = (nextRotaion - cameraRotaion) / transitionTime;
		transitionInProgress = true;
	}

	private void updateTransition ()
	{
		if (transitionTime <= 0) {
			cameraOffset = nextOffset;
			cameraRotaion = nextRotaion;

			transitionTime = 0;
			transitionInProgress = false;
		} else {
			cameraOffset += Time.deltaTime * offsetChangeRate;
			cameraRotaion += Time.deltaTime * rotationChangeRate;

			transitionTime -= Time.deltaTime;
		}
	}
	
	// 0.24, 0.98, -3.07
	// 3.37, 7.89, 0

	void updateInitialState ()
	{
		var p = playerTarget.transform.position;
		Camera.main.transform.position = p + initialOffset;
		Camera.main.transform.eulerAngles = initialRotation;
	}

	void updateKickInProgressState ()
	{
		var p = playerTarget.transform.position;

		if( Application.isPlaying )
		{
//			updateTransition();
		}
		else
		{
			Camera.main.transform.position = p + kickInProgressOffset;
			Camera.main.transform.eulerAngles = kickInProgressRotation;
		} 
	}

	void updateInGameState ()
	{
		var p = playerTarget.transform.position;
		Camera.main.transform.position = p + inGameOffset;
		Camera.main.transform.eulerAngles = inGameRotation;
	}
}
