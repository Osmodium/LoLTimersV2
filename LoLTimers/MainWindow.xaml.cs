using System.Windows;

namespace LoLTimers
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Load settings
            timerControl0.TimerName = "Top";
            timerControl1.TimerName = "Jun";
            timerControl2.TimerName = "Mid";
            timerControl3.TimerName = "Bot";
            timerControl4.TimerName = "Sup";
        }
    }
}
