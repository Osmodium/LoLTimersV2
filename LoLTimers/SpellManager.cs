using System;
using System.Windows;
using System.Windows.Controls;
using LoLTimers.DataTypes;

namespace LoLTimers
{
    public sealed class SpellManager
    {
        // Lazy singleton
        private static readonly Lazy<SpellManager> lazy = new(() => new SpellManager());
        public static SpellManager Instance => lazy.Value;

        public event EventHandler<SpellChangedEventArgs> SpellChanged;
        
        public ContextMenu SpellsContextMenu;

        private object m_Sender = null;

        private SpellManager()
        {
            InitializeContextMenu();
        }

        private void InitializeContextMenu()
        {
            SpellsContextMenu = new ContextMenu();
            foreach (Spell spell in Settings.Instance.Spells)
            {
                MenuItem spellMenuItem = new()
                {
                    Header = spell.Name,
                    Icon = new Image {Source = spell.Image},
                    Tag = spell
                };
                spellMenuItem.Click += SpellMenuItemOnClick;

                SpellsContextMenu.Items.Add(spellMenuItem);
            }

            SpellsContextMenu.Opened += SpellsContextMenuOnOpened;
        }

        private void SpellsContextMenuOnOpened(object sender, RoutedEventArgs e)
        {
            m_Sender = ((ContextMenu) sender)?.PlacementTarget;
            e.Handled = true;
        }

        private void SpellMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            Spell spell = menuItem.Tag as Spell;

            SpellChanged?.Invoke(this, new SpellChangedEventArgs(m_Sender, spell));
            e.Handled = true;
        }
    }
}