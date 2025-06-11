// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.PivotAnalysis.DynamicPropertyDescriptor
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable
namespace Syncfusion.XlsIO.Implementation.PivotAnalysis;

public class DynamicPropertyDescriptor(string name, Attribute[] attributes) : PropertyDescriptor(name, attributes)
{
  public override bool IsReadOnly => true;

  public override Type PropertyType => typeof (object);

  public override Type ComponentType => typeof (object);

  public override bool CanResetValue(object component) => false;

  public override object GetValue(object component)
  {
    return component is IDictionary<string, object> dictionary ? dictionary[this.Name] : (object) null;
  }

  public override void ResetValue(object component)
  {
  }

  public override void SetValue(object component, object value)
  {
  }

  public override bool ShouldSerializeValue(object component) => false;
}
