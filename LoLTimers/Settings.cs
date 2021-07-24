using System;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using LoLTimers.DataTypes;

namespace LoLTimers
{
    public sealed class Settings
    {
        // Lazy singleton
        private static readonly Lazy<Settings> lazy = new(() => new Settings());
        public static Settings Instance => lazy.Value;

        public ContextMenu SpellChangeContextMenu;

        public Spell[] Spells;

        private Settings()
        {
            // Todo: change to read from a settings file

            InitializeSpells();

            InitializeContextMenu();
        }

        private void InitializeContextMenu()
        {
            SpellChangeContextMenu = new ContextMenu();
            foreach (Spell spell in Spells)
            {
                MenuItem spellMenuItem = new()
                {
                    Header = spell.Name,
                    Icon = new Image { Source = spell.Image }
                };
                SpellChangeContextMenu.Items.Add(spellMenuItem);
            }

        }

        private void InitializeSpells()
        {
            Spells = new Spell[9];
            Spells[0] = new Spell
            {
                Name = "Barrier",
                Cooldown = 100,
                Image = new BitmapImage(GetImageUri("/Images/Barrier.png"))
            };
            Spells[1] = new Spell
            {
                Name = "Cleanse",
                Cooldown = 120,
                Image = new BitmapImage(GetImageUri("/Images/Cleanse.png"))
            };
            Spells[2] = new Spell
            {
                Name = "Exhaust",
                Cooldown = 220,
                Image = new BitmapImage(GetImageUri("/Images/Exhaust.png"))
            };
            Spells[3] = new Spell
            {
                Name = "Flash",
                Cooldown = 260,
                Image = new BitmapImage(GetImageUri("/Images/Flash.png"))
            };
            Spells[4] = new Spell
            {
                Name = "Ghost",
                Cooldown = 210,
                Image = new BitmapImage(GetImageUri("/Images/Ghost.png"))
            };
            Spells[5] = new Spell
            {
                Name = "Heal",
                Cooldown = 180,
                Image = new BitmapImage(GetImageUri("/Images/Heal.png"))
            };
            Spells[6] = new Spell
            {
                Name = "Ignite",
                Cooldown = 205,
                Image = new BitmapImage(GetImageUri("/Images/Ignite.png"))
            };
            Spells[7] = new Spell
            {
                Name = "Smite",
                Cooldown = 225,
                Image = new BitmapImage(GetImageUri("/Images/Smite.png"))
            };
            Spells[8] = new Spell
            {
                Name = "Teleport",
                Cooldown = 215,
                Image = new BitmapImage(new Uri("/Images/Teleport.png", UriKind.Relative))
            };
        }

        public Uri GetImageUri(string path, UriKind uriKind = UriKind.Relative)
        {
            return new(path, uriKind);
        }

        //public Uri GetImageUri(string path, UriKind uriKind = UriKind.RelativeOrAbsolute)
        //{
        //    return new(Path.Combine(GetApplicationFolder(), path), uriKind);
        //}

        //public string GetApplicationFolder()
        //{
        //    return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //}
    }
}