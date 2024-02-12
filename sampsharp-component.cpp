#include "sampsharp-component.hpp"

void Test(IPlayer *x)
{
	
    x->setCameraLookAt(Vector3(1,1,1), 1);
}
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
	core_->printLn("onInit");

	for (auto player : core_->getPlayers().players())
	{
	    player->allowTeleport(true);
	}
    
	core_->printLn("<demo-net>");
  
    managed_host_.initialize();
    managed_host_.loadFor(L"D:\\projects\\full-template\\managed\\SashManaged\\SashManaged\\bin\\Debug\\net8.0\\", L"SashManaged");
	managed_host_.getEntryPoint(STR("OnInit"), (void**)&on_init_);

	on_init_(core_);
	core_->printLn("</demo-net>");
}

void SampSharpComponent::onReady()
{
}

void SampSharpComponent::onFree(IComponent* component)
{
}

void SampSharpComponent::free()
{
	delete this;
}

void SampSharpComponent::reset()
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
	if (core_)
	{
		core_->getEventDispatcher().removeEventHandler(this);
	}
}
