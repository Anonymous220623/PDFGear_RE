// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.EmfObjectCollection
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using Syncfusion.Pdf.Native;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal class EmfObjectCollection
{
  private const uint StockFlag = 2147483648 /*0x80000000*/;
  private const int StockModifFlag = 2147483647 /*0x7FFFFFFF*/;
  private static Hashtable s_standartGraphicObjects = new Hashtable();
  private Hashtable m_createdGraphicObjects;
  private ArrayList m_avaibleIndexes;

  static EmfObjectCollection()
  {
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 0, (object) (Brushes.White.Clone() as Brush));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 1, (object) (Brushes.LightGray.Clone() as Brush));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 2, (object) (Brushes.Gray.Clone() as Brush));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 3, (object) (Brushes.DarkGray.Clone() as Brush));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 4, (object) (Brushes.Black.Clone() as Brush));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 5, (object) (Brushes.Transparent.Clone() as Brush));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 6, (object) (Pens.White.Clone() as Pen));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 7, (object) (Pens.Black.Clone() as Pen));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 8, (object) (Pens.Transparent.Clone() as Pen));
    Font defaultFont = Control.DefaultFont;
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 10, (object) (defaultFont.Clone() as Font));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 11, (object) (defaultFont.Clone() as Font));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 12, (object) (defaultFont.Clone() as Font));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 13, (object) (defaultFont.Clone() as Font));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 14, (object) (defaultFont.Clone() as Font));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 15, (object) (defaultFont.Clone() as Font));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 16 /*0x10*/, (object) (defaultFont.Clone() as Font));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 17, (object) (defaultFont.Clone() as Font));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 18, (object) (Brushes.White.Clone() as Brush));
    EmfObjectCollection.s_standartGraphicObjects.Add((object) 19, (object) (Pens.Black.Clone() as Pen));
  }

  protected internal Hashtable CreatedGraphicObjects
  {
    get
    {
      if (this.m_createdGraphicObjects == null)
        this.m_createdGraphicObjects = new Hashtable();
      return this.m_createdGraphicObjects;
    }
  }

  private ArrayList AvaibleIndexes
  {
    get
    {
      if (this.m_avaibleIndexes == null)
        this.m_avaibleIndexes = new ArrayList();
      return this.m_avaibleIndexes;
    }
  }

  public void AddObject(object value, int index)
  {
    this.CreatedGraphicObjects[(object) index] = value != null ? value : throw new ArgumentNullException(nameof (value));
  }

  public void AddObject(object value)
  {
    if (value == null)
      throw new ArgumentNullException(nameof (value));
    int index;
    if (this.AvaibleIndexes.Count > 0)
    {
      index = (int) this.AvaibleIndexes[0];
      this.AvaibleIndexes.RemoveAt(0);
    }
    else
      index = this.CreatedGraphicObjects.Count;
    this.AddObject(value, index);
  }

  public object SelectObject(int index)
  {
    object obj;
    if (!this.IsInStock(index))
    {
      if (this.AvaibleIndexes.Contains((object) index))
        this.AvaibleIndexes.Remove((object) index);
      obj = this.CreatedGraphicObjects[(object) index];
    }
    else
      obj = this.GetStockObjectMasked(index);
    return obj;
  }

  public object DeleteObject(int index)
  {
    object createdGraphicObject = this.CreatedGraphicObjects[(object) index];
    this.CreatedGraphicObjects[(object) index] = (object) null;
    if (!this.AvaibleIndexes.Contains((object) index))
      this.AvaibleIndexes.Add((object) index);
    return createdGraphicObject;
  }

  public void Clear()
  {
    if (this.m_createdGraphicObjects != null)
      this.m_createdGraphicObjects.Clear();
    if (this.m_avaibleIndexes == null)
      return;
    this.m_avaibleIndexes.Clear();
  }

  public bool IsStockObject(object value)
  {
    return value != null ? EmfObjectCollection.s_standartGraphicObjects.ContainsValue(value) : throw new ArgumentNullException(nameof (value));
  }

  public object GetStockObject(STOCK objId)
  {
    return EmfObjectCollection.s_standartGraphicObjects[(object) (int) objId];
  }

  private bool IsInStock(int objId) => ((long) objId & 2147483648L /*0x80000000*/) != 0L;

  private object GetStockObjectMasked(int objId)
  {
    objId &= int.MaxValue;
    return EmfObjectCollection.s_standartGraphicObjects[(object) objId];
  }
}
