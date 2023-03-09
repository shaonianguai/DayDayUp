#pragma once

class Util
{
public:
    static void* my_memcpy(void* dest, const void* src, size_t num);
    static void* my_memmove(void* dest, const void* src, size_t num);
    static char* my_strcpy(char* strDest, const char* strSrc);
    static int my_strcpy_s(char* strDest, size_t size, const char* strSrc);
    static void* my_memeset(void* dest, int val, size_t len);
    static int my_memcmp(const void* str1, const void* str2, size_t count);
    static char* my_strstr(const char* s1, const char* s2);
};