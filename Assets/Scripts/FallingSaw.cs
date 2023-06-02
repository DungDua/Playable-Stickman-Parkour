using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class FallingSaw : MonoBehaviour
    {
        private Rigidbody2D _rigid2D;

        // Start is called before the first frame update
        void Start()
        {
            _rigid2D = this.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_rigid2D.velocity.y > 10)
                _rigid2D.velocity = new Vector2(0, 10);

            if (_rigid2D.velocity.y < -10f)
                _rigid2D.velocity = new Vector2(0f, -10f);
        }
    }
}