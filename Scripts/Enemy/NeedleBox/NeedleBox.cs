using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleBox : EnemyManager
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        Sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (leftHit || rightHit)
        {
            //Debug.Log("壁判定");
            rbody.velocity = new Vector2(-speed * dir, rbody.velocity.y);
        }
        else if (hit)
        {
            //Debug.Log("地面判定");
            rbody.velocity = new Vector2(-speed * dir, 0);
        }
        else
        {
            //Debug.Log("落下判定");
            rbody.velocity = new Vector2(-speed * dir, rbody.velocity.y);
        }

    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (FirstPos != new Vector2(0, 0)) gameObject.transform.position = FirstPos;
    }

    public override void Defeat_Player()
    {
        //base.Player_Defeat();

        PlayerManager_Ray.Instance.Death_process();//ぶっ殺した…
    }

    public override void Defeat_Enemy()
    {
        //base.Defeat_Enemy();

        PlayerManager_Ray.Instance.Death_process();//ぶっ殺した…
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            Defeat_Player();
        }
    }

    public override void Reset_process()
    {
        base.Reset_process();
    }
}
