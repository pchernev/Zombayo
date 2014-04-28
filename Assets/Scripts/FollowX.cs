using UnityEngine;

public class FollowX : MonoBehaviour
{
  public Transform target;
  public float offset;

	void Start()
	{
    follow();
	}

  void Update()
  {
    follow();
  }

  public void follow()
  {
    transform.position = new Vector3(target.position.x + offset, transform.position.y, transform.position.z);
  }
}
