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
    public partial class AddHrdatum
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

        protected override async Task OnInitializedAsync()
        {
            hrdatum = new LOBR.Models.LOBRCOnfiguration.Hrdatum();
        }
        protected bool errorVisible;
        protected LOBR.Models.LOBRCOnfiguration.Hrdatum hrdatum;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await LOBRCOnfigurationService.CreateHrdatum(hrdatum);
                DialogService.Close(hrdatum);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}