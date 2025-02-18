using EasyNetQ;

namespace MessageClient.Drivers.EasyNetQ.MessagingStrategies;

public class PubSubStrategy : MessagingStrategies.MessagingStrategy
{
  private readonly string? _clientId;
  public PubSubStrategy(string? clientId)
  {
    _clientId = clientId;
  }
  public override void Send<TMessage>(TMessage message, IBus? bus)
  {
    if(bus == null)
      throw new InvalidOperationException("You must call Connect before Send");
    
    var source = new CancellationTokenSource(1000);
    bus.PubSub.Publish(message, source.Token);
    source.Dispose();
  }

  public override IDisposable Listen<TMessage>(Action<TMessage> callback, IBus? bus)
  {
    if(bus == null)
      throw new InvalidOperationException("You must call Connect before Listen");
    
    return bus.PubSub.Subscribe(_clientId, callback);
  }
}