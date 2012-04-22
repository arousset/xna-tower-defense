using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;


namespace GameStateManagement
{
    class MultiPlayerMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry Ok;
        GameComponentCollection Components;
        Game game;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public MultiPlayerMenuScreen(ContentManager Content, GameComponentCollection Components, Game game)
            : base("Multiplayer")
        {
            MenuEntry back = new MenuEntry("Back");
            back.Selected += OnCancel;
            MenuEntry info = new MenuEntry("You need to have a local profil or a Xbox Live account");
            info.Selectable = false;
            MenuEntry info2 = new MenuEntry("to play with multiplayer version.");
            info2.Selectable = false;
            MenuEntry vide = new MenuEntry("");
            vide.Selectable = false;
            MenuEntry Ok = new MenuEntry("Ok");
            Ok.Selected += OkSelected;
            MenuEntry thx = new MenuEntry("Thank to XNA 4.0");
            thx.Selectable = false;
            this.Components = Components;
            this.game = game;
         
            MenuEntries.Add(info);
            MenuEntries.Add(info2);
            MenuEntries.Add(vide);
            MenuEntries.Add(Ok);
            MenuEntries.Add(back);
            SelectedEntry = 3;
        }
        #endregion

        void OkSelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreenMulti(1, Components, game));
        }

    }
}
