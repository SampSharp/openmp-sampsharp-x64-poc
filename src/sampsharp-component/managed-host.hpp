#pragma once

#include <hostfxr.h>
#include <coreclr_delegates.h>
#include <string>
#include "compat.hpp"

class ManagedHost final
{
private:
    bool _isReady = false;

    string_t assem_path_;
    hostfxr_initialize_for_runtime_config_fn init_for_config_fptr = nullptr;
    hostfxr_get_runtime_delegate_fn get_delegate_fptr = nullptr;
    hostfxr_close_fn close_fptr = nullptr;
    load_assembly_and_get_function_pointer_fn load_assembly_and_get_function_pointer_fptr = nullptr;

    static void * load_library(const char_t *);
    static void * get_export(void *, const char *);

    bool load_hostfxr(const char_t * assembly_path);
    load_assembly_and_get_function_pointer_fn load_runtime(const char_t * config_path) const;

public:
    bool isReady() const;
    bool initialize();
    bool loadFor(const char_t * root_path, const char_t * assembly_name);
    bool getEntryPoint(const char_t * entry_type_name, const char_t * name, void ** delegate_ptr) const;
};