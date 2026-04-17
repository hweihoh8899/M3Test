using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<eStateGame> StateChangedAction = delegate { };
    public static readonly int TWEEN_GROUP_GAMEPLAY = "Gameplay".GetHashCode();
    public enum eLevelMode
    {
        TIMER,
        MOVES
    }

    public enum eStateGame
    {
        SETUP,
        MAIN_MENU,
        GAME_STARTED,
        PAUSE,
        GAME_OVER,
    }

    private eStateGame m_state;
    public eStateGame State
    {
        get { return m_state; }
        private set
        {
            m_state = value;

            StateChangedAction(m_state);
        }
    }


    private GameSettings m_gameSettings;


    private BoardController m_boardController;

    private UIMainManager m_uiMenu;

    private LevelCondition m_levelCondition;

    private void Awake()
    {
        State = eStateGame.SETUP;

        m_gameSettings = Resources.Load<GameSettings>(Constants.GAME_SETTINGS_PATH);

        m_uiMenu = FindObjectOfType<UIMainManager>();
        m_uiMenu.Setup(this);
    }

    void Start()
    {
        State = eStateGame.MAIN_MENU;
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (m_boardController != null) m_boardController.Update();
    // }


    internal void SetState(eStateGame state)
    {
        State = state;

        if (State == eStateGame.PAUSE)
        {
            DOTween.Pause(TWEEN_GROUP_GAMEPLAY);
        }
        else
        {
            DOTween.Play(TWEEN_GROUP_GAMEPLAY);
        }
    }

    public void LoadLevel(eLevelMode mode)
    {
        currentMode = mode;
        
        m_boardController = new GameObject("BoardController").AddComponent<BoardController>();
        m_boardController.StartGame(this, m_gameSettings);

        if (mode == eLevelMode.MOVES)
        {
            m_levelCondition = this.gameObject.AddComponent<LevelMoves>();
            m_levelCondition.Setup(m_gameSettings.LevelMoves, m_uiMenu.GetLevelConditionView(), m_boardController);
        }
        else if (mode == eLevelMode.TIMER)
        {
            m_levelCondition = this.gameObject.AddComponent<LevelTime>();
            m_levelCondition.Setup(m_gameSettings.LevelMoves, m_uiMenu.GetLevelConditionView(), this);
        }

        m_levelCondition.ConditionCompleteEvent += GameOver;

        State = eStateGame.GAME_STARTED;
    }

    public void GameOver()
    {
        StartCoroutine(WaitBoardController());
    }

    internal void ClearLevel()
    {
        if (m_boardController)
        {
            m_boardController.Clear();
            Destroy(m_boardController.gameObject);
            m_boardController = null;
        }
    }

    private IEnumerator WaitBoardController()
    {
        while (m_boardController.IsBusy)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1f);

        State = eStateGame.GAME_OVER;

        if (m_levelCondition != null)
        {
            m_levelCondition.ConditionCompleteEvent -= GameOver;

            Destroy(m_levelCondition);
            m_levelCondition = null;
        }
    }
    private eLevelMode currentMode;
    public void ResetGame()
    {
        if (m_boardController == null) return;

        m_boardController.RevertBoard();
        
        if (m_levelCondition != null)
        {
            m_levelCondition.ConditionCompleteEvent -= GameOver;
            Destroy(m_levelCondition);
            m_levelCondition = null;
            
            if (currentMode == eLevelMode.MOVES)
            {
                m_levelCondition = gameObject.AddComponent<LevelMoves>();
                m_levelCondition.Setup(m_gameSettings.LevelMoves,
                    m_uiMenu.GetLevelConditionView(), m_boardController);
            }
            else
            {
                m_levelCondition = gameObject.AddComponent<LevelTime>();
                m_levelCondition.Setup(m_gameSettings.LevelTime,
                    m_uiMenu.GetLevelConditionView(), this);
            }

            m_levelCondition.ConditionCompleteEvent += GameOver;

            State = eStateGame.GAME_STARTED;
        }
    }
}
