using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Text;
using System.IO;

namespace GameStateManagement
{
    class SelectLevelMenuScreen : MenuScreen
    {
        public SelectLevelMenuScreen(ContentManager Content)
            : base("Select level")
        {
            List<MenuEntry> listEntry = new List<MenuEntry>();
            Texture2D unlockTexture = Content.Load<Texture2D>("Screen/Unlock");
            MenuEntry back = new MenuEntry("Back", unlockTexture);
            back.Selected += OnCancel;
            MenuEntry lvl1 = new MenuEntry("Level 1", unlockTexture);
            lvl1.Selected += Lvl1Selected;
            listEntry.Add(lvl1);
            MenuEntry lvl2 = new MenuEntry("Level 2", unlockTexture);
            lvl2.Selected += Lvl1Selected;
            listEntry.Add(lvl2);
            MenuEntry lvl3 = new MenuEntry("Level 3", unlockTexture);
            lvl3.Selected += Lvl1Selected;
            listEntry.Add(lvl3);
            MenuEntry lvl4 = new MenuEntry("Level 4", unlockTexture);
            lvl4.Selected += Lvl1Selected;
            listEntry.Add(lvl4);
            MenuEntry lvl5 = new MenuEntry("Level 5", unlockTexture);
            lvl5.Selected += Lvl1Selected;
            listEntry.Add(lvl5);

            MenuEntries.Add(lvl1);
            MenuEntries.Add(lvl2);
            MenuEntries.Add(lvl3);
            MenuEntries.Add(lvl4);
            MenuEntries.Add(lvl5);
            MenuEntries.Add(back);

            try
            {
                // Création d'une instance de StreamReader pour permettre la lecture de notre fichier 
                StreamReader monStreamReader = new StreamReader("Content/save.txt");
                string ligne = monStreamReader.ReadLine();
                int i = 0;
                // Lecture de toutes les lignes et affichage de chacune sur la page 
                while (ligne != null)
                {
                    if (ligne[0] == 'y')
                    {
                        listEntry.ElementAt(i).LvlUnlock = true;
                    }
                    else
                    {
                        listEntry.ElementAt(i).Selectable = false;
                    }
                    ligne = monStreamReader.ReadLine();
                    i++;
                }
                monStreamReader.Close();
            }
            catch (Exception ex)
            {
            } 
        }

        void Lvl1Selected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen());
        }
    }
}
