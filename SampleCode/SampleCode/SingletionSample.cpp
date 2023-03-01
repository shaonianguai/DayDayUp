#include "SingletionSample.h"
#include <iostream>

SingletionSample::SingletionSample():
    m_index(0)
{

}

SingletionSample::~SingletionSample()
{

}

// SingletionSample::GetInstance()->OutIndex();

void SingletionSample::OutIndex()
{
    std::cout << m_index++;
}