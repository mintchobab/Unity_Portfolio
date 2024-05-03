using UnityEngine;

namespace lsy
{
    public class BossWolfController : MonoBehaviour
    {
        private Animator anim;

        private int hashIsBarking = Animator.StringToHash("isBarking");


        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            anim.SetBool(hashIsBarking, true);
        }


        public void StartBT()
        {
            anim.SetBool(hashIsBarking, false);
            GetComponent<BossWolfBT>().enabled = true;

            this.enabled = false;
        }
    }
}
