// Decompiled with JetBrains decompiler
// Type: Syncfusion.Pdf.Security.TSGenerator
// Assembly: Syncfusion.Pdf.Base, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: D80BE54F-56D0-445B-815D-7CBF7AF400A1
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Pdf.Base.dll

using System;
using System.Threading;

#nullable disable
namespace Syncfusion.Pdf.Security;

internal class TSGenerator
{
  internal byte[] Generate(int numBytes, bool fast)
  {
    return new TSGenerator.SGenerator().GenerateS(numBytes, fast);
  }

  private class SGenerator
  {
    private int m_count;
    private bool m_stop;

    private void Run(object ignored)
    {
      while (!this.m_stop)
        ++this.m_count;
    }

    internal byte[] GenerateS(int numBytes, bool fast)
    {
      ThreadPriority priority = Thread.CurrentThread.Priority;
      try
      {
        Thread.CurrentThread.Priority = ThreadPriority.Normal;
        return this.Generate(numBytes, fast);
      }
      finally
      {
        Thread.CurrentThread.Priority = priority;
      }
    }

    private byte[] Generate(int numBytes, bool fast)
    {
      this.m_count = 0;
      this.m_stop = false;
      byte[] numArray = new byte[numBytes];
      int num1 = 0;
      int num2 = fast ? numBytes : numBytes * 8;
      ThreadPool.QueueUserWorkItem(new WaitCallback(this.Run));
      for (int index1 = 0; index1 < num2; ++index1)
      {
        while (this.m_count == num1)
        {
          try
          {
            Thread.Sleep(1);
          }
          catch (Exception ex)
          {
          }
        }
        num1 = this.m_count;
        if (fast)
        {
          numArray[index1] = (byte) num1;
        }
        else
        {
          int index2 = index1 / 8;
          numArray[index2] = (byte) ((int) numArray[index2] << 1 | num1 & 1);
        }
      }
      this.m_stop = true;
      return numArray;
    }
  }
}
