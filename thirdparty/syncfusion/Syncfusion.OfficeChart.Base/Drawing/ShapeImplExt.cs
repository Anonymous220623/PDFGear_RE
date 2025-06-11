// Decompiled with JetBrains decompiler
// Type: Syncfusion.Drawing.ShapeImplExt
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using Syncfusion.OfficeChart;
using Syncfusion.OfficeChart.Implementation;
using Syncfusion.OfficeChart.Implementation.Shapes;
using Syncfusion.OfficeChart.Implementation.XmlSerialization;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.Drawing;

internal class ShapeImplExt
{
  private string m_textlink;
  private bool m_fLocksText;
  private bool m_fPublished;
  private ClientAnchor m_clientAnchor;
  private ExcelAutoShapeType m_shapeType;
  private AutoShapeType m_autoShapeType;
  private ShapeDrawingType m_shapeDrawingType;
  private int m_shapeID;
  private TextFrame m_textframe;
  private Dictionary<string, Stream> m_preservedElements;
  private string m_decription;
  private string m_name;
  private bool m_isHidden;
  private bool m_flipVertical;
  private bool m_flipHorizontal;
  private string m_title;
  private WorksheetImpl m_worksheet;
  private AnchorType m_anchorType;
  private string m_macro;
  private string m_text;
  private bool m_lockText;
  private bool m_published;
  private bool m_isCreated;
  private double m_shapeRotation;
  private ShapeFillImpl m_fill;
  private ShapeLineFormatImpl m_line;
  private RelationCollection m_relations;
  private PreservationLogger m_logger;

  public ShapeImplExt(AutoShapeType autoShapeType, WorksheetImpl worksheetImpl)
  {
    this.m_worksheet = worksheetImpl;
    this.m_autoShapeType = autoShapeType;
    this.m_anchorType = AnchorType.TwoCell;
    this.m_logger = new PreservationLogger();
    this.CreateShapeType(autoShapeType);
  }

  private void CreateShapeType(AutoShapeType autoShapeType)
  {
    switch (autoShapeType)
    {
      case AutoShapeType.Line:
      case AutoShapeType.StraightConnector:
      case AutoShapeType.ElbowConnector:
      case AutoShapeType.CurvedConnector:
      case AutoShapeType.BentConnector2:
      case AutoShapeType.BentConnector4:
      case AutoShapeType.BentConnector5:
      case AutoShapeType.CurvedConnector2:
      case AutoShapeType.CurvedConnector4:
      case AutoShapeType.CurvedConnector5:
        this.m_shapeType = ExcelAutoShapeType.cxnSp;
        break;
      default:
        this.m_shapeType = ExcelAutoShapeType.sp;
        break;
    }
  }

  internal double Rotation
  {
    get => this.m_shapeRotation;
    set => this.m_shapeRotation = value;
  }

  internal WorksheetImpl Worksheet
  {
    get => this.m_worksheet;
    set => this.m_worksheet = value;
  }

  internal string Description
  {
    get => this.m_decription;
    set => this.m_decription = value;
  }

  internal Dictionary<string, Stream> PreservedElements
  {
    get
    {
      if (this.m_preservedElements == null)
        this.m_preservedElements = new Dictionary<string, Stream>();
      return this.m_preservedElements;
    }
  }

  internal string Title
  {
    get => this.m_title;
    set => this.m_title = value;
  }

  internal bool IsCreated
  {
    get => this.m_isCreated;
    set => this.m_isCreated = value;
  }

  public TextFrame TextFrame
  {
    get
    {
      if (this.m_textframe == null)
        this.m_textframe = new TextFrame(this);
      return this.m_textframe;
    }
  }

  public ClientAnchor ClientAnchor
  {
    get
    {
      if (this.m_clientAnchor == null)
        this.m_clientAnchor = new ClientAnchor(this.m_worksheet);
      return this.m_clientAnchor;
    }
  }

  public ShapeFillImpl Fill
  {
    get
    {
      if (this.m_fill == null)
        this.m_fill = new ShapeFillImpl((IApplication) this.m_worksheet.AppImplementation, (object) this.m_worksheet, OfficeFillType.SolidColor);
      return this.m_fill;
    }
  }

  public ShapeLineFormatImpl Line
  {
    get
    {
      if (this.m_line == null)
        this.m_line = new ShapeLineFormatImpl((IApplication) this.m_worksheet.AppImplementation, (object) this.m_worksheet, this.m_logger);
      return this.m_line;
    }
  }

  public int ShapeID
  {
    get => this.m_shapeID;
    set => this.m_shapeID = value;
  }

  public ExcelAutoShapeType ShapeType
  {
    get => this.m_shapeType;
    set => this.m_shapeType = value;
  }

  public AutoShapeType AutoShapeType => this.m_autoShapeType;

  public string Macro
  {
    get => this.m_macro;
    set => this.m_macro = value;
  }

  public string TextLink
  {
    get => this.m_textlink;
    set => this.m_textlink = value;
  }

  public bool LocksText
  {
    get => this.m_lockText;
    set => this.m_lockText = value;
  }

  public bool Published
  {
    get => this.m_published;
    set => this.m_published = value;
  }

  public AnchorType AnchorType
  {
    get => this.m_anchorType;
    set => this.m_anchorType = value;
  }

  public string Name
  {
    get => this.m_name;
    set => this.m_name = value;
  }

  public bool IsHidden
  {
    get => this.m_isHidden;
    set => this.m_isHidden = value;
  }

  public RelationCollection Relations
  {
    get
    {
      if (this.m_relations == null)
        this.m_relations = new RelationCollection();
      return this.m_relations;
    }
  }

  public PreservationLogger Logger => this.m_logger;

  public bool FlipVertical
  {
    get => this.m_flipVertical;
    set => this.m_flipVertical = value;
  }

  public bool FlipHorizontal
  {
    get => this.m_flipHorizontal;
    set => this.m_flipHorizontal = value;
  }

  internal ShapeImplExt Clone(ShapeImpl parent)
  {
    ShapeImplExt parent1 = (ShapeImplExt) this.MemberwiseClone();
    this.m_worksheet = parent.ParentShapes.Worksheet;
    if (this.m_fill != null)
      parent1.m_fill = this.m_fill.Clone((object) parent);
    if (this.m_line != null)
      parent1.m_line = this.m_line.Clone((object) parent);
    if (this.m_relations != null)
      parent1.m_relations = this.m_relations.Clone();
    if (this.m_textframe != null)
      parent1.m_textframe = this.m_textframe.Clone((object) parent1);
    return parent1;
  }
}
