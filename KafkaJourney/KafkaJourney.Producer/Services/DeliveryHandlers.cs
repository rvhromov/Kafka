namespace KafkaJourney.Producer.Services;

public static class DeliveryHandlers
{
    public static void HandleError(DeliveryReport<int, EventAction> deliveryReport)
    {
        if (deliveryReport.Error.Code != ErrorCode.NoError)
        {
            Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
        }
    }
}