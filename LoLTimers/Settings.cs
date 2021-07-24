using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using LoLTimers.DataTypes;

namespace LoLTimers
{
    public sealed class Settings
    {
        // Lazy singleton
        private static readonly Lazy<Settings> lazy = new(() => new Settings());
        public static Settings Instance => lazy.Value;

        private float m_DefaultAlertThreshold = 30;
        
        public Spell[] Spells;

        private Settings()
        {
            // Todo: change to read from a settings file

            InitializeSpells();
        }

        private void InitializeSpells()
        {
            Spells = new Spell[12];
            Spells[0] = new Spell
            {
                Name = "None",
                Cooldown = 0,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri("/Images/None64.png"))
            };
            Spells[1] = new Spell
            {
                Name = "Barrier",
                Cooldown = 10,
                AlertThreshold = 5,
                Image = new BitmapImage(GetImageUri("/Images/Barrier.png"))
            };
            Spells[2] = new Spell
            {
                Name = "Clarity",
                Cooldown = 240,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri("/Images/Clarity.png"))
            };
            Spells[3] = new Spell
            {
                Name = "Cleanse",
                Cooldown = 210,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri("/Images/Cleanse.png"))
            };
            Spells[4] = new Spell
            {
                Name = "Exhaust",
                Cooldown = 210,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri("/Images/Exhaust.png"))
            };
            Spells[5] = new Spell
            {
                Name = "Flash",
                Cooldown = 300,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri("/Images/Flash.png"))
            };
            Spells[6] = new Spell
            {
                Name = "Ghost",
                Cooldown = 210,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri("/Images/Ghost.png"))
            };
            Spells[7] = new Spell
            {
                Name = "Heal",
                Cooldown = 240,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri("/Images/Heal.png"))
            };
            Spells[8] = new Spell
            {
                Name = "Ignite",
                Cooldown = 180,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri("/Images/Ignite.png"))
            };
            Spells[9] = new Spell
            {
                Name = "Mark",
                Cooldown = 80,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri("/Images/Mark.png"))
            };
            Spells[10] = new Spell
            {
                Name = "Smite",
                Cooldown = 90,
                AlertThreshold = 15,
                Image = new BitmapImage(GetImageUri("/Images/Smite.png"))
            };
            Spells[11] = new Spell
            {
                Name = "Teleport",
                Cooldown = 210,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri("/Images/Teleport.png"))
            };
        }

        public Uri GetImageUri(string path, UriKind uriKind = UriKind.Relative)
        {
            return new(path, uriKind);
        }
    }
}