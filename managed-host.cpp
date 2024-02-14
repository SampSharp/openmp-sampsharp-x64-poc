#include "managed-host.hpp"

#if !defined WINDOWS
#include <dlfcn.h>
#include <limits.h>

#define MAX_PATH PATH_MAX
#endif

#include <cassert>

#include "dotnet/nethost.h"


bool ManagedHost::initialize(){
    if(_isReady) {
        return true;
    }

    // Load host resolver and load functions from hosting library
    if (!load_hostfxr(nullptr))
    {
        assert(false && "Failure: load_hostfxr()");
        return false;
    }

    _isReady = true;
    return true;
}

bool ManagedHost::isReady() const {
    return _isReady;
}

bool ManagedHost::loadFor(const char_t * root_path, const char_t * assembly_name) {

    const string_t config_path = root_path + string_t() + assembly_name + STR(".runtimeconfig.json");
    load_assembly_and_get_function_pointer = get_dotnet_load_assembly(config_path.c_str());
    assert(load_assembly_and_get_function_pointer != nullptr && "Failure: get_dotnet_load_assembly()");

    assem_path_ = root_path + string_t() + assembly_name + STR(".dll");
 
    return true;
}

bool ManagedHost::getEntryPoint(const char_t *dotnet_type, const char_t *name, void **delegate_ptr) const {
    
    const int rc = load_assembly_and_get_function_pointer(
        assem_path_.c_str(),
        dotnet_type,
        name,
        UNMANAGEDCALLERSONLY_METHOD,
        nullptr,
        delegate_ptr);

    return rc == 0;
}

void *ManagedHost::load_library(const char_t *path) {
#ifdef WINDOWS
    HMODULE h = ::LoadLibraryW(path);
    assert(h != nullptr);
    return (void*)h;
#else
    void *h = dlopen(path, RTLD_LAZY | RTLD_LOCAL);
    assert(h != nullptr);
    return h;
#endif
}

void *ManagedHost::get_export(void *h, const char *name) {
#ifdef WINDOWS
    void *f = ::GetProcAddress(static_cast<HMODULE>(h), name);  // NOLINT(clang-diagnostic-microsoft-cast)
    assert(f != nullptr);
    return f;
#else
    void *f = dlsym(h, name);
    assert(f != nullptr);
    return f;
#endif
}

bool ManagedHost::load_hostfxr(const char_t *assembly_path) {
    const get_hostfxr_parameters params { sizeof(get_hostfxr_parameters), assembly_path, nullptr };
    // Pre-allocate a large buffer for the path to hostfxr
    char_t buffer[MAX_PATH];
    size_t buffer_size = sizeof(buffer) / sizeof(char_t);
    const int rc = get_hostfxr_path(buffer, &buffer_size, &params);
    if (rc != 0)
        return false;

    // Load hostfxr and get desired exports
    void *lib = load_library(buffer);
    init_for_config_fptr = (hostfxr_initialize_for_runtime_config_fn)get_export(lib, "hostfxr_initialize_for_runtime_config");
    get_delegate_fptr = (hostfxr_get_runtime_delegate_fn)get_export(lib, "hostfxr_get_runtime_delegate");
    close_fptr = (hostfxr_close_fn)get_export(lib, "hostfxr_close");

    return (init_for_config_fptr && get_delegate_fptr && close_fptr);
}

load_assembly_and_get_function_pointer_fn ManagedHost::get_dotnet_load_assembly(const char_t *config_path) const {
    // Load .NET Core
    void *ptr = nullptr;
    hostfxr_handle cxt = nullptr;
    int rc = init_for_config_fptr(config_path, nullptr, &cxt);
    if (rc != 0 || cxt == nullptr)
    {
        close_fptr(cxt);
        return nullptr;
    }

    // Get the load assembly function pointer
    rc = get_delegate_fptr(
        cxt,
        hdt_load_assembly_and_get_function_pointer,
        &ptr);
    if (rc != 0 || ptr == nullptr)
    {
        // fail
    }

    close_fptr(cxt);
    return (load_assembly_and_get_function_pointer_fn)ptr;
}