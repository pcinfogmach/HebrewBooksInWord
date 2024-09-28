// See https://aka.ms/new-console-template for more information

using HebrewBooks;

var catalogue = new Catalogue();

for (int i = 0; i < 100; i++)
{
    Console.WriteLine( $"{i}\\{catalogue.bookEntries.Count}: {catalogue.bookEntries[i]}");
    Downloader.Download(catalogue.bookEntries[i].Title, catalogue.bookEntries[i].ID_Book);
}
