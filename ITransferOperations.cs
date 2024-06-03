public interface ITransferOperations<in T> where T : Account
{
    void Transfer(T fromAccount, T toAccount, decimal amount);
}
