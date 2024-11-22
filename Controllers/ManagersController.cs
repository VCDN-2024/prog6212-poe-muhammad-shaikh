using Microsoft.AspNetCore.Mvc;
using CMCSApp1.Models;
using System.Linq;
using CMCSApp1.Models;

namespace CMCSApp1.Controllers
{
    public class ManagerController : Controller
    {
        private readonly CMCSContext _context;

        public ManagerController(CMCSContext context)
        {
            _context = context;
        }

        public IActionResult PendingClaims()
        {
            var claims = _context.Claims.Where(c => c.Status == ClaimStatus.PendingVerification).ToList();
            return View(claims);
        }

        [HttpPost]
        public IActionResult ApproveClaim(int claimId)
        {
            var claim = _context.Claims.Find(claimId);
            if (claim != null)
            {
                claim.Status = ClaimStatus.Approved;
                _context.SaveChanges();
            }
            return RedirectToAction("PendingClaims");
        }

        [HttpPost]
        public IActionResult RejectClaim(int claimId)
        {
            var claim = _context.Claims.Find(claimId);
            if (claim != null)
            {
                claim.Status = ClaimStatus.Rejected;
                _context.SaveChanges();
            }
            return RedirectToAction("PendingClaims");
        }
    }
}
