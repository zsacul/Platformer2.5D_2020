using UnityEngine;

public class CameraFollowEdges : MonoBehaviour
{    
    private PlayerEdges player1, player2;
    float margin = 0.01f;
 
    Vector3 get_middlepoint()
    {
        float x = (player1.transform.position.x + player2.transform.position.x) / 2.0f;
        float y = (player1.transform.position.y + player2.transform.position.y) / 2.0f;
        float z = Camera.main.transform.position.z;
        return new Vector3(x, y, z);
    }

    void Start()
    {
        player1 = GameObject.FindWithTag("player_1").GetComponent<PlayerEdges>();
        player2 = GameObject.FindWithTag("player_2").GetComponent<PlayerEdges>();
        Camera.main.transform.position = get_middlepoint();
    }

    void restrainMovement(PlayerEdges player)
    {
        Vector3 p = Camera.main.WorldToViewportPoint(player.transform.position);

        if (p.x < margin)
            player.moveLeft = false;
        else
            player.moveLeft = true;


        if (p.x > 1 - margin)
            player.moveRight = false;
        else
            player.moveRight = true;


        if (p.y < margin)
            player.moveDown = false;
        else
            player.moveDown = true;


        if (p.y > 1 - margin)
            player.moveUp = false;
        else
            player.moveUp = true;
    }

    void Update()
    {
        Camera.main.transform.position = get_middlepoint();
        restrainMovement(player1);
        restrainMovement(player2);
    }
}