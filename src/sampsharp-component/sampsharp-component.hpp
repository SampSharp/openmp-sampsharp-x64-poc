#pragma once

#include <sdk.hpp>

#include "managed-host.hpp"

typedef void (CORECLR_DELEGATE_CALLTYPE *on_init_fn)(ICore *, IComponentList*);

struct ISampSharpComponent : IComponent
{
	PROVIDE_UID(0x0B61929D1E94A319);
};

class SampSharpComponent final
	: public ISampSharpComponent
{
private:
	ICore* core_ = nullptr;
	ManagedHost managed_host_ {};
	inline static SampSharpComponent* instance_ = nullptr;
	on_init_fn on_init_ = nullptr;

public:
	StringView componentName() const override;

	SemanticVersion componentVersion() const override;

	void onLoad(ICore* c) override;

	void provideConfiguration(ILogger& logger, IEarlyConfig& config, bool defaults) override;
	
	void onInit(IComponentList* components) override;

	void onReady() override;

	void free() override;

	void reset() override;
	
	static SampSharpComponent* getInstance();
};
