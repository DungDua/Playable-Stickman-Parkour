using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField] private Animator _animElevator;

        // Start is called before the first frame update
        void Start()
        {

        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == Const.TAG_PLAYER)
            {
                collision.transform.SetParent(this.transform);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == Const.TAG_PLAYER)
            {
                collision.transform.parent = null;
            }
        }

        public void OnStopElevator()
        {
            StartCoroutine(IETamStop());
        }

        private IEnumerator IETamStop()
        {
            _animElevator.enabled = false;
            yield return new WaitForSeconds(1.5f);
            _animElevator.enabled = true;
        }

    }
}