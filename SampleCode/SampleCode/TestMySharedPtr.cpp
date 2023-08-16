#include "MySharedPtr.h"

//-测试类
class Student {
public:
    int id;
    int age;
    Student(int id_, int age_) :id(id_), age(age_) {}
};

void print(MySpace::shared_ptr<Student>& s) {
    if (s.get() == nullptr) {
        cout << "nullptr" << endl;
        return;
    }
    cout << "id:" << s->id << "  " << "age:" << s->age << endl;
    cout << "use count:" << s.count() << endl;
}


void TestMySharedPtr()
{
    {
        cout << L"拷贝构造测试，解引用，箭头函数测试" << endl;
        MySpace::shared_ptr<Student> s1(new Student(0, 18));
        MySpace::shared_ptr<Student> s2(s1);
        (*s1).age = 19;
        print(s2);
        cout << L"自赋值测试" << endl;
        s1 = s1;
        print(s1);
    }

    {
        cout << L"测试reset" << endl;
        MySpace::shared_ptr<Student> s1(new Student(0, 18));
        s1.reset();
        print(s1);
        s1.reset(new Student(1, 19));
        print(s1);

    }
    {
        cout << L"移动构造测试" << endl;
        MySpace::shared_ptr<Student> s1(MySpace::shared_ptr<Student>(new Student(0, 18)));
        print(s1);

        cout << L"拷贝赋值运算符测试" << endl;
        MySpace::shared_ptr<Student> s2;
        s2 = s1;
        print(s1);
        print(s2);

        cout << L"移动赋值运算符测试" << endl;
        MySpace::shared_ptr<Student> s3(new Student(2, 20));
        s1 = std::move(s3);
        print(s1);
        print(s2);
        print(s3);
    }
}