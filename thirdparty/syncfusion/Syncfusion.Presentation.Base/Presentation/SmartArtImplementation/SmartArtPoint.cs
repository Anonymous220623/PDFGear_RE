// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArtPoint
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.Drawing;
using Syncfusion.Presentation.RichText;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArtPoint
{
  private DataModel _dataModel;
  private Guid _modelId;
  private SmartArtPointType _type;
  private Guid _connectionId;
  private bool _isPlaceholder;
  private TextBody _textBody;
  private SmartArtShape _pointShape;
  private string _placeholderText;
  private Guid _presentationElementId;
  private string _presentationName;
  private bool _hasPropertySet;
  private bool _hasShapeProperties;
  private int _customAngle;
  private int _customScaleX;
  private int _customScaleY;
  private int _factorNeighbourX;
  private int _factorNeighbourY;
  private Guid _parentTransitionId;
  private Guid _siblingTransitionId;
  private Guid _parentSiblingConnectionId;
  private bool? _isTextChanged;
  private Dictionary<string, string> _customAttributes;
  private Syncfusion.Presentation.RichText.Hyperlink _hyperlink;
  private int _smartArtId;
  private string _smartArtName;

  internal SmartArtPoint(DataModel dataModel)
  {
    this._dataModel = dataModel;
    this._textBody = new TextBody((Shape) this._dataModel.ParentSmartArt);
  }

  internal SmartArtShape PointShapeProperties
  {
    get
    {
      if (this._pointShape == null)
        this._pointShape = new SmartArtShape(this._dataModel.ParentSmartArt, ShapeType.Point);
      this._pointShape.DrawingType = DrawingType.SmartArt;
      return this._pointShape;
    }
  }

  internal TextBody TextBody
  {
    get => this._textBody;
    set => this._textBody = value;
  }

  internal Guid ModelId
  {
    get => this._modelId;
    set => this._modelId = value;
  }

  internal Guid ParentTransitionId
  {
    get => this._parentTransitionId;
    set => this._parentTransitionId = value;
  }

  internal Guid SiblingTransitionId
  {
    get => this._siblingTransitionId;
    set => this._siblingTransitionId = value;
  }

  internal DataModel DataModel => this._dataModel;

  internal SmartArtPointType Type
  {
    get => this._type;
    set => this._type = value;
  }

  internal Guid ConnectionId
  {
    get => this._connectionId;
    set => this._connectionId = value;
  }

  internal Guid ParentSiblingConnectionId
  {
    get => this._parentSiblingConnectionId;
    set => this._parentSiblingConnectionId = value;
  }

  internal bool IsPlaceholder
  {
    get => this._isPlaceholder;
    set => this._isPlaceholder = value;
  }

  internal string PlaceholderText
  {
    get => this._placeholderText;
    set => this._placeholderText = value;
  }

  internal Guid PresentationElementId
  {
    get => this._presentationElementId;
    set => this._presentationElementId = value;
  }

  internal string PresentationName
  {
    get => this._presentationName;
    set => this._presentationName = value;
  }

  internal bool HasPropertySet
  {
    get => this._hasPropertySet;
    set => this._hasPropertySet = value;
  }

  internal bool HasShapeProperties
  {
    get => this._hasShapeProperties;
    set => this._hasShapeProperties = value;
  }

  internal int CustomAngle
  {
    get => this._customAngle;
    set => this._customAngle = value;
  }

  internal Dictionary<string, string> CustomAttributes
  {
    get => this._customAttributes ?? (this._customAttributes = new Dictionary<string, string>());
  }

  internal int CustomScaleX
  {
    get => this._customScaleX;
    set => this._customScaleX = value;
  }

  internal int CustomScaleY
  {
    get => this._customScaleY;
    set => this._customScaleY = value;
  }

  internal bool? IsTextChanged
  {
    get => this._isTextChanged;
    set => this._isTextChanged = value;
  }

  internal int FactorNeighbourX
  {
    get => this._factorNeighbourX;
    set => this._factorNeighbourX = value;
  }

  internal int FactorNeighbourY
  {
    get => this._factorNeighbourY;
    set => this._factorNeighbourY = value;
  }

  internal void SetPointShape(SmartArtShape shape) => this._pointShape = shape;

  internal int SmartArtId
  {
    get => this._smartArtId;
    set => this._smartArtId = value;
  }

  internal string SmartArtName
  {
    get => this._smartArtName;
    set => this._smartArtName = value;
  }

  internal IHyperLink Hyperlink => (IHyperLink) this._hyperlink;

  internal void SetHyperlink(Syncfusion.Presentation.RichText.Hyperlink hyperLink)
  {
    this._hyperlink = hyperLink;
  }

  internal void Close()
  {
    this._dataModel = (DataModel) null;
    if (this._pointShape != null)
      this._pointShape.Close();
    if (this._textBody != null)
      this._textBody.Close();
    if (this._hyperlink == null)
      return;
    this._hyperlink.Close();
  }

  public SmartArtPoint Clone()
  {
    SmartArtPoint smartArtPoint = (SmartArtPoint) this.MemberwiseClone();
    if (this._customAttributes != null)
      smartArtPoint._customAttributes = Helper.CloneDictionary(this._customAttributes);
    if (this._pointShape != null)
      smartArtPoint._pointShape = (SmartArtShape) this._pointShape.Clone();
    smartArtPoint._textBody = this._textBody.Clone();
    return smartArtPoint;
  }

  internal void SetParent(DataModel dataModel)
  {
    this._dataModel = dataModel;
    if (this._pointShape != null)
      this._pointShape.SetParent(dataModel.ParentSmartArt);
    this._textBody.SetParent((Shape) dataModel.ParentSmartArt);
  }
}
