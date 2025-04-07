using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using LOBR.Data;

namespace LOBR
{
    public partial class LOBRCOnfigurationService
    {
        LOBRCOnfigurationContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly LOBRCOnfigurationContext context;
        private readonly NavigationManager navigationManager;

        public LOBRCOnfigurationService(LOBRCOnfigurationContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportHrdataToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/lobrconfiguration/hrdata/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/lobrconfiguration/hrdata/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportHrdataToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/lobrconfiguration/hrdata/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/lobrconfiguration/hrdata/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnHrdataRead(ref IQueryable<LOBR.Models.LOBRCOnfiguration.Hrdatum> items);

        public async Task<IQueryable<LOBR.Models.LOBRCOnfiguration.Hrdatum>> GetHrdata(Query query = null)
        {
            var items = Context.Hrdata.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnHrdataRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnHrdatumGet(LOBR.Models.LOBRCOnfiguration.Hrdatum item);
        partial void OnGetHrdatumByEmplid(ref IQueryable<LOBR.Models.LOBRCOnfiguration.Hrdatum> items);


        public async Task<LOBR.Models.LOBRCOnfiguration.Hrdatum> GetHrdatumByEmplid(int emplid)
        {
            var items = Context.Hrdata
                              .AsNoTracking()
                              .Where(i => i.Emplid == emplid);

 
            OnGetHrdatumByEmplid(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnHrdatumGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnHrdatumCreated(LOBR.Models.LOBRCOnfiguration.Hrdatum item);
        partial void OnAfterHrdatumCreated(LOBR.Models.LOBRCOnfiguration.Hrdatum item);

        public async Task<LOBR.Models.LOBRCOnfiguration.Hrdatum> CreateHrdatum(LOBR.Models.LOBRCOnfiguration.Hrdatum hrdatum)
        {
            OnHrdatumCreated(hrdatum);

            var existingItem = Context.Hrdata
                              .Where(i => i.Emplid == hrdatum.Emplid)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Hrdata.Add(hrdatum);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(hrdatum).State = EntityState.Detached;
                throw;
            }

            OnAfterHrdatumCreated(hrdatum);

            return hrdatum;
        }

        public async Task<LOBR.Models.LOBRCOnfiguration.Hrdatum> CancelHrdatumChanges(LOBR.Models.LOBRCOnfiguration.Hrdatum item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnHrdatumUpdated(LOBR.Models.LOBRCOnfiguration.Hrdatum item);
        partial void OnAfterHrdatumUpdated(LOBR.Models.LOBRCOnfiguration.Hrdatum item);

        public async Task<LOBR.Models.LOBRCOnfiguration.Hrdatum> UpdateHrdatum(int emplid, LOBR.Models.LOBRCOnfiguration.Hrdatum hrdatum)
        {
            OnHrdatumUpdated(hrdatum);

            var itemToUpdate = Context.Hrdata
                              .Where(i => i.Emplid == hrdatum.Emplid)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(hrdatum);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterHrdatumUpdated(hrdatum);

            return hrdatum;
        }

        partial void OnHrdatumDeleted(LOBR.Models.LOBRCOnfiguration.Hrdatum item);
        partial void OnAfterHrdatumDeleted(LOBR.Models.LOBRCOnfiguration.Hrdatum item);

        public async Task<LOBR.Models.LOBRCOnfiguration.Hrdatum> DeleteHrdatum(int emplid)
        {
            var itemToDelete = Context.Hrdata
                              .Where(i => i.Emplid == emplid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnHrdatumDeleted(itemToDelete);


            Context.Hrdata.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterHrdatumDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}