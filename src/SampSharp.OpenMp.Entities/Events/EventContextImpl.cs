namespace SampSharp.Entities;

internal class EventContextImpl : EventContext
{
    public EventContextImpl(string name, IServiceProvider eventServices)
    {
        _name = name;
        _eventServices = eventServices;
    }

    private object[]? _arguments;
    private IServiceProvider _eventServices;
    private string _name;

    public override string Name => _name;

    public override object[] Arguments => _arguments!;

    public override IServiceProvider EventServices => _eventServices;

    public void SetArguments(object[] arguments)
    {
        _arguments = arguments;
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public void SetEventServices(IServiceProvider eventServices)
    {
        _eventServices = eventServices;
    }
}