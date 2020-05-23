using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float maxDepth;
	public float minDepth;
	public float margin;
	public float smoothSpeed;
	
	WallCollisionDetector leftScript;
	WallCollisionDetector rightScript;
	WallCollisionDetector topScript;
	WallCollisionDetector bottomScript;
	
	public GameObject wallPrefab;
	private Transform player1, player2;
	
	public bool wallCollision = false;
	public bool bufferCollision = false;
	public bool falling = false;
	Vector3 stairsPos;
	
	public float maxWidth;
	public float maxHeight;
	
	public float verticalFOV;
	public float horizontalFOV;
	
	private float zPosition;
	
	public void CreateWalls()
	{
		float depth = transform.position.z;
		transform.SetPositionAndRotation(new Vector3(0f, 0f, maxDepth), transform.rotation);
		
		//angle
		Quaternion leftAngle = Quaternion.Euler(0f, horizontalFOV/-2f, -90f);
		Quaternion rightAngle = Quaternion.Euler(0f, horizontalFOV/2f, 90f);
		Quaternion topAngle = Quaternion.Euler(verticalFOV/-2f, 0f, 180f);
		Quaternion bottomAngle = Quaternion.Euler( verticalFOV/2f, 0f, 0f);
		
		//scale
		float verLength = Mathf.Abs(maxDepth) / Mathf.Cos(Mathf.Deg2Rad * horizontalFOV/2f);
		float horLength = Mathf.Abs(maxDepth) / Mathf.Cos(Mathf.Deg2Rad * verticalFOV/2f);
		float verHeight = maxHeight;
		float horHeight = maxWidth;
		
		//position
		Vector3 leftPosition = new Vector3(maxWidth/-4f + margin, 0f, maxDepth/2f);
		Vector3 rightPosition = new Vector3(maxWidth/4f - margin, 0f, maxDepth/2f);
		Vector3 topPosition = new Vector3(0f, maxHeight/4 - margin, maxDepth/2f);
		Vector3 bottomPosition = new Vector3(0f, maxHeight/-4 + margin, maxDepth/2f);
		
		//instantiate
		GameObject left = Instantiate(wallPrefab, leftPosition, leftAngle, GetComponent<Transform>());
		leftScript = left.GetComponent<WallCollisionDetector>();
		//left.transform.localScale = new Vector3(verHeight, 1f, verLength);
		GameObject right = Instantiate(wallPrefab, rightPosition, rightAngle, GetComponent<Transform>());
		rightScript = right.GetComponent<WallCollisionDetector>();
		//right.transform.localScale = new Vector3(verHeight, 1f, verLength);
		GameObject top = Instantiate(wallPrefab, topPosition, topAngle, GetComponent<Transform>());
		topScript = top.GetComponent<WallCollisionDetector>();
		//top.transform.localScale = new Vector3(horHeight, 1f, horLength);
		GameObject bottom = Instantiate(wallPrefab, bottomPosition, bottomAngle, GetComponent<Transform>());
		bottomScript = bottom.GetComponent<WallCollisionDetector>();
		//bottom.transform.localScale = new Vector3(horHeight, 1f, horLength);
	}
	
	Vector3 GetMiddlePoint()
    {
        Vector3 newPos = new Vector3(0.0f, 0.0f, transform.position.z);
		
        newPos.x = (player1.position.x + player2.position.x) / 2.0f;
        newPos.y = (player1.position.y + player2.position.y) / 2.0f;
	
        return newPos;
    }
	
	void SetZPosition()
	{		
		if(wallCollision)
			DecreaseZPosition(0.2f);
		else
			if(!bufferCollision)
				IncreaseZPosition(0.2f);
	}
	
	Vector3 SetCameraPosition()
	{
		Vector3 target = GetMiddlePoint();
		SetZPosition();
		bool follow = true;
		if(leftScript.collision && target.x > transform.position.x)
			follow = false;
		if(rightScript.collision && target.x < transform.position.x)
			follow = false;
		if(topScript.collision && target.y < transform.position.y)
			follow = false;
		if(bottomScript.collision && target.y > transform.position.y)
			follow = false;
		
		if(!follow)
			target = transform.position;
		target.z = zPosition;
		
		return target;	
	}
	
	void Awake()
	{
		verticalFOV = GetComponent<Camera>().fieldOfView;
		horizontalFOV = Camera.VerticalToHorizontalFieldOfView(verticalFOV, GetComponent<Camera>().aspect);
		maxWidth = 2 * Mathf.Abs(maxDepth) * Mathf.Tan(Mathf.Deg2Rad * horizontalFOV/2f);
		maxHeight = 2 * Mathf.Abs(maxDepth) * Mathf.Tan(Mathf.Deg2Rad * verticalFOV/2f);
		CreateWalls();
		player1 = GameObject.FindWithTag("Player1").GetComponent<Transform>();
        player2 = GameObject.FindWithTag("Player2").GetComponent<Transform>();
		zPosition = -10;
	}
	
	public void DecreaseZPosition(float z)
	{
		if(z > 0 && zPosition - z >= maxDepth)
		{
			zPosition -= z;
		}
	}
	
	public void IncreaseZPosition(float z)
	{
		if(z > 0 && zPosition + z <= minDepth)
		{
			zPosition += z;
		}
	}
	
	public bool Stairs(GameObject stairsGameObject)
	{
		stairsPos = stairsGameObject.transform.position;
		Vector3 position = transform.position;

		float horZ;
		if(stairsPos.x > position.x)
        {
			horZ = -(stairsPos.x + 2f - position.x) / Mathf.Tan(horizontalFOV/2 * Mathf.Deg2Rad);
        }
        else
        {
			horZ = -(position.x - stairsPos.x + 2f ) / Mathf.Tan(horizontalFOV / 2 * Mathf.Deg2Rad);
		}

		float verZ;
		if (stairsPos.y > position.y)
		{
			verZ = -(stairsPos.y + 2f - position.y) / Mathf.Tan(verticalFOV / 2 * Mathf.Deg2Rad);
		}
		else
		{
			verZ = -(position.y - stairsPos.y + 2f) / Mathf.Tan(verticalFOV / 2 * Mathf.Deg2Rad);
		}

		float newZ = Mathf.Min(Mathf.Min(verZ, horZ), zPosition);

		if(newZ >= maxDepth)
        {
			zPosition = newZ;
			transform.SetPositionAndRotation(new Vector3(transform.position.x, transform.position.y, zPosition), transform.rotation);
			return true;
		}
        else
        {
			//UnityEngine.Debug.Log("za daleko te schody");
			return false;
        }

	}

	void Falling()
    {
		UnityEngine.Debug.Log("falling");
		transform.Translate(new Vector3(0f, 0f, -10f * Time.deltaTime));
    }
	
	void Update()
    {
		if (falling)
		{
			Falling();
		}
		else
		{
			Vector3 target = SetCameraPosition();
			Vector3 smoothedPosition = Vector3.Lerp(transform.position, target, smoothSpeed);
			transform.position = smoothedPosition;
			//transform.Translate(new Vector3(target.x - transform.position.x, target.y - transform.position.y, 10f * Time.deltaTime * (zPosition - transform.position.z)));
		}

    }
}