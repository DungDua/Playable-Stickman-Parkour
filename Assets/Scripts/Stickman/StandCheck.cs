using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Skymare
{
    public class StandCheck : MonoBehaviour
    {
        public bool OnGround()
        {
            Collider2D col = Physics2D.OverlapCircle(this.transform.position, 0.1f, 1 << 8);
            return col ;
        }


    }
}