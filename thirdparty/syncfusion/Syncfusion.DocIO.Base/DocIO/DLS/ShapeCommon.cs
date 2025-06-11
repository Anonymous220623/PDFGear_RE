// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.ShapeCommon
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.DocIO.DLS;

public abstract class ShapeCommon : ParagraphItem
{
  internal const byte WidthScaleKey = 0;
  private long m_ID;
  private string m_Name;
  private string m_AlternateText;
  private string m_Title;
  private float m_Height;
  private float m_Width;
  private float m_widthScale = 100f;
  private float m_heightScale = 100f;
  internal Dictionary<string, Stream> m_docxProps = new Dictionary<string, Stream>();
  private Dictionary<int, object> m_propertiesHash;
  private string m_path;
  private float m_coordinateXOrigin;
  private float m_coordinateYOrigin;
  private string m_coordinateSize;

  internal long ShapeID
  {
    get => this.m_ID;
    set => this.m_ID = value;
  }

  public float Height
  {
    get => this.m_Height;
    set
    {
      if (this is WChart)
        (this as WChart).OfficeChart.Height = (double) value;
      this.m_Height = value;
    }
  }

  public float Width
  {
    get => this.m_Width;
    set
    {
      if (this is WChart)
        (this as WChart).OfficeChart.Width = (double) value;
      this.m_Width = value;
    }
  }

  public float HeightScale
  {
    get => this.m_heightScale;
    set
    {
      this.m_heightScale = (double) value > 0.0 ? value : throw new ArgumentOutOfRangeException("Scale factor must be greater than 0");
    }
  }

  public float WidthScale
  {
    get
    {
      return this is Shape && (this as Shape).IsHorizontalRule && this.HasKey(0) ? (float) this.PropertiesHash[0] : this.m_widthScale;
    }
    set
    {
      this.m_widthScale = !(this is Shape) || (this as Shape).IsHorizontalRule || (double) value > 0.0 ? value : throw new ArgumentOutOfRangeException("Scale factor must be greater than 0");
      if (!(this is Shape) || !(this as Shape).IsHorizontalRule)
        return;
      this.SetKeyValue(0, (object) value);
    }
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

  internal void SetKeyValue(int propKey, object value) => this[propKey] = value;

  protected object this[int key]
  {
    get => (object) key;
    set => this.PropertiesHash[key] = value;
  }

  internal bool HasKey(int Key)
  {
    return this.m_propertiesHash != null && this.m_propertiesHash.ContainsKey(Key);
  }

  public string AlternativeText
  {
    get => this.m_AlternateText;
    set => this.m_AlternateText = value;
  }

  public string Name
  {
    get => this.m_Name;
    set => this.m_Name = value;
  }

  public string Title
  {
    get => this.m_Title;
    set => this.m_Title = value;
  }

  internal Dictionary<string, Stream> DocxProps
  {
    get
    {
      if (this.m_docxProps == null)
        this.m_docxProps = new Dictionary<string, Stream>();
      return this.m_docxProps;
    }
    set => this.m_docxProps = value;
  }

  internal string Path
  {
    get => this.m_path;
    set => this.m_path = value;
  }

  internal string CoordinateSize
  {
    get => this.m_coordinateSize;
    set => this.m_coordinateSize = value;
  }

  internal float CoordinateXOrigin
  {
    get => this.m_coordinateXOrigin;
    set => this.m_coordinateXOrigin = value;
  }

  internal float CoordinateYOrigin
  {
    get => this.m_coordinateYOrigin;
    set => this.m_coordinateYOrigin = value;
  }

  protected override object CloneImpl()
  {
    ShapeCommon shapeCommon = (ShapeCommon) base.CloneImpl();
    if (this.m_docxProps != null && this.m_docxProps.Count > 0)
      shapeCommon.m_doc.CloneProperties(this.DocxProps, ref shapeCommon.m_docxProps);
    if (this.m_propertiesHash != null && this.m_propertiesHash.Count > 0)
    {
      this.m_propertiesHash = new Dictionary<int, object>();
      foreach (KeyValuePair<int, object> keyValuePair in this.m_propertiesHash)
        shapeCommon.PropertiesHash.Add(keyValuePair.Key, keyValuePair.Value);
    }
    return (object) shapeCommon;
  }

  internal ShapeCommon(WordDocument doc)
    : base(doc)
  {
  }

  internal override void Close()
  {
    if (this.m_propertiesHash != null)
    {
      this.m_propertiesHash.Clear();
      this.m_propertiesHash = (Dictionary<int, object>) null;
    }
    if (this.m_docxProps != null)
    {
      foreach (Stream stream in this.m_docxProps.Values)
        stream.Close();
      this.m_docxProps.Clear();
      this.m_docxProps = (Dictionary<string, Stream>) null;
    }
    base.Close();
  }
}
