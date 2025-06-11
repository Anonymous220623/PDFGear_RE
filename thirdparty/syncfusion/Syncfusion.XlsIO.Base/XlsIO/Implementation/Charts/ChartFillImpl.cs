// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.Charts.ChartFillImpl
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Shapes;
using Syncfusion.XlsIO.Parser;
using Syncfusion.XlsIO.Parser.Biff_Records;
using Syncfusion.XlsIO.Parser.Biff_Records.Charts;
using Syncfusion.XlsIO.Parser.Biff_Records.MsoDrawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.Charts;

public class ChartFillImpl : ShapeFillImpl
{
  private ChartGelFrameRecord m_gel;
  private bool m_invertIfNegative;

  public ChartFillImpl(IApplication application, object parent)
    : base(application, parent)
  {
    this.m_gel = (ChartGelFrameRecord) BiffRecordFactory.GetRecord(TBIFFRecord.ChartGelFrame);
    this.m_bIsShapeFill = false;
  }

  [CLSCompliant(false)]
  public ChartFillImpl(IApplication application, object parent, ChartGelFrameRecord gel)
    : base(application, parent)
  {
    this.m_gel = gel != null ? gel : throw new ArgumentNullException(nameof (gel));
    this.m_bIsShapeFill = false;
    this.Parse();
  }

  private void Parse()
  {
    IList optionList = (IList) this.m_gel.OptionList;
    int index = 0;
    for (int count = optionList.Count; index < count; ++index)
      this.ParseOption(optionList[index] as MsofbtOPT.FOPTE);
    if (this.ParsePictureData == null)
      return;
    this.ParsePictureOrUserDefinedTexture(this.m_fillType == ExcelFillType.Picture);
  }

  [CLSCompliant(false)]
  public void Serialize(IList<IBiffStorage> records)
  {
    if ((this.Parent as IFillColor).IsAutomaticFormat || !this.Visible)
      return;
    this.m_gel.UpdateToSerialize();
    this.Serialize((IFopteOptionWrapper) new FopteOptionWrapper(this.m_gel.OptionList));
    List<BiffRecordRaw> addInStream = ((ChartGelFrameRecord) this.m_gel.Clone()).UpdatesToAddInStream();
    int index = 0;
    for (int count = addInStream.Count; index < count; ++index)
      records.Add((IBiffStorage) addInStream[index]);
  }

  public override double TransparencyFrom
  {
    get => throw new NotSupportedException("This property doesnt support in chart fill format.");
    set => throw new NotSupportedException("This property doesnt support in chart fill format.");
  }

  public override double TransparencyTo
  {
    get => throw new NotSupportedException("This property doesnt support in chart fill format.");
    set => throw new NotSupportedException("This property doesnt support in chart fill format.");
  }

  public override ColorObject ForeColorObject => (this.Parent as IFillColor).ForeGroundColorObject;

  public override ColorObject BackColorObject => (this.Parent as IFillColor).BackGroundColorObject;

  public override bool Visible
  {
    get => (this.Parent as IFillColor).Pattern != ExcelPattern.None;
    set
    {
      IFillColor parent = this.Parent as IFillColor;
      parent.IsAutomaticFormat = false;
      if (value)
      {
        if (parent.Pattern != ExcelPattern.None)
          return;
        parent.Pattern = ExcelPattern.Solid;
      }
      else
        parent.Pattern = ExcelPattern.None;
    }
  }

  public bool InvertIfNegative
  {
    get => this.m_invertIfNegative;
    set => this.m_invertIfNegative = value;
  }

  [CLSCompliant(false)]
  protected override IFopteOptionWrapper SetPicture(IFopteOptionWrapper opt)
  {
    if (opt == null)
      throw new ArgumentNullException(nameof (opt));
    MemoryStream inputStream = new MemoryStream();
    this.m_picture.Save((Stream) inputStream, this.m_picture.RawFormat);
    byte[] buffer = inputStream.GetBuffer();
    byte[] addData = new byte[buffer.Length + 25];
    byte[] numArray = new byte[4]
    {
      (byte) 160 /*0xA0*/,
      (byte) 70,
      (byte) 29,
      (byte) 240 /*0xF0*/
    };
    try
    {
      new MD5CryptoServiceProvider().ComputeHash((Stream) inputStream).CopyTo((Array) addData, 8);
    }
    catch (InvalidOperationException ex)
    {
      new MACTripleDES().ComputeHash((Stream) inputStream).CopyTo((Array) addData, 8);
    }
    addData[24] = byte.MaxValue;
    buffer.CopyTo((Array) addData, 25);
    BitConverter.GetBytes(buffer.Length + 17).CopyTo((Array) addData, 4);
    numArray.CopyTo((Array) addData, 0);
    ShapeImpl.SerializeForte(opt, MsoOptions.PatternTexture, 0, addData, true);
    return opt;
  }

  protected override int SetPictureToBse(Image im, string strName) => 0;

  [CLSCompliant(false)]
  protected override IFopteOptionWrapper SerializeTransparency(IFopteOptionWrapper opt) => opt;

  internal override void ChangeVisible()
  {
    if ((this.Parent as IFillColor).IsAutomaticFormat || this.Parent is ChartFrameFormatImpl)
      this.Visible = true;
    if (!(this.Parent is ChartWallOrFloorImpl))
      return;
    (this.Parent as ChartWallOrFloorImpl).HasShapeProperties = true;
  }
}
