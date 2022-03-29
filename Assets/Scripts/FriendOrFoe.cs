using UnityEngine;

namespace Assets.Scripts
{
    public class FriendOrFoe : MonoBehaviour
    {
        public enum Relation
        {
            friend, 
            foe
        }

        public int type;
        public Relation relation { get; private set; }

        void Update()
        {
            type = Mathf.Clamp(type, 0, 1);
            relation = type == 0 ? Relation.friend : Relation.foe;
        }
    }
}
