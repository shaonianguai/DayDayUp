#pragma once

class Util
{
public:
    static void* my_memcpy(void* dest, const void* src, size_t num);
    static void* my_memmove(void* dest, const void* src, size_t num);
    static char* my_strcpy(char* strDest, const char* strSrc);
    static int my_strcpy_s(char* strDest, size_t size, const char* strSrc);
};