using System;
using System.Windows;

namespace BankApp
{
    public partial class NewClientWindow : Window
    {
        public event EventHandler<Client> ClientAdded;

        public NewClientWindow()
        {
            InitializeComponent();
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            string clientName = ClientNameTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(clientName))
            {
                Client newClient = new Client(clientName);
                ClientAdded?.Invoke(this, newClient);
                this.Close();
            }
            else
            {
                MessageBox.Show("Введите имя клиента.");
            }
        }
    }
}
