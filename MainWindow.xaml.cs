using System.Collections.Generic;
using System.Windows;

namespace BankApp
{
    public partial class MainWindow : Window
    {
        public static List<Client> Clients { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Clients = ClientDataHandler.LoadClients();
            LoadClientsIntoListBox();
        }

        private void LoadClientsIntoListBox()
        {
            ClientsListBox.Items.Clear();
            foreach (var client in Clients)
            {
                ClientsListBox.Items.Add(client.Name);
            }
        }

        private void NewClient_Click(object sender, RoutedEventArgs e)
        {
            NewClientWindow newClientWindow = new NewClientWindow();
            newClientWindow.ClientAdded += NewClientWindow_ClientAdded;
            newClientWindow.ShowDialog();
        }

        private void NewClientWindow_ClientAdded(object sender, Client newClient)
        {
            Clients.Add(newClient);
            ClientDataHandler.SaveClients(Clients);
            LoadClientsIntoListBox();
        }

        private void ClientsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ClientsListBox.SelectedItem != null)
            {
                string selectedClientName = ClientsListBox.SelectedItem.ToString();
                Client selectedClient = Clients.Find(c => c.Name == selectedClientName);
                if (selectedClient != null)
                {
                    ClientDetailsWindow clientDetailsWindow = new ClientDetailsWindow(selectedClient);
                    clientDetailsWindow.Show();
                }
            }
        }
    }
}
