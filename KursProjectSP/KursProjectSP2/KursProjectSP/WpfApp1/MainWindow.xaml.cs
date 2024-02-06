using System.Windows;
using WpfApp1.ServiceforKurs;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ServiceforKurs.IClient_serverCallback
    {
        bool isConnected = false;
        ServiceforKurs.Client_serverClient client;
        int id;
        public MainWindow()
        {
            InitializeComponent();
            proceedButton.Visibility = Visibility.Collapsed;
        }

        private void Window_loaded(object s, RoutedEventArgs e)
        {

        }
        public void MessageCallBack(string msg)
        {
            listData.Items.Add(msg);
        }

        void Connect_User()
        {

            if(!isConnected)
            {
                client = new Client_serverClient(new System.ServiceModel.InstanceContext(this));
                id = client.Connect("");
                isConnected = true;
                proceedButton.Visibility = Visibility.Visible; 
                launchButton.Content = "Dislaunch";
            }
        }

        void Disconnect_User()
        {
            if (isConnected)
            {
                client.Disconnect(id);
                isConnected = false;
                proceedButton.Visibility= Visibility.Collapsed;
                launchButton.Content = "Launch";
            }
        }

        private void Launch_Click(object sender, RoutedEventArgs e)
        {
            if (!isConnected)
            {
                Connect_User();
            }
            else
            {
                Disconnect_User();
            }
        }

        private void Proceed_Click(object sender, RoutedEventArgs e)
        {
            client.SendMessage("", id);
        }
                
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            client = new ServiceforKurs.Client_serverClient(new System.ServiceModel.InstanceContext(this));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Disconnect_User();
        }
    }
}
