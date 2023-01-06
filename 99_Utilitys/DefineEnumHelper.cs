using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineEnumHelper
{
    public enum PoolingObj
    {
        HPParticle,
        Buff,
        FootStepEffect,
        BuffEffect,
        HittingEffect,
    }
    public enum WindowObj
    {
        InGameWindow,
        InventoryWindow,
        CharacterInfoWindow,
        LoadingWindow

    }
    public enum ItemType
    {
        UsedItem = 1,
        MountedItem,
        OtherItem = 99
    }
    public enum ItemKind
    {
        HPPortion = 1,
        MPPortion,
        HelMat,
        Armor,
        Weapon,
        Glove,
        Boots,
    }
    public enum ItemStat
    {
        HP =1,
        MP,
        DEF,
        DAM,
    }
    public enum MonsterObj
    {
        Slime,
        SoldierSlime,
        VikingSlime,
        KingSlime
    }
    public enum MonsterDropItem
    {
        None = 0,
        Yoko_Necklace = 1001,
    }
    public enum SkillKind
    {
        JumpAttack,
        WheelWind,
        Rage
    }
    public enum BGMKind
    {
        LoginScene,
        LoadingScene,
        IngameScene
    }
    public enum SFXSound
    {
        UIClick,
        LevelUP,
    }

}
