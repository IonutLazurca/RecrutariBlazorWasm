namespace RecrutariBlazorWasm.Shared.Entities
{
    public class MultipleSelectorModel
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public MultipleSelectorModel(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
        public MultipleSelectorModel()
        {

        }
    }
}
