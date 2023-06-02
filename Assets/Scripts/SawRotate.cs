using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class SawRotate : MonoBehaviour
    {
        [SerializeField] private Transform _saw;
        [SerializeField] private float _speed;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _saw.Rotate(Vector3.forward * Time.deltaTime * _speed);
        }
    }
}