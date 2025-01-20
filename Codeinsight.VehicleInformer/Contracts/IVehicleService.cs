using Codeinsight.VehicleInformer.DTOs;

namespace Codeinsight.VehicleInformer.Contracts
{
    public interface IVehicleService
    {
        void GenerateVehicleReport();
        void DisplayVehicleReportInTabular();
        ICollection <CarDto> SearchVehicleByModel();
        ICollection <CarDto> FilterVehiclesByManufacturingYear();
        ICollection <CarDto> SortVehiclesByPrice();
        ICollection<AverageRatingDto> VehiclesAvergeRating();
        ICollection <CarDto> CountVehiclesBasedOnRating();
    }
}

