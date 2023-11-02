using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using static WpfApp1.WinApi;

namespace WpfApp1
{

    /// <summary>
    /// 这个类用来读取usn日志
    /// </summary>
    class UsnJournal
    {
        /// <summary>
        /// 字段注释见构造函数 UsnJournal(string DriveName)
        /// </summary>
        public IntPtr rootHandle;
        public IntPtr receiveBuffer;
        public int receiveBufferSize = 65536;
        public string DriveName;
        public IntPtr zero = IntPtr.Zero;
        WinApi.USN_JOURNAL_DATA usnData = new WinApi.USN_JOURNAL_DATA();
        WinApi.MFT_ENUM_DATA mftData = new WinApi.MFT_ENUM_DATA();
        /// <summary>
        /// 根据磁盘名称初始化
        /// </summary>
        /// <param name="DriveName"></param>
        public UsnJournal(string DriveName)
        {
            this.DriveName = DriveName;
            rootHandle = GetRootHandle(DriveName);//得到磁盘句柄
            receiveBuffer = Marshal.AllocHGlobal(receiveBufferSize);//分配接收缓冲区
            GetUsnData(ref usnData);//获取USN日志属性信息（不是内容）
            mftData.StartFileReferenceNumber = 0;//起始文件编号
            mftData.LowUsn = 0;//起始usn
            mftData.HighUsn = usnData.NextUsn;//终止usn

            USN_JOURNAL_DATA usnJournalState = new USN_JOURNAL_DATA();
            int sizeUsnJournalState = Marshal.SizeOf(usnJournalState);
            IntPtr outBufferpPtr = Marshal.AllocHGlobal(sizeUsnJournalState);
            if (QueryUsnJournal(outBufferpPtr, sizeUsnJournalState) != UsnJournalReturnCode.USN_JOURNAL_SUCCESS)//检查usn日志是否启用
                CreateUsnJournal(1000 * 1024, 16 * 1024);//创建usn日志
            Marshal.FreeHGlobal(outBufferpPtr);
        }

        public UsnJournal()
        {

        }
        /// <summary>
        /// 获取USN日志内容，并从中解析出文件名与完整的文件路径，保存在参数集合中
        /// </summary>
        /// <param name="files"></param>
        public void GetFiles(BlockingCollection<FileModel> files)
        {
            WinApi.USN_RECORD record = new WinApi.USN_RECORD();
            IntPtr mftPtr = Marshal.AllocHGlobal(Marshal.SizeOf(mftData));
            Marshal.StructureToPtr(mftData, mftPtr, true);

            int retBytes = 0;
            int cb = 0;
            Dictionary<long, FSNode> FSNodes = new Dictionary<long, FSNode>(100000);

            string FileName;
            do
            {
                if (WinApi.DeviceIoControl(rootHandle, WinApi.FSCTL_ENUM_USN_DATA, mftPtr, Marshal.SizeOf(mftData), receiveBuffer, receiveBufferSize, out retBytes, IntPtr.Zero))
                {
                    cb = retBytes;
                    IntPtr recPtr = new IntPtr(receiveBuffer.ToInt64() + 8);
                    while (retBytes > 64)
                    {
                        record = (WinApi.USN_RECORD)Marshal.PtrToStructure(recPtr, typeof(WinApi.USN_RECORD));
                        FileName = Marshal.PtrToStringUni(new IntPtr(recPtr.ToInt64() + record.FileNameOffset), record.FileNameLength / 2);
                        bool IsFile = !record.FileAttributes.HasFlag(FileAttributes.Directory);
                        long lastWriteTime = record.TimeStamp;
                        FSNodes.Add(record.FileReferenceNumber, new FSNode(record.FileReferenceNumber, record.ParentFileReferenceNumber, FileName, IsFile, lastWriteTime));
                        recPtr = new IntPtr(recPtr.ToInt64() + record.RecordLength);
                        retBytes -= record.RecordLength;
                    }
                    Marshal.WriteInt64(mftPtr, Marshal.ReadInt64(receiveBuffer, 0));
                }
                else
                {
                    break;
                }

            } while (cb > 8);
            string path;
            foreach (FSNode FSNode in FSNodes.Values)
            {
                path = FSNode.FileName;
                FSNode ParentFSNode = FSNode;
                while (FSNodes.TryGetValue(ParentFSNode.ParentFRN, out ParentFSNode))
                {
                    path = string.Concat(ParentFSNode.FileName, "\\", path);
                }
                path = string.Concat(DriveName, path);
                FileModel file = new FileModel(FSNode.FileName, path, FSNode.IsFile);
                files.Add(file);
            }
            FSNodes.Clear();
            GC.Collect();
            Cleanup();
            Marshal.FreeHGlobal(mftPtr);
            Marshal.FreeHGlobal(receiveBuffer);
        }
        /// <summary>
        /// 查询USN日志状态
        /// </summary>
        /// <param name="usnJournalState"></param>
        /// <param name="sizeUsnJournalState"></param>
        /// <returns></returns>
        private UsnJournalReturnCode QueryUsnJournal(IntPtr usnJournalState, int sizeUsnJournalState)
        {
            UsnJournalReturnCode usnReturnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
            Int32 cb;
            bool fOk = WinApi.DeviceIoControl(
                rootHandle,
                WinApi.FSCTL_QUERY_USN_JOURNAL,
                IntPtr.Zero,
                0,
                usnJournalState,
                sizeUsnJournalState,
                out cb,
                IntPtr.Zero);
            if (!fOk)
            {
                int lastWin32Error = Marshal.GetLastWin32Error();
                usnReturnCode = ConvertWin32ErrorToUsnError((WinApi.GetLastErrorEnum)Marshal.GetLastWin32Error());
            }

            return usnReturnCode;
        }
        /// <summary>
        /// Win32API错误信息转化
        /// </summary>
        /// <param name="Win32LastError"></param>
        /// <returns></returns>
        private UsnJournalReturnCode ConvertWin32ErrorToUsnError(WinApi.GetLastErrorEnum Win32LastError)
        {
            UsnJournalReturnCode usnRtnCode;
            switch (Win32LastError)
            {
                case WinApi.GetLastErrorEnum.ERROR_JOURNAL_NOT_ACTIVE:
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_NOT_ACTIVE;
                    break;
                case WinApi.GetLastErrorEnum.ERROR_SUCCESS:
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_SUCCESS;
                    break;
                case WinApi.GetLastErrorEnum.ERROR_HANDLE_EOF:
                    usnRtnCode = UsnJournalReturnCode.ERROR_HANDLE_EOF;
                    break;
                default:
                    usnRtnCode = UsnJournalReturnCode.USN_JOURNAL_ERROR;
                    break;
            }
            return usnRtnCode;
        }
        /// <summary>
        /// 创建USN日志
        /// </summary>
        /// <param name="maxSize"></param>
        /// <param name="allocationDelta"></param>
        public void CreateUsnJournal(ulong maxSize, ulong allocationDelta)
        {
            Int32 cb;
            WinApi.CREATE_USN_JOURNAL_DATA cujd = new WinApi.CREATE_USN_JOURNAL_DATA();
            cujd.MaximumSize = maxSize;
            cujd.AllocationDelta = allocationDelta;
            int sizeCujd = Marshal.SizeOf(cujd);
            IntPtr cujdBuffer = Marshal.AllocHGlobal(sizeCujd);
            WinApi.ZeroMemory(cujdBuffer, sizeCujd);
            Marshal.StructureToPtr(cujd, cujdBuffer, true);

            bool fOk = WinApi.DeviceIoControl(
                rootHandle,
                WinApi.FSCTL_CREATE_USN_JOURNAL,
                cujdBuffer,
                sizeCujd,
                IntPtr.Zero,
                0,
                out cb,
                IntPtr.Zero);

            Marshal.FreeHGlobal(cujdBuffer);


        }
        /// <summary>
        /// 获取关于USN日志的信息
        /// </summary>
        /// <param name="UsnData"></param>
        private void GetUsnData(ref WinApi.USN_JOURNAL_DATA UsnData)
        {
            int sizeUsnJournalState = Marshal.SizeOf(UsnData);
            int cb;
            IntPtr usnPtr = Marshal.AllocHGlobal(Marshal.SizeOf(UsnData));
            Marshal.StructureToPtr(usnData, usnPtr, true);
            bool fOk = WinApi.DeviceIoControl(
                rootHandle,
                WinApi.FSCTL_QUERY_USN_JOURNAL,
                IntPtr.Zero,
                0,
                usnPtr,
                sizeUsnJournalState,
                out cb,
                IntPtr.Zero);
            UsnData = (WinApi.USN_JOURNAL_DATA)Marshal.PtrToStructure(usnPtr, typeof(WinApi.USN_JOURNAL_DATA));
            //WinApi.CloseHandle(usnPtr);
            Marshal.FreeHGlobal(usnPtr);

        }
        /// <summary>
        /// 关闭句柄
        /// </summary>
        public void Cleanup()
        {
            if (rootHandle != IntPtr.Zero)
            {
                WinApi.CloseHandle(rootHandle);
                rootHandle = WinApi.INVALID_HANDLE_VALUE;
            }

            if (receiveBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(receiveBuffer);
                receiveBuffer = IntPtr.Zero;
            }

        }
        /// <summary>
        /// 获取磁盘句柄
        /// </summary>
        /// <param name="drivesName"></param>
        /// <returns></returns>
        public IntPtr GetRootHandle(string drivesName)
        {
            drivesName = drivesName.Trim('\\');
            IntPtr rootHandle = WinApi.CreateFile("\\\\.\\" + drivesName, WinApi.GENERIC_READ, WinApi.FILE_SHARE_READ | WinApi.FILE_SHARE_WRITE, IntPtr.Zero, WinApi.OPEN_EXISTING, 0, IntPtr.Zero);
            return rootHandle;
        }
        /// <summary>
        /// 根据文件路径获取文件句柄
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IntPtr GetFileHandle(string filePath)
        {
            IntPtr rootHandle = WinApi.CreateFile(filePath, WinApi.GENERIC_READ, WinApi.FILE_SHARE_READ | WinApi.FILE_SHARE_WRITE, IntPtr.Zero, WinApi.OPEN_EXISTING, 0, IntPtr.Zero);
            return rootHandle;
        }
        /// <summary>
        /// 临时文件数据模型
        /// </summary>
        private class FSNode
        {
            public long FRN;
            public long ParentFRN;
            public string FileName;
            public long LastWriteTime;
            public bool IsFile;
            public FSNode(long lFRN, long lParentFSN, string sFileName, bool bIsFile, long lastWriteTime)
            {
                FRN = lFRN;
                ParentFRN = lParentFSN;
                FileName = sFileName;
                IsFile = bIsFile;
                LastWriteTime = lastWriteTime;
            }

        }
    }
}
