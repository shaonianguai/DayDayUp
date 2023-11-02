using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections.Concurrent;

namespace WpfApp1
{

    class FileParallelQuery
    {
        /// <summary>
        /// 查询文件
        /// </summary>
        /// <param name="startWith"></param>
        /// <param name="inputs"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public IEnumerable<FileModel> SearchMatchFileMethod(string startWith, string inputs, List<string> filters)
        {
            if (string.IsNullOrEmpty(inputs) && string.IsNullOrEmpty(startWith))
                return DataSource.Files;

            if (filters.Contains("*"))
                return DataSource.Files.AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount).
                     Where<FileModel>(r => r.FilePath.StartsWith(startWith) && r.FileName.Contains(inputs));

            return DataSource.Files.AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount).
                Where<FileModel>(r => r.FilePath.StartsWith(startWith) && r.FileName.Contains(inputs) && filters.Contains(Path.GetExtension(r.FileName).Trim('.').ToLower()));

        }
        /// <summary>
        /// 根据正则表达式查询文件
        /// </summary>
        /// <param name="startWith"></param>
        /// <param name="regex"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public IEnumerable<FileModel> SearchMatchFileMethod(string startWith, Regex regex, List<string> filters)
        {
            if (filters.Contains("*"))
                return DataSource.Files.AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount).
                    Where<FileModel>(r => r.FilePath.StartsWith(startWith) && regex.IsMatch(r.FileName));

            return DataSource.Files.AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount).
                Where<FileModel>(r => r.FilePath.StartsWith(startWith) && regex.IsMatch(r.FileName) && filters.Contains(Path.GetExtension(r.FileName).Trim('.').ToLower()));
        }

        /// <summary>
        /// 获取各种类文件的数量
        /// </summary>
        /// <returns></returns>
        public Task GetClassificNumberAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (FileModel r in DataSource.Files.AsParallel())
                {
                    for (int i = 1; i < DataSource.Filters.Count; i++)
                    {
                        if (DataSource.Filters[i].Param.Contains(Path.GetExtension(r.FileName).Trim('.').ToLower()))
                            DataSource.ClassificNumber.AddOrUpdate(i, 0, (k, v) => ++v);
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }
        /// <summary>
        /// 读取所有NTFS磁盘的USN日志
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> InitCollectionFromDisks()
        {
            DataSource.Files = new BlockingCollection<FileModel>(); ;
            var tasks = new List<Task<string>>();
            foreach (DriveInfo drive in DataSource.allDrives)
            {
                var task = QueryUsnDataAsync(drive.Name);
                tasks.Add(task);
            }

            while (tasks.Count > 0)
            {
                var completeTask = Task.WhenAny(tasks).Result;
                tasks.Remove(completeTask);
                yield return completeTask.Result;
            }
            DataSource.Files.CompleteAdding();
        }
        /// <summary>
        /// 根据磁盘名，查询usn日志
        /// </summary>
        /// <param name="driveName"></param>
        /// <returns></returns>
        private Task<string> QueryUsnDataAsync(string driveName)
        {
            return Task.Factory.StartNew(() =>
            {
                UsnJournal usn = new UsnJournal(driveName);
                usn.GetFiles(DataSource.Files);
                return driveName;
            }, TaskCreationOptions.LongRunning);
        }
    }
}
