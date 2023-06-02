using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Skymare
{

    public enum HeroType
    {
        Default, Iron, Spider, Captain, Thor, Wanda, Panther, Flash, Superman, Luffy, Pubg, Choper, Lee, Goku, Kelly, Sakura, Bunny,
        Hulk, GreenLantern, LaserEye, Joker, Naruto, SquidMan, SquidSoldier, HeadIron, Wolf, MatrixMan, RainbowBlue, RainbowRed, Venom,
        Craftman, Nobita, BlueHair, AmongUs, Mummy, Mario, Shazam, Batman, Satan, Ashe, Gohan, Pokemon, Cadic, HaleyQuin, Sasuke, WonderWomen,
        Yasuo, Jinx, MasterJi, Akali, Lux, Sona, LeeSin, Missfortune, DeadPool, DoctorStrange, WindArcher, Avatar, AdamWarlock, HighEvolution,
        Kraglin, Rocket, StarLord, Drax, Nebula, Mantis, Gamora, Groot, Cosmo
    }



    public enum PetTypes
    {
        None, Axolotl, Camel, Chicken, Pig, Salmon , Bee, Fox, Parrot, Squid, Tiger
    }

    public enum UnlockTypes
    {
        Free, Coin, Ads , Spin, Daily_Reward, Level, Other, Premium, Galaxy
    }

    public class PetElement : MonoBehaviour
    {
        public PetTypes Pet;
        public UnlockTypes Unlock;
        public int NumberUnlock;

    }
}