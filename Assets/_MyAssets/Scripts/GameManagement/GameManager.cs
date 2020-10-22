using PlayerMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Player m_player = default;

        [Header("START")]
        [SerializeField] private float m_waitTimeToStart = 3f;

        [Header("GOAL")]
        [SerializeField] private Transform m_goalPlanet = default;
        [SerializeField] private float m_goalThreshold = 5f;

        [Header("Timer")]
        [SerializeField] private TimeController m_playTime = default;

        private enum State
        {
            None = -1,
            Start,
            Play,
            End,
        }

        private State m_state;
        private State m_nextState = State.Start;
        private float m_timer = 0f;

        private void Update()
        {
            // タイマー処理.
            m_timer += Time.deltaTime;

            // 状態遷移のチェック.
            if(m_nextState != State.None)
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
                    m_playTime.enabled = false;
                    m_playTime.InitializeTime();

                    // プレイヤーは制御不能に.
                    m_player.m_canMove = false;
                    break;
                case State.Play:
                    // タイム測定スタート.
                    m_playTime.enabled = true;
                    m_playTime.InitializeTime();

                    // プレイヤーは制御可能に.
                    m_player.m_canMove = true;
                    break;
                case State.End:
                    // タイム計測ストップ.
                    m_playTime.enabled = false;

                    // プレイヤーは制御不能に.
                    m_player.m_canMove = false;
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
                    break;
            }
        }

        private bool IsGoal() => Vector3.Distance(m_player.transform.position, m_goalPlanet.position) < m_goalThreshold;

        /// <summary>プレイ開始前のカウントダウン用残り時間</summary>
        /// <remarks>カウントダウンをしていない場合は-1を返す</remarks>
        public float StartRestTime
        {
            get
            {
                if(m_state == State.Start)
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
                if(m_state == State.Start)
                {
                    return null;
                }
                else
                {
                    return m_playTime.GetTimeText();
                }
            }
        }
    }
}