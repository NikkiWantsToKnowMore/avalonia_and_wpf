using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace word_game.Models
{
    public class ModelColod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WordCount { get; set; }
        public int Progress { get; set; }
        public bool IsChecked { get; set; }
    }
}
