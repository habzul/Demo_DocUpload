using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;
namespace eRehat.Models
{
    public class DocUpload
    {
        public int book_id { get; set; }
        public string doc_id { get; set; }
        public string ket_doc { get; set; }
        public string FilePath { get; set; }
        public int FileSize { get; set; }
    }
}
