using System.Windows;
using System.Windows.Controls;

namespace Raden_Booster
{
    /// <summary>
    /// Interaction logic for Tweak.xaml
    /// </summary>
    public partial class Tweak : Page
    {
        public Tweak()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window w in App.Current.Windows)
                if (w.GetType() == typeof(Task_Manager))
                {
                    w.Activate();
                    return;
                }
            Task_Manager task_Manager = new Task_Manager();
            task_Manager.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (Window w in App.Current.Windows)
                if (w.GetType() == typeof(StartupManagement))
                {
                    w.Activate();
                    return;
                }
            StartupManagement startupManagement = new StartupManagement();
            startupManagement.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (Window w in App.Current.Windows)
                if (w.GetType() == typeof(Cleaner))
                {
                    w.Activate();
                    return;
                }
            Cleaner cleaner = new Cleaner();
            cleaner.Show();
        }
    }
}
