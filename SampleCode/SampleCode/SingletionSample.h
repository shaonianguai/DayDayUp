#pragma once
#include "Singleton.h"

class SingletionSample : public singleton<SingletionSample>
{
public:
    SingletionSample();
    ~SingletionSample();

    void OutIndex();
private:
    int m_index;
};