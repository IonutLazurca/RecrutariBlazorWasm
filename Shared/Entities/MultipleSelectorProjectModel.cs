namespace RecrutariBlazorWasm.Shared.Entities
{
    public class MultipleSelectorProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }

        public MultipleSelectorProjectModel(int id, string name, string company)
        {
            this.Id = id;
            this.Name = name;
            this.Company = company;
        }
    }
}
