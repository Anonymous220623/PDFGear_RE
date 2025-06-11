// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.XmlReaders.TextSettings
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.XmlReaders;

internal class TextSettings
{
  public string FontName;
  public float? FontSize;
  public bool? Bold;
  public bool? Italic;
  public bool? Underline;
  public bool? Striked;
  public string Language;
  public Color? FontColor;
  public int Baseline;
  public bool? HasLatin;
  public bool? HasComplexScripts;
  public bool? HasEastAsianFont;
  public string ActualFontName;
  internal bool? ShowSizeProperties;
  internal Dictionary<string, Stream> PreservedElements = new Dictionary<string, Stream>();
  internal float KerningValue;
  internal int SpacingValue;
}
