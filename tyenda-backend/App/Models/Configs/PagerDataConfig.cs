using System.Collections.Generic;

namespace tyenda_backend.App.Models.Configs
{
    public class PagerDataConfig<T>
    {
        public ICollection<T>? Data { get; set; }
        public int Count { get; set; } = 0;
    }
}