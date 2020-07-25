# Installing the Windows 7 SDK

Download the SDK from [here](https://download.microsoft.com/download/A/6/A/A6AC035D-DA3F-4F0C-ADA4-37C8E5D34E3D/winsdk_web.exe).

You'll run into installation issues if you have ever updated Windows 7 or use another version of it.

If you have .NET 4.0 installed, remove it.

If you have a newer version of 4.x installed, open your Registry Editor, go to:

* `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Client\Version`
* `HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\Version`
* `HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\NET Framework Setup\NDP\v4\Client\Version`
* `HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\NET Framework Setup\NDP\v4\Full\Version`

Save the current version string, and then change it to `4.0.30319`.

If you attempt to install the SDK, it should succeed. Change the version string back to what it was originally, and then you're setup!