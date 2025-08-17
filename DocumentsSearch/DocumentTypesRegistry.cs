using DocumentsSearch.Documents;

namespace DocumentsSearch
{
    public class DocumentTypesRegistry
    {
        private Dictionary<DocumentType, Type> implementations = new();

        public void Register(DocumentType type, Type implementationType)
        {
            this.implementations[type] = implementationType;
        }

        public Type ResolveImplementationType(DocumentType type)
        {
            return this.implementations[type];
        }
    }
}
