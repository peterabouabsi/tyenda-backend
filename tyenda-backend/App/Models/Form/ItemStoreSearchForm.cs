using System;

namespace tyenda_backend.App.Models.Form
{
    public class ItemStoreSearchForm
    {
        public string? Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string[]? Categories { get; set; }//[Id1, Id2, Id3, etc.]
        public string? City { get; set; }//{Id, Value}
        public int[]? Price { get; set; } //[Min, Max]
    }
}