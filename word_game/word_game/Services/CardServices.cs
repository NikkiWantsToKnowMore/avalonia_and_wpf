using word_game.ConnectContext;
using Microsoft.EntityFrameworkCore;
using word_game.Models;

namespace word_game.Services
{

    public class CardServices
    {
        WordContext db;
       
        public void AddCard( string _word, string _translate, int _progress, int _colods, bool _isChecked) {

            db = new WordContext();
            
            db.Words.Load(); // загрузка данных

            ModelWord AddCard = new ModelWord();

            AddCard.Word = _word;
            AddCard.Translate = _translate;
            AddCard.IsChecked = _isChecked;
            AddCard.Progress = _progress;
            AddCard.Colods = _colods;
            db.Words.Add(AddCard);
            db.SaveChanges();
        }

        public List<Words> GetCards( int idColod) {
            
            db = new WordContext();
            db.Words.Load(); // загрузка данных

            List<Words> cards = new List<Words>();
            //Words card = new Words();
            var cardsFromDb = db.Words;

            foreach (var cardFromDb in cardsFromDb) {

                if (cardFromDb.Colods == idColod)
                {
                    Words card = new Words();
                    card.Id = cardFromDb.Id;
                    card.Word = cardFromDb.Word;
                    card.Translate = cardFromDb.Translate;
                    card.Progress = cardFromDb.Progress;
                    card.IsChecked = cardFromDb.IsChecked;
                    cards.Add(card);
                }
                
            }
            return cards;
        }
        public void DeleteCard(int _id) {

            db = new WordContext();
            db.Words.Load(); // загрузка данных
            WordContext WordContext = new WordContext();

            ModelWord card = WordContext.Words
                .Where(x => x.Id == _id)
                .FirstOrDefault();

            WordContext.Words.Remove(card);
            WordContext.SaveChanges();
           
        }


        //public void Remove() {

        //    db = new WordContext();
        //    db.ModelWords.Load(); // загрузка данных
        //    db.ModelWords.Remove();
        //}
    }
}
