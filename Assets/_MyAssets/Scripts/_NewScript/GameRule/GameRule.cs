using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameRules
{
    public interface ITimeController
    {
        void InitializeTime();
        float CurrentTime { get; }

        void UpdateTime();
    }

    public interface IPlayer
    {
        bool CanMove { get; set; }
        Transform transform { get; }
    }
    public class GameRule : MonoBehaviour
    {

        [Header("Player")]
        [Inject] private IPlayer m_player = default;
        [Header("START")]
        [SerializeField] private float m_waitTimeToStart = 3f;
        [Header("GOAL")]
        [SerializeField] private Transform m_goalPlanet = default;
        [SerializeField] private float m_goalThreshold = 5f;//ここで初期化あんましたくない
        [Header("Timer")]
        [Inject] private ITimeController m_playTime = default;//シリアライズ不可能なのでzenject?
        public enum State
        {
            None = -1,
            Start,
            Play,
            End,
        }
        private float m_timer = 0f;
        [SerializeField]private State m_state;
        private State m_nextState = State.Start;

        public State NowState { get { return m_state; } }
        private void Update()
        {
            // タイマー処理.
            m_timer += Time.deltaTime;

            // 状態遷移のチェック.
            if (m_nextState != State.None)
            {
                // 状態を変更.
                m_state = m_nextState;
                m_nextState = State.None;

                // 初期化.
                m_timer = 0f;
                InitializeState(m_state);

                // 遷移処理と更新処理は同時に行わないことにする.
                return;
            }

            // 状態ごとの更新処理.
            UpdateState(m_state);
        }

        private void InitializeState(State state)
        {
            Debug.Log("Initialize: " + state);

            switch (state)
            {
                case State.Start:
                    // タイム測定はしない.
                    //m_playTime.enabled = false;
                    m_playTime.InitializeTime();

                    // プレイヤーは制御不能に.
                    m_player.CanMove = false;
                    break;
                case State.Play:
                    // タイム測定スタート.
                    //m_playTime.enabled = true;
                    m_playTime.InitializeTime();

                    // プレイヤーは制御可能に.
                    m_player.CanMove = true;
                    break;
                case State.End:
                    // タイム計測ストップ.
                    //m_playTime.enabled = false;

                    // プレイヤーは制御不能に.
                    m_player.CanMove = false;

                    // ゴール演出を表示.
                    //m_goalView.GoalViewAction();//ゴール側から参照する
                    break;
            }
        }

        private void UpdateState(State m_state)
        {
            switch (m_state)
            {
                case State.Start:
                    // 時間経過でプレイスタート.
                    if (m_timer > m_waitTimeToStart) m_nextState = State.Play;
                    break;
                case State.Play:

                    // ゴールしたら終了.
                    if (IsGoal()) m_nextState = State.End;
                    else m_playTime.UpdateTime();
                    break;
            }
        }
        private bool IsGoal() => Vector3.Distance(m_player.transform.position, m_goalPlanet.position) < m_goalThreshold;
        public float StartRestTime
        {
            get
            {
                if (m_state == State.Start)
                {
                    return m_waitTimeToStart - m_timer;
                }
                else
                {
                    return -1f;
                }
            }
        }
        /// <summary>プレイ時間</summary>
         /// <remarks>プレイが始まっていない場合はnullを返す</remarks>
        public string PlayTime
        {
            get
            {
                if (m_state == State.Start)
                {
                    return null;
                }
                else
                {
                    return m_playTime.CurrentTime.ToString();
                }
            }
        }
    }

}