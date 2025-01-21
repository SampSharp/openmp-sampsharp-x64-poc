#pragma once

#include <sdk.hpp>

#include "managed-host.hpp"

struct SampSharpInfo
{
	SampSharpInfo(size_t size, int api_version, SemanticVersion version) : 
		size(size), 
		api_version(api_version),
		version(version) { }

	// sizeof(SampSharpInfo) for backwards compatibility
	size_t size; 
	// version of SampSharp component <> hosted API. Version mismatch will cause launch failure.
	int api_version; 
	// version of the SampSharp component
	SemanticVersion version; 
};

typedef void (CORECLR_DELEGATE_CALLTYPE *on_init_fn)(ICore *, IComponentList *, SampSharpInfo *);
typedef void (CORECLR_DELEGATE_CALLTYPE *on_cleanup_fn)();

struct ISampSharpComponent : IComponent
{
	PROVIDE_UID(0x0B61929D1E94A319);
};

class SampSharpComponent final
	: public ISampSharpComponent
{
private:
	ICore * core_ = nullptr;
	ManagedHost managed_host_ {};
	inline static SampSharpComponent * instance_ = nullptr;
	on_cleanup_fn on_cleanup_ = nullptr;

public:
	StringView componentName() const override;

	SemanticVersion componentVersion() const override;

	void onLoad(ICore * c) override;

	void provideConfiguration(ILogger & logger, IEarlyConfig & config, bool defaults) override;
	
	void onInit(IComponentList * components) override;

	void onReady() override;

	void free() override;

	void reset() override;
	
	static SampSharpComponent * getInstance();
};
