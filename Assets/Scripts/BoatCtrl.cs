using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class BoatCtrl : MonoBehaviour
    {
        private Animator _myAnim;

        // Start is called before the first frame update
        void Start()
        {
            _myAnim = this.GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == Const.TAG_PLAYER)
            {
                _myAnim.SetTrigger("nhun");
            }
        }
    }
}