using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystem
{
    public enum Character
    {
        Scope
    }

    [CreateAssetMenu(fileName = "Character Prefab Registry", menuName = "Player/Character Prefab Registry")]
    public class CharacterPrefabRegistry : ScriptableObject
    {
        [System.Serializable]
        public struct CharacterPrefabEntry
        {
            public Character Character;
            public GameObject Prefab;
        }

        [SerializeField] private CharacterPrefabEntry[] characterPrefabs;

        private static Dictionary<Character, GameObject> prefabDictionary;

        public static IReadOnlyDictionary<Character, GameObject> Prefabs
        {
            get
            {
                if (prefabDictionary == null)
                {
                    Initialize();
                }
                return prefabDictionary;
            }
        }

        private static void Initialize()
        {
            prefabDictionary = new Dictionary<Character, GameObject>();
            var registry = Resources.Load<CharacterPrefabRegistry>("Character Prefab Registry");

            if (registry == null)
            {
                Debug.LogError("CharacterPrefabRegistry asset not found in a Resources folder!");
                return;
            }

            foreach (var entry in registry.characterPrefabs)
            {
                if (entry.Prefab != null && !prefabDictionary.ContainsKey(entry.Character))
                {
                    prefabDictionary.Add(entry.Character, entry.Prefab);
                }
            }
        }
    }
}