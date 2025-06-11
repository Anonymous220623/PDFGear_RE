// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartColorModel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartColorModel
{
  private List<Brush> metroBrushes;
  private List<Brush> currentBrushes;
  private ChartColorPalette palette;
  private List<Brush> customBrushes;
  private List<Brush> autumnBrightsBrushes;
  private List<Brush> floraHuesBrushes;
  private List<Brush> pineappleBrushes;
  private List<Brush> tomatoSpectrumBrushes;
  private List<Brush> redChromeBrushes;
  private List<Brush> purpleChromeBrushes;
  private List<Brush> blueChromeBrushes;
  private List<Brush> greenChromeBrushes;
  private List<Brush> eliteBrushes;
  private List<Brush> sandyBeachBrushes;
  private List<Brush> lightCandyBrushes;

  public ChartColorModel()
  {
    this.CustomBrushes = new List<Brush>();
    this.ApplyPalette(ChartColorPalette.Metro);
  }

  public ChartColorModel(ChartColorPalette palette)
  {
    if (this.Palette == palette)
      this.ApplyPalette(palette);
    this.Palette = palette;
  }

  public List<Brush> CustomBrushes
  {
    get => this.customBrushes;
    set
    {
      this.customBrushes = value;
      if (this.Palette == ChartColorPalette.Custom)
        this.ApplyPalette(this.Palette);
      if (this.Series != null)
      {
        if (this.Palette != ChartColorPalette.Custom)
          return;
        if (this.Series.Segments != null)
          this.Series.Segments.Clear();
        if (this.Series.ActualArea == null)
          return;
        this.Series.ActualArea.IsUpdateLegend = true;
        this.Series.ActualArea.ScheduleUpdate();
      }
      else
      {
        if (this.ChartArea == null || this.ChartArea.VisibleSeries.Count <= 0)
          return;
        for (int index = 0; index < this.ChartArea.VisibleSeries.Count; ++index)
        {
          ChartSeriesBase chartSeriesBase = this.ChartArea.VisibleSeries[index];
          if (chartSeriesBase.Segments != null)
            chartSeriesBase.Segments.Clear();
        }
        this.ChartArea.IsUpdateLegend = true;
        this.ChartArea.ScheduleUpdate();
      }
    }
  }

  internal ChartColorPalette Palette
  {
    get => this.palette;
    set
    {
      if (value == this.palette)
        return;
      this.palette = value;
      this.ApplyPalette(this.palette);
    }
  }

  internal ChartBase ChartArea { get; set; }

  internal ChartSeriesBase Series { get; set; }

  public List<Brush> GetBrushes(ChartColorPalette palette)
  {
    switch (palette)
    {
      case ChartColorPalette.None:
        return new List<Brush>();
      case ChartColorPalette.Metro:
        return this.GetMetroBrushes();
      case ChartColorPalette.Custom:
        return this.CustomBrushes;
      case ChartColorPalette.AutumnBrights:
        return this.GetAutumnBrushes();
      case ChartColorPalette.FloraHues:
        return this.GetFloraHuesBrushes();
      case ChartColorPalette.Pineapple:
        return this.GetPineappleBrushes();
      case ChartColorPalette.TomotoSpectrum:
        return this.GetTomatoSpectrumBrushes();
      case ChartColorPalette.RedChrome:
        return this.GetRedChromeBrushes();
      case ChartColorPalette.PurpleChrome:
        return this.GetPurpleChromeBrushes();
      case ChartColorPalette.BlueChrome:
        return this.GetBlueChromeBrushes();
      case ChartColorPalette.GreenChrome:
        return this.GetGreenChromeBrushes();
      case ChartColorPalette.Elite:
        return this.GetEliteBrushes();
      case ChartColorPalette.SandyBeach:
        return this.GetSandyBeachBrushes();
      case ChartColorPalette.LightCandy:
        return this.GetLightCandyBrushes();
      default:
        return (List<Brush>) null;
    }
  }

  public List<Brush> GetMetroBrushes()
  {
    if (this.metroBrushes == null)
      this.metroBrushes = new List<Brush>()
      {
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 27, (byte) 161, (byte) 226)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 160 /*0xA0*/, (byte) 80 /*0x50*/, (byte) 0)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 51, (byte) 153, (byte) 51)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 162, (byte) 193, (byte) 57)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 216, (byte) 0, (byte) 115)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 240 /*0xF0*/, (byte) 150, (byte) 9)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 230, (byte) 113, (byte) 184)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 162, (byte) 0, byte.MaxValue)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 229, (byte) 20, (byte) 0)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 171, (byte) 169))
      };
    return this.metroBrushes;
  }

  public List<Brush> GetAutumnBrushes()
  {
    if (this.autumnBrightsBrushes == null)
      this.autumnBrightsBrushes = new List<Brush>()
      {
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 239, (byte) 242, (byte) 203)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 228, (byte) 99)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 254, (byte) 170, (byte) 15)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 219, (byte) 100, (byte) 15)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 53, (byte) 69, (byte) 43)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 196, (byte) 219, (byte) 171)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 150, (byte) 53, (byte) 10)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 254, (byte) 160 /*0xA0*/, (byte) 51)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 119, (byte) 158, (byte) 92)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 254, (byte) 237, (byte) 43))
      };
    return this.autumnBrightsBrushes;
  }

  public List<Brush> GetFloraHuesBrushes()
  {
    if (this.floraHuesBrushes == null)
      this.floraHuesBrushes = new List<Brush>()
      {
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 221, (byte) 218, (byte) 232)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 236, (byte) 121, (byte) 168)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 166, (byte) 64 /*0x40*/, (byte) 90)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 70, (byte) 78, (byte) 62)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 87, (byte) 76)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 215, (byte) 145)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 124, (byte) 10, (byte) 40)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 230, (byte) 1, (byte) 52)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 47, (byte) 42, (byte) 18)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 250, (byte) 157, (byte) 96 /*0x60*/))
      };
    return this.floraHuesBrushes;
  }

  public List<Brush> GetPineappleBrushes()
  {
    if (this.pineappleBrushes == null)
      this.pineappleBrushes = new List<Brush>()
      {
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 194, (byte) 223, (byte) 219)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 101, (byte) 114, (byte) 129)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 71, (byte) 57, (byte) 57)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 62, (byte) 86, (byte) 63 /*0x3F*/)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 129, (byte) 181, (byte) 100)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 219, (byte) 215, (byte) 167)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 159, (byte) 94, (byte) 68)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 41, (byte) 78, (byte) 17)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 114, (byte) 59, (byte) 14)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 110, (byte) 74, (byte) 66))
      };
    return this.pineappleBrushes;
  }

  [Obsolete("Use GetTomatoSpectrumBrushes")]
  public List<Brush> GetTomotoSpectramBrushes()
  {
    if (this.tomatoSpectrumBrushes == null)
      this.tomatoSpectrumBrushes = new List<Brush>()
      {
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 230, (byte) 220, (byte) 150)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 180, (byte) 181, (byte) 101)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 87, (byte) 107, (byte) 82)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 65, (byte) 102, (byte) 117)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 217, (byte) 109, (byte) 50)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 253, (byte) 169, (byte) 78)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 163, (byte) 159, (byte) 16 /*0x10*/)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 248, (byte) 148, (byte) 11)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 219, (byte) 213, (byte) 75)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 189, (byte) 73, (byte) 22))
      };
    return this.tomatoSpectrumBrushes;
  }

  public List<Brush> GetTomatoSpectrumBrushes()
  {
    if (this.tomatoSpectrumBrushes == null)
      this.tomatoSpectrumBrushes = new List<Brush>()
      {
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 230, (byte) 220, (byte) 150)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 180, (byte) 181, (byte) 101)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 87, (byte) 107, (byte) 82)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 65, (byte) 102, (byte) 117)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 217, (byte) 109, (byte) 50)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 253, (byte) 169, (byte) 78)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 163, (byte) 159, (byte) 16 /*0x10*/)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 248, (byte) 148, (byte) 11)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 219, (byte) 213, (byte) 75)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 189, (byte) 73, (byte) 22))
      };
    return this.tomatoSpectrumBrushes;
  }

  public List<Brush> GetRedChromeBrushes()
  {
    if (this.redChromeBrushes == null)
      this.redChromeBrushes = new List<Brush>()
      {
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 183, (byte) 28, (byte) 28)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 198, (byte) 40, (byte) 40)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 211, (byte) 47, (byte) 47)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 229, (byte) 57, (byte) 53)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 244, (byte) 67, (byte) 54)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 239, (byte) 83, (byte) 80 /*0x50*/)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 229, (byte) 115, (byte) 115)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 239, (byte) 154, (byte) 154)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 205, (byte) 210)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 222, (byte) 219, (byte) 222))
      };
    return this.redChromeBrushes;
  }

  public List<Brush> GetPurpleChromeBrushes()
  {
    if (this.purpleChromeBrushes == null)
      this.purpleChromeBrushes = new List<Brush>()
      {
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 74, (byte) 20, (byte) 140)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 106, (byte) 27, (byte) 154)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 123, (byte) 31 /*0x1F*/, (byte) 162)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 142, (byte) 36, (byte) 170)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 156, (byte) 39, (byte) 176 /*0xB0*/)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 171, (byte) 71, (byte) 188)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 186, (byte) 104, (byte) 200)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 206, (byte) 147, (byte) 216)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 225, (byte) 190, (byte) 231)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 243, (byte) 229, (byte) 245))
      };
    return this.purpleChromeBrushes;
  }

  public List<Brush> GetBlueChromeBrushes()
  {
    if (this.blueChromeBrushes == null)
      this.blueChromeBrushes = new List<Brush>()
      {
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 13, (byte) 71, (byte) 161)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 21, (byte) 101, (byte) 192 /*0xC0*/)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 25, (byte) 118, (byte) 210)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 30, (byte) 136, (byte) 229)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 33, (byte) 150, (byte) 243)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 66, (byte) 165, (byte) 245)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 100, (byte) 181, (byte) 246)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 144 /*0x90*/, (byte) 202, (byte) 249)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 187, (byte) 222, (byte) 251)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 224 /*0xE0*/, (byte) 242, byte.MaxValue))
      };
    return this.blueChromeBrushes;
  }

  public List<Brush> GetGreenChromeBrushes()
  {
    if (this.greenChromeBrushes == null)
      this.greenChromeBrushes = new List<Brush>()
      {
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 27, (byte) 94, (byte) 32 /*0x20*/)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 46, (byte) 125, (byte) 50)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 56, (byte) 142, (byte) 60)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 67, (byte) 160 /*0xA0*/, (byte) 71)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 76, (byte) 175, (byte) 80 /*0x50*/)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 187, (byte) 106)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 129, (byte) 199, (byte) 132)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 165, (byte) 214, (byte) 167)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 200, (byte) 230, (byte) 201)),
        (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 232, (byte) 245, (byte) 233))
      };
    return this.greenChromeBrushes;
  }

  public List<Brush> GetEliteBrushes()
  {
    if (this.eliteBrushes == null)
    {
      this.eliteBrushes = new List<Brush>();
      LinearGradientBrush linearGradientBrush1 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop1 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 249, (byte) 182, (byte) 220),
        Offset = 0.0
      };
      GradientStop gradientStop2 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 234, (byte) 14, (byte) 140),
        Offset = 0.5
      };
      linearGradientBrush1.GradientStops.Add(gradientStop1);
      linearGradientBrush1.GradientStops.Add(gradientStop2);
      this.eliteBrushes.Add((Brush) linearGradientBrush1);
      LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop3 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 180, (byte) 222, (byte) 236),
        Offset = 0.0
      };
      GradientStop gradientStop4 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 6, (byte) 145, (byte) 193),
        Offset = 0.5
      };
      linearGradientBrush2.GradientStops.Add(gradientStop3);
      linearGradientBrush2.GradientStops.Add(gradientStop4);
      this.eliteBrushes.Add((Brush) linearGradientBrush2);
      LinearGradientBrush linearGradientBrush3 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop5 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 211, (byte) 189, (byte) 219),
        Offset = 0.0
      };
      GradientStop gradientStop6 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 146, (byte) 92, (byte) 167),
        Offset = 0.5
      };
      linearGradientBrush3.GradientStops.Add(gradientStop5);
      linearGradientBrush3.GradientStops.Add(gradientStop6);
      this.eliteBrushes.Add((Brush) linearGradientBrush3);
      LinearGradientBrush linearGradientBrush4 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop7 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 240 /*0xF0*/, (byte) 180),
        Offset = 0.0
      };
      GradientStop gradientStop8 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 254, (byte) 207, (byte) 13),
        Offset = 0.5
      };
      linearGradientBrush4.GradientStops.Add(gradientStop7);
      linearGradientBrush4.GradientStops.Add(gradientStop8);
      this.eliteBrushes.Add((Brush) linearGradientBrush4);
      LinearGradientBrush linearGradientBrush5 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop9 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 193, (byte) 228, (byte) 187),
        Offset = 0.0
      };
      GradientStop gradientStop10 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 105, (byte) 189, (byte) 91),
        Offset = 0.5
      };
      linearGradientBrush5.GradientStops.Add(gradientStop9);
      linearGradientBrush5.GradientStops.Add(gradientStop10);
      this.eliteBrushes.Add((Brush) linearGradientBrush5);
      LinearGradientBrush linearGradientBrush6 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop11 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 241, (byte) 179, (byte) 116),
        Offset = 0.0
      };
      GradientStop gradientStop12 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 241, (byte) 90, (byte) 36),
        Offset = 0.5
      };
      linearGradientBrush6.GradientStops.Add(gradientStop11);
      linearGradientBrush6.GradientStops.Add(gradientStop12);
      this.eliteBrushes.Add((Brush) linearGradientBrush6);
      LinearGradientBrush linearGradientBrush7 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop13 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 238, (byte) 184, (byte) 236),
        Offset = 0.0
      };
      GradientStop gradientStop14 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 221, (byte) 116, (byte) 218),
        Offset = 0.5
      };
      linearGradientBrush7.GradientStops.Add(gradientStop13);
      linearGradientBrush7.GradientStops.Add(gradientStop14);
      this.eliteBrushes.Add((Brush) linearGradientBrush7);
      LinearGradientBrush linearGradientBrush8 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop15 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 178, (byte) 227, (byte) 246),
        Offset = 0.0
      };
      GradientStop gradientStop16 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 104, (byte) 200, (byte) 237),
        Offset = 0.5
      };
      linearGradientBrush8.GradientStops.Add(gradientStop15);
      linearGradientBrush8.GradientStops.Add(gradientStop16);
      this.eliteBrushes.Add((Brush) linearGradientBrush8);
      LinearGradientBrush linearGradientBrush9 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop17 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 229, (byte) 230, (byte) 155),
        Offset = 0.0
      };
      GradientStop gradientStop18 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 204, (byte) 207, (byte) 58),
        Offset = 0.5
      };
      linearGradientBrush9.GradientStops.Add(gradientStop17);
      linearGradientBrush9.GradientStops.Add(gradientStop18);
      this.eliteBrushes.Add((Brush) linearGradientBrush9);
      LinearGradientBrush linearGradientBrush10 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop19 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 225, (byte) 149, (byte) 180),
        Offset = 0.0
      };
      GradientStop gradientStop20 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 199, (byte) 54, (byte) 114),
        Offset = 0.5
      };
      linearGradientBrush10.GradientStops.Add(gradientStop19);
      linearGradientBrush10.GradientStops.Add(gradientStop20);
      this.eliteBrushes.Add((Brush) linearGradientBrush10);
    }
    return this.eliteBrushes;
  }

  public List<Brush> GetSandyBeachBrushes()
  {
    if (this.sandyBeachBrushes == null)
    {
      this.sandyBeachBrushes = new List<Brush>();
      LinearGradientBrush linearGradientBrush1 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop1 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 206, (byte) 128 /*0x80*/),
        Offset = 0.0
      };
      GradientStop gradientStop2 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 158, (byte) 2),
        Offset = 0.5
      };
      linearGradientBrush1.GradientStops.Add(gradientStop1);
      linearGradientBrush1.GradientStops.Add(gradientStop2);
      this.sandyBeachBrushes.Add((Brush) linearGradientBrush1);
      LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop3 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 210, (byte) 165, (byte) 127 /*0x7F*/),
        Offset = 0.0
      };
      GradientStop gradientStop4 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 167, (byte) 78, (byte) 3),
        Offset = 0.5
      };
      linearGradientBrush2.GradientStops.Add(gradientStop3);
      linearGradientBrush2.GradientStops.Add(gradientStop4);
      this.sandyBeachBrushes.Add((Brush) linearGradientBrush2);
      LinearGradientBrush linearGradientBrush3 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop5 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 223, (byte) 183, (byte) 127 /*0x7F*/),
        Offset = 0.0
      };
      GradientStop gradientStop6 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 192 /*0xC0*/, (byte) 112 /*0x70*/, (byte) 2),
        Offset = 0.5
      };
      linearGradientBrush3.GradientStops.Add(gradientStop5);
      linearGradientBrush3.GradientStops.Add(gradientStop6);
      this.sandyBeachBrushes.Add((Brush) linearGradientBrush3);
      LinearGradientBrush linearGradientBrush4 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop7 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 235, (byte) 201, (byte) 142),
        Offset = 0.0
      };
      GradientStop gradientStop8 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 215, (byte) 148, (byte) 32 /*0x20*/),
        Offset = 0.5
      };
      linearGradientBrush4.GradientStops.Add(gradientStop7);
      linearGradientBrush4.GradientStops.Add(gradientStop8);
      this.sandyBeachBrushes.Add((Brush) linearGradientBrush4);
      LinearGradientBrush linearGradientBrush5 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop9 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 248, (byte) 225, (byte) 191),
        Offset = 0.0
      };
      GradientStop gradientStop10 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 242, (byte) 195, (byte) 128 /*0x80*/),
        Offset = 0.5
      };
      linearGradientBrush5.GradientStops.Add(gradientStop9);
      linearGradientBrush5.GradientStops.Add(gradientStop10);
      this.sandyBeachBrushes.Add((Brush) linearGradientBrush5);
      LinearGradientBrush linearGradientBrush6 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop11 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 248, (byte) 230, (byte) 204),
        Offset = 0.0
      };
      GradientStop gradientStop12 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 242, (byte) 206, (byte) 155),
        Offset = 0.5
      };
      linearGradientBrush6.GradientStops.Add(gradientStop11);
      linearGradientBrush6.GradientStops.Add(gradientStop12);
      this.sandyBeachBrushes.Add((Brush) linearGradientBrush6);
      LinearGradientBrush linearGradientBrush7 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop13 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 219, (byte) 236, (byte) 217),
        Offset = 0.0
      };
      GradientStop gradientStop14 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 186, (byte) 220, (byte) 183),
        Offset = 0.5
      };
      linearGradientBrush7.GradientStops.Add(gradientStop13);
      linearGradientBrush7.GradientStops.Add(gradientStop14);
      this.sandyBeachBrushes.Add((Brush) linearGradientBrush7);
      LinearGradientBrush linearGradientBrush8 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop15 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 209, (byte) 232, (byte) 184),
        Offset = 0.0
      };
      GradientStop gradientStop16 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 168, (byte) 212, (byte) 119),
        Offset = 0.5
      };
      linearGradientBrush8.GradientStops.Add(gradientStop15);
      linearGradientBrush8.GradientStops.Add(gradientStop16);
      this.sandyBeachBrushes.Add((Brush) linearGradientBrush8);
      LinearGradientBrush linearGradientBrush9 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop17 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 246, (byte) 232, (byte) 175),
        Offset = 0.0
      };
      GradientStop gradientStop18 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 237, (byte) 212, (byte) 102),
        Offset = 0.5
      };
      linearGradientBrush9.GradientStops.Add(gradientStop17);
      linearGradientBrush9.GradientStops.Add(gradientStop18);
      this.sandyBeachBrushes.Add((Brush) linearGradientBrush9);
      LinearGradientBrush linearGradientBrush10 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop19 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 254, (byte) 198, (byte) 174),
        Offset = 0.0
      };
      GradientStop gradientStop20 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 253, (byte) 146, (byte) 100),
        Offset = 0.5
      };
      linearGradientBrush10.GradientStops.Add(gradientStop19);
      linearGradientBrush10.GradientStops.Add(gradientStop20);
      this.sandyBeachBrushes.Add((Brush) linearGradientBrush10);
    }
    return this.sandyBeachBrushes;
  }

  public List<Brush> GetLightCandyBrushes()
  {
    if (this.lightCandyBrushes == null)
    {
      this.lightCandyBrushes = new List<Brush>();
      LinearGradientBrush linearGradientBrush1 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop1 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 178, (byte) 223, (byte) 195),
        Offset = 0.0
      };
      GradientStop gradientStop2 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 102, (byte) 192 /*0xC0*/, (byte) 136),
        Offset = 0.5
      };
      linearGradientBrush1.GradientStops.Add(gradientStop1);
      linearGradientBrush1.GradientStops.Add(gradientStop2);
      this.lightCandyBrushes.Add((Brush) linearGradientBrush1);
      LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop3 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 163, (byte) 171, (byte) 179),
        Offset = 0.0
      };
      GradientStop gradientStop4 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 74, (byte) 91, (byte) 105),
        Offset = 0.5
      };
      linearGradientBrush2.GradientStops.Add(gradientStop3);
      linearGradientBrush2.GradientStops.Add(gradientStop4);
      this.lightCandyBrushes.Add((Brush) linearGradientBrush2);
      LinearGradientBrush linearGradientBrush3 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop5 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 247, (byte) 176 /*0xB0*/, (byte) 176 /*0xB0*/),
        Offset = 0.0
      };
      GradientStop gradientStop6 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 240 /*0xF0*/, (byte) 99, (byte) 98),
        Offset = 0.5
      };
      linearGradientBrush3.GradientStops.Add(gradientStop5);
      linearGradientBrush3.GradientStops.Add(gradientStop6);
      this.lightCandyBrushes.Add((Brush) linearGradientBrush3);
      LinearGradientBrush linearGradientBrush4 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop7 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 160 /*0xA0*/, (byte) 226, (byte) 248),
        Offset = 0.0
      };
      GradientStop gradientStop8 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 67, (byte) 197, (byte) 241),
        Offset = 0.5
      };
      linearGradientBrush4.GradientStops.Add(gradientStop7);
      linearGradientBrush4.GradientStops.Add(gradientStop8);
      this.lightCandyBrushes.Add((Brush) linearGradientBrush4);
      LinearGradientBrush linearGradientBrush5 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop9 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 233, (byte) 177, (byte) 143),
        Offset = 0.0
      };
      GradientStop gradientStop10 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 211, (byte) 101, (byte) 33),
        Offset = 0.5
      };
      linearGradientBrush5.GradientStops.Add(gradientStop9);
      linearGradientBrush5.GradientStops.Add(gradientStop10);
      this.lightCandyBrushes.Add((Brush) linearGradientBrush5);
      LinearGradientBrush linearGradientBrush6 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop11 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 234, (byte) 159),
        Offset = 0.0
      };
      GradientStop gradientStop12 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 214, (byte) 65),
        Offset = 0.5
      };
      linearGradientBrush6.GradientStops.Add(gradientStop11);
      linearGradientBrush6.GradientStops.Add(gradientStop12);
      this.lightCandyBrushes.Add((Brush) linearGradientBrush6);
      LinearGradientBrush linearGradientBrush7 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop13 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 184, (byte) 180, (byte) 209),
        Offset = 0.0
      };
      GradientStop gradientStop14 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 120, (byte) 113, (byte) 167),
        Offset = 0.5
      };
      linearGradientBrush7.GradientStops.Add(gradientStop13);
      linearGradientBrush7.GradientStops.Add(gradientStop14);
      this.lightCandyBrushes.Add((Brush) linearGradientBrush7);
      LinearGradientBrush linearGradientBrush8 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop15 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 155, (byte) 209, (byte) 199),
        Offset = 0.0
      };
      GradientStop gradientStop16 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 66, (byte) 167, (byte) 149),
        Offset = 0.5
      };
      linearGradientBrush8.GradientStops.Add(gradientStop15);
      linearGradientBrush8.GradientStops.Add(gradientStop16);
      this.lightCandyBrushes.Add((Brush) linearGradientBrush8);
      LinearGradientBrush linearGradientBrush9 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop17 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 202, (byte) 203, (byte) 222),
        Offset = 0.0
      };
      GradientStop gradientStop18 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 154, (byte) 156, (byte) 192 /*0xC0*/),
        Offset = 0.5
      };
      linearGradientBrush9.GradientStops.Add(gradientStop17);
      linearGradientBrush9.GradientStops.Add(gradientStop18);
      this.lightCandyBrushes.Add((Brush) linearGradientBrush9);
      LinearGradientBrush linearGradientBrush10 = new LinearGradientBrush()
      {
        StartPoint = new Point(0.0, 0.0),
        EndPoint = new Point(1.0, 1.0)
      };
      GradientStop gradientStop19 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 221, (byte) 213, (byte) 180),
        Offset = 0.0
      };
      GradientStop gradientStop20 = new GradientStop()
      {
        Color = Color.FromArgb(byte.MaxValue, (byte) 191, (byte) 175, (byte) 114),
        Offset = 0.5
      };
      linearGradientBrush10.GradientStops.Add(gradientStop19);
      linearGradientBrush10.GradientStops.Add(gradientStop20);
      this.lightCandyBrushes.Add((Brush) linearGradientBrush10);
    }
    return this.lightCandyBrushes;
  }

  public Brush GetBrush(int colorIndex)
  {
    return this.currentBrushes != null && this.currentBrushes.Count > 0 ? this.currentBrushes[colorIndex % this.currentBrushes.Count<Brush>()] : (Brush) new SolidColorBrush(Colors.Transparent);
  }

  internal void ApplyPalette(ChartColorPalette palette)
  {
    switch (palette)
    {
      case ChartColorPalette.Metro:
        this.currentBrushes = this.GetMetroBrushes();
        break;
      case ChartColorPalette.Custom:
        this.currentBrushes = this.CustomBrushes;
        break;
      case ChartColorPalette.AutumnBrights:
        this.currentBrushes = this.GetAutumnBrushes();
        break;
      case ChartColorPalette.FloraHues:
        this.currentBrushes = this.GetFloraHuesBrushes();
        break;
      case ChartColorPalette.Pineapple:
        this.currentBrushes = this.GetPineappleBrushes();
        break;
      case ChartColorPalette.TomotoSpectrum:
        this.currentBrushes = this.GetTomatoSpectrumBrushes();
        break;
      case ChartColorPalette.RedChrome:
        this.currentBrushes = this.GetRedChromeBrushes();
        break;
      case ChartColorPalette.PurpleChrome:
        this.currentBrushes = this.GetPurpleChromeBrushes();
        break;
      case ChartColorPalette.BlueChrome:
        this.currentBrushes = this.GetBlueChromeBrushes();
        break;
      case ChartColorPalette.GreenChrome:
        this.currentBrushes = this.GetGreenChromeBrushes();
        break;
      case ChartColorPalette.Elite:
        this.currentBrushes = this.GetEliteBrushes();
        break;
      case ChartColorPalette.SandyBeach:
        this.currentBrushes = this.GetSandyBeachBrushes();
        break;
      case ChartColorPalette.LightCandy:
        this.currentBrushes = this.GetLightCandyBrushes();
        break;
    }
  }

  internal void Dispose()
  {
    this.CustomBrushes = (List<Brush>) null;
    this.ChartArea = (ChartBase) null;
    this.Series = (ChartSeriesBase) null;
  }
}
