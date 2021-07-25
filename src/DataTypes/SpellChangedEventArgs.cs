using LoLTimers.DataTypes;

namespace LoLTimers
{
    public record SpellChangedEventArgs
    {
        public object Sender;
        public Spell Spell;
        
        public SpellChangedEventArgs(object sender, Spell spell)
        {
            Sender = sender;
            Spell = spell;
        }
    }
}