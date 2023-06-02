using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Skymare
{
    public class SawTrapCtrl : MonoBehaviour
    {
        [SerializeField] private Transform _sawLeft;
        [SerializeField] private Transform _sawRight;

        // Start is called before the first frame update
        void Start()
        {

        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == Const.TAG_PLAYER)
            {
                if (this.transform.position.x > collision.transform.position.x)
                    _sawRight.DOLocalRotate(Vector3.forward * 180f, 0.1f).OnComplete(()=> {
                        _sawRight.DOLocalRotate(Vector3.zero, 0.3f).SetDelay(2f);
                    });
                else
                    _sawLeft.DOLocalRotate(Vector3.forward * -180f, 0.1f).OnComplete(()=> {
                        _sawLeft.DOLocalRotate(Vector3.zero, 0.3f).SetDelay(2f);
                    });
            }
        }


    }
}