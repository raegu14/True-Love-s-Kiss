using UnityEngine;
using System.Collections;


// Original taken from https://github.com/prime31/CharacterController2D
public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;

	private Player _playerController;
	private Vector3 _smoothDampVelocity;
	private Vector3 offset;
	private float lastX;

    public float leftClamp;
    public float rightClamp;

    public bool yClamp;
    public float bottomClamp;


	void Awake()
	{
		if(target == null)
		{
			target = GameObject.Find("Player").transform;
		}
		gameObject.transform.position = target.transform.position;
		transform = gameObject.transform;
		_playerController = target.GetComponent<Player>();
		offset = new Vector3(0, 0, -10);
		lastX = target.position.x;
    rightClamp = GameObject.Find("DOOR").GetComponent<BoxCollider2D>().bounds.min.x;
    var vertExtent = GetComponent<Camera>().orthographicSize;
    var horzExtent = vertExtent * Screen.width / Screen.height;
    rightClamp -= horzExtent;
    leftClamp = GameObject.Find("LEFT DOOR").GetComponent<BoxCollider2D>().bounds.max.x;
    leftClamp += horzExtent;
  }


  void Update()
	{
		if(Time.timeScale > 0)
			updateCameraPosition();
	}


	void updateCameraPosition()
	{
		if( _playerController == null )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
			return;
		}

		if(lastX - target.position.x > 0 )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
		}
		else
		{
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp( transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime );
		}
		lastX = target.position.x;
		transform.position += offset;

        Clamp();
	}

    void Clamp()
    {
        Vector3 clamped = transform.position;
        clamped.x = Mathf.Min(clamped.x, rightClamp);
        clamped.x = Mathf.Max(clamped.x, leftClamp);
        if(yClamp)
        {
            clamped.y = Mathf.Max(clamped.y, bottomClamp);
        }
        transform.position = clamped;
    }

}
