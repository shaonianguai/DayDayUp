#pragma once
#include <windows.h>

class MyWindow
{

public:
    void CreateTestWindow(HINSTANCE hInstance);
    static bool RegisterMyWindowClass(HINSTANCE hInstance);
    MyWindow() : m_hWnd(0) {}
    ~MyWindow();

private:
    static LRESULT CALLBACK WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam);
    static const wchar_t* m_pszClassName;
    HWND m_hWnd;

};