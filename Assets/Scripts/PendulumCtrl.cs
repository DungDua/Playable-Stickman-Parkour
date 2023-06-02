using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class PendulumCtrl : MonoBehaviour
    {

        [SerializeField] private Transform _transPendulum;
        [SerializeField] private LineRenderer _line;
        

        

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            _line.SetPosition(0, Vector3.zero);
            _line.SetPosition(1, _transPendulum.localPosition);
        }


  
    }
}