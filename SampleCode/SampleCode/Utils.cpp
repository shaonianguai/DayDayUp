#include "Utils.h"
#include <cassert>
#include <string.h>

/*
memcpy() 用来复制内存，其原型为：
    void * memcpy ( void * dest, const void * src, size_t num );

memcpy() 会复制 src 所指的内存内容的前 num 个字节到 dest 所指的内存地址上。

memcpy() 并不关心被复制的数据类型，只是逐字节地进行复制，这给函数的使用带来了很大的灵活性，可以面向任何数据类型进行复制。

注意

dest 指针要分配足够的空间，也即大于等于 num 字节的空间。如果没有分配空间，会出现断错误。
dest 和 src 所指的内存空间不能重叠（如果发生了重叠，使用 memmove() 会更加安全）。

与 strcpy() 不同的是，memcpy() 会完整的复制 num 个字节，不会因为遇到“\0”而结束。

【返回值】返回指向 dest 的指针。注意返回的指针类型是 void，使用时一般要进行强制类型转换。
*/
void* Util::my_memcpy(void* dest, const void* src, size_t num)
{
    assert(dest);
    assert(src);

    void* ret = dest;

    char* tmpDest = (char*)dest;
    const char* tmpSrc = (char*)src;

    // 不支持 解决 源地址和目标地址重叠 问题
    while (num--)
    {
        *tmpDest++ = *tmpSrc++;
    }

    return ret;
}

/*
memcpy()函数从src内存中拷贝n个字节到dest内存区域，但是源和目的的内存区域不能重叠 在对待重叠区域的时候，
memmove()可以正确的完成对应的拷贝。
*/
// Copies "num" bytes from address "src" to address "dest"
void* Util::my_memmove(void* dest, const void* src, size_t num)
{
    assert(dest);
    assert(src);

    void* ret = dest;

    char* tmpDest = (char*)dest;
    const char* tmpSrc = (char*)src;

    if (tmpDest > tmpSrc && tmpDest < tmpSrc + num) // 如果 源地址和目标地址重叠，低字节向高字节拷贝
    {
        tmpDest = tmpDest + num - 1;
        tmpSrc = tmpSrc + num - 1;
        while (num--)
        {
            *tmpDest = *tmpSrc;
            tmpDest--;
            tmpSrc--;
        }
    }
    else
    {
        while (num--)
        {
            *tmpDest = *tmpSrc;
            tmpDest++;
            tmpSrc++;
        }
    }

    return ret;
}

char* Util::my_strcpy(char* strDest, const char* strSrc)
{
    assert(strDest);
    assert(strSrc);

    char* tmp = strDest;

    // 不支持 解决 源地址和目标地址重叠 问题
    while (*strSrc != '\0')
    {
        *strDest = *strSrc;
        strDest++;
        strSrc++;
    }

    return tmp; // 返回原首地址
}

inline void fill_string(char* str, size_t size, size_t offset)
{
    if (offset < size)
    {
        memset(str + offset, 0, (size - offset) * sizeof(char));
    }
}

int Util::my_strcpy_s(char* strDest, size_t size, const char* strSrc)
{
    assert(strDest);
    assert(strSrc);
    assert(size >= 0);

    char* tmpStrDest;
    tmpStrDest = strDest;
    size_t available = size;

    while (--available > 0)
    {
        *tmpStrDest = *strSrc;

        if (*strSrc == 0)
            break;

        tmpStrDest++;
        strSrc++;
    }

    if (available == 0)
    {
        *strDest = 0;
        fill_string(strDest, size, 1);
        return -1; // error range
    }

    fill_string(strDest, size, size - available + 1);

    return 0;
}