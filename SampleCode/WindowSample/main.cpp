#include <windows.h>
#include "MyWindow.h"

int WINAPI WinMain(_In_ HINSTANCE hInstance, _In_opt_ HINSTANCE hPrevInstance, _In_ LPSTR lpCmdLine, _In_ int nShowCmd)
{
    MyWindow::RegisterMyWindowClass(hInstance);
    MyWindow* pMyWnd = new MyWindow();
    pMyWnd->CreateTestWindow(hInstance);

    if (pMyWnd == 0)
        return 0;

    MSG msg;
    while (GetMessage(&msg, nullptr, 0, 0) > 0)
    {
        TranslateMessage(&msg);
        DispatchMessage(&msg);
    }

    delete pMyWnd;

    return 0;
}