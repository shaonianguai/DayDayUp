#include "SmartPtr.h"

void TestMySmpartPtr()
{

    MySpace::smartPtr<int> x(new int(5));
    MySpace::smartPtr<float> y(new float(3.14));
    std::cout << "the x value is: " << (*x) << std::endl;
    std::cout << "the y value is: " << (*y) << std::endl;
}