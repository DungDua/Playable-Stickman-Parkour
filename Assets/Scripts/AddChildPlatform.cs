using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class AddChildPlatform : MonoBehaviour
    {
        [SerializeField] private ElevatorCtrl _ctrl;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == Const.TAG_PLAYER)
            {
                collision.gameObject.transform.SetParent(this.transform);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == Const.TAG_PLAYER)
            {
                collision.gameObject.transform.parent = null;
            }
        }

        public void OnStop()
        {
            _ctrl.OnSetDelay(true);
            StartCoroutine(IEDelay());

            IEnumerator IEDelay()
            {
                yield return new WaitForSeconds(1.3f);
                _ctrl.OnSetDelay(false);
            }
        }

    }
}