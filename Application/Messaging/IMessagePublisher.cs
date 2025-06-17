namespace Application.Messaging;
public interface IMessagePublisher
{
    Task PublishOrderSubmittedAsync(Guid Id);
}
