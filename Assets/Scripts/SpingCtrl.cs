using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class SpingCtrl : MonoBehaviour
    {
        [SerializeField] private float _xForce;
        private Animator _myAnim;
        private GameObject _player;

        // Start is called before the first frame update
        void Start()
        {
            _myAnim = this.GetComponent<Animator>();
        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == Const.TAG_PLAYER && collision.transform.position.y > this.transform.position.y)
            {
                _myAnim.SetTrigger("push");
                _player = collision.gameObject;
                this.OnPush();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == Const.TAG_PLAYER)
            {
                _player = null;
            }
        }

        void OnPush()
        {
            if(_player != null)
            {
                _player.GetComponent<StickmanCtrl>()?.OnSpringPush(_xForce);
            }

        }

    }
}