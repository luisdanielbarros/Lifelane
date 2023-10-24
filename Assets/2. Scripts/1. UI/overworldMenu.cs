using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class overworldMenu : MonoBehaviour
{
    [SerializeField]
    private battleStats playerStats, partnerStats;
    //UI
    ////Tabs
    [SerializeField]
    private GameObject[] storyModeTabs;
    ////Team Tab
    #region Team UI
    ////Player
    [SerializeField]
    private Image playerImage;
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI playerGender;
    [SerializeField]
    private TextMeshProUGUI playerAge;
    [SerializeField]
    private TextMeshProUGUI playerPersonality;
    [SerializeField]
    private TextMeshProUGUI playerHealth;
    [SerializeField]
    private TextMeshProUGUI playerAnima;
    [SerializeField]
    private TextMeshProUGUI playerStrength;
    [SerializeField]
    private TextMeshProUGUI playerToughness;
    [SerializeField]
    private TextMeshProUGUI playerChannelling;
    [SerializeField]
    private TextMeshProUGUI playerSensitivity;
    [SerializeField]
    private TextMeshProUGUI playerSpeed;
    [SerializeField]
    private TextMeshProUGUI playerIntuition;

    ////Partner
    [SerializeField]
    private Image partnerImage;
    [SerializeField]
    private TextMeshProUGUI partnerName;
    [SerializeField]
    private TextMeshProUGUI partnerGender;
    [SerializeField]
    private TextMeshProUGUI partnerAge;
    [SerializeField]
    private TextMeshProUGUI partnerPersonality;
    [SerializeField]
    private GameObject partnerStatsLabel;
    [SerializeField]
    private TextMeshProUGUI partnerHealth;
    [SerializeField]
    private TextMeshProUGUI partnerAnima;
    [SerializeField]
    private TextMeshProUGUI partnerStrength;
    [SerializeField]
    private TextMeshProUGUI partnerToughness;
    [SerializeField]
    private TextMeshProUGUI partnerChannelling;
    [SerializeField]
    private TextMeshProUGUI partnerSensitivity;
    [SerializeField]
    private TextMeshProUGUI partnerSpeed;
    [SerializeField]
    private TextMeshProUGUI partnerIntuition;
    #endregion
    //Open
    public void Open(gameModes _currentMode)
    {
        if (_currentMode == gameModes.StoryMode)
        {
            for (int i = 0; i < storyModeTabs.Length; i++) storyModeTabs[i].SetActive(true);
            updateInformation();
        }
        else
        {
            for (int i = 0; i < storyModeTabs.Length; i++) storyModeTabs[i].SetActive(false);
        }
    }
    //Update Information
    private void updateInformation()
    {
        //Player Personal Information
        playerImage.sprite = runtimeSaveData.Instance.playerData.imageProfile;
        playerName.SetText(runtimeSaveData.Instance.playerData.Name);
        playerGender.SetText(runtimeSaveData.Instance.playerData.Gender.ToString());
        playerAge.SetText(runtimeSaveData.Instance.playerData.Age.ToString());
        playerPersonality.SetText(runtimeSaveData.Instance.playerData.Personality.ToString());
        //Partner Personal Information
        if (!ReferenceEquals(runtimeSaveData.Instance.partnerData, null) ? false : true)
        {
            if (runtimeSaveData.Instance.partnerData != null)
            {
                partnerImage.gameObject.SetActive(true);
                partnerImage.sprite = runtimeSaveData.Instance.partnerData.imageProfile;
                partnerName.SetText(runtimeSaveData.Instance.partnerData.Name);
                partnerGender.SetText(runtimeSaveData.Instance.partnerData.Gender.ToString());
                partnerAge.SetText(runtimeSaveData.Instance.partnerData.Age.ToString());
                partnerPersonality.SetText(runtimeSaveData.Instance.partnerData.Personality.ToString());
            }
        }
        else
        {
            partnerImage.gameObject.SetActive(false);
            partnerName.SetText("");
            partnerGender.SetText("");
            partnerAge.SetText("");
            partnerPersonality.SetText("");
        }
        //Player Stats
        if (playerStats != null)
        {
            foreach (baseStat finalStat in playerStats.Stats)
            {
                if (finalStat.statName == statsList.Health) playerHealth.SetText("Health: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Anima) playerAnima.SetText("Anima: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Strength) playerStrength.SetText("Strength: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Toughness) playerToughness.SetText("Toughness: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Channelling) playerChannelling.SetText("Channelling: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Sensitivity) playerSensitivity.SetText("Sensitivity: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Speed) playerSpeed.SetText("Speed: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Intuition) playerIntuition.SetText("Intuition: " + finalStat.twiceDerivedValue.ToString());
            }
        }
        else
        {
            playerHealth.SetText("Health: -");
            playerAnima.SetText("Anima: -");
            playerStrength.SetText("Strength: -");
            playerToughness.SetText("Toughness: -");
            playerChannelling.SetText("Channelling: -");
            playerSensitivity.SetText("Sensitivity: -");
            playerSpeed.SetText("Speed: -");
            playerIntuition.SetText("Intuition: -");
        }
        //Partner Stats
        if (partnerStats != null)
        {
            foreach (baseStat finalStat in partnerStats.Stats)
            {
                partnerStatsLabel.SetActive(true);
                if (finalStat.statName == statsList.Health) partnerHealth.SetText("Health: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Anima) partnerAnima.SetText("Anima: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Strength) partnerStrength.SetText("Strength: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Toughness) partnerToughness.SetText("Toughness: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Channelling) partnerChannelling.SetText("Channelling: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Sensitivity) partnerSensitivity.SetText("Sensitivity: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Speed) partnerSpeed.SetText("Speed: " + finalStat.twiceDerivedValue.ToString());
                if (finalStat.statName == statsList.Intuition) partnerIntuition.SetText("Intuition: " + finalStat.twiceDerivedValue.ToString());
            }
        }
        else
        {
            partnerStatsLabel.SetActive(false);
            partnerHealth.SetText("");
            partnerAnima.SetText("");
            partnerStrength.SetText("");
            partnerToughness.SetText("");
            partnerChannelling.SetText("");
            partnerSensitivity.SetText("");
            partnerSpeed.SetText("");
            partnerIntuition.SetText("");
        }
    }
    //Event Listeners
    #region Event Listeneres
    public void Resume()
    {
        gameState.Instance.setGameState(gameStates.OverWorld);
    }
    public void Save()
    {
        gameState.Instance.setGameState(gameStates.SaveMenu);
    }
    public void Options()
    {
        gameState.Instance.setGameState(gameStates.OptionsMenu);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void UIClick()
    {
        AudioManager.Instance.UIClick();
    }
    public void UIHover()
    {
        AudioManager.Instance.UIHover();
    }
    #endregion
}
