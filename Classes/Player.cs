using System;
using System.Collections.Generic;

namespace Blackjack
{
	public class Player
	{
		public string Name { get; set; }

		public bool User { get; set; }

		public List<Card> Deck { get; set; }

		public bool Active { get; set; }

		public int getDeckValue ()
		{
			int sum = 0;
			foreach (Card card in Deck) {
				sum += card.Value;
			}
			return sum;
		}

		public bool makeDecision()
		{
			if (User)
			{
				return makeDecisionHuman();
			}
			else
			{
				return makeDecisionBot();
			}
		}

		public bool makeDecisionBot()
		{
			int sumValue = getDeckValue();
			if (sumValue > 18)
			{
				Active = false;
				return false;
			}
			else if (sumValue <= 18 && sumValue >= 12)
			{
				Random rnd = new Random();
				bool decision = rnd.NextDouble() >= 0.5;
				Active = decision;
				return decision;
			}
			else
			{
				return true;
			}
		}

		public bool makeDecisionHuman()
		{
			Console.WriteLine("Ваши карты:");
			foreach (Card card in Deck)
			{
				Console.WriteLine(card.getFullTitle());
			}
			Console.WriteLine("Ваш счёт: " + getDeckValue());
			Console.WriteLine("");
			Console.WriteLine("Чтобы взять карту, напишите 'hit'. Чтобы отказаться от карты, напишите 'stop'.");

			while (true) {
				Console.Write("> ");
				string inputAction = Console.ReadLine();
				if (inputAction == "hit")
				{
					return true;
				}
				else if (inputAction == "stop")
				{
					Active = false;
					return false;
				}
				else
				{
					Console.WriteLine("Некорректная команда! Чтобы взять карту, напишите 'hit'. Чтобы отказаться от карты, напишите 'stop'.");
					continue;
				}

			}
		}
	}
}

