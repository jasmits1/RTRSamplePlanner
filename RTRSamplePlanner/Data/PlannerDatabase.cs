using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RTRSamplePlanner.Model;
using SQLite;

namespace RTRSamplePlanner.Data
{
    public class PlannerDatabase
    {
        SQLiteAsyncConnection Database;

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            var result = await Database.CreateTableAsync<PlannerEvent>();
            await LoadSampleDataAsync();
        }

        public async Task<List<PlannerEvent>> GetAllEventsAsync()
        {
            await Init();
            return await Database.Table<PlannerEvent>().ToListAsync();
        }

        private async Task<int> LoadSampleDataAsync()
        {
            List<PlannerEvent> seedData = new List<PlannerEvent>();
            PlannerEvent p = new PlannerEvent();
            p.Name = "First Item";
            seedData.Add(p);
            return await Database.InsertAsync(p);
        }
           

    }
}
