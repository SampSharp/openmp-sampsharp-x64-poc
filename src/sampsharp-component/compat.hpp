
#pragma once

#include <coreclr_delegates.h>
#include <string>

#ifdef WIN32
#  define WINDOWS true
#endif

#ifdef WINDOWS
#  include <Windows.h>
#  define STR(s) L ## s
#  define widen(x) widen_impl(x)

std::wstring widen_impl(std::string const &in);

#else
#  define STR(s) s
#  define widen(x) x
#endif

using string_t = std::basic_string<char_t>;
