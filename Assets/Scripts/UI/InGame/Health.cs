using UnityEngine;
using UnityEngine.UI;

namespace Unexpected.UI
{
    public class Health : MonoBehaviour
    {
        #region Serialized Fields
#pragma warning disable CS0649
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _empty;
#pragma warning restore CS0649
        #endregion

        public void SetHealth(int amount)
        {
            //Debug.Log($"setting health to {amount}/{transform.childCount}");
            if (amount > transform.childCount)
                return;

            for (int i = 0; i < transform.childCount; i++)
            {
                var heart = transform.GetChild(i)
                    .GetComponent<Image>();
                if (i < amount)
                {
                    heart.sprite = _full;
                    //Debug.Log("FULL");
                }
                else
                {
                    heart.sprite = _empty;
                    //Debug.Log("EMPTY");
                }
            }
        }
    }
}