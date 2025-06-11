// Decompiled with JetBrains decompiler
// Type: Standard.IShellLinkW
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

#nullable disable
namespace Standard;

[Guid("000214F9-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IShellLinkW
{
  void GetPath([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszFile, int cchMaxPath, [In, Out] WIN32_FIND_DATAW pfd, SLGP fFlags);

  void GetIDList(out IntPtr ppidl);

  void SetIDList(IntPtr pidl);

  void GetDescription([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszFile, int cchMaxName);

  void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

  void GetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszDir, int cchMaxPath);

  void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

  void GetArguments([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszArgs, int cchMaxPath);

  void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

  short GetHotKey();

  void SetHotKey(short wHotKey);

  uint GetShowCmd();

  void SetShowCmd(uint iShowCmd);

  void GetIconLocation([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

  void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

  void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);

  void Resolve(IntPtr hwnd, uint fFlags);

  void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
}
