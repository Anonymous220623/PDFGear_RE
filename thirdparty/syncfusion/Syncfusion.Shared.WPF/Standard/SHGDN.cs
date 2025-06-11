// Decompiled with JetBrains decompiler
// Type: Standard.SHGDN
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;

#nullable disable
namespace Standard;

[Flags]
internal enum SHGDN
{
  SHGDN_NORMAL = 0,
  SHGDN_INFOLDER = 1,
  SHGDN_FOREDITING = 4096, // 0x00001000
  SHGDN_FORADDRESSBAR = 16384, // 0x00004000
  SHGDN_FORPARSING = 32768, // 0x00008000
}
