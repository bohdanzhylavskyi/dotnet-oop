namespace DocumentsSearch.Documents
{
    public class Magazine : Document
    {
        public required string Title { get; init; }
        public required string Publisher { get; init; }
        public required int ReleaseNumber { get; init; }
        public required DateOnly PublishDate { get; init; }

        public override DocumentCardInfo GetInfo()
        {
            var info = new DocumentCardInfo();

            info.AddProperty("Document Type", "Magazine");
            info.AddProperty("Document Number", this.DocumentNumber.ToString());
            info.AddProperty("Title", this.Title);
            info.AddProperty("Publisher", this.Publisher);
            info.AddProperty("Release Number", this.ReleaseNumber.ToString());
            info.AddProperty("Date Published", this.PublishDate.ToString());

            return info;
        }
    }
}
