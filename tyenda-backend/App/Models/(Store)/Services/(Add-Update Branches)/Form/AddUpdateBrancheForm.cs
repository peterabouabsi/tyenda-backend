namespace tyenda_backend.App.Models._Store_.Services._Add_Update_Branches_.Form
{
    public class AddUpdateBranchForm
    {
        public string CityId { get; set; } = "";
        public string AddressDetails { get; set; } = "";
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
}