using Godot;
using System;

public partial class AcknowledgeContext : IDisposable
{
	private IEventBus _eventBus;
	private bool _acknowledged;
	private string _receiver;

	public AcknowledgeContext(IEventBus messenger)
	{
		_eventBus = messenger;
	}

	public void Acknowledge()
	{
		_acknowledged = true;
		//_eventBus.EmitActionAcked();
	}

	public void Dispose() 
	{
		if (!_acknowledged)
			throw new Exception($"Action was not acknowledged by {_receiver}");
	}

	public void SetReceiver(string rName)
	{
		_receiver = rName;
	}
}
