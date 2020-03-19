using UnityEngine;

public class CameraFollow : MonoBehaviour
{    
    private PlayerScript player1, player2;
    float margin = 0.04f;
 
    Vector3 GetMiddlepoint()
    {
        float x = (player1.transform.position.x + player2.transform.position.x) / 2.0f;
        float y = (player1.transform.position.y + player2.transform.position.y) / 2.0f;
        float z = Camera.main.transform.position.z;
        return new Vector3(x, y, z);
    }

    void Start()
    {
        player1 = GameObject.FindWithTag("Player1").GetComponent<PlayerScript>();
        player2 = GameObject.FindWithTag("Player2").GetComponent<PlayerScript>();
        Camera.main.transform.position = GetMiddlepoint();
    }

    void RestrictMovement(PlayerScript player)
    {
        Vector3 p = Camera.main.WorldToViewportPoint(player.transform.position);
        // bottom-left is (x=0, y=0), top-right is (x=1, y=1)

        if (p.x < margin)
            player.RestrictLeft();
        else
            player.UnrestrictLeft();


        if (p.x > 1 - margin)
            player.RestrictRigth();
        else
            player.UnrestrictRigth();


        if (p.y > 1 - margin)
            player.RestrictUp();
        else
            player.UnrestrictUp();


        if (p.y < margin)
            player.RestrictDown();
        else
            player.UnrestrictDown();
    }

    void Update()
    {
        Camera.main.transform.position = GetMiddlepoint();
        RestrictMovement(player1);
        RestrictMovement(player2);
    }
}