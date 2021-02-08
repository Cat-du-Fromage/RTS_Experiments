using System;

[Serializable]
public class Units
{
    #region Attributs
    private string _name { get;}
    private int _combatMelee { get;}
    private int _combatRange { get;}
    private int _lifePoint { get; set; }
    private int _sizeRegiment { get; }
    #endregion Attributs
    /// <summary>
    /// Constructor of a single Unit (not the group)
    /// </summary>
    /// <param name="_name">Unit Name</param>
    /// <param name="_combatMelee">Melee combat value</param>
    /// <param name="_combatRange">Range combat value</param>
    /// <param name="_lifePoint">life point</param>
    /// <param name="_sizeRegiment">size of a regiment/group</param>
    #region Constructor
    public Units(string _name, int _combatMelee, int _combatRange, int _lifePoint, int _sizeRegiment)
    {
        this._name = _name;
        this._combatMelee = _combatMelee;
        this._combatRange = _combatRange;
        this._lifePoint = _lifePoint;
        this._sizeRegiment = _sizeRegiment;
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
    #endregion Accessors
}
