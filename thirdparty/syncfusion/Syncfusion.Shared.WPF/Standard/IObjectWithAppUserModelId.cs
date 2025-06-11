// Decompiled with JetBrains decompiler
// Type: Standard.IObjectWithAppUserModelId
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[Guid("36db0196-9665-46d1-9ba7-d3709eecf9ed")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IObjectWithAppUserModelId
{
  void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

  [return: MarshalAs(UnmanagedType.LPWStr)]
  string GetAppID();
}
