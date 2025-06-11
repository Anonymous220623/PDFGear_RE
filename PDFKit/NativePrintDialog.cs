// Decompiled with JetBrains decompiler
// Type: PDFKit.NativePrintDialog
// Assembly: PDFKit, Version=0.0.49.0, Culture=neutral, PublicKeyToken=null
// MVID: CB43C11C-4ACB-4F72-B4B2-5CBC22103942
// Assembly location: C:\Program Files\PDFgear\PDFKit.dll

using System;
using System.Printing;
using System.Printing.Interop;
using System.Runtime.InteropServices;
using System.Windows.Controls;

#nullable disable
namespace PDFKit;

internal class NativePrintDialog
{
  private PrintQueue mPrintQueue = (PrintQueue) null;
  private PrintTicket mPrintTicket = (PrintTicket) null;
  private uint mMaxPage = 9999;
  private uint mMinPage = 1;
  private PageRange mPageRange;
  private bool mPageRangeEnabled;
  private PageRangeSelection mPageRangeSelection = PageRangeSelection.AllPages;

  internal uint ShowDialog(IntPtr hwnd)
  {
    using (NativePrintDialog.PrintDlgEx printDlgEx = new NativePrintDialog.PrintDlgEx(hwnd, this))
      return printDlgEx.ShowPrintDlgEx();
  }

  internal PrintQueue PrintQueue
  {
    get => this.mPrintQueue;
    set => this.mPrintQueue = value;
  }

  internal PrintTicket PrintTicket
  {
    get => this.mPrintTicket;
    set => this.mPrintTicket = value;
  }

  internal uint MinPage
  {
    get => this.mMinPage;
    set => this.mMinPage = value;
  }

  internal uint MaxPage
  {
    get => this.mMaxPage;
    set => this.mMaxPage = value;
  }

  internal bool PageRangeEnabled
  {
    get => this.mPageRangeEnabled;
    set => this.mPageRangeEnabled = value;
  }

  internal PageRange PageRange
  {
    get => this.mPageRange;
    set => this.mPageRange = value;
  }

  internal PageRangeSelection PageRangeSelection
  {
    get => this.mPageRangeSelection;
    set => this.mPageRangeSelection = value;
  }

  private class PrintDlgEx : IDisposable
  {
    private IntPtr mPrintDlgExHnd;
    private NativePrintDialog mDialogOwner;
    private IntPtr mWinHandle;

    internal PrintDlgEx(IntPtr owner, NativePrintDialog dialog)
    {
      this.mWinHandle = owner;
      this.mDialogOwner = dialog;
      this.mPrintDlgExHnd = this.AllocatePrintDlgExStruct();
    }

    internal uint ShowPrintDlgEx()
    {
      return NativeMethods.PrintDlgEx(this.mPrintDlgExHnd) == 0 ? this.GetResult() : 0U;
    }

    ~PrintDlgEx() => this.Dispose(true);

    private bool Is64Bits() => Marshal.SizeOf<IntPtr>(IntPtr.Zero) == 8;

    private IntPtr AllocatePrintDlgExStruct()
    {
      IntPtr ptr = IntPtr.Zero;
      NativeMethods.PRINTPAGERANGE structure1;
      structure1.nToPage = (uint) this.mDialogOwner.PageRange.PageTo;
      structure1.nFromPage = (uint) this.mDialogOwner.PageRange.PageFrom;
      try
      {
        if (!this.Is64Bits())
        {
          NativeMethods.PRINTDLGEX32 structure2 = new NativeMethods.PRINTDLGEX32();
          structure2.hwndOwner = this.mWinHandle;
          structure2.nMinPage = this.mDialogOwner.MinPage;
          structure2.nMaxPage = this.mDialogOwner.MaxPage;
          structure2.Flags = 10223620U;
          if (this.mDialogOwner.PageRangeEnabled)
          {
            structure2.lpPageRanges = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (NativeMethods.PRINTPAGERANGE)));
            structure2.nMaxPageRanges = 1U;
            if (this.mDialogOwner.PageRangeSelection == PageRangeSelection.UserPages)
            {
              structure2.nPageRanges = 1U;
              Marshal.StructureToPtr<NativeMethods.PRINTPAGERANGE>(structure1, structure2.lpPageRanges, false);
              structure2.Flags |= 2U;
            }
            else
              structure2.nPageRanges = 0U;
          }
          else
          {
            structure2.lpPageRanges = IntPtr.Zero;
            structure2.nMaxPageRanges = 0U;
            structure2.Flags |= 8U;
          }
          if (this.mDialogOwner.PrintQueue != null)
          {
            structure2.hDevNames = this.InitializeDevNames(this.mDialogOwner.PrintQueue.FullName);
            if (this.mDialogOwner.PrintTicket != null)
              structure2.hDevMode = this.InitializeDevMode(this.mDialogOwner.PrintQueue.FullName, this.mDialogOwner.PrintTicket);
          }
          ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (NativeMethods.PRINTDLGEX32)));
          Marshal.StructureToPtr<NativeMethods.PRINTDLGEX32>(structure2, ptr, false);
          return ptr;
        }
        NativeMethods.PRINTDLGEX64 structure3 = new NativeMethods.PRINTDLGEX64();
        structure3.hwndOwner = this.mWinHandle;
        structure3.nMinPage = this.mDialogOwner.MinPage;
        structure3.nMaxPage = this.mDialogOwner.MaxPage;
        structure3.Flags = 10223620U;
        if (this.mDialogOwner.PageRangeEnabled)
        {
          structure3.lpPageRanges = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (NativeMethods.PRINTPAGERANGE)));
          structure3.nMaxPageRanges = 1U;
          if (this.mDialogOwner.PageRangeSelection == PageRangeSelection.UserPages)
          {
            structure3.nPageRanges = 1U;
            Marshal.StructureToPtr<NativeMethods.PRINTPAGERANGE>(structure1, structure3.lpPageRanges, false);
            structure3.Flags |= 2U;
          }
          else
            structure3.nPageRanges = 0U;
        }
        else
        {
          structure3.lpPageRanges = IntPtr.Zero;
          structure3.nMaxPageRanges = 0U;
          structure3.Flags |= 8U;
        }
        if (this.mDialogOwner.PrintQueue != null)
        {
          structure3.hDevNames = this.InitializeDevNames(this.mDialogOwner.PrintQueue.FullName);
          if (this.mDialogOwner.PrintTicket != null)
            structure3.hDevMode = this.InitializeDevMode(this.mDialogOwner.PrintQueue.FullName, this.mDialogOwner.PrintTicket);
        }
        ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (NativeMethods.PRINTDLGEX64)));
        Marshal.StructureToPtr<NativeMethods.PRINTDLGEX64>(structure3, ptr, false);
        return ptr;
      }
      catch (Exception ex)
      {
        this.DeallocatePrintDlgExStruct(ptr);
        throw;
      }
    }

    private void DeallocatePrintDlgExStruct(IntPtr ptr)
    {
      if (!(ptr != IntPtr.Zero))
        return;
      IntPtr zero1 = IntPtr.Zero;
      IntPtr zero2 = IntPtr.Zero;
      IntPtr zero3 = IntPtr.Zero;
      IntPtr hDevMode;
      IntPtr hDevNames;
      IntPtr lpPageRanges;
      if (this.Is64Bits())
      {
        NativeMethods.PRINTDLGEX64 structure = (NativeMethods.PRINTDLGEX64) Marshal.PtrToStructure(ptr, typeof (NativeMethods.PRINTDLGEX64));
        hDevMode = structure.hDevMode;
        hDevNames = structure.hDevNames;
        lpPageRanges = structure.lpPageRanges;
      }
      else
      {
        NativeMethods.PRINTDLGEX32 structure = (NativeMethods.PRINTDLGEX32) Marshal.PtrToStructure(ptr, typeof (NativeMethods.PRINTDLGEX32));
        hDevMode = structure.hDevMode;
        hDevNames = structure.hDevNames;
        lpPageRanges = structure.lpPageRanges;
      }
      if (hDevMode != IntPtr.Zero)
        NativeMethods.GlobalFree(hDevMode);
      if (hDevNames != IntPtr.Zero)
        NativeMethods.GlobalFree(hDevNames);
      if (lpPageRanges != IntPtr.Zero)
        NativeMethods.GlobalFree(lpPageRanges);
      Marshal.FreeHGlobal(ptr);
    }

    private PrintQueue FindPrintQueue(string printerName)
    {
      EnumeratedPrintQueueTypes[] enumerationFlag = new EnumeratedPrintQueueTypes[2]
      {
        EnumeratedPrintQueueTypes.Local,
        EnumeratedPrintQueueTypes.Connections
      };
      PrintQueueIndexedProperty[] propertiesFilter = new PrintQueueIndexedProperty[2]
      {
        PrintQueueIndexedProperty.Name,
        PrintQueueIndexedProperty.QueueAttributes
      };
      Helpers.SecurityAssert();
      try
      {
        using (LocalPrintServer localPrintServer = new LocalPrintServer())
        {
          foreach (PrintQueue printQueue in localPrintServer.GetPrintQueues(propertiesFilter, enumerationFlag))
          {
            if (printerName.Equals(printQueue.FullName, StringComparison.OrdinalIgnoreCase))
            {
              printQueue.InPartialTrust = true;
              return printQueue;
            }
          }
        }
      }
      finally
      {
        Helpers.SecurityRevert();
      }
      return (PrintQueue) null;
    }

    private PrintTicket FindPrintTicket(IntPtr dModeHnd, string printQueueName)
    {
      byte[] numArray = (byte[]) null;
      IntPtr num = IntPtr.Zero;
      try
      {
        num = NativeMethods.GlobalLock(dModeHnd);
        NativeMethods.DEVMODE structure = (NativeMethods.DEVMODE) Marshal.PtrToStructure(num, typeof (NativeMethods.DEVMODE));
        numArray = new byte[(int) structure.dmSize + (int) structure.dmDriverExtra];
        Marshal.Copy(num, numArray, 0, numArray.Length);
      }
      finally
      {
        if (num != IntPtr.Zero)
          NativeMethods.GlobalUnlock(dModeHnd);
      }
      Helpers.SecurityAssert();
      try
      {
        using (PrintTicketConverter printTicketConverter = new PrintTicketConverter(printQueueName, PrintTicketConverter.MaxPrintSchemaVersion))
          return printTicketConverter.ConvertDevModeToPrintTicket(numArray);
      }
      finally
      {
        Helpers.SecurityRevert();
      }
    }

    private IntPtr InitializeDevMode(string printerName, PrintTicket printTicket)
    {
      Helpers.SecurityAssert();
      byte[] source = (byte[]) null;
      try
      {
        using (PrintTicketConverter printTicketConverter = new PrintTicketConverter(printerName, PrintTicketConverter.MaxPrintSchemaVersion))
          source = printTicketConverter.ConvertPrintTicketToDevMode(printTicket, BaseDevModeType.UserDefault);
      }
      finally
      {
        Helpers.SecurityRevert();
      }
      IntPtr destination = Marshal.AllocHGlobal(source.Length);
      Marshal.Copy(source, 0, destination, source.Length);
      return destination;
    }

    private IntPtr InitializeDevNames(string printerName)
    {
      IntPtr zero = IntPtr.Zero;
      char[] charArray = printerName.ToCharArray();
      IntPtr ptr = Marshal.AllocHGlobal((charArray.Length + 3) * Marshal.SystemDefaultCharSize + Marshal.SizeOf(typeof (NativeMethods.DEVNAMES)));
      ushort num = (ushort) Marshal.SizeOf(typeof (NativeMethods.DEVNAMES));
      NativeMethods.DEVNAMES structure;
      structure.wDeviceOffset = (ushort) ((uint) num / (uint) Marshal.SystemDefaultCharSize);
      structure.wDriverOffset = (ushort) ((int) structure.wDeviceOffset + charArray.Length + 1);
      structure.wOutputOffset = (ushort) ((uint) structure.wDriverOffset + 1U);
      structure.wDefault = (ushort) 0;
      Marshal.StructureToPtr<NativeMethods.DEVNAMES>(structure, ptr, false);
      IntPtr destination1 = (IntPtr) ((long) ptr + (long) num);
      IntPtr destination2 = (IntPtr) ((long) destination1 + (long) (charArray.Length * Marshal.SystemDefaultCharSize));
      byte[] source = new byte[3 * Marshal.SystemDefaultCharSize];
      Array.Clear((Array) source, 0, source.Length);
      Marshal.Copy(charArray, 0, destination1, charArray.Length);
      Marshal.Copy(source, 0, destination2, source.Length);
      return ptr;
    }

    private uint GetResultPrintDlgExHnd(IntPtr ptrPrintDlg)
    {
      return this.Is64Bits() ? ((NativeMethods.PRINTDLGEX64) Marshal.PtrToStructure(ptrPrintDlg, typeof (NativeMethods.PRINTDLGEX64))).dwResultAction : ((NativeMethods.PRINTDLGEX32) Marshal.PtrToStructure(ptrPrintDlg, typeof (NativeMethods.PRINTDLGEX32))).dwResultAction;
    }

    private void GetSettings(
      IntPtr nativeBuffer,
      out string printerName,
      out uint flags,
      out PageRange pageRange,
      out IntPtr dModeHnd)
    {
      IntPtr zero1 = IntPtr.Zero;
      IntPtr zero2 = IntPtr.Zero;
      IntPtr hDevNames;
      IntPtr lpPageRanges;
      if (this.Is64Bits())
      {
        NativeMethods.PRINTDLGEX64 structure = (NativeMethods.PRINTDLGEX64) Marshal.PtrToStructure(nativeBuffer, typeof (NativeMethods.PRINTDLGEX64));
        dModeHnd = structure.hDevMode;
        hDevNames = structure.hDevNames;
        flags = structure.Flags;
        lpPageRanges = structure.lpPageRanges;
      }
      else
      {
        NativeMethods.PRINTDLGEX32 structure = (NativeMethods.PRINTDLGEX32) Marshal.PtrToStructure(nativeBuffer, typeof (NativeMethods.PRINTDLGEX32));
        dModeHnd = structure.hDevMode;
        hDevNames = structure.hDevNames;
        flags = structure.Flags;
        lpPageRanges = structure.lpPageRanges;
      }
      if (((int) flags & 2) == 2 && lpPageRanges != IntPtr.Zero)
      {
        NativeMethods.PRINTPAGERANGE structure = (NativeMethods.PRINTPAGERANGE) Marshal.PtrToStructure(lpPageRanges, typeof (NativeMethods.PRINTPAGERANGE));
        pageRange = new PageRange((int) structure.nFromPage, (int) structure.nToPage);
      }
      else
        pageRange = new PageRange(1);
      if (hDevNames != IntPtr.Zero)
      {
        IntPtr ptr = IntPtr.Zero;
        try
        {
          ptr = NativeMethods.GlobalLock(hDevNames);
          NativeMethods.DEVNAMES structure = (NativeMethods.DEVNAMES) Marshal.PtrToStructure(ptr, typeof (NativeMethods.DEVNAMES));
          printerName = Marshal.PtrToStringAuto((IntPtr) ((long) ptr + (long) ((int) structure.wDeviceOffset * Marshal.SystemDefaultCharSize)));
        }
        finally
        {
          if (ptr != IntPtr.Zero)
            NativeMethods.GlobalUnlock(hDevNames);
        }
      }
      else
        printerName = string.Empty;
    }

    private uint GetResult()
    {
      if (this.mPrintDlgExHnd == IntPtr.Zero)
        return 0;
      uint resultPrintDlgExHnd = this.GetResultPrintDlgExHnd(this.mPrintDlgExHnd);
      if (resultPrintDlgExHnd == 1U || resultPrintDlgExHnd == 2U)
      {
        string printerName;
        uint flags;
        PageRange pageRange;
        IntPtr dModeHnd;
        this.GetSettings(this.mPrintDlgExHnd, out printerName, out flags, out pageRange, out dModeHnd);
        this.mDialogOwner.PrintQueue = this.FindPrintQueue(printerName);
        this.mDialogOwner.PrintTicket = this.FindPrintTicket(dModeHnd, printerName);
        if (((int) flags & 2) == 2)
        {
          if (pageRange.PageFrom > pageRange.PageTo)
          {
            int pageTo = pageRange.PageTo;
            pageRange.PageTo = pageRange.PageFrom;
            pageRange.PageFrom = pageTo;
          }
          this.mDialogOwner.PageRangeSelection = PageRangeSelection.UserPages;
          this.mDialogOwner.PageRange = pageRange;
          return resultPrintDlgExHnd;
        }
        this.mDialogOwner.PageRangeSelection = PageRangeSelection.AllPages;
      }
      return resultPrintDlgExHnd;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    private void Dispose(bool disposing)
    {
      if (!disposing || !(this.mPrintDlgExHnd != IntPtr.Zero))
        return;
      this.DeallocatePrintDlgExStruct(this.mPrintDlgExHnd);
      this.mPrintDlgExHnd = IntPtr.Zero;
    }
  }
}
