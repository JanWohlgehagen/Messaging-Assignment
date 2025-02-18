using EasyNetQ;

namespace MessageClient.Drivers.EasyNetQ.MessagingStrategies;

public class SendReceiveStrategy : MessagingStrategies.MessagingStrategy
{
  private readonly string _queueName;
  public SendReceiveStrategy(string queueName)
  {
    _queueName = queueName;
  }
  public override void Send<TMessage>(TMessage message, IBus? bus)
  {
    if(bus == null)
      throw new InvalidOperationException("You must call Connect before Send");
    
    var source = new CancellationTokenSource(1000);
    bus.SendReceive.Send(_queueName, message, source.Token);
    source.Dispose();
  }

  public override IDisposable Listen<TMessage>(Action<TMessage> callback, IBus? bus)
  {
    if(bus == null)
      throw new InvalidOperationException("You must call Connect before Listen");
    return bus.SendReceive.Receive(_queueName, callback);
  }
}