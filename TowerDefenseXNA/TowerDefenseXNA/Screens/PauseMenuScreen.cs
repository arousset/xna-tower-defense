#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class PauseMenuScreen : MenuScreen
    {
        ContentManager Content;
        #region Initialization
        MenuEntry audioGameMenuEntry;
        static string[] audio = { "On", "Off"};
        public static int currentAudio = 0;

        /// <summary>
        /// Constructor.
        /// </summary>
        public PauseMenuScreen(ContentManager Content)
            : base("Paused")
        {
            this.Content = Content;

            // Create our menu entries.
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            audioGameMenuEntry = new MenuEntry(string.Empty);
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");
            SetMenuEntryText();
            // Hook up menu event handlers.
            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            audioGameMenuEntry.Selected += AudioGameMenuEntry;

            // Add entries to the menu.
            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(audioGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }


        #endregion

        void SetMenuEntryText()
        {
            audioGameMenuEntry.Text = "Audio : " + audio[currentAudio];
        }

        #region Handle Input

        void AudioGameMenuEntry(object sender, PlayerIndexEventArgs e)
        {
            currentAudio = (currentAudio + 1) % audio.Length;

            SetMenuEntryText();
        }

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this level?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to quit" message box. This uses the loading screen to
        /// transition from the game back to the main menu screen.
        /// </summary>
        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen(Content));
        }


        #endregion
    }
}
