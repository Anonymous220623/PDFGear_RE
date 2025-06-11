// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Graphics.Images.Metafiles.ObjectData
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Drawing;

#nullable disable
namespace Syncfusion.Pdf.Graphics.Images.Metafiles;

internal class ObjectData
{
  private const int IndexMask = 255 /*0xFF*/;
  private object[] m_objects;
  private object[] m_states;
  private System.Drawing.Graphics m_graphics;
  private Image m_bmp;

  public System.Drawing.Graphics Graphics => this.m_graphics;

  public ObjectData()
  {
    this.m_bmp = (Image) new Bitmap(1, 1);
    this.m_graphics = System.Drawing.Graphics.FromImage(this.m_bmp);
    this.m_objects = new object[256 /*0x0100*/];
    this.m_states = new object[256 /*0x0100*/];
  }

  public void Dispose() => this.DisposeObjects();

  public Font GetFont(int index)
  {
    index &= (int) byte.MaxValue;
    return this.GetObject(index) as Font;
  }

  public Brush GetBrush(int index)
  {
    index &= (int) byte.MaxValue;
    return this.GetObject(index) as Brush;
  }

  public Pen GetPen(int index)
  {
    index &= (int) byte.MaxValue;
    return this.GetObject(index) as Pen;
  }

  public void SetPen(int index, Pen pen)
  {
    index &= (int) byte.MaxValue;
    this.SetObject(index, (object) pen);
  }

  public object GetObject(int index)
  {
    object obj = (object) null;
    index &= (int) byte.MaxValue;
    if (index >= 0 || index < this.m_objects.Length)
      obj = this.m_objects[index];
    return obj;
  }

  public void SetObject(int index, object obj)
  {
    index &= (int) byte.MaxValue;
    if (index < 0 || index >= this.m_objects.Length || obj == null)
      return;
    if (this.m_objects[index] is IDisposable disposable)
      disposable.Dispose();
    this.m_objects[index] = obj;
  }

  public object GetState(int index)
  {
    object state = (object) null;
    index &= (int) byte.MaxValue;
    if (index >= 0 || index < this.m_states.Length)
      state = this.m_states[index];
    return state;
  }

  public void SetState(int index, object state)
  {
    index &= (int) byte.MaxValue;
    if (index < 0 || index >= this.m_states.Length || state == null)
      return;
    if (this.m_states[index] is IDisposable state1)
      state1.Dispose();
    this.m_states[index] = state;
  }

  private void DisposeObjects()
  {
    if (this.m_objects == null)
      return;
    int index = 0;
    for (int length = this.m_objects.Length; index < length; ++index)
    {
      if (this.m_objects[index] is IDisposable disposable)
      {
        disposable.Dispose();
        this.m_objects[index] = (object) null;
      }
    }
    this.m_objects = (object[]) null;
  }
}
