public class Bank
{
    public List<Client> Clients { get; set; }

    public Bank()
    {
        Clients = new List<Client>();
    }

    public void AddClient(Client client)
    {
        Clients.Add(client);
    }

    public void OpenAccount(Client client, Account account)
    {
        client.Accounts.Add(account);
    }

    public void CloseAccount(Client client, Account account)
    {
        client.Accounts.Remove(account);
    }

    public void Transfer(Client fromClient, Account fromAccount, Client toClient, Account toAccount, decimal amount)
    {
        fromAccount.Withdraw(amount);
        toAccount.Deposit(amount);
    }
}
