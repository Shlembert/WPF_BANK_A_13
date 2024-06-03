using System.Windows;

namespace BankApp
{
    public partial class DepositWithdrawWindow : Window
    {
        private Account account;
        private bool isDeposit;

        public DepositWithdrawWindow(Account account, bool isDeposit)
        {
            InitializeComponent();
            this.account = account;
            this.isDeposit = isDeposit;
        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AmountTextBox.Text))
            {
                MessageBox.Show("Введите сумму операции.");
                return;
            }

            if (!decimal.TryParse(AmountTextBox.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную положительную сумму.");
                return;
            }

            try
            {
                if (isDeposit)
                {
                    account.Deposit(amount);
                }
                else
                {
                    account.Withdraw(amount);
                }

                ClientDataHandler.SaveClients(MainWindow.Clients);
                this.Close();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
