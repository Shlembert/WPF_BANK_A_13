
public class AccountOperations : IAccountOperations<Account>
{
    public Account CreateAccount(int accountNumber, decimal initialBalance)
    {
        return new NonDepositAccount(accountNumber, initialBalance);
    }
}

public class DepositAccountOperations : IAccountOperations<DepositAccount>
{
    public DepositAccount CreateAccount(int accountNumber, decimal initialBalance)
    {
        return new DepositAccount(accountNumber, initialBalance);
    }
}

public class NonDepositAccountOperations : IAccountOperations<NonDepositAccount>
{
    public NonDepositAccount CreateAccount(int accountNumber, decimal initialBalance)
    {
        return new NonDepositAccount(accountNumber, initialBalance);
    }
}
