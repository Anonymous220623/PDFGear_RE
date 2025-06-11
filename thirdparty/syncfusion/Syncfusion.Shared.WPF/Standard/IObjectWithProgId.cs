// Decompiled with JetBrains decompiler
// Type: Standard.IObjectWithProgId
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("71e806fb-8dee-46fc-bf8c-7748a8a1ae13")]
[ComImport]
internal interface IObjectWithProgId
{
  void SetProgID([MarshalAs(UnmanagedType.LPWStr)] string pszProgID);

  [return: MarshalAs(UnmanagedType.LPWStr)]
  string GetProgID();
}
