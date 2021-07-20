namespace RecrutariBlazorWasm.Shared.Entities
{
    public class SliderSelectorModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public bool Selected { get; set; }
        public SliderSelectorModel(int id, string name, string company, bool selected = true)
        {
            this.Id = id;
            this.Name = name;
            this.Selected = selected;
            this.Company = company;
        }
    }
}
