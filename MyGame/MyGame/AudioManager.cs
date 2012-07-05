using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Helper;
using control;

namespace MyGame
{
    /// <summary>
    /// This class represent Audio Manager that play different music/sound effects at certain moments
    /// </summary>
    public class AudioManager : GameComponent,IEvent
    {
        protected List<Event> events;

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue trackCue;
        Cue levelCompleteCue;
        bool levelCompleteRunning = false;

        // Shot variables
        int musicDelay = 800;
        int musicCountdown = 0;

        private MyGame myGame;

        public AudioManager(MyGame game)
            :base(game)
        {
            myGame = game;
            events = new List<Event>();
            game.mediator.register(this, MyEvent.C_ATTACK_BULLET_END, MyEvent.M_BITE,
                MyEvent.G_NextLevel, MyEvent.G_GameOver,MyEvent.M_HIT);


            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");

            trackCue =  soundBank.GetCue("Cowboy");
            levelCompleteCue = soundBank.GetCue("LevelComplete");
            trackCue.Play();
            trackCue.Pause();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            musicCountdown -= gameTime.ElapsedGameTime.Milliseconds;
            if (musicCountdown <= 0)
            {
                if (keyboard.IsKeyDown(Keys.M))
                {
                    if (trackCue.IsPaused)
                        trackCue.Resume();
                    else
                        trackCue.Pause();
                    musicCountdown = musicDelay;
                }
                else
                    musicCountdown = 0;
                
            }

            foreach (Event ev in events)
            {
                switch (ev.EventId)
                {
                    case (int)MyEvent.C_ATTACK_BULLET_END:  soundBank.PlayCue("shot"); break;
                    case (int)MyEvent.G_NextLevel: levelCompleteCue.Play(); levelCompleteRunning = true; break;
                    case (int)MyEvent.M_HIT:                soundBank.PlayCue("monsterHit"); break;
                    case (int)MyEvent.M_BITE:               soundBank.PlayCue("Bite"); break;
                    case (int)MyEvent.G_GameOver:           soundBank.PlayCue("ScreamAndDie"); break;
                }
            }

            if (levelCompleteRunning && levelCompleteCue.IsStopped)
            {
                levelCompleteRunning = false;
                myGame.mediator.fireEvent(MyEvent.G_NextLevel_END_OF_MUSIC);
            }
            events.Clear();
            base.Update(gameTime);
        }

        public void addEvent(Event ev)
        {
            events.Add(ev);
        }
    }
}
