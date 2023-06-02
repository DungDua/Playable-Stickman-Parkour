using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


namespace Skymare
{
    public class StartAI : MonoBehaviour
    {
        public List< AICtrl> _aiCtrl;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == Const.TAG_PLAYER)
            {
                for(int i = 0; i < _aiCtrl.Count; i++)
                    _aiCtrl[i].OnStart();
            }
        }
    }
}