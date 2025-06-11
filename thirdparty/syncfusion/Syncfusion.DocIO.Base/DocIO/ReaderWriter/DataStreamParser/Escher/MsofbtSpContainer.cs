// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher.MsofbtSpContainer
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO.ReaderWriter.Biff_Records;
using Syncfusion.DocIO.ReaderWriter.Escher;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

#nullable disable
namespace Syncfusion.DocIO.ReaderWriter.DataStreamParser.Escher;

internal class MsofbtSpContainer : BaseContainer
{
  public const int DEF_TXID_INCREMENT = 65536 /*0x010000*/;
  public const string DEF_PICTMARK_STRING = "WordPictureWatermark";
  public const string DEF_TEXTMARK_STRING = "PowerPlusWaterMarkObject";
  public const string DEF_NULL_STRING = "\0";
  private const uint DEF_NOTALLOWINCELL = 2147483648 /*0x80000000*/;
  private MsofbtBSE m_bse;
  private byte m_bFlags;

  internal MsofbtBSE Bse
  {
    get => this.m_bse;
    set => this.m_bse = value;
  }

  internal int Pib
  {
    get => this.ShapeOptions.Pib != null ? (int) this.ShapeOptions.Pib.Value : -1;
    set => this.ShapeOptions.Pib.Value = (uint) value;
  }

  internal MsofbtSp Shape
  {
    get => this.FindContainerByType(typeof (MsofbtSp)) as MsofbtSp;
    set
    {
      MsofbtSp containerByType = (MsofbtSp) this.FindContainerByType(typeof (MsofbtSp));
    }
  }

  internal MsofbtOPT ShapeOptions => this.FindContainerByType(typeof (MsofbtOPT)) as MsofbtOPT;

  internal int Txid
  {
    get => this.ShapeOptions.Txid != null ? (int) this.ShapeOptions.Txid.Value : -1;
    set
    {
      if (this.ShapeOptions == null)
        throw new ArgumentNullException("Shape options are null.");
      if (!(this.ShapeOptions.Properties[128 /*0x80*/] is FOPTEBid property))
        throw new ArgumentException("Txid property does not exist.");
      property.Value = (uint) value;
    }
  }

  internal MsofbtTertiaryFOPT ShapePosition
  {
    get => this.FindContainerByType(typeof (MsofbtTertiaryFOPT)) as MsofbtTertiaryFOPT;
  }

  internal bool IsWatermark
  {
    get => ((int) this.m_bFlags & 1) != 0;
    set => this.m_bFlags = (byte) ((int) this.m_bFlags & 254 | (value ? 1 : 0));
  }

  internal MsofbtSpContainer(WordDocument doc)
    : base(MSOFBT.msofbtSpContainer, doc)
  {
  }

  public uint GetPropertyValue(int key)
  {
    MsofbtOPT shapeOptions = this.ShapeOptions;
    return shapeOptions != null ? shapeOptions.GetPropertyValue(key) : uint.MaxValue;
  }

  public byte[] GetComplexPropValue(int key) => this.ShapeOptions?.GetComplexPropValue(key);

  internal bool HasFillEffect()
  {
    bool flag = false;
    if (this.ShapeOptions == null)
      return false;
    if (!this.ShapeOptions.Properties.ContainsKey(447))
      return true;
    uint propertyValue = this.GetPropertyValue(447);
    if (propertyValue != uint.MaxValue)
      flag = ((int) propertyValue & 16 /*0x10*/) == 16 /*0x10*/;
    return flag;
  }

  internal BackgroundFillType GetBackgroundFillType()
  {
    uint propertyValue = this.GetPropertyValue(384);
    return propertyValue != uint.MaxValue ? (BackgroundFillType) propertyValue : BackgroundFillType.msofillSolid;
  }

  internal BackgroundType GetBackgroundType()
  {
    switch (this.GetPropertyValue(384))
    {
      case 1:
        return BackgroundType.NoBackground;
      case 2:
        return BackgroundType.Texture;
      case 3:
        return BackgroundType.Picture;
      case uint.MaxValue:
        return BackgroundType.NoBackground;
      default:
        return BackgroundType.Gradient;
    }
  }

  internal ImageRecord GetBackgroundImage(EscherClass escher)
  {
    uint propertyValue = this.GetPropertyValue(390);
    if (propertyValue != uint.MaxValue && (long) (propertyValue - 1U) < (long) escher.m_msofbtDggContainer.BstoreContainer.Children.Count)
    {
      MsofbtBSE child = (MsofbtBSE) escher.m_msofbtDggContainer.BstoreContainer.Children[(int) propertyValue - 1];
      if (child.Blip != null)
      {
        try
        {
          return child.Blip.ImageRecord;
        }
        catch
        {
        }
      }
    }
    return (ImageRecord) null;
  }

  internal byte[] GetBackgroundImBytes(EscherClass escher)
  {
    uint propertyValue = this.GetPropertyValue(390);
    if (propertyValue != uint.MaxValue && (long) (propertyValue - 1U) < (long) escher.m_msofbtDggContainer.BstoreContainer.Children.Count)
    {
      MsofbtBSE child = (MsofbtBSE) escher.m_msofbtDggContainer.BstoreContainer.Children[(int) propertyValue - 1];
      if (child.Blip != null)
      {
        try
        {
          return child.Blip.ImageBytes;
        }
        catch
        {
        }
      }
    }
    return (byte[]) null;
  }

  internal Color GetBackgroundColor(bool isPictureBackground)
  {
    Color backgroundColor = Color.White;
    uint propertyValue = this.GetPropertyValue(isPictureBackground ? 387 : 385);
    if (propertyValue != uint.MaxValue)
      backgroundColor = WordColor.ConvertRGBToColor(propertyValue);
    return backgroundColor;
  }

  internal GradientShadingStyle GetGradientShadingStyle(BackgroundFillType fillType)
  {
    if (fillType == BackgroundFillType.msofillShadeCenter)
      return GradientShadingStyle.FromCorner;
    if (fillType == BackgroundFillType.msofillShadeShape)
      return GradientShadingStyle.FromCenter;
    uint propertyValue = this.GetPropertyValue(395);
    if (propertyValue != uint.MaxValue)
    {
      switch (propertyValue)
      {
        case 4286119936:
          return GradientShadingStyle.DiagonalUp;
        case 4289069056:
          return GradientShadingStyle.Vertical;
        case 4292018176:
          return GradientShadingStyle.DiagonalDown;
      }
    }
    return GradientShadingStyle.Horizontal;
  }

  internal GradientShadingVariant GetGradientShadingVariant(GradientShadingStyle shadingStyle)
  {
    if (shadingStyle == GradientShadingStyle.FromCorner)
      return this.GetCornerStyleVariant();
    uint propertyValue = this.GetPropertyValue(396);
    if (propertyValue == uint.MaxValue)
      return GradientShadingVariant.ShadingDown;
    switch (propertyValue)
    {
      case 50:
        return GradientShadingVariant.ShadingMiddle;
      case 4294967246:
        return GradientShadingVariant.ShadingOut;
      default:
        return GradientShadingVariant.ShadingUp;
    }
  }

  internal MsofbtSpContainer CreateRectangleContainer()
  {
    MsofbtSp msofbtSp = new MsofbtSp(this.m_doc);
    msofbtSp.ShapeType = EscherShapeType.msosptRectangle;
    msofbtSp.HasAnchor = true;
    msofbtSp.HasShapeTypeProperty = true;
    msofbtSp.IsBackground = true;
    msofbtSp.ShapeId = 1025;
    MsofbtOPT msofbtOpt = new MsofbtOPT(this.m_doc);
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(447, false, 1048576U /*0x100000*/));
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(459, false, 0U));
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(511 /*0x01FF*/, false, 524288U /*0x080000*/));
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(772, false, 9U));
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(831, false, 65537U /*0x010001*/));
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(191, false, 131072U /*0x020000*/));
    msofbtOpt.Header.Instance = msofbtOpt.Properties.Count;
    MsofbtClientData msofbtClientData = new MsofbtClientData(this.m_doc);
    msofbtClientData.Data = new byte[4]
    {
      (byte) 1,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    MsofbtTertiaryFOPT msofbtTertiaryFopt = new MsofbtTertiaryFOPT(this.m_doc);
    msofbtTertiaryFopt.Unknown1 = 6291520U /*0x600040*/;
    this.Children.Add((object) msofbtSp);
    this.Children.Add((object) msofbtOpt);
    this.Children.Add((object) msofbtTertiaryFopt);
    this.Children.Add((object) msofbtClientData);
    return this;
  }

  internal void UpdateBackground(WordDocument doc, Background background)
  {
    if (background.Type == BackgroundType.NoBackground)
      return;
    this.CheckEscher(doc);
    EscherClass escher = doc.Escher;
    switch (background.Type)
    {
      case BackgroundType.Gradient:
        this.UpdateFillGradient(background.Gradient);
        break;
      case BackgroundType.Picture:
      case BackgroundType.Texture:
        if (background.ImageRecord != null && background.ImageRecord.m_imageBytes != null)
        {
          this.m_bse = new MsofbtBSE(this.m_doc);
          this.m_bse.Initialize(background.ImageRecord);
          this.ShapeOptions.Properties.Remove(390);
          this.ShapeOptions.Properties.Remove(384);
          escher.m_msofbtDggContainer.BstoreContainer.Children.Add((object) this.m_bse);
          int count = escher.m_msofbtDggContainer.BstoreContainer.Children.Count;
          this.UpdateFillPicture(background, count);
          break;
        }
        break;
      case BackgroundType.Color:
        this.UpdateFillColor(background.Color);
        break;
    }
    if (background.Type == BackgroundType.Color)
      return;
    this.SetShapeOption(this.ShapeOptions.Properties, 1310740U, 447, false);
  }

  internal void UpdateFillGradient(BackgroundGradient gradient)
  {
    if (gradient.Color1 != Color.White)
      this.SetShapeOption(this.ShapeOptions.Properties, WordColor.ConvertColorToRGB(gradient.Color1), 385, false);
    if (gradient.Color2 != Color.White)
      this.SetShapeOption(this.ShapeOptions.Properties, WordColor.ConvertColorToRGB(gradient.Color2), 387, false);
    this.AddGradientFillAngle(gradient.ShadingStyle);
    this.AddGradientFillType(gradient.ShadingStyle);
    this.AddFillProperties(gradient.ShadingStyle, gradient.ShadingVariant);
    this.AddGradientFocusFopte(gradient.ShadingStyle, gradient.ShadingVariant);
  }

  internal void UpdateFillPicture(Background background, int fillBlipIndex)
  {
    if (background.Type == BackgroundType.Picture)
      this.SetShapeOption(this.ShapeOptions.Properties, 3U, 384, false);
    else
      this.SetShapeOption(this.ShapeOptions.Properties, 2U, 384, false);
    if (background.PictureBackColor != Color.White)
      this.SetShapeOption(this.ShapeOptions.Properties, WordColor.ConvertColorToRGB(background.PictureBackColor), 387, false);
    this.SetShapeOption(this.ShapeOptions.Properties, (uint) fillBlipIndex, 390, true);
    this.SetShapeOption(this.ShapeOptions.Properties, 2U, 392, false);
  }

  internal void UpdateFillColor(Color color)
  {
    uint rgb1 = WordColor.ConvertColorToRGB(color);
    if (rgb1 == 4278190080U /*0xFF000000*/)
    {
      this.SetBoolShapeOption(this.ShapeOptions.Properties, 447, 16 /*0x10*/, 4, 0, 1048576U /*0x100000*/);
    }
    else
    {
      uint rgb2 = WordColor.ConvertColorToRGB(Color.White, true);
      this.SetBoolShapeOption(this.ShapeOptions.Properties, 447, 16 /*0x10*/, 4, 1, 1048592U /*0x100010*/);
      if ((int) rgb1 != (int) rgb2)
        this.SetShapeOption(this.ShapeOptions.Properties, rgb1, 385, false);
      uint opacity = this.GetOpacity(color.A);
      if (opacity == uint.MaxValue)
        return;
      this.SetShapeOption(this.ShapeOptions.Properties, opacity, 386, false);
    }
  }

  internal void CreateTextWatermarkContainer(int watermarkNum, TextWatermark textWatermark)
  {
    MsofbtSp msofbtSp = new MsofbtSp(this.m_doc);
    msofbtSp.ShapeType = EscherShapeType.msosptTextPlainText;
    msofbtSp.HasShapeTypeProperty = true;
    msofbtSp.HasAnchor = true;
    MsofbtOPT msofbtOpt = new MsofbtOPT(this.m_doc);
    if (textWatermark.Layout == WatermarkLayout.Diagonal)
      msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(4, false, 20643840U /*0x013B0000*/));
    textWatermark.Text = textWatermark.Text == null ? string.Empty : textWatermark.Text;
    byte[] bytes1 = Encoding.Unicode.GetBytes(textWatermark.Text.EndsWith("\0", StringComparison.Ordinal) ? textWatermark.Text : textWatermark.Text + "\0");
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEComplex(192 /*0xC0*/, false, bytes1.Length)
    {
      Value = bytes1
    });
    uint num = (uint) (int) textWatermark.Size << 16 /*0x10*/;
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(195, false, num));
    byte[] bytes2 = Encoding.Unicode.GetBytes(textWatermark.FontName.EndsWith("\0", StringComparison.Ordinal) ? textWatermark.FontName : textWatermark.FontName + "\0");
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEComplex(197, false, bytes2.Length)
    {
      Value = bytes2
    });
    uint rgb = WordColor.ConvertColorToRGB(textWatermark.Color);
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(385, false, rgb));
    if (textWatermark.Semitransparent)
      msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(386, false, 32768U /*0x8000*/));
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(447, false, 1048529U));
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(511 /*0x01FF*/, false, 524288U /*0x080000*/));
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(959, false, 2097184U /*0x200020*/));
    byte[] bytes3 = Encoding.Unicode.GetBytes($"PowerPlusWaterMarkObject{watermarkNum.ToString()}\0");
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEComplex(896, false, bytes3.Length)
    {
      Value = bytes3
    });
    msofbtOpt.Header.Instance = msofbtOpt.Properties.Count;
    MsofbtTertiaryFOPT msofbtTertiaryFopt = new MsofbtTertiaryFOPT(this.m_doc);
    msofbtTertiaryFopt.XAlign = (uint) textWatermark.HorizontalAlignment;
    msofbtTertiaryFopt.YAlign = (uint) textWatermark.VerticalAlignment;
    msofbtTertiaryFopt.XRelTo = (uint) textWatermark.HorizontalOrigin;
    msofbtTertiaryFopt.YRelTo = (uint) textWatermark.VerticalOrigin;
    msofbtTertiaryFopt.AllowInTableCell = false;
    MsofbtClientAnchor msofbtClientAnchor = new MsofbtClientAnchor(this.m_doc);
    msofbtClientAnchor.Data = new byte[4]
    {
      (byte) 2,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    MsofbtClientData msofbtClientData = new MsofbtClientData(this.m_doc);
    msofbtClientData.Data = new byte[4]
    {
      (byte) 1,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    this.Children.Add((object) msofbtSp);
    this.Children.Add((object) msofbtOpt);
    this.Children.Add((object) msofbtTertiaryFopt);
    this.Children.Add((object) msofbtClientAnchor);
    this.Children.Add((object) msofbtClientData);
  }

  internal void CreatePictWatermarkContainer(int watermarkNum, PictureWatermark pictWatermark)
  {
    _Blip blip = (_Blip) null;
    MsofbtSp msofbtSp = new MsofbtSp(this.m_doc);
    msofbtSp.ShapeType = EscherShapeType.msosptPictureFrame;
    msofbtSp.HasShapeTypeProperty = true;
    msofbtSp.HasAnchor = true;
    MsofbtOPT msofbtOpt = new MsofbtOPT(this.m_doc);
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(260, true, 1U));
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(262, false, 2U));
    if (pictWatermark.Washout)
    {
      msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(264, false, 19661U));
      msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(265, false, 22938U));
    }
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(511 /*0x01FF*/, false, 524288U /*0x080000*/));
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(959, false, 2097184U /*0x200020*/));
    byte[] bytes = Encoding.Unicode.GetBytes($"WordPictureWatermark{watermarkNum.ToString()}\0");
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEComplex(896, false, bytes.Length)
    {
      Value = bytes
    });
    msofbtOpt.Header.Instance = msofbtOpt.Properties.Count;
    if (pictWatermark.OriginalPib == -1)
    {
      if (pictWatermark.WordPicture.IsMetaFile)
      {
        blip = (_Blip) new MsofbtMetaFile(pictWatermark.WordPicture.ImageRecord, this.m_doc);
      }
      else
      {
        bool isBitmap = this.IsBitmap(pictWatermark.Picture.RawFormat);
        if (pictWatermark.WordPicture.ImageRecord != null)
          blip = (_Blip) new MsofbtImage(pictWatermark.WordPicture.ImageRecord, isBitmap, this.m_doc);
      }
      Guid uid = blip.Uid;
      MsofbtBSE msofbtBse = new MsofbtBSE(this.m_doc);
      msofbtBse.Header.Instance = (int) blip.Type;
      msofbtBse.Fbse.m_btWin32 = (int) blip.Type;
      msofbtBse.Fbse.m_btMacOS = (int) blip.Type;
      msofbtBse.Fbse.m_rgbUid = uid.ToByteArray();
      msofbtBse.Fbse.m_tag = (int) byte.MaxValue;
      msofbtBse.Fbse.m_cRef = 1;
      msofbtBse.Blip = blip;
      this.Bse = msofbtBse;
    }
    MsofbtTertiaryFOPT msofbtShapePosition = new MsofbtTertiaryFOPT(this.m_doc);
    this.ApplyPictureProperties(msofbtShapePosition, pictWatermark.WordPicture);
    msofbtShapePosition.AllowInTableCell = false;
    MsofbtClientAnchor msofbtClientAnchor = new MsofbtClientAnchor(this.m_doc);
    msofbtClientAnchor.Data = new byte[4]
    {
      (byte) 2,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    MsofbtClientData msofbtClientData = new MsofbtClientData(this.m_doc);
    msofbtClientData.Data = new byte[4]
    {
      (byte) 1,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    this.Children.Add((object) msofbtSp);
    this.Children.Add((object) msofbtOpt);
    this.Children.Add((object) msofbtClientAnchor);
    this.Children.Add((object) msofbtClientData);
    this.Children.Add((object) msofbtShapePosition);
  }

  private void ApplyPictureProperties(MsofbtTertiaryFOPT msofbtShapePosition, WPicture picture)
  {
    if (picture.HorizontalAlignment != ShapeHorizontalAlignment.None)
      msofbtShapePosition.XAlign = (uint) picture.HorizontalAlignment;
    if (picture.VerticalAlignment != ShapeVerticalAlignment.None)
      msofbtShapePosition.YAlign = (uint) picture.VerticalAlignment;
    msofbtShapePosition.XRelTo = picture.HorizontalOrigin == HorizontalOrigin.LeftMargin || picture.HorizontalOrigin == HorizontalOrigin.RightMargin || picture.HorizontalOrigin == HorizontalOrigin.InsideMargin || picture.HorizontalOrigin == HorizontalOrigin.OutsideMargin ? 0U : (uint) picture.HorizontalOrigin;
    msofbtShapePosition.YRelTo = (uint) picture.VerticalOrigin;
  }

  public MsofbtSpContainer CreateImageContainer(WPicture pict, PictureShapeProps pictProps)
  {
    _Blip blip;
    if (pict.ImageRecord != null && pict.ImageRecord.IsMetafile)
    {
      blip = (_Blip) new MsofbtMetaFile(pict.ImageRecord, this.m_doc);
    }
    else
    {
      bool isBitmap = !WordDocument.EnablePartialTrustCode ? this.IsBitmap(pict.Image.RawFormat) : this.IsBitmapForPartialTrustMode(pict.ImageForPartialTrustMode.Format);
      blip = (_Blip) new MsofbtImage(pict.ImageRecord, isBitmap, this.m_doc);
    }
    this.Children.Add((object) new MsofbtSp(this.m_doc)
    {
      ShapeType = EscherShapeType.msosptPictureFrame,
      HasShapeTypeProperty = true,
      HasAnchor = true
    });
    MsofbtOPT msofbtOpt = new MsofbtOPT(this.m_doc);
    if (pict.PictureShape.ShapeContainer != null && pict.PictureShape.ShapeContainer.ShapeOptions != null)
      msofbtOpt = pict.PictureShape.ShapeContainer.ShapeOptions.Clone() as MsofbtOPT;
    this.Children.Add((object) msofbtOpt);
    this.WritePictureOptions(pictProps, pict);
    msofbtOpt.Header.Instance = msofbtOpt.Properties.Count;
    MsofbtBSE msofbtBse = new MsofbtBSE(this.m_doc);
    msofbtBse.Header.Instance = (int) blip.Type;
    msofbtBse.Fbse.m_btWin32 = (int) blip.Type;
    msofbtBse.Fbse.m_btMacOS = (int) blip.Type;
    msofbtBse.Fbse.m_rgbUid = blip.Uid.ToByteArray();
    msofbtBse.Fbse.m_tag = (int) byte.MaxValue;
    msofbtBse.Fbse.m_cRef = 1;
    msofbtBse.Blip = blip;
    this.Bse = msofbtBse;
    MsofbtClientAnchor msofbtClientAnchor = new MsofbtClientAnchor(this.m_doc);
    msofbtClientAnchor.Data = new byte[4];
    MsofbtClientData msofbtClientData = new MsofbtClientData(this.m_doc);
    msofbtClientData.Data = new byte[4]
    {
      (byte) 1,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    this.Children.Add((object) msofbtClientAnchor);
    this.Children.Add((object) msofbtClientData);
    this.Shape.ShapeId = pictProps.Spid;
    this.Children.Add((object) new MsofbtTertiaryFOPT(this.m_doc));
    this.ShapePosition.XAlign = (uint) pictProps.HorizontalAlignment;
    this.ShapePosition.XRelTo = (uint) pictProps.RelHrzPos;
    this.ShapePosition.YAlign = (uint) pictProps.VerticalAlignment;
    this.ShapePosition.YRelTo = (uint) pictProps.RelVrtPos;
    return this;
  }

  public MsofbtSpContainer CreateInlineImageContainer(WPicture pict)
  {
    MsofbtSp msofbtSp = new MsofbtSp(this.m_doc);
    msofbtSp.ShapeType = EscherShapeType.msosptPictureFrame;
    msofbtSp.HasShapeTypeProperty = true;
    msofbtSp.HasAnchor = true;
    MsofbtOPT msofbtOpt = new MsofbtOPT(this.m_doc);
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(260, true, 1U));
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(262, false, 2U));
    if (!string.IsNullOrEmpty(pict.AlternativeText))
    {
      if (msofbtOpt.Properties.ContainsKey(897))
        msofbtOpt.Properties.Remove(897);
      byte[] bytes = Encoding.Unicode.GetBytes(pict.AlternativeText + "\0");
      msofbtOpt.Properties.Add((FOPTEBase) new FOPTEComplex(897, false, bytes.Length)
      {
        Value = bytes
      });
    }
    if (!string.IsNullOrEmpty(pict.Name))
    {
      if (msofbtOpt.Properties.ContainsKey(896))
        msofbtOpt.Properties.Remove(896);
      byte[] bytes = Encoding.Unicode.GetBytes(pict.Name + "\0");
      msofbtOpt.Properties.Add((FOPTEBase) new FOPTEComplex(896, false, bytes.Length)
      {
        Value = bytes
      });
    }
    if ((double) pict.FillRectangle.BottomOffset != 0.0)
      msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(257, false, this.SetPictureCropValue(pict.FillRectangle.BottomOffset)));
    if ((double) pict.FillRectangle.RightOffset != 0.0)
      msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(259, false, this.SetPictureCropValue(pict.FillRectangle.RightOffset)));
    if ((double) pict.FillRectangle.LeftOffset != 0.0)
      msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(258, false, this.SetPictureCropValue(pict.FillRectangle.LeftOffset)));
    if ((double) pict.FillRectangle.TopOffset != 0.0)
      msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(256 /*0x0100*/, false, this.SetPictureCropValue(pict.FillRectangle.TopOffset)));
    msofbtOpt.Header.Instance = msofbtOpt.Properties.Count;
    _Blip blip;
    if (pict.ImageRecord != null && pict.ImageRecord.IsMetafile)
    {
      blip = (_Blip) new MsofbtMetaFile(pict, this.m_doc);
    }
    else
    {
      bool isBitmap = this.IsBitmap(pict.ImageRecord.ImageFormat);
      blip = (_Blip) new MsofbtImage(pict.ImageRecord, isBitmap, this.m_doc);
    }
    if ((double) pict.Rotation != 0.0 && pict.TextWrappingStyle != TextWrappingStyle.Inline)
      msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(4, false, this.SetPictureRotationValue(pict.Rotation)));
    Guid uid = blip.Uid;
    MsofbtBSE msofbtBse = new MsofbtBSE(this.m_doc);
    msofbtBse.Header.Instance = (int) blip.Type;
    msofbtBse.Fbse.m_btWin32 = (int) blip.Type;
    msofbtBse.Fbse.m_btMacOS = (int) blip.Type;
    msofbtBse.Fbse.m_rgbUid = uid.ToByteArray();
    msofbtBse.Fbse.m_tag = (int) byte.MaxValue;
    msofbtBse.Fbse.m_cRef = 1;
    msofbtBse.IsInlineBlip = true;
    msofbtBse.Blip = blip;
    MsofbtClientAnchor msofbtClientAnchor = new MsofbtClientAnchor(this.m_doc);
    msofbtClientAnchor.Data = new byte[4]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 128 /*0x80*/
    };
    this.Children.Add((object) msofbtSp);
    this.Children.Add((object) msofbtOpt);
    this.Children.Add((object) msofbtClientAnchor);
    if (pict.PictureShape.ShapeContainer != null && pict.PictureShape.ShapeContainer.ShapePosition != null)
      this.Children.Add((object) pict.PictureShape.ShapeContainer.ShapePosition);
    this.Bse = msofbtBse;
    return this;
  }

  public MsofbtSpContainer CreateTextBoxContainer(bool visible, WTextBoxFormat txbxFormat)
  {
    this.Children.Add((object) new MsofbtSp(this.m_doc)
    {
      ShapeType = EscherShapeType.msosptTextBox,
      HasShapeTypeProperty = true,
      HasAnchor = true
    });
    MsofbtOPT msofbtOpt = new MsofbtOPT(this.m_doc);
    this.Children.Add((object) msofbtOpt);
    this.WriteTextBoxOptions(visible, txbxFormat);
    msofbtOpt.Header.Instance = msofbtOpt.Properties.Count;
    if (!string.IsNullOrEmpty(txbxFormat.Name))
    {
      if (msofbtOpt.Properties.ContainsKey(896))
        msofbtOpt.Properties.Remove(896);
      byte[] bytes = Encoding.Unicode.GetBytes(txbxFormat.Name + "\0");
      msofbtOpt.Properties.Add((FOPTEBase) new FOPTEComplex(896, false, bytes.Length)
      {
        Value = bytes
      });
    }
    MsofbtClientAnchor msofbtClientAnchor = new MsofbtClientAnchor(this.m_doc);
    msofbtClientAnchor.Data = new byte[4];
    MsofbtClientData msofbtClientData = new MsofbtClientData(this.m_doc);
    msofbtClientData.Data = new byte[4]
    {
      (byte) 1,
      (byte) 0,
      (byte) 0,
      (byte) 0
    };
    MsofbtClientTextbox msofbtClientTextbox = new MsofbtClientTextbox(this.m_doc);
    msofbtClientTextbox.Txid = (int) txbxFormat.TextBoxIdentificator;
    this.Children.Add((object) msofbtClientAnchor);
    this.Children.Add((object) msofbtClientData);
    this.Children.Add((object) msofbtClientTextbox);
    this.Children.Add((object) new MsofbtTertiaryFOPT(this.m_doc));
    this.ShapePosition.XAlign = (uint) txbxFormat.HorizontalAlignment;
    this.ShapePosition.YAlign = (uint) txbxFormat.VerticalAlignment;
    if (txbxFormat.TextWrappingStyle == TextWrappingStyle.Inline)
    {
      this.ShapePosition.XRelTo = 3U;
      this.ShapePosition.YRelTo = 3U;
      this.ShapePosition.Unknown1 = 6291456U /*0x600000*/;
      this.ShapePosition.Unknown2 = 65537U /*0x010001*/;
    }
    else
    {
      this.ShapePosition.XRelTo = txbxFormat.HorizontalOrigin == HorizontalOrigin.LeftMargin || txbxFormat.HorizontalOrigin == HorizontalOrigin.RightMargin || txbxFormat.HorizontalOrigin == HorizontalOrigin.InsideMargin || txbxFormat.HorizontalOrigin == HorizontalOrigin.OutsideMargin ? 0U : (uint) txbxFormat.HorizontalOrigin;
      this.ShapePosition.YRelTo = txbxFormat.VerticalOrigin == VerticalOrigin.TopMargin || txbxFormat.VerticalOrigin == VerticalOrigin.BottomMargin || txbxFormat.VerticalOrigin == VerticalOrigin.InsideMargin || txbxFormat.VerticalOrigin == VerticalOrigin.OutsideMargin ? 1U : (uint) txbxFormat.VerticalOrigin;
    }
    this.ShapePosition.AllowInTableCell = txbxFormat.AllowInCell;
    if (this.ShapePosition != null)
      this.ShapePosition.AllowOverlap = txbxFormat.AllowOverlap;
    else if (this.ShapeOptions != null)
      this.ShapeOptions.AllowOverlap = txbxFormat.AllowOverlap;
    this.Shape.ShapeId = txbxFormat.TextBoxShapeID;
    this.ShapeOptions.DistanceFromTop = (uint) ((double) txbxFormat.WrapDistanceTop * 12700.0);
    this.ShapeOptions.DistanceFromBottom = (uint) ((double) txbxFormat.WrapDistanceBottom * 12700.0);
    this.ShapeOptions.DistanceFromLeft = (uint) ((double) txbxFormat.WrapDistanceLeft * 12700.0);
    this.ShapeOptions.DistanceFromRight = (uint) ((double) txbxFormat.WrapDistanceRight * 12700.0);
    return this;
  }

  public bool IsMetafile(System.Drawing.Imaging.ImageFormat imageFormat)
  {
    return imageFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Emf) || imageFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Wmf);
  }

  private bool IsBitmap(System.Drawing.Imaging.ImageFormat imageFormat)
  {
    return imageFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Png) || imageFormat.Equals((object) System.Drawing.Imaging.ImageFormat.Bmp) || imageFormat.Equals((object) System.Drawing.Imaging.ImageFormat.MemoryBmp);
  }

  private bool IsBitmapForPartialTrustMode(Syncfusion.DocIO.DLS.Entities.ImageFormat imageFormat)
  {
    return imageFormat.Equals((object) Syncfusion.DocIO.DLS.Entities.ImageFormat.Png) || imageFormat.Equals((object) Syncfusion.DocIO.DLS.Entities.ImageFormat.Bmp) || imageFormat.Equals((object) Syncfusion.DocIO.DLS.Entities.ImageFormat.MemoryBmp);
  }

  public void WriteContainer(Stream stream)
  {
    this.WriteMsofbhWithRecord(stream);
    if (this.Bse == null)
      return;
    this.Bse.WriteMsofbhWithRecord(stream);
  }

  internal MsofbtSpContainer CreateInlineTxbxImageCont()
  {
    MsofbtSp msofbtSp = new MsofbtSp(this.m_doc);
    msofbtSp.ShapeType = EscherShapeType.msosptPictureFrame;
    msofbtSp.HasShapeTypeProperty = true;
    msofbtSp.HasAnchor = true;
    MsofbtOPT msofbtOpt = new MsofbtOPT(this.m_doc);
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid((int) sbyte.MaxValue, false, 20971840U));
    msofbtOpt.Header.Instance = msofbtOpt.Properties.Count;
    MsofbtClientAnchor msofbtClientAnchor = new MsofbtClientAnchor(this.m_doc);
    msofbtClientAnchor.Data = new byte[4]
    {
      (byte) 0,
      (byte) 0,
      (byte) 0,
      (byte) 128 /*0x80*/
    };
    MsofbtTertiaryFOPT msofbtTertiaryFopt = new MsofbtTertiaryFOPT(this.m_doc);
    msofbtTertiaryFopt.Unknown2 = 65537U /*0x010001*/;
    this.Children.Add((object) msofbtSp);
    this.Children.Add((object) msofbtOpt);
    this.Children.Add((object) msofbtTertiaryFopt);
    this.Children.Add((object) msofbtClientAnchor);
    return this;
  }

  internal void CheckOptContainer()
  {
    if (this.ShapeOptions != null)
      return;
    MsofbtOPT msofbtOpt = new MsofbtOPT(this.m_doc);
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(260, true, 1U));
    msofbtOpt.Properties.Add((FOPTEBase) new FOPTEBid(262, false, 2U));
    msofbtOpt.Header.Instance = msofbtOpt.Properties.Count;
    this.Children.Add((object) msofbtOpt);
  }

  public static MsofbtSpContainer ReadInlineImageContainers(
    int length,
    Stream stream,
    WordDocument doc)
  {
    ContainerCollection containerCollection = new ContainerCollection(doc);
    containerCollection.Read(stream, length);
    if (containerCollection[0] is MsofbtSpContainer msofbtSpContainer && containerCollection.Count > 1)
      msofbtSpContainer.Bse = containerCollection[1] as MsofbtBSE;
    return msofbtSpContainer;
  }

  public static _Blip GetBlipFromShapeContainer(BaseEscherRecord escherRecord)
  {
    if (escherRecord == null)
      throw new NullReferenceException("Container is null");
    if (!(escherRecord is MsofbtSpContainer msofbtSpContainer))
      throw new ArgumentException("Container is not a shape container.");
    return msofbtSpContainer.Shape.ShapeType != EscherShapeType.msosptPictureFrame || msofbtSpContainer.Bse == null ? (_Blip) null : msofbtSpContainer.Bse.Blip;
  }

  internal override BaseEscherRecord Clone()
  {
    MsofbtSpContainer msofbtSpContainer = (MsofbtSpContainer) base.Clone();
    if (this.m_bse != null)
      msofbtSpContainer.Bse = (MsofbtBSE) this.m_bse.Clone();
    msofbtSpContainer.m_doc = this.m_doc;
    return (BaseEscherRecord) msofbtSpContainer;
  }

  internal override void CloneRelationsTo(WordDocument doc)
  {
    this.m_doc = doc;
    this.Header.m_doc = doc;
    foreach (BaseEscherRecord child in (List<object>) this.Children)
    {
      child.m_doc = doc;
      child.Header.m_doc = doc;
    }
    if (this.Bse == null)
      return;
    this.Bse.m_doc = doc;
    this.Bse.Header.m_doc = doc;
    if (this.Bse.Blip == null)
      return;
    this.Bse.Blip.m_doc = doc;
    this.Bse.Blip.Header.m_doc = doc;
    Size size = this.Bse.Blip.ImageRecord.Size;
    System.Drawing.Imaging.ImageFormat imageFormat = this.Bse.Blip.ImageRecord.ImageFormat;
    if (this.Bse.Blip is MsofbtMetaFile)
      (this.Bse.Blip as MsofbtMetaFile).ImageRecord = doc.Images.LoadMetaFileImage(this.Bse.Blip.ImageRecord.m_imageBytes, true);
    else
      this.Bse.Blip.ImageRecord = doc.Images.LoadImage(this.Bse.Blip.ImageRecord.ImageBytes);
    this.Bse.Blip.ImageRecord.Size = size;
    this.Bse.Blip.ImageRecord.ImageFormat = imageFormat;
  }

  internal void RemoveSpContainerOle()
  {
    this.Shape.IsOle = false;
    if (this.ShapeOptions.Properties.ContainsKey(267))
      this.ShapeOptions.Properties.Remove(267);
    FOPTEBid property = (FOPTEBid) this.ShapeOptions.Properties[319];
    if (property == null)
      return;
    property.Value = (uint) BaseWordRecord.SetBitsByMask((int) property.Value, 1, 1, 0);
  }

  internal void WriteTextBoxOptions(bool visible, WTextBoxFormat txbxFormat)
  {
    msofbtRGFOPTE properties = this.ShapeOptions.Properties;
    if (txbxFormat.TextWrappingStyle == TextWrappingStyle.Inline)
      this.SetBoolShapeOption(properties, (int) sbyte.MaxValue, 64 /*0x40*/, 6, 0, 20971840U);
    if (txbxFormat.TextWrappingStyle == TextWrappingStyle.Through || txbxFormat.TextWrappingStyle == TextWrappingStyle.Tight)
    {
      PointF[] array = txbxFormat.WrapPolygon.Vertices.ToArray();
      byte[] destinationArray = new byte[6 + array.Length * 8];
      byte[] bytes1 = BitConverter.GetBytes((short) array.Length);
      byte[] sourceArray = bytes1;
      byte[] bytes2 = BitConverter.GetBytes((short) 8);
      int destinationIndex1 = 0;
      Array.Copy((Array) bytes1, 0, (Array) destinationArray, destinationIndex1, 2);
      int destinationIndex2 = destinationIndex1 + 2;
      Array.Copy((Array) sourceArray, 0, (Array) destinationArray, destinationIndex2, 2);
      int destinationIndex3 = destinationIndex2 + 2;
      Array.Copy((Array) bytes2, 0, (Array) destinationArray, destinationIndex3, 2);
      int destinationIndex4 = destinationIndex3 + 2;
      for (int index = 0; index < array.Length; ++index)
      {
        byte[] bytes3 = BitConverter.GetBytes((int) array[index].X);
        Array.Copy((Array) bytes3, 0, (Array) destinationArray, destinationIndex4, bytes3.Length);
        int destinationIndex5 = destinationIndex4 + bytes3.Length;
        byte[] bytes4 = BitConverter.GetBytes((int) array[index].Y);
        Array.Copy((Array) bytes4, 0, (Array) destinationArray, destinationIndex5, bytes4.Length);
        destinationIndex4 = destinationIndex5 + bytes4.Length;
      }
      if (properties.ContainsKey(899))
        properties.Remove(899);
      properties.Add((FOPTEBase) new FOPTEComplex(899, false, destinationArray.Length)
      {
        Value = destinationArray
      });
    }
    if (properties.ContainsKey(128 /*0x80*/))
      properties.Remove(128 /*0x80*/);
    properties.Add((FOPTEBase) new FOPTEBid(128 /*0x80*/, false, (uint) txbxFormat.TextBoxIdentificator));
    if (properties.ContainsKey(138))
    {
      properties.Remove(138);
      properties.Add((FOPTEBase) new FOPTEBid(138, false, (uint) txbxFormat.TextBoxShapeID));
    }
    float curValue1 = txbxFormat.LineWidth * 12700f;
    if ((double) txbxFormat.LineWidth != 0.75)
      this.SetShapeOption(properties, (uint) curValue1, 459, false);
    if (txbxFormat.LineDashing != LineDashing.Solid)
      this.SetShapeOption(properties, (uint) txbxFormat.LineDashing, 462, false);
    if (txbxFormat.LineStyle != TextBoxLineStyle.Simple)
      this.SetShapeOption(properties, (uint) txbxFormat.LineStyle, 461, false);
    if (txbxFormat.TextDirection != TextDirection.Horizontal)
    {
      if (properties.ContainsKey(136))
        properties.Remove(136);
      uint num;
      switch (txbxFormat.TextDirection)
      {
        case TextDirection.VerticalFarEast:
          num = 1U;
          break;
        case TextDirection.VerticalBottomToTop:
          num = 2U;
          break;
        case TextDirection.VerticalTopToBottom:
          num = 3U;
          break;
        case TextDirection.HorizontalFarEast:
          num = 4U;
          break;
        case TextDirection.Vertical:
          num = 5U;
          break;
        default:
          num = 0U;
          break;
      }
      properties.Add((FOPTEBase) new FOPTEBid(136, false, num));
    }
    if ((int) WordColor.ConvertColorToRGB(txbxFormat.LineColor) != (int) WordColor.ConvertColorToRGB(Color.Black))
    {
      uint rgb = WordColor.ConvertColorToRGB(txbxFormat.LineColor, true);
      this.SetShapeOption(properties, rgb, 448, false);
      uint opacity = this.GetOpacity(txbxFormat.LineColor.A);
      if (opacity != uint.MaxValue)
        this.SetShapeOption(properties, opacity, 449, false);
    }
    if (txbxFormat.NoLine)
      this.SetBoolShapeOption(properties, 511 /*0x01FF*/, 8, 3, 0, 524288U /*0x080000*/);
    if (txbxFormat.AutoFit)
      this.SetBoolShapeOption(properties, 191, 2, 1, 1, 131074U /*0x020002*/);
    if (!txbxFormat.AllowInCell)
      this.SetBoolShapeOption(properties, 959, int.MaxValue, 31 /*0x1F*/, 1, 2147483648U /*0x80000000*/);
    if (this.ShapePosition != null)
      this.ShapePosition.AllowOverlap = txbxFormat.AllowOverlap;
    else if (this.ShapeOptions != null)
      this.ShapeOptions.AllowOverlap = txbxFormat.AllowOverlap;
    if (txbxFormat.TextWrappingStyle == TextWrappingStyle.Inline)
      this.SetBoolShapeOption(properties, 959, 1, 0, 0, 2097152U /*0x200000*/);
    else if (txbxFormat.IsBelowText)
      this.SetBoolShapeOption(properties, 959, 32 /*0x20*/, 5, 1, 2097184U /*0x200020*/);
    if ((double) txbxFormat.InternalMargin.Left != 7.0869998931884766)
    {
      uint curValue2 = (uint) Math.Round((double) txbxFormat.InternalMargin.Left * 12700.0);
      this.SetShapeOption(properties, curValue2, 129, false);
    }
    if ((double) txbxFormat.InternalMargin.Right != 7.0869998931884766)
    {
      uint curValue3 = (uint) Math.Round((double) txbxFormat.InternalMargin.Right * 12700.0);
      this.SetShapeOption(properties, curValue3, 131, false);
    }
    if ((double) txbxFormat.InternalMargin.Top != 3.684999942779541)
    {
      uint curValue4 = (uint) Math.Round((double) txbxFormat.InternalMargin.Top * 12700.0);
      this.SetShapeOption(properties, curValue4, 130, false);
    }
    if ((double) txbxFormat.InternalMargin.Bottom != 3.684999942779541)
    {
      uint curValue5 = (uint) Math.Round((double) txbxFormat.InternalMargin.Bottom * 12700.0);
      this.SetShapeOption(properties, curValue5, 132, false);
    }
    if (txbxFormat.Document != null)
      this.UpdateBackground(txbxFormat.Document, txbxFormat.FillEfects);
    this.ShapeOptions.Visible = visible;
  }

  internal void WritePictureOptions(PictureShapeProps pictProps, WPicture pic)
  {
    msofbtRGFOPTE properties = this.ShapeOptions.Properties;
    if (pic.TextWrappingStyle != TextWrappingStyle.Inline)
    {
      this.ShapeOptions.DistanceFromBottom = (uint) ((double) pic.DistanceFromBottom * 12700.0);
      this.ShapeOptions.DistanceFromLeft = (uint) ((double) pic.DistanceFromLeft * 12700.0);
      this.ShapeOptions.DistanceFromRight = (uint) ((double) pic.DistanceFromRight * 12700.0);
      this.ShapeOptions.DistanceFromTop = (uint) ((double) pic.DistanceFromTop * 12700.0);
    }
    if ((double) pic.Rotation != 0.0 && pic.TextWrappingStyle != TextWrappingStyle.Inline)
      this.ShapeOptions.Roation = (uint) ((double) pic.Rotation * 65536.0);
    if ((double) pic.FillRectangle.BottomOffset != 0.0)
      this.SetShapeOption(properties, this.SetPictureCropValue(pic.FillRectangle.BottomOffset), 257, false);
    if ((double) pic.FillRectangle.RightOffset != 0.0)
      this.SetShapeOption(properties, this.SetPictureCropValue(pic.FillRectangle.RightOffset), 259, false);
    if ((double) pic.FillRectangle.LeftOffset != 0.0)
      this.SetShapeOption(properties, this.SetPictureCropValue(pic.FillRectangle.LeftOffset), 258, false);
    if ((double) pic.FillRectangle.TopOffset != 0.0)
      this.SetShapeOption(properties, this.SetPictureCropValue(pic.FillRectangle.TopOffset), 256 /*0x0100*/, false);
    if (properties.ContainsKey(263))
      this.ShapeOptions.SetPropertyValue(263, WordColor.ConvertColorToRGB(pic.ChromaKeyColor));
    if (pic.TextWrappingStyle == TextWrappingStyle.Through || pic.TextWrappingStyle == TextWrappingStyle.Tight)
    {
      PointF[] array = pic.WrapPolygon.Vertices.ToArray();
      byte[] destinationArray = new byte[6 + array.Length * 8];
      byte[] bytes1 = BitConverter.GetBytes((short) array.Length);
      byte[] sourceArray = bytes1;
      byte[] bytes2 = BitConverter.GetBytes((short) 8);
      int destinationIndex1 = 0;
      Array.Copy((Array) bytes1, 0, (Array) destinationArray, destinationIndex1, 2);
      int destinationIndex2 = destinationIndex1 + 2;
      Array.Copy((Array) sourceArray, 0, (Array) destinationArray, destinationIndex2, 2);
      int destinationIndex3 = destinationIndex2 + 2;
      Array.Copy((Array) bytes2, 0, (Array) destinationArray, destinationIndex3, 2);
      int destinationIndex4 = destinationIndex3 + 2;
      for (int index = 0; index < array.Length; ++index)
      {
        byte[] bytes3 = BitConverter.GetBytes((int) array[index].X);
        Array.Copy((Array) bytes3, 0, (Array) destinationArray, destinationIndex4, bytes3.Length);
        int destinationIndex5 = destinationIndex4 + bytes3.Length;
        byte[] bytes4 = BitConverter.GetBytes((int) array[index].Y);
        Array.Copy((Array) bytes4, 0, (Array) destinationArray, destinationIndex5, bytes4.Length);
        destinationIndex4 = destinationIndex5 + bytes4.Length;
      }
      if (properties.ContainsKey(899))
        properties.Remove(899);
      properties.Add((FOPTEBase) new FOPTEComplex(899, false, destinationArray.Length)
      {
        Value = destinationArray
      });
    }
    if (!properties.ContainsKey(260))
      this.SetShapeOption(properties, 1U, 260, true);
    if (!properties.ContainsKey(262))
      this.SetShapeOption(properties, 2U, 262, false);
    if (!string.IsNullOrEmpty(pictProps.AlternativeText))
    {
      if (properties.ContainsKey(897))
        properties.Remove(897);
      byte[] bytes = Encoding.Unicode.GetBytes(pictProps.AlternativeText + "\0");
      properties.Add((FOPTEBase) new FOPTEComplex(897, false, bytes.Length)
      {
        Value = bytes
      });
    }
    if (!string.IsNullOrEmpty(pictProps.Name))
    {
      if (properties.ContainsKey(896))
        properties.Remove(896);
      byte[] bytes = Encoding.Unicode.GetBytes(pictProps.Name + "\0");
      properties.Add((FOPTEBase) new FOPTEComplex(896, false, bytes.Length)
      {
        Value = bytes
      });
    }
    if (pictProps.IsBelowText)
      this.SetBoolShapeOption(properties, 959, 32 /*0x20*/, 5, 1, 2097184U /*0x200020*/);
    this.ShapeOptions.Visible = pic.Visible;
  }

  private uint SetPictureCropValue(float offset) => (uint) ((double) offset / 1.5259 * 1000.0);

  private uint SetPictureRotationValue(float rotation) => (uint) ((double) rotation * 65536.0);

  private void SetShapeOption(msofbtRGFOPTE shapeProps, uint curValue, int fopteKey, bool isBid)
  {
    if (shapeProps.ContainsKey(fopteKey))
    {
      FOPTEBid shapeProp = (FOPTEBid) shapeProps[fopteKey];
      if ((int) shapeProp.Value == (int) curValue)
        return;
      shapeProp.Value = curValue;
    }
    else
      shapeProps.Add((FOPTEBase) new FOPTEBid(fopteKey, isBid, curValue));
  }

  private void SetBoolShapeOption(
    msofbtRGFOPTE shapeProps,
    int fopteKey,
    int bitMask,
    int startBit,
    int value,
    uint defValue)
  {
    if (shapeProps.ContainsKey(fopteKey))
    {
      FOPTEBid shapeProp = (FOPTEBid) shapeProps[fopteKey];
      if ((int) BaseWordRecord.GetBitsByMask(shapeProp.Value, bitMask, startBit) == value)
        return;
      shapeProp.Value = (uint) BaseWordRecord.SetBitsByMask((int) shapeProp.Value, bitMask, startBit, value);
    }
    else
      shapeProps.Add((FOPTEBase) new FOPTEBid(fopteKey, false, defValue));
  }

  private uint GetOpacity(byte opacity)
  {
    float num = (float) (100.0 - (double) opacity / 2.55);
    if ((double) num == 0.0)
      return uint.MaxValue;
    return (double) num != 0.0 ? (uint) Math.Round((double) (100f - num) * 655.35) : 0U;
  }

  private void CheckEscher(WordDocument doc)
  {
    if (doc == null)
      return;
    EscherClass escher = doc.Escher;
    if (escher != null && escher.m_dgContainers.Count == 0 || escher == null)
    {
      EscherClass escherClass = new EscherClass(doc);
      escherClass.CreateDgForSubDocuments();
      doc.Escher = escherClass;
    }
    else
    {
      if (escher.m_msofbtDggContainer.BstoreContainer != null)
        return;
      escher.m_msofbtDggContainer.Children.Add((object) new MsofbtBstoreContainer(this.m_doc));
    }
  }

  private GradientShadingVariant GetCornerStyleVariant()
  {
    uint propertyValue1 = this.GetPropertyValue(397);
    uint propertyValue2 = this.GetPropertyValue(400);
    if (propertyValue1 == uint.MaxValue && propertyValue2 == uint.MaxValue)
      return GradientShadingVariant.ShadingUp;
    if (propertyValue1 != uint.MaxValue && propertyValue2 != uint.MaxValue)
      return GradientShadingVariant.ShadingMiddle;
    return propertyValue1 != uint.MaxValue ? GradientShadingVariant.ShadingDown : GradientShadingVariant.ShadingOut;
  }

  private void AddGradientFillAngle(GradientShadingStyle shadingStyle)
  {
    if (shadingStyle == GradientShadingStyle.Horizontal)
      return;
    uint curValue;
    switch (shadingStyle - 1)
    {
      case GradientShadingStyle.Horizontal:
        curValue = 4289069056U;
        break;
      case GradientShadingStyle.Vertical:
        curValue = 4286119936U;
        break;
      default:
        curValue = 4292018176U;
        break;
    }
    this.SetShapeOption(this.ShapeOptions.Properties, curValue, 395, false);
  }

  private void AddGradientFocusFopte(
    GradientShadingStyle shadingStyle,
    GradientShadingVariant shadingVariant)
  {
    uint curValue = 0;
    if (shadingStyle != GradientShadingStyle.FromCorner)
    {
      switch (shadingVariant)
      {
        case GradientShadingVariant.ShadingUp:
          curValue = 100U;
          break;
        case GradientShadingVariant.ShadingOut:
          curValue = 4294967246U;
          break;
        case GradientShadingVariant.ShadingMiddle:
          curValue = 50U;
          break;
      }
    }
    else
      curValue = 100U;
    if (curValue == 0U)
      return;
    this.SetShapeOption(this.ShapeOptions.Properties, curValue, 396, false);
  }

  private void AddGradientFillType(GradientShadingStyle shadingStyle)
  {
    uint curValue;
    switch (shadingStyle)
    {
      case GradientShadingStyle.FromCorner:
        curValue = 5U;
        break;
      case GradientShadingStyle.FromCenter:
        curValue = 6U;
        break;
      default:
        curValue = 7U;
        break;
    }
    this.SetShapeOption(this.ShapeOptions.Properties, curValue, 384, false);
  }

  private void AddFillProperties(
    GradientShadingStyle shadingStyle,
    GradientShadingVariant shadingVariant)
  {
    uint curValue1 = 0;
    uint curValue2 = 0;
    uint curValue3 = 0;
    uint curValue4 = 0;
    if (shadingStyle == GradientShadingStyle.FromCorner)
    {
      if (shadingVariant == GradientShadingVariant.ShadingDown)
        curValue1 = curValue2 = 65536U /*0x010000*/;
      if (shadingVariant == GradientShadingVariant.ShadingOut)
        curValue3 = curValue4 = 65536U /*0x010000*/;
      if (shadingVariant == GradientShadingVariant.ShadingMiddle)
      {
        int num;
        curValue2 = (uint) (num = 65536 /*0x010000*/);
        curValue1 = (uint) num;
        curValue4 = (uint) num;
        curValue3 = (uint) num;
      }
    }
    else
    {
      int num;
      curValue2 = (uint) (num = 32768 /*0x8000*/);
      curValue1 = (uint) num;
      curValue4 = (uint) num;
      curValue3 = (uint) num;
    }
    if (curValue1 != 0U)
      this.SetShapeOption(this.ShapeOptions.Properties, curValue1, 397, false);
    if (curValue2 != 0U)
      this.SetShapeOption(this.ShapeOptions.Properties, curValue2, 399, false);
    if (curValue3 != 0U)
      this.SetShapeOption(this.ShapeOptions.Properties, curValue3, 398, false);
    if (curValue4 == 0U)
      return;
    this.SetShapeOption(this.ShapeOptions.Properties, curValue4, 400, false);
  }
}
