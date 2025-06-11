// Decompiled with JetBrains decompiler
// Type: PDFKit.Utils.WatermarkUtils.WatermarkParam
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using Patagames.Pdf.Enums;

#nullable disable
namespace PDFKit.Utils.WatermarkUtils;

public class WatermarkParam
{
  public float Opacity { get; set; }

  public bool ShowOnPrint { get; set; }

  public bool ShowOnDisplay { get; set; }

  public float Vdistance { get; set; }

  public float Hdistance { get; set; }

  public float Rotation { get; set; }

  public PdfContentAlignment Alignment { get; set; }

  public float Scale { get; set; }

  public bool IsTile { get; set; }

  public int[] PageRange { get; set; }
}
