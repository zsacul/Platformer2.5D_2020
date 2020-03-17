using UnityEngine;

public class CameraFollowWalls : MonoBehaviour
{
    public GameObject prefabWalls;
    private GameObject walls;
    private PlayerWalls player1, player2;
 
    Vector3 get_middlepoint()
    {
        float x = (player1.transform.position.x + player2.transform.position.x) / 2.0f;
        float y = (player1.transform.position.y + player2.transform.position.y) / 2.0f;
        float z = Camera.main.transform.position.z;
        return new Vector3(x, y, z);
    }

    void Start()
    {
        player1 = GameObject.FindWithTag("player_1").GetComponent<PlayerWalls>();
        player2 = GameObject.FindWithTag("player_2").GetComponent<PlayerWalls>();
        Camera.main.transform.position = get_middlepoint();

        walls = Instantiate(prefabWalls, Camera.main.transform.position, Quaternion.identity);
    }

    void Update()
    {
        Camera.main.transform.position = get_middlepoint();
        walls.transform.position = Camera.main.transform.position;
    }
}