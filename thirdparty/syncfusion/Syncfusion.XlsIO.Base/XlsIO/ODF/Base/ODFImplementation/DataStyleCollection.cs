﻿// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.ODF.Base.ODFImplementation.DataStyleCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System.Collections.Generic;

#nullable disable
namespace Syncfusion.XlsIO.ODF.Base.ODFImplementation;

internal class DataStyleCollection
{
  internal List<string> FillImage;
  internal List<string> Gradient;
  internal List<string> Hatch;
  internal List<string> Marker;
  internal List<string> Opacity;
  internal List<string> StrokeDash;
  internal List<BooleanStyle> BooleanStyles = new List<BooleanStyle>();
  internal List<NumberStyle> NumberStyles = new List<NumberStyle>();
  internal List<PercentageStyle> PercentageStyles = new List<PercentageStyle>();
  internal List<CurrencyStyle> CurrencyStyles;
  internal List<DateStyle> DateStyles;
  internal List<TimeStyle> TimeStyles;
  internal List<TextStyle> ListStyles;
  internal List<string> DefaultSyles;
  internal List<string> DefaultPageLayout;
  internal List<string> Styles;
  internal List<string> LinearGradients;
  internal List<string> RadialGradients;
  internal List<string> TableTemplate;
  internal List<string> BibiliographyConfiguration;
  internal List<string> LineNumberingConfiguration;
  internal List<string> ListStyles1;
  internal List<string> Notes;
  internal List<string> OutlineStyles;

  internal void LoadStyles()
  {
  }
}
