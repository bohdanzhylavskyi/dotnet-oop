namespace DocumentsSearch.Documents
{
    public class LocalizedBook : Document
    {
        public required string ISBN { get; init; }
        public required string Title { get; init; }
        public required List<string> Authors { get; init; }
        public required int PageCount { get; init; }
        public required string OriginalPublisher { get; init; }
        public required string CountryOfLocalization { get; init; }
        public required string LocalPublisher { get; init; }
        public required DateOnly PublishDate { get; init; }

        public override DocumentCardInfo GetInfo()
        {
            var info = new DocumentCardInfo();

            info.AddProperty("Document Type", "Localized Book");
            info.AddProperty("ISBN", this.ISBN);
            info.AddProperty("Title", this.Title);
            info.AddProperty("Authors", string.Join(",", this.Authors.ToArray()));
            info.AddProperty("Number Of Pages", this.PageCount.ToString());
            info.AddProperty("Original Publisher", this.OriginalPublisher);
            info.AddProperty("Country Of Localization", this.CountryOfLocalization);
            info.AddProperty("Local Publisher", this.LocalPublisher);
            info.AddProperty("Date Published", this.PublishDate.ToString());

            return info;
        }
    }
}
