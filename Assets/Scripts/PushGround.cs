using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class PushGround : MonoBehaviour
    {
        private Animator _myAnim;

        // Start is called before the first frame update
        void Start()
        {
            _myAnim = this.GetComponent<Animator>();
        }

        public void OnPlayAnim()
        {
            _myAnim.SetTrigger("push");
        }

    }
}