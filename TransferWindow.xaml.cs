using System;
using System.Linq;
using System.Windows;

namespace BankApp
{
    public partial class TransferWindow : Window
    {
        private Client client;
        private Account sourceAccount;

        public TransferWindow(Client client, Account sourceAccount)
        {
            InitializeComponent();
            this.client = client;
            this.sourceAccount = sourceAccount;

            // Установка текста TextBlock для выбранного счета
            SourceAccountTextBlock.Text = $"Счет списания: {sourceAccount.AccountNumber}";
            SourceAccountTextBlock.Visibility = Visibility.Visible;

            // Заполнение комбо-бокса счетами клиента
            FillAccountsComboBox();
        }

        private void FillAccountsComboBox()
        {
            // Очистка комбо-бокса перед заполнением
            AccountsComboBox.Items.Clear();

            // Добавление номеров счетов клиента в комбо-бокс, исключая выбранный для списания
            foreach (var account in client.Accounts.Where(a => a.AccountNumber != sourceAccount.AccountNumber))
            {
                AccountsComboBox.Items.Add(account.AccountNumber);
            }
        }

        private void FillClientsComboBox()
        {
            // Очистка комбо-бокса перед заполнением
            ClientsComboBox.Items.Clear();

            // Добавление других клиентов в комбо-бокс
            foreach (var otherClient in MainWindow.Clients.Where(c => c != client))
            {
                ClientsComboBox.Items.Add(otherClient);
            }
        }

        private void TransferButton_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную положительную сумму перевода.");
                return;
            }

            if (SelfTransferRadioButton.IsChecked == true)
            {
                // Перевод на свой счет
                if (AccountsComboBox.SelectedItem != null)
                {
                    int selectedAccountNumber = (int)AccountsComboBox.SelectedItem;
                    Account selectedAccount = client.Accounts.FirstOrDefault(account => account.AccountNumber == selectedAccountNumber);
                    if (selectedAccount != null)
                    {
                        if (sourceAccount.Balance >= amount)
                        {
                            sourceAccount.Withdraw(amount);
                            selectedAccount.Deposit(amount);
                            MessageBox.Show($"Средства успешно переведены на счет {selectedAccountNumber}");
                            ClientDataHandler.SaveClients(MainWindow.Clients);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Недостаточно средств на счете для перевода.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Выберите счет для перевода.");
                    }
                }
            }
            else if (OtherClientTransferRadioButton.IsChecked == true)
            {
                // Перевод на счет другого клиента
                if (ClientsComboBox.SelectedItem is Client selectedClient)
                {
                    Account destinationAccount = selectedClient.Accounts.FirstOrDefault();

                    if (sourceAccount.Balance >= amount)
                    {
                        sourceAccount.Withdraw(amount);

                        if (destinationAccount == null)
                        {
                            // Создание нового счета, если у клиента нет счетов
                            destinationAccount = new DepositAccount(int.Parse(GenerateNewAccountNumber()), amount) { CreationDate = DateTime.Now };
                            selectedClient.Accounts.Add(destinationAccount);
                        }
                        else
                        {
                            // Пополнение первого счета, если счета уже существуют
                            destinationAccount.Deposit(amount);
                        }

                        MessageBox.Show($"Средства успешно переведены на счет клиента {selectedClient.Name}");
                        ClientDataHandler.SaveClients(MainWindow.Clients);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Недостаточно средств на счете для перевода.");
                    }
                }
                else
                {
                    MessageBox.Show("Выберите клиента для перевода.");
                }
            }
            else
            {
                MessageBox.Show("Выберите тип перевода.");
            }
        }

        private string GenerateNewAccountNumber()
        {
            Random rand = new Random();
            int accountNumber;
            do
            {
                accountNumber = rand.Next(100000000, 999999999);
            } while (MainWindow.Clients.SelectMany(c => c.Accounts).Any(a => a.AccountNumber == accountNumber));
            return accountNumber.ToString(); // Преобразование числа в строку
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SelfTransferRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // При выборе опции "На свой счет" скрываем комбо-бокс выбора клиента
            ClientsComboBox.Visibility = Visibility.Collapsed;
            AccountsComboBox.Visibility = Visibility.Visible;

            // Заполняем комбо-бокс счетами клиента
            FillAccountsComboBox();
        }

        private void OtherClientTransferRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // При выборе опции "На счет другого клиента" скрываем комбо-бокс выбора счета и отображаем комбо-бокс выбора клиента
            ClientsComboBox.Visibility = Visibility.Visible;
            AccountsComboBox.Visibility = Visibility.Collapsed;

            // Заполняем комбо-бокс другими клиентами
            FillClientsComboBox();
        }
    }
}
