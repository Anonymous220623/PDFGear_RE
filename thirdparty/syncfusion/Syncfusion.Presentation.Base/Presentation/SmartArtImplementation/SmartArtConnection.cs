// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.SmartArtConnection
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using System;

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class SmartArtConnection
{
  private DataModel _dataModel;
  private Guid _modelId;
  private SmartArtConnectionType _type;
  private Guid _sourceId;
  private Guid _destinationId;
  private uint _sourcePosition;
  private uint _destinationPosition;
  private Guid _parentTransitionId;
  private Guid _siblingTransitionId;
  private string _presentationId;

  internal SmartArtConnection(DataModel dataModel) => this._dataModel = dataModel;

  internal Guid ModelId
  {
    get => this._modelId;
    set => this._modelId = value;
  }

  internal Guid SourceId
  {
    get => this._sourceId;
    set => this._sourceId = value;
  }

  internal Guid DestinationId
  {
    get => this._destinationId;
    set => this._destinationId = value;
  }

  internal uint SourcePosition
  {
    get => this._sourcePosition;
    set => this._sourcePosition = value;
  }

  internal uint DestinationPosition
  {
    get => this._destinationPosition;
    set => this._destinationPosition = value;
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

  internal SmartArtConnectionType Type
  {
    get => this._type;
    set => this._type = value;
  }

  internal string PresentationId
  {
    get => this._presentationId;
    set => this._presentationId = value;
  }

  internal void Close() => this._dataModel = (DataModel) null;

  public SmartArtConnection Clone() => (SmartArtConnection) this.MemberwiseClone();

  internal void SetParent(DataModel dataModel) => this._dataModel = dataModel;
}
