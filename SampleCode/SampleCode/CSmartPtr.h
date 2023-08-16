#pragma once

//对资源进行引用计数的类
template<class T>
class CRefCount
{
public:
    CRefCount(T* ptr = nullptr) :m_ptr(ptr)
    {
        if (m_ptr != nullptr)
            m_refCount++;
    }

    void AddReff()
    {
        m_refCount++;//增加资源的引用计数
    }

    int DelRef()
    {
        return m_refCount--;//减少资源的引用计数
    }

private:
    T* m_ptr;
    int m_refCount;
};

template<class T>
class CSmartClass
{
public:
    CSmartClass(T* ptr = nullptr) :m_ptr(ptr)
    {
        m_RefCount = new CRefCount<T>(m_ptr);
    }
    //释放指针所指向的内存资源
    ~CSmartClass()
    {
        if (m_RefCount->DelRef() == 0)
        {
            delete m_ptr;
            m_ptr = nullptr;
        }
    }

    CSmartClass(const CSmartClass<T>& ptr)
    {
        m_ptr = ptr->m_ptr;
        m_RefCount = ptr->m_RefCount;
        if (m_ptr)
            m_RefCount->AddReff();
    }

    CSmartClass<T>& operator=(const CSmartClass<T>& ptr)
    {
        if (this->m_ptr == ptr->m_ptr)
            return *this;

        if (m_RefCount->DelRef() == 0)
            delete m_ptr;

        m_ptr = ptr->m_ptr;
        m_RefCount = ptr->m_RefCount;

        if (m_ptr)
            m_RefCount->AddReff();

        return *this;
    }

    T& operator*()
    {
        return *m_ptr;
    }

    T* operator->()
    {
        return m_ptr;
    }

private:
    T* m_ptr;//指向管理内存资源的指针
    CRefCount<T>* m_RefCount;
};
