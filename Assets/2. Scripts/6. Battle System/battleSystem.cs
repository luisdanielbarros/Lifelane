using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//Battle States
public enum battleStates { Start, PlayersTurn, playersSelection, PartnersTurn, partnersSelection, EnemiesTurn, Win, Loss, Ran }
[Serializable]
public class battleSystem : MonoBehaviour
{
    #region Variables
    //Instance
    private static battleSystem instance = null;
    public static battleSystem Instance { get { return instance; } }
    //Battle State
    private battleStates battlestate;
    public battleStates battleState
    {
        get { return battlestate; }
        set { battlestate = value; manageBattleState(); }
    }
    //Battles Data
    private battleData[] battlesdata;
    public battleData[] battlesData { get { return battlesdata; } }
    private int battleIndex;
    //Battle Units
    [SerializeField]
    private battleUnit playerUnit;
    [SerializeField]
    private battleUnit partnerUnit;
    [SerializeField]
    private battleUnit enemyUnit;
    [SerializeField]
    private battleUnit enemyUnitTwo;
    [SerializeField]
    private battleUnit enemyUnitThree;
    [SerializeField]
    private battleUnit enemyUnitFour;
    //Battle Unit Selector
    private battleUnitSelector unitSelector;
    //Battle Simulator
    private battleSimulator bttlSimulator;
    //Stored information
    private battleMove chosenMove;
    //Random
    private readonly System.Random Rnd = new System.Random();
    //Scenery
    private string jsonPath = "Battle Warp";
    //UI Prefabs
    [SerializeField]
    private GameObject unitAttackBtn;
    //Overworld Return, Integrated w/ Load Level & Event Loader
    //Player Transform
    private Transform playertransform;
    public Transform playerTransform { get { return playertransform; } }
    //Just Battles
    private bool justbattled;
    public bool justBattled
    {
        get
        {
            if (justbattled)
            {
                justbattled = false;
                return true;
            }
            else return justbattled;
        }
    }
    #endregion
    void Awake()
    {
        //Instatiate
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        //Battle Unit Selector
        unitSelector = new battleUnitSelector();
        //Battle Simulator
        bttlSimulator = new battleSimulator();
    }
    void Update()
    {
        if (battleState == battleStates.playersSelection || battleState == battleStates.partnersSelection)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                getSelectedUnit().Unselect();
                unitSelector.changeColumn(true);
                getSelectedUnit().Select();

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                getSelectedUnit().Unselect();
                unitSelector.changeColumn(false);
                getSelectedUnit().Select();
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                chooseAttackTargets();
            }
        }
    }
    #region Battle Methods
    public void setupBattle(battleData[] _battlesData, Transform _playerTransform)
    {
        //Battle State
        battleState = battleStates.Start;
        //Battle Data
        battlesdata = _battlesData;
        battleIndex = 0;
        //Battle Units
        battleData battleIndexData = battlesdata[battleIndex];
        if (battleIndexData.playerEntity != null)
        {
            playerUnit.Serialize(battleIndexData.playerEntity);
            genDropdownMenu(playerUnit.unitAttacks.transform, battleIndexData.playerEntity.Movepool);
            playerUnit.updateHUD();
        }
        if (battleIndexData.partnerEntity != null)
        {
            partnerUnit.Serialize(battleIndexData.partnerEntity);
            genDropdownMenu(partnerUnit.unitAttacks.transform, battleIndexData.partnerEntity.Movepool);
            partnerUnit.updateHUD();
        }
        if (battleIndexData.enemyEntity != null) enemyUnit.Serialize(battleIndexData.enemyEntity);
        if (battleIndexData.enemyTwoEntity != null) enemyUnitTwo.Serialize(battleIndexData.enemyTwoEntity);
        if (battleIndexData.enemyThreeEntity != null) enemyUnitThree.Serialize(battleIndexData.enemyThreeEntity);
        if (battleIndexData.enemyFourEntity != null) enemyUnitFour.Serialize(battleIndexData.enemyFourEntity);
        unitSelector.Reset();
        //Overworld Data
        playertransform = _playerTransform;
        justbattled = true;
        //Start Battle
        startBattle();
    }
    public void startBattle()
    {
        //Serialize the Default Warp Data
        warpData battleWarpData = jsonLoader.Instance.loadWarpData(jsonPath, jsonWarpType.Battle);

        //Override Default Warp Data's Music
        soundLibEntry battleMusic = battlesdata[battleIndex].Music;
        switch (battleMusic)
        {
            case soundLibEntry.BATTLE_NORMAL:
                battleWarpData.audioMusic = battleMusic;
                break;
            default:
                battleWarpData.audioMusic = soundLibEntry.MUSIC_NONE;
                break;
        }

        //Load Level based on the Warp Data
        loadingManager.Instance.LoadLevel(battleWarpData, true);

        //Battle State
        bool isPlayerDead = unitSelector.isPartyMemberDead(0, 0);
        bool isPartnerDead = unitSelector.isPartyMemberDead(0, 1);
        if (!isPlayerDead) battleState = battleStates.PlayersTurn;
        else if (!isPartnerDead) battleState = battleStates.PartnersTurn;
    }
    private void nextBattle()
    {
        battleIndex++;
        if (battleIndex < battlesdata.Length) startBattle();
    }
    private void endBattle()
    {
        switch (battleState)
        {
            case battleStates.Win:
                loadingManager.Instance.LoadPreviousLevel();
                break;
            case battleStates.Loss:
                loadingManager.Instance.LoadPreviousLevel();
                break;
            case battleStates.Ran:
                loadingManager.Instance.LoadPreviousLevel();
                break;
        }
    }
    #endregion
    #region Player Controls
    public void chooseAttack(battleMove _Move)
    {
        if (battleState == battleStates.PlayersTurn)
        {
            chosenMove = _Move;
            unitSelector.resetUnitSelection();
            getSelectedUnit().Select();
            battleState = battleStates.playersSelection;
        }
        else if (battleState == battleStates.PartnersTurn)
        {
            chosenMove = _Move;
            unitSelector.resetUnitSelection();
            getSelectedUnit().Select();
            battleState = battleStates.partnersSelection;
        }
    }
    public void chooseAttackTargets()
    {
        if (battleState != battleStates.playersSelection && battleState != battleStates.partnersSelection) return;

        //Variable Unwrapping
        ////Attacker Stats
        battleStats attackerStats = null;
        if (battleState == battleStates.playersSelection) attackerStats = playerUnit.Stats;
        else if (battleState == battleStates.partnersSelection) attackerStats = partnerUnit.Stats;
        ////Target Entity
        battleUnit targetUnit = getSelectedUnit();
        battleEntity targetEntity = targetUnit.Entity;

        //Battle Simulation
        battleSimulation bttlSimulation = bttlSimulator.Simulate(ref attackerStats, ref targetEntity, chosenMove);

        //Update Units
        updateUnits();

        //Afermath
        battleAftermath(bttlSimulation);
    }
    public void Inventory()
    {

    }
    public void Team()
    {

    }
    public void Run()
    {
        if (battleState != battleStates.PlayersTurn) return;
        //Variable Unwrapping
        ////Attacker Stats
        battleStats attackerStats = playerUnit.Stats;
        ////Target Entity
        battleUnit targetUnit = getSelectedUnit();
        battleEntity targetEntity = targetUnit.Entity;

        //Battle Simulation
        battleSimulation bttlSimulation = bttlSimulator.SimulateRun(ref attackerStats, ref targetEntity);

        //Aftermath
        bool Ran = bttlSimulation.Ran;
        if (Ran) battleState = battleStates.Ran;
    }
    #endregion
    #region Enemy AI
    private void enemiesTurn()
    {
        if (battleState != battleStates.EnemiesTurn) return;

        //Reset Selection
        unitSelector.resetUnitSelection();

        int[] enemiesAlive = unitSelector.getPartyMembersAlive(1);
        battleSimulation bttlSimulation = null;
        for (int i = 0; i < enemiesAlive.Length; i++)
        {
            //Get Enemy Unit
            ////Set Selected Enemy via unitSelector.selectUnit
            unitSelector.selectUnit(1, enemiesAlive[i]);
            ////Obtain Selected Enemy via batleSystem.getSelectedUnit
            battleUnit Enemy = getSelectedUnit();

            //Get Player Unit
            ////Set Selected Ally via unitSelector.selectUnit
            int[] alliesAlive = unitSelector.getPartyMembersAlive(0);
            int randomAliveAllyIndex = Rnd.Next(0, alliesAlive.Length);
            int randomAliveAlly = alliesAlive[randomAliveAllyIndex];
            unitSelector.selectUnit(0, randomAliveAlly);
            ////Obtain Selected Ally via batleSystem.getSelectedUnit
            battleUnit Ally = getSelectedUnit();

            //Variable Unwrapping
            ////Attacker Stats
            battleStats attackerStats = Enemy.Stats;
            ////Target Entity
            battleEntity targetEntity = Ally.Entity;
            ////Move
            battleMove[] attackerMovepool = Enemy.Movepool;
            int moveIndex = Rnd.Next(0, attackerMovepool.Length);
            battleMove attackerMove = attackerMovepool[moveIndex];
            chosenMove = attackerMove;

            //Battle Simulation
            bttlSimulation = bttlSimulator.Simulate(ref attackerStats, ref targetEntity, chosenMove);

            //Update Units
            updateUnits();

            //If it's not the last enemy
            bool isPlayerPartyDead = unitSelector.isPartyDead(0);
            if (isPlayerPartyDead) break;
            else if (i + 1 < enemiesAlive.Length)
            {
                //Set Dead
                bool Died = bttlSimulation.Died;
                if (Died) unitSelector.setDead();
            }

        }
        //Afermath
        battleAftermath(bttlSimulation);
    }
    private void battleAftermath(battleSimulation _bttlSimulation)
    {
        //Set Dead
        bool Died = _bttlSimulation.Died;
        if (Died) unitSelector.setDead();

        //Variables Unwrapping
        bool isPlayerPartyDead = unitSelector.isPartyDead(0);
        bool isEnemyPartyDead = unitSelector.isPartyDead(1);
        bool isPlayerDead = unitSelector.isPartyMemberDead(0, 0);
        bool isPartnerDead = unitSelector.isPartyMemberDead(0, 1);

        //Battle State
        if (isPlayerPartyDead)
        {
            battleState = battleStates.Loss;
        }
        else if (isEnemyPartyDead)
        {
            //Earn Experience
            int experienceEarned = _bttlSimulation.experienceEarned;
            battleState = battleStates.Win;
        }
        else if (battleState == battleStates.playersSelection)
        {
            if (!isPartnerDead)
            {
                battleState = battleStates.PartnersTurn;
            }
            else
            {
                battleState = battleStates.EnemiesTurn;
            }
        }
        else if (battleState == battleStates.partnersSelection)
        {
            battleState = battleStates.EnemiesTurn;
        }
        else if (battleState == battleStates.EnemiesTurn)
        {
            if (!isPlayerDead)
            {
                battleState = battleStates.PlayersTurn;
            }
            else
            {
                battleState = battleStates.PartnersTurn;
            }
        }
    }
    #endregion
    #region Battle Unit Selector
    //Get Selected Unit
    private battleUnit getSelectedUnit()
    {
        switch (unitSelector.getUnit())
        {
            case 0:
                return playerUnit;
            case 1:
            case 2:
            case 3:
                return partnerUnit;
            case 4:
                return enemyUnit;
            case 5:
                return enemyUnitTwo;
            case 6:
                return enemyUnitThree;
            case 7:
                return enemyUnitFour;
            default:
                return null;
        }
    }
    #endregion
    #region Private Utilities
    //Manage Battle State
    private void manageBattleState()
    {
        playerUnit.unitAttacks.SetActive(false);
        partnerUnit.unitAttacks.SetActive(false);
        switch (battleState)
        {
            case battleStates.Start:
                break;
            case battleStates.PlayersTurn:
                playerUnit.unitAttacks.SetActive(true);
                break;
            case battleStates.playersSelection:
                break;
            case battleStates.PartnersTurn:
                partnerUnit.unitAttacks.SetActive(true);
                break;
            case battleStates.partnersSelection:
                break;
            case battleStates.EnemiesTurn:
                enemiesTurn();
                break;
            case battleStates.Win:
            case battleStates.Loss:
            case battleStates.Ran:
                endBattle();
                break;
        }
    }
    //Generate Dropdown
    private void genDropdownMenu(Transform _unitAttacks, battleMove[] _battleMoves)
    {
        if (_unitAttacks != null && unitAttackBtn != null && _battleMoves != null)
        {
            for (int i = 0; i < _battleMoves.Length; i++)
            {
                battleMove Move = _battleMoves[i];
                string moveName = Move.Name;
                //Instatiate & Parent Button Prefab
                GameObject btnObj = Instantiate(unitAttackBtn);
                btnObj.transform.SetParent(_unitAttacks.transform, false);
                //Update the Button's Text
                btnObj.GetComponentInChildren<TextMeshProUGUI>().SetText(moveName);
                //Add the Event Listeners
                //Delete
                btnObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    chooseAttack(Move);
                }
                );
            }
        }
    }
    //Update Units
    private void updateUnits()
    {
        if (playerUnit != null) playerUnit.updateHUD();
        if (partnerUnit != null) partnerUnit.updateHUD();
        if (enemyUnit != null)
        {
            enemyUnit.Unselect();
            enemyUnit.updateHUD();
        }
        if (enemyUnitTwo != null)
        {
            enemyUnitTwo.Unselect();
            enemyUnitTwo.updateHUD();
        }
        if (enemyUnitThree != null)
        {
            enemyUnitThree.Unselect();
            enemyUnitThree.updateHUD();
        }
        if (enemyUnitFour != null)
        {
            enemyUnitFour.Unselect();
            enemyUnitFour.updateHUD();
        }
    }
    #endregion
}
