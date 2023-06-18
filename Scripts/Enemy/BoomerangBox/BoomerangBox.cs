using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoomerangBox : EnemyManager
{
    [Range(0,2f)] public float duration = 1f;//投げる周期
    [Range(0, 17f)] public float distance = 5f;
    [Range(0, 10f)] public float Boomerang_Spped = 0.6f;
    [Range(0, 20f)] public float Boomerang_Height = 2f;//投げる周期
    private float t;//時間
    public GameObject Boomerang;//ブーメランオブジェクト
    private Transform player;

    private float Du;
    private float Di;
    private float BS;
    private float BH;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        transform.GetChild(0).gameObject.SetActive(false);//予備動作を消す

        player = PlayerManager_Ray.Instance.gameObject.transform;

    }

    public override void ResisterPosition()
    {
        base.ResisterPosition();

        Du = duration;
        Di = distance;
        BS = Boomerang_Spped;
        BH = Boomerang_Height;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (gameObject.transform.position.x - player.position.x <= 0)
        {
            dir = -1;
            Sp.flipX = true;
        }
        else
        { 
            dir = 1;
            Sp.flipX = false;
        }

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

        t += Time.deltaTime;

        if(t >= duration)
        {
            t = 0;
            ThrowBoomerang();//ブーメランを投げる
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
        //踏んだ音
        AudioManager.Instance.Play_SE(7);
        gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            Defeat_Player();
        }
    }

    //ブーメランを投げる
    private void ThrowBoomerang()
    {
        transform.GetChild(0).gameObject.SetActive(true);//予備動作を設ける

        GameObject obj = null;

        DOVirtual.DelayedCall(0.5f, () =>
        {

            transform.GetChild(0).gameObject.SetActive(false);//予備動作を消す
            if (!gameObject.activeSelf) return;

            //効果音
            AudioManager.Instance.PlayWithNotDuplication_SE(10);

            //ブーメランを生成する
            obj = Instantiate(Boomerang, this.transform.position, Quaternion.identity);

            obj.transform.DOLocalMoveX(transform.position.x - (distance * dir), 1 / Boomerang_Spped).SetLoops(2, LoopType.Yoyo).SetLink(obj);
            obj.transform.DOLocalMoveY(transform.position.y + + Random.Range(0f,Boomerang_Height), 1 / (Boomerang_Spped * 2)).SetLoops(2, LoopType.Yoyo).SetLink(obj);

            Destroy(obj, (1 / Boomerang_Spped) * 2);
        }).SetLink(obj);

    }

    public override void Reset_process()
    {
        base.Reset_process();

        duration = Du;
        distance = Di;
        Boomerang_Spped = BS;
        Boomerang_Height = BH;
    }
}
