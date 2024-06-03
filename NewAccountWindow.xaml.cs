using System;
using System.Windows;

namespace BankApp
{
    public partial class NewAccountWindow : Window
    {
        public event EventHandler<Account> AccountAdded;

        private Client client;

        public NewAccountWindow(Client client)
        {
            InitializeComponent();
            this.client = client;
        }

        private string GenerateAccountNumber()
        {
            Random random = new Random();
            return random.Next(100000000, 999999999).ToString();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            string accountNumber = GenerateAccountNumber();
            AccountNumberTextBox.Text = accountNumber;
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AccountNumberTextBox.Text))
            {
                MessageBox.Show("Сначала сгенерируйте номер счета.");
                return;
            }

            string accountNumber = AccountNumberTextBox.Text.Replace(" ", "");
            decimal initialBalance = 0;

            Account newAccount;
            if (DepositRadioButton.IsChecked == true)
            {
                newAccount = new DepositAccount(int.Parse(accountNumber), initialBalance);
            }
            else if (NonDepositRadioButton.IsChecked == true)
            {
                newAccount = new NonDepositAccount(int.Parse(accountNumber), initialBalance);
            }
            else
            {
                MessageBox.Show("Выберите тип счета.");
                return;
            }

            client.Accounts.Add(newAccount);
            ClientDataHandler.SaveClients(MainWindow.Clients);
            AccountAdded?.Invoke(this, newAccount);
            OperationLogWindow.AddOperationLog(new OperationLog(DateTime.Now, "Создан новый счет", client.Name, accountNumber, initialBalance));
            this.Close();
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
