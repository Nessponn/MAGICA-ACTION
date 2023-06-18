using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectSystem_Title : MonoBehaviour
{
    private GameObject EffectWhite;
    private GameObject EffectWhite_Small;
    private GameObject EffectYellow;
    private GameObject EffectYellow_Small;

    [Range(0f, 10f)] public float Speed = 0.5f;
    [Range(1f, 10f)] public float Range_x = 8f;
    [Range(1f, 10f)] public float Range_y = 8f;

    Tween tween;
    private void Start()
    {
        var GM = TitleManager.Instance;

        EffectWhite = GM.EffectWhite;
        EffectWhite_Small = GM.EffectWhite_Small;
        EffectYellow = GM.EffectYellow;
        EffectYellow_Small = GM.EffectYellow_Small;

        InstantEffect();
    }

    private void InstantEffect()
    {
        int num = Random.Range(0, 3);

        float rangex = Random.Range(-Range_x, Range_x);
        float rangey = Random.Range(-Range_y, Range_y);

        GameObject obj = null;
        if (num == 0) obj = Instantiate(EffectWhite);
        else  if(num == 1)obj = Instantiate(EffectYellow);
        else if(num == 2) obj = Instantiate(EffectWhite_Small);
        else if(num == 3) obj = Instantiate(EffectYellow_Small);

        //obj.transform.parent = gameObject.transform;

        //obj.transform.DOLocalMove(new Vector3(Direction_x, Direction_y, 0), 1).SetLink(obj);

        obj.transform.position = gameObject.transform.position;

        obj.transform.DOLocalMove(new Vector3(obj.transform.localPosition.x + rangex, obj.transform.localPosition.y + rangey, 0), 1 / Speed).SetLink(obj);

        Destroy(obj, 1/Speed);

        if (TitleManager.Instance.Title_End) return;

        tween = DOVirtual.DelayedCall(0.1f, () => { InstantEffect();  }).SetLink(obj);
    }

    public void ENDEFFECT()
    {
        tween.Kill();
    }
}
