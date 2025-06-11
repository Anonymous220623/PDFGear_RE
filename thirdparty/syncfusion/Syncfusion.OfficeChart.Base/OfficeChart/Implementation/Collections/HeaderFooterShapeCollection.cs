// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.Collections.HeaderFooterShapeCollection
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Parser.Biff_Records;
using Syncfusion.OfficeChart.Parser.Biff_Records.MsoDrawing;
using Syncfusion.OfficeChart.Parser.Biff_Records.ObjRecords;
using System;
using System.Drawing;
using System.IO;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation.Collections;

internal class HeaderFooterShapeCollection : ShapeCollectionBase
{
  public HeaderFooterShapeCollection(IApplication application, object parent)
    : base(application, parent)
  {
  }

  [CLSCompliant(false)]
  public HeaderFooterShapeCollection(
    IApplication application,
    object parent,
    MsofbtSpgrContainer container,
    OfficeParseOptions options)
    : base(application, parent, container, options)
  {
  }

  public override TBIFFRecord RecordCode => TBIFFRecord.HeaderFooterImage;

  [CLSCompliant(false)]
  protected override ShapeImpl CreateShape(
    TObjType objType,
    MsofbtSpContainer shapeContainer,
    OfficeParseOptions options,
    System.Collections.Generic.List<ObjSubRecord> subRecords,
    int cmoIndex)
  {
    ShapeImpl shape = (ShapeImpl) null;
    if (objType == TObjType.otPicture)
      shape = (ShapeImpl) new BitmapShapeImpl(this.Application, (object) this, shapeContainer);
    return shape;
  }

  [CLSCompliant(false)]
  protected override void CreateData(
    Stream stream,
    MsofbtDgContainer dgContainer,
    System.Collections.Generic.List<int> arrBreaks,
    System.Collections.Generic.List<System.Collections.Generic.List<BiffRecordRaw>> arrRecords)
  {
    stream.Write(HeaderFooterImageRecord.DEF_WORKSHEET_RECORD_START, 0, HeaderFooterImageRecord.DEF_WORKSHEET_RECORD_START.Length);
    base.CreateData(stream, dgContainer, arrBreaks, arrRecords);
  }

  [CLSCompliant(false)]
  protected override ShapeImpl AddShape(
    MsofbtSpContainer shapeContainer,
    OfficeParseOptions options)
  {
    System.Collections.Generic.List<MsoBase> itemsList = shapeContainer.ItemsList;
    return this.AddShape(this.CreateShape(TObjType.otPicture, shapeContainer, options, (System.Collections.Generic.List<ObjSubRecord>) null, -1));
  }

  public override WorkbookShapeDataImpl ShapeData => this.Workbook.HeaderFooterData;

  protected override void RegisterInWorksheet()
  {
    this.WorksheetBase.InnerHeaderFooterShapes = this;
  }

  [CLSCompliant(false)]
  public void Parse(HeaderFooterImageRecord record, OfficeParseOptions options)
  {
    this.ParseMsoStructures(record.StructuresList, options);
  }

  public ShapeImpl SetPicture(string strShapeName, Image image)
  {
    return this.SetPicture(strShapeName, image, -1);
  }

  public ShapeImpl SetPicture(string strShapeName, Image image, int iIndex)
  {
    return this.SetPicture(strShapeName, image, iIndex, true, (string) null);
  }

  public ShapeImpl SetPicture(
    string strShapeName,
    Image image,
    int iIndex,
    bool bIncludeOptions,
    string preservedStyles)
  {
    switch (strShapeName)
    {
      case null:
        throw new ArgumentNullException(nameof (strShapeName));
      case "":
        throw new ArgumentException("strShapeName - string cannot be empty.");
      default:
        ShapeImpl shapeImpl = (ShapeImpl) null;
        BitmapShapeImpl newShape = this[strShapeName] as BitmapShapeImpl;
        bool flag = newShape != null;
        WorkbookShapeDataImpl shapeData = this.ShapeData;
        if (newShape != null)
        {
          uint blipId = newShape.BlipId;
          shapeData.RemovePicture(blipId, true);
        }
        if (image != null)
        {
          if (!flag)
            newShape = new BitmapShapeImpl(this.Application, (object) this, bIncludeOptions);
          int num = iIndex != -1 ? iIndex : shapeData.AddPicture(image, ExcelImageFormat.Original, strShapeName);
          newShape.BlipId = (uint) num;
          newShape.SetName(strShapeName);
          newShape.IsShortVersion = true;
          double pixels = ApplicationImpl.ConvertToPixels(1.0, MeasureUnits.Inch);
          newShape.ClientAnchor.TopRow = (int) Math.Round((double) image.Height * pixels / (double) image.VerticalResolution);
          newShape.ClientAnchor.LeftColumn = (int) Math.Round((double) image.Width * pixels / (double) image.HorizontalResolution);
          newShape.VmlShape = true;
          if (preservedStyles != null && preservedStyles.Length > 0)
            newShape.PreserveStyleString = preservedStyles;
          shapeImpl = this.AddShape((ShapeImpl) newShape);
        }
        else if (flag)
          this.Remove((IShape) newShape);
        return shapeImpl;
    }
  }
}
