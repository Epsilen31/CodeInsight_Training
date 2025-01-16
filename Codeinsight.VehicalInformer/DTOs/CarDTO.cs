
namespace Codeinsight.VehicalInformer.DTOs
{
    public class CarDTO
    {
        public required string Model { get; set; }
        public required string Company { get; set; }
        public required string ManufacturingYear { get; set; }
        public required string BasePrice { get; set; }
        public required string InsurencePrice { get; set; }
        public required string AfterTotalPrice { get; set; }
        public required string Rating { get; set; }
    }
}
