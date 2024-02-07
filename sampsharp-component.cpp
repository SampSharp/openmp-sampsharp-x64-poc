
#include <Server/Components/Pawn/Impl/pawn_impl.hpp>

#include "sampsharp-component.hpp"

StringView SampSharpComponent::componentName() const
{
	return "SampSharp";
}

SemanticVersion SampSharpComponent::componentVersion() const
{
	return { 0, 0, 1, 0 };
}

void SampSharpComponent::onLoad(ICore* c)
{
	core_ = c;

	core_->getEventDispatcher().addEventHandler(this);

	core_->printLn("onLoad");
}

void SampSharpComponent::onInit(IComponentList* components)
{
	pawn_ = components->queryComponent<IPawnComponent>();
	
	core_->printLn("onInit");

	if (pawn_)
	{
		setAmxFunctions(pawn_->getAmxFunctions());
		pawn_->getEventDispatcher().addEventHandler(this);
	}
    
	core_->printLn("<demo-net>");
  
    managed_host_.initialize();
    managed_host_.loadFor(L"D:\\projects\\full-template\\managed\\SashManaged\\SashManaged\\bin\\Debug\\net8.0\\", L"SashManaged");
	managed_host_.test(lib_args { L"this is a test", 12345 });
	core_->printLn("</demo-net>");
}

void SampSharpComponent::onReady()
{
}

void SampSharpComponent::onFree(IComponent* component)
{
	if (component == pawn_)
	{
		pawn_ = nullptr;
		setAmxFunctions();
	}
}

void SampSharpComponent::free()
{
	delete this;
}

void SampSharpComponent::reset()
{
}

void SampSharpComponent::onAmxLoad(IPawnScript& script)
{
}

void SampSharpComponent::onAmxUnload(IPawnScript& script)
{
}

void SampSharpComponent::onTick(Microseconds elapsed, TimePoint now)
{
}

SampSharpComponent* SampSharpComponent::getInstance()
{
	if (instance_ == nullptr)
	{
		instance_ = new SampSharpComponent();
	}
	return instance_;
}

SampSharpComponent::~SampSharpComponent()
{
	if (pawn_)
	{
		pawn_->getEventDispatcher().removeEventHandler(this);
	}
	if (core_)
	{
		core_->getEventDispatcher().removeEventHandler(this);
	}
}
