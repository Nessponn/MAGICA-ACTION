using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMushroom : EnemyManager
{
    private GameObject cam;//ÉJÉÅÉâ
    public override void Start()
    {
        base.Start();
        cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (hit)
        {
            rbody.velocity = new Vector2(speed * 3, 0);
        }

        if (cam.transform.position.y + transform.position.y < -8) Destroy(gameObject);
    }
    public override void Defeat_Player()
    {
        //base.Player_Defeat();

        PlayerManager_Ray.Instance.Death_process();//Ç‘Ç¡éEÇµÇΩÅc
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            Defeat_Player();//Ç‹Ç∏Ç°Åc
            gameObject.SetActive(false);
        }
    }
}
