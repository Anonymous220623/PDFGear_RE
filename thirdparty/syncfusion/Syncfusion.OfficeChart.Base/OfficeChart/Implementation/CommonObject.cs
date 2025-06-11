// Decompiled with JetBrains decompiler
// Type: Syncfusion.OfficeChart.Implementation.CommonObject
// Assembly: Syncfusion.OfficeChart.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 13B65F2C-9671-4985-A628-598A518023BF
// Assembly location: C:\Program Files\PDFgear\Syncfusion.OfficeChart.Base.dll

using System;
using System.Diagnostics;
using System.Threading;

#nullable disable
namespace Syncfusion.OfficeChart.Implementation;

internal class CommonObject : IParentApplication, IDisposable
{
  private IApplication m_appl;
  private object m_parent;
  private int m_iReferenceCount;
  protected bool m_bIsDisposed;

  public IApplication Application
  {
    [DebuggerStepThrough] get => this.m_appl;
  }

  public object Parent
  {
    [DebuggerStepThrough] get => this.m_parent;
  }

  public ApplicationImpl AppImplementation => (ApplicationImpl) this.m_appl;

  protected CommonObject()
  {
  }

  public CommonObject(IApplication application, object parent)
    : this()
  {
    if (application == null)
      throw new ArgumentNullException(nameof (application));
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    this.m_appl = application;
    this.m_parent = parent;
  }

  ~CommonObject() => this.Dispose();

  public object FindParent(Type parentType) => CommonObject.FindParent(this.m_parent, parentType);

  public object FindParent(Type parentType, bool bSubTypes)
  {
    return CommonObject.FindParent(this.m_parent, parentType, bSubTypes);
  }

  public static object FindParent(object parentStart, Type parentType)
  {
    return CommonObject.FindParent(parentStart, parentType, false);
  }

  public static object FindParent(object parentStart, Type parentType, bool bSubTypes)
  {
    if (parentType == (Type) null)
      throw new ArgumentNullException(nameof (parentType));
    int num = 0;
    IParentApplication parent = (IParentApplication) parentStart;
    bool isInterface = parentType.IsInterface;
    while (num <= 100)
    {
      if (parent != null)
      {
        Type type = parent.GetType();
        if (!isInterface)
        {
          if (type.Equals(parentType) || bSubTypes && type.IsSubclassOf(parentType))
            goto label_10;
        }
        else if (type.GetInterface(parentType.Name, false) != (Type) null)
          goto label_10;
        parent = (IParentApplication) parent.Parent;
        ++num;
        if (parent != null)
          continue;
      }
label_10:
      return (object) parent;
    }
    throw new ArgumentException("links Cycle in object tree detected!");
  }

  public object[] FindParents(Type[] arrTypes)
  {
    int num = 0;
    IParentApplication parent = (IParentApplication) this.m_parent;
    object[] parents = new object[arrTypes.Length];
    while (num <= 100)
    {
      if (parent != null)
      {
        int index = Array.IndexOf<Type>(arrTypes, parent.GetType());
        if (index != -1)
          parents[index] = (object) parent;
        parent = (IParentApplication) parent.Parent;
        ++num;
        if (parent != null)
          continue;
      }
      return parents;
    }
    throw new ArgumentException("links Cycle in object tree detected!");
  }

  public object FindParent(Type[] arrTypes)
  {
    int num = 0;
    IParentApplication parent = (IParentApplication) this.Parent;
    while (num <= 100)
    {
      if (parent != null)
      {
        if (Array.IndexOf<Type>(arrTypes, parent.GetType()) != -1)
          return (object) parent;
        parent = (IParentApplication) parent.Parent;
        ++num;
        if (parent != null)
          continue;
      }
      return (object) parent;
    }
    throw new ArgumentException("links Cycle in object tree detected!");
  }

  protected internal void SetParent(object parent)
  {
    this.m_parent = parent != null ? parent : throw new ArgumentNullException(nameof (parent));
  }

  protected void CheckDisposed()
  {
    if (this.m_bIsDisposed)
      throw new ApplicationException("Object was disposed.");
  }

  [DebuggerStepThrough]
  public virtual int AddReference() => Interlocked.Increment(ref this.m_iReferenceCount);

  [DebuggerStepThrough]
  public virtual int ReleaseReference() => Interlocked.Decrement(ref this.m_iReferenceCount);

  public int ReferenceCount
  {
    [DebuggerStepThrough] get => this.m_iReferenceCount;
  }

  public virtual void Dispose()
  {
    if (this.m_bIsDisposed)
      return;
    this.OnDispose();
    this.m_parent = (object) null;
    this.m_appl = (IApplication) null;
    this.m_bIsDisposed = true;
    GC.SuppressFinalize((object) this);
  }

  protected virtual void OnDispose()
  {
  }

  protected void SetParent(IApplication application, object parent)
  {
    if (application == null)
      throw new ArgumentNullException(nameof (application));
    if (parent == null)
      throw new ArgumentNullException(nameof (parent));
    this.m_appl = application;
    this.m_parent = parent;
  }
}
