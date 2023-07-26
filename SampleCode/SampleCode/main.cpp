#include <Windows.h>
#include <iostream>
#include "SingletionSample.h"
#include "Utils.h"
#include "BlockingCollection.h"

struct S
{
    char name[10];
    int age;
};

class aaa
{
public:
    aaa()
    {
        asd = 0;
        qwe = 0;
        int b = 1;
    }
    ~aaa()
    {
        int c = 1;
    }
private:
    int asd;
    int qwe;
};

static aaa wo;

int main()
{
    std::cout << "Hello World!\n";

    const char* strSrc = "hello";
    char strDest[16] = { 0 };
    Util::my_strcpy(strDest, strSrc);


    struct S arr1[2] = { {"aas",20}, {"qwe",18} };
    struct S arr2[3] = { 0 };
    Util::my_memcpy(arr2, arr1, sizeof(arr1));

    char a[] = "Firststring";
    const char b[] = "Secondstring";
    Util::my_memmove(a, b, 9);

    const char* source = "123456789";
    char dest[20];
    Util::my_strcpy_s(dest, sizeof(dest), source);




    return 0;
}