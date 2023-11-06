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



//  Singleton& instance_1 = Singleton::getInstance();
//class Singleton
//{
//public:
//    ~Singleton() {
//        std::cout << "destructor called!" << std::endl;
//    }
//
//    Singleton(const Singleton&) = delete;
//    Singleton& operator=(const Singleton&) = delete;
//
//    static Singleton& getInstance() {
//        static Singleton instance;
//        return instance;
//    }
//
//private:
//    Singleton() {
//        std::cout << "constructor called!" << std::endl;
//    }
//};

//class Singleton {
//public:
//    static Singleton& getInstance() {
//        static Singleton instance;
//        return instance;
//    }
//private:
//    Singleton() = default;
//    ~Singleton() = default;
//    Singleton(const Singleton&) = delete;
//    Singleton& operator=(const Singleton&) = delete;
//};