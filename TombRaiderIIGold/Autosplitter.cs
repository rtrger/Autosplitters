﻿using System;
using System.Diagnostics.CodeAnalysis;
using LiveSplit.Model;
using LiveSplit.UI.Components.AutoSplit;

namespace TR2Gold
{
    /// <summary>
    ///     The game's level and demo values.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal enum Level : uint
    {
        // Levels
        LarasHome = 00,
        TheColdWar = 01,
        FoolsGold = 02,
        FurnaceOfTheGods = 03,
        Kingdom = 04,
        // Bonus
        NightmareInVegas = 05
    }

    /// <summary>
    ///     Implementation of <see cref="IAutoSplitter"/> for an <see cref="AutoSplitComponent"/>'s use.
    /// </summary>
    internal class Autosplitter : IAutoSplitter, IDisposable
    {
        private Level _farthestLevelCompleted = Level.LarasHome;
        private bool _newGameSelected;

        internal readonly ComponentSettings Settings = new ComponentSettings();
        internal GameData GameData = new GameData();

        /// <summary>A constructor that primarily exists to handle events/delegations.</summary>
        public Autosplitter() => GameData.OnGameFound += Settings.SetGameVersion;

        /// <summary>
        ///     Determines the IGT.
        /// </summary>
        /// <param name="state"><see cref="LiveSplitState"/> passed by LiveSplit</param>
        /// <returns>IGT as a <see cref="TimeSpan"/> if available, otherwise <see langword="null"/></returns>
        public TimeSpan? GetGameTime(LiveSplitState state)
        {
            uint currentLevelTicks = GameData.LevelTime.Current;
            double currentLevelTime = GameData.LevelTimeAsDouble(currentLevelTicks);

            // IL
            if (!Settings.FullGame)
                return TimeSpan.FromSeconds(currentLevelTime);

            // FG
            Level currentLevel = GameData.Level.Current;
            double finishedLevelsTime = GameData.SumCompletedLevelTimes(currentLevel);
            return TimeSpan.FromSeconds(currentLevelTime + finishedLevelsTime);
        }

        /// <summary>
        ///     Determines if IGT should be paused when the game is quit or <see cref="GetGameTime"/> returns <see langword="null"/>
        /// </summary>
        /// <param name="state"><see cref="LiveSplitState"/> passed by LiveSplit</param>
        /// <returns><see langword="true"/> if IGT should be paused, <see langword="false"/> otherwise</returns>
        public bool IsGameTimePaused(LiveSplitState state) => true;

        /// <summary>
        ///     Determines if the timer should split.
        /// </summary>
        /// <param name="state"><see cref="LiveSplitState"/> passed by LiveSplit</param>
        /// <returns><see langword="true"/> if the timer should split, <see langword="false"/> otherwise</returns>
        public bool ShouldSplit(LiveSplitState state)
        {
            Level currentLevel = GameData.Level.Current;
            bool onCorrectLevel = _farthestLevelCompleted == currentLevel - 1;

            // Deathrun
            bool laraJustDied = GameData.Health.Old > 0 && GameData.Health.Current == 0;
            if (Settings.Deathrun && onCorrectLevel && laraJustDied)
                return true;
            
            // FG & IL/Section
            bool levelJustCompleted = !GameData.LevelComplete.Old && GameData.LevelComplete.Current;
            if (levelJustCompleted && onCorrectLevel)
            {
                _farthestLevelCompleted++;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Determines if the timer should reset.
        /// </summary>
        /// <param name="state"><see cref="LiveSplitState"/> passed by LiveSplit</param>
        /// <returns><see langword="true"/> if the timer should reset, <see langword="false"/> otherwise</returns>
        public bool ShouldReset(LiveSplitState state)
        {
            /* It is hypothetically reasonable to use _fullGameFarthestLevel to reset
             * if the player loads into a level ahead of their current level.
             * However, considering a case where a runner accidentally loads an incorrect
             * save after dying, it's clear that this should be avoided.
             */
            return GameData.PickedPassportFunction.Current == 2;
        }

        /// <summary>
        ///     Determines if the timer should start.
        /// </summary>
        /// <param name="state"><see cref="LiveSplitState"/> passed by LiveSplit</param>
        /// <returns><see langword="true"/> if the timer should start, <see langword="false"/> otherwise</returns>
        public bool ShouldStart(LiveSplitState state)
        {
            Level currentLevel = GameData.Level.Current;
            Level oldLevel = GameData.Level.Old;

            // Determine if a new game was started; it applies to FG runs and IL runs of the first level.
            uint oldPassportPage = GameData.PickedPassportFunction.Old;
            uint currentPassportPage = GameData.PickedPassportFunction.Current;
            if (oldPassportPage == 0 && currentPassportPage == 1)
                _newGameSelected = true;
            bool cameFromTitleScreenOrLarasHome = GameData.TitleScreen.Old && !GameData.TitleScreen.Current || oldLevel == Level.LarasHome;
            bool justStartedColdWar = currentLevel == Level.TheColdWar;
            bool newGameStarted = cameFromTitleScreenOrLarasHome && justStartedColdWar && _newGameSelected;
            if (newGameStarted)
                return true;

            // The remaining logic only applies to non-FG runs starting on a level besides the first.
            if (Settings.FullGame)
                return false;

            uint oldTime = GameData.LevelTime.Old;
            uint currentTime = GameData.LevelTime.Current;
            bool wentToNextLevel = oldLevel == currentLevel - 1;
            if (wentToNextLevel && oldTime > currentTime)
            {
                _farthestLevelCompleted = oldLevel;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Resets values for full game runs.
        /// </summary>
        public void ResetValues()
        {
            _farthestLevelCompleted = Level.LarasHome;
            _newGameSelected = false;
        }

        public void Dispose() 
        {
            GameData.OnGameFound -= Settings.SetGameVersion;
            GameData = null;
        }
    }
}
