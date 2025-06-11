// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Parsing.FontNotFoundEventArgs
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;

#nullable disable
namespace Syncfusion.Pdf.Parsing;

public class FontNotFoundEventArgs : EventArgs
{
  private string m_orignalFont;
  private string m_alternateFont;

  public string UsedFont => this.m_orignalFont;

  public string AlternateFont
  {
    get => this.m_alternateFont;
    set => this.m_alternateFont = value;
  }

  internal FontNotFoundEventArgs(string usedFont) => this.m_orignalFont = usedFont;
}
