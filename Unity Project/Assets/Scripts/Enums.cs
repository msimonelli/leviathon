
public enum ModifierCalculation
{
    Addition = 0,
    Multiplication,
    Percentage
};


public enum InitiativeType
{
   Random,
   Party,
   Enemy
};


public enum Attribute
{
   None = -1,
   Strength = 0,
   Intelligence,
   Constitution,
   Dexterity,
   Charisma,
   Wisdom,
   Total,
};


public enum ModifierType
{
   None = -1,
   Strength = 0,
   Intelligence,
   Constitution,
   Dexterity,
   Charisma,
   Wisdom,
   MaxHealthPoints,
   MaxSpellPoints,
   ArmorClass,
   WeaponDamage,
   Total,
};


public enum Location
{
   Wilderness = 1,
   Dungeon = 2,
   Town = 4,
   All = 7,
};


public enum Races
{
   Human,
   Elf,
   Dwarf,
   Halfling,
   HalfElf,
   Gnome,
};


public enum Classes
{
   None = 0,
   Fighter = 1,
   Barbarian = 2,
   Sorcerer = 4,
   Rogue = 8,
   Bard = 16,
   Paladin = 32,
   Cleric = 64,
   Monk = 128,
   Ranger = 256,
   Wizard = 512,
   All = 1023,
};


public enum Size
{
   Small,
   Medium,
   Large
};


public enum Direction
{
   None = 0,
   North,
   NorthEast,
   East,
   SouthEast,
   South,
   SouthWest,
   West,
   NorthWest,
};


public enum Move
{
   None = 0,
   Forward = 1,
   Back = 2,
   Left = 4,
   Right = 8,
};


public enum Turn
{
   None = 0,
   Left = 1,
   Around = 2,
   Right = 3,
};


public enum ItemType
{
   Weapon = 1,
   Shield = 2,
   Clothing = 4,
   Jewelry = 8,
   Potion = 16,
   All = 31
};


public enum EquipableArea
{
   None = 0,
   Head = 1,
   Neck = 2,
   Torso = 4,
   Feet = 8,
   Hands = 16,
   MainFinger = 32,
   OffFinger = 64,
   MainHand = 128,
   OffHand = 256,
   Any = 511,
};


public enum ItemAvailability
{
   Common,
   Uncommon,
   Rare,
   VeryRare,
};


public enum WeaponType
{
   Melee,
   Ranged,
};


public enum HandsRequired
{
   None,
   One,
   Two
};


public enum DamageType
{
   None = 0,
   Pierce = 1,
   Slash = 2,
   Blunt = 4,
};


public enum Distance
{
   Distance0 = 0,
   Distance10,
   Distance20,
   Distance30,
   Distance40,
   Distance50,
   Distance60,
   Distance70,
   Distance80,
   Distance90,
   Distance100,
   Distance120,
   Distance130,
   Distance140,
   Distance150,
   Distance160,
   Distance170,
   Distance180,
   Distance190,
   Distance200,
};


public enum ImmuneDamage
{
   None = 0,
   Pierce = 1,
   Slash = 2,
   Blunt = 4,
};


public enum Gender
{
   Male,
   Female,
};


public enum CommandType
{
   None = 0,
   Forward,
   Backward,
   Left,
   Right,
   AttackTarget,
   CastSpellOnTarget,
   CastSpell,
   Guard
};


public enum BattleTurn
{
   Party,
   Monsters
};
