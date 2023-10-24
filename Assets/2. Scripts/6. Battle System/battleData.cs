using System;
[Serializable]
public class battleData
{
    //Player Entity
    private battleEntity playerentity;
    public battleEntity playerEntity { get { return playerentity; } }

    //Partner Entity
    private battleEntity partnerentity;
    public battleEntity partnerEntity { get { return partnerentity; } }

    //Enemy Entity
    private battleEntity enemyentity;
    public battleEntity enemyEntity { get { return enemyentity; } }

    //Enemy Two Entity
    private battleEntity enemytwoentity;
    public battleEntity enemyTwoEntity { get { return enemytwoentity; } }

    //Enemy Three Entity
    private battleEntity enemythreeentity;
    public battleEntity enemyThreeEntity { get { return enemythreeentity; } }

    //Enemy Four Entity
    private battleEntity enemyfourentity;
    public battleEntity enemyFourEntity { get { return enemyfourentity; } }

    //Scenery
    private battleScenes scene;
    public battleScenes Scene { get { return scene; } }

    //Music
    private soundLibEntry music;
    public soundLibEntry Music { get { return music; } }

    //Constructor
    public battleData(battleEntity _playerEntity, battleEntity _partnerEntity, battleEntity _enemyEntity, battleEntity _enemyTwoEntity, battleEntity _enemyThreeEntity, battleEntity _enemyFourEntity, battleScenes _Scene, soundLibEntry _Music)
    {
        playerentity = _playerEntity;
        partnerentity = _partnerEntity;
        enemyentity = _enemyEntity;
        enemytwoentity = _enemyTwoEntity;
        enemythreeentity = _enemyThreeEntity;
        enemyfourentity = _enemyFourEntity;
        scene = _Scene;
        music = _Music;
    }
}
