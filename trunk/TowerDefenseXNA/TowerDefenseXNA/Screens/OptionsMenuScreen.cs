#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using System.IO;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry resetSave;


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            resetSave = new MenuEntry("Reset save");
            resetSave.Selected += ResetSave;
            MenuEntries.Add(resetSave);
            MenuEntry back = new MenuEntry("Back");
            back.Selected += OnCancel;
            MenuEntries.Add(back);
        }


        #endregion

        #region Handle Input

        void ResetSave(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to reset the save?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            try
            {
                // Instanciation du StreamWriter avec passage du nom du fichier 
                StreamWriter monStreamWriter = new StreamWriter("Content/save.txt");
                int i = 0;
                while(i < 5)
                {
                    if (i == 0)
                        monStreamWriter.WriteLine("yn");
                    else
                        monStreamWriter.WriteLine("nn");
                    i++;
                }
                // Fermeture du StreamWriter (Très important) 
                monStreamWriter.Close();
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
