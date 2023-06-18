using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBell : MonoBehaviour
{
    private Animator anim;
    public GameObject ThankTx;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        string layername = LayerMask.LayerToName(col.gameObject.layer);

        if (layername == "Player")
        {
            ThankTx.SetActive(true);

            anim.SetTrigger("Ring");
            
            var obj = transform.GetChild(0).gameObject;

            obj.SetActive(false);

            AudioManager.Instance.PlayWithNotDuplication_SE(6);
            AudioManager.Instance.Play_SE(11);
            //ÉNÉäÉAéûÇÃèàóù
            GameMaster.Instance.ClearFlag();
            PlayerManager_Ray.Instance.ClearFlag();
        }
    }
}
