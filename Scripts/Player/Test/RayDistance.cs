using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDistance : MonoBehaviour
{
    public LayerMask blockLayer;

    private Rigidbody2D rbody;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //�n�_�ƏI�_�iRay�`�ʗp�B���ۂ̔���ł͂Ȃ��j
        Vector3 origin = transform.position;
        Vector3 distance = Vector3.down * 0.5f;

        //���ۂ�Ray�̔���
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.5f, blockLayer);

        // Does the ray intersect any objects excluding the player layer
        if (hit)
        {
            Debug.Log(hit.distance);
            //Debug.Log(hit.point);
            Debug.DrawRay(origin, distance, Color.yellow);

            transform.position = new Vector2(transform.position.x, hit.point.y + 0.5f);

            rbody.velocity = new Vector2(rbody.velocity.x, 0);


        }
        else
        {
            Debug.DrawRay(origin, distance, Color.blue);
        }
    }

}
