using System;
using System.Windows.Media.Imaging;
using LoLTimers.DataTypes;

namespace LoLTimers.Managers
{
    public sealed class Settings
    {
        // Lazy singleton
        private static readonly Lazy<Settings> lazy = new(() => new Settings());
        public static Settings Instance => lazy.Value;

        private float m_DefaultAlertThreshold = 30;
        
        public Spell[] SpellList;

        private Settings()
        {
            // Todo: change to read from a settings file

            InitializeSpells();
        }

        private void InitializeSpells()
        {
            SpellList = new Spell[12];
            SpellList[(int)Spells.None] = new Spell
            {
                Name = nameof(Spells.None),
                Cooldown = 0,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri("/Images/None64.png"))
            };
            SpellList[(int)Spells.Barrier] = new Spell
            {
                Name = nameof(Spells.Barrier),
                Cooldown = 10,
                AlertThreshold = 5,
                Image = new BitmapImage(GetImageUri($"/Images/{nameof(Spells.Barrier)}.png"))
            };
            SpellList[(int)Spells.Clarity] = new Spell
            {
                Name = nameof(Spells.Clarity),
                Cooldown = 240,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri($"/Images/{nameof(Spells.Clarity)}.png"))
            };
            SpellList[(int)Spells.Cleanse] = new Spell
            {
                Name = nameof(Spells.Cleanse),
                Cooldown = 210,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri($"/Images/{nameof(Spells.Cleanse)}.png"))
            };
            SpellList[(int)Spells.Exhaust] = new Spell
            {
                Name = nameof(Spells.Exhaust),
                Cooldown = 210,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri($"/Images/{nameof(Spells.Exhaust)}.png"))
            };
            SpellList[(int)Spells.Flash] = new Spell
            {
                Name = nameof(Spells.Flash),
                Cooldown = 300,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri($"/Images/{nameof(Spells.Flash)}.png"))
            };
            SpellList[(int)Spells.Ghost] = new Spell
            {
                Name = nameof(Spells.Ghost),
                Cooldown = 210,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri($"/Images/{nameof(Spells.Ghost)}.png"))
            };
            SpellList[(int)Spells.Heal] = new Spell
            {
                Name = nameof(Spells.Heal),
                Cooldown = 240,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri($"/Images/{nameof(Spells.Heal)}.png"))
            };
            SpellList[(int)Spells.Ignite] = new Spell
            {
                Name = nameof(Spells.Ignite),
                Cooldown = 180,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri($"/Images/{nameof(Spells.Ignite)}.png"))
            };
            SpellList[(int)Spells.Mark] = new Spell
            {
                Name = nameof(Spells.Mark),
                Cooldown = 80,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri($"/Images/{nameof(Spells.Mark)}.png"))
            };
            SpellList[(int)Spells.Smite] = new Spell
            {
                Name = nameof(Spells.Smite),
                Cooldown = 90,
                AlertThreshold = 15,
                Image = new BitmapImage(GetImageUri($"/Images/{nameof(Spells.Smite)}.png"))
            };
            SpellList[(int)Spells.Teleport] = new Spell
            {
                Name = nameof(Spells.Teleport),
                Cooldown = 210,
                AlertThreshold = m_DefaultAlertThreshold,
                Image = new BitmapImage(GetImageUri($"/Images/{nameof(Spells.Teleport)}.png"))
            };
        }

        public Uri GetImageUri(string path, UriKind uriKind = UriKind.Relative)
        {
            return new(path, uriKind);
        }
    }
}