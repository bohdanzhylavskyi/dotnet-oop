using DocumentsSearch.Documents;
using DocumentsSearch.DocumentsStores;
using DocumentsSearch.DocumentStores;

namespace DocumentsSearch
{
    internal class Program
    {
        static string DocumentsFolderPath = "./documents-store";

        static void Main(string[] args)
        {
            var docTypesRegistry = new DocumentTypesRegistry();

            docTypesRegistry.Register(DocumentType.Book, typeof(Book));
            docTypesRegistry.Register(DocumentType.Patent, typeof(Patent));
            docTypesRegistry.Register(DocumentType.LocalizedBook, typeof(LocalizedBook));
            docTypesRegistry.Register(DocumentType.Magazine, typeof(Magazine));

            var documentDeserializer = new DocumentDeserializer(docTypesRegistry);

            var fsDocumentsStore = new FSDocumentsStore(documentDeserializer, DocumentsFolderPath);
            var documentsCache = new DocumentsCache(
                new DocumentsCacheConfiguration()
                {
                    config = new Dictionary<DocumentType, DocumentTypeCacheConfiguration>()
                    {
                        { DocumentType.Book, new DocumentTypeCacheConfiguration() { ExpirationInMs = 10000 } }
                    }
                }
            );
            var cachedFsDocumentsStore = new CachedDocumentsStore(fsDocumentsStore, documentsCache); // TODO check

            var documentsService = new DocumentsService(cachedFsDocumentsStore);

            while (true)
            {
                var input = Console.ReadLine();

                if (input == "exit")
                {
                    break;
                }

                var documentNumber = int.Parse(input);
                var searchResult = documentsService.SearchDocuments(documentNumber);

                PrintSearchResult(searchResult);
            }
        }

        private static void PrintSearchResult(List<DocumentCardInfo> infos)
        {
            Console.WriteLine("\nSearch Results:\n");

            foreach (var info in infos)
            {
                foreach (var property in info.GetProperties())
                {
                    Console.WriteLine($"{property.Name}: {property.Value}");
                }

                Console.WriteLine("-------------------------");
            }

        }
    }
}
