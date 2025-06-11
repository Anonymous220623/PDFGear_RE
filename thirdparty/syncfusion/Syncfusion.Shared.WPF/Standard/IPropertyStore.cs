// Decompiled with JetBrains decompiler
// Type: Standard.IPropertyStore
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[Guid("886d8eeb-8cf2-4446-8d02-cdba1dbdcf99")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[ComImport]
internal interface IPropertyStore
{
  uint GetCount();

  PKEY GetAt(uint iProp);

  void GetValue([In] ref PKEY pkey, [In, Out] PROPVARIANT pv);

  void SetValue([In] ref PKEY pkey, PROPVARIANT pv);

  void Commit();
}
