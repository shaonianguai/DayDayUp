#pragma once
#include <iostream>

namespace MySpace {
    template<class T>//类模板
    class smartPtr
    {
    private:
        T* m_ptr;//指针对象
    public:
        smartPtr(T* ptr)//构造函数
            :m_ptr(ptr)
        {}

        ~smartPtr()//析构函数
        {
            if (m_ptr != nullptr)
            {
                delete m_ptr;
                m_ptr = nullptr;
            }
        }
        T& operator*()//重载*运算符
        {
            return *m_ptr;
        }
        T* operator->()//重载->运算符
        {
            return m_ptr;
        }
    };
}

void TestMySmpartPtr();