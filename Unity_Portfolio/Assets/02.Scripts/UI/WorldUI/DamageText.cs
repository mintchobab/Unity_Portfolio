using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

namespace lsy
{
    public class DamageText : MonoBehaviour
    {
        private TextMeshPro textMesh;
        private Vector3 originScale;

        private readonly float floatingTime = 0.8f;


        private void Awake()
        {
            textMesh = GetComponent<TextMeshPro>();

            textMesh.GetComponent<MeshRenderer>().sortingOrder = 2;
            originScale = transform.localScale;
        }


        public void ShowDamage(Vector3 position, int damage)
        {
            textMesh.text = damage.ToString();
            StartCoroutine(Floating(position));
        }


        private IEnumerator Floating(Vector3 position)
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(originScale, 0.4f).SetEase(Ease.OutElastic);

            Vector3 randomPos = Random.insideUnitSphere * 0.2f;
            transform.position = position + randomPos;

            yield return new WaitForSeconds(floatingTime);

            Managers.Instance.PoolManager.Push(gameObject);
        }
    }
}
