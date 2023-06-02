using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public interface IItem
    {
        void CollectItem();
    }

    public interface IAttack
    {
        void OnBeHit(float dame);
    }
}

