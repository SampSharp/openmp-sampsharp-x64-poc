#include "sampsharp-component.hpp"
#include "compat.hpp"

#define CFG_FOLDER "sampsharp.folder"
#define CFG_ASSEMBLY "sampsharp.assembly"
#define CFG_ENTRY_POINT_TYPE "sampsharp.entry_point_type"
#define CFG_ENTRY_POINT_METHOD "sampsharp.entry_point_method"

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
}

void SampSharpComponent::provideConfiguration(ILogger& logger, IEarlyConfig& config, const bool defaults)
{
    #define initConfigString(key, value) \
        if(defaults) { \
            config.setString(key, value); } \
        else if (config.getType(key) == ConfigOptionType_None) { \
            config.setString(key, value); \
        }
	
	initConfigString(CFG_FOLDER, "gamemode");
	initConfigString(CFG_ASSEMBLY, "GameMode");
	initConfigString(CFG_ENTRY_POINT_TYPE, "SampSharp.OpenMp.Core.Interop");
	initConfigString(CFG_ENTRY_POINT_METHOD, "OnInit");
}

void SampSharpComponent::onInit(IComponentList* components)
{
	const IConfig& config = core_->getConfig();

	const auto folder = config.getString(CFG_FOLDER);
	const auto assembly = config.getString(CFG_ASSEMBLY);
	const auto entry_point_type = config.getString(CFG_ENTRY_POINT_TYPE);
	const auto entry_point_method = config.getString(CFG_ENTRY_POINT_METHOD);

	const auto folder_w = widen(folder.to_string());
	const auto assembly_w = widen(assembly.to_string());
	const auto full_entry_point_w = widen(entry_point_type.to_string() + ", " + assembly.to_string()); // namespace.class, assembly
	const auto entry_point_method_w = widen(entry_point_method.to_string());

    if(!managed_host_.initialize())
	{
		core_->logLn(LogLevel::Error, "Failed to initialize the .NET host framework resolver. Has the .NET runtime been installed?");
		return;
	}

    if(!managed_host_.loadFor(folder_w.c_str(), assembly_w.c_str()))
	{
		core_->logLn(LogLevel::Error, "Failed to initialize the .NET runtime for '%s/%s'. Is the '*.dll.runtimeconfig.json' file available? Is the .NET runtime available?", folder.to_string().c_str(), assembly.to_string().c_str());
		return;
	}

	if(!managed_host_.getEntryPoint(full_entry_point_w.c_str(), entry_point_method_w.c_str(), reinterpret_cast<void**>(&on_init_)))
	{
		core_->logLn(LogLevel::Error, "The entrypoint '%s.%s, %s' could not be found.", entry_point_type.to_string().c_str(), entry_point_method.to_string().c_str(), assembly.to_string().c_str());
		return;
	}

	on_init_(core_, components);
}

void SampSharpComponent::onReady()
{
}

void SampSharpComponent::free()
{
	// TODO: hook for cleaning up event handlers
	delete this;
}

void SampSharpComponent::reset()
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
