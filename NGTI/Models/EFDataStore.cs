using Google.Apis.Util.Store;
using Newtonsoft.Json;
using NGTI.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace NGTI.Models
{
    public class EFDataStore : IDataStore
    {
        public async Task ClearAsync()
        {
            using (var context = new ApplicationDbContext())
            {
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                await objectContext.ExecuteStoreCommandAsync("TRUNCATE TABLE [AspNetUsers]");
            }
        }

        public async Task DeleteAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }

            using (var context = new ApplicationDbContext())
            {
                var generatedKey = GenerateStoredKey(key, typeof(T));
                var item = context.AspNetUsers.FirstOrDefault(x => x.TokenKey == generatedKey);
                if (item != null)
                {
                    context.AspNetUsers.Remove(item);
                    await context.SaveChangesAsync();
                }
            }
        }

        public Task<T> GetAsync<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }

            using (var context = new ApplicationDbContext())
            {
                var generatedKey = GenerateStoredKey(key, typeof(T));
                var item = context.AspNetUsers.FirstOrDefault(x => x.TokenKey == generatedKey);
                T value = item == null ? default(T) : JsonConvert.DeserializeObject<T>(item.TokenValue);
                return Task.FromResult<T>(value);
            }
        }

        public async Task StoreAsync<T>(string key, T value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key MUST have a value");
            }

            using (var context = new ApplicationDbContext())
            {
                var generatedKey = GenerateStoredKey(key, typeof(T));
                string json = JsonConvert.SerializeObject(value);

                var item = await context.AspNetUsers.SingleOrDefaultAsync(x => x.TokenKey == generatedKey);

                if (item == null)
                {
                    context.AspNetUsers.Add(new ApplicationUser { TokenKey = generatedKey, TokenValue = json });
                }
                else
                {
                    item.TokenValue = json;
                }

                await context.SaveChangesAsync();
            }
        }

        private static string GenerateStoredKey(string key, Type t)
        {
            return string.Format("{0}-{1}", t.FullName, key);
        }
    }
}
