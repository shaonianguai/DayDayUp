#include <vector>

// 一个vector，含有大量(100万) int 类型(1~10之间)的数据，清除vector中的指定某个值
void removeNumberFromArray(std::vector<int>& v, int a)
{
    auto it = std::remove(v.begin(), v.end(), a);

    v.erase(it, v.end());
}