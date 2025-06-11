// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ImageEffect
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class ImageEffect
{
  internal const byte SharpenAmountKey = 0;
  internal const byte ColorTempratureKey = 1;
  internal const byte SaturationKey = 2;
  internal const byte BrightnessKey = 3;
  internal const byte ContrastKey = 4;
  internal const byte PencilSizeKey = 5;
  internal const byte TransparencyKey = 6;
  internal const byte BlurRadiusKey = 7;
  internal const byte CementTransparencyKey = 8;
  internal const byte CementCrackingKey = 9;
  internal const byte ChalkSketchTransparencyKey = 10;
  internal const byte ChalkSketchPressureKey = 11;
  internal const byte CrisscrossEtchingTransparencyKey = 12;
  internal const byte CrisscrossEtchingPressureKey = 13;
  internal const byte CutoutTransparencyKey = 14;
  internal const byte CutoutShadesKey = 15;
  internal const byte GrainTransparencyKey = 16 /*0x10*/;
  internal const byte GrainSizeKey = 17;
  internal const byte GlassTransparencyKey = 18;
  internal const byte GlassScalingKey = 19;
  internal const byte GlowDiffusedTransparencyKey = 20;
  internal const byte GlowDiffusedIntensityKey = 21;
  internal const byte GlowEdgesTransparencyKey = 22;
  internal const byte GlowEdgesSmoothnessKey = 23;
  internal const byte LightScreenTransparencyKey = 24;
  internal const byte LightScreenGridKey = 25;
  internal const byte LineDrawingTransparencyKey = 26;
  internal const byte LineDrawingPensilSizeKey = 27;
  internal const byte MarkerTransparencyKey = 28;
  internal const byte MarkerSizeKey = 29;
  internal const byte MosiaicBubbleTransparencyKey = 30;
  internal const byte MosiaicBubblePressureKey = 31 /*0x1F*/;
  internal const byte StrokeTransparencyKey = 32 /*0x20*/;
  internal const byte StrokeIntensityKey = 33;
  internal const byte BrushTransparencyKey = 34;
  internal const byte BrushSizeKey = 35;
  internal const byte PastelTransparencyKey = 36;
  internal const byte PastelSizeKey = 37;
  internal const byte PencilGrayScaleTransparencyKey = 38;
  internal const byte PencilGrayScaleSizeKey = 39;
  internal const byte PencilSketchTransparencyKey = 40;
  internal const byte PencilSketchSizeKey = 41;
  internal const byte PhotocopyTransparencyKey = 42;
  internal const byte PhotocopySizeKey = 43;
  internal const byte PlasticWrapTransparencyKey = 44;
  internal const byte PlasticWrapSmoothnessKey = 45;
  internal const byte TexturizerTransparencyKey = 46;
  internal const byte TexturizerSizeKey = 47;
  internal const byte SpongeTransparencyKey = 48 /*0x30*/;
  internal const byte SpongeBrushSizeKey = 49;
  internal const byte BackgroundRemovalRectangleKey = 50;
  private List<PointF> m_foregroundVertices;
  private List<PointF> m_backgroundVertices;
  private Dictionary<int, object> m_propertiesHash;
  private bool m_hasBackgroundRemoval;

  internal float SharpenAmount
  {
    get => (float) this.PropertiesHash[0];
    set
    {
      if ((double) value < -100000.0 || (double) value > 100000.0)
        throw new ArgumentException("Sharpen Amount should be between -100% to 100%");
      this.SetKeyValue(0, (object) value);
    }
  }

  internal float ColorTemprature
  {
    get => (float) this.PropertiesHash[1];
    set
    {
      if ((double) value < 1500.0 || (double) value > 11500.0)
        throw new ArgumentException("ColorTemprature should be between 1500 to 11500");
      this.SetKeyValue(1, (object) value);
    }
  }

  internal float Saturation
  {
    get => (float) this.PropertiesHash[2];
    set
    {
      if ((double) value < 0.0 || (double) value > 400000.0)
        throw new ArgumentException("Saturation should be between 0% to 400%");
      this.SetKeyValue(2, (object) value);
    }
  }

  internal float Brightness
  {
    get => (float) this.PropertiesHash[3];
    set
    {
      if ((double) value < -100000.0 || (double) value > 100000.0)
        throw new ArgumentException("Brightness should be between -100% to 100%");
      this.SetKeyValue(3, (object) value);
    }
  }

  internal float Contrast
  {
    get => (float) this.PropertiesHash[4];
    set
    {
      if ((double) value < -100000.0 || (double) value > 100000.0)
        throw new ArgumentException("Contrast should be between -100% to 100%");
      this.SetKeyValue(4, (object) value);
    }
  }

  internal float BlurRadius
  {
    get => (float) this.PropertiesHash[7];
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("Blur radius should be between 0 to 100");
      this.SetKeyValue(7, (object) value);
    }
  }

  internal float CementTransparency
  {
    get => (float) this.PropertiesHash[8];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Cement transparency should be between 0% to 100%");
      this.SetKeyValue(8, (object) value);
    }
  }

  internal float CementCracking
  {
    get => (float) this.PropertiesHash[9];
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("Cement space cracking should be between 0 to 100");
      this.SetKeyValue(9, (object) value);
    }
  }

  internal float ChalkSketchTransparency
  {
    get => (float) this.PropertiesHash[10];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Chalk Sketch transparency should be between 0% to 100%");
      this.SetKeyValue(10, (object) value);
    }
  }

  internal float ChalkSketchPressure
  {
    get => (float) this.PropertiesHash[11];
    set
    {
      if ((double) value < 0.0 || (double) value > 4.0)
        throw new ArgumentException("Chalk Sketch Pressure should be between 0 to 4");
      this.SetKeyValue(11, (object) value);
    }
  }

  internal float CrisscrossEtchingTransparency
  {
    get => (float) this.PropertiesHash[12];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Crisscross Etching Transparency should be between 0% to 100%");
      this.SetKeyValue(12, (object) value);
    }
  }

  internal float CrisscrossEtchingPressure
  {
    get => (float) this.PropertiesHash[13];
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("Crisscross Etching Pressure should be between 0 to 100");
      this.SetKeyValue(13, (object) value);
    }
  }

  internal float CutoutTransparency
  {
    get => (float) this.PropertiesHash[14];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Cutout Transparency should be between 0% to 100%");
      this.SetKeyValue(14, (object) value);
    }
  }

  internal float CutoutShades
  {
    get => (float) this.PropertiesHash[15];
    set
    {
      if ((double) value < 0.0 || (double) value > 6.0)
        throw new ArgumentException("Cutout shades should be between 0 to 6");
      this.SetKeyValue(15, (object) value);
    }
  }

  internal float GrainTransparency
  {
    get => (float) this.PropertiesHash[16 /*0x10*/];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Grain Transparency should be between 0% to 100%");
      this.SetKeyValue(16 /*0x10*/, (object) value);
    }
  }

  internal float GrainSize
  {
    get => (float) this.PropertiesHash[17];
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("Grain Size should be between 0 to 100");
      this.SetKeyValue(17, (object) value);
    }
  }

  internal float GlassTransparency
  {
    get => (float) this.PropertiesHash[18];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Glass Transparency should be between 0% to 100%");
      this.SetKeyValue(18, (object) value);
    }
  }

  internal float GlassScaling
  {
    get => (float) this.PropertiesHash[19];
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("Glass Scaling should be between 0 to 100");
      this.SetKeyValue(19, (object) value);
    }
  }

  internal float GlowDiffusedTransparency
  {
    get => (float) this.PropertiesHash[20];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Glow Transparency should be between 0% to 100%");
      this.SetKeyValue(20, (object) value);
    }
  }

  internal float GlowDiffusedIntensity
  {
    get => (float) this.PropertiesHash[21];
    set
    {
      if ((double) value < 0.0 || (double) value > 10.0)
        throw new ArgumentException("Glow Scaling should be between 0 to 10");
      this.SetKeyValue(21, (object) value);
    }
  }

  internal float GlowEdgesTransparency
  {
    get => (float) this.PropertiesHash[22];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Glow edges Transparency should be between 0% to 100%");
      this.SetKeyValue(20, (object) value);
    }
  }

  internal float GlowEdgesSmoothness
  {
    get => (float) this.PropertiesHash[23];
    set
    {
      if ((double) value < 0.0 || (double) value > 10.0)
        throw new ArgumentException("Glow edges smoothing factor should be between 0 to 10");
      this.SetKeyValue(23, (object) value);
    }
  }

  internal float LightScreenTransparency
  {
    get => (float) this.PropertiesHash[24];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Light screen Transparency should be between 0% to 100%");
      this.SetKeyValue(24, (object) value);
    }
  }

  internal float LightScreenGrid
  {
    get => (float) this.PropertiesHash[25];
    set
    {
      if ((double) value < 0.0 || (double) value > 10.0)
        throw new ArgumentException("Lightscreen grid factor should be between 0 to 10");
      this.SetKeyValue(25, (object) value);
    }
  }

  internal float LineDrawingTransparency
  {
    get => (float) this.PropertiesHash[26];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("LineDrawing Transparency should be between 0% to 100%");
      this.SetKeyValue(26, (object) value);
    }
  }

  internal float LineDrawingPensilSize
  {
    get => (float) this.PropertiesHash[27];
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("LineDrawing pensil size should be between 0 to 100");
      this.SetKeyValue(27, (object) value);
    }
  }

  internal float MarkerTransparency
  {
    get => (float) this.PropertiesHash[28];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Marker Transparency should be between 0% to 100%");
      this.SetKeyValue(28, (object) value);
    }
  }

  internal float MarkerSize
  {
    get => (float) this.PropertiesHash[29];
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("Marker size should be between 0 to 100");
      this.SetKeyValue(29, (object) value);
    }
  }

  internal float MosiaicBubbleTransparency
  {
    get => (float) this.PropertiesHash[30];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Mosiaic Transparency should be between 0% to 100%");
      this.SetKeyValue(30, (object) value);
    }
  }

  internal float MosiaicBubblePressure
  {
    get => (float) this.PropertiesHash[31 /*0x1F*/];
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("Mosiaic pressure should be between 0 to 100");
      this.SetKeyValue(31 /*0x1F*/, (object) value);
    }
  }

  internal float StrokeTransparency
  {
    get => (float) this.PropertiesHash[32 /*0x20*/];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Paint stroke Transparency should be between 0% to 100%");
      this.SetKeyValue(32 /*0x20*/, (object) value);
    }
  }

  internal float StrokeIntensity
  {
    get => (float) this.PropertiesHash[33];
    set
    {
      if ((double) value < 0.0 || (double) value > 10.0)
        throw new ArgumentException("Paint Stroke intensity should be between 0 to 10");
      this.SetKeyValue(33, (object) value);
    }
  }

  internal float BrushTransparency
  {
    get => (float) this.PropertiesHash[34];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Brush Transparency should be between 0% to 100%");
      this.SetKeyValue(34, (object) value);
    }
  }

  internal float BrushSize
  {
    get => (float) this.PropertiesHash[35];
    set
    {
      if ((double) value < 0.0 || (double) value > 10.0)
        throw new ArgumentException("Brush size should be between 0 to 10");
      this.SetKeyValue(35, (object) value);
    }
  }

  internal float PastelTransparency
  {
    get => (float) this.PropertiesHash[36];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Pastel Smooth Transparency should be between 0% to 100%");
      this.SetKeyValue(36, (object) value);
    }
  }

  internal float PastelSize
  {
    get => (float) this.PropertiesHash[37];
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("Pastel size should be between 0 to 100");
      this.SetKeyValue(37, (object) value);
    }
  }

  internal float PencilGrayScaleTransparency
  {
    get => (float) this.PropertiesHash[38];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Pastel Smooth Transparency should be between 0% to 100%");
      this.SetKeyValue(38, (object) value);
    }
  }

  internal float PencilGraySize
  {
    get => (float) this.PropertiesHash[39];
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("Pastel size should be between 0 to 100");
      this.SetKeyValue(39, (object) value);
    }
  }

  internal float PencilSketchTransparency
  {
    get => (float) this.PropertiesHash[40];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Pastel Smooth Transparency should be between 0% to 100%");
      this.SetKeyValue(40, (object) value);
    }
  }

  internal float PencilSketchSize
  {
    get => (float) this.PropertiesHash[41];
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("Pastel size should be between 0 to 100");
      this.SetKeyValue(41, (object) value);
    }
  }

  internal float PhotocopyTransparency
  {
    get => (float) this.PropertiesHash[42];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Pastel Smooth Transparency should be between 0% to 100%");
      this.SetKeyValue(42, (object) value);
    }
  }

  internal float PhotocopySize
  {
    get => (float) this.PropertiesHash[43];
    set
    {
      if ((double) value < 0.0 || (double) value > 10.0)
        throw new ArgumentException("Pastel size should be between 0 to 10");
      this.SetKeyValue(43, (object) value);
    }
  }

  internal float PlasticWrapTransparency
  {
    get => (float) this.PropertiesHash[44];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Plastic wrap Transparency should be between 0% to 100%");
      this.SetKeyValue(44, (object) value);
    }
  }

  internal float PlasticWrapSmoothness
  {
    get => (float) this.PropertiesHash[45];
    set
    {
      if ((double) value < 0.0 || (double) value > 10.0)
        throw new ArgumentException("Plastic wrap smoothness should be between 0 to 10");
      this.SetKeyValue(45, (object) value);
    }
  }

  internal float TexturizerTransparency
  {
    get => (float) this.PropertiesHash[46];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Texturizer Transparency should be between 0% to 100%");
      this.SetKeyValue(46, (object) value);
    }
  }

  internal float TexturizerSize
  {
    get => (float) this.PropertiesHash[47];
    set
    {
      if ((double) value < 0.0 || (double) value > 100.0)
        throw new ArgumentException("Texturizer Size should be between 0 to 100");
      this.SetKeyValue(47, (object) value);
    }
  }

  internal float SpongeTransparency
  {
    get => (float) this.PropertiesHash[48 /*0x30*/];
    set
    {
      if ((double) value < 0.0 || (double) value > 100000.0)
        throw new ArgumentException("Watercolor Sponge Transparency should be between 0% to 100%");
      this.SetKeyValue(48 /*0x30*/, (object) value);
    }
  }

  internal float SpongeBrushSize
  {
    get => (float) this.PropertiesHash[49];
    set
    {
      if ((double) value < 0.0 || (double) value > 10.0)
        throw new ArgumentException("Sponge Brush Size should be between 0 to 10");
      this.SetKeyValue(49, (object) value);
    }
  }

  internal List<PointF> ForegroundVertices
  {
    get
    {
      if (this.m_foregroundVertices == null)
        this.m_foregroundVertices = new List<PointF>();
      return this.m_foregroundVertices;
    }
    set => this.m_foregroundVertices = value;
  }

  internal List<PointF> BackgroundVertices
  {
    get
    {
      if (this.m_backgroundVertices == null)
        this.m_backgroundVertices = new List<PointF>();
      return this.m_backgroundVertices;
    }
    set => this.m_backgroundVertices = value;
  }

  internal TileRectangle BackgroundRemovalRectangle
  {
    get
    {
      if (!this.PropertiesHash.ContainsKey(50))
        this.PropertiesHash.Add(50, (object) new TileRectangle());
      if (!(this.PropertiesHash[50] is TileRectangle))
        this.PropertiesHash[50] = (object) new TileRectangle();
      return this.PropertiesHash[50] as TileRectangle;
    }
    set => this.SetKeyValue(50, (object) value);
  }

  internal bool HasBackgroundRemovalEffect
  {
    get => this.m_hasBackgroundRemoval;
    set => this.m_hasBackgroundRemoval = value;
  }

  internal Dictionary<int, object> PropertiesHash
  {
    get
    {
      if (this.m_propertiesHash == null)
        this.m_propertiesHash = new Dictionary<int, object>();
      return this.m_propertiesHash;
    }
  }

  internal ImageEffect()
  {
    this.ColorTemprature = 6500f;
    this.Saturation = 100000f;
  }

  private void SetKeyValue(int propKey, object value) => this[propKey] = value;

  protected object this[int key]
  {
    get => (object) key;
    set => this.PropertiesHash[key] = value;
  }

  internal void Close()
  {
    if (this.m_propertiesHash != null || this.m_propertiesHash.Count > 0)
    {
      this.m_propertiesHash.Clear();
      this.m_propertiesHash = (Dictionary<int, object>) null;
    }
    if (this.m_backgroundVertices != null && this.m_backgroundVertices.Count > 0)
    {
      this.m_backgroundVertices.Clear();
      this.m_backgroundVertices = (List<PointF>) null;
    }
    if (this.m_foregroundVertices == null || this.m_foregroundVertices.Count <= 0)
      return;
    this.m_foregroundVertices.Clear();
    this.m_foregroundVertices = (List<PointF>) null;
  }

  internal ImageEffect Clone()
  {
    ImageEffect imageEffect = (ImageEffect) this.MemberwiseClone();
    if (this.PropertiesHash != null && this.PropertiesHash.Count > 0)
    {
      imageEffect.m_propertiesHash = new Dictionary<int, object>();
      foreach (KeyValuePair<int, object> keyValuePair in this.PropertiesHash)
        imageEffect.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    }
    if (this.BackgroundVertices != null && this.BackgroundVertices.Count > 0)
    {
      imageEffect.m_backgroundVertices = new List<PointF>();
      foreach (PointF backgroundVertex in this.BackgroundVertices)
        imageEffect.BackgroundVertices.Add(backgroundVertex);
    }
    if (this.ForegroundVertices != null && this.ForegroundVertices.Count > 0)
    {
      imageEffect.m_foregroundVertices = new List<PointF>();
      foreach (PointF foregroundVertex in this.ForegroundVertices)
        imageEffect.ForegroundVertices.Add(foregroundVertex);
    }
    return imageEffect;
  }
}
