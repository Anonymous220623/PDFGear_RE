// Decompiled with JetBrains decompiler
// Type: Syncfusion.Presentation.SmartArtImplementation.DataModel
// Assembly: Syncfusion.Presentation.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 687EF99F-80E0-4C13-B922-D8C9F0498AA8
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Presentation.Base.dll

using Syncfusion.Presentation.RichText;
using System;
using System.Collections.Generic;

#nullable disable
namespace Syncfusion.Presentation.SmartArtImplementation;

internal class DataModel
{
  private SmartArtPoints _pointList;
  private SmartArtType _smartArtType;
  private bool _isSmartArtTypeSet;
  private SmartArtConnections _connectionList;
  private string _smartArtcategory;
  private SmartArt _smartArt;
  private uint _nodeCount;
  private uint _childNodeCount;
  private RelationCollection _topRelation;
  private RelationCollection _drawingRelation;
  private Dictionary<Guid, SmartArtShape> _smartArtShapeCollection;
  private string _relationId;
  private string _colorSchemeType;
  private string _quickStyleType;

  internal DataModel(SmartArt smartArt)
  {
    this._smartArt = smartArt;
    this._pointList = new SmartArtPoints(this);
    this._connectionList = new SmartArtConnections(this);
    this._smartArtShapeCollection = new Dictionary<Guid, SmartArtShape>();
  }

  internal Dictionary<Guid, SmartArtShape> SmartArtShapeCollection => this._smartArtShapeCollection;

  internal RelationCollection TopRelation
  {
    get => this._topRelation;
    set => this._topRelation = value;
  }

  internal RelationCollection DrawingRelation
  {
    get => this._drawingRelation;
    set => this._drawingRelation = value;
  }

  internal SmartArtConnections ConnectionCollection => this._connectionList;

  internal SmartArt ParentSmartArt => this._smartArt;

  internal string Category
  {
    get => this._smartArtcategory;
    set => this._smartArtcategory = value;
  }

  internal SmartArtPoints PointCollection => this._pointList;

  internal string QuickStyleType
  {
    get => this._quickStyleType;
    set => this._quickStyleType = value;
  }

  internal string RelationId
  {
    get => this._relationId;
    set => this._relationId = value;
  }

  internal string ColorSchemeType
  {
    get => this._colorSchemeType;
    set => this._colorSchemeType = value;
  }

  internal SmartArtType SmartArtType
  {
    get => this._smartArtType;
    set
    {
      this._smartArtType = value;
      this._isSmartArtTypeSet = true;
      this._smartArt.SetCategoryByType();
    }
  }

  internal bool IsSmartArtTypeSet => this._isSmartArtTypeSet;

  internal uint NodeCount
  {
    get => this._nodeCount;
    set => this._nodeCount = value;
  }

  internal uint ChildNodeCount
  {
    get => this._childNodeCount;
    set => this._childNodeCount = value;
  }

  internal List<SmartArtShape> GetSmartArtShapeList()
  {
    SmartArtShape[] smartArtShapeArray = new SmartArtShape[this._smartArtShapeCollection.Count];
    this._smartArtShapeCollection.Values.CopyTo(smartArtShapeArray, 0);
    return new List<SmartArtShape>((IEnumerable<SmartArtShape>) smartArtShapeArray);
  }

  internal void AddParentNodesToSmartArt()
  {
    SortedList<uint, SmartArtNode> sortedList = new SortedList<uint, SmartArtNode>();
    foreach (KeyValuePair<Guid, SmartArtConnection> keyValuePair in this._connectionList.Dictionary)
    {
      SmartArtConnection smartArtConnection = keyValuePair.Value;
      if (smartArtConnection.Type == SmartArtConnectionType.ParentOf && smartArtConnection.SourceId == this._pointList[0].ModelId)
      {
        SmartArtNode smartArtNode = new SmartArtNode(this._smartArt.Nodes as SmartArtNodes);
        SmartArtPoint point1 = this._pointList[smartArtConnection.DestinationId];
        if (point1 != null)
        {
          smartArtNode.ObtainShapes().Add(point1.PointShapeProperties);
          smartArtNode.BasePoint = point1;
          SmartArtPoint point2 = this._pointList[smartArtConnection.SiblingTransitionId];
          smartArtNode.ObtainShapes().Add(point2.PointShapeProperties);
          smartArtNode.SiblingPoint = point2;
          smartArtNode.Id = point1.ModelId;
          smartArtNode.SetTextBody(point1.TextBody);
          sortedList.Add(smartArtConnection.SourcePosition, smartArtNode);
        }
      }
    }
    foreach (KeyValuePair<uint, SmartArtNode> keyValuePair in sortedList)
      (this._smartArt.Nodes as SmartArtNodes).Add(keyValuePair.Value);
    sortedList.Clear();
  }

  internal void AddChildNodes()
  {
    this.IterateFromNodeCollection(this._smartArt.Nodes as SmartArtNodes);
  }

  private void IterateFromNodeCollection(SmartArtNodes nodeCollection)
  {
    foreach (SmartArtNode node in nodeCollection)
    {
      foreach (KeyValuePair<uint, SmartArtConnection> keyValuePair in this.GetConnectionDestinationId(node.Id))
      {
        SmartArtPoint point1 = this._pointList[keyValuePair.Value.DestinationId];
        SmartArtNode newNode = new SmartArtNode(this._smartArt.Nodes as SmartArtNodes);
        newNode.Id = point1.ModelId;
        newNode.BasePoint = point1;
        newNode.ObtainShapes().Add(point1.PointShapeProperties);
        SmartArtPoint point2 = this._pointList[keyValuePair.Value.SiblingTransitionId];
        newNode.SiblingPoint = point2;
        newNode.ObtainShapes().Add(point2.PointShapeProperties);
        newNode.SetTextBody(point1.TextBody);
        (node.ChildNodes as SmartArtNodes).Add(newNode);
      }
      this.IterateFromNodeCollection(node.ChildNodes as SmartArtNodes);
    }
  }

  private SortedList<uint, SmartArtConnection> GetConnectionDestinationId(Guid nodeId)
  {
    SortedList<uint, SmartArtConnection> connectionDestinationId = new SortedList<uint, SmartArtConnection>();
    foreach (KeyValuePair<Guid, SmartArtConnection> keyValuePair in this._connectionList.Dictionary)
    {
      SmartArtConnection smartArtConnection = keyValuePair.Value;
      if (smartArtConnection.Type == SmartArtConnectionType.ParentOf && smartArtConnection.SourceId == nodeId)
        connectionDestinationId.Add(smartArtConnection.SourcePosition, smartArtConnection);
    }
    return connectionDestinationId;
  }

  internal void RefreshSmartArtShapeCollection()
  {
    if (this._smartArt.CreatedSmartArt)
      return;
    foreach (SmartArtPoint point in this._pointList.GetPointList())
    {
      if (point.Type == SmartArtPointType.Node && point.HasShapeProperties)
      {
        Guid destinationId = this.GetDestinationId(point.ModelId);
        if (destinationId != Guid.Empty && this._smartArtShapeCollection.ContainsKey(destinationId))
          this._smartArtShapeCollection[destinationId].SetFill(point.PointShapeProperties.GetFillFormat());
      }
    }
  }

  private Guid GetDestinationId(Guid guid)
  {
    foreach (SmartArtConnection connection in this._connectionList.GetConnectionList())
    {
      if (connection.Type == SmartArtConnectionType.PresentationOf && connection.SourceId == guid)
        return connection.DestinationId;
    }
    return Guid.Empty;
  }

  internal void SetSmartArtNodeCount(uint nodeCount, uint childNodeCount)
  {
    this._nodeCount = nodeCount;
    this._childNodeCount = childNodeCount;
  }

  private void IterateAndSetTextContentToSmartArtShapeCollection(SmartArtNodes nodes)
  {
    foreach (SmartArtNode node in nodes)
    {
      TextBody textBody = node.ObtainTextBody();
      SmartArtShape smartArtShape = this._smartArtShapeCollection[node.Id];
      if (textBody.Text != "" && smartArtShape.TextBody.Paragraphs.Count > 0)
        smartArtShape.TextBody.Paragraphs[0].TextParts[0].Text = textBody.Text;
      else
        smartArtShape.TextBody.AddParagraph(textBody.Text);
      this.IterateAndSetTextContentToSmartArtShapeCollection(node.ChildNodes as SmartArtNodes);
    }
  }

  internal void Close()
  {
    this._connectionList.Close();
    this.CloseDrawingShapeList();
    this._pointList.Close();
    this._smartArt = (SmartArt) null;
    if (this._topRelation != null)
      this._topRelation.Close();
    if (this._drawingRelation == null)
      return;
    this._drawingRelation.Close();
  }

  private void CloseDrawingShapeList()
  {
    foreach (KeyValuePair<Guid, SmartArtShape> smartArtShape in this._smartArtShapeCollection)
      smartArtShape.Value.Close();
    this._smartArtShapeCollection.Clear();
  }

  public DataModel Clone()
  {
    DataModel dataModel = (DataModel) this.MemberwiseClone();
    dataModel._connectionList = this._connectionList.Clone();
    dataModel._pointList = this._pointList.Clone();
    dataModel._smartArtShapeCollection = this.CloneShapeCollection();
    if (this._topRelation != null)
      dataModel._topRelation = this._topRelation.Clone();
    if (this._relationId != null)
      dataModel._relationId = (string) null;
    return dataModel;
  }

  private Dictionary<Guid, SmartArtShape> CloneShapeCollection()
  {
    Dictionary<Guid, SmartArtShape> dictionary = new Dictionary<Guid, SmartArtShape>();
    foreach (KeyValuePair<Guid, SmartArtShape> smartArtShape1 in this._smartArtShapeCollection)
    {
      SmartArtShape smartArtShape2 = (SmartArtShape) smartArtShape1.Value.Clone();
      dictionary.Add(smartArtShape1.Key, smartArtShape2);
    }
    return dictionary;
  }

  internal void SetParent(SmartArt smartArt)
  {
    this._smartArt = smartArt;
    this._pointList.SetParent(this);
    this._connectionList.SetParent(this);
    foreach (KeyValuePair<Guid, SmartArtShape> smartArtShape in this._smartArtShapeCollection)
      smartArtShape.Value.SetParent(smartArt);
  }
}
