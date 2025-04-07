using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace LOBR.Components.Pages
{
    public partial class Hrdata
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public LOBRCOnfigurationService LOBRCOnfigurationService { get; set; }

        protected IEnumerable<LOBR.Models.LOBRCOnfiguration.Hrdatum> hrdata;

        protected RadzenDataGrid<LOBR.Models.LOBRCOnfiguration.Hrdatum> grid0;

        [Inject]
        protected SecurityService Security { get; set; }
        protected override async Task OnInitializedAsync()
        {
            hrdata = await LOBRCOnfigurationService.GetHrdata();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddHrdatum>("Add Hrdatum", null);
            await grid0.Reload();
        }

        protected async Task EditRow(LOBR.Models.LOBRCOnfiguration.Hrdatum args)
        {
            await DialogService.OpenAsync<EditHrdatum>("Edit Hrdatum", new Dictionary<string, object> { {"Emplid", args.Emplid} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, LOBR.Models.LOBRCOnfiguration.Hrdatum hrdatum)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await LOBRCOnfigurationService.DeleteHrdatum(hrdatum.Emplid);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Hrdatum"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await LOBRCOnfigurationService.ExportHrdataToCSV(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
                }, "Hrdata");
            }

            if (args == null || args.Value == "xlsx")
            {
                await LOBRCOnfigurationService.ExportHrdataToExcel(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
                }, "Hrdata");
            }
        }
    }
}