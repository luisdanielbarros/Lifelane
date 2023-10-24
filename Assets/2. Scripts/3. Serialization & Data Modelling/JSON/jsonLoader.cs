using System;
using System.Collections.Generic;
using UnityEngine;
public enum jsonWarpType { Literal, Event, Battle }
public class jsonLoader : MonoBehaviour
{
    //Instance
    private static jsonLoader instance = null;
    public static jsonLoader Instance { get { return instance; } }
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
    }
    #region Public Loaders
    //Load Event Data
    public eventData loadEventData(string jsonPath)
    {
        //Find the JSON File
        TextAsset myTextAsset = Resources.Load("JSON/Event System/" + jsonPath) as TextAsset;
        if (myTextAsset == null)
        {
            Debug.LogError("Error at jsonLoader.cs, method loadEventData, could not find file at " + jsonPath);
            return null;
        }
        jsonEventData jsonEventData = JsonUtility.FromJson<jsonEventData>(myTextAsset.text);

        //Get the data
        //0. Story Flag
        storyFlags jsonStoryFlag = (storyFlags)jsonEventData.storyflag;
        int jsonStoryFlagIndex = jsonEventData.storyflagindex;
        bool jsonIncStoryFlag = jsonEventData.incstoryflag;
        bool jsonIncStoryFlagIndex = jsonEventData.incstoryflagindex;

        //1. Music
        string[] musicList = jsonEventData.music.Split(',');
        soundLibEntry[] Music = new soundLibEntry[musicList.Length];
        for (int i = 0; i < musicList.Length; i++)
        {
            Music[i] = convertToSoundLibEntry(musicList[i]);
        }

        //2. Player Coordinates
        bool jsonTeleportsPlayer = jsonEventData.teleportsplayer;
        Vector3 jsonPlayerCoordinates = new Vector3(jsonEventData.playercoordinatesx, jsonEventData.playercoordinatesy, jsonEventData.playercoordinatesz);
        //2. Player Rotation
        float jsonPlayerRotation = jsonEventData.playerrotation;
        //0. Player Movement
        playerMovement jsonPlayerMovement = convertToPlayerMovement(jsonEventData.playermovement);

        //4. Interactions
        List<Interactable> eventInteractions = new List<Interactable>();
        //Regular
        if (jsonEventData.interactioncutscene != null && jsonEventData.interactionregular.Length > 1)
        {
            string interactionName = jsonEventData.interactionregular[0];
            int arrayLength = (int)((jsonEventData.interactionregular.Length - 1) / 3);
            string[] interacionDialogue = new string[arrayLength];
            interactionImgLibEntry[][] interactionImages = new interactionImgLibEntry[arrayLength][];
            soundLibEntry[][] interactionMusic = new soundLibEntry[arrayLength][];
            for (int i = 1; i < jsonEventData.interactionregular.Length; i += 3)
            {
                //Text
                int arrayIndex = (int)((i - 1) / 3);
                interacionDialogue[arrayIndex] = jsonEventData.interactionregular[i];
                //Images
                string[] imageList = jsonEventData.interactionregular[i + 1].Split(',');
                interactionImages[arrayIndex] = new interactionImgLibEntry[3];
                interactionImages[arrayIndex][0] = convertToInteractionImgLibEntry(imageList[0]);
                if (imageList.Length > 1) interactionImages[arrayIndex][1] = convertToInteractionImgLibEntry(imageList[1]);
                if (imageList.Length > 2) interactionImages[arrayIndex][2] = convertToInteractionImgLibEntry(imageList[2]);
                //Music
                string[] interactionMusicList = jsonEventData.interactionregular[i + 2].Split(',');
                interactionMusic[arrayIndex] = new soundLibEntry[interactionMusicList.Length];
                for (int j = 0; j < interactionMusicList.Length; j++)
                {
                    interactionMusic[arrayIndex][j] = convertToSoundLibEntry(interactionMusicList[j]);
                }
            }
            eventInteractions.Add(new interactableRegular(interacionDialogue, interactionImages, interactionMusic, interactionName));
        }
        //Cutscene
        if (jsonEventData.interactioncutscene != null && jsonEventData.interactioncutscene.Length > 0)
        {
            eventInteractions.Add(new interactableCutscene(jsonEventData.interactioncutscene));
        }
        //Pickup Item
        if (jsonEventData.interactionpickupitem != null && jsonEventData.interactionpickupitem.Length > 1)
        {
            string[] interactionPickupItem = jsonEventData.interactionpickupitem;
            string[] Dialogue = new string[interactionPickupItem.Length - 4];
            for (int i = 0; i < interactionPickupItem.Length - 4; i++)
            {
                Dialogue[i] = interactionPickupItem[i + 4];
            }
            string itemId = interactionPickupItem[0];
            soundLibEntry itemMusic = convertToSoundLibEntry(interactionPickupItem[1]);
            soundLibEntry itemPickUpMusic = convertToSoundLibEntry(interactionPickupItem[2]);
            particleEffect itemEffect = convertToParticleEffect(interactionPickupItem[3]);
            eventInteractions.Add(new interactablePickupItem(Dialogue, itemId, itemMusic, itemPickUpMusic, itemEffect));
        }

        //5. Collectible Item
        string collectibleItemId = jsonEventData.collectibleitemid;

        //5. Map Warp
        string mapWarp = jsonEventData.warpdata;

        //5. Battles Data
        battleData[] battleData = null;
        if (jsonEventData.battlesdata) battleData = new battleData[] { loadBattleData(jsonPath) };

        //6. Game Modes
        bool storyMode = jsonEventData.storymode;
        bool explorationMode = jsonEventData.explorationmode;

        //7. Repeat
        bool Repeat = jsonEventData.repeat;

        //Create the data structure & return it
        eventData newEventData = new eventData(jsonStoryFlag, jsonStoryFlagIndex, jsonIncStoryFlag, jsonIncStoryFlagIndex, Music, jsonTeleportsPlayer, jsonPlayerCoordinates, jsonPlayerRotation, jsonPlayerMovement, eventInteractions.ToArray(), collectibleItemId, mapWarp, battleData, storyMode, explorationMode, Repeat);
        return newEventData;
    }
    //Load Warp Data
    public warpData loadWarpData(string jsonPath, jsonWarpType Type)
    {
        //Find the JSON File
        if (Type == jsonWarpType.Literal) jsonPath = "JSON/Literal Warps/" + jsonPath;
        else if (Type == jsonWarpType.Event) jsonPath = "JSON/Event System/" + jsonPath + ", Warp";
        else if (Type == jsonWarpType.Battle) jsonPath = "JSON/Battle System/Warps/" + jsonPath;
        TextAsset myTextAsset = Resources.Load(jsonPath) as TextAsset;
        if (myTextAsset == null)
        {
            Debug.LogError("Error at jsonLoader.cs, method loadWarpData, could not find file at " + jsonPath);
            return null;
        }
        jsonWarpData jsonEventData = JsonUtility.FromJson<jsonWarpData>(myTextAsset.text);
        int buildIndex = jsonEventData.buildindex;
        string levelName = jsonEventData.levelname;
        bool showLevelName = jsonEventData.showlevelname;
        soundLibEntry Music = convertToSoundLibEntry(jsonEventData.music);
        soundLibEntry audioIndustrial = convertToSoundLibEntry(jsonEventData.audioindustrial);
        soundLibEntry audioNature = convertToSoundLibEntry(jsonEventData.audionature);
        soundLibEntry audioWeather = convertToSoundLibEntry(jsonEventData.audioweather);
        Vector3 playerCoordinates = new Vector3(jsonEventData.playercoordinatesx, jsonEventData.playercoordinatesy, jsonEventData.playercoordinatesz);
        float playerRotation = jsonEventData.playerrotation;
        playerPerspective playerPerspective = convertToPlayerPerspective(jsonEventData.playerperspective);
        Weather levelWeather = convertToWeather(jsonEventData.weather);
        bool isTimeRunning = jsonEventData.istimerunning;
        warpData newWarpData = new warpData(buildIndex, levelName, showLevelName, Music, audioIndustrial, audioNature, audioWeather, playerCoordinates, playerRotation, playerPerspective, levelWeather, isTimeRunning);
        return newWarpData;
    }
    //Load Battle Entity
    public battleEntity loadBattleEntity(string[] jsonEntity)
    {
        string entityIdentifier = jsonEntity[0];
        int Experience = 0;
        if (!Int32.TryParse(jsonEntity[1], out Experience)) return null;
        //Find the JSON File
        TextAsset myTextAsset = Resources.Load("JSON/Battle System/Entities/" + entityIdentifier) as TextAsset;
        if (myTextAsset == null)
        {
            Debug.LogError("Error at jsonLoader.cs, method loadBattleEntity, could not find file at " + entityIdentifier);
            return null;
        }
        jsonBattleEntity _jsonBattleEntity = JsonUtility.FromJson<jsonBattleEntity>(myTextAsset.text);
        //Name
        string Name = _jsonBattleEntity.name;
        //Stats
        int Health = _jsonBattleEntity.health;
        int Anima = _jsonBattleEntity.anima;
        int Strength = _jsonBattleEntity.strength;
        int Toughness = _jsonBattleEntity.toughness;
        int Channelling = _jsonBattleEntity.channelling;
        int Sensitivity = _jsonBattleEntity.sensitivity;
        int Speed = _jsonBattleEntity.speed;
        int Intuition = _jsonBattleEntity.intuition;
        battleStats Stats = new battleStats(Health, Anima, Strength, Toughness, Channelling, Sensitivity, Speed, Intuition, Experience);
        //Moves
        int Level = Stats.Level;
        string[] jsonLevelUpMoves = _jsonBattleEntity.levelupmoves;
        List<battleMove> Moves = new List<battleMove>();
        for (int i = 0; i < jsonLevelUpMoves.Length; i++)
        {
            string[] moveInfo = jsonLevelUpMoves[i].Split(',');
            int moveLevel = 999;
            if (Int32.TryParse(moveInfo[0], out moveLevel) && moveLevel <= Level)
            {
                string moveName = moveInfo[1].Trim();
                Moves.Add(loadBattleMoves(moveName));
            }
        }
        return new battleEntity(Name, Stats, Moves.ToArray());
    }
    private battleData loadBattleData(string jsonPath)
    {
        //Find the JSON File
        jsonPath = jsonPath + ", Battle";
        TextAsset myTextAsset = Resources.Load("JSON/Event System/" + jsonPath) as TextAsset;
        if (myTextAsset == null)
        {
            Debug.LogError("Error at jsonLoader.cs, method loadBattleData, could not find file at " + jsonPath);
            return null;
        }
        jsonBattleData _jsonBattleData = JsonUtility.FromJson<jsonBattleData>(myTextAsset.text);
        battleEntity playerEntity = loadBattleEntity(new string[] { "sam", "3181" });
        battleEntity partnerEntity = loadBattleEntity(new string[] { "marie", "3181" });
        battleEntity enemyOneEntity = null;
        battleEntity enemyTwoEntity = null;
        battleEntity enemyThreeEntity = null;
        battleEntity enemyFourEntity = null;
        if (_jsonBattleData.enemyone.Length > 0)
        {
            string Name = _jsonBattleData.enemyone[0];
            int Level = 1;
            int Experience = 0;
            if (Int32.TryParse(_jsonBattleData.enemyone[1], out Level)) Experience = calculateExperienceNeeded(Level);
            enemyOneEntity = loadBattleEntity(new string[] { Name, Experience.ToString() });
        }
        if (_jsonBattleData.enemytwo.Length > 0)
        {
            string Name = _jsonBattleData.enemytwo[0];
            int Level = 1;
            int Experience = 0;
            if (Int32.TryParse(_jsonBattleData.enemytwo[1], out Level)) Experience = calculateExperienceNeeded(Level);
            enemyTwoEntity = loadBattleEntity(new string[] { Name, Experience.ToString() });
        }
        if (_jsonBattleData.enemythree.Length > 0)
        {
            string Name = _jsonBattleData.enemythree[0];
            int Level = 1;
            int Experience = 0;
            if (Int32.TryParse(_jsonBattleData.enemythree[1], out Level)) Experience = calculateExperienceNeeded(Level);
            enemyThreeEntity = loadBattleEntity(new string[] { Name, Experience.ToString() });
        }
        if (_jsonBattleData.enemyfour.Length > 0)
        {
            string Name = _jsonBattleData.enemyfour[0];
            int Level = 1;
            int Experience = 0;
            if (Int32.TryParse(_jsonBattleData.enemyfour[1], out Level)) Experience = calculateExperienceNeeded(Level);
            enemyFourEntity = loadBattleEntity(new string[] { Name, Experience.ToString() });
        }
        battleScenes Scene = battleScenes.Urban;
        soundLibEntry Music = convertToSoundLibEntry(_jsonBattleData.music);
        return new battleData(playerEntity, partnerEntity, enemyOneEntity, enemyTwoEntity, enemyThreeEntity, enemyFourEntity, Scene, Music);
    }
    //Load Character Data
    public characterData loadCharacterData(string jsonPath)
    {
        //Find the JSON File
        TextAsset myTextAsset = Resources.Load("JSON/Storage System/Characters/" + jsonPath) as TextAsset;
        if (myTextAsset == null)
        {
            Debug.LogError("Error at jsonLoader.cs, method loadCharacterData, could not find file at " + jsonPath);
            return null;
        }
        jsonCharacterData _jsonCharacterData = JsonUtility.FromJson<jsonCharacterData>(myTextAsset.text);
        string Name = _jsonCharacterData.name;
        Sprite imageProfile = Resources.Load<Sprite>("/Sprites/Team Images/" + _jsonCharacterData.imageprofile);
        string Gender = _jsonCharacterData.gender;
        string Age = _jsonCharacterData.age;
        string Personality = _jsonCharacterData.personality;
        modelData Physic = loadModelData(_jsonCharacterData.physic);
        modelData Head = loadModelData(_jsonCharacterData.head);
        modelData Eyes = loadModelData(_jsonCharacterData.eyes);
        modelData Hair = loadModelData(_jsonCharacterData.hair);
        modelData Jacket = loadModelData(_jsonCharacterData.jacket);
        modelData Shirt = loadModelData(_jsonCharacterData.shirt);
        modelData Pants = loadModelData(_jsonCharacterData.pants);
        modelData Socks = loadModelData(_jsonCharacterData.socks);
        modelData Shoes = loadModelData(_jsonCharacterData.shoes);
        modelData Acessory1 = loadModelData(_jsonCharacterData.acessory1);
        modelData Acessory2 = loadModelData(_jsonCharacterData.acessory2);
        modelData Acessory3 = loadModelData(_jsonCharacterData.acessory3);
        modelData[] Acessories = new modelData[] { Acessory1, Acessory2, Acessory3 };
        float headScale = _jsonCharacterData.headscale;
        float bodyScale = _jsonCharacterData.bodyscale;
        modelSetData Model = new modelSetData(Physic, Head, Eyes, Hair, Jacket, Shirt, Pants, Socks, Shoes, Acessories, headScale, bodyScale);
        return new characterData(Name, imageProfile, Gender, Age, Personality, Model);
    }
    //Load Item Data
    public itemData loadItemData(string jsonPath)
    {
        //Find the JSON File
        TextAsset myTextAsset = Resources.Load("JSON/Storage System/Items/" + jsonPath) as TextAsset;
        if (myTextAsset == null)
        {
            Debug.LogError("Error at jsonLoader.cs, method loadItemData, could not find file at " + jsonPath);
            return null;
        }
        jsonItemData _jsonItemData = JsonUtility.FromJson<jsonItemData>(myTextAsset.text);
        string itemId = jsonPath;
        string Name = _jsonItemData.name;
        string Description = _jsonItemData.description;
        Sprite itemIcon = Resources.Load<Sprite>("/Sprites/Item Icons/" + _jsonItemData.itemicon);
        itemDataType Type = convertToItemType(_jsonItemData.type);
        itemRarity Rarity = convertToItemRarity(_jsonItemData.rarity);
        int slotSpace = _jsonItemData.slotspace;
        return new itemData(itemId, Name, Description, itemIcon, Type, Rarity, slotSpace);
    }
    //Load Note
    public jsonNote loadNote(string jsonPath)
    {
        //Find the JSON File
        TextAsset myTextAsset = Resources.Load("JSON/Storage System/Items/" + jsonPath + ", note") as TextAsset;
        if (myTextAsset == null)
        {
            Debug.LogError("Error at jsonLoader.cs, method loadNote, could not find file at " + jsonPath);
            return null;
        }
        jsonNote _jsonNote = JsonUtility.FromJson<jsonNote>(myTextAsset.text);
        return _jsonNote;
    }
    //Load Model Data
    public modelData loadModelData(string jsonPath)
    {
        //Find the JSON File
        TextAsset myTextAsset = Resources.Load("JSON/Generator System/Characters/" + jsonPath) as TextAsset;
        if (myTextAsset == null)
        {
            Debug.LogError("Error at jsonLoader.cs, method loadItemData, could not find file at " + jsonPath);
            return null;
        }
        jsonModelData _jsonModelData = JsonUtility.FromJson<jsonModelData>(myTextAsset.text);
        string modelId = jsonPath;
        string Name = _jsonModelData.name;
        modelDataType Type = convertToModelType(_jsonModelData.type);
        string Path = _jsonModelData.path;
        modelDataMaterial Material1 = convertToModelMaterial(_jsonModelData.material1);
        modelDataMaterial Material2 = convertToModelMaterial(_jsonModelData.material2);
        modelDataMaterial Material3 = convertToModelMaterial(_jsonModelData.material3);
        return new modelData(modelId, Name, Type, Path, new modelDataMaterial[] { Material1, Material2, Material3 });
    }
    #endregion
    //Private Loaders
    #region Private Loaders
    #region Battle System Private Loaders
    //Load Battle Moves
    private battleMove loadBattleMoves(string moveName)
    {
        //Find the JSON File
        TextAsset myTextAsset = Resources.Load("JSON/Battle System/Moves/" + moveName) as TextAsset;
        if (myTextAsset == null)
        {
            Debug.LogError("Error at jsonLoader.cs, method loadBattleMoves, could not find file at " + moveName);
            return null;
        }
        jsonBattleMove _jsonBattleMove = JsonUtility.FromJson<jsonBattleMove>(myTextAsset.text);
        battleMoves Move = convertToBattleMoves(_jsonBattleMove.move);
        string Name = _jsonBattleMove.move;
        battleMoveTypes Type = convertToBattleMoveTypes(_jsonBattleMove.type);
        battleMoveRanges Range = convertToBattleMoveRanges(_jsonBattleMove.range);
        bool Contact = _jsonBattleMove.contact;
        int Damage = _jsonBattleMove.damage;
        int Accuracy = _jsonBattleMove.accuracy;
        int Hits = _jsonBattleMove.hits;
        return new battleMove(Move, Name, Type, Range, Contact, Damage, Accuracy, Hits);
    }
    #endregion
    #endregion
    //Convertors
    #region Convertors
    #region General Convertors
    //Sound Library Entry
    private soundLibEntry convertToSoundLibEntry(string jsonMusic)
    {
        switch (jsonMusic)
        {
            case "MUSIC_NONE":
                return soundLibEntry.MUSIC_NONE;
            case "MUSIC_MAIN_MENU":
                return soundLibEntry.MUSIC_MAIN_MENU;
            case "MUSIC_OVERWORLD_1":
                return soundLibEntry.MUSIC_OVERWORLD_1;
            case "MUSIC_NOSTALGIA_1":
                return soundLibEntry.MUSIC_NOSTALGIA_1;
            case "MUSIC_LEVEL_HORROR_1":
                return soundLibEntry.MUSIC_LEVEL_HORROR_1;
            case "INDUSTRIAL_NONE":
                return soundLibEntry.INDUSTRIAL_NONE;
            case "INDUSTRIAL_1":
                return soundLibEntry.INDUSTRIAL_1;
            case "NATURE_NONE":
                return soundLibEntry.NATURE_NONE;
            case "NATURE_1":
                return soundLibEntry.NATURE_1;
            case "WEATHER_NONE":
                return soundLibEntry.WEATHER_NONE;
            case "WEATHER_RAIN":
                return soundLibEntry.WEATHER_RAIN;
            case "EXTRA_MACHINES_NONE":
                return soundLibEntry.SFX_EXTRA_MACHINES_NONE;
            case "CAR_1":
                return soundLibEntry.SFX_EXTRA_MACHINES_CAR_1;
            case "CAR_2":
                return soundLibEntry.SFX_EXTRA_MACHINES_CAR_2;
            case "CAR_3":
                return soundLibEntry.SFX_EXTRA_MACHINES_CAR_3;
            case "CREEPY":
                return soundLibEntry.SFX_EXTRA_MACHINES_CREEPY;
            case "EMERGENCY":
                return soundLibEntry.SFX_EXTRA_MACHINES_EMERGENCY;
            case "RADIO":
                return soundLibEntry.SFX_EXTRA_MACHINES_RADIO_1;
            case "EXTRA_HUMANS_NONE":
                return soundLibEntry.SFX_EXTRA_HUMANS_NONE;
            case "BREATH":
                return soundLibEntry.SFX_EXTRA_BREATH;
            case "WHISPERS":
                return soundLibEntry.SFX_EXTRA_WHISPERS;
            case "BATTLE_NORMAL":
                return soundLibEntry.BATTLE_NORMAL;
            case "NO_CHANGE":
                return soundLibEntry.NO_CHANGE;
            default:
                return soundLibEntry.NO_CHANGE;
        }
    }
    //Player Perspective
    private playerPerspective convertToPlayerPerspective(string jsonPlayerPerspective)
    {
        switch (jsonPlayerPerspective)
        {
            case "controlled":
                return playerPerspective.Controlled;
            case "pov":
                return playerPerspective.POV;
            case "mapped":
                return playerPerspective.Mapped;
            default:
                return playerPerspective.None;
        }
    }
    //Player Movement
    private playerMovement convertToPlayerMovement(string jsonPlayerMovement)
    {
        switch (jsonPlayerMovement)
        {
            case "normal":
                return playerMovement.Normal;
            case "quiet":
                return playerMovement.Quiet;
            case "unchanged":
            default:
                return playerMovement.Unchanged;
        }
    }
    #endregion
    #region Event System Convertors
    //Interaction Image Library
    private interactionImgLibEntry convertToInteractionImgLibEntry(string jsoninteractionImg)
    {
        switch (jsoninteractionImg)
        {
            //Story
            ////Introduction
            case "Story1PG1":
                return interactionImgLibEntry.Story1PG1;
            case "Story1PG2":
                return interactionImgLibEntry.Story1PG2;
            case "Story1PG3":
                return interactionImgLibEntry.Story1PG3;
            case "Story1PG4":
                return interactionImgLibEntry.Story1PG4;
            case "Story1PG5":
                return interactionImgLibEntry.Story1PG5;
            case "Story1PG6":
                return interactionImgLibEntry.Story1PG6;
            case "Story1PG7":
                return interactionImgLibEntry.Story1PG7;
            case "Story1PG8":
                return interactionImgLibEntry.Story1PG8;
            case "Story1PG9":
                return interactionImgLibEntry.Story1PG9;
            case "Story1PG10":
                return interactionImgLibEntry.Story1PG10;
            case "Story1PG11":
                return interactionImgLibEntry.Story1PG11;
            case "Story1PG12":
                return interactionImgLibEntry.Story1PG12;
            //Characters
            ////Samuel
            case "CharSamNeutral":
                return interactionImgLibEntry.CharSamNeutral;
            case "CharSamNeutral2":
                return interactionImgLibEntry.CharSamNeutral2;
            case "CharSamSpeaking":
                return interactionImgLibEntry.CharSamSpeaking;
            case "CharSamSpeaking2":
                return interactionImgLibEntry.CharSamSpeaking2;
            case "CharSamThinking":
                return interactionImgLibEntry.CharSamThinking;
            case "CharSamThinking2":
                return interactionImgLibEntry.CharSamThinking2;
            case "CharSamPsychologicalPain":
                return interactionImgLibEntry.CharSamPsychologicalPain;
            case "CharSamPsychologicalPain2":
                return interactionImgLibEntry.CharSamPsychologicalPain2;
            case "CharSamPhysicalPain":
                return interactionImgLibEntry.CharSamPhysicalPain;
            case "CharSamPhysicalPain2":
                return interactionImgLibEntry.CharSamPhysicalPain2;
            case "CharSamApology":
                return interactionImgLibEntry.CharSamApology;
            case "CharSamApology2":
                return interactionImgLibEntry.CharSamApology2;
            case "CharSamGettingUp":
                return interactionImgLibEntry.CharSamGettingUp;
            case "CharSamGettingUp2":
                return interactionImgLibEntry.CharSamGettingUp2;
            case "CharSamConfused":
                return interactionImgLibEntry.CharSamConfused;
            case "CharSamConfused2":
                return interactionImgLibEntry.CharSamConfused2;

            case "CharMarieNeutral":
                return interactionImgLibEntry.CharMarieNeutral;
            case "CharMarieSpeaking":
                return interactionImgLibEntry.CharMarieSpeaking;
            case "CharMarieThinking":
                return interactionImgLibEntry.CharMarieThinking;
            case "CharMariePain":
                return interactionImgLibEntry.CharMariePain;

            default:
                return interactionImgLibEntry.None;
        }
    }
    //Weather
    private Weather convertToWeather(string jsonWeather)
    {
        switch (jsonWeather)
        {
            case "default":
                return Weather.Default;
            case "clear":
                return Weather.Clear;
            case "windy":
                return Weather.Windy;
            case "rain":
                return Weather.Rain;
            case "snow":
                return Weather.Snow;
            default:
                return Weather.Default;
        }
    }
    //Particle Effect
    private particleEffect convertToParticleEffect(string jsonEffect)
    {
        switch (jsonEffect)
        {
            case "glow":
                return particleEffect.Glow;
            default:
                return particleEffect.None;
        }
    }
    #endregion
    #region Battle System Convertors
    private battleScenes convertToBattleScenes(string jsonScene)
    {
        switch (jsonScene)
        {
            case "Urban":
                return battleScenes.Urban;
            default:
                return battleScenes.None;
        }
    }
    //Battle Moves
    private battleMoves convertToBattleMoves(string jsonBattleMove)
    {
        switch (jsonBattleMove)
        {
            case "basicAttack":
                return battleMoves.basicAttack;
            case "Goo":
                return battleMoves.Goo;
            case "Poltergeist":
                return battleMoves.Poltergeist;
            default:
                return battleMoves.basicAttack;
        }
    }
    //Battle Move Types
    private battleMoveTypes convertToBattleMoveTypes(string jsonBattleMoveType)
    {
        switch (jsonBattleMoveType)
        {
            case "Normal":
                return battleMoveTypes.Normal;
            case "Supernatural":
                return battleMoveTypes.Supernatural;
            default:
                return battleMoveTypes.Normal;
        }
    }
    //Battle Move Ranges
    private battleMoveRanges convertToBattleMoveRanges(string jsonBattleMoveRange)
    {
        switch (jsonBattleMoveRange)
        {
            case "Close":
                return battleMoveRanges.Close;
            case "Medium":
                return battleMoveRanges.Medium;
            case "Long":
                return battleMoveRanges.Long;
            default:
                return battleMoveRanges.Close;
        }
    }
    //Experience Needed
    private int calculateExperienceNeeded(int _Level)
    {
        //Experience Function Variables
        float Exponent = 1.125f;
        int baseExperience = 1000;
        int experienceNeeded = 0;
        for (int i = 1; i < _Level; i++)
        {
            experienceNeeded += (int)Math.Round(baseExperience * Math.Pow(i, Exponent));
        }
        return experienceNeeded;
    }
    #endregion
    #region Storage System Convertors
    //Item Type
    private itemDataType convertToItemType(string jsonType)
    {
        switch (jsonType)
        {
            case "Collectible":
                return itemDataType.Collectible;
            case "Medicine":
                return itemDataType.Medicine;
            case "Weapon":
                return itemDataType.Weapon;
            case "Armor":
                return itemDataType.Armor;
            default:
                return itemDataType.None;
        }
    }
    //Item Rarity
    private itemRarity convertToItemRarity(string jsonRarity)
    {
        switch (jsonRarity)
        {
            case "Common":
                return itemRarity.Common;
            case "Uncommon":
                return itemRarity.Uncommon;
            case "Rare":
                return itemRarity.Rare;
            case "Masterwork":
                return itemRarity.Masterwork;
            case "Mythic":
                return itemRarity.Mythic;
            default:
                return itemRarity.None;
        }
    }
    #endregion
    #region Generator System Convertors
    //Model Type
    private modelDataType convertToModelType(string jsonType)
    {
        switch (jsonType)
        {
            case "physic":
                return modelDataType.Physic;
            case "head":
                return modelDataType.Head;
            case "hair":
                return modelDataType.Hair;
            case "shirt":
                return modelDataType.Shirt;
            case "pants":
                return modelDataType.Pants;
            case "socks":
                return modelDataType.Socks;
            case "acessory":
                return modelDataType.Acessory;
            case "none":
                return modelDataType.None;
            default:
                return modelDataType.None;
        }
    }
    //Model Material
    private modelDataMaterial convertToModelMaterial(string jsonMaterial)
    {
        switch (jsonMaterial)
        {
            case "eyessamuel":
                return modelDataMaterial.EyesSamuel;
            case "hairsamuela":
                return modelDataMaterial.HairSamuelA;
            case "hairsamuelb":
                return modelDataMaterial.HairSamuelB;
            case "hairsamuelc":
                return modelDataMaterial.HairSamuelC;
            case "headsamuel":
                return modelDataMaterial.HeadSamuel;
            case "bodysamuel":
                return modelDataMaterial.BodySamuel;
            case "cashmerewhite":
                return modelDataMaterial.CashmereWhite;
            case "cashmereblack":
                return modelDataMaterial.CashmereBlack;
            case "cashmerelightblue":
                return modelDataMaterial.CashmereDarkBlue;
            case "cashmeredarkblue":
                return modelDataMaterial.CashmereLightBlue;
            case "cashmereimage1":
                return modelDataMaterial.CashmereImage1;
            case "denimlightblue":
                return modelDataMaterial.DenimLightBlue;
            case "denimdarkblue":
                return modelDataMaterial.DenimDarkBlue;
            case "metal":
                return modelDataMaterial.MedievalMetal;
            case "leather":
                return modelDataMaterial.ItalianLeather;
            case "chainmail":
                return modelDataMaterial.ChainMail;
            case "unchangeable":
                return modelDataMaterial.Unchangeable;
            default:
                return modelDataMaterial.None;
        }
    }
    #endregion
    #endregion
}
