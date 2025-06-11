// Decompiled with JetBrains decompiler
// Type: CommomLib.Commom.IInitializeWithWindow
// Assembly: CommomLib, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 75852081-BE40-4A44-9C18-BB37F6C8F51B
// Assembly location: C:\Program Files\PDFgear\CommomLib.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace CommomLib.Commom;

[Guid("3E68D4BD-7135-4D10-8018-9FB6D9F33FA1")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
public interface IInitializeWithWindow
{
  void Initialize(IntPtr hwnd);
}
