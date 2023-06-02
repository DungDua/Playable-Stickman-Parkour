using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class FallGround : MonoBehaviour
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
                Invoke("OnFall", 0.25f);
            }
        }

        private void OnFall()
        {
            _myAnim.SetTrigger("fall");
        }
    }
}