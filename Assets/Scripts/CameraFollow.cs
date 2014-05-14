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

	[SerializeField, HideInInspector]
	private CameraFollowState _state = CameraFollowState.Initial;
	[ExposeProperty]
	public CameraFollowState State {
		get {
			if( Application.isPlaying )
			{
				if( isTransitionInProgress() ) 
					_state = CameraFollowState.KickInProgress;
			}

			return _state;
		}
		set {
			if( _state != value )
			{
				_state = value;

				switch( _state )
				{
					case CameraFollowState.Initial: moveTransition( initialOffset, initialRotation ); break;
					case CameraFollowState.KickInProgress: moveTransition( kickInProgressOffset, kickInProgressRotation ); break;
					case CameraFollowState.InGame: moveTransition( inGameOffset, inGameRotation ); break;
				}
					
				Update();

				Debug.Log( string.Format( "Camera follow state: {0}", _state.ToString() ));
			}
		}
	}
	
	public void reset()
	{
		this.State = CameraFollowState.Initial;

		// reset instantlly initial position and angle
		stopTransition();
		Update();
	}

	void Awake()
	{
		reset();
	}

	void Update()
	{
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
		}

		var p = playerTarget.transform.position;
		Camera.main.transform.position = p + _transOffset;
		Camera.main.transform.eulerAngles = _transAngleOffset;
	}
	
	void updateInitialState ()
	{
		_transOffset = initialOffset;
		_transAngleOffset = initialRotation;
	}
	
	void updateKickInProgressState ()
	{
		if( isTransitionInProgress() )
		{
			updateTransition();
		}
		else
		{
			_transOffset = kickInProgressOffset;
			_transAngleOffset = kickInProgressRotation;
		} 
	}
	
	void updateInGameState ()
	{
		_transOffset = inGameOffset;
		_transAngleOffset = inGameRotation;
	}

	#region Transition methods

	private const float TransitionTime = 0.8f;

	private float _transitionRamainingTime;

	private Vector3 _transOffset;
	private Vector3 _transAngleOffset;

	private Vector3 _transStartPos;
	private Vector3 _transStartAngle;
	private Vector3 _transEndPos;
	private Vector3 _transEndAngle;

	private Vector3 _transPosStep;
	private Vector3 _transAngleStep;

	private bool isTransitionInProgress()
	{
		return _transitionRamainingTime > 0;
	}

	private void moveTransition( Vector3 endOffset, Vector3 endAngle )
	{
		startTransition( _transOffset, _transAngleOffset, endOffset, endAngle );
	}

	private void startTransition( Vector3 startOffset, Vector3 endOffset )
	{
		var angle = Camera.main.transform.eulerAngles;
		startTransition( startOffset, angle, endOffset, angle );
	}

	private void startTransition( Vector3 startOffset, Vector3 startAngle, Vector3 endOffset, Vector3 endAngle )
	{
		_transStartPos = startOffset;
		_transStartAngle = startAngle;
		_transEndPos = endOffset;
		_transEndAngle = endAngle;
		_transitionRamainingTime = TransitionTime;

		_transOffset = _transStartPos;
		_transAngleOffset = _transStartAngle;
		_transPosStep = (_transEndPos - _transStartPos) / _transitionRamainingTime;
		_transAngleStep = (_transEndAngle - _transStartAngle) / _transitionRamainingTime;
	}
	
	private void updateTransition ()
	{
		if( !isTransitionInProgress() )
			return;

		var dt = Time.deltaTime;
		if( dt <= 0 ) 
			dt = _transitionRamainingTime;

		_transOffset += _transPosStep * dt;
		_transAngleOffset += _transAngleStep * dt;

		_transitionRamainingTime -= dt;
	}

	private void stopTransition()
	{
		_transitionRamainingTime = 0F;
	}

	#endregion
	
	// 0.24, 0.98, -3.07
	// 3.37, 7.89, 0

}
