using Confluent.Kafka;
using Streamiz.Kafka.Net.SerDes;
using System.Text;

namespace KafkaStreams.Processor.SerDes;

public class GuidSerDes : AbstractSerDes<Guid>
{
    public override Guid Deserialize(byte[] data, SerializationContext context)
    {
        var bytesAsString = Encoding.UTF8.GetString(data);
        return new Guid(bytesAsString);
    }

    public override byte[] Serialize(Guid data, SerializationContext context)
    {
        var guidAsString = data.ToString();
        return Encoding.UTF8.GetBytes(guidAsString);
    }
}