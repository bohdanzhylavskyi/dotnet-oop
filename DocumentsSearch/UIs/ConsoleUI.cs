using DocumentsSearch.Documents;

namespace DocumentsSearch.UIs
{
    public class ConsoleUI
    {
        private DocumentsService documentsService;
        public ConsoleUI(DocumentsService documentsService)
        {
            this.documentsService = documentsService;
        }
        public void Bootstrap()
        {
            while (true)
            {
                Console.WriteLine("Enter document number query (or 'exit' to close the app / 'clear' to clear console):");

                var query = Console.ReadLine();

                if (query == "exit")
                {
                    break;
                }

                if (query == "clear")
                {
                    Console.Clear();
                    continue;
                }

                if (!DocumentNumberQuery.TryParse(query, out DocumentNumberQuery documentNumberQuery))
                {
                    Console.WriteLine("\nInvalid search query was provided, document number query is expected\n");

                    continue;
                }

                var searchResult = this.documentsService.SearchDocuments(documentNumberQuery);

                PrintSearchResult(searchResult);
            }
        }

        private static void PrintSearchResult(List<DocumentCardInfo> infos)
        {
            if (infos.Count == 0)
            {
                Console.WriteLine("\nNo matches found for your query\n");

                return;
            }


            Console.WriteLine("\nSearch Results:\n");

            foreach (var info in infos)
            {
                foreach (var property in info.GetProperties())
                {
                    Console.WriteLine($"  {property.Name}: {property.Value}");
                }

                Console.WriteLine("\n-------------------------\n");
            }

        }
    }
}
