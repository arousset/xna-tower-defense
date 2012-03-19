using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameStateManagement
{
    class CreditMenuScreen : MenuScreen
    {
        public CreditMenuScreen()
            : base("Credit")
        {
            MenuEntry back = new MenuEntry("Back");
            back.Selected += OnCancel;
            MenuEntry alban = new MenuEntry("Alban Rousset");
            alban.Selectable = false;
            MenuEntry clement = new MenuEntry("Clement Kawczak");
            clement.Selectable = false;

            MenuEntries.Add(alban);
            MenuEntries.Add(clement);
            MenuEntries.Add(back);
            SelectedEntry = 2;
        }
    }
}
