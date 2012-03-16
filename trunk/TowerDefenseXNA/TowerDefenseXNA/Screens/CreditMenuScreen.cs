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
            MenuEntry clement = new MenuEntry("Clement Kawczak");

            MenuEntries.Add(alban);
            MenuEntries.Add(clement);
            MenuEntries.Add(back);
        }
    }
}
