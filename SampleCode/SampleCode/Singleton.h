#pragma once
#include <mutex>

template <class T>
class singleton
{
protected:
    singleton() {};
private:
    singleton(const singleton&) = delete;
    singleton& operator=(const singleton&) = delete;
    static T* m_instance;
    static std::mutex m_bmutex;
public:
    static T* GetInstance();
};

template <class T>
T* singleton<T>::GetInstance()
{
    if (m_instance == NULL)
    {
        std::lock_guard<std::mutex> guard(m_bmutex);
        if (m_instance == NULL)
        {
            T* ptmp = new T();
            m_instance = ptmp;
        }
    }
    return m_instance;
}

template <class T>
std::mutex singleton<T>::m_bmutex;

template <class T>
T* singleton<T>::m_instance = NULL;