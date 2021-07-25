using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Speech.Synthesis;
using System.Windows.Controls;
using LoLTimers.DataTypes;
using LoLTimers.Managers;
using LoLTimers.Utilities;

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

        private string m_DecimalFormat = "N0";

        private DoubleAnimation m_PulseAnimation = new(1, 1.8, new Duration(TimeSpan.FromMilliseconds(500)));
        private DoubleAnimation m_SlightPulseAnimation = new(1, 1.01, new Duration(TimeSpan.FromSeconds(2)));

        private SpeechSynthesizer m_SpeechSynthesizer = new();

        public TimerUserControl()
        {
            InitializeComponent();
            SetLeftSpell(Settings.Instance.SpellList[0]);
            SetRightSpell(Settings.Instance.SpellList[0]);
            SpellManager.Instance.SpellChanged += InstanceOnSpellChanged;

            m_PulseAnimation.AutoReverse = true;
            m_PulseAnimation.RepeatBehavior = RepeatBehavior.Forever;
            m_SlightPulseAnimation.AutoReverse = true;
            m_SlightPulseAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // TODO make these adjustable settings
            //var voices = m_SpeechSynthesizer.GetInstalledVoices();
            //voices[1].Enabled = true;
            //m_SpeechSynthesizer.SelectVoice(voices[1].VoiceInfo.Name);
            m_SpeechSynthesizer.Volume = 80;
            m_SpeechSynthesizer.Rate = 0;
        }

        public void Update()
        {
            if (!m_IsLeftRunning && !m_IsRightRunning)
                return;

            if (m_IsLeftRunning)
            {
                UpdateSpellTimer(SpellSlot.Left, m_LeftTimer, m_LeftSpell, m_LeftAlert, txtLeftSummonerTimer);
            }

            if (m_IsRightRunning)
            {
                UpdateSpellTimer(SpellSlot.Right, m_RightTimer, m_RightSpell, m_RightAlert, txtRightSummonerTimer);
            }
        }

        private void UpdateSpellTimer(SpellSlot slot, DateTime timer, Spell spell, bool alert, OutlinedTextBlock textBlock)
        {
            var t = timer - DateTime.Now;
            var at = TimeSpan.FromSeconds(spell.AlertThreshold);
            var cd = TimeSpan.FromSeconds(spell.Cooldown);
            if (t <= at && !alert)
                Alert(slot);

            if (t <= TimeSpan.Zero)
            {
                t = cd;
                Reset(slot);
            }
            textBlock.Text = t.TotalSeconds.ToString(m_DecimalFormat);
            var rt = (t - at) / (cd - at);
            var red = Color.Multiply(Colors.Red, 1 - (float)rt);
            var white = Color.Multiply(Colors.White, (float)rt);
            textBlock.Fill = new SolidColorBrush(Color.Add(white, red));
        }

        private void Alert(SpellSlot slot)
        {
            switch (slot)
            {
                case SpellSlot.Left:
                    m_LeftAlert = true;
                    m_SpeechSynthesizer.SpeakAsync($"{lblTimerName.Content} {m_LeftSpell.Name} in {m_LeftSpell.AlertThreshold}");
                    txtLeftSummonerTimer.Fill = new SolidColorBrush(Colors.Red);
                    rectScaleLeft.BeginAnimation(ScaleTransform.ScaleXProperty, m_PulseAnimation);
                    rectScaleLeft.BeginAnimation(ScaleTransform.ScaleYProperty, m_PulseAnimation);
                    break;
                case SpellSlot.Right:
                    m_RightAlert = true;
                    m_SpeechSynthesizer.SpeakAsync($"{lblTimerName.Content} {m_RightSpell.Name} in {m_RightSpell.AlertThreshold}");
                    txtRightSummonerTimer.Fill = new SolidColorBrush(Colors.Red);
                    rectScaleRight.BeginAnimation(ScaleTransform.ScaleXProperty, m_PulseAnimation);
                    rectScaleRight.BeginAnimation(ScaleTransform.ScaleYProperty, m_PulseAnimation);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
        }

        private void Reset(SpellSlot slot)
        {
            switch (slot)
            {
                case SpellSlot.Left:
                    m_IsLeftRunning = false;
                    m_LeftAlert = false;
                    m_SpeechSynthesizer.SpeakAsync($"{lblTimerName.Content} {m_LeftSpell.Name} ready!");
                    imgLeftSummoner.Source = m_LeftSpell.Image;
                    txtLeftSummonerTimer.Fill = new SolidColorBrush(Colors.White);
                    rectScaleLeft.BeginAnimation(ScaleTransform.ScaleXProperty, null);
                    rectScaleLeft.BeginAnimation(ScaleTransform.ScaleYProperty, null);
                    rectScaleLeft.ScaleX = 1;
                    rectScaleLeft.ScaleY = 1;
                    break;
                case SpellSlot.Right:
                    m_RightAlert = false;
                    m_IsRightRunning = false;
                    m_SpeechSynthesizer.SpeakAsync($"{lblTimerName.Content} {m_RightSpell.Name} ready!");
                    imgRightSummoner.Source = m_RightSpell.Image;
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
            txtLeftSummonerTimer.Text = spell.Cooldown.ToString(m_DecimalFormat);
            imgLeftSummoner.Source = spell.Image;
        }

        public void SetRightSpell(Spell spell)
        {
            m_RightSpell = spell;
            txtRightSummonerTimer.Text = spell.Cooldown.ToString(m_DecimalFormat);
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
            switch (slot)
            {
                case SpellSlot.Left:
                    m_LeftTimer = DateTime.Now.AddSeconds(m_LeftSpell.Cooldown);
                    imgLeftSummoner.Source = ImageUtilities.ConvertImageToGrayScaleImage(m_LeftSpell.Image);
                    rectScaleLeft.BeginAnimation(ScaleTransform.ScaleXProperty, m_SlightPulseAnimation);
                    rectScaleLeft.BeginAnimation(ScaleTransform.ScaleYProperty, m_SlightPulseAnimation);
                    m_IsLeftRunning = true;
                    break;
                case SpellSlot.Right:
                    m_RightTimer = DateTime.Now.AddSeconds(m_RightSpell.Cooldown);
                    imgRightSummoner.Source = ImageUtilities.ConvertImageToGrayScaleImage(m_RightSpell.Image);
                    rectScaleRight.BeginAnimation(ScaleTransform.ScaleXProperty, m_SlightPulseAnimation);
                    rectScaleRight.BeginAnimation(ScaleTransform.ScaleYProperty, m_SlightPulseAnimation);
                    m_IsRightRunning = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(slot), slot, null);
            }
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