using System.Windows;

namespace BankApp
{
    public partial class DepositWithdrawWindow : Window
    {
        public event EventHandler BalanceUpdated;

        private Account account;
        private bool isDeposit;
        private string clientName;

        public DepositWithdrawWindow(Account account, bool isDeposit, string clientName)
        {
            InitializeComponent();
            this.account = account;
            this.isDeposit = isDeposit;
            this.clientName = clientName;
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
                    OperationLogWindow.AddOperationLog(new OperationLog(DateTime.Now, "Пополнение счета", clientName, account.AccountNumber.ToString(), amount));
                }
                else
                {
                    account.Withdraw(amount);
                    OperationLogWindow.AddOperationLog(new OperationLog(DateTime.Now, "Списание со счета", clientName, account.AccountNumber.ToString(), -amount));
                }

                ClientDataHandler.SaveClients(MainWindow.Clients);
                BalanceUpdated?.Invoke(this, EventArgs.Empty);
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
