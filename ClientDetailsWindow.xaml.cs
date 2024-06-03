using System;
using System.Windows;

namespace BankApp
{
    public partial class ClientDetailsWindow : Window
    {
        private Client client;
        private string clientName;

        public ClientDetailsWindow(Client client)
        {
            InitializeComponent();
            this.client = client;
            this.clientName = client.Name;
            NameTextBlock.Text = client.Name;
            AccountsDataGrid.ItemsSource = client.Accounts;
        }

        private void NewAccountButton_Click(object sender, RoutedEventArgs e)
        {
            NewAccountWindow newAccountWindow = new NewAccountWindow(client);
            newAccountWindow.AccountAdded += NewAccountWindow_AccountAdded;
            newAccountWindow.ShowDialog();
        }

        private void NewAccountWindow_AccountAdded(object sender, Account newAccount)
        {
            AccountsDataGrid.Items.Refresh();
        }

        private void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountsDataGrid.SelectedItem is Account selectedAccount)
            {
                client.Accounts.Remove(selectedAccount);
                ClientDataHandler.SaveClients(MainWindow.Clients);
                AccountsDataGrid.Items.Refresh();

                MessageBox.Show($"Счет {selectedAccount.AccountNumber} удален.");
            }
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountsDataGrid.SelectedItem is Account selectedAccount && selectedAccount.Balance > 0)
            {
                TransferWindow transferWindow = new TransferWindow(client, selectedAccount);
                transferWindow.ShowDialog();
                AccountsDataGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Выберите счет с достаточным балансом для перевода.");
            }
        }

        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountsDataGrid.SelectedItem is Account selectedAccount)
            {
                try
                {
                    DepositWithdrawWindow depositWindow = new DepositWithdrawWindow(selectedAccount, true, clientName);
                    depositWindow.BalanceUpdated += DepositWindow_BalanceUpdated;
                    depositWindow.ShowDialog();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите счет для пополнения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            if (AccountsDataGrid.SelectedItem is Account selectedAccount)
            {
                try
                {
                    DepositWithdrawWindow withdrawWindow = new DepositWithdrawWindow(selectedAccount, false, clientName);
                    withdrawWindow.BalanceUpdated += WithdrawWindow_BalanceUpdated;
                    withdrawWindow.ShowDialog();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите счет для списания средств.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DepositWindow_BalanceUpdated(object sender, EventArgs e)
        {
            AccountsDataGrid.Items.Refresh();
        }

        private void WithdrawWindow_BalanceUpdated(object sender, EventArgs e)
        {
            AccountsDataGrid.Items.Refresh();
        }
    }
}
