#region File Description
//-----------------------------------------------------------------------------
// StepOptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace WindowsGSM1
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class StepOptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry breakItDownMenuEntry;
        MenuEntry slowMoMenuEntry;
        MenuEntry danceItMenuEntry;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public StepOptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            breakItDownMenuEntry = new MenuEntry(string.Empty);
            slowMoMenuEntry = new MenuEntry(string.Empty);
            danceItMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            breakItDownMenuEntry.Selected += BreakItDownMenuEntrySelected;
            slowMoMenuEntry.Selected += SlowMoMenuEntrySelected;
            danceItMenuEntry.Selected += DanceItMenuEntrySelected;
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(breakItDownMenuEntry);
            MenuEntries.Add(slowMoMenuEntry);
            MenuEntries.Add(danceItMenuEntry);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            breakItDownMenuEntry.Text = "BreakItDown";
            slowMoMenuEntry.Text = "SlowMo";
            danceItMenuEntry.Text = "DanceIt";
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>
        void BreakItDownMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Language menu entry is selected.
        /// </summary>
        void SlowMoMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Frobnicate menu entry is selected.
        /// </summary>
        void DanceItMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            SetMenuEntryText();
        }


        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void ElfMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            SetMenuEntryText();
        }


        #endregion
    }
}
