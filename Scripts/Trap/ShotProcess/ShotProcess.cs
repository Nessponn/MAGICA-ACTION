using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShotProcess : TrapManager
{
    public GameObject ShotObject;//ë≈Çøè„Ç∞ÇÈÇ‡ÇÃ
    public Vector3 ShotPower;//ë≈Çøè„Ç∞ÇÈóÕÇ∆ï˚å¸
    private EnemyManager EM;
    private SpriteRenderer Sp;

    private GameObject obj;

    public override void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (TrapPlayed) return;

        if (layername == "Player")
        {
            TrapPlayed = true;

            //ëÂñCÇ›ÇΩÇ¢Ç»âπ
            AudioManager.Instance.Play_SE(9);

            obj = Instantiate(ShotObject, transform.position, Quaternion.identity);

            obj.transform.parent = gameObject.transform;

            obj.transform.DOLocalJump(new Vector3(transform.position.x + ShotPower.x, transform.position.y + ShotPower.y), 1, 1, 0.3f);

            EM = obj.GetComponent<EnemyManager>();
            Sp = obj.GetComponent<SpriteRenderer>();

            if (EM) 
            {
                if (ShotPower.x > 0)
                {
                    EM.dir = -1;
                    if (Sp) Sp.flipX = true;
                }

                EM.enabled = false;
                DOVirtual.DelayedCall(0.3f, () =>
                {
                    EM.enabled = true;
                }).SetLink(obj);

                

                
            }


            

            //obj.transform.DOLocalJump(new Vector3(transform.position.x + ShotPower.x, transform.position.y + ShotPower.y),1, 1, 0.5f);
        }
    }

    public override void Trap_Reset()
    {
        //base.Trap_Reset();

        if (TrapPlayed) Destroy(obj);
        TrapPlayed = false;


    }
}
