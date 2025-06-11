// Decompiled with JetBrains decompiler
// Type: Syncfusion.XlsIO.Implementation.ParametersCollection
// Assembly: Syncfusion.XlsIO.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 92616603-9632-451A-8DB0-51A3184E0997
// Assembly location: C:\Program Files\PDFgear\Syncfusion.XlsIO.Base.dll

using Syncfusion.XlsIO.Implementation.Collections;
using System;
using System.Collections;

#nullable disable
namespace Syncfusion.XlsIO.Implementation;

internal class ParametersCollection(IApplication application, object parent) : 
  CollectionBaseEx<IParameter>(application, parent),
  IParameters,
  IParentApplication,
  IEnumerable
{
  public IParameter Add(string name, ExcelParameterDataType dataType)
  {
    IParameter parameter = (IParameter) this.CreateParameter(name, dataType);
    this.List.Add(parameter);
    return parameter;
  }

  private ParameterImpl CreateParameter(string name, ExcelParameterDataType dataType)
  {
    return new ParameterImpl(this.Application, (object) this, name, dataType);
  }

  internal ParametersCollection Clone(object parent)
  {
    ParametersCollection parent1 = parent != null ? (ParametersCollection) base.Clone(parent) : throw new ArgumentNullException(nameof (parent));
    parent1.Clear();
    for (int i = 0; i < this.Count; ++i)
    {
      ParameterImpl parameterImpl = this[i] as ParameterImpl;
      parent1.Add((IParameter) parameterImpl.Clone((object) parent1));
    }
    return parent1;
  }

  internal void Dispose()
  {
    for (int i = this.Count - 1; i > -1; --i)
      ((ParameterImpl) this[i]).Dispose();
  }
}
