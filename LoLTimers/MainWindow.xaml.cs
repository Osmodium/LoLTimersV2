using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace LoLTimers
{
    public partial class MainWindow : Window
    {
        public static event EventHandler Update;
        private DispatcherTimer m_UpdateFrequency;
        
        public MainWindow()
        {
            InitializeComponent();
            
            Closing += OnClosing;
            
            // Load settings
            timerControl0.TimerName = "Top";
            timerControl1.TimerName = "Jungler";
            timerControl2.TimerName = "Mid";
            timerControl3.TimerName = "Bot";
            timerControl4.TimerName = "Support";

            m_UpdateFrequency = new DispatcherTimer();
            m_UpdateFrequency.Interval = TimeSpan.FromMilliseconds(100);
            m_UpdateFrequency.Tick += UpdateFrequencyOnTick;
            m_UpdateFrequency.Start();
        }
        
        private void UpdateFrequencyOnTick(object? sender, EventArgs e)
        {
            Update?.Invoke(this, EventArgs.Empty);
        }
        
        private void OnClosing(object sender, CancelEventArgs e)
        {
            m_UpdateFrequency.Stop();
        }
    }
}