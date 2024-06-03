using System;
using System.Windows;

namespace BankApp
{
    public partial class NewAccountWindow : Window
    {
        private Client client;

        public NewAccountWindow(Client client)
        {
            InitializeComponent();
            this.client = client;
        }

        private string GenerateAccountNumber()
        {
            Random random = new Random();
            return random.Next(100000000, 999999999).ToString(); // Преобразование числа в строку
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

            if (DepositRadioButton.IsChecked == true)
            {
                client.Accounts.Add(new DepositAccount(int.Parse(accountNumber), initialBalance));
            }
            else if (NonDepositRadioButton.IsChecked == true)
            {
                client.Accounts.Add(new NonDepositAccount(int.Parse(accountNumber), initialBalance));
            }
            else
            {
                MessageBox.Show("Выберите тип счета.");
                return;
            }

            ClientDataHandler.SaveClients(MainWindow.Clients);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
