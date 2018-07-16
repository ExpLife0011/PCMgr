#include "stdafx.h"
#include "reghlp.h"
#include <string>

M_CAPI(HKEY) MREG_CLSIDToHKEY(HKEY hRootKey, LPWSTR clsid)
{
	HKEY hKEY;
	std::wstring path; path += L"SOFTWARE\\Classes\\CLSID\\"; path += clsid;
	RegOpenKeyEx(hRootKey, (LPWSTR)path.c_str(), 0, KEY_READ, &hKEY);
	return hKEY;
}
M_CAPI(HKEY) MREG_CLSIDToHKEYInprocServer32(HKEY hRootKey, LPWSTR clsid)
{
	HKEY hKEY;
	std::wstring path; path += L"SOFTWARE\\Classes\\CLSID\\"; path += clsid; path += L"\\InprocServer32";
	RegOpenKeyEx(hRootKey, (LPWSTR)path.c_str(), 0, KEY_READ, &hKEY);
	return hKEY;
}
M_CAPI(LPWSTR) MREG_ROOTKEYToStr(HKEY hRootKey)
{
	switch ((ULONG_PTR)hRootKey)
	{
	case HKEY_CLASSES_ROOT:return L"HKEY_CLASSES_ROOT";
	case HKEY_CURRENT_USER:return L"HKEY_CURRENT_USER";
	case HKEY_LOCAL_MACHINE:return L"HKEY_LOCAL_MACHINE";
	case HKEY_USERS:return L"HKEY_USERS";
	case HKEY_PERFORMANCE_DATA:return L"HKEY_PERFORMANCE_DATA";
	case HKEY_PERFORMANCE_TEXT:return L"HKEY_PERFORMANCE_TEXT";
	case HKEY_PERFORMANCE_NLSTEXT:return L"HKEY_PERFORMANCE_NLSTEXT";
	case HKEY_CURRENT_CONFIG:return L"";
	case HKEY_DYN_DATA:return L"HKEY_DYN_DATA";
	case HKEY_CURRENT_USER_LOCAL_SETTINGS:return L"HKEY_CURRENT_USER_LOCAL_SETTINGS";
	}
	return L"";
}
