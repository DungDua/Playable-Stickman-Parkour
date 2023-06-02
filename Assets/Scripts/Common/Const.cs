using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skymare
{
    public class Const 
    {
        public const int JUMP = 0;
        public const int DOUBLE_JUMP = 1;
        public const int OVER_JUMP = 2;

        public const float MOVE_SPEED = 5.25f;
        public const float JUMP_FORCE = 600f;

        // Tag
        public const string TAG_GROUND = "Ground";
        public const string TAG_PLAYER = "Player";
        public const string TAG_ITEM = "Item";
        public const string TAG_GATE = "Gate";
        public const string TAG_FINISH = "Finish";
        public const string TAG_FLOOR = "Floor";
        public const string TAG_PUSH_GROUND = "PushGround";
        public const string TAG_THROW_GROUND = "ThrowGround";
        public const string TAG_ARROW = "Arrow";
        public const string TAG_CLIMB_WALL = "ClimbWall";
        public const string TAG_FADE_SWITCH = "FadeSwitch";
        public const string TAG_TNT = "TNT";
        public const string TAG_JUMP_BOX = "JumpBox";
        public const string TAG_TRAP_BOX = "TrapBox";
        public const string TAG_CHECK_POINT = "CheckPoint";
        public const string TAG_TRY_SKIN = "TrySkin";
        public const string TAG_PIKE = "Pike";
        public const string TAG_DASH = "Dash";


        public const string KEY_PET_SELECTED = "pet-selected";
        public const string KEY_PET_OPEN = "pet-opened-";
        public const string KEY_PET_WATCH_ADS = "watch-ads-pet-";
        public const string KEY_COIN = "total-coin";
        public const string KEY_HERO_SELECTED = "hero-selected";
        public const string KEY_HERO_OPEN = "hero-opened-";
        public const string KEY_REMOVE_ADS = "has-remove-ads";

    }


    public enum RewardType
    {
        Unlock_Band, Unlock_Pet, Unlock_Hero, X2_Coin, Skip_Level, Try_Skin, Spin, Chicken_Jocky, Claim_Skin, Get_Prize
    }

}
