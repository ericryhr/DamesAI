using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    //Similar als escacs pero amb dames
    [HideInInspector]
    public GameState gameState;
    int colorToMove;
    [HideInInspector] public int whitePieces = 0;
    [HideInInspector] public int blackPieces = 0;

    List<Move> currentMoves;
    Stack<Move> moveHistory;
    AI AInegre;
    public int AIDepth = 1;

    public string startingFEN = "1p1p1p1p/p1p1p1p1/1p1p1p1p/8/8/P1P1P1P1/1P1P1P1P/P1P1P1P1 w - - 0 1";
    public GameObject boardIndexs;

    // Start is called before the first frame update
    void Start()
    {
        moveHistory = new Stack<Move>();
        AInegre = new AI(Piece.Negre, AIDepth);

        LoadPositionFromFen(startingFEN);
        Torn(Piece.Blanc, -1);
    }

    void Update()
	{
        if (Input.GetKeyDown(KeyCode.H)) boardIndexs.SetActive(!boardIndexs.activeSelf);
        if (Input.GetKeyDown(KeyCode.P)) Board.PrintBoard();
        if (Input.GetKeyDown(KeyCode.R)) UnMakeNMoves(2);
        //if (Input.GetKeyDown(KeyCode.R)) UnMakeMove();
    }

    void LoadPositionFromFen(string fen)
	{
        Dictionary<char, int> pieceTypeFromSymbol = new Dictionary<char, int>()
        {
            ['p'] = Piece.Peo,
            ['d'] = Piece.Dama
        };

        string fenBoard = fen.Split(' ')[0];
        int columna = 0, fila = 7;

		foreach (char symbol in fenBoard)
		{
            if (symbol == '/')
            {
                columna = 0;
                fila--;
            }
            else
            {
                if (char.IsDigit(symbol)) columna += (int)char.GetNumericValue(symbol);
                else
                {
                    int pieceColor = (char.IsUpper(symbol)) ? Piece.Blanc : Piece.Negre;
                    int pieceType = pieceTypeFromSymbol[char.ToLower(symbol)];
                    Board.PlaceBoardPiece(pieceType | pieceColor, fila * 8 + columna);
                    if (Piece.EsColor(pieceColor, Piece.Blanc)) whitePieces++;
                    else blackPieces++;
                    columna++;
                }
            }
		}

        PieceUIManager.instance.RegenerateUIBoard();
	}

    //Captura es la posicio nova de la peça que ha matat al torn anterior, -1 si no s'ha matat cap peça
    void Torn(int color, int captura)
	{
        if (gameState == GameState.GameOver) return;

        colorToMove = color;

        Debug.Log(((color == Piece.Blanc) ? "Blanc" : "Negre") + " mou");
        gameState = GameState.Calculant;
        currentMoves = MoveGenerator.GenerateMoves(color, captura);
        Debug.Log("Moviments disponibles: " + currentMoves.Count);
        gameState = GameState.Torn;

        //Si no hi ha cap moviment disponible
        if (currentMoves.Count == 0)
		{
            //Si es un torn normal el jugador ha de passar i per tant perd
            if (captura == -1) GameOver(Piece.AltreColor(color));
            //Si ja s'havia matat una unitat i no pot moure mes simplement canvia el torn
            else Torn(Piece.AltreColor(color), -1);
            return;
        }

        //Si es el torn de la IA
        if(color == Piece.Negre)
		{
            MovePiece(color, AInegre.ComputeAIMove(captura));
		}
    }

    public bool ValidStateToMove(int color)
	{
        if (gameState == GameState.Calculant || gameState == GameState.GameOver) return false;
        else
		{
            if (color == colorToMove && gameState == GameState.Torn) return true;
            else Debug.Log("No es el teu torn!");
		}
        return false;
	}

    public int ValidMove(Move move)
	{
        int index = currentMoves.IndexOf(move);
        if(index == -1)
		{
            Debug.Log("Moviment invalid. Els moviments disponibles son:");
			foreach (Move m in currentMoves)
			{
                m.printMove();
			}
		}
        return index;
	}

    //Passem el moveIndex i no el move pk sino es perd si el moviment ha matat alguna peça
    public void MovePiece(int color, int moveIndex)
	{
        Move move = currentMoves[moveIndex];
        moveHistory.Push(move);

        //Comprovem si han matat una peça
        int captura = -1;
        if (move.haCapturat) { SubtractPiece(Piece.AltreColor(color)); captura = move.newPos; }
        int nextColor = (captura != -1) ? color : Piece.AltreColor(color);

        //Comprovacions si la partida ha acabat
        if (whitePieces == 0 || blackPieces == 0) GameOver(color);
        Board.MakeBoardMove(move);
        PieceUIManager.instance.RegenerateUIBoard();
        Torn(nextColor, captura);
    }

    //Aquesta funcio nomes la utilitzara la IA
    void MovePiece(int color, Move move)
	{
        moveHistory.Push(move);

        //Comprovem si han matat una peça
        int captura = -1;
        if (move.haCapturat) { SubtractPiece(Piece.AltreColor(color)); captura = move.newPos; }
        int nextColor = (captura != -1) ? color : Piece.AltreColor(color);

        //Comprovacions si la partida ha acabat
        if (whitePieces == 0 || blackPieces == 0) GameOver(color);
        Board.MakeBoardMove(move);
        PieceUIManager.instance.RegenerateUIBoard();
        Torn(nextColor, captura);
    }

    void UnMakeMove()
	{
        if (moveHistory.Count == 0) return;
        Move move = moveHistory.Pop();
        Board.UnmakeBoardMove(move);

        if (move.haCapturat) AddPiece(colorToMove);
        int nextColor = move.color;

        PieceUIManager.instance.RegenerateUIBoard();
        //Comprovem el top de la pila per saber si s'havia matat una peça
        int captura = -1;
        if (moveHistory.Count != 0 && moveHistory.Peek().haCapturat) captura = moveHistory.Peek().newPos;
        Torn(nextColor, captura);
	}

    void UnMakeNMoves(int n)
	{
        if (moveHistory.Count < n) return;
        
		for (int i = 0; i < n; i++)
		{
            Move move = moveHistory.Pop();
            Board.UnmakeBoardMove(move);

            if (move.haCapturat) AddPiece(colorToMove);
            colorToMove = move.color;
        }

        PieceUIManager.instance.RegenerateUIBoard();
        //Comprovem el top de la pila per saber si s'havia matat una peça
        int captura = -1;
        if (moveHistory.Count != 0 && moveHistory.Peek().haCapturat) captura = moveHistory.Peek().newPos;
        Torn(colorToMove, captura);
    }

    void AddPiece(int color)
	{
        if (Piece.EsColor(color, Piece.Blanc)) whitePieces++;
        else blackPieces++;
	}

    void SubtractPiece(int color)
	{
        if (Piece.EsColor(color, Piece.Blanc)) whitePieces--;
        else blackPieces--;
	}

    void GameOver(int winner)
	{
        gameState = GameState.GameOver;
        if (Piece.EsColor(winner, Piece.Blanc)) Debug.Log("Ha guanyat el blanc!");
        else Debug.Log("Ha guanyat el negre!");
	}
}

public enum GameState { Calculant, Torn, GameOver };

/*
 * TODO:
 * - Acabar el undo moves
 * - Print Board
 * - Search
 * - Peces maques
 * 
 */