using DocumentsSearch.Documents;

namespace DocumentsSearch
{
    public class DocumentTypesRegistry
    {
        private Dictionary<DocumentType, Type> implementations = new();

        public void Register(DocumentType type, Type implementationType)
        {
            if (!typeof(Document).IsAssignableFrom(implementationType))
            {
                throw new ArgumentException($"'{nameof(Document)}' subclasses are the only types allowed for registration");
            }

            this.implementations[type] = implementationType;
        }

        public Type ResolveImplementationType(DocumentType type)
        {
            return this.implementations[type];
        }
    }
}
