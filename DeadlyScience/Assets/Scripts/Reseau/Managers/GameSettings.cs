using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ds
{
    [CreateAssetMenu(menuName = "Manager/GameSettings")]

    public class GameSettings : ScriptableObject
    {
        [SerializeField]
        private string _gameVersion = "0.0.0";

        public string GameVersion => _gameVersion;
        [SerializeField]
        private string _nickname = "Joueur";

        public string Nickname
        {
            get
            {
                int value = Random.Range(0, 9999);
                return _nickname + value.ToString();
            }
        }

    }
}
