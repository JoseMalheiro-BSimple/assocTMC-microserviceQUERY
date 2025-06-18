namespace Application.Publishers;
public interface IMessagePublisher
{
    Task PublishOrderSubmittedAsync(Guid Id);
}
