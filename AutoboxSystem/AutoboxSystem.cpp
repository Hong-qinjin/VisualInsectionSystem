// AutoboxSystem.cpp: WinMain 的实现


#include "pch.h"
#include "framework.h"
#include "resource.h"
#include "AutoboxSystem_i.h"


using namespace ATL;


class CAutoboxSystemModule : public ATL::CAtlExeModuleT< CAutoboxSystemModule >
{
public :
	DECLARE_LIBID(LIBID_AutoboxSystemLib)
	DECLARE_REGISTRY_APPID_RESOURCEID(IDR_AUTOBOXSYSTEM, "{0f741792-f5ac-466b-8f09-4cb659f9a639}")
};

CAutoboxSystemModule _AtlModule;



//
extern "C" int WINAPI _tWinMain(HINSTANCE /*hInstance*/, HINSTANCE /*hPrevInstance*/,
								LPTSTR /*lpCmdLine*/, int nShowCmd)
{
	return _AtlModule.WinMain(nShowCmd);
}

