using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WpfApp1
{
    /// <summary>
    /// 保存本程序运行过程中的所有数据
    /// </summary>
    public class DataSource
    {
        /// <summary>
        /// 计算机中所有NTFS磁盘
        /// </summary>
        public static DriveInfo[] allDrives = GetDrives();
        /// <summary>
        /// 并发集合，保存USN日志内容
        /// </summary>
        private static BlockingCollection<FileModel> files = new BlockingCollection<FileModel>();
        public static BlockingCollection<FileModel> Files { get => files; set => files = value; }
        /// <summary>
        /// 并发集合，保存各个种类文件的数量
        /// </summary>
        private static ConcurrentDictionary<int, int> classificNumber = new ConcurrentDictionary<int, int>();
        public static ConcurrentDictionary<int, int> ClassificNumber { get => classificNumber; set => classificNumber = value; }
        /// <summary>
        /// 临时保存每次查询到的文件
        /// </summary>
        private static List<FileModel> TemporaryList = new List<FileModel>();
        public static List<FileModel> TemporaryList1 { get => TemporaryList; set => TemporaryList = value; }
        /// <summary>
        /// 固定过滤器
        /// </summary>
        private static List<Filter> fixedFilters = GetFixedFilters();
        public static List<Filter> FixedFilters
        {
            get => fixedFilters;
        }
        /// <summary>
        /// 用户自定义过滤器与固定过滤器
        /// </summary>
        private static List<Filter> filters = new List<Filter>(fixedFilters);
        public static List<Filter> Filters
        {
            get
            {
                return filters;
            }
            set => filters = value;
        }

        //static DataSource()
        //{
        //    filters.AddRange(XmlManage.readXml());
        //}

        private static List<Filter> GetFixedFilters()
        {
            List<Filter> fixedFilters = new List<Filter>();
            Filter all = new Filter("全部", new List<string>() { "*" },
                System.Windows.Forms.Application.StartupPath + "\\resource\\1162348.gif");

            Filter picture = new Filter("图片", (from s in new List<string>()
            {
                "WebP", "JPEG", "JPG", "BMP", "PCX", "TIF", "GIF", "TGA", "EXIF",
                "FPX", "SVG","PSD", "CDR", "PCD", "DXF", "UFO", "EPS",
                "AI", "PNG", "HDRI", "RAW", "WMF", "FLIC", "EMF", "ICO"
            }
                                               select s.ToLower()).ToList<string>(),
                 System.Windows.Forms.Application.StartupPath + "\\resource\\1198602.gif");

            Filter video = new Filter("视频", (from s in new List<string>()
            {
                "avi", "rmvb", "rm", "asf", "divx", "mpg", "mpeg", "mpe", "wmv", "mp4", "mkv", "vob", "mov"
            }
                                             select s.ToLower()).ToList<string>(),
                System.Windows.Forms.Application.StartupPath + "\\resource\\1223561.gif");
            Filter music = new Filter("音频", (from s in new List<string>()
            {
                "WAV", "FLAC", "APE", "ALAC", "WavPack", "MP3", "AAC", "OGG", "opus"
            }
                                             select s.ToLower()).ToList<string>(),
                System.Windows.Forms.Application.StartupPath + "\\resource\\1225500.gif");

            Filter txt = new Filter("文档", (from s in new List<string>()
            {
                "txt", "doc", "hlp", "rtf", "pdf", "html", "xml"
            }
                                           select s.ToLower()).ToList<string>(),
                System.Windows.Forms.Application.StartupPath + "\\resource\\1198227.gif");

            Filter zip = new Filter("压缩文件", (from s in new List<string>()
            { "zip","rar"}
                                             select s.ToLower()).ToList<string>(),
                System.Windows.Forms.Application.StartupPath + "\\resource\\1177489.gif");
            fixedFilters.Add(all);
            fixedFilters.Add(picture);
            fixedFilters.Add(video);
            fixedFilters.Add(music);
            fixedFilters.Add(txt);
            fixedFilters.Add(zip);
            return fixedFilters;
        }

        private static DriveInfo[] GetDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed && d.DriveFormat.ToUpper() == "NTFS").ToArray();
            return drives.ToArray();
        }

    }
}
