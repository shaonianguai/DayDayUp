#include <Windows.h>
#include <iostream>
#include "SingletionSample.h"
#include "Utils.h"
#include "BlockingCollection.h"
#include "MySharedPtr.h"
#include "SmartPtr.h"

struct S
{
    char name[10];
    int age;
};

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


    TestMySharedPtr();
    TestMySmpartPtr();

    return 0;
}


// ConsoleApplication1.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

//【输入】I am @!#$software1234engineer1234

//【输出】engineer software am I

//#include <iostream>
//#include <string>
//
//std::string Slove(std::string source)
//{
//    std::string ret;
//    std::string tmp;
//    std::string tmpSource;
//
//    for (int i = 0; i < source.length(); i++)
//    {
//        if (source[i] >= 'a' && source[i] <= 'z' || source[i] >= 'A' && source[i] <= 'Z')
//        {
//            tmpSource += source[i];
//        }
//        else
//        {
//            if (tmpSource.empty())
//            {
//                continue;
//            }
//            else
//            {
//                if (tmpSource[tmpSource.length() - 1] == ' ')
//                {
//                    continue;
//                }
//                else
//                {
//                    tmpSource += ' ';
//                }
//            }
//        }
//    }
//
//    if (tmpSource[tmpSource.length() - 1] == ' ')
//    {
//        tmpSource.erase(tmpSource.length() - 1);
//    }
//
//    reverse(tmpSource.begin(), tmpSource.end());
//
//    for (int i = 0; i < tmpSource.length(); i++)
//    {
//        if (tmpSource[i] >= 'a' && tmpSource[i] <= 'z' || tmpSource[i] >= 'A' && tmpSource[i] <= 'Z')
//        {
//            tmp += tmpSource[i];
//            if (i == tmpSource.length() - 1)
//            {
//                reverse(tmp.begin(), tmp.end());
//                ret += tmp;
//            }
//        }
//        else
//        {
//            reverse(tmp.begin(), tmp.end());
//            ret += tmp;
//            ret += ' ';
//            tmp.clear();
//        }
//    }
//
//    return ret;
//}
//
//int main()
//{
//    std::string input("   I am @!#$software1234engineer1234");
//    std::string ret = Slove(input);
//    std::cout << ret;
//}

