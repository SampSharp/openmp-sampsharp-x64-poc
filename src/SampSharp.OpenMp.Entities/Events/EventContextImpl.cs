namespace SampSharp.Entities;

internal class EventContextImpl : EventContext
{
    private object[]? _arguments;
    private IServiceProvider? _eventServices;
    private string? _name;

    public override string Name => _name!;

    public override object[] Arguments => _arguments!;

    public override IServiceProvider EventServices => _eventServices!;

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