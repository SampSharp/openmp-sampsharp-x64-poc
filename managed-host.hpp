#pragma once

#include "dotnet/hostfxr.h"
#include "dotnet/coreclr_delegates.h"

    struct lib_args
    {
        const char_t *message;
        int number;
    };

    typedef void (CORECLR_DELEGATE_CALLTYPE *custom_entry_point_fn)(lib_args args);

class ManagedHost final
{
private:
    bool _isReady;

    hostfxr_initialize_for_runtime_config_fn init_for_config_fptr;
    hostfxr_get_runtime_delegate_fn get_delegate_fptr;
    hostfxr_close_fn close_fptr;

    static void *load_library(const char_t *);
    static void *get_export(void *, const char *);

    bool load_hostfxr(const char_t *assembly_path);
    load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t *config_path) const;

    custom_entry_point_fn entry_point;

public:
    bool isReady() const;
    bool initialize();
    bool loadFor(const char_t * root_path, const char_t * assembly_name);

    void test(lib_args args) const;
};