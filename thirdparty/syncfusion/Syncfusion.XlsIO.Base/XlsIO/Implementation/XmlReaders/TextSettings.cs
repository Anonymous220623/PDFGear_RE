// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.XmlReaders.TextSettings
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Drawing;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.XmlReaders;

public class TextSettings
{
  public string FontName;
  public string Underline;
  public float? FontSize;
  public bool? Bold;
  public bool? Italic;
  public bool? Striked;
  public string Language;
  public Color? FontColor;
  public int Baseline;
  public bool? HasLatin;
  public bool? HasComplexScripts;
  public bool? HasEastAsianFont;
  public string ActualFontName;
  internal bool? ShowSizeProperties;
  internal bool? ShowBoldProperties;
  internal float KerningValue;
  internal int SpacingValue;
  internal bool IsNormalizeHeights;
  internal TextCapsType CapitalizationType;
}
