#include "sampsharp-component.hpp"

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

	const auto full_entry_point = StringView(entry_point_type.to_string() + ", " + assembly.to_string());

	const char * error = nullptr;
    if(!managed_host_.initialize(&error))
	{
		core_->logLn(LogLevel::Error, "Failed to initialize the .NET host framework resolver. Has the .NET runtime been installed?");
		core_->logLn(LogLevel::Error, "Error message: %s", error);
		return;
	}

    if(!managed_host_.loadFor(folder, assembly, &error))
	{
		core_->logLn(LogLevel::Error, "Failed to initialize the .NET runtime for '%s/%s'. Is the '*.runtimeconfig.json' file available? Is the .NET runtime available?", folder.to_string().c_str(), assembly.to_string().c_str());
		core_->logLn(LogLevel::Error, "Error message: %s", error);
		return;
	}

	if(!managed_host_.getEntryPoint(full_entry_point, entry_point_method, reinterpret_cast<void**>(&on_init_), &error))
	{
		core_->logLn(LogLevel::Error, "The entrypoint '%s.%s, %s' could not be found.", entry_point_type.to_string().c_str(), entry_point_method.to_string().c_str(), assembly.to_string().c_str());
		core_->logLn(LogLevel::Error, "Error message: %s", error);
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
