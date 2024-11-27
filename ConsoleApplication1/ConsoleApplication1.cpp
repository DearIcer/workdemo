#include <algorithm>
#include <iostream>
#include <vector>

template<typename T>
void sortVector(std::vector<T>& v)
{
    std::sort(v.begin(), v.end());
}

int main(int argc, char* argv[])
{
    std::vector<int> v{ 5, 4, 3, 2, 1 };
    sortVector(v);
    for (auto i : v)
        std::cout << i << " ";
    return 0;
}
