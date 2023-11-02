using System;

namespace WpfApp1
{

    public class FileModel
    {
        private string filename = null;
        private string filepath = null;
        private bool isFile = true;
        private DateTime writeTime = new DateTime();
        private long filesize = 0;
        private DateTime createTime = new DateTime();
        private DateTime lastAccessTime = new DateTime();
        private bool isEnrich = false;

        public FileModel()
        {

        }

        public FileModel(string filename, string filepath, bool isFile)
        {
            this.filename = filename;
            this.filepath = filepath;
            this.isFile = isFile;
        }


        public string FileName { get => filename; set => filename = value; }
        public string FilePath { get => filepath; set => filepath = value; }
        public long FileSize { get => filesize; set => filesize = value; }
        public DateTime CreateTime { get => createTime; set => createTime = value; }
        public DateTime LastAccessTime { get => lastAccessTime; set => lastAccessTime = value; }
        public DateTime WriteTime { get => writeTime; set => writeTime = value; }
        public bool IsFile { get => isFile; set => isFile = value; }
        public bool IsEnrich { get => isEnrich; set => isEnrich = value; }

        public override bool Equals(object obj)
        {
            var model = obj as FileModel;
            return model != null &&
                   filename == model.filename &&
                   filepath == model.filepath &&
                   filesize == model.filesize &&
                   createTime == model.createTime &&
                   lastAccessTime == model.lastAccessTime &&
                   writeTime == model.writeTime;
        }
    }
}
