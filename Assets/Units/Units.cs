using System;

public class Unit
{
    #region Attributs
    private string _name;
    private int _combatMelee;
    private int _combatRange;
    private int _lifePoint;
    private int _sizeRegiment;
    /*
     public enum Grades
    {
        foot,
        horse,
        artillerie
    };
    private Grades _grade;
    */
    #endregion Attributs
    /// <summary>
    /// Constructor, need a Regiment to exist
    /// </summary>
    /// <param name="_name">Unit Name</param>
    /// <param name="_combatMelee">Melee combat value</param>
    /// <param name="_combatRange">Range combat value</param>
    /// <param name="_lifePoint">life point</param>
    /// <param name="_sizeRegiment">size of a regiment/group</param>
    #region Constructor
    public Unit(string name, int combatMelee, int combatRange, int lifePoint, int sizeRegiment)
    {
        _name = name;
        _combatMelee = combatMelee;
        _combatRange = combatRange;
        _lifePoint = lifePoint;
        //json regiment
        _sizeRegiment = sizeRegiment;
    }
    #endregion Constructor

    #region Methods
    #endregion Methods

    #region Accessors
    /// <summary>
    /// Unit Name
    /// </summary>
    public string Name
    {
        get { return _name; }
    }
    /// <summary>
    /// Unit combat value when attacking in melee
    /// </summary>
    public int CombatMelee
    {
        get { return _combatMelee; }
    }
    /// <summary>
    /// Unit combat value when attacking at range
    /// </summary>
    public int CombatRange
    {
        get { return _combatRange; }
    }
    /// <summary>
    /// how many life point the Unit has
    /// </summary>
    public int LifePoint
    {
        get { return _lifePoint; }
        set { _lifePoint = value; }
    }
    /// <summary>
    /// return how many unit compose a regiment/group
    /// </summary>
    public int SizeRegiment
    {
        get { return _sizeRegiment; }
    }
    /*
    public Grades grade
    {
        get { return _grade; }
    }
    */
    #endregion Accessors
}
