using Microsoft.VisualStudio.TestTools.UnitTesting;
using PathFinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder.Tests
{
    [TestClass()]
    public class CardTests
    {
        [TestMethod()]
        public void CardTest()
        {
            Card card = new Card("", "");
        }

        [TestMethod()]
        public void ArrangeLinqTest()
        {
           
            //Card card = new Card("", "");
            List<Card> cards;
            try
            {
                cards = Card.Arrange(null, null);
            }
            catch
            {
                Assert.IsTrue(true);
            }
            
            cards = new List<Card>() { new Card("MSK", "LON"),
                                       new Card("LON", "PAR"),
                                       new Card("PAR", "JAP"),
                                       new Card("JAP", "DUB"),
                                       new Card("DUB", "TAL"),
                                       new Card("TAL", "USA")
             };


            List<Card> temp;
            try
            {
                temp = Card.Arrange(cards, "");
            }
            catch (Exception e)
            {
                Assert.Fail("Пустое/несуществующее значение в списке приводит к ошибке\n{0}\n{1}", e.Message, e.GetType().ToString());
            }

            try
            {
                cards = Card.Arrange(cards, "JAP");
            }
            catch (Exception e)
            {
                Assert.Fail("Проход узлов вызывает ошибку\n{0}\n{1}", e.Message, e.GetType().ToString());
            }

            temp = new List<Card>() { new Card("PAR", "JAP"),
                                      new Card("JAP", "DUB"),
                                      new Card("DUB", "TAL"),
                                      new Card("TAL", "USA")
            };


            if (temp.Count != cards.Count)
            {
                Assert.Fail("Список содержит неверное количество узлов");
            }
            
            bool match = false;
            for (int i = 0; i < cards.Count; i++)
            {
                match = true;
                if (cards[i].From != temp[i].From || cards[i].Dest != temp[i].Dest)
                {
                    match = false;
                    break;
                }
            }

            if (!match)
            {
                Assert.Fail("Список не содержит ожидаемых элементов после прохода узлов");
            }
        }

        [TestMethod()]
        public void ArrangeTripTest()
        {
            List<Card> cards = new List<Card>() { new Card("MSK", "LON"),
                                            new Card("LON", "PAR"),
                                            new Card("PAR", "JAP"),
                                            new Card("JAP", "DUB"),
                                            new Card("DUB", "TAL"),
                                            new Card("TAL", "USA")
                };
            List<Card> temp = new List<Card>(cards);

            for(int i = 0; i < temp.Count; i++)
            {
                temp[i] = new Card(temp[i].From, temp[i].Dest);
            }
            
            Random r = new Random();
            int rv = 0;
            cards.Sort(delegate (Card f, Card t)
            {
                rv = r.Next(0, 3);
                return rv - 1;
            });            

            cards = Card.ArrangeTrip(cards);
            bool match = false;
            for (int i = 0; i < cards.Count; i++)
            {
                match = true;
                if (cards[i].From != temp[i].From || cards[i].Dest != temp[i].Dest)
                {
                    match = false;
                    break;
                }
            }

            if (!match)
            {
                Assert.Fail("Список не содержит ожидаемых элементов после расчета поездки");
            }
        }

        [TestMethod()]
        public void ArrangeTest()
        {
            Card[] cards = new Card[] { new Card("MSK", "LON"),
                                            new Card("LON", "PAR"),
                                            new Card("PAR", "JAP"),
                                            new Card("JAP", "DUB"),
                                            new Card("DUB", "TAL"),
                                            new Card("TAL", "USA")
            };
            Card[] temp = new Card[cards.Length];
            cards.CopyTo(temp, 0);

            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = new Card(temp[i].From, temp[i].Dest);
            }

            Random r = new Random();
            int rv = 0;
            for (int i = 0; i < cards.Length; i++)
            {
                rv = r.Next(0, cards.Length);
                Card ct = cards[i];
                cards[i] = cards[rv];
                cards[rv] = ct;
            }

            cards = Card.ArrageTrip(cards);
            bool match = false;
            for (int i = 0; i < cards.Length; i++)
            {
                match = true;
                if (cards[i].From != temp[i].From || cards[i].Dest != temp[i].Dest)
                {
                    match = false;
                    break;
                }
            }

            if (!match)
            {
                Assert.Fail("Массив не содержит ожидаемых элементов после расчета поездки");
            }
        }
    }
}