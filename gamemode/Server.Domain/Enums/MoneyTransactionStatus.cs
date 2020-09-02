namespace Server.Domain.Enums
{
    public enum MoneyTransactionStatus
    {
        INIT,
        SENDER_INSUFFICIENT_FUNDS,
        RECEIVER_AT_MAXIMUM_FUNDS,
        CANCELED,
        FINISHED
    }
}