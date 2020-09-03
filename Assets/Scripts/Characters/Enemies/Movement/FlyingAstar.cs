﻿using UnityEngine;
using Pathfinding;
namespace Unexpected.Enemy.Movement
{
    public class FlyingAstar : MonoBehaviour, IMovement
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [Header("Movement Variables")]
        [SerializeField] private float _speed = 200f;
        [SerializeField] private Rigidbody2D _rigidbody2d;
        [SerializeField] private Transform _sprite;
        [Header("A* Pathfinding")]
        [SerializeField] private Seeker _seeker;
        [SerializeField] private float _nextWaypointDistance = 3;
#pragma warning restore CS0649
        #endregion

        private Path _path;
        private int _currentWaypoint = 0;
#pragma warning disable CS0414
        private bool _reachedEndOfPath = false;
#pragma warning restore CS0414
        private bool _started = false;
        private GameObject _player;
        private Transform _target;

        #region Monobehaviour
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _target = _player.transform;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject != _player || _started)
                return;
            InvokeRepeating("UpdatePath", 0f, 1f);
            _started = true;
        }
        #endregion

        public void Move()
        {
            if (_path == null)
                return;
            if (_currentWaypoint >= _path.vectorPath.Count)
            {
                _reachedEndOfPath = true;
                return;
            }
            else
            {
                _reachedEndOfPath = false;
            }

            // Set new destination to the next position along path
            Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint]
                - _rigidbody2d.position).normalized;
            Vector2 force = direction * _speed * Time.fixedDeltaTime;
            _rigidbody2d.AddForce(force);

            float distance = Vector2.Distance(_rigidbody2d.position,
                _path.vectorPath[_currentWaypoint]);
            if (distance < _nextWaypointDistance)
                _currentWaypoint++;

            _sprite.localScale = (_rigidbody2d.velocity.x <= 0f)
                ? new Vector3(-1f, 1f, 1f)
                : Vector3.one; 
        }

        public void Die() => CancelInvoke();

        private void UpdatePath()
        {
            if (_seeker.IsDone())
                _seeker.StartPath(
                    _rigidbody2d.position,
                    _target.position,
                    OnPathComplete);
        }

        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                _path = p;
                _currentWaypoint = 0;
            }
        }
    }
}
/* Implementation of the Unity version of the A* Pathfinding Project.
 * When the player enters the aggro range of the enemy, starts pathfinding
 * directly towards the player. */