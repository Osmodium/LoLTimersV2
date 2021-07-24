using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LoLTimers.DataTypes;

namespace LoLTimers.UserControls
{
    /// <summary>
    /// Interaction logic for TimerUserControl.xaml
    /// </summary>
    public partial class TimerUserControl : UserControl
    {
        public string TimerName { get; set; }
        public int Id { get; set; }

        private Spell m_LeftSpell;
        private Spell m_RightSpell;

        private DateTime m_LeftTimer;
        private DateTime m_RightTimer;

        private bool m_IsLeftRunning;
        private bool m_IsRightRunning;
        
        public TimerUserControl()
        {
            InitializeComponent();
            SetLeftSpell(Settings.Instance.Spells[3]);
            SetRightSpell(Settings.Instance.Spells[0]);
            SpellManager.Instance.SpellChanged += InstanceOnSpellChanged;
            MainWindow.Update += Update;
        }

        private void Update(object? sender, EventArgs e)
        {
            if (m_IsLeftRunning)
            {
                var t = m_LeftTimer - DateTime.Now;
                if (t < TimeSpan.Zero)
                {
                    t = TimeSpan.Zero;
                    m_IsLeftRunning = false;
                }
                txtLeftSummonerTimer.Text = t.TotalSeconds.ToString();
            }
            
            if (m_IsRightRunning)
            {
                var t = m_RightTimer - DateTime.Now;
                if (t < TimeSpan.Zero)
                {
                    t = TimeSpan.Zero;
                    m_IsRightRunning = false;
                }
                txtRightSummonerTimer.Text = t.TotalSeconds.ToString();
            }
        }

        public void SetLeftSpell(Spell spell)
        {
            m_LeftSpell = spell;
            txtLeftSummonerTimer.Text = $"{spell.Cooldown}";
            imgLeftSummoner.Source = spell.Image;
        }

        public void SetRightSpell(Spell spell)
        {
            m_RightSpell = spell;
            txtRightSummonerTimer.Text = $"{spell.Cooldown}";
            imgRightSummoner.Source = spell.Image;
        }
        
        private void InstanceOnSpellChanged(object? sender, SpellChangedEventArgs e)
        {
            if (e == null)
                throw new Exception($"No {nameof(SpellChangedEventArgs)} was supplied!");

            if (e.Sender.Equals(txtLeftSummonerTimer) || e.Sender.Equals(imgLeftSummoner))
            {
                SetLeftSpell(e.Spell);
            }
            else if (e.Sender.Equals(txtRightSummonerTimer) || e.Sender.Equals(imgRightSummoner))
            {
                SetRightSpell(e.Spell);
            }
        }

        private void ImgRightSummoner_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!m_RightSpell.Equals(Settings.Instance.Spells[0]))
                StartTimer(SpellSlot.Right);
        }

        private void ImgLeftSummoner_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!m_LeftSpell.Equals(Settings.Instance.Spells[0]))
                StartTimer(SpellSlot.Left);
        }

        private void StartTimer(SpellSlot slot)
        {
            if (slot == SpellSlot.Left)
            {
                m_LeftTimer = DateTime.Now.AddSeconds(m_LeftSpell.Cooldown);
                m_IsLeftRunning = true;
                return;
            }

            m_RightTimer = DateTime.Now.AddSeconds(m_RightSpell.Cooldown);
            m_IsRightRunning = true;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            lblTimerName.Content = string.IsNullOrEmpty(TimerName) ? $"Summoner {Id}" : TimerName;

            txtLeftSummonerTimer.ContextMenu = SpellManager.Instance.SpellsContextMenu;
            imgLeftSummoner.ContextMenu = SpellManager.Instance.SpellsContextMenu;
            txtRightSummonerTimer.ContextMenu = SpellManager.Instance.SpellsContextMenu;
            imgRightSummoner.ContextMenu = SpellManager.Instance.SpellsContextMenu;
        }
    }
}