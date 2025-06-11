// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReferenceKind
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO;

public enum ReferenceKind
{
  NumberFullContext = -4, // 0xFFFFFFFC
  NumberNoContext = -3, // 0xFFFFFFFD
  NumberRelativeContext = -2, // 0xFFFFFFFE
  ContentText = -1, // 0xFFFFFFFF
  PageNumber = 7,
  AboveBelow = 15, // 0x0000000F
}
