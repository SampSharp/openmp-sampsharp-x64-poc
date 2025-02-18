#include <sdk.hpp>

struct wrap {
    IPool<IIDProvider>::Iterator it;
    wrap(IPool<IIDProvider>::Iterator it) : it(it) {}
};

extern "C" SDK_EXPORT wrap __CDECL IPool_begin(IPool<IIDProvider>* pool)
{
    return wrap(pool->begin());
}

extern "C" SDK_EXPORT wrap __CDECL IPool_end(IPool<IIDProvider>* pool)
{
    return wrap(pool->end());
}

extern "C" SDK_EXPORT void __CDECL IPool_inc(wrap& wrap)
{
    ++wrap.it;
}

void test123(IPool<IIDProvider>* pool)
{
    IPlayer p
    for(auto it = pool->begin(); it != pool->end(); ++it)
    {
        // Do something with the iterator
    }
}