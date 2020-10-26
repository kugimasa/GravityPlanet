using System;
using System.Collections;
using UnityEngine;
using Views;
using PlayerMovement;
using StageGeneration;
using GravitySystem;
using StateMachine;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        [Header("Start")]
        [SerializeField] private float m_startWaitTime = 3f;

        [Header("Play")]
        [SerializeField] private Transform m_player = default;

        [Header("Stage")]
        [SerializeField] private Transform m_start = default;
        [SerializeField] private Transform m_goal = default;
        [SerializeField] private float m_goalThreshold = 1f;
        [SerializeField] private GameObject m_stageGenerator = default;

        [Header("Views")]
        [SerializeField] private GameObject m_viewManager = default;

        private IEnumerator Start()
        {
            // コンポーネントの取得と設定.
            var views = m_viewManager.GetComponent<IViewController>();
            var inputReciever = m_player.GetComponent<IInputReciever>();
            var stageGenerator = m_stageGenerator.GetComponent<RandomStageGenerator>();
            stageGenerator.OnRailMove = m_player.GetComponent<IPlanetSetter>();

            // ステージ作成.
            stageGenerator.Create(m_start, m_goal);

            // カウントダウン.
            inputReciever.PlayerInput = new PlayerInputNull(); // 操作不能に.
            var startState = new WaitTimeState(m_startWaitTime);
            yield return StateCoroutine(startState, () => views.SetRestTime(startState.RestTime));

            // プレイ.
            inputReciever.PlayerInput = new PlayerInputAxis(); // 操作可能に.
            var playState = new PlayState(m_player, m_goal, m_goalThreshold);
            yield return StateCoroutine(playState, () => views.SetPlayTime(playState.PlayTime));

            // ゲーム終了.
            inputReciever.PlayerInput = new PlayerInputNull(); // 操作不能に.
            views.OnGoal();
        }

        private IEnumerator StateCoroutine(IState state, Action update = null)
        {
            state.OnStart();
            while (state.OnNext())
            {
                update?.Invoke();
                yield return null;
            }
        }
    }
}