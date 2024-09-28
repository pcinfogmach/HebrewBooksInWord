using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HebrewBooks
{
    internal class Catalogue
    {
        public List<BookEntry> bookEntries;

        public Catalogue()
        {
            LoadBookEntries();
        }

        void LoadBookEntries()
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = System.IO.Path.Combine(appPath, "Resources", "FullIndex.txt");
            var entries = new List<BookEntry>();

            using (var reader = new StreamReader(fullPath))
            {
                while (!reader.EndOfStream)
                {
                    var splitEntry = reader.ReadLine().Split(',');
                    entries.Add(new BookEntry(splitEntry[0], splitEntry[1], splitEntry[2], splitEntry[3], splitEntry[4], splitEntry[5], splitEntry[6], splitEntry[7], splitEntry[8], splitEntry[9], splitEntry[10]));
                }
            }

            bookEntries = entries.OrderBy(e => e.Title).ToList();
        }
    }
}
