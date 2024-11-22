using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMCSApp1.Models;
using CMCSApp1.Services;
using System.Linq;

namespace CMCSApp1.Controllers
{
    public class CoordinatorController : Controller
    {
        private readonly CMCSContext _context;
        private readonly ClaimVerificationService _claimVerificationService;

        // Constructor injection for ClaimVerificationService
        public CoordinatorController(CMCSContext context, ClaimVerificationService claimVerificationService)
        {
            _context = context;
            _claimVerificationService = claimVerificationService;
        }

        // Action for viewing the pending claims
        public IActionResult PendingClaims()
        {
            var pendingClaims = _context.Claims
                                        .Include(c => c.Lecturer) // Load Lecturer information along with Claim
                                        .Where(c => c.Status == ClaimStatus.PendingVerification)
                                        .ToList();

            return View(pendingClaims);
        }

        [HttpPost]
        public IActionResult ApproveClaim(int claimId)
        {
            // Retrieve the claim by its ID with Lecturer data
            var claim = _context.Claims
                                .Include(c => c.Lecturer) // Ensure Lecturer is loaded
                                .FirstOrDefault(c => c.Id == claimId);

            // Check if the claim is found
            if (claim != null)
            {
                // Validate the claim using the ClaimVerificationService
                var validationMessage = _claimVerificationService.ValidateClaim(claim);

                if (validationMessage != null)
                {
                    // Store the validation message in TempData to display in the view
                    TempData["ClaimWarningMessage"] = validationMessage;
                    return RedirectToAction("PendingClaims");
                }

                // If validation passes, approve the claim
                claim.Status = ClaimStatus.Approved;
                _context.SaveChanges();

                // Generate the invoice after the claim is approved
                var invoiceHtml = claim.GenerateInvoice();

                // Store the generated invoice in TempData to pass it to the ViewInvoice action
                TempData["InvoiceHtml"] = invoiceHtml;

                // Redirect to the ViewInvoice action
                return RedirectToAction("ViewInvoice", new { claimId = claim.Id });
            }

            return RedirectToAction("PendingClaims");
        }

        [HttpPost]
        public IActionResult RejectClaim(int claimId)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.Id == claimId);
            if (claim != null)
            {
                // Reject the claim directly if no validation is needed
                claim.Status = ClaimStatus.Rejected;
                _context.SaveChanges();
            }

            return RedirectToAction("PendingClaims");
        }

        // Action for viewing the generated invoice
        public IActionResult ViewInvoice(int claimId)
        {
            // Retrieve the claim by its ID with Lecturer details included
            var claim = _context.Claims.Include(c => c.Lecturer).FirstOrDefault(c => c.Id == claimId);

            if (claim == null)
            {
                return NotFound(); // Return 404 if the claim is not found
            }

            // Check if there's an invoice HTML stored in TempData
            if (TempData["InvoiceHtml"] != null)
            {
                // Pass the invoice HTML content to the view
                ViewData["InvoiceHtml"] = TempData["InvoiceHtml"];
            }
            else
            {
                // If no invoice HTML is available, return an error message
                ViewData["InvoiceHtml"] = "Invoice generation failed. Please try again.";
            }

            return View();
        }
    }
}
