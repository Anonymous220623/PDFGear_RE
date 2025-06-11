// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.RevisionType
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;

#nullable disable
namespace Syncfusion.DocIO.DLS;

[Flags]
public enum RevisionType
{
  None = 1,
  Insertions = 2,
  Deletions = 4,
  Formatting = 8,
  StyleDefinitionChange = 16, // 0x00000010
  MoveFrom = 32, // 0x00000020
  MoveTo = 64, // 0x00000040
}
