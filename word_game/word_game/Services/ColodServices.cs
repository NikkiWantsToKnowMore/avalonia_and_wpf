using word_game.ConnectContext;
using Microsoft.EntityFrameworkCore;
using word_game.Models;

namespace word_game.Services
{
    public class ColodServices
    {
        ColodContext db;
        WordContext wordDb;

        public void AddColod(string _name, bool _isChecked, int _progress)
        {

            db = new ColodContext();
            db.Colods.Load(); // загрузка данных

            ModelColod AddColod = new ModelColod();

            AddColod.Name = _name;
            AddColod.Progress = _progress;
            AddColod.IsChecked = false;

            db.Colods.Add(AddColod);
            db.SaveChanges();
        }
        
        public List<Colod> GetColod() {
            
            db = new ColodContext();
            db.Colods.Load(); // загрузка данных

            List<Colod> cards = new List<Colod>();
            //Words card = new Words();
            var colodsFromDb = db.Colods;

            foreach (var colodFromDb in colodsFromDb) {
                Colod card = new Colod();
                card.Id = colodFromDb.Id;
                card.Name = colodFromDb.Name;
                card.IsChecked = colodFromDb.IsChecked;
                card.WordCount = colodFromDb.WordCount;
                cards.Add(card);
                
            }
            return cards;
        }

        public void DeleteColod(int _id)
        {

            db = new ColodContext();
            wordDb = new WordContext();

            db.Colods.Load(); // загрузка данных
            wordDb.Words.Load();

            ColodContext ColodContext = new ColodContext();
            WordContext wordContext = new WordContext();

            ModelColod card = ColodContext.Colods
                .Where(x => x.Id == _id)
                .FirstOrDefault();

            ColodContext.Colods.Remove(card);
            ColodContext.SaveChanges();
       
            ModelWord modelWord = wordContext.Words //удаление всех слов с таким же id колоды
               .Where(x => x.Colods == _id)
               .FirstOrDefault();

            if (modelWord != null)
            {
                wordContext.Words.Remove(modelWord);
                wordContext.SaveChanges();
            }
        }

        public void ChandeStatus(int idColod, bool status) //изменение isChecked
        { 

            db = new ColodContext();
            db.Colods.Load();
            ColodContext colodContext = new ColodContext();
            ModelColod colod = colodContext.Colods
                .Where(x => x.Id == idColod)
                .FirstOrDefault();
            colod.IsChecked = status;
            colodContext.SaveChanges();
        }

        public void AddWordCount( int idColod) { //увеличить индекс в колоде при добавлении новой карты

            db = new ColodContext();
            db.Colods.Load();
            ColodContext colodContext = new ColodContext();
            ModelColod colod = colodContext.Colods
                .Where(x => x.Id == idColod)
                .FirstOrDefault();
            colod.WordCount++;
            colodContext.SaveChanges();
        }

        public void DeleteWordCount(int idColod) //уменьшить индекс в колоде при добавлении новой карты
        {
            db = new ColodContext();

            db.Colods.Load();
            ColodContext colodContext = new ColodContext();
            ModelColod colod = colodContext.Colods
                .Where(x => x.Id == idColod)
                .FirstOrDefault();

            if (colod.WordCount >= 0)
            {
                colod.WordCount--;
                colodContext.SaveChanges();
            }
        }
    }
}
