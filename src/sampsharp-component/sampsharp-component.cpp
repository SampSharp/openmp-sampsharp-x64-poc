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
}

void SampSharpComponent::provideConfiguration(ILogger& logger, IEarlyConfig& config, bool defaults)
{
    #define initConfigString(key, value) \
        if(defaults) { \
            config.setString(key, value); } \
        else if (config.getType(key) == ConfigOptionType_None) { \
            config.setString(key, value); \
        }
	
	initConfigString("sampsharp.folder", "gamemode");
	initConfigString("sampsharp.assembly", "GameMode");
	initConfigString("sampsharp.entry_point_type", "SampSharp.OpenMp.Core.Interop");
	initConfigString("sampsharp.entry_point_method", "OnInit");
}

std::wstring widen(std::string const &in)
{
    std::wstring out{};

    if (in.length() > 0)
    {
        const int len = MultiByteToWideChar(CP_UTF8, MB_ERR_INVALID_CHARS,
                                      in.c_str(), in.size(), nullptr, 0);
        if ( len == 0 )
        {
            throw std::runtime_error("Invalid character sequence.");
        }

        out.resize(len);
        MultiByteToWideChar(CP_UTF8, MB_ERR_INVALID_CHARS,
                            in.c_str(), in.size(), out.data(), out.size());
    }

    return out;
}

void SampSharpComponent::onInit(IComponentList* components)
{
	const IConfig& config = core_->getConfig();

	auto folder = config.getString("sampsharp.folder");
	auto assembly = config.getString("sampsharp.assembly");
	auto entry_point_type = config.getString("sampsharp.entry_point_type");
	auto entry_point_method = config.getString("sampsharp.entry_point_method");

	auto full_entry_point = entry_point_type.to_string() + ", " + assembly.to_string(); // namespace.class, assembly

	// TODO: this is windows-only
	auto folder_w = widen(folder.to_string());
	auto assembly_w = widen(assembly.to_string());
	auto full_entry_point_w = widen(full_entry_point);
	auto entry_point_method_w = widen(entry_point_method.to_string());

    managed_host_.initialize();
    managed_host_.loadFor(folder_w.c_str(), assembly_w.c_str());
	managed_host_.getEntryPoint(full_entry_point_w.c_str(), entry_point_method_w.c_str(), (void**)&on_init_);

	on_init_(core_, components);
}

void SampSharpComponent::onReady()
{
}

void SampSharpComponent::free()
{
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
