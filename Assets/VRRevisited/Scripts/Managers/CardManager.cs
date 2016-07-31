using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardManager : MonoBehaviour {

	private Stack<Card> cardStack = new Stack<Card>();
	private Card currentCard = null;
	private Stack<Card> drawnCards = new Stack<Card>();
	public static CardManager Instance;

	void Start() {
		InitFromGameConfig();
	}

	void Awake()
	{
		//Check if instance already exists
		if (Instance == null)
		    
		    //if not, set instance to this
		    Instance = this;

		//If instance already exists and it's not this:
		else if (Instance != this)
		    
		    //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
		    Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);
	}

	public Card CurrentCard() {
		return currentCard;
	}

	public Card DrawCard() {//draw a new card and return it
		if(cardStack.Count>0){
			if(currentCard!=null) {
				//put current card to drawnCards stack
				drawnCards.Push(currentCard);
			}

			currentCard = cardStack.Pop();//draw card

			Debug.Log(currentCard.name+" was drawn ("+currentCard.description+")");
			if(cardStack.Count==0) {
				//card stack is empty after drawing card
				Debug.LogWarning("Last card was drawn");
			}
			return currentCard;
		}else {
			//card stack is empty and user is drawing a card
			Debug.LogWarning("Card stack is empty. Shuffeling cards.");
			this.Reset();
			return DrawCard();
		}
	}

	private void InitFromGameConfig() {//get cards that are in game from GameConfig and shuffle them
		cardStack.Clear();
		drawnCards.Clear();
		currentCard = null;
		foreach(Card card in GameConfig.Instance.cards){
			cardStack.Push(card);
		}
		ShuffleCardStack();
	}

	public void Reset() {//reset all cards to start and shuffle
		while(drawnCards.Count>0) {
			cardStack.Push(drawnCards.Pop());
		}
		if(currentCard!=null) {
			cardStack.Push(currentCard);
			currentCard = null;
		}
		ShuffleCardStack();
	}

	public void ShuffleCardStack() {//shuffle cardStack (not drawnCards stack)
		List<Card> tempcards = new List<Card>();
		while(cardStack.Count>0) {
			tempcards.Add(cardStack.Pop());
		}
		while(tempcards.Count>0) {
			int randomIndex = (int) Random.Range(0,tempcards.Count);
			cardStack.Push(tempcards[randomIndex]);
			tempcards.RemoveAt(randomIndex);
		}
	}

	public void PrintCardStack() {//print all card in the order that they will be drawn (and put them back on the stack)
		if(cardStack.Count>0){
			Debug.Log("Card stack:");
			Stack<Card> tempcards = new Stack<Card>();
			while(cardStack.Count>0) {
				Debug.Log(cardStack.Peek().name);
				tempcards.Push(cardStack.Pop());
			}
			while(tempcards.Count>0) {
				cardStack.Push(tempcards.Pop());
			}
		} else {
			Debug.LogWarning("Card stack is empty and cannot be printed.");
		}
	}
}
