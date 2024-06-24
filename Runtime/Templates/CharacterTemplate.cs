using UnityEngine;

namespace TransparentGames.Entities
{
    [CreateAssetMenu(fileName = "New Character Template", menuName = "Transparent Games/Characters/Character Template", order = 0)]
    public class CharacterTemplate : EntityTemplate
    {
        public string DisplayName => name; // put later on id via translation
        public Sprite fullArt;
        public Sprite bust;
    }
}