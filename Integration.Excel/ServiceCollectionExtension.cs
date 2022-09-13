using Integration.Excel.ExcelSettings;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;

namespace Integration.Excel
{
    public static class ServiceCollectionExtension
    {
        public static void AddExcelProvider(this IServiceCollection services)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            services.AddTransient<IExcelWriter, EpPlusWriter>();
            services.AddTransient<IExcelReader, EpPlusReader>();
        }
    }
}
