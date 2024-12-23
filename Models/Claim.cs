﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CMCSApp1.Models
{
    public class Claim
    {
        public int Id { get; set; }

        // Foreign key for Lecturer
        public int LecturerId { get; set; } // Foreign key property
        public Lecturer Lecturer { get; set; } // Navigation property

        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }

        // Auto-calculated total payment
        public double TotalPayment
        {
            get { return HoursWorked * HourlyRate; }
        }

        public string Notes { get; set; }
        public ClaimStatus Status { get; set; } = ClaimStatus.PendingVerification; // Default status

        public DateTime DateSubmitted { get; set; } = DateTime.UtcNow; // Auto-set upon submission
        public DateTime? DateReviewed { get; set; } // Nullable for unreviewed claims

        public List<SupportingDocument> Documents { get; set; } = new List<SupportingDocument>();

        // Method to generate the invoice in HTML format
        public string GenerateInvoice()
        {
            // Initialize StringBuilder to construct the invoice HTML
            var sb = new StringBuilder();

            sb.AppendLine("<html><body>");
            sb.AppendLine("<h1>Invoice for Claim</h1>");

            // Check if the Lecturer object is null
            if (Lecturer == null)
            {
                sb.AppendLine("<p>Error: Lecturer information is missing.</p>");
            }
            else
            {
                // Generate the lecturer details if available
                sb.AppendLine($"<h2>Lecturer: {Lecturer.Name ?? "N/A"}</h2>"); // Handle missing Lecturer.Name
                sb.AppendLine($"<p><strong>Lecturer ID:</strong> {Lecturer.Id}</p>");
            }

            sb.AppendLine($"<p><strong>Claim ID:</strong> {Id}</p>");
            sb.AppendLine($"<p><strong>Hours Worked:</strong> {HoursWorked}</p>");
            sb.AppendLine($"<p><strong>Hourly Rate:</strong> ${HourlyRate}</p>");
            sb.AppendLine($"<p><strong>Total Payment:</strong> ${TotalPayment}</p>");
            sb.AppendLine($"<p><strong>Claim Status:</strong> {Status}</p>");
            sb.AppendLine($"<p><strong>Notes:</strong> {Notes}</p>");
            sb.AppendLine($"<p><strong>Date Submitted:</strong> {DateSubmitted.ToString("yyyy-MM-dd HH:mm")}</p>");
            sb.AppendLine($"<p><strong>Date Reviewed:</strong> {(DateReviewed.HasValue ? DateReviewed.Value.ToString("yyyy-MM-dd HH:mm") : "N/A")}</p>");
            sb.AppendLine("<p>Invoice generated by the Contract Monthly Claim System.</p>");
            sb.AppendLine("</body></html>");

            return sb.ToString(); // Return the HTML content of the invoice
        }
    }

    public enum ClaimStatus
    {
        PendingVerification, // For initial verification
        PendingApproval,     // After verification, awaiting approval
        Approved,            // Approved by coordinator/manager
        Rejected             // Rejected after review
    }
}
