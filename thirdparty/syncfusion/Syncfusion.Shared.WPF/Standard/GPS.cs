// Decompiled with JetBrains decompiler
// Type: Standard.GPS
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

#nullable disable
namespace Standard;

internal enum GPS
{
  DEFAULT = 0,
  HANDLERPROPERTIESONLY = 1,
  READWRITE = 2,
  TEMPORARY = 4,
  FASTPROPERTIESONLY = 8,
  OPENSLOWITEM = 16, // 0x00000010
  DELAYCREATION = 32, // 0x00000020
  BESTEFFORT = 64, // 0x00000040
  NO_OPLOCK = 128, // 0x00000080
  MASK_VALID = 255, // 0x000000FF
}
