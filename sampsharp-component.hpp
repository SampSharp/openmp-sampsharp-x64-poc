#pragma once

#include <sdk.hpp>

#include "interface.hpp"
#include "managed-host.hpp"

#include <Server/Components/Pawn/pawn.hpp>

using namespace Impl;

typedef void (CORECLR_DELEGATE_CALLTYPE *on_init_fn)(ICore *);

class SampSharpComponent final
	: public ISampSharpComponent
	, public PawnEventHandler
	, public CoreEventHandler
{
private:
	ICore* core_ = nullptr;
	IPawnComponent* pawn_;
	ManagedHost managed_host_;
	inline static SampSharpComponent* instance_ = nullptr;

	on_init_fn on_init_ = nullptr;
public:
	// Required component methods.
	StringView componentName() const override;

	SemanticVersion componentVersion() const override;

	void onLoad(ICore* c) override;

	void onInit(IComponentList* components) override;

	void onReady() override;

	void onFree(IComponent* component) override;

	void free() override;

	void reset() override;
	
	void onAmxLoad(IPawnScript& script) override;

	void onAmxUnload(IPawnScript& script) override;

	void onTick(Microseconds elapsed, TimePoint now) override;
	
	static SampSharpComponent* getInstance();

	~SampSharpComponent();
};
