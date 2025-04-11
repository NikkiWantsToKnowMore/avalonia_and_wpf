using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace word_game.Models
{
    public class ModelWord
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Translate { get; set; }
        public bool IsChecked { get; set; }
        public int Progress { get; set; }
        public int Colods { get; set; }
    }
}
