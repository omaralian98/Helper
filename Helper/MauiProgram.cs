global using System.Globalization;

using Helper.Data;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
namespace Helper
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddSingleton(typeof(HelperDatabase<>));
            builder.Services.AddSingleton<OperationRepository>();
            builder.Services.AddSingleton<IncomeRepository>();
            builder.Services.AddSingleton<InventoryRepository>();

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            var app = builder.Build();
            EnsureTriggers(app);
            return app;
        }

        public static void EnsureTriggers(MauiApp app)
        {
            var _dbOperation = app.Services.GetRequiredService<OperationRepository>();
            var _dbIncome = app.Services.GetRequiredService<IncomeRepository>();

            _ = _dbOperation.AddOperationTrigger();
            _ = _dbIncome.AddIncomeTrigger();
        }
    }
}
