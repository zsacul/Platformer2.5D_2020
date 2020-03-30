using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothSpeed = 0.2f;

    private Transform cameraTransform;
    private Transform player1, player2;
    private Transform leftWall, rightWall, topWall, bottomWall;

    [SerializeField]
    private GameObject wallVerticalPrefab;
    [SerializeField]
    private float wallsHeight = 3f;
    [SerializeField]
    private GameObject wallHorizontalPrefab;
    [SerializeField]
    private float wallsWidth = 4f;

    [SerializeField]
    private float margin = 0.0f;
    [SerializeField]
    private float sceneDepth = 10f;


    void CreateScreenEdgeWalls()
    {
        Vector3 sc = wallVerticalPrefab.GetComponent<Transform>().localScale;
        sc.x = wallsHeight;
        wallVerticalPrefab.GetComponent<Transform>().localScale = sc;

        sc = wallHorizontalPrefab.GetComponent<Transform>().localScale;
        sc.x = wallsWidth;
        wallHorizontalPrefab.GetComponent<Transform>().localScale = sc;


        Vector3 left = Camera.main.ViewportToWorldPoint(new Vector3(margin, 0.5f, sceneDepth));
        Vector3 right = Camera.main.ViewportToWorldPoint(new Vector3(1.0f - margin, 0.5f, sceneDepth));
        Vector3 top = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.0f - margin, sceneDepth));
        Vector3 bottom = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, margin, sceneDepth));


        leftWall = Instantiate(wallVerticalPrefab, left, Quaternion.Euler(0.0f, 0.0f, -90.0f), cameraTransform).GetComponent<Transform>();
        rightWall = Instantiate(wallVerticalPrefab, right, Quaternion.Euler(0.0f, 0.0f, 90.0f), cameraTransform).GetComponent<Transform>();
        topWall = Instantiate(wallHorizontalPrefab, top, Quaternion.Euler(0.0f, 0.0f, 180.0f), cameraTransform).GetComponent<Transform>();
        bottomWall = Instantiate(wallHorizontalPrefab, bottom, Quaternion.identity, cameraTransform).GetComponent<Transform>();
    }

    Vector3 GetMiddlePoint()
    {
        Vector3 newPos = new Vector3(0.0f, 0.0f, cameraTransform.position.z);

        newPos.x = (player1.position.x + player2.position.x) / 2.0f;
        newPos.y = (player1.position.y + player2.position.y) / 2.0f;

        return newPos;
    }

    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        player1 = GameObject.FindWithTag("Player1").GetComponent<Transform>();
        player2 = GameObject.FindWithTag("Player2").GetComponent<Transform>();
        cameraTransform.position = GetMiddlePoint();
        CreateScreenEdgeWalls();
    }

    void LateUpdate()
    {
        Vector3 target = GetMiddlePoint();

        Vector3 destination = target;// + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, destination, smoothSpeed);
        cameraTransform.position = smoothedPosition;

    }
}