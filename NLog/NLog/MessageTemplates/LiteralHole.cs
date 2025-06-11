// Decompiled with JetBrains decompiler
// Type: NLog.MessageTemplates.LiteralHole
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

#nullable disable
namespace NLog.MessageTemplates;

internal struct LiteralHole(Literal literal, Hole hole)
{
  public Literal Literal = literal;
  public Hole Hole = hole;

  public bool MaybePositionalTemplate
  {
    get
    {
      return this.Literal.Skip != 0 && this.Hole.Index != (short) -1 && this.Hole.CaptureType == CaptureType.Normal;
    }
  }
}
