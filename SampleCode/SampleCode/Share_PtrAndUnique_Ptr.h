﻿#pragma once
template<typename T>
class smart
{
private:
    T* m_ptr;
    int* m_count; //reference couting

public:
    //构造函数
    smart(T* ptr = nullptr) :m_ptr(ptr)
    {
        if (m_ptr)
            m_count = new int(1);
        else
            m_count = new int(0);
    }

    //拷贝构造
    smart(const smart& ptr)
    {
        if (this != &ptr)
        {
            m_ptr = ptr.m_ptr;
            m_count = ptr.m_count;

            *m_count++;
        }
    }

    //析构函数
    ~smart()
    {
        Release();
    }

    //重载operator=
    smart& operator=(const smart& ptr)
    {
        if (m_ptr == ptr.m_ptr)
        {
            return *this;
        }

        if (m_ptr)
            Release();

        m_ptr = ptr.m_ptr;
        m_count = ptr.m_count;
        *m_count++;
        return *this;
    }

    //operator*重载
    T& operator*()
    {
        if (m_ptr)
        {
            return *m_ptr;
        }
    }

    //operator->重载
    T* operator->()
    {
        if (m_ptr)
        {
            return m_ptr;
        }
    }

    void Release()
    {
        *m_count--;
        if (*m_count == 0)
        {
            delete m_ptr;
            delete m_count;
        }
    }
};

template<typename T>
class UniquePtr
{
public:
    UniquePtr(T* pResource = NULL)
        : m_pResource(pResource)
    {

    }

    ~UniquePtr()
    {
        del();
    }

public:
    void reset(T* pResource) // 先释放资源(如果持有), 再持有资源
    {
        del();
        m_pResource = pResource;
    }

    T* release() // 返回资源，资源的释放由调用方处理
    {
        T* pTemp = m_pResource;
        m_pResource = nullptr;
        return pTemp;
    }

    T* get() // 获取资源，调用方应该只使用不释放，否则会两次delete资源
    {
        return m_pResource;
    }

public:
    operator bool() const // 是否持有资源
    {
        return m_pResource != nullptr;
    }

    T& operator * ()
    {
        return *m_pResource;
    }

    T* operator -> ()
    {
        return m_pResource;
    }

private:
    void del()
    {
        if (nullptr == m_pResource) return;
        delete m_pResource;
        m_pResource = nullptr;
    }

private:
    UniquePtr(const UniquePtr&) = delete; // 禁用拷贝构造
    UniquePtr& operator = (const UniquePtr&) = delete; // 禁用拷贝赋值

private:
    T* m_pResource;
};