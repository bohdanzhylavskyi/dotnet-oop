namespace DocumentsSearch.Documents
{
    public class DocumentCardInfoProperty
    {
        public required string Name;
        public required string Value;
    }

    public class DocumentCardInfo
    {
        private List<DocumentCardInfoProperty> properties = new();

        public void AddProperty(string propertyName, string value)
        {
            this.properties.Add(new DocumentCardInfoProperty()
            {
                Name = propertyName,
                Value = value
            });
        }

        public List<DocumentCardInfoProperty> GetProperties()
        {
            return this.properties;
        }
    }

    public abstract class Document
    {
        public abstract DocumentCardInfo GetInfo();
    }
}
