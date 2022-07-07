using System;
using System.Threading.Tasks;
using System.Windows;

namespace Raden_Booster
{
    /// <summary>
    /// Interaction logic for WinPopupMessage.xaml
    /// </summary>
    public partial class WinPopupMessage : Window
    {
        public WinPopupMessage()
        {
            InitializeComponent();
        }

        public async void Show(int mili, String text)
        {
            this.Show();
            textMessage.Content = text;
            await Task.Delay(mili); ;
            this.Close();
        }
    }
}
