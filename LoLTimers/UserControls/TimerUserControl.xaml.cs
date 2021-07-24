using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Speech.Synthesis;
using System.Windows.Controls;
using LoLTimers.DataTypes;
using LoLTimers.Managers;

namespace LoLTimers.UserControls
{
    public partial class TimerUserControl : UserControl, IDisposable
    {
        public string TimerName { get; set; }
        public int Id { get; set; }

        private Spell m_LeftSpell;
        private Spell m_RightSpell;

        private DateTime m_LeftTimer;
        private DateTime m_RightTimer;

        private bool m_IsLeftRunning;
        private bool m_IsRightRunning;

        private bool m_LeftAlert;
        private bool m_RightAlert;

        private string m_DecimalFormat = "N1";

        private DoubleAnimation m_PulseAnimation = new(1, 1.8, new Duration(TimeSpan.FromMilliseconds(500)));

        private SpeechSynthesizer m_SpeechSynthesizer = new();

        public TimerUserControl()
        {
            InitializeComponent();
            SetLeftSpell(Settings.Instance.SpellList[5]);
            SetRightSpell(Settings.Instance.SpellList[0]);
            SpellManager.Instance.SpellChanged += InstanceOnSpellChanged;

            m_PulseAnimation.AutoReverse = true;
            m_PulseAnimation.RepeatBehavior = RepeatBehavior.Forever;


            var voices = m_SpeechSynthesizer.GetInstalledVoices();
            voices[1].Enabled = true;
            m_SpeechSynthesizer.SelectVoice(voices[1].VoiceInfo.Name);
            m_SpeechSynthesizer.Volume = 80;
            m_SpeechSynthesizer.Rate = 2;
        }

        //public void Update(object? sender, EventArgs e)
        public void Update()
        {
            if (m_IsLeftRunning)
            {
                var t = m_LeftTimer - DateTime.Now;
                if (t <= TimeSpan.FromSeconds(m_LeftSpell.AlertThreshold) && !m_LeftAlert)
                    Alert(SpellSlot.Left);

                if (t < TimeSpan.Zero)
                {
                    t = TimeSpan.FromSeconds(m_LeftSpell.Cooldown);
                    m_IsLeftRunning = false;
                    StopAlert(SpellSlot.Left);
                }
                txtLeftSummonerTimer.Text = t.TotalSeconds.ToString(m_DecimalFormat);
            }

            if (m_IsRightRunning)
            {
                var t = m_RightTimer - DateTime.Now;
                if (t <= TimeSpan.FromSeconds(m_RightSpell.AlertThreshold) && !m_RightAlert)
                    Alert(SpellSlot.Right);

                if (t < TimeSpan.Zero)
                {
                    t = TimeSpan.FromSeconds(m_RightSpell.Cooldown);
                    m_IsRightRunning = false;
                    StopAlert(SpellSlot.Right);
                }
                txtRightSummonerTimer.Text = t.TotalSeconds.ToString(m_DecimalFormat);
            }
        }

        private void Alert(SpellSlot slot)
        {
            switch (slot)
            {
                case SpellSlot.Left:
                    m_LeftAlert = true;
                    m_SpeechSynthesizer.SpeakAsync($"{lblTimerName.Content} {m_LeftSpell.Name} ready in {m_LeftSpell.AlertThreshold} seconds");
                    txtLeftSummonerTimer.Fill = new SolidColorBrush(Colors.Red);
                    rectScaleLeft.BeginAnimation(ScaleTransform.ScaleXProperty, m_PulseAnimation);
                    rectScaleLeft.BeginAnimation(ScaleTransform.ScaleYProperty, m_PulseAnimation);
                    break;
                case SpellSlot.Right:
                    m_RightAlert = true;
                    m_SpeechSynthesizer.SpeakAsync($"{lblTimerName.Content} {m_RightSpell.Name} ready in {m_RightSpell.AlertThreshold} seconds");
                    txtRightSummonerTimer.Fill = new SolidColorBrush(Colors.Red);
                    rectScaleRight.BeginAnimation(ScaleTransform.ScaleXProperty, m_PulseAnimation);
                    rectScaleRight.BeginAnimation(ScaleTransform.ScaleYProperty, m_PulseAnimation);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }

        private void StopAlert(SpellSlot slot)
        {
            switch (slot)
            {
                case SpellSlot.Left:
                    m_LeftAlert = false;
                    m_SpeechSynthesizer.SpeakAsync($"{lblTimerName.Content} {m_LeftSpell.Name} ready");
                    txtLeftSummonerTimer.Fill = new SolidColorBrush(Colors.White);
                    rectScaleLeft.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                    rectScaleLeft.BeginAnimation(ScaleTransform.ScaleYProperty, null);
                    rectScaleLeft.ScaleX = 1;
                    rectScaleLeft.ScaleY = 1;
                    break;
                case SpellSlot.Right:
                    m_RightAlert = false;
                    m_SpeechSynthesizer.SpeakAsync($"{lblTimerName.Content} {m_RightSpell.Name} ready");
                    txtRightSummonerTimer.Fill = new SolidColorBrush(Colors.White);
                    rectScaleRight.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                    rectScaleRight.BeginAnimation(ScaleTransform.ScaleYProperty, null);
                    rectScaleRight.ScaleX = 1;
                    rectScaleRight.ScaleY = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
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
            if (!m_RightSpell.Equals(Settings.Instance.SpellList[0]))
                StartTimer(SpellSlot.Right);
        }

        private void ImgLeftSummoner_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!m_LeftSpell.Equals(Settings.Instance.SpellList[0]))
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

        public void Dispose()
        {
            m_SpeechSynthesizer?.Dispose();
        }
    }
}