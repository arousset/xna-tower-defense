#region File Description
//-----------------------------------------------------------------------------
// PauseMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The pause menu comes up over the top of the game,
    /// giving the player options to resume or quit.
    /// </summary>
    class WinMenuScreen : MenuScreen
    {
        ContentManager Content;
        int levelNb;

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public WinMenuScreen(ContentManager Content, int levelNb)
            : base("You Win !")
        {
            this.levelNb = levelNb;

            // save
            char[] currentsave = new char[5];
            char[] currentsave2 = new char[5];
             try
            {
                // Création d'une instance de StreamReader pour permettre la lecture de notre fichier 
                StreamReader monStreamReader = new StreamReader("Content/save.txt");
                string ligne = monStreamReader.ReadLine();
                int i = 0;
                // Lecture de toutes les lignes et affichage de chacune sur la page 
                while (ligne != null)
                {

                    currentsave[i] = ligne[0];
                    currentsave2[i] = ligne[1];
         
                    ligne = monStreamReader.ReadLine();
                    i++;
                }
                monStreamReader.Close();
            }
            catch (Exception)
            {
            } 
            try
            {
                // Instanciation du StreamWriter avec passage du nom du fichier 
                StreamWriter monStreamWriter = new StreamWriter("Content/save.txt");
                int i = 0;
                while(i < 5)
                {
                    if (i == levelNb - 1)
                        monStreamWriter.WriteLine("yy");
                    else if (i == levelNb)
                        monStreamWriter.WriteLine("y" + (char)currentsave2[i]);
                    else
                        monStreamWriter.WriteLine("" + currentsave[i] + currentsave2[i]);
                    i++;
                }
                // Fermeture du StreamWriter (Très important) 
                monStreamWriter.Close();
            }
            catch (Exception)
            {
            }

            if (levelNb == 5)
            {
                this.menuTitle = "Gratz ! You have finished the game !";
            }
            else
            {
                MenuEntry nextLevelGameMenuEntry = new MenuEntry("Next level");
                nextLevelGameMenuEntry.Selected += NextLevelGameMenuEntry;
                MenuEntries.Add(nextLevelGameMenuEntry);
            }
            this.Content = Content;
            // Create our menu entries.
          //  MenuEntry selectLevelGameMenuEntry = new MenuEntry("Next level");
            MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            // Hook up menu event handlers.
         //   selectLevelGameMenuEntry.Selected += SelectLevelGameMenuEntry;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
           // MenuEntries.Add(selectLevelGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }


        #endregion

        #region Handle Input

        void NextLevelGameMenuEntry(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen(levelNb+1));
        }

        /// <summary>
        /// Event handler for when the Quit Game menu entry is selected.
        /// </summary>
        void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(),
                                                           new MainMenuScreen(Content));
        }


        #endregion
    }
}
