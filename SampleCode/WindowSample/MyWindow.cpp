#include "MyWindow.h"

const wchar_t* MyWindow::m_pszClassName = L"MyWndClass";

void MyWindow::CreateTestWindow(HINSTANCE hInst)
{
    DWORD dwExStyle = 0;
    m_hWnd = CreateWindowExW(dwExStyle, m_pszClassName, L"Test", WS_VISIBLE | WS_OVERLAPPEDWINDOW, 300, 300, 800, 800, NULL, NULL, hInst, NULL);

    if (m_hWnd == NULL)
    {
        throw 0;
    }
}

bool MyWindow::RegisterMyWindowClass(HINSTANCE hInstance)
{
    WNDCLASSW wincl;
    if (!GetClassInfo(hInstance, m_pszClassName, &wincl))
    {
        wincl.style = 0;
        wincl.hInstance = hInstance;
        wincl.lpszClassName = m_pszClassName;
        wincl.lpfnWndProc = WindowProc;
        wincl.cbClsExtra = 0;
        wincl.cbWndExtra = 0;
        wincl.hIcon = NULL;
        wincl.hCursor = NULL;
        wincl.hbrBackground = (HBRUSH)(COLOR_BTNFACE + 1);
        wincl.lpszMenuName = NULL;

        if (RegisterClass(&wincl) == 0)
        {
            return false;
        }
    }

    return true;
}

LRESULT CALLBACK MyWindow::WindowProc(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
    return DefWindowProc(hwnd, uMsg, wParam, lParam);
}

MyWindow::~MyWindow()
{
    if (m_hWnd)
    {
        DestroyWindow(m_hWnd);
    }
}