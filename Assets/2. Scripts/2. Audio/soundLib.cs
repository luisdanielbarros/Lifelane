using UnityEngine;
public enum soundLibEntry
{
    NO_CHANGE,

    MUSIC_NONE,
    MUSIC_MAIN_MENU,
    MUSIC_OVERWORLD_1,
    MUSIC_NOSTALGIA_1,
    MUSIC_LEVEL_HORROR_1,
    MUSIC_COLLECTIBLES,
    MUSIC_CREDITS,

    INDUSTRIAL_NONE,
    INDUSTRIAL_1,

    NATURE_NONE,
    NATURE_1,

    WEATHER_NONE,
    WEATHER_RAIN,

    BATTLE_NONE,
    BATTLE_NORMAL,

    SFX_UI_CLICK,
    SFX_UI_HOVER,

    SFX_INTERACTION_START,
    SFX_INTERACTION_CONTINUE,
    SFX_INTERACTION_END,

    SFX_PLAYER_WALKING_NONE,
    SFX_PLAYER_WALKING_CONCRETE,

    SFX_EXTRA_MACHINES_NONE,
    SFX_EXTRA_MACHINES_CAR_1,
    SFX_EXTRA_MACHINES_CAR_2,
    SFX_EXTRA_MACHINES_CAR_3,
    SFX_EXTRA_MACHINES_CREEPY,
    SFX_EXTRA_MACHINES_EMERGENCY,
    SFX_EXTRA_MACHINES_RADIO_1,

    SFX_EXTRA_HUMANS_NONE,
    SFX_EXTRA_BREATH,
    SFX_EXTRA_WHISPERS,

    SFX_EXTRA_MONSTERS_NONE,
    SFX_EXTRA_MONSTERS_1

}
public static class soundLib
{

    public static Sound getEntry(soundLibEntry Entry)
    {
        float masterVolume = 0.5f;
        switch (Entry)
        {
            //Music
            ////None
            case soundLibEntry.MUSIC_NONE:
                return new Sound(Entry, audioTrack.MUSIC, 0f, 1f, true, Resources.Load<AudioClip>("Audio/Music/Holizna/HoliznaCC0 - Deja Vu"));
            ////Main Menu
            case soundLibEntry.MUSIC_MAIN_MENU:
                return new Sound(Entry, audioTrack.MUSIC, 1f * masterVolume, 1f, true, Resources.Load<AudioClip>("Audio/Music/Holizna/HoliznaCC0 - Deja Vu"));
            ////OverWorld 1
            case soundLibEntry.MUSIC_OVERWORLD_1:
                return new Sound(Entry, audioTrack.MUSIC, 1f * masterVolume, 1f, true, Resources.Load<AudioClip>("Audio/Music/Holizna/HoliznaCC0 - Something In the Air"));
            ////Nostalgia 1
            case soundLibEntry.MUSIC_NOSTALGIA_1:
                return new Sound(Entry, audioTrack.MUSIC, 1f * masterVolume, 1f, true, Resources.Load<AudioClip>("Audio/Music/Ending Satellites/Ending Satellites - Children@seas"));
            ////Level Horror 1
            case soundLibEntry.MUSIC_LEVEL_HORROR_1:
                return new Sound(Entry, audioTrack.MUSIC, 1f * masterVolume, 1f, true, Resources.Load<AudioClip>("Audio/Industrial/Horror Elements/Ambient/Amb_scary"));
            ////Collectibles
            case soundLibEntry.MUSIC_COLLECTIBLES:
                return new Sound(Entry, audioTrack.MUSIC, 1f * masterVolume, 1f, true, Resources.Load<AudioClip>("Audio/Industrial/Horror Elements/Ambient/Amb_underwater"));
            ////Credits
            case soundLibEntry.MUSIC_CREDITS:
                return new Sound(Entry, audioTrack.MUSIC, 0.5f * masterVolume, 1f, true, Resources.Load<AudioClip>("Audio/Music/GameMusicPack_SUITE/wav/Anime/anime_07_loop"));

            //Industrial
            ////None
            case soundLibEntry.INDUSTRIAL_NONE:
                return new Sound(Entry, audioTrack.AMBIENT_INDUSTRIAL, 0f, 1f, true, Resources.Load<AudioClip>("Audio/Industrial/Horror Elements/Ambient/Amb_Rumble"));

            ////Destruction 1
            case soundLibEntry.INDUSTRIAL_1:
                return new Sound(Entry, audioTrack.AMBIENT_INDUSTRIAL, 1f * masterVolume, 1f, true, Resources.Load<AudioClip>("Audio/Industrial/Horror Elements/Ambient/Amb_Rumble"));

            //Nature
            ////None
            case soundLibEntry.NATURE_NONE:
                return new Sound(Entry, audioTrack.AMBIENT_NATURE, 0f, 1f, true, Resources.Load<AudioClip>("Audio/Natural/Nature - Essentials/Ambiance_Forest_Birds_Loop_Stereo"));
            ////Nature 1
            case soundLibEntry.NATURE_1:
                return new Sound(Entry, audioTrack.AMBIENT_NATURE, 1f * masterVolume, 1f, true, Resources.Load<AudioClip>("Audio/Natural/Nature - Essentials/Ambiance_Forest_Birds_Loop_Stereo"));

            ////None
            //Weather
            case soundLibEntry.WEATHER_NONE:
                return new Sound(Entry, audioTrack.AMBIENT_WEATHER, 0f, 1f, true, Resources.Load<AudioClip>("Audio/Natural/Nature - Essentials/Ambiance_Rain_Calm_Loop_Stereo"));
            ////Nature 1
            case soundLibEntry.WEATHER_RAIN:
                return new Sound(Entry, audioTrack.AMBIENT_WEATHER, 1f * masterVolume, 1f, true, Resources.Load<AudioClip>("Audio/Natural/Nature - Essentials/Ambiance_Rain_Calm_Loop_Stereo"));

            //Battle
            ////None
            case soundLibEntry.BATTLE_NONE:
                return new Sound(Entry, audioTrack.MUSIC, 0f, 1f, true, Resources.Load<AudioClip>("Audio/GameMusicPack_SUITE/wav/EDM/edm_01_loop"));
            ////Normal
            case soundLibEntry.BATTLE_NORMAL:
                return new Sound(Entry, audioTrack.MUSIC, 0f, 1f, true, Resources.Load<AudioClip>("Audio/GameMusicPack_SUITE/wav/EDM/edm_01_loop"));

            //SFX
            ////UI
            //////Click
            case soundLibEntry.SFX_UI_CLICK:
                return new Sound(Entry, audioTrack.SFX_UI_CLICK, 1f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/UI/UI_Buttons_Pack2/Button_11_Pack2"));
            //////Hover
            case soundLibEntry.SFX_UI_HOVER:
                return new Sound(Entry, audioTrack.SFX_UI_HOVER, 1f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/UI/UI_Buttons_Pack2/Button_1_Pack2"));

            ////Interaction
            //////Start
            case soundLibEntry.SFX_INTERACTION_START:
                return new Sound(Entry, audioTrack.SFX_INTERACTION_START, 1f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/UI/UI_Buttons_Pack2/Button_30_Pack2"));
            //////Continue
            case soundLibEntry.SFX_INTERACTION_CONTINUE:
                return new Sound(Entry, audioTrack.SFX_INTERACTION_CONTINUE, 2f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/UI/UI_Buttons_Pack2/Button_30_Pack2"));
            //////End
            case soundLibEntry.SFX_INTERACTION_END:
                return new Sound(Entry, audioTrack.SFX_INTERACTION_END, 1f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/UI/UI_Buttons_Pack2/Button_6_Pack2"));

            ////Player Walking
            //////None
            case soundLibEntry.SFX_PLAYER_WALKING_NONE:
                return new Sound(Entry, audioTrack.SFX_PLAYER_WALKING, 0f, 1f, false, Resources.Load<AudioClip>("Audio/Player/Footsteps - Essentials/Footsteps_Tile/Footsteps_Tile_Walk/Footsteps_Tile_Walk_01"));
            //////Concrete
            case soundLibEntry.SFX_PLAYER_WALKING_CONCRETE:
                return new Sound(Entry, audioTrack.SFX_PLAYER_WALKING, 1f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/Player/Footsteps - Essentials/Footsteps_Tile/Footsteps_Tile_Walk/Footsteps_Tile_Walk_01"));

            //Extra, Machines
            ////None
            case soundLibEntry.SFX_EXTRA_MACHINES_NONE:
                return new Sound(Entry, audioTrack.SFX_EXTRA_MACHINES, 0f, 1f, false, Resources.Load<AudioClip>("Audio/Industrial/Horror Elements/Misc/Misc_Cry"));
            ////Car 1
            case soundLibEntry.SFX_EXTRA_MACHINES_CAR_1:
                return new Sound(Entry, audioTrack.SFX_EXTRA_MACHINES, 2f * masterVolume, 1f, true, Resources.Load<AudioClip>("Audio/Industrial/Vehicle_Essentials/Vehicle_Car/Vehicle_Car_Engine/Vehicle_Car_Engine_1000_RPM_Front_Exterior_Loop"));
            ////Car 2
            case soundLibEntry.SFX_EXTRA_MACHINES_CAR_2:
                return new Sound(Entry, audioTrack.SFX_EXTRA_MACHINES, 2f * masterVolume, 1f, true, Resources.Load<AudioClip>("Audio/Industrial/Vehicle_Essentials/Vehicle_Car/Vehicle_Car_Engine/Vehicle_Car_Engine_1000_RPM_Rear_Exterior_Loop"));
            ////Car 3
            case soundLibEntry.SFX_EXTRA_MACHINES_CAR_3:
                return new Sound(Entry, audioTrack.SFX_EXTRA_MACHINES, 2f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/Industrial/Vehicle_Essentials/Vehicle_Car/Vehicle_Car_Engine/Vehicle_Car_Stop_Engine_Exterior"));
            ////Creepy
            case soundLibEntry.SFX_EXTRA_MACHINES_CREEPY:
                return new Sound(Entry, audioTrack.SFX_EXTRA_MACHINES, 1f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/Industrial/Horror Elements/Misc/Misc_horrific"));
            ////Emergency
            case soundLibEntry.SFX_EXTRA_MACHINES_EMERGENCY:
                return new Sound(Entry, audioTrack.SFX_EXTRA_MACHINES, 1f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/Industrial/Horror Elements/Misc/Misc_Emergency"));
            ////Radio 1
            case soundLibEntry.SFX_EXTRA_MACHINES_RADIO_1:
                return new Sound(Entry, audioTrack.SFX_EXTRA_MACHINES, 1f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/Industrial/Horror Elements/Misc/Misc_Cry"));

            //Extra, Humans
            ////None
            case soundLibEntry.SFX_EXTRA_HUMANS_NONE:
                return new Sound(Entry, audioTrack.SFX_EXTRA_HUMANS, 0f, 1f, false, Resources.Load<AudioClip>("Audio/Industrial/Horror Elements/Misc/Misc_breath"));
            ////Breath
            case soundLibEntry.SFX_EXTRA_BREATH:
                return new Sound(Entry, audioTrack.SFX_EXTRA_HUMANS, 1f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/Industrial/Horror Elements/Misc/Misc_breath"));
            ////Whispers
            case soundLibEntry.SFX_EXTRA_WHISPERS:
                return new Sound(Entry, audioTrack.SFX_EXTRA_HUMANS, 1f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/Industrial/Horror Elements/Misc/Misc_Whisper"));

            //Extra, Monsters
            ////None
            case soundLibEntry.SFX_EXTRA_MONSTERS_NONE:
                return new Sound(Entry, audioTrack.SFX_EXTRA_MONSTERS, 0f, 1f, false, Resources.Load<AudioClip>("Audio/Human/Human Agony Screams/Wav/Female/Female Scream 08"));
            ////Monsters 1
            case soundLibEntry.SFX_EXTRA_MONSTERS_1:
                return new Sound(Entry, audioTrack.SFX_EXTRA_MONSTERS, 1f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/Human/Human Agony Screams/Wav/Female/Female Scream 08"));

            //Default
            default:
                Debug.Log(Entry);
                errorManager.Instance.createErrorReport("soundLib", "getEntry", errorType.switchCase);
                return new Sound(Entry, audioTrack.MUSIC, 1f * masterVolume, 1f, false, Resources.Load<AudioClip>("Audio/GameMusicPack_SUITE/wav/Horror/horror_02_loop"));
        }
    }
}
