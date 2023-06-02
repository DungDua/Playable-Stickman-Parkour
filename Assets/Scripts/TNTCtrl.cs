using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class TNTCtrl : MonoBehaviour
    {
        

        // Start is called before the first frame update
        void Start()
        {
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == Const.TAG_PLAYER)
            {
                this.GetComponent<SpriteRenderer>().enabled = false;
                this.GetComponent<BoxCollider2D>().enabled = false;
                this.GetComponentInChildren<ParticleSystem>().Play();
                SoundManager.Instance?.OnPlaySound(SoundType.TNT);
            }
        }


    }
}