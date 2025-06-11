// Decompiled with JetBrains decompiler
// Type: Syncfusion.DocIO.DLS.WControlField
// Assembly: Syncfusion.DocIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 5B963185-A109-4004-8296-CCBE35E10BFD
// Assembly location: C:\Program Files\PDFgear\Syncfusion.DocIO.Base.dll

#nullable disable
namespace Syncfusion.DocIO.DLS;

public class WControlField : WField
{
  private int m_storagePicLocation;
  private Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject m_oleObject;

  public override EntityType EntityType => EntityType.ControlField;

  internal int StoragePicLocation
  {
    get => this.m_storagePicLocation;
    set => this.m_storagePicLocation = value;
  }

  internal Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject OleObject
  {
    get
    {
      if (this.m_oleObject == null)
        this.m_oleObject = new Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject();
      return this.m_oleObject;
    }
  }

  internal WControlField(IWordDocument doc)
    : base(doc)
  {
    this.m_paraItemType = ParagraphItemType.ControlField;
  }

  protected override object CloneImpl()
  {
    WControlField wcontrolField = (WControlField) base.CloneImpl();
    if (this.m_oleObject != null)
    {
      wcontrolField.m_oleObject = this.m_oleObject.Clone();
      wcontrolField.m_storagePicLocation = WOleObject.NextOleObjId;
      wcontrolField.m_oleObject.Storage.StorageName = this.m_storagePicLocation.ToString();
    }
    return (object) wcontrolField;
  }

  internal override void CloneRelationsTo(WordDocument doc, OwnerHolder nextOwner)
  {
    base.CloneRelationsTo(doc, nextOwner);
    if (doc == null)
      return;
    string collection = this.OleObject.AddOleObjectToCollection(doc.OleObjectCollection, this.m_storagePicLocation.ToString());
    if (string.IsNullOrEmpty(collection))
      return;
    this.OleObject.Storage.StorageName = collection;
  }

  internal override void Close()
  {
    if (this.m_oleObject != null)
    {
      this.m_oleObject.Close();
      this.m_oleObject = (Syncfusion.DocIO.ReaderWriter.DataStreamParser.OLEObject.OLEObject) null;
    }
    base.Close();
  }
}
