using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Skymare
{
    public class FinishBox : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == Const.TAG_PLAYER)
            {
                this.gameObject.name = "lose";
            }
        }
    }
}