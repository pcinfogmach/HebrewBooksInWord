using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HebrewBooks.Models
{
    internal class BookEntriesList : List<BookEntry>
    {
        public BookEntriesList()
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = System.IO.Path.Combine(appPath, "Resources", "FullIndex.txt");

            // Ensure the file exists before attempting to read
            if (File.Exists(fullPath))
            {
                using (var reader = new StreamReader(fullPath))
                {
                    while (!reader.EndOfStream)
                    {
                        var splitEntry = reader.ReadLine().Split(',');

                        if (splitEntry.Length == 11)  // Ensure correct number of entries
                        {
                            Add(new BookEntry(
                                splitEntry[0], splitEntry[1], splitEntry[2], splitEntry[3],
                                splitEntry[4], splitEntry[5], splitEntry[6], splitEntry[7],
                                splitEntry[8], splitEntry[9], splitEntry[10]));
                        }
                    }
                }

                // Sort the list by the title field in place
                Sort((entry1, entry2) => string.Compare(entry1.Title, entry2.Title, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                // Handle file not found case if necessary
                throw new FileNotFoundException($"The file '{fullPath}' does not exist.");
            }
        }
    }
}
