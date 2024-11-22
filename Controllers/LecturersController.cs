using Microsoft.AspNetCore.Mvc;
using CMCSApp1.Models;
using System.Linq;
using System.IO;

namespace CMCSApp1.Controllers
{
    public class LecturerController : Controller
    {
        private readonly CMCSContext _context;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string[] AllowedExtensions = { ".doc", ".docx" };

        public LecturerController(CMCSContext context)
        {
            _context = context;
        }

        public IActionResult SubmitClaim()
        {
            return View(new Claim());
        }

        [HttpPost]
        public IActionResult SubmitClaim(Claim claim, IFormFile supportingDocument)
        {
            // Validate the supporting document
            if (supportingDocument != null)
            {
                var fileExtension = Path.GetExtension(supportingDocument.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(fileExtension))
                {
                    TempData["ErrorMessage"] = "Only Word documents (.doc, .docx) are allowed.";
                    return View(claim);
                }

                if (supportingDocument.Length > MaxFileSize)
                {
                    TempData["ErrorMessage"] = "File size exceeds the maximum allowed size of 5 MB.";
                    return View(claim);
                }
            }

            // Check if the lecturer already exists in the database
            var lecturer = _context.Lecturers.FirstOrDefault(l => l.Name == claim.Lecturer.Name);
            if (lecturer == null)
            {
                lecturer = new Lecturer { Name = claim.Lecturer.Name };
                _context.Lecturers.Add(lecturer);
                _context.SaveChanges(); // Save to generate an ID for the lecturer
            }

            // Associate the lecturer with the claim and set the foreign key
            claim.LecturerId = lecturer.Id;
            claim.Lecturer = lecturer;

            // Set the claim status to pending
            claim.Status = ClaimStatus.PendingVerification;

            // If there's a supporting document, store it
            if (supportingDocument != null)
            {
                var document = new SupportingDocument
                {
                    FileName = supportingDocument.FileName,
                    Claim = claim
                };

                using (var stream = new MemoryStream())
                {
                    supportingDocument.CopyTo(stream);
                    document.FileData = stream.ToArray();
                }

                claim.Documents.Add(document);
            }

            // Add the claim to the database and save
            _context.Claims.Add(claim);
            _context.SaveChanges();

            // Redirect to a confirmation page
            return RedirectToAction("ClaimSubmitted");
        }

        public IActionResult ClaimSubmitted()
        {
            return View();
        }
    }
}
