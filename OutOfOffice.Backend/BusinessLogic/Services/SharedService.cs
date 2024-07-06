using BusinessLogic.Options;
using BusinessLogic.Services.Interfaces;
using Microsoft.Data.SqlClient;

namespace BusinessLogic.Services
{
    public class SharedService : ISharedService
    {
        public List<T> SortEntities<T>(List<T> entities, SortOptions options)
        {
            // Sorting logic based on options
            if (options.SortBy == "Name")
            {
                if (options.SortOrder == SortOrder.Ascending)
                    entities = entities.OrderBy(e => e.GetType().GetProperty("Name").GetValue(e, null)).ToList();
                else
                    entities = entities.OrderByDescending(e => e.GetType().GetProperty("Name").GetValue(e, null)).ToList();
            }
            else if (options.SortBy == "Date")
            {
                // Example sorting by date, assuming entities have a "Date" property
                if (options.SortOrder == SortOrder.Ascending)
                    entities = entities.OrderBy(e => e.GetType().GetProperty("Date").GetValue(e, null)).ToList();
                else
                    entities = entities.OrderByDescending(e => e.GetType().GetProperty("Date").GetValue(e, null)).ToList();
            }

            return entities;
        }

        public List<T> FilterEntities<T>(List<T> entities, FilterOptions options)
        {
            // Filtering logic based on options
            if (options.FilterBy == "Active")
            {
                entities = entities.Where(e => e.GetType().GetProperty("Status").GetValue(e, null).ToString() == "Active").ToList();
            }

            return entities;
        }
    }
}
