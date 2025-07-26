using Godot;
using System;

public partial class AcknowledgeContext : IDisposable
{
	private IMessenger _messenger;
	private bool _acknowledged;
	private string _receiver;

	public AcknowledgeContext(IMessenger messenger)
	{
		_messenger = messenger;
	}

	public void Acknowledge()
	{
		_acknowledged = true;
		//_messenger.EmitActionAcked();
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
