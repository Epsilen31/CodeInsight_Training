namespace Codeinsight.VehicleInsights.WebApi.Constants
{
    public static class RouteKey
    {
        public const string GenerateReport = "generateReport";
        public const string DisplayReport = "displayReport";
        public const string SearchByModel = "searchModel/{model}";
        public const string FilterByYear = "filterYear/{year}";
        public const string SortByPrice = "sortPrice/{price}";
        public const string AverageRating = "averageRatings";
        public const string CountByRating = "countRating";
        public const string HeadRoute = "api/[controller]";
    }
}
