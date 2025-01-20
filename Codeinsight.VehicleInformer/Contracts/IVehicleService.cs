using Codeinsight.VehicleInformer.DTOs;

namespace Codeinsight.VehicleInformer.Contracts
{
    public interface IVehicleService
    {
        void GenerateVehicleReport();
        void DisplayVehicleReportInTabular();
        IList <CarDto> SearchVehicleByModel();
        IList <CarDto> FilterVehiclesByManufacturingYear();
        IList <CarDto> SortVehiclesByPrice();
        IList<AverageRatingDto> VehiclesAvergeRating();
        IList <CarDto> CountVehiclesBasedOnRating();
    }
}
