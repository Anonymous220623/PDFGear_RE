// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.BlipFormat
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

internal class BlipFormat
{
  internal const byte ColorFromOpacityKey = 5;
  internal const byte ColorToOpacityKey = 6;
  internal const byte HasAlphaKey = 1;
  private float m_threshold;
  private Color m_inverseColor;
  private float m_inverseOpacity;
  private float m_alphaReplaceAmount;
  private float m_bilevelThreshold;
  private float m_blurRadius;
  private Color m_colorFrom;
  private Color m_colorTo;
  private float m_colorFromOpacity;
  private float m_colorToOpacity;
  private float m_hue;
  private float m_luminance;
  private float m_saturation;
  private float m_brightness;
  private float m_contrast;
  private float m_tintAmount;
  private float m_tintHue;
  private Color m_duotoneColor;
  private Color m_duotonePresetColor;
  private float m_duotoneOpacity;
  private Dictionary<string, Stream> m_docxProps;
  private ImageEffect m_imageEffect;
  private List<string> m_extensionUri;
  private ImageRecord m_ImageRecord;
  private byte m_flagA;
  private float m_transparency;
  private Shape m_shape;
  private BlipTransparency m_blipTransparency;

  internal float Threshold
  {
    get => this.m_threshold;
    set => this.m_threshold = value;
  }

  internal Color InverseColor
  {
    get => this.m_inverseColor;
    set => this.m_inverseColor = value;
  }

  internal float InverseOpacity
  {
    get => this.m_inverseOpacity;
    set => this.m_inverseOpacity = value;
  }

  internal float AlphaReplaceAmount
  {
    get => this.m_alphaReplaceAmount;
    set => this.m_alphaReplaceAmount = value;
  }

  internal float BilevelThreshold
  {
    get => this.m_bilevelThreshold;
    set => this.m_bilevelThreshold = value;
  }

  internal bool Grow
  {
    get => ((int) this.m_flagA & 1) != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 254 | (value ? 1 : 0));
  }

  internal float BlurRadius
  {
    get => this.m_blurRadius;
    set => this.m_blurRadius = value;
  }

  internal bool HasAlpha
  {
    get => ((int) this.m_flagA & 2) >> 1 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 253 | (value ? 1 : 0) << 1);
  }

  internal Color ColorFrom
  {
    get => this.m_colorFrom;
    set => this.m_colorFrom = value;
  }

  internal Color ColorTo
  {
    get => this.m_colorTo;
    set => this.m_colorTo = value;
  }

  internal float ColorFromOpacity
  {
    get => this.m_colorFromOpacity;
    set
    {
      this.m_flagA = (byte) ((int) this.m_flagA & 223 | 32 /*0x20*/);
      this.m_colorFromOpacity = value;
    }
  }

  internal float ColorToOpacity
  {
    get => this.m_colorToOpacity;
    set
    {
      this.m_flagA = (byte) ((int) this.m_flagA & 191 | 64 /*0x40*/);
      this.m_colorToOpacity = value;
    }
  }

  internal float Hue
  {
    get => this.m_hue;
    set => this.m_hue = value;
  }

  internal float Luminance
  {
    get => this.m_luminance;
    set => this.m_luminance = value;
  }

  internal float Saturation
  {
    get => this.m_saturation;
    set => this.m_saturation = value;
  }

  internal float Brightness
  {
    get => this.m_brightness;
    set => this.m_brightness = value;
  }

  internal float Contrast
  {
    get => this.m_contrast;
    set => this.m_contrast = value;
  }

  internal float TintAmount
  {
    get => this.m_tintAmount;
    set => this.m_tintAmount = value;
  }

  internal float TintHue
  {
    get => this.m_tintHue;
    set => this.m_tintHue = value;
  }

  internal Color DuotoneColor
  {
    get => this.m_duotoneColor;
    set => this.m_duotoneColor = value;
  }

  internal Color DuotonePresetColor
  {
    get => this.m_duotonePresetColor;
    set => this.m_duotonePresetColor = value;
  }

  internal float DuotoneOpacity
  {
    get => this.m_duotoneOpacity;
    set => this.m_duotoneOpacity = value;
  }

  internal List<string> ExtensionURI
  {
    get
    {
      if (this.m_extensionUri == null)
        this.m_extensionUri = new List<string>();
      return this.m_extensionUri;
    }
    set => this.m_extensionUri = value;
  }

  internal ImageEffect ImageEffect
  {
    get
    {
      if (this.m_imageEffect == null)
        this.m_imageEffect = new ImageEffect();
      return this.m_imageEffect;
    }
    set => this.m_imageEffect = value;
  }

  internal bool HasCompression
  {
    get => ((int) this.m_flagA & 4) >> 2 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 251 | (value ? 1 : 0) << 2);
  }

  internal bool HasImageProperties
  {
    get => ((int) this.m_flagA & 8) >> 3 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 247 | (value ? 1 : 0) << 3);
  }

  internal bool RotateWithObject
  {
    get => ((int) this.m_flagA & 16 /*0x10*/) >> 4 != 0;
    set => this.m_flagA = (byte) ((int) this.m_flagA & 239 | (value ? 1 : 0) << 4);
  }

  internal ImageRecord ImageRecord
  {
    get => this.m_ImageRecord;
    set => this.m_ImageRecord = value;
  }

  internal Dictionary<string, Stream> DocxProps
  {
    get
    {
      if (this.m_docxProps == null)
        this.m_docxProps = new Dictionary<string, Stream>();
      return this.m_docxProps;
    }
  }

  internal float Transparency
  {
    get => this.m_transparency;
    set => this.m_transparency = value;
  }

  internal BlipTransparency BlipTransparency
  {
    get => this.m_blipTransparency;
    set => this.m_blipTransparency = value;
  }

  internal BlipFormat(ShapeBase shape)
  {
  }

  internal BlipFormat(WPicture picture)
  {
  }

  internal void Close()
  {
    if (this.m_docxProps != null && this.m_docxProps.Count > 0)
      this.m_docxProps.Clear();
    if (this.m_imageEffect != null)
    {
      this.m_imageEffect.Close();
      this.m_imageEffect = (ImageEffect) null;
    }
    if (this.m_extensionUri != null && this.m_extensionUri.Count > 0)
      this.m_extensionUri.Clear();
    if (this.m_shape == null)
      return;
    this.m_shape = (Shape) null;
  }

  internal BlipFormat Clone()
  {
    BlipFormat blipFormat = (BlipFormat) this.MemberwiseClone();
    if (this.m_docxProps != null && this.m_docxProps.Count > 0)
      this.m_shape.Document.CloneProperties(this.DocxProps, ref blipFormat.m_docxProps);
    if (this.ImageEffect != null)
      blipFormat.ImageEffect = this.ImageEffect.Clone();
    return blipFormat;
  }

  internal bool HasKey(int propertyKey)
  {
    return ((int) this.m_flagA & (int) (ushort) Math.Pow(2.0, (double) propertyKey)) >> propertyKey != 0;
  }
}
