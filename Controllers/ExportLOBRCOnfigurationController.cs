using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using LOBR.Data;

namespace LOBR.Controllers
{
    public partial class ExportLOBRCOnfigurationController : ExportController
    {
        private readonly LOBRCOnfigurationContext context;
        private readonly LOBRCOnfigurationService service;

        public ExportLOBRCOnfigurationController(LOBRCOnfigurationContext context, LOBRCOnfigurationService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/LOBRCOnfiguration/hrdata/csv")]
        [HttpGet("/export/LOBRCOnfiguration/hrdata/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportHrdataToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetHrdata(), Request.Query, false), fileName);
        }

        [HttpGet("/export/LOBRCOnfiguration/hrdata/excel")]
        [HttpGet("/export/LOBRCOnfiguration/hrdata/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportHrdataToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetHrdata(), Request.Query, false), fileName);
        }
    }
}
