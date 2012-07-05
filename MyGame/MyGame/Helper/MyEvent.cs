using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    /// <summary>
    /// Enum that enforeces the uniqness of id for each event type.
    /// prefixes: 
    /// P_ player related events,
    /// M_ monster related events,
    /// C_ control related events.
    /// </summary>
    public enum MyEvent
    {
        /*
         * P_ player related events
         * M_ monster related events
         * C_ control related events
         */


        /// <summary>Event fired when player is running, to activate running animation.</summary>
        P_RUN = 0,

        /// <summary> Event fired when player is moving forward, in order to move the player forward.</summary>
        C_FORWARD,
        /// <summary> Event fired when player is moving backward, in order to move the player backward.</summary>
        C_BACKWARD,
        /// <summary> Event fired when player is moving left, in order to move the player left.</summary>
        C_LEFT,
        /// <summary> Event fired when player is moving right, in order to move the player right.</summary>
        C_RIGHT,
        /// <summary> Event fired when firing gesture (key) is active, to play the animation of shooting or when the animation is already running wait a delay before actual shooting to allow delay between shoots.</summary>
        C_ATTACK_BULLET_BEGIN,
        /// <summary> Event fired when the bullet is being shot, to play the sound of shooting and add bullet to the scene.</summary>
        C_ATTACK_BULLET_END,
        /// <summary> Event fired when the bullet is being shot, to play the sound of shooting and add bullet to the scene.</summary>
        C_Pointer,

        /// <summary> Event fired when the bullet hit the monster, to play the sound of monster being hit.</summary>
        M_HIT,
        /// <summary> Event fired when the monster dies to update the score.</summary>
        M_DIE,
        /// <summary> Event fired when monster attack to play bit animation and decrease the HP of player.</summary>
        M_BITE,

        /// <summary> Event fired when the game is paused, to pause the game and show the pause (start screen).</summary>
        G_PAUSE,
        /// <summary> Event fired when game is resumed to resume the game and remove the pause screen.</summary>
        G_RESUME,
        /// <summary> Event fired in order to display the startScreen.</summary>
        G_StartScreen,
        /// <summary> Event fired help is selected from the start screen to display the help screen</summary>
        G_HelpScreen,
        /// <summary> Event fired when start is selected form start screen (pause) screen, to initialize the game and display the level screen.</summary>
        G_StartGame,
        /// <summary> Event fired to remove the level screen to start the level (in-game).</summary>
        G_StartLevel,
        /// <summary> Event fired to play the music at the end of the level and to disable the pause screen.</summary>
        G_NextLevel,
        /// <summary> Event fired to initialize the new level and displays the level screen or displays the credit screen if the final level was beaten.</summary>
        G_NextLevel_END_OF_MUSIC,
        /// <summary> Event fired when the credit is selected from the start screen or at the end of the game, to display the credit screen.</summary>
        G_CreditScreen,
        /// <summary> Event fired when HP of player reaches zero to play game over sound.</summary>
        G_GameOver,
        /// <summary> Event fired when the exit is selected from the start screen to exit and close the game.</summary>
        G_Exit,
    }
}
