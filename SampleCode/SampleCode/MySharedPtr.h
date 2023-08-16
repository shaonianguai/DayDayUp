#pragma once
#include <memory>
#include <iostream>
#include <atomic>
using namespace std;

namespace MySpace {
    //-定义一个callable对象类作为默认删除器
    template<typename T>
    class deafult_deleter {
    public:
        void operator()(T* ptr) {
            delete ptr;
        }
    };

    template<typename T, typename deleter = deafult_deleter<T>>
    class shared_ptr {
    private:
        //-原生堆指针
        T* ptr = nullptr;
        //-指向use_count的指针，事实上use_count本是更复杂的结构体其中一个成员，所以用指针保留拓展性
        std::atomic<int>* use_count = nullptr;
    public:
        //-默认构造
        shared_ptr() = default;
        //-原生指针构造
        shared_ptr(T* ptr_);
        //-拷贝构造
        shared_ptr(const shared_ptr& lsh);
        //-移动构造
        shared_ptr(shared_ptr&& lsh);
        //-拷贝赋值运算符
        shared_ptr& operator=(const shared_ptr& lsh);
        //-移动赋值运算符
        shared_ptr& operator = (shared_ptr&& lsh);
        //-析构
        ~shared_ptr();

        //-箭头函数运算符重载
        T* operator->();
        //-解引用运算符重载
        T& operator*();

        //-交换
        void swap(shared_ptr& lsh);
        //-解除对当前指针的管理, 如果有新的ptr则托管新的指针
        void reset(T* ptr_ = nullptr);
        //-获取引用计数
        int count();
        //-获取原生指针
        T* get();
    };

    //-原生指针构造
    template<typename T, typename deleter>
    shared_ptr<T, deleter>::shared_ptr(T* ptr_)
        :ptr(ptr_), use_count(new std::atomic<int>(1)) {}

    //-拷贝构造
    template<typename T, typename deleter>
    shared_ptr<T, deleter>::shared_ptr(const shared_ptr& lsh) {
        ptr = lsh.ptr;
        use_count = lsh.use_count;
        ++* use_count;
    }

    //-移动构造
    template<typename T, typename deleter>
    shared_ptr<T, deleter>::shared_ptr(shared_ptr&& lsh) {
        std::swap(ptr, lsh.ptr);
        std::swap(use_count, lsh.use_count);
    }

    //-拷贝赋值运算符
    template<typename T, typename deleter>
    shared_ptr<T, deleter>& shared_ptr<T, deleter>::operator=(const shared_ptr& lsh) {
        shared_ptr<T, deleter>lsh_copy(lsh);
        swap(lsh_copy);
        return *this;
    }

    //-移动赋值运算符
    template<typename T, typename deleter>
    shared_ptr<T, deleter>& shared_ptr<T, deleter>::operator=(shared_ptr&& lsh) {
        if (this != &lsh) {//-另一种方式实现自赋值
            //-如果原来有托管对象
            if (ptr) {
                --* use_count;
                if (*use_count <= 0) {
                    deleter()(ptr);
                    delete use_count;
                }
                ptr = nullptr;
                use_count = nullptr;
            }
            swap(lsh);
        }
        return *this;
    }

    //-析构
    template<typename T, typename deleter>
    shared_ptr<T, deleter>::~shared_ptr() {
        if (ptr) {
            --* use_count;
            if (*use_count <= 0) {
                deleter()(ptr);
                delete use_count;
            }
        }
    }

    //-箭头函数运算符重载
    template<typename T, typename deleter>
    T* shared_ptr<T, deleter>::operator->() {
        return ptr;
    }

    //-解引用运算符重载
    template<typename T, typename deleter>
    T& shared_ptr<T, deleter>::operator*() {
        return *ptr;
    }

    //-交换
    template<typename T, typename deleter>
    void shared_ptr<T, deleter>::swap(shared_ptr& lsh) {
        std::swap(ptr, lsh.ptr);
        std::swap(use_count, lsh.use_count);
    }

    //-解除对当前指针的管理, 如果有新的ptr则托管新的指针
    template<typename T, typename deleter>
    void shared_ptr<T, deleter>::reset(T* ptr_) {
        if (ptr) {//-如果当前shared_ptr有托管对象，先释放原托管
            --* use_count;
            if (*use_count <= 0) {
                deleter()(ptr);
                delete use_count;
            }
            ptr = nullptr;
            use_count = nullptr;
        }
        //-是否有新托管
        if (ptr_) {
            ptr = ptr_;
            use_count = new std::atomic<int>(1);
        }
    }

    //-获取引用计数
    template<typename T, typename deleter>
    int shared_ptr<T, deleter>::count() {
        return *use_count;
    }

    //-获取原生指针
    template<typename T, typename deleter>
    T* shared_ptr<T, deleter>::get() {
        return ptr;
    }
}


void TestMySharedPtr();