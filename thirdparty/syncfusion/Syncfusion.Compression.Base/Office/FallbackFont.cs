// Decompiled with JetBrains decompiler
// Type: Syncfusion.Office.FallbackFont
// Assembly: Syncfusion.Compression.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: A9A7FF4E-A031-4867-8B1F-3311AA2A62FF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Compression.Base.dll

using System;

#nullable disable
namespace Syncfusion.Office;

public class FallbackFont
{
  private uint m_startUnicodeRange;
  private uint m_endUnicodeRange;
  private string m_fontNames;

  [CLSCompliant(false)]
  public FallbackFont(uint startUnicodeRange, uint endUnicodeRange, string fontNames)
  {
    this.m_startUnicodeRange = startUnicodeRange;
    this.m_endUnicodeRange = endUnicodeRange;
    this.m_fontNames = fontNames;
  }

  [CLSCompliant(false)]
  public uint StartUnicodeRange
  {
    get => this.m_startUnicodeRange;
    set => this.m_startUnicodeRange = value;
  }

  [CLSCompliant(false)]
  public uint EndUnicodeRange
  {
    get => this.m_endUnicodeRange;
    set => this.m_endUnicodeRange = value;
  }

  public string FontNames
  {
    get => this.m_fontNames;
    set => this.m_fontNames = value;
  }

  internal bool IsWithInRange(string inputText)
  {
    foreach (char ch in inputText)
    {
      if (ch != ' ' && (int) ch >= (int) (ushort) this.StartUnicodeRange && (int) ch <= (int) (ushort) this.EndUnicodeRange)
        return true;
    }
    return false;
  }
}
