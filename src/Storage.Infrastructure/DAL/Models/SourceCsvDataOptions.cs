namespace Storage.Infrastructure.DAL.Models
{
    internal sealed class SourceCsvDataOptions
    {
        public string BaseUrl { get; set; }
        public string DirPathForSavingFiles { get; set; }
        public FileOptions ProductFileOptions { get; set; }
        public FileOptions InventoryFileOptions { get; set; }
        public FileOptions PricesFileOptions { get; set; }
    }

    internal sealed class FileOptions
    {
        public string FileName { get; set; }
        public bool HasHeader { get; set; }
        public string Delimiter { get; set; }
    }
}
