using DND.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DND.Repository
{
    public class BaseRepository
    {
        public IEnumerable<T> GetList<T>()
        {
            var fileName = this.GetType().Name;
            try
            {
                var result = System.IO.File.ReadAllText(fileName);
                return JsonConvert.DeserializeObject<IEnumerable<T>>(result);
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }

        public void Save<T>(IEnumerable<T> list)
        {
            var content = JsonConvert.SerializeObject(list);
            var fileName = this.GetType().Name;
            System.IO.File.WriteAllText(fileName, content);
        }

        public void InsertRange<T>(IEnumerable<T> list)
        {
            var result = GetList<T>().ToList();

            var resultJson = JsonConvert.SerializeObject(result);
            result.AddRange(list.Where(f => !resultJson.Contains(JsonConvert.SerializeObject(f))));

            Save(result);
        }

        public void Insert<T>(T item)
        {
            var list = GetList<T>().ToList();
            list.Add(item);
            Save(list);
        }

        public void Update<T>(T item)
        {
            var list = GetList<T>().ToList();
            var index = list.FindIndex(m => m.Equals(item));
            list[index] = item;
            Save(list);
        }

        public void Delete<T>(T item)
        {
            var list = GetList<T>().ToList();
            list.Remove(item);
            Save(list);
        }
    }
}
