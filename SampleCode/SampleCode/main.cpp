﻿#include <Windows.h>
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


//【输入】I am @!#$software1234engineer1234

//【输出】engineer software am I

//#include <iostream>
//#include <string>
//
//std::string Slove(std::string source)
//{
//    std::string ret;
//    std::string tmp;
//
//    for (int i = 0; i < source.length(); i++)
//    {
//        if (!(source[i] >= 'a' && source[i] <= 'z' || source[i] >= 'A' && source[i] <= 'Z'))
//        {
//            source[i] = ' ';
//        }
//    }
//
//    reverse(source.begin(), source.end());
//
//    for (int i = 0; i < source.length(); i++)
//    {
//        if (ret.empty())
//        {
//            if (source[i] >= 'a' && source[i] <= 'z' || source[i] >= 'A' && source[i] <= 'Z')
//            {
//                tmp += source[i];
//            }
//            else
//            {
//                if (tmp.empty())
//                {
//                    continue;
//                }
//                else
//                {
//                    reverse(tmp.begin(), tmp.end());
//                    ret += tmp;
//                    tmp.clear();
//                }
//            }
//        }
//        else
//        {
//            if (source[i] >= 'a' && source[i] <= 'z' || source[i] >= 'A' && source[i] <= 'Z')
//            {
//                tmp += source[i];
//
//                if (i == source.length() - 1)
//                {
//                    reverse(tmp.begin(), tmp.end());
//                    ret += tmp;
//                    tmp.clear();
//                }
//            }
//            else
//            {
//                if (tmp.empty())
//                {
//                    if (ret[ret.length() - 1] == ' ')
//                    {
//                        continue;
//                    }
//                    else
//                    {
//                        ret += ' ';
//                    }
//                }
//                else
//                {
//                    reverse(tmp.begin(), tmp.end());
//                    ret += tmp;
//                    tmp.clear();
//
//                    ret += ' ';
//                }
//            }
//        }
//    }
//
//    if (ret[ret.length() - 1] == ' ')
//        ret.erase(ret.length() - 1);
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