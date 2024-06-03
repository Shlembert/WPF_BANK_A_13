using System;

namespace BankApp
{
    public class OperationLog
    {
        public DateTime Time { get; set; }
        public string Event { get; set; }
        public string Participants { get; set; }
        public string Accounts { get; set; }
        public decimal Amount { get; set; }

        // Пустой конструктор для десериализации из JSON
        public OperationLog()
        {
        }

        public OperationLog(DateTime time, string eventDetails, string participants, string accounts, decimal amount)
        {
            Time = time;
            Event = eventDetails;
            Participants = participants;
            Accounts = accounts;
            Amount = amount;
        }

        public override string ToString()
        {
            return $"{Time}: {Event} - {Participants} - {Accounts} - {Amount}";
        }
    }
}
