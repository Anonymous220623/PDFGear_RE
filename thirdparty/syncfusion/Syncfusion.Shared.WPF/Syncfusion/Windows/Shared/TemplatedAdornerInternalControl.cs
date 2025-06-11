// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Shared.TemplatedAdornerInternalControl
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Syncfusion.Windows.Shared;

[DesignTimeVisible(false)]
public class TemplatedAdornerInternalControl : AutoTemplatedControl, IDisposable
{
  private TemplatedAdornerBase m_adorner;

  public UIElement AdornedElement => this.m_adorner.AdornedElement;

  public TemplatedAdornerBase Adorner => this.m_adorner;

  public TemplatedAdornerInternalControl(TemplatedAdornerBase adorner)
    : base(adorner.GetType())
  {
    this.m_adorner = adorner != null ? adorner : throw new ArgumentNullException(nameof (adorner));
  }

  public DependencyObject GetTemplateChildInternal(string childName)
  {
    return this.GetTemplateChild(childName);
  }

  internal void Dispose(bool disposing)
  {
    if (!disposing)
      return;
    this.m_adorner = (TemplatedAdornerBase) null;
  }

  public void Dispose() => this.Dispose(true);
}
