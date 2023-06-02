using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Skymare
{
    public class GroundCheck : MonoBehaviour
    {
        [HideInInspector]
        public bool OnGround
        {
            get { return _onGround; }
            set
            {
                if (_onGround != value)
                {
                    _onGround = value;
                    if (_onGround) _evtOnground?.Invoke();
                    else _evtExitGround?.Invoke();
                }
            }
        }

        private UnityAction _evtOnground, _evtExitGround;
        private GameObject _cacheGround;
        private bool _onGround;


        private void FixedUpdate()
        {
            Collider2D col = Physics2D.OverlapCircle(this.transform.position, 0.1f, 1 << 8 | 1 << 10);
            OnGround = col != null;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == Const.TAG_GROUND)
            {
            //    _evtOnground?.Invoke();
            //    OnGround = true;
                _cacheGround = collision.gameObject;
            }
        }



        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == Const.TAG_GROUND && collision.gameObject == _cacheGround)
            {
            //    OnGround = false;
                _evtExitGround?.Invoke();
            }
        }

        public void OnSetEvent(UnityAction evtOnground,UnityAction evtExit)
        {
            _evtExitGround = evtExit;
            _evtOnground = evtOnground;
        }
    }
}