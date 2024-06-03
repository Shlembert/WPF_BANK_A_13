using System;

public abstract class Account
{
    public Client Client { get; set; }
    public int AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreationDate { get; set; }

    protected Account(int accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
        CreationDate = DateTime.Now;
    }

    public virtual void Transfer(Account toAccount, decimal amount)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            toAccount.Balance += amount;
        }
        else
        {
            throw new InvalidOperationException("Недостаточно средств для перевода.");
        }
    }

    public virtual void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            Balance += amount;
        }
        else
        {
            throw new InvalidOperationException("Сумма для пополнения должна быть положительной.");
        }
    }

    public virtual void Withdraw(decimal amount)
    {
        if (amount > 0 && Balance >= amount)
        {
            Balance -= amount;
        }
        else
        {
            throw new InvalidOperationException("Недостаточно средств для снятия или сумма должна быть положительной.");
        }
    }
}

public class DepositAccount : Account
{
    public DepositAccount(int accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance) { }
}

public class NonDepositAccount : Account
{
    public NonDepositAccount(int accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance) { }
}
