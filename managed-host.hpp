#pragma once

#include "dotnet/hostfxr.h"
#include "dotnet/coreclr_delegates.h"
#include <string>

using string_t = std::basic_string<char_t>;

#ifdef WIN32
#define WINDOWS true
#endif

#ifdef WINDOWS
#include <Windows.h>
#define STR(s) L ## s
#else
#define STR(s) s
#endif

class ManagedHost final
{
private:
    bool _isReady;

    string_t assem_path_;
    hostfxr_initialize_for_runtime_config_fn init_for_config_fptr;
    hostfxr_get_runtime_delegate_fn get_delegate_fptr;
    hostfxr_close_fn close_fptr;

    static void *load_library(const char_t *);
    static void *get_export(void *, const char *);

    bool load_hostfxr(const char_t *assembly_path);
    load_assembly_and_get_function_pointer_fn get_dotnet_load_assembly(const char_t *config_path) const;

    load_assembly_and_get_function_pointer_fn load_assembly_and_get_function_pointer;

public:
    bool isReady() const;
    bool initialize();
    bool loadFor(const char_t * root_path, const char_t * assembly_name);
    bool getEntryPoint(const char_t *entry_type_name, const char_t *name, void**delegate_ptr) const;
};