using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LoLTimers
{
    /// <summary>
    /// Interaction logic for TimerUserControl.xaml
    /// </summary>
    public partial class TimerUserControl : UserControl
    {
        public string TimerName { get; set; }
        public int Id { get; set; }

        public TimerUserControl()
        {
            InitializeComponent();
        }
        
        private void ImgRightSummoner_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Right");
        }

        private void ImgLeftSummoner_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Left");
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            lblTimerName.Content = string.IsNullOrEmpty(TimerName) ? $"Summoner {Id}" : TimerName;

            txtLeftSummonerTimer.ContextMenu = Settings.Instance.SpellChangeContextMenu;
            imgLeftSummoner.ContextMenu = Settings.Instance.SpellChangeContextMenu;
            txtRightSummonerTimer.ContextMenu = Settings.Instance.SpellChangeContextMenu;
            imgRightSummoner.ContextMenu = Settings.Instance.SpellChangeContextMenu;
        }
    }
}
