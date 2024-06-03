public interface IAccountOperations<out T> where T : Account
{
    T CreateAccount(int accountNumber, decimal initialBalance);
}
