using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class CheckPointCtrl : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _fxRegenarate;


        private bool _hasCheck;

        // Start is called before the first frame update
        void Start()
        {

        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_hasCheck) return;

            if(collision.tag == Const.TAG_PLAYER)
            {
                _hasCheck = true;
                this.GetComponent<Collider2D>().enabled = false;
                _fxRegenarate.Play();
                this.enabled = false;
                
            }
        }


    }
}