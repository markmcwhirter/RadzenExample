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
    public partial class EditHrdatum
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

        [Parameter]
        public int Emplid { get; set; }

        protected override async Task OnInitializedAsync()
        {
            hrdatum = await LOBRCOnfigurationService.GetHrdatumByEmplid(Emplid);
        }
        protected bool errorVisible;
        protected LOBR.Models.LOBRCOnfiguration.Hrdatum hrdatum;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task FormSubmit()
        {
            try
            {
                await LOBRCOnfigurationService.UpdateHrdatum(Emplid, hrdatum);
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