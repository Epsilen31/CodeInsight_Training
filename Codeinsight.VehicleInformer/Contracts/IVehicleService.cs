using Codeinsight.VehicleInformer.DTOs;

namespace Codeinsight.VehicleInformer.Contracts
{
    public interface IVehicleService
    {
        void GenerateVehicleReport();
        void DisplayVehicleReportInTabular();
        ICollection <CarDto> SearchVehicleByModel();
        ICollection <CarDto> GetFilterVehiclesByManufacturingYear();
        ICollection <CarDto> GetSortVehiclesByPrice();
        ICollection<AverageRatingDto> GetVehiclesAverageRating();
        ICollection <CarDto> GetCountVehiclesBasedOnRating();
    }
}
