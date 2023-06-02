using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{

    [CreateAssetMenu(fileName = "Character", menuName = "ScriptableObjects/Character Config", order = 1)]
    public class CharacterConfig : ScriptableObject
    {
        public List<SkinCharacter> TotalSkins;
    }

    [System.Serializable]
    public class SkinCharacter
    {
        public string HeroName;
        public UnlockTypes TypeOpen;
        public HeroType TypeHero;
        public int Id;
        public int Price;

    }

}