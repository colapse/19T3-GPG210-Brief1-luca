using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Wobble : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOPunchScale(new Vector3(10,10,10),2f );
        var myTween = DOTween.To(() => transform.localScale, (x) => transform.localScale = x, new Vector3(2, 4, 5), 2);
        
        
        //transform.DOScale(new Vector3(10,10,10),2f );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
