using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder
{

    /// <summary>
    /// Trip card
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Trip From point
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// Trip Destination point
        /// </summary>
        public string Dest { get; set; }

        /// <summary>
        /// Create trip card
        /// </summary>
        /// <param name="From">Trip From point</param>
        /// <param name="Dest">Trip Destination point</param>
        public Card(string From, string Dest)
        {
            this.From = From;
            this.Dest = Dest;
        }

        /// <summary>
        /// Make recursive connection from From to Dest and return new list
        /// </summary>
        /// <param name="Cards">List of Card</param>
        /// <param name="From">Connect clause</param>
        /// <returns></returns>
        public static List<Card> Arrange(List<Card> Cards, string From)
        {
            return Cards.Where(x => x.Dest == From)
                        .Union(Cards.Where(x => x.From == From).SelectMany(y => Arrange(Cards, y.Dest)))
                        .ToList();
        }

        /// <summary>
        /// Claim root path for cards list
        /// </summary>
        /// <param name="Cards">List of card</param>
        /// <returns></returns>
        private static Card GetRoot(Card[] Cards)
        {
            //Determinant of connection nodes
            int marker = 0;
            for (int i = 0; i < Cards.Length; i++)
            {
                for (int j = 0; j < Cards.Length; j++)
                {
                    marker = -1;
                    if (Cards[i].From == Cards[j].Dest)
                    {
                        marker = i;
                        break;
                    }
                }
                if (marker == -1)
                {
                     return Cards[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Performs cards arrange
        /// </summary>
        /// <param name="Cards">List of Card</param>
        /// <returns></returns>
        public static List<Card> ArrangeTrip(List<Card> Cards)
        {
            Card root = GetRoot(Cards.ToArray());
            return Arrange(Cards, root.From);
        }

        /// <summary>
        /// Sorts cards using algoritm
        /// </summary>
        /// <param name="Cards">Array of Card</param>
        public static Card[] ArrageTrip(Card[] Cards)
        {
            if(Cards == null)
            {
                throw new Exception("Null variable");
            }
            Card root = Card.GetRoot(Cards);
            Card[] result = new Card[Cards.Length];
            result[0] = root;
            int pos = 0;
            while (pos < Cards.Length - 1)
            {
                for (int j = 0; j < Cards.Length; j++)
                {
                    if (Cards[j] == root)
                    {
                        continue;
                    }
                    if (Cards[j].From == result[pos].Dest)
                    {
                        pos++;
                        result[pos] = Cards[j];
                        break;
                    }
                }
            }
            return result;
        }
        
    }

    class Program
    {
        /// <summary>
        /// Print message for card
        /// </summary>
        /// <param name="list"></param>
        private static void debug(List<Card> list)
        {
            foreach (Card key in list)
            {
                Console.WriteLine(String.Format("Отправление из {0} в {1}", key.From, key.Dest));
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Нажмите любую клавишу для начала работы программы");
            Console.WriteLine("Нажмите q для выхода");
            while (Console.ReadKey().KeyChar != 'q')
            {
                Console.Clear();
                //Заполнение колоды карточками
                List<Card> cards = new List<Card>() { new Card("MSK", "LON"),
                                            new Card("LON", "PAR"),
                                            new Card("PAR", "JAP"),
                                            new Card("JAP", "DUB"),
                                            new Card("DUB", "TAL"),
                                            new Card("TAL", "USA")
                };

                Console.WriteLine("Составленный список карточек:");
                debug(cards);

                Console.WriteLine("Представим, что карточки обранили:");
                    //Сортировка в случайном порядке
                    Random r = new Random();
                    int rv = 0;
                    cards.Sort(delegate (Card f, Card t)
                    {
                        rv = r.Next(0, 3);
                        return rv - 1;
                    });
                debug(cards);

                Console.WriteLine("Восстановим порядок алгоритмом:");
                debug(Card.ArrageTrip(cards.ToArray()).ToList());

                Console.WriteLine("Восстановим порядок через Linq:");
                debug(Card.ArrangeTrip(cards));

                Console.WriteLine("Рассчитать поездку, например из JAP");
                //Lazy копия карточек
                List<Card> arrange = Card.Arrange(cards, "JAP");
                //Для пользовательского вывода, удаляется узел содержащий пункт прибытия в JAP
                arrange.RemoveAt(0);
                debug(arrange);

                Console.WriteLine("\nНажмите любую клавишу для повтора работы программы");
                Console.WriteLine("Нажмите q для выхода");
            }            
        }
    }
}
