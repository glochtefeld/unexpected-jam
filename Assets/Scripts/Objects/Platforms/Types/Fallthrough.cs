using System.Collections;
using TMPro;
using Unexpected.Player;
using UnityEngine;

namespace Unexpected.Objects.Platforms.Types
{
    public class Fallthrough : MonoBehaviour, IPlatform
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private Collider2D _collider2D;
#pragma warning restore CS0649
        #endregion

        public void Activate() { }

        public IEnumerator DisableCollider()
        {
            _collider2D.enabled = false;
            yield return new WaitForSeconds(0.5f);
            _collider2D.enabled = true;
            yield return null;
        }
    }
}