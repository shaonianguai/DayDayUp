using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{

    /// <summary>
    /// 本程序所需的数据结构以及Win32API
    /// </summary>
    class WinApi
    {
        public static IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        public const uint GENERIC_READ = 0x80000000;
        public const int FILE_SHARE_READ = 0x1;
        public const int FILE_SHARE_WRITE = 0x2;
        public const int OPEN_EXISTING = 3;
        public const int FILE_READ_ATTRIBUTES = 0x80;
        public const int FILE_NAME_IINFORMATION = 9;
        public const int FILE_FLAG_BACKUP_SEMANTICS = 0x2000000;
        public const int FILE_OPEN_FOR_BACKUP_INTENT = 0x4000;
        public const int FILE_OPEN_BY_FILE_ID = 0x2000;
        public const int FILE_OPEN = 0x1;
        public const int OBJ_CASE_INSENSITIVE = 0x40;
        public const int FSCTL_ENUM_USN_DATA = 0x900b3;

        public const int FILE_DEVICE_FILE_SYSTEM = 0x00000009;
        public const int METHOD_NEITHER = 3;
        public const int METHOD_BUFFERED = 0;
        public const int FILE_ANY_ACCESS = 0;
        public const int FILE_SPECIAL_ACCESS = 0;
        public const int FILE_READ_ACCESS = 1;
        public const int FILE_WRITE_ACCESS = 2;
        public const int FSCTL_QUERY_USN_JOURNAL = (FILE_DEVICE_FILE_SYSTEM << 16) | (FILE_ANY_ACCESS << 14) | (61 << 2) | METHOD_BUFFERED;
        public const UInt32 FSCTL_CREATE_USN_JOURNAL = (FILE_DEVICE_FILE_SYSTEM << 16) | (FILE_ANY_ACCESS << 14) | (57 << 2) | METHOD_NEITHER;

        [StructLayout(LayoutKind.Sequential)]
        public struct MFT_ENUM_DATA
        {
            public long StartFileReferenceNumber;
            public long LowUsn;
            public long HighUsn;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct CREATE_USN_JOURNAL_DATA
        {
            public UInt64 MaximumSize;
            public UInt64 AllocationDelta;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct USN_RECORD
        {
            public int RecordLength;
            public short MajorVersion;
            public short MinorVersion;
            public long FileReferenceNumber;
            public long ParentFileReferenceNumber;
            public long Usn;
            public long TimeStamp;
            public int Reason;
            public int SourceInfo;
            public int SecurityId;
            public FileAttributes FileAttributes;
            public short FileNameLength;
            public short FileNameOffset;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct USN_JOURNAL_DATA
        {
            public UInt64 UsnJournalID;
            public Int64 FirstUsn;
            public Int64 NextUsn;
            public Int64 LowestValidUsn;
            public Int64 MaxUsn;
            public UInt64 MaximumSize;
            public UInt64 AllocationDelta;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct WIN32_FILE_ATTRIBUTE_DATA
        {
            public int dwFileAttributes;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
            public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
            public int nFileSizeHigh;
            public int nFileSizeLow;
        }

        public enum GET_FILEEX_INFO_LEVELS
        {
            GetFileExInfoStandard = 0,
            GetFileExMaxInfoLevel = 1
        }
        public enum UsnJournalReturnCode
        {
            INVALID_HANDLE_VALUE = -1,
            USN_JOURNAL_SUCCESS = 0,
            ERROR_INVALID_FUNCTION = 1,
            ERROR_FILE_NOT_FOUND = 2,
            ERROR_PATH_NOT_FOUND = 3,
            ERROR_TOO_MANY_OPEN_FILES = 4,
            ERROR_ACCESS_DENIED = 5,
            ERROR_INVALID_HANDLE = 6,
            ERROR_INVALID_DATA = 13,
            ERROR_HANDLE_EOF = 38,
            ERROR_NOT_SUPPORTED = 50,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_JOURNAL_DELETE_IN_PROGRESS = 1178,
            USN_JOURNAL_NOT_ACTIVE = 1179,
            ERROR_JOURNAL_ENTRY_DELETED = 1181,
            ERROR_INVALID_USER_BUFFER = 1784,
            USN_JOURNAL_INVALID = 17001,
            VOLUME_NOT_NTFS = 17003,
            INVALID_FILE_REFERENCE_NUMBER = 17004,
            USN_JOURNAL_ERROR = 17005
        }

        public enum GetLastErrorEnum
        {
            INVALID_HANDLE_VALUE = -1,
            ERROR_SUCCESS = 0,
            ERROR_INVALID_FUNCTION = 1,
            ERROR_FILE_NOT_FOUND = 2,
            ERROR_PATH_NOT_FOUND = 3,
            ERROR_TOO_MANY_OPEN_FILES = 4,
            ERROR_ACCESS_DENIED = 5,
            ERROR_INVALID_HANDLE = 6,
            ERROR_INVALID_DATA = 13,
            ERROR_HANDLE_EOF = 38,
            ERROR_NOT_SUPPORTED = 50,
            ERROR_INVALID_PARAMETER = 87,
            ERROR_JOURNAL_DELETE_IN_PROGRESS = 1178,
            ERROR_JOURNAL_NOT_ACTIVE = 1179,
            ERROR_JOURNAL_ENTRY_DELETED = 1181,
            ERROR_INVALID_USER_BUFFER = 1784
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, int nInBufferSize, IntPtr lpOutBuffer, int nOutBufferSize, out int lpBytesReturned, IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Int32 CloseHandle(IntPtr lpObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetFileAttributesExA(string lpFileName, GET_FILEEX_INFO_LEVELS fInfoLevelId, ref WIN32_FILE_ATTRIBUTE_DATA lpFileInformation);

        [DllImport("kernel32.dll")]
        public static extern void ZeroMemory(IntPtr ptr, int size);

    }
}
