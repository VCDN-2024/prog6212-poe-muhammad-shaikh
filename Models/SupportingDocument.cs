namespace CMCSApp1.Models
{
    public class SupportingDocument
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }
    }
}
