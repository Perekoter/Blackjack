using System;
using System.Linq;
using System.Collections.Generic;

namespace Blackjack
{
	class MainClass
	{
		public static List<Card> Deck { get; set; }

		public static List<Player> Players { get; set; }

		public static void Main (string[] args)
		{
			Console.WriteLine("Добро пожаловать в Блекджек! ");
			while (true)
			{
				Console.WriteLine("Число игроков не может быть меньше двух или больше десяти. Для начала игры, пожалуйста, введите число игроков:");
				Console.Write("> ");
				int number;
				string inputPlayer = Console.ReadLine();
				if (Int32.TryParse(inputPlayer, out number))
				{
					if (number < 2 || number > 10)
					{
						Console.WriteLine("Число игроков не должно быть меньше двух или больше десяти!");
						Console.Write("> ");
						continue;
					}
					Start(number);
				}
				else
				{
					Console.WriteLine("Некорректное число!");
					Console.Write("> ");
					continue;
				}
				// DEBUG
				Console.ReadKey();
			}
		}

		public static void Start(int playersNum)
		{
            generateCards ();
			generatePlayers (playersNum);

			int stoppedPlayers = 0;

			while (stoppedPlayers < playersNum)
			{
				foreach (Player player in Players)
				{
					if (!player.Active)
					{
						continue;
					}
					bool takeCardDecision = player.makeDecision();

					if (takeCardDecision)
					{
						Card card = takeCard(player);
						if (player.User)
						{
							Console.WriteLine("Ваша карта:");
							Console.WriteLine(card.getFullTitle());
						}
						else
						{
							Console.WriteLine("Игрок " + player.Name + " взял карту.");
						}

						if (player.getDeckValue() >= 21)
						{
							player.Active = false;
							stoppedPlayers++;
							Console.WriteLine("Игрок " + player.Name + " достиг или превысил 21 очко.");
						}
					}
					else
					{
						stoppedPlayers++;
						if (player.User)
						{
							Console.WriteLine("Вы отказались от карты.");
						}
						else
						{
							Console.WriteLine("Игрок " + player.Name + " отказался.");
						}
					}
				}
			}

			Dictionary<string, int> rating = new Dictionary<string, int>();

			foreach (Player player in Players)
			{
				rating.Add(player.Name, player.getDeckValue());
			}

			rating = rating.Where( p => p.Value <= 21).ToDictionary(p => p.Key, p => p.Value);
			rating = rating.OrderBy( p => -p.Value).ToDictionary(p => p.Key, p => p.Value);

			Console.WriteLine("-----------------------------");
			Console.WriteLine("Рейтинг:");
			Console.WriteLine("-----------------------------");
			foreach (Player player in Players)
			{
				Console.WriteLine(player.Name + " (" + player.getDeckValue() + ")");
			}

			Dictionary<string, int> winner = new Dictionary<string, int>();
			int max = 0;

			foreach (KeyValuePair<string, int> player in rating)
			{
				if (max <= player.Value)
				{
					winner.Add(player.Key, player.Value);
					max = player.Value;
				}
				else
				{
					break;
				}
			}

			Console.WriteLine("-----------------------------");
			Console.WriteLine(winner.Count == 1 ? "Победитель:" : "Победители:");
			Console.WriteLine("-----------------------------");
			foreach (KeyValuePair<string, int> player in winner)
			{
				Console.WriteLine(player.Key + " (" + player.Value + ")");
			}

			Console.WriteLine("ИГРА ОКОНЧЕНА!");
			Console.WriteLine("Для новой игры нажмите любую клавишу.");
		}

		public static void generateCards ()  
		{
			List<Card> cards = new List<Card>();
			var suits = new string[4] { "Hearts", "Spades", "Clubs", "Diamonds" };
			var nums = new string[12] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "Ace" };

			foreach (string suit in suits) {
				int value = 2;

				foreach (string num in nums) {
					Card newCard = new Card {
						Value = value,
						Suit = suit,
						Title = num
					};
					cards.Add (newCard);
					value++;
				}
			}

			Deck = cards;
		}

		public static void generatePlayers (int count) 
		{
			List<Player> players = new List<Player> ();
			string[] names = new string[10] { "Nikita", "Clifford", "Brady", "Vernita", "Nu", "Kurt", "Suzanna", "Clark", "Tad", "Soo" };

			for (int i = 0; i < count; i++) {
				bool user = i == 0;
				Player player = new Player {
					Name = user ? "Player" : names[i],
					User = user,
					Active = true,
					Deck = new List<Card> ()
				};
                takeCard(player);
				takeCard(player);
				players.Add (player);
			}
			Players = players;
		}

		public static Card takeCard(Player player)
		{
			Random rnd = new Random();
			int cardIndex = rnd.Next(Deck.Count - 1);
			player.Deck.Add(Deck[cardIndex]);
			Deck.RemoveAt(cardIndex);
			return player.Deck[player.Deck.Count - 1];
		}
	}
}
