namespace DocumentsSearch
{
    public class DocumentNumberQuery
    {
        private readonly string query;

        private DocumentNumberQuery(string query)
        {
            this.query = query;
        }

        public override string ToString()
        {
            return this.query;
        }

        public static bool TryParse(string query, out DocumentNumberQuery documentNumberQuery)
        {
            if (IsValidDocumentNumberQuery(query))
            {
                documentNumberQuery = new DocumentNumberQuery(query);

                return true;
            }

            documentNumberQuery = null;

            return false;
        }

        private static bool IsValidDocumentNumberQuery(string query)
        {
            if (query.Trim().Length == 0)
            {
                return false;
            }

            return query.All(char.IsDigit);
        }
    }
}
