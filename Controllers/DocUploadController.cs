using Demo_DocUpload.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace eRehat.Controllers
{
    public class DocUploadController : Controller
    {
        private readonly DocUploadRepository _docUploadRepository;
       
        public DocUploadController(DocUploadRepository docUploadRepository)
        {
            _docUploadRepository = docUploadRepository;
           
        }

        public async Task<IActionResult> frm_booking_docUpload()
        {
            var book_id = 1;
           
            var documents = await _docUploadRepository.GetAllDocumentsAsync(book_id);

            return View(documents); // Returns the Razor view with the list of documents
        }

        [HttpPost]
        public async Task<IActionResult> frm_booking_docUpload(int book_id,string doc_id, IFormFile File)
        {
            
            int id;
            if (File == null || File.Length == 0)
            {

                TempData["Error"] = "Please select a valid file.";                
                 id = book_id; // Example parameter
                return RedirectToAction("frm_booking_docUpload");
            }

            var allowedExtensions = new[] { ".jpg", ".png" };
            var fileExtension = Path.GetExtension(File.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                TempData["Error"] = "Only .jpg and .png files are allowed.";
                id = book_id; // Example parameter
                return RedirectToAction("frm_booking_docUpload");
            }

            if (File.Length > 2 * 1024 * 1024) // Limit file size to 2MB
            {
                TempData["Error"] = "File size cannot exceed 2MB.";
                id = book_id; // Example parameter
                return RedirectToAction("frm_booking_docUpload");
            }

            // Save the file to the server
            var FolderFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/docUploads");
            if (!Directory.Exists(FolderFile))
            {
                Directory.CreateDirectory(FolderFile);
            }

            var filePath = Path.Combine(FolderFile, $"{book_id}_{doc_id}_{File.FileName}");
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }

            // Update the database with the file path
            var success = await _docUploadRepository.UpdateDocumentPathAsync(book_id,doc_id, $"/docUploads/{book_id}_{doc_id}_{File.FileName}");
            if (success)
            {
                TempData["Success"] = "File uploaded successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to update the database.";
            }

            id = book_id; // Example parameter
            return RedirectToAction("frm_booking_docUpload");
        }
    }
}
