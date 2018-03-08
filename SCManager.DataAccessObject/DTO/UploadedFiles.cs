using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCManager.DataAccessObject.DTO
{
    public class UploadedFiles
    {
        public string FileStatus { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public int RecordCount { get; set; }
        public Guid ID { get; set; }
        public LogDetails logDetails { get; set; }
        public Common CommonObj { get; set; }
        public string CreatedDate { get; set; }
        public int RemovedDataCount { get; set; }
        public List<ImportForm8> Form8List { get; set; }
    }
}