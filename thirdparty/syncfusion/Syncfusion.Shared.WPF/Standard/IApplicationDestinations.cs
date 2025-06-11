// Decompiled with JetBrains decompiler
// Type: Standard.IApplicationDestinations
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("12337d35-94c6-48a0-bce7-6a9c69d4d600")]
[ComImport]
internal interface IApplicationDestinations
{
  void SetAppID([MarshalAs(UnmanagedType.LPWStr), In] string pszAppID);

  void RemoveDestination([MarshalAs(UnmanagedType.IUnknown)] object punk);

  void RemoveAllDestinations();
}
