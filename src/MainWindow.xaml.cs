using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using LoLTimers.DataTypes;
using LoLTimers.Managers;

namespace LoLTimers
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer m_UpdateFrequency;

        public MainWindow()
        {
            InitializeComponent();

            Closing += OnClosing;

            // Load settings
            TopTimerControl.TimerName = "Top";
            TopTimerControl.SetLeftSpell(Settings.Instance.SpellList[(int)Spells.Flash]);
            TopTimerControl.SetRightSpell(Settings.Instance.SpellList[(int)Spells.Teleport]);
            JunglerTimerControl.TimerName = "Jungler";
            JunglerTimerControl.SetLeftSpell(Settings.Instance.SpellList[(int)Spells.Flash]);
            JunglerTimerControl.SetRightSpell(Settings.Instance.SpellList[(int)Spells.Smite]);
            MidTimerControl.TimerName = "Mid";
            MidTimerControl.SetLeftSpell(Settings.Instance.SpellList[(int)Spells.Flash]);
            MidTimerControl.SetRightSpell(Settings.Instance.SpellList[(int)Spells.Ignite]);
            BotTimerControl.TimerName = "Bot";
            BotTimerControl.SetLeftSpell(Settings.Instance.SpellList[(int)Spells.Flash]);
            BotTimerControl.SetRightSpell(Settings.Instance.SpellList[(int)Spells.Heal]);
            SupportTimerControl.TimerName = "Support";
            SupportTimerControl.SetLeftSpell(Settings.Instance.SpellList[(int)Spells.Flash]);
            SupportTimerControl.SetRightSpell(Settings.Instance.SpellList[(int)Spells.Exhaust]);

            m_UpdateFrequency = new DispatcherTimer();
            m_UpdateFrequency.Interval = TimeSpan.FromMilliseconds(100);
            m_UpdateFrequency.Tick += UpdateFrequencyOnTick;
            m_UpdateFrequency.Start();
        }

        private void UpdateFrequencyOnTick(object? sender, EventArgs e)
        {
            TopTimerControl.Update();
            JunglerTimerControl.Update();
            MidTimerControl.Update();
            BotTimerControl.Update();
            SupportTimerControl.Update();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            m_UpdateFrequency.Stop();

            TopTimerControl.Dispose();
            JunglerTimerControl.Dispose();
            MidTimerControl.Dispose();
            BotTimerControl.Dispose();
            SupportTimerControl.Dispose();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.IsDown || e.Key != Key.RightAlt)
                return;

            WindowStyle = WindowStyle.ToolWindow;
            e.Handled = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.IsDown || e.Key == Key.RightAlt)
                return;

            WindowStyle = WindowStyle.None;
            e.Handled = true;
        }
    }
}