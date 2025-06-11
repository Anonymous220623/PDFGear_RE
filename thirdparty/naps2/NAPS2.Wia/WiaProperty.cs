// Decompiled with JetBrains decompiler
// Type: NAPS2.Wia.WiaProperty
// Assembly: NAPS2.Wia, Version=2.0.0.0, Culture=neutral, PublicKeyToken=e28619810b0dcd38
// MVID: 676F057C-91D3-4AC6-8454-A801FD0BB848
// Assembly location: D:\PDFGear\bin\NAPS2.Wia.dll

using NAPS2.Wia.Native;
using System;

#nullable enable
namespace NAPS2.Wia;

public class WiaProperty
{
  private WiaPropertyAttributes? _attributes;

  protected internal WiaProperty(IntPtr storage, int id, string name, ushort type)
  {
    this.Storage = storage;
    this.Id = id;
    this.Name = name;
    this.Type = type;
  }

  private IntPtr Storage { get; }

  public int Id { get; set; }

  public string Name { get; }

  public ushort Type { get; }

  public object Value
  {
    get
    {
      if (this.Type == (ushort) 3)
      {
        int num;
        WiaException.Check(NativeWiaMethods.GetPropertyInt(this.Storage, this.Id, out num));
        return (object) num;
      }
      if (this.Type != (ushort) 8)
        throw new NotImplementedException($"Not implemented property type: {this.Type}");
      string str;
      WiaException.Check(NativeWiaMethods.GetPropertyBstr(this.Storage, this.Id, out str));
      return (object) str;
    }
    set
    {
      if (this.Type != (ushort) 3)
        throw new NotImplementedException($"Not implemented property type: {this.Type}");
      uint hresult = NativeWiaMethods.SetPropertyInt(this.Storage, this.Id, (int) value);
      if (hresult == 2147942487U)
        throw new ArgumentException($"Could not set property {this.Id} ({this.Name}) value to \"{value}\"", nameof (value));
      WiaException.Check(hresult);
    }
  }

  public WiaPropertyAttributes Attributes
  {
    get
    {
      return this._attributes ?? (this._attributes = new WiaPropertyAttributes(this.Storage, this.Id));
    }
  }

  public override string ToString() => this.Name;
}
