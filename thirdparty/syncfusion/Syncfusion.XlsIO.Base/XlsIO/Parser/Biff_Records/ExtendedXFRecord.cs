// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Parser.Biff_Records.ExtendedXFRecord
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.IO;

#nullable disable
namespace Syncfusion.XlsIO.Parser.Biff_Records;

[Biff(TBIFFRecord.ExtendedXFRecord)]
[CLSCompliant(false)]
public class ExtendedXFRecord : BiffRecordRaw
{
  public const int StartLength = 20;
  private FutureHeader m_header;
  private ushort m_usXFIndex;
  private ushort m_propertyCount;
  private List<ExtendedProperty> m_properties;

  public ushort XFIndex
  {
    get => this.m_usXFIndex;
    set => this.m_usXFIndex = value;
  }

  public ushort PropertyCount
  {
    get => this.m_propertyCount;
    set => this.m_propertyCount = value;
  }

  public List<ExtendedProperty> Properties
  {
    get => this.m_properties;
    set => this.m_properties = value;
  }

  public ExtendedXFRecord() => this.InitializeObjects();

  public ExtendedXFRecord(Stream stream, out int itemSize)
    : base(stream, out itemSize)
  {
  }

  public ExtendedXFRecord(int iReserve)
    : base(iReserve)
  {
    this.m_iCode = 2173;
  }

  private void InitializeObjects()
  {
    this.m_header = new FutureHeader();
    this.m_header.Type = (ushort) 2173;
    this.m_properties = new List<ExtendedProperty>();
  }

  public override void ParseStructure(
    DataProvider provider,
    int iOffset,
    int iLength,
    ExcelVersion version)
  {
    this.m_header.Type = provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_header.Attributes = provider.ReadUInt16(iOffset);
    iOffset += 2;
    int num1 = (int) provider.ReadUInt16(iOffset);
    iOffset += 8;
    int num2 = (int) provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_usXFIndex = provider.ReadUInt16(iOffset);
    iOffset += 2;
    int num3 = (int) provider.ReadUInt16(iOffset);
    iOffset += 2;
    this.m_propertyCount = provider.ReadUInt16(iOffset);
    iOffset += 2;
    for (int index = 0; index < (int) this.m_propertyCount; ++index)
    {
      ExtendedProperty extendedProperty = new ExtendedProperty();
      iOffset = extendedProperty.ParseExtendedProperty(provider, iOffset, version);
      this.m_properties.Add(extendedProperty);
    }
  }

  public override void InfillInternalData(DataProvider provider, int iOffset, ExcelVersion version)
  {
    provider.WriteUInt16(iOffset, this.m_header.Type);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.m_header.Attributes);
    iOffset += 2;
    provider.WriteUInt16(iOffset, (ushort) 0);
    iOffset += 8;
    provider.WriteUInt16(iOffset, (ushort) 0);
    iOffset += 2;
    provider.WriteUInt16(iOffset, this.XFIndex);
    iOffset += 2;
    provider.WriteUInt16(iOffset, (ushort) 0);
    iOffset += 2;
    provider.WriteUInt16(iOffset, (ushort) this.Properties.Count);
    iOffset += 2;
    foreach (ExtendedProperty property in this.Properties)
      iOffset = property.InfillInternalData(provider, iOffset, version);
  }

  public override int GetStoreSize(ExcelVersion version)
  {
    int num = 0;
    foreach (ExtendedProperty property in this.m_properties)
      num += (int) property.Size;
    return 20 + num;
  }

  public override int GetHashCode()
  {
    return this.m_header.Type.GetHashCode() ^ this.m_header.Attributes.GetHashCode() ^ this.m_usXFIndex.GetHashCode() ^ this.m_propertyCount.GetHashCode() ^ this.Properties.GetHashCode();
  }

  public int CompareTo(ExtendedXFRecord twin)
  {
    if (twin == null)
      throw new ArgumentNullException(nameof (twin));
    int num1 = (int) this.m_header.Type - (int) twin.m_header.Type;
    if (num1 != 0)
      return num1;
    int num2 = (int) this.m_header.Attributes - (int) twin.m_header.Attributes;
    if (num2 != 0)
      return num2;
    int num3 = (int) this.m_usXFIndex - (int) twin.m_usXFIndex;
    if (num3 != 0)
      return num3;
    int num4 = (int) this.m_propertyCount - (int) twin.m_propertyCount;
    return num4 != 0 || this.m_properties == twin.m_properties ? num4 : 1;
  }

  public override void CopyTo(BiffRecordRaw raw)
  {
    if (raw == null)
      throw new ArgumentNullException(nameof (raw));
    if (!(raw is ExtendedXFRecord twin))
      throw new ArgumentException(nameof (raw));
    this.CopyTo(twin);
  }

  public void CopyTo(ExtendedXFRecord twin)
  {
    twin.m_header.Type = this.m_header.Type;
    twin.m_header.Attributes = this.m_header.Attributes;
    twin.m_usXFIndex = this.m_usXFIndex;
    twin.m_propertyCount = this.m_propertyCount;
    twin.m_properties = this.m_properties;
  }

  public override object Clone()
  {
    ExtendedXFRecord extendedXfRecord = (ExtendedXFRecord) base.Clone();
    extendedXfRecord.Properties = new List<ExtendedProperty>();
    return (object) extendedXfRecord;
  }

  public ExtendedXFRecord CloneObject() => (ExtendedXFRecord) this.MemberwiseClone();
}
