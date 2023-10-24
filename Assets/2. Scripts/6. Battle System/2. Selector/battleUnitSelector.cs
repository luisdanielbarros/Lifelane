using System.Collections.Generic;

public class battleUnitSelector
{
    #region Variables
    private int[] enemyUnits, alliedUnits;
    private bool[] selectableEnemies, selectableAllies;
    private int columnindex_DONOTUSE;
    private int columnIndex
    {
        get { return columnindex_DONOTUSE; }
        set
        {
            if (value >= 0 && value <= 3) columnindex_DONOTUSE = value;
            else if (value == -1) columnindex_DONOTUSE = 3;
            else if (value == 4) columnindex_DONOTUSE = 0;
        }
    }
    private int rowindex_DONOTUSE;
    private int rowIndex
    {
        get { return rowindex_DONOTUSE; }
        set
        {
            if (value >= 0 && value <= 1) rowindex_DONOTUSE = value;
            else if (value == -1) rowindex_DONOTUSE = 1;
            else if (value == 2) rowindex_DONOTUSE = 0;
        }
    }
    private int alliesNum;
    #endregion
    //Constructor
    public battleUnitSelector()
    {
        enemyUnits = new int[4] { 4, 5, 6, 7 };
        alliedUnits = new int[4] { 0, 1, 2, 3 };
        selectableEnemies = new bool[4] { true, true, true, true };
        selectableAllies = new bool[4] { true, true, true, true };
        alliesNum = 2;
        Reset();
    }
    //Reset
    public void Reset()
    {
        columnIndex = 0;
        rowIndex = 1;
        for (int i = 0; i < selectableEnemies.Length; i++)
        {
            selectableEnemies[i] = true;
            if (i < alliesNum) selectableAllies[i] = true;
            else selectableAllies[i] = false;
        }
    }
    //Get Unit
    public int getUnit(int? _rowIndex = null, int? _columnIndex = null)
    {
        if (_rowIndex == null) _rowIndex = rowIndex;
        if (_columnIndex == null) _columnIndex = columnIndex;
        if (_rowIndex == 0) return alliedUnits[(int)_columnIndex];
        else return enemyUnits[(int)_columnIndex];
    }
    #region Selection
    //Change Column
    public void changeColumn(bool _Next)
    {
        if (isPartyDead(rowIndex)) return;
        if (_Next) columnIndex++;
        else columnIndex--;
        if (!getUnitSelectability()) changeColumn(_Next);
    }
    //Select Unit
    public bool selectUnit(int _rowIndex, int _columnIndex)
    {
        rowIndex = _rowIndex;
        columnIndex = _columnIndex;
        if (!getUnitSelectability())
        {
            changeColumn(true);
            return false;
        }
        else return true;
    }
    //Get Unit Selectability
    private bool getUnitSelectability()
    {
        if (rowIndex == 0) return selectableAllies[columnIndex];
        else return selectableEnemies[columnIndex];
    }
    //Reset Unit Selection
    public void resetUnitSelection()
    {
        columnIndex = 0;
        rowIndex = 1;
        if (!getUnitSelectability()) changeColumn(true);
    }
    #endregion
    #region Dead Status
    //Set Dead
    public void setDead()
    {
        if (rowIndex == 0) selectableAllies[columnIndex] = false;
        else selectableEnemies[columnIndex] = false;
        changeColumn(true);
    }
    //Is Party Dead
    public bool isPartyDead(int _rowIndex)
    {
        if (_rowIndex == 0)
        {
            for (int i = 0; i < selectableAllies.Length; i++)
            {
                if (selectableAllies[i] == true)
                {
                    return false;
                }
            }
        }
        else
        {
            for (int i = 0; i < selectableEnemies.Length; i++)
            {
                if (selectableEnemies[i] == true)
                {
                    return false;
                }
            }
        }
        return true;
    }
    //Is Party Member Dead
    public bool isPartyMemberDead(int _rowIndex, int _columnIndex)
    {
        if (_rowIndex == 0) return !selectableAllies[_columnIndex];
        else return !selectableEnemies[_columnIndex];
    }
    //Get Party Members Alive
    public int[] getPartyMembersAlive(int _rowIndex)
    {
        List<int> membersAlive = new List<int>();
        if (_rowIndex == 0)
        {
            for (int i = 0; i < selectableAllies.Length; i++)
            {
                if (selectableAllies[i] == true) membersAlive.Add(alliedUnits[i]);
            }
        }
        else
        {
            for (int i = 0; i < selectableEnemies.Length; i++)
            {
                if (selectableEnemies[i] == true) membersAlive.Add(enemyUnits[i]);
            }
        }
        return membersAlive.ToArray();
    }
    #endregion
}
