// Decompiled with JetBrains decompiler
// Type: Standard.SHARDAPPIDINFOLINK
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Standard;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
internal class SHARDAPPIDINFOLINK
{
  private IntPtr psl;
  [MarshalAs(UnmanagedType.LPWStr)]
  private string pszAppID;
}
