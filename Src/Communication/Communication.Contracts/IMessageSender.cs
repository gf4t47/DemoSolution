namespace Communication;

using System.Threading.Tasks;
public enum ResponseType
{
    Ack,
    Nack
}

public record PublishResponse(ResponseType Type)
{
    public ResponseType Type { get; } = Type;
}

public interface IMessageSender<in T>
{
    Task<PublishResponse> Send(IMessage<T> message);
}
