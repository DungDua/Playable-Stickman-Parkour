using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using UnityEngine.Events;

public class CircleCollider : MonoBehaviour
{
    public UnityEvent Action;


    private void OnMouseDown()
    {
        Action?.Invoke();
    }

    



}
